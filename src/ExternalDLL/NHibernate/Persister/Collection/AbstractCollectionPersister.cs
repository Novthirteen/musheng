using System;
using System.Collections;
using System.Data;
using System.Text;
using Iesi.Collections;
using log4net;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Id;
using NHibernate.Impl;
using NHibernate.Loader.Collection;
using NHibernate.Mapping;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using Array=NHibernate.Mapping.Array;

namespace NHibernate.Persister.Collection
{
	/// <summary>
	/// Summary description for AbstractCollectionPersister.
	/// </summary>
	public abstract class AbstractCollectionPersister : ICollectionMetadata, ISqlLoadableCollection
	{
		private readonly string role;

		//SQL statements
		private readonly SqlCommandInfo sqlDeleteString;
		private readonly SqlCommandInfo sqlInsertRowString;
		private readonly SqlCommandInfo sqlUpdateRowString;
		private readonly SqlCommandInfo sqlDeleteRowString;
		private bool insertCallable;
		private bool updateCallable;
		private bool deleteCallable;
		private bool deleteAllCallable;
		private ExecuteUpdateResultCheckStyle insertCheckStyle;
		private ExecuteUpdateResultCheckStyle updateCheckStyle;
		private ExecuteUpdateResultCheckStyle deleteCheckStyle;
		private ExecuteUpdateResultCheckStyle deleteAllCheckStyle;

		// TODO H3:
		//		private readonly SqlString sqlSelectSizeString;
		//		private readonly SqlString sqlSelectRowByIndexString;
		//		private readonly SqlString sqlDetectRowByIndexString;
		//		private readonly SqlString sqlDetectRowByElementString;

		private readonly string sqlOrderByString;
		protected readonly string sqlWhereString;
		private readonly string sqlOrderByStringTemplate;
		private readonly string sqlWhereStringTemplate;
		private readonly bool hasOrder;
		private readonly bool hasWhere;
		// TODO H3:
		//private readonly int baseIndex;

		private readonly bool hasOrphanDelete;

		//types
		private readonly IType keyType;
		private readonly IType indexType;
		private readonly IType elementType;
		private readonly IType identifierType;

		//columns
		private readonly string[] keyColumnNames;
		private readonly string[] indexColumnNames;
		// TODO H3:
		//private readonly string[] indexFormulaTemplates;
		//private readonly string[] indexFormulas;
		private readonly string[] elementColumnNames;
		protected readonly string[] elementFormulaTemplates;
		protected readonly string[] elementFormulas;
		protected readonly bool[] elementColumnIsSettable;
		protected readonly bool[] elementColumnIsInPrimaryKey;
		private readonly string[] indexColumnAliases;
		protected readonly string[] elementColumnAliases;
		private readonly string[] keyColumnAliases;
		private readonly bool primitiveArray;
		private readonly bool array;
		//private readonly bool isOneToMany;
		protected readonly string qualifiedTableName;

		private readonly bool hasIndex;
		private readonly bool isLazy;
		private readonly bool isInverse;
		protected readonly int batchSize;
		private readonly System.Type elementClass;
		private readonly ICacheConcurrencyStrategy cache;
		private readonly CollectionType collectionType;
		private readonly FetchMode fetchMode;
		private readonly System.Type ownerClass;
		//private readonly bool isSorted;
		private readonly IIdentifierGenerator identifierGenerator;
		protected readonly bool hasIdentifier;

		//protected readonly string identifierColumnName;
		private readonly string identifierColumnName;
		private readonly string identifierColumnAlias;

		private readonly Dialect.Dialect dialect;
		//private readonly SQLExceptionConverter sqlExceptionConverter;
		private readonly ISessionFactoryImplementor factory;
		private readonly IPropertyMapping elementPropertyMapping;
		private readonly IEntityPersister elementPersister;

		private ICollectionInitializer initializer;

		private IDictionary collectionPropertyColumnAliases = new Hashtable();
		private IDictionary collectionPropertyColumnNames = new Hashtable();

		// dynamic filters for the collection
		private readonly FilterHelper filterHelper;

		// dynamic filters specifically for many-to-many inside the collection
		private readonly FilterHelper manyToManyFilterHelper;
		private readonly string manyToManyWhereString;
		private readonly string manyToManyWhereTemplate;
		private readonly string manyToManyOrderByString;
		private readonly string manyToManyOrderByTemplate;

		private static readonly ILog log = LogManager.GetLogger(typeof(ICollectionPersister));
		private string queryLoaderName;
		private bool subselectLoadable;
		private IEntityPersister ownerPersister;
		private bool isVersioned;

