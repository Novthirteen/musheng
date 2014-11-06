
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;

namespace NHibernate.Collection.Generic
{
	/// <summary>
	/// Implements "bag" semantics more efficiently than <see cref="PersistentBag" /> by adding
	/// a synthetic identifier column to the table.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The identifier is unique for all rows in the table, allowing very efficient
	/// updates and deletes.  The value of the identifier is never exposed to the 
	/// application. 
	/// </para>
	/// <para>
	/// Identifier bags may not be used for a many-to-one association.  Furthermore,
	/// there is no reason to use <c>inverse="true"</c>.
	/// </para>
	/// </remarks>
	[Serializable]
	[DebuggerTypeProxy(typeof(CollectionProxy<>))]
	public class PersistentIdentifierBag<T> : AbstractPersistentCollection, IList, IList<T>
	{
		private IList<T> values; //element
		private IDictionary identifiers; //index -> id 

		public PersistentIdentifierBag(ISessionImplementor session) : base(session)
		{
		}

		public PersistentIdentifierBag(ISessionImplementor session, ICollection coll) : base(session)
		{
			IList<T> list = coll as IList<T>;

			if (list != null)
			{
				values = list;
			}
			else
			{
				values = new List<T>();
				foreach (T obj in coll)
				{
					values.Add(obj);
				}
			}

			SetInitialized();
			IsDirectlyAccessible = true;
			identifiers = new Hashtable();
		}

		/// <summary>
		/// Initializes this Bag from the cached values.
		/// </summary>
		/// <param name="persister">The CollectionPersister to use to reassemble the PersistentIdentifierBag.</param>
		/// <param name="disassembled">The disassembled PersistentIdentifierBag.</param>
		/// <param name="owner">The owner object.</param>
		public override void InitializeFromCache(ICollectionPersister persister, object disassembled, object owner)
		{
			BeforeInitialize(persister);
			object[] array = (object[]) disassembled;

			for (int i = 0; i < array.Length; i += 2)
			{
				//object obj = persister.ElementType.Assemble( array[ i + 1 ], session, owner );
				//identifiers[ obj ] = persister.IdentifierType.Assemble( array[ i ], session, owner );
				identifiers[i / 2] = persister.IdentifierType.Assemble(array[i], Session, owner);
				values.Add((T) persister.ElementType.Assemble(array[i + 1], Session, owner));
			}

			SetInitialized();
		}

		public override bool IsWrapper(object collection)
		{
			return values == collection;
		}

		#region IList Members

		public int Add(object value)
		{
			Write();
			return ((IList) values).Add(value);
		}

		public void Clear()
		{
			Initialize(true);
			if (values.Count > 0 || identifiers.Count > 0)
			{
				values.Clear();
				identifiers.Clear();
				Dirty();
			}
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public object this[int index]
		{
			get
			{
				Read();
				return ((IList) values)[index];
			}
			set
			{
				Write();
				((IList) values)[index] = value;
			}
		}

		public void Insert(int index, object value)
		{
			Insert(index, (T) value);
		}

		public void RemoveAt(int index)
		{
			Initialize(true);
			BeforeRemove(index);
			((IList) values).RemoveAt(index);
			Dirty();
		}

		public void Remove(object value)
		{
			Remove((T) value);
		}

		public bool Contains(object value)
		{
			Read();
			return ((IList) values).Contains(value);
		}

		public int IndexOf(object value)
		{
			Read();
			return ((IList) values).IndexOf(value);
		}

		public bool IsFixedSize
		{
			get { return false; }
		}

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get { return false; }
		}

