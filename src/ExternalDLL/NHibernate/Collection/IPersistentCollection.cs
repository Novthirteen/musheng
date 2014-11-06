using System;
using System.Collections;
using System.Data;
using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;

namespace NHibernate.Collection
{
	/// <summary>
	/// <para>
	/// Persistent collections are treated as value objects by NHibernate.
	/// ie. they have no independent existence beyond the object holding
	/// a reference to them. Unlike instances of entity classes, they are
	/// automatically deleted when unreferenced and automatically become
	/// persistent when held by a persistent object. Collections can be
	/// passed between different objects (change "roles") and this might
	/// cause their elements to move from one database table to another.
	/// </para>
	/// <para>
	/// NHibernate "wraps" a collection in an instance of
	/// <see cref="IPersistentCollection" />. This mechanism is designed
	/// to support tracking of changes to the collection's persistent
	/// state and lazy instantiation of collection elements. The downside
	/// is that only certain abstract collection types are supported and
	/// any extra semantics are lost.
	/// </para>
	/// <para>
	/// Applications should <b>never</b> use classes in this namespace
	/// directly, unless extending the "framework" here.
	/// </para>
	/// <para>
	/// Changes to <b>structure</b> of the collection are recorded by the
	/// collection calling back to the session. Changes to mutable
	/// elements (ie. composite elements) are discovered by cloning their
	/// state when the collection is initialized and comparing at flush
	/// time.
	/// </para>
	/// </summary>
	public interface IPersistentCollection
	{
		/// <summary>
		/// The owning entity.
		/// </summary>
		/// <remarks>
		/// Note that the owner is only set during the flush
		/// cycle, and when a new collection wrapper is created
		/// while loading an entity.
		/// </remarks>
		object Owner { get; set; }

		/// <summary>
		/// Is the initialized collection empty?
		/// </summary>
		bool Empty { get; }

		/// <summary>
		/// Clears out any Queued Additions.
		/// </summary>
		/// <remarks>
		/// After a Flush() the database is in synch with the in-memory
		/// contents of the Collection.  Since everything is in synch remove
		/// any Queued Additions.
		/// </remarks>
		void PostAction();

		/// <summary>
		/// Return the user-visible collection (or array) instance
		/// </summary>
		/// <returns>
		/// By default, the NHibernate wrapper is an acceptable collection for
		/// the end user code to work with because it is interface compatible.
		/// An NHibernate PersistentList is an IList, an NHibernate PersistentMap is an IDictionary
		/// and those are the types user code is expecting.
		/// </returns>
		object GetValue();

		/// <summary>
		/// Called just before reading any rows from the <see cref="IDataReader" />
		/// </summary>
		void BeginRead();

		/// <summary>
		/// Called after reading all rows from the <see cref="IDataReader" />
		/// </summary>
		/// <remarks>
		/// This should be overridden by sub collections that use temporary collections
		/// to store values read from the db.
		/// </remarks>
		bool EndRead(ICollectionPersister persister);

		/// <summary>
		/// Gets a <see cref="Boolean"/> indicating if the underlying collection is directly
		/// accessable through code.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if we are not guaranteed that the NHibernate collection wrapper
		/// is being used.
		/// </value>
		/// <remarks>
		/// This is typically <see langword="false" /> whenever a transient object that contains a collection is being
		/// associated with an <see cref="ISession" /> through <see cref="ISession.Save" /> or <see cref="ISession.SaveOrUpdate" />.
		/// NHibernate can't guarantee that it will know about all operations that would cause NHibernate's collections
		/// to call <see cref="AbstractPersistentCollection.Read" /> or <see cref="AbstractPersistentCollection.Write" />.
		/// </remarks>
		bool IsDirectlyAccessible { get; set; }

		/// <summary>
		/// Disassociate this collection from the given session.
		/// </summary>
		/// <param name="session"></param>
		/// <returns>true if this was currently associated with the given session</returns>
		bool UnsetSession(ISessionImplementor session);

		/// <summary>
		/// Associate the collection with the given session.
		/// </summary>
		/// <param name="session"></param>
		/// <returns>false if the collection was already associated with the session</returns>
		bool SetCurrentSession(ISessionImplementor session);

		/// <summary>
		/// Read the state of the collection from a disassembled cached value.
		/// </summary>
		/// <param name="persister"></param>
		/// <param name="disassembled"></param>
		/// <param name="owner"></param>
		void InitializeFromCache(ICollectionPersister persister, object disassembled, object owner);

		/// <summary>
		/// Iterate all collection entries, during update of the database
		/// </summary>
		/// <returns>
		/// An <see cref="IEnumerable"/> that gives access to all entries
		/// in the collection.
		/// </returns>
		IEnumerable Entries();

		/// <summary>
		/// Reads the row from the <see cref="IDataReader"/>.
		/// </summary>
		/// <remarks>
		/// This method should be prepared to handle duplicate elements caused by fetching multiple collections,
		/// or <see cref="NHibernate.Hql.Classic.QueryTranslator.FetchedCollections.IsUnsafe" /> should be updated
		/// to return <see langword="true" /> for the collection type.
		/// </remarks>
		/// <param name="reader">The IDataReader that contains the value of the Identifier</param>
		/// <param name="role">The persister for this Collection.</param>
		/// <param name="descriptor">The descriptor providing result set column names</param>
		/// <param name="owner">The owner of this Collection.</param>
		/// <returns>The object that was contained in the row.</returns>
		object ReadFrom(IDataReader reader, ICollectionPersister role, ICollectionAliases descriptor, object owner);