		public AbstractCollectionPersister(Mapping.Collection collection, ICacheConcurrencyStrategy cache,
		                                   ISessionFactoryImplementor factory)
		{
			this.factory = factory;
			dialect = factory.Dialect;
			this.cache = cache;
			//sqlExceptionConverter = factory.SQLExceptionConverter;
			collectionType = collection.CollectionType;
			role = collection.Role;
			ownerClass = collection.OwnerClass;
			ownerPersister = factory.GetEntityPersister(ownerClass);
			queryLoaderName = collection.LoaderName;
			Alias alias = new Alias("__");

			sqlOrderByString = collection.OrderBy;
			hasOrder = sqlOrderByString != null;
			sqlOrderByStringTemplate = hasOrder
			                           	? Template.RenderOrderByStringTemplate(sqlOrderByString, dialect,
			                           	                                       factory.SQLFunctionRegistry) : null;

			sqlWhereString = collection.Where;
			hasWhere = sqlWhereString != null;
			sqlWhereStringTemplate = hasWhere
			                         	? Template.RenderWhereStringTemplate(sqlWhereString, dialect, factory.SQLFunctionRegistry)
			                         	: null;

			hasOrphanDelete = collection.OrphanDelete;

			batchSize = collection.BatchSize;
			isVersioned = collection.IsOptimisticLocked;

			keyType = collection.Key.Type;
			int keySpan = collection.Key.ColumnSpan;
			keyColumnNames = new string[keySpan];
			string[] keyAliases = new string[keySpan];
			int k = 0;
			foreach (Column col in collection.Key.ColumnCollection)
			{
				keyColumnNames[k] = col.GetQuotedName(dialect);
				keyAliases[k] = col.GetAlias(dialect);
				k++;
			}
			keyColumnAliases = alias.ToAliasStrings(keyAliases, dialect);
			//unquotedKeyColumnNames = StringHelper.Unquote( keyColumnAliases );
			ISet distinctColumns = new HashedSet();
			CheckColumnDuplication(distinctColumns, collection.Key.ColumnCollection);

			//isSet = collection.IsSet;
			//isSorted = collection.IsSorted;
			primitiveArray = collection.IsPrimitiveArray;
			array = collection.IsArray;
			subselectLoadable = collection.IsSubselectLoadable;

			IValue element = collection.Element;
			int elementSpan = element.ColumnSpan;
			Table table = collection.CollectionTable;
			fetchMode = element.FetchMode;
			elementType = element.Type;

			if (!collection.IsOneToMany)
			{
				CheckColumnDuplication(distinctColumns, element.ColumnCollection);
			}

			if (elementType.IsEntityType)
			{
				elementPersister = factory.GetEntityPersister(((EntityType) elementType).AssociatedClass);
			}
			else
			{
				elementPersister = null;
			}

			qualifiedTableName = table.GetQualifiedName(dialect, factory.DefaultSchema);
			elementColumnAliases = new string[elementSpan];
			elementColumnNames = new string[elementSpan];
			elementFormulaTemplates = new string[elementSpan];
			elementFormulas = new string[elementSpan];
			elementColumnIsSettable = new bool[elementSpan];
			elementColumnIsInPrimaryKey = new bool[elementSpan];
			int j = 0;
			foreach (ISelectable selectable in element.ColumnCollection)
			{
				elementColumnAliases[j] = selectable.GetAlias(dialect);
				if (selectable.IsFormula)
				{
					Formula form = (Formula) selectable;
					elementFormulaTemplates[j] = form.GetTemplate(dialect, factory.SQLFunctionRegistry);
					elementFormulas[j] = form.FormulaString;
				}
				else
				{
					Column col = (Column) selectable;
					elementColumnNames[j] = col.GetQuotedName(dialect);
					elementColumnIsSettable[j] = true;
					elementColumnIsInPrimaryKey[j] = !col.IsNullable;
				}
				j++;
			}

			hasIndex = collection.IsIndexed;

			if (hasIndex)
			{
				IndexedCollection indexedCollection = (IndexedCollection) collection;

				indexType = indexedCollection.Index.Type;
				int indexSpan = indexedCollection.Index.ColumnSpan;
				indexColumnNames = new string[indexSpan];

				string[] indexAliases = new string[indexSpan];
				int i = 0;
				foreach (Column indexCol in indexedCollection.Index.ColumnCollection)
				{
					indexAliases[i] = indexCol.GetAlias(dialect);
					indexColumnNames[i] = indexCol.GetQuotedName(dialect);
					i++;
				}
				indexColumnAliases = alias.ToAliasStrings(indexAliases, dialect);
				CheckColumnDuplication(distinctColumns, indexedCollection.Index.ColumnCollection);
			}
			else
			{
				indexType = null;
				indexColumnNames = null;
				indexColumnAliases = null;
			}

			hasIdentifier = collection.IsIdentified;

			if (hasIdentifier)
			{
				if (collection.IsOneToMany)
				{
					throw new MappingException("one-to-many collections with identifiers are not supported.");
				}
				IdentifierCollection idColl = (IdentifierCollection) collection;
				identifierType = idColl.Identifier.Type;

				Column col = null;
				foreach (Column column in idColl.Identifier.ColumnCollection)
				{
					col = column;
					break;
				}

				identifierColumnName = col.GetQuotedName(dialect);
				identifierColumnAlias = alias.ToAliasString(col.GetAlias(dialect), dialect);
				identifierGenerator = idColl.Identifier.CreateIdentifierGenerator(dialect);
				CheckColumnDuplication(distinctColumns, idColl.Identifier.ColumnCollection);
			}
			else
			{
				identifierType = null;
				identifierColumnName = null;
				identifierColumnAlias = null;
				identifierGenerator = null;
			}

			sqlInsertRowString = GenerateInsertRowString();
			if (collection.CustomSQLInsert == null)
			{
				insertCallable = false;
				insertCheckStyle = ExecuteUpdateResultCheckStyle.Count;
			}
			else
			{
				sqlInsertRowString = new SqlCommandInfo(collection.CustomSQLInsert, sqlInsertRowString.ParameterTypes);
				insertCallable = collection.IsCustomInsertCallable;
				insertCheckStyle = collection.CustomSQLInsertCheckStyle == null
				                   	? ExecuteUpdateResultCheckStyle.DetermineDefault(collection.CustomSQLInsert, insertCallable)
				                   	: collection.CustomSQLInsertCheckStyle;
			}

			sqlUpdateRowString = GenerateUpdateRowString();
			if (collection.CustomSQLUpdate == null)
			{
				updateCallable = false;
				updateCheckStyle = ExecuteUpdateResultCheckStyle.Count;
			}
			else
			{
				sqlUpdateRowString = new SqlCommandInfo(collection.CustomSQLUpdate, sqlUpdateRowString.ParameterTypes);
				updateCallable = collection.IsCustomUpdateCallable;
				updateCheckStyle = collection.CustomSQLUpdateCheckStyle == null
				                   	? ExecuteUpdateResultCheckStyle.DetermineDefault(collection.CustomSQLUpdate, updateCallable)
				                   	: collection.CustomSQLUpdateCheckStyle;
			}

			sqlDeleteRowString = GenerateDeleteRowString();
			if (collection.CustomSQLDelete == null)
			{
				deleteCallable = false;
				deleteCheckStyle = ExecuteUpdateResultCheckStyle.None;
			}
			else
			{
				sqlDeleteRowString = new SqlCommandInfo(collection.CustomSQLDelete, sqlDeleteRowString.ParameterTypes);
				deleteCallable = collection.IsCustomDeleteCallable;
				deleteCheckStyle = ExecuteUpdateResultCheckStyle.None;
			}

			sqlDeleteString = GenerateDeleteString();
			if (collection.CustomSQLDeleteAll == null)
			{
				deleteAllCallable = false;
				deleteAllCheckStyle = ExecuteUpdateResultCheckStyle.None;
			}
			else
			{
				sqlDeleteString = new SqlCommandInfo(collection.CustomSQLDeleteAll, sqlDeleteString.ParameterTypes);
				deleteAllCallable = collection.IsCustomDeleteAllCallable;
				deleteAllCheckStyle = ExecuteUpdateResultCheckStyle.None;
			}

			isLazy = collection.IsLazy;

			isInverse = collection.IsInverse;

			if (collection.IsArray)
			{
				elementClass = ((Array) collection).ElementClass;
			}
			else
			{
				// for non-arrays, we don't need to know the element class
				elementClass = null;
			}

			if (elementType.IsComponentType)
			{
				elementPropertyMapping = new CompositeElementPropertyMapping(
					elementColumnNames, elementFormulaTemplates,
					(IAbstractComponentType) elementType, factory);
			}
			else if (!elementType.IsEntityType)
			{
				elementPropertyMapping = new ElementPropertyMapping(elementColumnNames, elementType);
			}
			else
			{
				IEntityPersister persister = factory.GetEntityPersister(((EntityType) elementType).AssociatedClass);
				// Not all classpersisters implement IPropertyMapping!
				if (persister is IPropertyMapping)
				{
					elementPropertyMapping = (IPropertyMapping) persister;
				}
				else
				{
					elementPropertyMapping = new ElementPropertyMapping(elementColumnNames, elementType);
				}
			}

			// Handle any filters applied to this collection
			filterHelper = new FilterHelper(collection.FilterMap, dialect, factory.SQLFunctionRegistry);

			// Handle any filters applied to this collection for many-to-many
			manyToManyFilterHelper = new FilterHelper(collection.ManyToManyFilterMap, dialect, factory.SQLFunctionRegistry);
			manyToManyWhereString = StringHelper.IsNotEmpty(collection.ManyToManyWhere) ?
			                        "( " + collection.ManyToManyWhere + " )" :
			                        null;
			manyToManyWhereTemplate = manyToManyWhereString == null
			                          	?
			                          null
			                          	:
			                          Template.RenderWhereStringTemplate(manyToManyWhereString, factory.Dialect,
			                                                             factory.SQLFunctionRegistry);
				// , factory.getSqlFunctionRegistry() );
			manyToManyOrderByString = collection.ManyToManyOrdering;
			manyToManyOrderByTemplate = manyToManyOrderByString == null
			                            	? null
			                            	: Template.RenderOrderByStringTemplate(manyToManyOrderByString, factory.Dialect,
			                            	                                       factory.SQLFunctionRegistry);
				// , factory.getSqlFunctionRegistry() );

			InitCollectionPropertyMap();
		}

