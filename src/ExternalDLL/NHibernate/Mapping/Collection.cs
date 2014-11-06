using System;
using System.Collections;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.Mapping
{
	/// <summary>
	/// Base class that stores the mapping information for <c>&lt;array&gt;</c>, <c>&lt;bag&gt;</c>, 
	/// <c>&lt;id-bag&gt;</c>, <c>&lt;list&gt;</c>, <c>&lt;map&gt;</c>, and <c>&lt;set&gt;</c>
	/// collections.
	/// </summary>
	/// <remarks> 
	/// Subclasses are responsible for the specialization required for the particular
	/// collection style.
	/// </remarks>
	public abstract class Collection : IFetchable, IValue, IFilterable
	{
		private static readonly ICollection EmptyColumns = new object[0];

		public const string DefaultElementColumnName = "elt";
		public const string DefaultKeyColumnName = "id";

		private SimpleValue key;
		private IValue element;
		private Table collectionTable;
		private string role;
		private bool lazy;
		private bool inverse;
		private string cacheConcurrencyStrategy;
		private String cacheRegionName;
		private string orderBy;
		private string where;
		private PersistentClass owner;
		private bool sorted;
		private object comparer;
		private bool orphanDelete;
		private int batchSize = 1;
		private FetchMode fetchMode;
		private System.Type collectionPersisterClass;
		private string referencedPropertyName;
		private string typeName;

		private string loaderName;

		private SqlString customSQLInsert;
		private bool customInsertCallable;
		private ExecuteUpdateResultCheckStyle insertCheckStyle;

		private SqlString customSQLDelete;
		private bool customDeleteCallable;
		private ExecuteUpdateResultCheckStyle deleteCheckStyle;

		private SqlString customSQLUpdate;
		private bool customUpdateCallable;
		private ExecuteUpdateResultCheckStyle updateCheckStyle;

		private SqlString customSQLDeleteAll;
		private bool customDeleteAllCallable;
		private ExecuteUpdateResultCheckStyle deleteAllCheckStyle;


		private bool isGeneric;
		private System.Type[] genericArguments;
		private IDictionary filters = new Hashtable();
		private IDictionary manyToManyFilters = new Hashtable();
		private bool subselectLoadable;
		private string manyToManyWhere;
		private string manyToManyOrderBy;
		private bool optimisticLocked;

		protected Collection(PersistentClass owner)
		{
			this.owner = owner;
		}

		public int ColumnSpan
		{
			get { return 0; }
		}

		public virtual bool IsSet
		{
			get { return false; }
		}

		public SimpleValue Key
		{
			get { return key; }
			set { key = value; }
		}

		public IValue Element
		{
			get { return element; }
			set { element = value; }
		}

		public virtual bool IsIndexed
		{
			get { return false; }
		}

		public Table CollectionTable
		{
			get { return collectionTable; }
			set { collectionTable = value; }
		}

		public Table Table
		{
			get { return Owner.Table; }
		}

		public bool IsSorted
		{
			get { return sorted; }
			set { sorted = value; }
		}

		public bool HasOrder
		{
			get { return orderBy != null; }
		}

		public PersistentClass Owner
		{
			get { return owner; }
			set { owner = value; }
		}

		public System.Type CollectionPersisterClass
		{
			get { return collectionPersisterClass; }
			set { collectionPersisterClass = value; }
		}

		// The type of this property is object, so as to accomodate
		// both IComparer and IComparer<T>.
		public object Comparer
		{
			get { return comparer; }
			set { comparer = value; }
		}

		public bool IsLazy
		{
			get { return lazy; }
			set { lazy = value; }
		}

		public string Role
		{
			get { return role; }
			set { role = value; }
		}

		public ICollection ColumnCollection
		{
			get { return EmptyColumns; }
		}

		public Formula Formula
		{
			get { return null; }
		}

		public bool IsNullable
		{
			get { return true; }
		}

		public bool IsUnique
		{
			get { return false; }
		}

		public virtual CollectionType CollectionType
		{
			get
			{
				if (typeName == null)
				{
					return DefaultCollectionType;
				}
				else
				{
					return TypeFactory.CustomCollection(typeName, Role, ReferencedPropertyName);
				}
			}
		}

		public abstract CollectionType DefaultCollectionType { get; }

		public IType Type
		{
			get { return CollectionType; }
		}

		public virtual bool IsPrimitiveArray
		{
			get { return false; }
		}

		public virtual bool IsArray
		{
			get { return false; }
		}

		public virtual bool HasFormula
		{
			get { return false; }
		}

		public virtual bool IsIdentified
		{
			get { return false; }
		}

		public bool IsOneToMany
		{
			get { return element is OneToMany; }
		}

		public string CacheConcurrencyStrategy
		{
			get { return cacheConcurrencyStrategy; }
			set { cacheConcurrencyStrategy = value; }
		}

		public string CacheRegionName
		{
			get { return cacheRegionName == null ? Role : cacheRegionName; }
			set { cacheRegionName = value; }
		}

		public bool IsInverse
		{
			get { return inverse; }
			set { inverse = value; }
		}

		public System.Type OwnerClass
		{
			get { return owner.MappedClass; }
		}

		public string OrderBy
		{
			get { return orderBy; }
			set { orderBy = value; }
		}

		public string Where
		{
			get { return where; }
			set { where = value; }
		}

		public bool OrphanDelete
		{
			get { return orphanDelete; }
			set { orphanDelete = value; }
		}

		public int BatchSize
		{
			get { return batchSize; }
			set { batchSize = value; }
		}

		public FetchMode FetchMode
		{
			get { return fetchMode; }
			set { fetchMode = value; }
		}


		/// <summary>
		/// Gets or sets a <see cref="Boolean"/> indicating if this is a 
		/// mapping for a generic collection.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if a collection from the System.Collections.Generic namespace
		/// should be used, <see langword="false" /> if a collection from the System.Collections 
		/// namespace should be used.
		/// </value>
		/// <remarks>
		/// This has no affect on any versions of the .net framework before .net-2.0.
		/// </remarks>
		public bool IsGeneric
		{
			get { return isGeneric; }
			set { isGeneric = value; }
		}

		/// <summary>
		/// Gets or sets an array of <see cref="System.Type"/> that contains the arguments
		/// needed to construct an instance of a closed type.
		/// </summary>
		public System.Type[] GenericArguments
		{
			get { return genericArguments; }
			set { genericArguments = value; }
		}

		protected void CheckGenericArgumentsLength(int expectedLength)
		{
			if (genericArguments.Length != expectedLength)
			{
				throw new MappingException(
					string.Format(
						"Error mapping generic collection {0}: expected {1} generic parameters, but the property type has {2}",
						Role, expectedLength, genericArguments.Length));
			}
		}

		public void CreateForeignKey()
		{
		}

		private void CreateForeignKeys()
		{
			// for inverse collections, let the "other end" hanlde it
			if (!IsInverse)
			{
				Element.CreateForeignKey();
				Key.CreateForeignKeyOfClass(Owner.MappedClass);
			}
		}

		public abstract void CreatePrimaryKey();

		public virtual void CreateAllKeys()
		{
			CreateForeignKeys();
			if (!IsInverse)
			{
				CreatePrimaryKey();
			}
		}

		public bool IsSimpleValue
		{
			get { return false; }
		}

		public bool IsValid(IMapping mapping)
		{
			return true;
		}

		public string ReferencedPropertyName
		{
			get { return referencedPropertyName; }
			set { referencedPropertyName = StringHelper.InternedIfPossible(value); }
		}

		public virtual void Validate(IMapping mapping)
		{
			if (!Key.IsValid(mapping))
			{
				throw new MappingException(
					string.Format("collection foreign key mapping has wrong number of columns: {0} type: {1}", Role, Key.Type.Name));
			}

			if (!Element.IsValid(mapping))
			{
				throw new MappingException(
					string.Format("collection element key mapping has wrong number of columns: {0} type: {1}", Role, Element.Type.Name));
			}
		}

		public bool[] ColumnInsertability
		{
			get { return ArrayHelper.EmptyBoolArray; }
		}

		public bool[] ColumnUpdateability
		{
			get { return ArrayHelper.EmptyBoolArray; }
		}

		public string TypeName
		{
			get { return typeName; }
			set { typeName = value; }
		}

		public SqlString CustomSQLInsert
		{
			get { return customSQLInsert; }
		}

		public SqlString CustomSQLDelete
		{
			get { return customSQLDelete; }
		}

		public SqlString CustomSQLUpdate
		{
			get { return customSQLUpdate; }
		}

		public SqlString CustomSQLDeleteAll
		{
			get { return customSQLDeleteAll; }
		}

		public bool IsCustomInsertCallable
		{
			get { return customInsertCallable; }
		}

		public bool IsCustomDeleteCallable
		{
			get { return customDeleteCallable; }
		}

		public bool IsCustomUpdateCallable
		{
			get { return customUpdateCallable; }
		}

		public bool IsCustomDeleteAllCallable
		{
			get { return customDeleteAllCallable; }
		}

		public ExecuteUpdateResultCheckStyle CustomSQLInsertCheckStyle
		{
			get { return insertCheckStyle; }
		}

		public ExecuteUpdateResultCheckStyle CustomSQLDeleteCheckStyle
		{
			get { return deleteCheckStyle; }
		}

		public ExecuteUpdateResultCheckStyle CustomSQLUpdateCheckStyle
		{
			get { return updateCheckStyle; }
		}

		public ExecuteUpdateResultCheckStyle CustomSQLDeleteAllCheckStyle
		{
			get { return deleteAllCheckStyle; }
		}

		public void SetCustomSQLInsert(string sql, bool callable, ExecuteUpdateResultCheckStyle checkStyle)
		{
			customSQLInsert = SqlString.Parse(sql);
			customInsertCallable = callable;
			insertCheckStyle = checkStyle;
		}

		public void SetCustomSQLDelete(string sql, bool callable, ExecuteUpdateResultCheckStyle checkStyle)
		{
			customSQLDelete = SqlString.Parse(sql);
			customDeleteCallable = callable;
			deleteCheckStyle = checkStyle;
		}

		public void SetCustomSQLDeleteAll(string sql, bool callable, ExecuteUpdateResultCheckStyle checkStyle)
		{
			customSQLDeleteAll = SqlString.Parse(sql);
			customDeleteAllCallable = callable;
			deleteAllCheckStyle = checkStyle;
		}

		public void SetCustomSQLUpdate(string sql, bool callable, ExecuteUpdateResultCheckStyle checkStyle)
		{
			customSQLUpdate = SqlString.Parse(sql);
			customUpdateCallable = callable;
			updateCheckStyle = checkStyle;
		}

		public void AddFilter(string name, string condition)
		{
			filters.Add(name, condition);
		}

		public IDictionary FilterMap
		{
			get { return filters; }
		}

		public void AddManyToManyFilter(string name, string condition)
		{
			manyToManyFilters.Add(name, condition);
		}

		public IDictionary ManyToManyFilterMap
		{
			get { return manyToManyFilters; }
		}

		public string LoaderName
		{
			get { return loaderName; }
			set { loaderName = StringHelper.InternedIfPossible(value); }
		}

		public bool IsSubselectLoadable
		{
			get { return subselectLoadable; }
			set { subselectLoadable = value; }
		}

		public string ManyToManyWhere
		{
			get { return manyToManyWhere; }
			set { manyToManyWhere = value; }
		}

		public string ManyToManyOrdering
		{
			get { return manyToManyOrderBy; }
			set { manyToManyOrderBy = value; }
		}

		public bool IsOptimisticLocked
		{
			get { return optimisticLocked; }
			set { optimisticLocked = value; }
		}
	}
}