		public int Count
		{
			get
			{
				Read();
				return values.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			Read();
			((IList) values).CopyTo(array, index);
		}

		public object SyncRoot
		{
			get { return ((IList) values).SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			Read();
			return values.GetEnumerator();
		}

		#endregion

		#region IList<T> Members

		public int IndexOf(T item)
		{
			Read();
			return values.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			Initialize(true);
			BeforeAdd(index);
			values.Insert(index, item);
			Dirty();
		}

		T IList<T>.this[int index]
		{
			get
			{
				Read();
				return values[index];
			}
			set
			{
				Write();
				values[index] = value;
			}
		}

		public void Add(T item)
		{
			Initialize(true);
			values.Add(item);
			Dirty();
		}

		public bool Contains(T item)
		{
			Read();
			return values.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			Read();
			values.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			Initialize(true);
			int index = values.IndexOf(item);
			if (index >= 0)
			{
				RemoveAt(index);
				return true;
			}
			else
			{
				return false;
			}
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			Read();
			return values.GetEnumerator();
		}

		#endregion

		public override void BeforeInitialize(ICollectionPersister persister)
		{
			identifiers = new Hashtable();
			values = new List<T>();
		}

		public override object Disassemble(ICollectionPersister persister)
		{
			object[] result = new object[values.Count * 2];

			int i = 0;
			for (int j = 0; j < values.Count; j++)
			{
				object val = values[j];
				result[i++] = persister.IdentifierType.Disassemble(identifiers[j], Session);
				result[i++] = persister.ElementType.Disassemble(val, Session);
			}

			return result;
		}

		public override bool Empty
		{
			get { return (values.Count == 0); }
		}

		public override IEnumerable Entries()
		{
			return values;
		}

		public override bool EntryExists(object entry, int i)
		{
			return entry != null;
		}

		public override bool EqualsSnapshot(IType elementType)
		{
			IDictionary snap = (IDictionary) GetSnapshot();
			if (snap.Count != values.Count)
			{
				return false;
			}

			for (int i = 0; i < values.Count; i++)
			{
				object val = values[i];
				object id = identifiers[i];
				if (id == null)
				{
					return false;
				}

				object old = snap[id];
				if (elementType.IsDirty(old, val, Session))
				{
					return false;
				}
			}

			return true;
		}

		public override ICollection GetDeletes(IType elemType, bool indexIsFormula)
		{
			IDictionary snap = (IDictionary) GetSnapshot();
			IList deletes = new ArrayList(snap.Keys);

			for (int i = 0; i < values.Count; i++)
			{
				if (values[i] != null)
				{
					deletes.Remove(identifiers[i]);
				}
			}

			return deletes;
		}

		public override object GetIndex(object entry, int i)
		{
			return new NotImplementedException("Bags don't have indexes");
		}

		public override object GetElement(object entry)
		{
			return entry;
		}

		public override object GetIdentifier(object entry, int i)
		{
			return identifiers[i];
		}

		public override object GetSnapshotElement(object entry, int i)
		{
			IDictionary snap = (IDictionary) GetSnapshot();
			object id = identifiers[i];
			return snap[id];
		}

		public override bool NeedsInserting(object entry, int i, IType elemType)
		{
			IDictionary snap = (IDictionary) GetSnapshot();
			object id = identifiers[i];

			return entry != null && (id == null || snap[id] == null);
		}

		public override bool NeedsUpdating(object entry, int i, IType elemType)
		{
			if (entry == null)
			{
				return false;
			}
			IDictionary snap = (IDictionary) GetSnapshot();

			object id = identifiers[i];
			if (id == null)
			{
				return false;
			}

			object old = snap[id];
			return old != null && elemType.IsDirty(old, entry, Session);
		}

		public override object ReadFrom(IDataReader reader, ICollectionPersister role, ICollectionAliases descriptor,
		                                object owner)
		{
			object element = role.ReadElement(reader, owner, descriptor.SuffixedElementAliases, Session);
			values.Add((T) element);
			identifiers[values.Count - 1] = role.ReadIdentifier(reader, descriptor.SuffixedIdentifierAlias, Session);
			return element;
		}

		protected override ICollection Snapshot(ICollectionPersister persister)
		{
			IDictionary map = new Hashtable(values.Count);

			int i = 0;
			foreach (object obj in values)
			{
				object key = identifiers[i++];
				if (key != null)
				{
					map[key] = persister.ElementType.DeepCopy(obj);
				}
			}

			return map;
		}

		public override ICollection GetOrphans(object snapshot, System.Type entityName)
		{
			/*
			IDictionary sn = ( IDictionary ) GetSnapshot();
			ArrayList result = new ArrayList();
			result.AddRange( sn.Values );
			AbstractPersistentCollection.IdentityRemoveAll( result, values, session );
			return result;
			*/

			return GetOrphans(((IDictionary) snapshot).Values, (ICollection) values, Session);
		}

		public override void PreInsert(ICollectionPersister persister)
		{
			try
			{
				int i = 0;
				foreach (object entry in values)
				{
					int loc = i++;
					if (!identifiers.Contains(loc)) // TODO: native ids
					{
						object id = persister.IdentifierGenerator.Generate(Session, entry);
						identifiers[loc] = id;
					}
				}
			}
			catch (Exception sqle)
			{
				throw new ADOException("Could not generate idbag row id.", sqle);
			}
		}

		private void BeforeRemove(int index)
		{
			// Move the identifier being removed to the end of the list (i.e. it isn't actually removed).
			object removedId = identifiers[index];
			int last = values.Count - 1;
			for (int i = index; i < last; i++)
			{
				object id = identifiers[i + 1];
				if (id == null)
				{
					identifiers.Remove(i);
				}
				else
				{
					identifiers[i] = id;
				}
			}
			identifiers[last] = removedId;
		}

		private void BeforeAdd(int index)
		{
			for (int i = index; i < values.Count; i++)
			{
				identifiers[i + 1] = identifiers[i];
			}
			identifiers.Remove(index);
		}
	}
}