		public void Initialize(object key, ISessionImplementor session)
		{
			try
			{
				GetAppropriateInitializer(key, session).Initialize(key, session);
			}
			catch (HibernateException)
			{
				// Do not call Convert on HibernateExceptions
				throw;
			}
			catch (Exception sqle)
			{
				throw Convert(sqle, "could not initialize collection: " + MessageHelper.InfoString(this, key));
			}
		}

		protected ICollectionInitializer GetAppropriateInitializer(object key, ISessionImplementor session)
		{
			if (queryLoaderName != null)
			{
				//if there is a user-specified loader, return that
				//TODO: filters!?
				return initializer;
			}

			ICollectionInitializer subselectInitializer = GetSubselectInitializer(key, session);
			if (subselectInitializer != null)
			{
				return subselectInitializer;
			}
			else if (session.EnabledFilters.Count == 0)
			{
				return initializer;
			}
			else
			{
				return CreateCollectionInitializer(session.EnabledFilters);
			}
		}

		private ICollectionInitializer GetSubselectInitializer(object key, ISessionImplementor session)
		{
			if (!IsSubselectLoadable)
			{
				return null;
			}

			SubselectFetch subselect = session.BatchFetchQueue.GetSubselect(new EntityKey(key, OwnerEntityPersister));

			if (subselect == null)
			{
				return null;
			}
			else
			{
				// Take care of any entities that might have
				// been evicted!
				ArrayList keysToRemove = new ArrayList();
				foreach (EntityKey entityKey in subselect.Result)
				{
					if (!session.ContainsEntity(entityKey))
					{
						keysToRemove.Add(entityKey);
					}
				}
				subselect.Result.RemoveAll(keysToRemove);

				// Run a subquery loader
				return CreateSubselectInitializer(subselect, session);
			}
		}

