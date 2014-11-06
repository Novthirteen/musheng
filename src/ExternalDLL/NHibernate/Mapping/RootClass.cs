using System;
using System.Collections;
using Iesi.Collections;
using log4net;
using NHibernate.Engine;

namespace NHibernate.Mapping
{
	/// <summary>
	/// Declaration of a System.Type mapped with the <c>&lt;class&gt;</c> element that
	/// is the root class of a table-per-sublcass, or table-per-concrete-class 
	/// inheritance heirarchy.
	/// </summary>
	public class RootClass : PersistentClass
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(RootClass));

		/// <summary>
		/// The default name of the column for the Identifier
		/// </summary>
		/// <value><c>id</c> is the default column name for the Identifier.</value>
		public const string DefaultIdentifierColumnName = "id";

		/// <summary>
		/// The default name of the column for the Discriminator
		/// </summary>
		/// <value><c>class</c> is the default column name for the Discriminator.</value>
		public const string DefaultDiscriminatorColumnName = "class";

		private Property identifierProperty;
		private SimpleValue identifier;
		private Property version;
		private bool polymorphic;
		private string cacheConcurrencyStrategy;
		private string cacheRegionName;
		private SimpleValue discriminator;
		private bool mutable;
		private bool embeddedIdentifier = false;
		private bool explicitPolymorphism;
		private System.Type classPersisterClass;
		private bool forceDiscriminator;
		private string where;
		private bool discriminatorInsertable = true;
		private int nextSubclassId = 0;

		internal override int NextSubclassId()
		{
			return ++nextSubclassId;
		}

		public override int SubclassId
		{
			get { return 0; }
		}

		/// <summary>
		/// Gets or sets the <see cref="Property"/> that is used as the <c>id</c>.
		/// </summary>
		/// <value>
		/// The <see cref="Property"/> that is used as the <c>id</c>.
		/// </value>
		public override Property IdentifierProperty
		{
			get { return identifierProperty; }
			set
			{
				identifierProperty = value;
				identifierProperty.PersistentClass = this;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="SimpleValue"/> that contains information about the identifier.
		/// </summary>
		/// <value>The <see cref="SimpleValue"/> that contains information about the identifier.</value>
		public override SimpleValue Identifier
		{
			get { return identifier; }
			set { identifier = value; }
		}

		/// <summary>
		/// Gets a boolean indicating if the mapped class has a Property for the <c>id</c>.
		/// </summary>
		/// <value><see langword="true" /> if there is a Property for the <c>id</c>.</value>
		public override bool HasIdentifierProperty
		{
			get { return identifierProperty != null; }
		}

		/// <summary>
		/// Gets or sets the <see cref="SimpleValue"/> that contains information about the discriminator.
		/// </summary>
		/// <value>The <see cref="SimpleValue"/> that contains information about the discriminator.</value>
		public override SimpleValue Discriminator
		{
			get { return discriminator; }
			set { discriminator = value; }
		}

		/// <summary>
		/// Gets a boolean indicating if this mapped class is inherited from another. 
		/// </summary>
		/// <value>
		/// <see langword="false" /> because this is the root mapped class.
		/// </value>
		public override bool IsInherited
		{
			get { return false; }
		}

		/// <summary>
		/// Gets or sets if the mapped class has subclasses.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the mapped class has subclasses.
		/// </value>
		public override bool IsPolymorphic
		{
			get { return polymorphic; }
			set { polymorphic = value; }
		}

		/// <summary>
		/// Gets the <see cref="RootClass"/> of the class that is mapped in the <c>class</c> element.
		/// </summary>
		/// <value>
		/// <c>this</c> since this is the root mapped class.
		/// </value>
		public override RootClass RootClazz
		{
			get { return this; }
		}

		/// <summary>
		/// Gets an <see cref="ICollection"/> of <see cref="Property"/> objects that this mapped class contains.
		/// </summary>
		/// <value>
		/// An <see cref="ICollection"/> of <see cref="Property"/> objects that 
		/// this mapped class contains.
		/// </value>
		public override ICollection PropertyClosureCollection
		{
			get { return PropertyCollection; }
		}

		/// <summary>
		/// Gets an <see cref="ICollection"/> of <see cref="Table"/> objects that this 
		/// mapped class reads from and writes to.
		/// </summary>
		/// <value>
		/// An <see cref="ICollection"/> of <see cref="Table"/> objects that 
		/// this mapped class reads from and writes to.
		/// </value>
		/// <remarks>
		/// There is only one <see cref="Table"/> in the <see cref="ICollection"/> since
		/// this is the root class.
		/// </remarks>
		public override ICollection TableClosureCollection
		{
			get
			{
				ArrayList retVal = new ArrayList();
				retVal.Add(Table);
				return retVal;
			}
		}

		/// <summary>
		/// Adds a <see cref="Subclass"/> to the class hierarchy.
		/// </summary>
		/// <param name="subclass">The <see cref="Subclass"/> to add to the hierarchy.</param>
		/// <remarks>
		/// When a <see cref="Subclass"/> is added this mapped class has the property <see cref="IsPolymorphic"/>
		/// set to <see langword="true" />.
		/// </remarks>
		public override void AddSubclass(Subclass subclass)
		{
			base.AddSubclass(subclass);
			polymorphic = true;
		}

		/// <summary>
		/// Gets or sets a boolean indicating if explicit polymorphism should be used in Queries.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if only classes queried on should be returned, <see langword="false" />
		/// if any class in the heirarchy should implicitly be returned.
		/// </value>
		public override bool IsExplicitPolymorphism
		{
			get { return explicitPolymorphism; }
			set { explicitPolymorphism = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="Property"/> that is used as the version.
		/// </summary>
		/// <value>The <see cref="Property"/> that is used as the version.</value>
		public override Property Version
		{
			get { return version; }
			set { version = value; }
		}

		/// <summary>
		/// Gets a boolean indicating if the mapped class has a version property.
		/// </summary>
		/// <value><see langword="true" /> if there is a Property for a <c>version</c>.</value>
		public override bool IsVersioned
		{
			get { return version != null; }
		}

		/// <summary>
		/// Gets or sets the CacheConcurrencyStrategy
		/// to use to read/write instances of the persistent class to the Cache.
		/// </summary>
		/// <value>The CacheConcurrencyStrategy used with the Cache.</value>
		public override string CacheConcurrencyStrategy
		{
			get { return cacheConcurrencyStrategy; }
			set { cacheConcurrencyStrategy = value; }
		}

		/// <summary>
		/// Gets or sets the cache region name.
		/// </summary>
		/// <value>The region name used with the Cache.</value>
		public string CacheRegionName
		{
			get { return cacheRegionName == null ? Name : cacheRegionName; }
			set { cacheRegionName = value; }
		}

		/// <summary>
		/// Gets or set a boolean indicating if the mapped class has properties that can be changed.
		/// </summary>
		/// <value><see langword="true" /> if the object is mutable.</value>
		public override bool IsMutable
		{
			get { return mutable; }
			set { mutable = value; }
		}

		/// <summary>
		/// Gets or sets a boolean indicating if the identifier is 
		/// embedded in the class.
		/// </summary>
		/// <value><see langword="true" /> if the class identifies itself.</value>
		/// <remarks>
		/// An embedded identifier is true when using a <c>composite-id</c> specifying
		/// properties of the class as the <c>key-property</c> instead of using a class
		/// as the <c>composite-id</c>.
		/// </remarks>
		public override bool HasEmbeddedIdentifier
		{
			get { return embeddedIdentifier; }
			set { embeddedIdentifier = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Type"/> of the Persister.
		/// </summary>
		/// <value>The <see cref="System.Type"/> of the Persister.</value>
		public override System.Type ClassPersisterClass
		{
			get { return classPersisterClass; }
			set { classPersisterClass = value; }
		}

		/// <summary>
		/// Gets the <see cref="Table"/> of the class
		/// that is mapped in the <c>class</c> element.
		/// </summary>
		/// <value>
		/// The <see cref="Table"/> of the class this mapped class.
		/// </value>
		public override Table RootTable
		{
			get { return Table; }
		}

		/// <summary>
		/// Gets or sets the <see cref="PersistentClass"/> that this mapped class is extending.
		/// </summary>
		/// <value>
		/// <see langword="null" /> since this is the root class.
		/// </value>
		/// <exception cref="InvalidOperationException">
		/// Thrown when the setter is called.  The Superclass can not be set on the 
		/// RootClass, only the Subclass can have a Superclass set.
		/// </exception>
		public override PersistentClass Superclass
		{
			get { return null; }
			set { throw new InvalidOperationException("Can not set the Superclass on a RootClass."); }
		}

		/// <summary>
		/// Gets or sets the <see cref="SimpleValue"/> that contains information about the Key.
		/// </summary>
		/// <value>The <see cref="SimpleValue"/> that contains information about the Key.</value>
		public override SimpleValue Key
		{
			get { return Identifier; }
			set { throw new InvalidOperationException(); }
		}

		/// <summary>
		/// Gets or sets a boolean indicating if only values in the discriminator column that
		/// are mapped will be included in the sql.
		/// </summary>
		/// <value><see langword="true" /> if the mapped discriminator values should be forced.</value>
		public override bool IsForceDiscriminator
		{
			get { return forceDiscriminator; }
			set { this.forceDiscriminator = value; }
		}

		/// <summary>
		/// Gets or sets the sql string that should be a part of the where clause.
		/// </summary>
		/// <value>
		/// The sql string that should be a part of the where clause.
		/// </value>
		public override string Where
		{
			get { return where; }
			set { where = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsJoinedSubclass
		{
			get { return false; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsDiscriminatorInsertable
		{
			get { return discriminatorInsertable; }
			set { discriminatorInsertable = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapping"></param>
		public override void Validate(IMapping mapping)
		{
			base.Validate(mapping);
			if (!Identifier.IsValid(mapping))
			{
				throw new MappingException(
					string.Format("identifier mapping has wrong number of columns: {0} type: {1}", MappedClass.Name,
					              Identifier.Type.Name));
			}
		}

		public override ISet SynchronizedTables
		{
			get { return synchronizedTablesField; }
		}
	}
}