		/// <summary>
		/// Get the identifier of the given collection entry
		/// </summary>
		object GetIdentifier(object entry, int i);

		/// <summary>
		/// Get the index of the given collection entry
		/// </summary>
		object GetIndex(object entry, int i);

		/// <summary>
		/// Get the value of the given collection entry
		/// </summary>
		object GetElement(object entry);

		/// <summary>
		/// Called before any elements are read into the collection,
		/// allowing appropriate initializations to occur.
		/// </summary>
		/// <param name="persister">The <see cref="ICollectionPersister"/> for this persistent collection.</param>
		void BeforeInitialize(ICollectionPersister persister);

		/// <summary>
		/// Does the current state exactly match the snapshot?
		/// </summary>
		/// <param name="elementType">The <see cref="IType"/> to compare the elements of the Collection.</param>
		/// <returns>
		/// <see langword="true" /> if the wrapped collection is different than the snapshot
		/// of the collection or if one of the elements in the collection is
		/// dirty.
		/// </returns>
		bool EqualsSnapshot(IType elementType);

		/// <summary>
		/// Disassemble the collection, ready for the cache
		/// </summary>
		/// <param name="persister">The <see cref="ICollectionPersister"/> for this Collection.</param>
		/// <returns>The contents of the persistent collection in a cacheable form.</returns>
		object Disassemble(ICollectionPersister persister);

		/// <summary>
		/// Gets a <see cref="Boolean"/> indicating if the rows for this collection
		/// need to be recreated in the table.
		/// </summary>
		/// <param name="persister">The <see cref="ICollectionPersister"/> for this Collection.</param>
		/// <returns>
		/// <see langword="false" /> by default since most collections can determine which rows need to be
		/// individually updated/inserted/deleted.  Currently only <see cref="PersistentBag"/>'s for <c>many-to-many</c>
		/// need to be recreated.
		/// </returns>
		bool NeedsRecreate(ICollectionPersister persister);

		/// <summary>
		/// Return a new snapshot of the current state of the collection
		/// </summary>
		ICollection GetSnapshot(ICollectionPersister persister);

		/// <summary>
		/// To be called internally by the session, forcing
		/// immediate initalization.
		/// </summary>
		/// <remarks>
		/// This method is similar to <see cref="AbstractPersistentCollection.Initialize" />, except that different exceptions are thrown.
		/// </remarks>
		void ForceInitialization();

		/// <summary>
		/// Does an element exist at this entry in the collection?
		/// </summary>
		bool EntryExists(object entry, int i);

		/// <summary>
		/// Do we need to insert this element?
		/// </summary>
		bool NeedsInserting(object entry, int i, IType elemType);

		/// <summary>
		/// Do we need to update this element?
		/// </summary>
		bool NeedsUpdating(object entry, int i, IType elemType);

		/// <summary>
		/// Get all the elements that need deleting
		/// </summary>
		ICollection GetDeletes(IType elemType, bool indexIsFormula);

		/// <summary>
		/// Is this the wrapper for the given underlying collection instance?
		/// </summary>
		/// <param name="collection">The collection to see if this IPersistentCollection is wrapping.</param>
		/// <returns>
		/// <see langword="true" /> if the IPersistentCollection is wrappping the collection instance,
		/// <see langword="false" /> otherwise.
		/// </returns>
		bool IsWrapper(object collection);

		/// <summary></summary>
		bool WasInitialized { get; }

		/// <summary></summary>
		bool HasQueuedAdds { get; }

		/// <summary></summary>
		ICollection QueuedAddsCollection { get; }

		/// <summary></summary>
		ICollectionSnapshot CollectionSnapshot { get; set; }

		/// <summary>
		/// Called before inserting rows, to ensure that any surrogate keys are fully generated
		/// </summary>
		/// <param name="persister"></param>
		void PreInsert(ICollectionPersister persister);

		/// <summary>
		/// Called after inserting a row, to fetch the natively generated id
		/// </summary>
		/// <param name="persister"></param>
		/// <param name="entry"></param>
		/// <param name="i"></param>
		void AfterRowInsert(ICollectionPersister persister, object entry, int i);

		/// <summary>
		/// Get all "orphaned" elements
		/// </summary>
		/// <param name="snapshot">The snapshot of the collection.</param>
		/// <param name="entityName">The persistent class whose objects
		/// the collection is expected to contain.</param>
		/// <returns>
		/// An <see cref="ICollection"/> that contains all of the elements
		/// that have been orphaned.
		/// </returns>
		ICollection GetOrphans(object snapshot, System.Type entityName);

		/// <summary>
		/// Get the snapshot value of the given collection entry
		/// </summary>
		object GetSnapshotElement(object entry, int i);

		bool IsSnapshotEmpty(ICollection snapshot);

		/// <summary>
		/// Called after initializing from cache
		/// </summary>
		// NH: added persister parameter to fix NH-739
		bool AfterInitialize(ICollectionPersister persister);

		/// <summary>
		/// Is the collection dirty? Note that this is only
		/// reliable during the flush cycle, after the
		/// collection elements are dirty checked against
		/// the snapshot.
		/// </summary>
		bool IsDirty { get; }

		/// <summary>
		/// Clear the dirty flag, after flushing changes
		/// to the database.
		/// </summary>
		void ClearDirty();

		/// <summary>
		/// Mark the collection as dirty
		/// </summary>
		void Dirty();
	}
}