		protected abstract ICollectionInitializer CreateSubselectInitializer(SubselectFetch subselect,
		                                                                     ISessionImplementor session);

		protected abstract ICollectionInitializer CreateCollectionInitializer(IDictionary enabledFilters);

		public ICacheConcurrencyStrategy Cache
		{
			get { return cache; }
		}

		public bool HasCache
		{
			get { return cache != null; }
		}

		public CollectionType CollectionType
		{
			get { return collectionType; }
		}

		public string GetSQLWhereString(string alias)
		{
			return StringHelper.Replace(sqlWhereStringTemplate, Template.Placeholder, alias);
		}

		public string GetSQLOrderByString(string alias)
		{
			return StringHelper.Replace(sqlOrderByStringTemplate, Template.Placeholder, alias);
		}

		public FetchMode FetchMode
		{
			get { return fetchMode; }
		}

		public bool HasOrdering
		{
			get { return hasOrder; }
		}

		public bool HasWhere
		{
			get { return hasWhere; }
		}

		protected SqlCommandInfo SqlDeleteString
		{
			get { return sqlDeleteString; }
		}

		protected SqlCommandInfo SqlInsertRowString
		{
			get { return sqlInsertRowString; }
		}

		protected SqlCommandInfo SqlUpdateRowString
		{
			get { return sqlUpdateRowString; }
		}

		protected SqlCommandInfo SqlDeleteRowString
		{
			get { return sqlDeleteRowString; }
		}

		public IType KeyType
		{
			get { return keyType; }
		}

		public IType IndexType
		{
			get { return indexType; }
		}

		public IType ElementType
		{
			get { return elementType; }
		}

		/// <summary>
		/// Return the element class of an array, or null otherwise
		/// </summary>
		public System.Type ElementClass
		{
			// needed by arrays
			get { return elementClass; }
		}

		/// <summary>
		/// Reads the Element from the IDataReader.  The IDataReader will probably only contain
		/// the id of the Element.
		/// </summary>
		/// <remarks>See ReadElementIdentifier for an explanation of why this method will be depreciated.</remarks>
		public object ReadElement(IDataReader rs, object owner, string[] aliases, ISessionImplementor session)
		{
			object element = ElementType.NullSafeGet(rs, aliases, session, owner);
			return element;
		}

		public object ReadIndex(IDataReader rs, string[] aliases, ISessionImplementor session)
		{
			object index = IndexType.NullSafeGet(rs, aliases, session, null);
			if (index == null)
			{
				throw new HibernateException("null index column for collection: " + role);
			}
			return index;
		}

		public object ReadIdentifier(IDataReader rs, string alias, ISessionImplementor session)
		{
			object id = IdentifierType.NullSafeGet(rs, alias, session, null);
			if (id == null)
			{
				throw new HibernateException("null identifier column for collection: " + role);
			}
			return id;
		}

		public object ReadKey(IDataReader dr, string[] aliases, ISessionImplementor session)
		{
			return KeyType.NullSafeGet(dr, aliases, session, null);
		}

		protected int WriteElement(IDbCommand st, object elt, int i, ISessionImplementor session)
		{
			ElementType.NullSafeSet(st, elt, i, elementColumnIsSettable, session);
			return i + ArrayHelper.CountTrue(elementColumnIsSettable);
		}

		protected int WriteIndex(IDbCommand st, object idx, int i, ISessionImplementor session)
		{
			IndexType.NullSafeSet(st, idx, i, session);
			return i + indexColumnNames.Length;
		}

		protected int WriteElementToWhere(IDbCommand st, object elt, int i, ISessionImplementor session)
		{
			// TODO H3:
			//			if( elementIsPureFormula )
			//			{
			//				throw new AssertionFailure( "cannot use a formula-based element in the where condition" );
			//			}

			ElementType.NullSafeSet(st, elt, i, elementColumnIsSettable, session);
			return i + ArrayHelper.CountTrue(elementColumnIsSettable);
		}

		protected int WriteIndexToWhere(IDbCommand st, object index, int i, ISessionImplementor session)
		{
			// TODO H3:
			//			if( indexContainsFormula )
			//			{
			//				throw new AssertionFailure( "cannot use a formula-based index in the where condition" );
			//			}

			IndexType.NullSafeSet(st, index, i, session);
			return i + indexColumnAliases.Length;
		}

		protected int WriteIdentifier(IDbCommand st, object idx, int i, ISessionImplementor session)
		{
			IdentifierType.NullSafeSet(st, idx, i, session);
			return i + 1;
		}

		protected int WriteKey(IDbCommand st, object id, int i, ISessionImplementor session)
		{
			if (id == null)
			{
				throw new NullReferenceException("Null key for collection: " + role);
			}
			KeyType.NullSafeSet(st, id, i, session);
			return i + keyColumnAliases.Length;
		}

		public bool IsPrimitiveArray
		{
			get { return primitiveArray; }
		}

		public bool IsArray
		{
			get { return array; }
		}

