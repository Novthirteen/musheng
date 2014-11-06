using System;
using System.Data;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.SqlTypes;

namespace NHibernate.Type
{
	/// <summary>
	/// A many-to-one association to an entity
	/// </summary>
	[Serializable]
	public class ManyToOneType : EntityType, IAssociationType
	{
		private readonly bool _isIgnoreNotFound;

		private IType GetReferencedType(IMapping mapping)
		{
			if (uniqueKeyPropertyName == null)
			{
				return mapping.GetIdentifierType(AssociatedClass);
			}
			else
			{
				return mapping.GetPropertyType(AssociatedClass, uniqueKeyPropertyName);
			}
		}

		public override int GetColumnSpan(IMapping mapping)
		{
			return GetReferencedType(mapping).GetColumnSpan(mapping);
		}

		public override SqlType[] SqlTypes(IMapping mapping)
		{
			return GetReferencedType(mapping).SqlTypes(mapping);
		}

		public ManyToOneType(System.Type persistentClass)
			: this(persistentClass, false)
		{
		}

		public ManyToOneType(System.Type persistentClass, bool lazy)
			: this(persistentClass, null, lazy, false)
		{
		}

		public ManyToOneType(
			System.Type persistentClass,
			string uniqueKeyPropertyName,
			bool lazy,
			bool ignoreNotFound)
			: base(persistentClass, uniqueKeyPropertyName, !lazy)
		{
			_isIgnoreNotFound = ignoreNotFound;
		}


		public override void NullSafeSet(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			GetIdentifierOrUniqueKeyType(session.Factory)
				.NullSafeSet(st, GetIdentifier(value, session), index, settable, session);
		}

		public override void NullSafeSet(IDbCommand cmd, object value, int index, ISessionImplementor session)
		{
			GetIdentifierOrUniqueKeyType(session.Factory)
				.NullSafeSet(cmd, GetIdentifier(value, session), index, session);
		}

		public override bool IsOneToOne
		{
			get { return false; }
		}

		public override ForeignKeyDirection ForeignKeyDirection
		{
			get { return ForeignKeyDirection.ForeignKeyFromParent; }
		}

		/// <summary>
		/// Hydrates the Identifier from <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="rs">The <see cref="IDataReader"/> that contains the query results.</param>
		/// <param name="names">A string array of column names to read from.</param>
		/// <param name="session">The <see cref="ISessionImplementor"/> this is occuring in.</param>
		/// <param name="owner">The object that this Entity will be a part of.</param>
		/// <returns>
		/// An instantiated object that used as the identifier of the type.
		/// </returns>
		public override object Hydrate(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			object id = GetIdentifierOrUniqueKeyType(session.Factory)
				.NullSafeGet(rs, names, session, owner);
			ScheduleBatchLoadIfNeeded(id, session);
			return id;
		}

		private void ScheduleBatchLoadIfNeeded(object id, ISessionImplementor session)
		{
			//cannot batch fetch by unique key (property-ref associations)
			if (uniqueKeyPropertyName == null && id != null)
			{
				IEntityPersister persister = session.Factory.GetEntityPersister(AssociatedClass);
				EntityKey entityKey = new EntityKey(id, persister);
				if (!session.ContainsEntity(entityKey))
				{
					session.BatchFetchQueue.AddBatchLoadableEntityKey(entityKey);
				}
			}
		}

		public override bool UseLHSPrimaryKey
		{
			get { return false; }
		}

		public override bool IsModified(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			if (current == null)
			{
				return old != null;
			}
			if (old == null)
			{
				return current != null;
			}
			//the ids are fully resolved, so compare them with isDirty(), not isModified()
			return GetIdentifierOrUniqueKeyType(session.Factory).IsDirty(old, GetIdentifier(current, session), session);
		}

		public override object Disassemble(object value, ISessionImplementor session)
		{
			if (value == null)
			{
				return null;
			}
			else
			{
				// cache the actual id of the object, not the value of the
				// property-ref, which might not be initialized
				object id = session.GetEntityIdentifierIfNotUnsaved(value);
				if (id == null)
				{
					throw new AssertionFailure("cannot cache a reference to an object with a null id: " + AssociatedClass.Name);
				}
				return GetIdentifierType(session).Disassemble(id, session);
			}
		}

		public override object Assemble(object oid, ISessionImplementor session, object owner)
		{
			object id = GetIdentifierType(session).Assemble(oid, session, owner);
			if (id == null)
			{
				return null;
			}
			else
			{
				return ResolveIdentifier(id, session);
			}
		}

		public override bool IsAlwaysDirtyChecked
		{
			get { return _isIgnoreNotFound; }
		}

		public override bool IsDirty(object old, object current, ISessionImplementor session)
		{
			if (IsSame(old, current))
			{
				return false;
			}

			object oldid = GetIdentifier(old, session);
			object newid = GetIdentifier(current, session);
			return GetIdentifierType(session).IsDirty(oldid, newid, session);
		}

		public bool IsSame(object x, object y)
		{
			return x == y;
		}

		public override bool IsDirty(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			if (IsAlwaysDirtyChecked)
			{
				return IsDirty(old, current, session);
			}
			else
			{
				if (IsSame(old, current))
				{
					return false;
				}

				object oldid = GetIdentifier(old, session);
				object newid = GetIdentifier(current, session);
				return GetIdentifierType(session).IsDirty(oldid, newid, checkable, session);
			}
		}

		public override bool IsNullable
		{
			get { return _isIgnoreNotFound; }
		}
	}
}