		public string IdentifierColumnName
		{
			get
			{
				if (hasIdentifier)
				{
					return identifierColumnName;
				}
				else
				{
					return null;
				}
			}
		}

		public string SelectFragment(string alias, string columnSuffix)
		{
			SelectFragment frag = GenerateSelectFragment(alias, columnSuffix);
			AppendElementColumns(frag, alias);
			AppendIndexColumns(frag, alias);
			AppendIdentifierColumns(frag, alias);

			return frag.ToSqlStringFragment(false);
		}

		protected virtual SelectFragment GenerateSelectFragment(string alias, string columnSuffix)
		{
			SelectFragment frag = new SelectFragment(dialect)
				.SetSuffix(columnSuffix)
				.AddColumns(alias, keyColumnNames, keyColumnAliases);

			return frag;
		}

		protected virtual void AppendElementColumns(SelectFragment frag, string elemAlias)
		{
			for (int i = 0; i < elementColumnIsSettable.Length; i++)
			{
				if (elementColumnIsSettable[i])
				{
					frag.AddColumn(elemAlias, elementColumnNames[i], elementColumnAliases[i]);
				}
				else
				{
					frag.AddFormula(elemAlias, elementFormulaTemplates[i], elementColumnAliases[i]);
				}
			}
		}

		protected virtual void AppendIndexColumns(SelectFragment frag, string alias)
		{
			if (hasIndex)
			{
				frag.AddColumns(alias, indexColumnNames, indexColumnAliases);
			}
		}

		protected virtual void AppendIdentifierColumns(SelectFragment frag, string alias)
		{
			if (hasIdentifier)
			{
				frag.AddColumn(alias, identifierColumnName, identifierColumnAlias);
			}
		}

		public string[] IndexColumnNames
		{
			get { return indexColumnNames; }
		}

		public string[] GetIndexColumnNames(string alias)
		{
			return StringHelper.Qualify(alias, indexColumnNames);
			// TODO H3: return Qualify( alias, indexColumnNames, indexFormulaTemplates );
		}

		public string[] ElementColumnNames
		{
			get { return elementColumnNames; }
		}

		public string[] GetElementColumnNames(string alias)
		{
			return StringHelper.Qualify(alias, elementColumnNames);
			// TODO H3: return Qualify( alias, elementColumnNames, elementFormulaTemplates );
		}

		public string[] KeyColumnNames
		{
			get { return keyColumnNames; }
		}

		public bool HasIndex
		{
			get { return hasIndex; }
		}

		public bool IsLazy
		{
			get { return isLazy; }
		}

		public bool IsInverse
		{
			get { return isInverse; }
		}

		public string TableName
		{
			get { return qualifiedTableName; }
		}

		public void Remove(object id, ISessionImplementor session)
		{
			if (!isInverse)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("Deleting collection: " + MessageHelper.InfoString(this, id));
				}

				int offset = 0;
				IExpectation expectation = Expectations.AppropriateExpectation(deleteAllCheckStyle);
				bool useBatch = expectation.CanBeBatched;

				// Remove all the old entries
				try
				{
					IDbCommand st = useBatch
					                	? session.Batcher.PrepareBatchCommand(SqlDeleteString.CommandType, SqlDeleteString.Text,
					                	                                      SqlDeleteString.ParameterTypes)
					                	: session.Batcher.PrepareCommand(SqlDeleteString.CommandType, SqlDeleteString.Text,
					                	                                 SqlDeleteString.ParameterTypes);

					try
					{
						//offset += expectation.Prepare(st, factory.ConnectionProvider.Driver);
						WriteKey(st, id, offset, session);
						if (useBatch)
						{
							session.Batcher.AddToBatch(expectation);
						}
						else
						{
							expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(st), st);
						}
					}
					catch (Exception e)
					{
						if (useBatch)
						{
							session.Batcher.AbortBatch(e);
						}
						throw;
					}
					finally
					{
						if (!useBatch)
						{
							session.Batcher.CloseCommand(st, null);
						}
					}

					if (log.IsDebugEnabled)
					{
						log.Debug("done deleting collection");
					}
				}
				catch (HibernateException)
				{
					// Do not call Convert on HibernateExceptions
					throw;
				}
				catch (Exception sqle)
				{
					throw Convert(sqle, "could not delete collection: " + MessageHelper.InfoString(this, id));
				}
			}
		}

		public void Recreate(IPersistentCollection collection, object id, ISessionImplementor session)
		{
			if (!isInverse)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("Inserting collection: " + MessageHelper.InfoString(this, id));
				}

				try
				{
					// create all the new entries
					IEnumerable entries = collection.Entries();
					IExpectation expectation = Expectations.AppropriateExpectation(insertCheckStyle);
					bool useBatch = expectation.CanBeBatched;

					int i = 0;
					int count = 0;
					collection.PreInsert(this);

					IDbCommand st = null;
					foreach (object entry in entries)
					{
						if (collection.EntryExists(entry, i))
						{
							try
							{
								int offset = 0;
								if (useBatch)
								{
									if (st == null)
									{
										st = session.Batcher.PrepareBatchCommand(
											SqlInsertRowString.CommandType,
											SqlInsertRowString.Text,
											SqlInsertRowString.ParameterTypes);
									}
								}
								else
								{
									st = session.Batcher.PrepareCommand(
										SqlInsertRowString.CommandType,
										SqlInsertRowString.Text,
										SqlInsertRowString.ParameterTypes);
								}
								//offset += expectation.Prepare(st, factory.ConnectionProvider.Driver);
								int loc = WriteKey(st, id, offset, session);
								if (hasIdentifier)
								{
									loc = WriteIdentifier(st, collection.GetIdentifier(entry, i), loc, session);
								}
								if (hasIndex /*&&! !indexIsFormula */)
								{
									loc = WriteIndex(st, collection.GetIndex(entry, i), loc, session);
								}
								loc = WriteElement(st, collection.GetElement(entry), loc, session);
								if (useBatch)
								{
									session.Batcher.AddToBatch(expectation);
								}
								else
								{
									expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(st), st);
								}
								collection.AfterRowInsert(this, entry, i);
								count++;
							}
							catch (Exception e)
							{
								if (useBatch)
								{
									session.Batcher.AbortBatch(e);
								}
								throw;
							}
							finally
							{
								if (!useBatch)
								{
									session.Batcher.CloseCommand(st, null);
								}
							}
						}
						i++;
					}

					if (log.IsDebugEnabled)
					{
						log.Debug(string.Format("done inserting collection: {0} rows inserted", count));
					}

					if (count == 0 && log.IsDebugEnabled)
					{
						log.Debug("collection was empty");
					}
				}
				catch (HibernateException)
				{
					// Do not call Convert on HibernateExceptions
					throw;
				}
				catch (Exception sqle)
				{
					throw Convert(sqle, "could not insert collection: " + MessageHelper.InfoString(this, id));
				}
			}
		}

		public void DeleteRows(IPersistentCollection collection, object id, ISessionImplementor session)
		{
			if (!isInverse)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("Deleting rows of collection: " + MessageHelper.InfoString(this, id));
				}

				bool deleteByIndex = !IsOneToMany && hasIndex /* TODO H3: && !indexContainsFormula*/;

				try
				{
					// delete all the deleted entries
					ICollection deletes = collection.GetDeletes(elementType, !deleteByIndex);
					if (deletes.Count > 0)
					{
						int offset = 0;
						int count = 0;
						IExpectation expectation = Expectations.AppropriateExpectation(deleteCheckStyle);
						bool useBatch = expectation.CanBeBatched;
						IDbCommand st = null;

						try
						{
							foreach (object entry in deletes)
							{
								if (useBatch)
								{
									if (st != null)
									{
										st = session.Batcher.PrepareBatchCommand(
											SqlDeleteRowString.CommandType,
											SqlDeleteRowString.Text,
											SqlDeleteRowString.ParameterTypes);
									}
								}
								else
								{
									st = session.Batcher.PrepareCommand(
										SqlDeleteRowString.CommandType,
										SqlDeleteRowString.Text,
										SqlDeleteRowString.ParameterTypes);
								}
								//offset += expectation.Prepare(st, factory.ConnectionProvider.Driver);
								int loc = offset;
								if (hasIdentifier)
								{
									WriteIdentifier(st, entry, loc, session);
								}
								else
								{
									loc = WriteKey(st, id, loc, session);

									if (deleteByIndex)
									{
										WriteIndexToWhere(st, entry, loc, session);
									}
									else
									{
										WriteElementToWhere(st, entry, loc, session);
									}
								}
								if (useBatch)
								{
									session.Batcher.AddToBatch(expectation);
								}
								else
								{
									expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(st), st);
								}
								count++;
							}
						}
						catch (Exception e)
						{
							if (useBatch)
							{
								session.Batcher.AbortBatch(e);
							}
							throw;
						}
						finally
						{
							if (!useBatch)
							{
								session.Batcher.CloseCommand(st, null);
							}
						}

						if (log.IsDebugEnabled)
						{
							log.Debug("done deleting collection rows: " + count + " deleted");
						}
					}
					else
					{
						if (log.IsDebugEnabled)
						{
							log.Debug("no rows to delete");
						}
					}
				}
				catch (HibernateException)
				{
					// Do not call Convert on HibernateExceptions
					throw;
				}
				catch (Exception sqle)
				{
					throw Convert(sqle, "could not delete collection rows: " + MessageHelper.InfoString(this, id));
				}
			}
		}

		public void InsertRows(IPersistentCollection collection, object id, ISessionImplementor session)
		{
			if (!isInverse)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("Inserting rows of collection: " + role + "#" + id);
				}

				try
				{
					// insert all the new entries
					collection.PreInsert(this);
					IExpectation expectation = Expectations.AppropriateExpectation(insertCheckStyle);
					bool useBatch = expectation.CanBeBatched;
					int i = 0;
					int count = 0;
					int offset = 0;

					IEnumerable entries = collection.Entries();
					foreach (object entry in entries)
					{
						if (collection.NeedsInserting(entry, i, elementType))
						{
							IDbCommand st = useBatch
							                	? session.Batcher.PrepareBatchCommand(
							                	  	SqlInsertRowString.CommandType,
							                	  	SqlInsertRowString.Text,
							                	  	SqlInsertRowString.ParameterTypes)
							                	: session.Batcher.PrepareCommand(
							                	  	SqlInsertRowString.CommandType,
							                	  	SqlInsertRowString.Text,
							                	  	SqlInsertRowString.ParameterTypes);
							try
							{
								//offset += expectation.Prepare(st, factory.ConnectionProvider.Driver);
								int loc = WriteKey(st, id, offset, session);
								if (hasIdentifier)
								{
									loc = WriteIdentifier(st, collection.GetIdentifier(entry, i), loc, session);
								}
								if (hasIndex /*&& !indexIsFormula*/)
								{
									loc = WriteIndex(st, collection.GetIndex(entry, i), loc, session);
								}
								loc = WriteElement(st, collection.GetElement(entry), loc, session);
								if (useBatch)
								{
									session.Batcher.AddToBatch(expectation);
								}
								else
								{
									expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(st), st);
								}
								collection.AfterRowInsert(this, entry, i);
								count++;
							}
							catch (Exception e)
							{
								if (useBatch)
								{
									session.Batcher.AbortBatch(e);
								}
								throw;
							}
							finally
							{
								if (!useBatch)
								{
									session.Batcher.CloseCommand(st, null);
								}
							}
						}
						i++;
					}

					if (log.IsDebugEnabled)
					{
						log.Debug(string.Format("done inserting rows: {0} inserted", count));
					}
				}
				catch (HibernateException)
				{
					// Do not call Convert on HibernateExceptions
					throw;
				}
				catch (Exception sqle)
				{
					throw Convert(sqle, "could not insert collection rows: " + MessageHelper.InfoString(this, id));
				}
			}
		}

		/// <summary>
		/// Get the name of this collection role (the fully qualified class name,
		/// extended by a "property path")
		/// </summary>
		public string Role
		{
			get { return role; }
		}

		public System.Type OwnerClass
		{
			get { return ownerClass; }
		}

		public IIdentifierGenerator IdentifierGenerator
		{
			get { return identifierGenerator; }
		}

		public IType IdentifierType
		{
			get { return identifierType; }
		}

		public bool HasOrphanDelete
		{
			get { return hasOrphanDelete; }
		}

		private void CheckColumnDuplication(ISet distinctColumns, ICollection columns)
		{
			foreach (ISelectable sel in columns)
			{
				Column col = sel as Column;
				if (col == null)
				{
					// Ignore formulas
					continue;
				}
				if (distinctColumns.Contains(col.Name))
				{
					throw new MappingException(
						"Repeated column in mapping for collection: " +
						role +
						" column: " +
						col.Name
						);
				}
				else
				{
					distinctColumns.Add(col.Name);
				}
			}
		}

		public IType ToType(string propertyName)
		{
			if ("index".Equals(propertyName))
			{
				return indexType;
			}
			return elementPropertyMapping.ToType(propertyName);
		}

		public string[] ToColumns(string alias, string propertyName)
		{
			if ("index".Equals(propertyName))
			{
				if (IsManyToMany)
				{
					throw new QueryException("index() function not supported for many-to-many association");
				}
				return StringHelper.Qualify(alias, indexColumnNames);
			}
			return elementPropertyMapping.ToColumns(alias, propertyName);
		}

		public IType Type
		{
			get { return elementPropertyMapping.Type; }
		}

		public string Name
		{
			get { return Role; }
		}

		public IEntityPersister ElementPersister
		{
			get
			{
				if (elementPersister == null)
				{
					throw new AssertionFailure("Not an association");
				}

				return elementPersister;
			}
		}

		public bool IsCollection
		{
			get { return true; }
		}

		public object CollectionSpace
		{
			get { return TableName; }
		}

		protected abstract SqlCommandInfo GenerateDeleteString();
		protected abstract SqlCommandInfo GenerateDeleteRowString();
		protected abstract SqlCommandInfo GenerateUpdateRowString();
		protected abstract SqlCommandInfo GenerateInsertRowString();

		public void UpdateRows(IPersistentCollection collection, object id, ISessionImplementor session)
		{
			if (!isInverse)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug(string.Format("Updating rows of collection: {0}#{1}", role, id));
				}

				// update all the modified entries
				int count = DoUpdateRows(id, collection, session);

				if (log.IsDebugEnabled)
				{
					log.Debug(string.Format("done updating rows: {0} updated", count));
				}
			}
		}

		protected abstract int DoUpdateRows(object key, IPersistentCollection collection, ISessionImplementor session);

		public ICollectionMetadata CollectionMetadata
		{
			get { return this; }
		}

		protected ADOException Convert(Exception sqlException, string message)
		{
			return ADOExceptionHelper.Convert(sqlException, message);
		}

		public override string ToString()
		{
			// Java has StringHelper.root( getClass().getName() ) instead of GetType().Name,
			// but it doesn't make sense to me.
			return GetType().Name + '(' + role + ')';
		}

		/// <summary>
		/// Gets just the Identifier of the Element for the Collection.
		/// </summary>
		/// <param name="rs"></param>
		/// <param name="owner"></param>
		/// <param name="session"></param>
		/// <returns></returns>
		/// <remarks>
		/// This was created in addition to ReadElement because ADO.NET does not allow
		/// for 2 IDataReaders to be open against a single IDbConnection at one time.  
		/// 
		/// When a Collection is loaded it was recursively opening IDbDataReaders to resolve
		/// the Element for the Collection while the IDbDataReader was open that contained the
		/// record for the Collection.
		/// </remarks>		
		public object ReadElementIdentifier(IDataReader rs, object owner, ISessionImplementor session)
		{
			return ElementType.Hydrate(rs, elementColumnAliases, session, owner);
		}

		public abstract string SelectFragment(IJoinable rhs, string rhsAlias, string lhsAlias, string currentEntitySuffix,
		                                      string currentCollectionSuffix, bool includeCollectionColumns);

		public abstract SqlString FromJoinFragment(string alias, bool innerJoin, bool includeSubclasses);
		public abstract SqlString WhereJoinFragment(string alias, bool innerJoin, bool includeSubclasses);

		public abstract bool ConsumesEntityAlias();
		public abstract bool ConsumesCollectionAlias();
		public abstract bool IsManyToMany { get; }
		public abstract bool IsOneToMany { get; }

		public void PostInstantiate()
		{
			initializer = queryLoaderName == null
			              	? CreateCollectionInitializer(CollectionHelper.EmptyMap)
			              	: new NamedQueryCollectionInitializer(queryLoaderName, this);
		}

		protected virtual string FilterFragment(string alias)
		{
			return HasWhere ? " and " + GetSQLWhereString(alias) : "";
		}

		public virtual string FilterFragment(string alias, IDictionary enabledFilters)
		{
			StringBuilder sessionFilterFragment = new StringBuilder();
			filterHelper.Render(sessionFilterFragment, alias, enabledFilters);

			return sessionFilterFragment.Append(FilterFragment(alias)).ToString();
		}

		public string GetManyToManyFilterFragment(string alias, IDictionary enabledFilters)
		{
			StringBuilder buffer = new StringBuilder();
			manyToManyFilterHelper.Render(buffer, alias, enabledFilters);

			if (manyToManyWhereString != null)
			{
				buffer.Append(" and ")
					.Append(StringHelper.Replace(manyToManyWhereTemplate, Template.Placeholder, alias));
			}

			return buffer.ToString();
		}

		public bool IsAffectedByEnabledFilters(ISessionImplementor session)
		{
			return filterHelper.IsAffectedBy(session.EnabledFilters)
			       || (IsManyToMany && manyToManyFilterHelper.IsAffectedBy(session.EnabledFilters));
		}

		public ISessionFactoryImplementor Factory
		{
			get { return factory; }
		}

		public string[] GetKeyColumnAliases(string suffix)
		{
			return new Alias(suffix).ToAliasStrings(keyColumnAliases, dialect);
		}

		public string[] GetElementColumnAliases(string suffix)
		{
			return new Alias(suffix).ToAliasStrings(elementColumnAliases, dialect);
		}

		public string[] GetIndexColumnAliases(string suffix)
		{
			if (hasIndex)
			{
				return new Alias(suffix).ToAliasStrings(indexColumnAliases, dialect);
			}
			else
			{
				return null;
			}
		}

		public string GetIdentifierColumnAlias(string suffix)
		{
			if (hasIdentifier)
			{
				return new Alias(suffix).ToAliasString(identifierColumnAlias, dialect);
			}
			else
			{
				return null;
			}
		}

		public string OneToManyFilterFragment(string alias)
		{
			return string.Empty;
		}

		public string[] GetCollectionPropertyColumnAliases(string propertyName, string suffix)
		{
			string[] rawAliases = (string[]) collectionPropertyColumnAliases[propertyName];

			if (rawAliases == null)
			{
				return null;
			}

			string[] result = new string[rawAliases.Length];
			for (int i = 0; i < rawAliases.Length; i++)
			{
				result[i] = new Alias(suffix).ToUnquotedAliasString(rawAliases[i], dialect);
			}
			return result;
		}

		public void InitCollectionPropertyMap()
		{
			InitCollectionPropertyMap("key", keyType, keyColumnAliases, keyColumnNames);
			InitCollectionPropertyMap("element", elementType, elementColumnAliases, elementColumnNames);

			if (hasIndex)
			{
				InitCollectionPropertyMap("index", indexType, indexColumnAliases, indexColumnNames);
			}

			if (hasIdentifier)
			{
				InitCollectionPropertyMap(
					"id",
					identifierType,
					new string[] {identifierColumnAlias},
					new string[] {identifierColumnName});
			}
		}

		private void InitCollectionPropertyMap(string aliasName, IType type, string[] columnAliases, string[] columnNames)
		{
			collectionPropertyColumnAliases[aliasName] = columnAliases;
			collectionPropertyColumnNames[aliasName] = columnNames;

			if (type.IsComponentType)
			{
				IAbstractComponentType ct = (IAbstractComponentType) type;
				string[] propertyNames = ct.PropertyNames;
				for (int i = 0; i < propertyNames.Length; i++)
				{
					string name = propertyNames[i];
					collectionPropertyColumnAliases[aliasName + "." + name] = columnAliases[i];
					collectionPropertyColumnNames[aliasName + "." + name] = columnNames[i];
				}
			}
		}

		protected ExecuteUpdateResultCheckStyle InsertCheckStyle
		{
			get { return insertCheckStyle; }
		}

		protected ExecuteUpdateResultCheckStyle UpdateCheckStyle
		{
			get { return updateCheckStyle; }
		}

		protected ExecuteUpdateResultCheckStyle DeleteCheckStyle
		{
			get { return deleteCheckStyle; }
		}

		protected ExecuteUpdateResultCheckStyle DeleteAllCheckStyle
		{
			get { return deleteAllCheckStyle; }
		}

		public bool IsSubselectLoadable
		{
			get { return subselectLoadable; }
		}

		public IEntityPersister OwnerEntityPersister
		{
			get { return ownerPersister; }
		}

		public bool HasManyToManyOrdering
		{
			get { return IsManyToMany && manyToManyOrderByTemplate != null; }
		}

		public string GetManyToManyOrderByString(string alias)
		{
			if (IsManyToMany && manyToManyOrderByString != null)
			{
				return StringHelper.Replace(manyToManyOrderByTemplate, Template.Placeholder, alias);
			}
			else
			{
				return "";
			}
		}

		public bool IsVersioned
		{
			get { return isVersioned && OwnerEntityPersister.IsVersioned; }
		}
	}
}