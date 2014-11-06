using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using Iesi.Collections;
using log4net;
using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Impl;
using NHibernate.Mapping;
using NHibernate.Proxy;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.Cfg
{
	/// <summary>
	/// Allows the application to specify properties and mapping documents to be used when creating
	/// a <see cref="ISessionFactory" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Usually an application will create a single <see cref="Configuration" />, build a single instance
	/// of <see cref="ISessionFactory" />, and then instantiate <see cref="ISession"/> objects in threads
	/// servicing client requests.
	/// </para>
	/// <para>
	/// The <see cref="Configuration" /> is meant only as an initialization-time object. <see cref="ISessionFactory" />
	/// is immutable and does not retain any association back to the <see cref="Configuration" />
	/// </para>
	/// </remarks>
	public class Configuration
	{
		#region NHibernate-Specific Members

		private XmlSchemaCollection mappingSchemaCollection;
		private XmlSchemaCollection cfgSchemaCollection;

		/// <summary>
		/// The XML Namespace for the nhibernate-mapping
		/// </summary>
		public const string MappingSchemaXMLNS = "urn:nhibernate-mapping-2.2";

		private const string MappingSchemaResource = "NHibernate.nhibernate-mapping.xsd";

		/// <summary>
		/// The XML Namespace for the nhibernate-configuration
		/// </summary>
		public const string CfgSchemaXMLNS = "urn:nhibernate-configuration-2.2";

		private const string CfgSchemaResource = "NHibernate.nhibernate-configuration.xsd";
		private const string CfgNamespacePrefix = "cfg";

		private string currentDocumentName;

		/// <summary></summary>
		/// <remarks>Allocate on first use as we are expensive in time/space</remarks>
		private XmlSchemaCollection MappingSchemaCollection
		{
			get
			{
				if (mappingSchemaCollection == null)
				{
					mappingSchemaCollection = new XmlSchemaCollection();
					mappingSchemaCollection.Add(
						XmlSchema.Read(Assembly.GetExecutingAssembly().GetManifestResourceStream(MappingSchemaResource), null));
				}
				return mappingSchemaCollection;
			}
			set { mappingSchemaCollection = value; }
		}

		/// <summary></summary>
		/// <remarks>Allocate on first use as we are expensive in time/space</remarks>
		private XmlSchemaCollection CfgSchemaCollection
		{
			get
			{
				if (cfgSchemaCollection == null)
				{
					cfgSchemaCollection = new XmlSchemaCollection();
					cfgSchemaCollection.Add(
						XmlSchema.Read(Assembly.GetExecutingAssembly().GetManifestResourceStream(CfgSchemaResource), null));
				}
				return cfgSchemaCollection;
			}
			set { cfgSchemaCollection = value; }
		}

		#endregion

		private IDictionary classes;
		private Hashtable imports;
		private Hashtable collections;
		private Hashtable tables;
		private Hashtable namedQueries;
		private Hashtable namedSqlQueries;
		private Hashtable sqlResultSetMappings;
		private ArrayList secondPasses;
		private ArrayList propertyReferences;
		private IInterceptor interceptor;
		private IDictionary properties;
		private IDictionary filterDefinitions;
		private IList auxiliaryDatabaseObjects;
		private IDictionary sqlFunctions;

		private INamingStrategy namingStrategy = DefaultNamingStrategy.Instance;

		private MappingsQueue mappingsQueue;

		private static readonly ILog log = LogManager.GetLogger(typeof(Configuration));

		/// <summary>
		/// Clear the internal state of the <see cref="Configuration"/> object.
		/// </summary>
		private void Reset()
		{
			classes = new Hashtable(); //new SequencedHashMap(); - to make NH-369 bug deterministic
			imports = new Hashtable();
			collections = new Hashtable();
			tables = new Hashtable();
			namedQueries = new Hashtable();
			namedSqlQueries = new Hashtable();
			sqlResultSetMappings = new Hashtable();
			secondPasses = new ArrayList();
			propertyReferences = new ArrayList();
			filterDefinitions = new Hashtable();
			interceptor = emptyInterceptor;
			mapping = new Mapping(this);
			properties = Environment.Properties;
			auxiliaryDatabaseObjects = new ArrayList();
			sqlFunctions = new Hashtable();
			mappingsQueue = new MappingsQueue();
		}

		private class Mapping : IMapping
		{
			private Configuration configuration;

			public Mapping(Configuration configuration)
			{
				this.configuration = configuration;
			}

			private PersistentClass GetPersistentClass(System.Type type)
			{
				PersistentClass pc = (PersistentClass) configuration.classes[type];
				if (pc == null)
				{
					throw new MappingException("persistent class not known: " + type.FullName);
				}
				return pc;
			}

			public IType GetIdentifierType(System.Type persistentClass)
			{
				return GetPersistentClass(persistentClass).Identifier.Type;
			}

			public string GetIdentifierPropertyName(System.Type persistentClass)
			{
				PersistentClass pc = GetPersistentClass(persistentClass);
				if (!pc.HasIdentifierProperty)
				{
					return null;
				}
				return pc.IdentifierProperty.Name;
			}

			public IType GetPropertyType(System.Type persistentClass, string propertyName)
			{
				PersistentClass pc = GetPersistentClass(persistentClass);
				NHibernate.Mapping.Property prop = pc.GetProperty(propertyName);

				if (prop == null)
				{
					throw new MappingException("property not known: " + persistentClass.FullName + '.' + propertyName);
				}
				return prop.Type;
			}
		}

		private Mapping mapping;

		/// <summary>
		/// Create a new Configuration object.
		/// </summary>
		public Configuration()
		{
			Reset();
		}

		/// <summary>
		/// The class mappings 
		/// </summary>
		public ICollection ClassMappings
		{
			get { return classes.Values; }
		}

		/// <summary>
		/// The collection mappings
		/// </summary>
		public ICollection CollectionMappings
		{
			get { return collections.Values; }
		}

		/// <summary>
		/// The table mappings
		/// </summary>
		private ICollection TableMappings
		{
			get { return tables.Values; }
		}

		/// <summary>
		/// Get the mapping for a particular class
		/// </summary>
		public PersistentClass GetClassMapping(System.Type persistentClass)
		{
			return (PersistentClass) classes[persistentClass];
		}

		/// <summary>
		/// Get the mapping for a particular collection role
		/// </summary>
		/// <param name="role">a collection role</param>
		/// <returns><see cref="NHibernate.Mapping.Collection" /></returns>
		public NHibernate.Mapping.Collection GetCollectionMapping(string role)
		{
			return (NHibernate.Mapping.Collection) collections[role];
		}

		/// <summary>
		/// Read mappings from a particular XML file. This method is equivalent
		/// to <see cref="AddXmlFile(string)" />.
		/// </summary>
		/// <param name="xmlFile"></param>
		/// <returns></returns>
		public Configuration AddFile(string xmlFile)
		{
			return AddXmlFile(xmlFile);
		}

		public Configuration AddFile(FileInfo xmlFile)
		{
			return AddFile(xmlFile.FullName);
		}

		private static void LogAndThrow(MappingException me)
		{
			log.Error(me.Message, me);
			throw me;
		}

		/// <summary>
		/// Read mappings from a particular XML file.
		/// </summary>
		/// <param name="xmlFile">a path to a file</param>
		/// <returns>This configuration object.</returns>
		public Configuration AddXmlFile(string xmlFile)
		{
			log.Info("Mapping file: " + xmlFile);
			XmlTextReader textReader = null;
			try
			{
				textReader = new XmlTextReader(xmlFile);
				AddXmlReader(textReader, xmlFile);
			}
			catch (MappingException)
			{
				throw;
			}
			catch (Exception e)
			{
				LogAndThrow(new MappingException("Could not configure datastore from file " + xmlFile, e));
			}
			finally
			{
				if (textReader != null)
				{
					textReader.Close();
				}
			}
			return this;
		}

		public Configuration AddXml(string xml)
		{
			return AddXml(xml, "(string)");
		}

		/// <summary>
		/// Read mappings from a <see cref="string" />. This method is equivalent to
		/// <see cref="AddXmlString(string)" />.
		/// </summary>
		/// <param name="xml">an XML string</param>
		/// <param name="name">The name to use in error reporting. May be <see langword="null" />.</param>
		/// <returns>This configuration object.</returns>
		public Configuration AddXml(string xml, string name)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Mapping XML:\n" + xml);
			}
			XmlTextReader reader = null;
			try
			{
				reader = new XmlTextReader(xml, XmlNodeType.Document, null);
				// make a StringReader for the string passed in - the StringReader
				// inherits from TextReader.  We can use the XmlTextReader.ctor that
				// takes the TextReader to build from a string...
				AddXmlReader(reader, name);
			}
			catch (MappingException)
			{
				throw;
			}
			catch (Exception e)
			{
				LogAndThrow(new MappingException("Could not configure datastore from XML string " + name, e));
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}
			return this;
		}

		/// <summary>
		/// Read mappings from a <see cref="string" />.
		/// </summary>
		/// <param name="xml">an XML string</param>
		/// <returns>This configuration object.</returns>
		public Configuration AddXmlString(string xml)
		{
			return AddXml(xml);
		}

		/// <summary>
		/// Read mappings from a URL.
		/// </summary>
		/// <param name="url">a URL</param>
		/// <returns>This configuration object.</returns>
		public Configuration AddUrl(string url)
		{
			// AddFile works for URLs currently
			return AddFile(url);
		}

		/// <summary>
		/// Read mappings from a URL.
		/// </summary>
		/// <param name="url">a <see cref="Uri" /> to read the mappings from.</param>
		/// <returns>This configuration object.</returns>
		public Configuration AddUrl(Uri url)
		{
			return AddUrl(url.AbsolutePath);
		}

		public Configuration AddDocument(XmlDocument doc)
		{
			return AddDocument(doc, "(XmlDocument)");
		}

		/// <summary>
		/// Read mappings from an <see cref="XmlDocument" />.
		/// </summary>
		/// <param name="doc">A loaded <see cref="XmlDocument" /> that contains the mappings.</param>
		/// <param name="name">The name of the document, for error reporting purposes.</param>
		/// <returns>This configuration object.</returns>
		public Configuration AddDocument(XmlDocument doc, string name)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Mapping XML:\n" + doc.OuterXml);
			}

			try
			{
				using (MemoryStream ms = new MemoryStream())
				{
					doc.Save(ms);
					ms.Position = 0;
					AddInputStream(ms, name);
				}
				return this;
			}
			catch (MappingException)
			{
				throw;
			}
			catch (Exception e)
			{
				LogAndThrow(new MappingException("Could not configure datastore from XML document " + name, e));
				return this; // To please the compiler
			}
		}

		/// <summary>
		/// Takes the validated XmlDocument and has the Binder do its work of
		/// creating Mapping objects from the Mapping Xml.
		/// </summary>
		/// <param name="doc">The NamedXmlDocument that contains the <b>validated</b> mapping XML file.</param>
		private void AddValidatedDocument(NamedXmlDocument doc)
		{
			try
			{
				HbmBinder.dialect = Dialect.Dialect.GetDialect(properties);
				HbmBinder.BindRoot(doc.Document, CreateMappings());
			}
			catch (Exception e)
			{
				string nameFormatted = doc.Name == null ? "(unknown)" : doc.Name;
				LogAndThrow(new MappingException("Could not compile the mapping document: " + nameFormatted, e));
			}
		}

		/// <summary>
		/// Create a new <see cref="Mappings" /> to add classes and collection
		/// mappings to.
		/// </summary>
		public Mappings CreateMappings()
		{
			return new Mappings(
				classes,
				collections,
				tables,
				namedQueries,
				namedSqlQueries,
				sqlResultSetMappings,
				imports,
				secondPasses,
				propertyReferences,
				namingStrategy,
				filterDefinitions,
				auxiliaryDatabaseObjects,
				defaultAssembly,
				defaultNamespace
				);
		}

		/// <summary>
		/// Read mappings from a <see cref="Stream" />.
		/// </summary>
		/// <param name="xmlInputStream">The stream containing XML</param>
		/// <returns>This Configuration object.</returns>
		/// <remarks>
		/// The <see cref="Stream"/> passed in through the parameter <paramref name="xmlInputStream" />
		/// is not <em>guaranteed</em> to be cleaned up by this method.  It is the caller's responsiblity to
		/// ensure that <paramref name="xmlInputStream" /> is properly handled when this method
		/// completes.
		/// </remarks>
		public Configuration AddInputStream(Stream xmlInputStream)
		{
			return AddInputStream(xmlInputStream, null);
		}

		/// <summary>
		/// Read mappings from a <see cref="Stream" />.
		/// </summary>
		/// <param name="xmlInputStream">The stream containing XML</param>
		/// <param name="name">The name of the stream to use in error reporting. May be <see langword="null" />.</param>
		/// <returns>This Configuration object.</returns>
		/// <remarks>
		/// The <see cref="Stream"/> passed in through the parameter <paramref name="xmlInputStream" />
		/// is not <em>guaranteed</em> to be cleaned up by this method.  It is the caller's responsiblity to
		/// ensure that <paramref name="xmlInputStream" /> is properly handled when this method
		/// completes.
		/// </remarks>
		public Configuration AddInputStream(Stream xmlInputStream, string name)
		{
			XmlTextReader textReader = null;
			try
			{
				textReader = new XmlTextReader(xmlInputStream);
				AddXmlReader(textReader, name);
				return this;
			}
			catch (MappingException)
			{
				throw;
			}
			catch (Exception e)
			{
				LogAndThrow(new MappingException("Could not configure datastore from input stream " + name, e));
				return this; // To please the compiler
			}
			finally
			{
				if (textReader != null)
				{
					textReader.Close();
				}
			}
		}

		/// <summary>
		/// Adds the mappings in the resource of the assembly.
		/// </summary>
		/// <param name="path">The path to the resource file in the assembly.</param>
		/// <param name="assembly">The assembly that contains the resource file.</param>
		/// <returns>This configuration object.</returns>
		public Configuration AddResource(string path, Assembly assembly)
		{
			string debugName = path;
			log.Info("Mapping resource: " + debugName);
			Stream rsrc = assembly.GetManifestResourceStream(path);
			if (rsrc == null)
			{
				LogAndThrow(new MappingException("Resource not found: " + debugName));
			}

			try
			{
				return AddInputStream(rsrc, debugName);
			}
			catch (MappingException)
			{
				throw;
			}
			catch (Exception e)
			{
				LogAndThrow(new MappingException("Could not configure datastore from resource " + debugName, e));
				return this; // To please the compiler
			}
			finally
			{
				rsrc.Close();
			}
		}

		// Not ported - addResource(String path) - not applicable

		/// <summary>
		/// Read a mapping from an embedded resource, using a convention.
		/// </summary>
		/// <param name="persistentClass">The type to map.</param>
		/// <returns>This configuration object.</returns>
		/// <remarks>
		/// The convention is for class <c>Foo.Bar.Foo</c> to be mapped by
		/// the resource named <c>Foo.Bar.Foo.hbm.xml</c>, embedded in
		/// the class' assembly. If the mappings and classes are defined
		/// in different assemblies or don't follow the naming convention,
		/// this method cannot be used.
		/// </remarks>
		public Configuration AddClass(System.Type persistentClass)
		{
			return AddResource(persistentClass.FullName + ".hbm.xml", persistentClass.Assembly);
		}

		/// <summary>
		/// Adds all of the assembly's embedded resources whose names end with <c>.hbm.xml</c>.
		/// </summary>
		/// <param name="assemblyName">The name of the assembly to load.</param>
		/// <returns>This configuration object.</returns>
		/// <remarks>
		/// The assembly must be loadable using <see cref="Assembly.Load(string)" />. If this
		/// condition is not satisfied, load the assembly manually and call
		/// <see cref="AddAssembly(Assembly)"/> instead.
		/// </remarks>
		public Configuration AddAssembly(string assemblyName)
		{
			log.Info("Searching for mapped documents in assembly: " + assemblyName);

			Assembly assembly = null;
			try
			{
				assembly = Assembly.Load(assemblyName);
			}
			catch (Exception e)
			{
				LogAndThrow(new MappingException("Could not add assembly " + assemblyName, e));
			}

			return AddAssembly(assembly);
		}

		/// <summary>
		/// Adds all of the assembly's embedded resources whose names end with <c>.hbm.xml</c>.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <returns>This configuration object.</returns>
		public Configuration AddAssembly(Assembly assembly)
		{
			IList resourceNames = GetAllHbmXmlResourceNames(assembly);

			foreach (string name in resourceNames)
			{
				AddResource(name, assembly);
			}
			return this;
		}

		private static StringCollection GetAllHbmXmlResourceNames(Assembly assembly)
		{
			StringCollection result = new StringCollection();

			foreach (string resource in assembly.GetManifestResourceNames())
			{
				if (resource.EndsWith(".hbm.xml"))
				{
					result.Add(resource);
				}
			}

			return result;
		}

		[Obsolete("Use AddAssembly(Assembly), since version 1.2.1 resource ordering cannot be skipped.")]
		public Configuration AddAssembly(Assembly assembly, bool skipOrdering)
		{
			return AddAssembly(assembly);
		}

		/// <summary>
		/// Read all mapping documents from a directory tree. Assume that any
		/// file named <c>*.hbm.xml</c> is a mapping document.
		/// </summary>
		/// <param name="dir">a directory</param>
		public Configuration AddDirectory(DirectoryInfo dir)
		{
			foreach (DirectoryInfo subDirectory in dir.GetDirectories())
			{
				AddDirectory(subDirectory);
			}

			foreach (FileInfo hbmXml in dir.GetFiles("*.hbm.xml"))
			{
				AddFile(hbmXml);
			}

			return this;
		}

		private ICollection CollectionGenerators(Dialect.Dialect dialect)
		{
			Hashtable generators = new Hashtable();
			foreach (PersistentClass clazz in classes.Values)
			{
				IIdentifierGenerator ig = clazz.Identifier.CreateIdentifierGenerator(dialect);
				if (ig is IPersistentIdentifierGenerator)
				{
					generators[((IPersistentIdentifierGenerator) ig).GeneratorKey()] = ig;
				}
			}

			foreach (NHibernate.Mapping.Collection collection in collections.Values)
			{
				if (collection is IdentifierCollection)
				{
					IIdentifierGenerator ig = ((IdentifierCollection) collection)
						.Identifier
						.CreateIdentifierGenerator(dialect);

					if (ig is IPersistentIdentifierGenerator)
					{
						generators[((IPersistentIdentifierGenerator) ig).GeneratorKey()] = ig;
					}
				}
			}
			return generators.Values;
		}

		/// <summary>
		/// Generate DDL for droping tables
		/// </summary>
		/// <seealso cref="NHibernate.Tool.hbm2ddl.SchemaExport" />
		public string[] GenerateDropSchemaScript(Dialect.Dialect dialect)
		{
			SecondPassCompile();

			string defaultSchema = PropertiesHelper.GetString(Environment.DefaultSchema, properties, null);

			ArrayList script = new ArrayList(50);

			// drop them in reverse order in case db needs it done that way...
			for (int i = auxiliaryDatabaseObjects.Count - 1; i >= 0; i--)
			{
				IAuxiliaryDatabaseObject auxDbObj = (IAuxiliaryDatabaseObject) auxiliaryDatabaseObjects[i];
				if (auxDbObj.AppliesToDialect(dialect))
				{
					script.Add(auxDbObj.SqlDropString(dialect, defaultSchema));
				}
			}

			if (dialect.DropConstraints)
			{
				foreach (Table table in TableMappings)
				{
					foreach (ForeignKey fk in table.ForeignKeyCollection)
					{
						script.Add(fk.SqlDropString(dialect, defaultSchema));
					}
				}
			}

			foreach (Table table in TableMappings)
			{
				script.Add(table.SqlDropString(dialect, defaultSchema));
			}

			foreach (IPersistentIdentifierGenerator idGen in CollectionGenerators(dialect))
			{
				string dropString = idGen.SqlDropString(dialect);
				if (dropString != null)
				{
					script.Add(dropString);
				}
			}

			return ArrayHelper.ToStringArray(script);
		}

		/// <summary>
		/// Generate DDL for creating tables
		/// </summary>
		/// <param name="dialect"></param>
		public string[] GenerateSchemaCreationScript(Dialect.Dialect dialect)
		{
			SecondPassCompile();

			string defaultSchema = PropertiesHelper.GetString(Environment.DefaultSchema, properties, null);

			ArrayList script = new ArrayList(50);

			foreach (Table table in TableMappings)
			{
				script.Add(table.SqlCreateString(dialect, mapping, defaultSchema));
			}

			foreach (Table table in TableMappings)
			{
				/*
				TODO: H2.1.8 has the code below, but only TimesTen dialect ever
				enters the if, so I don't want to add this to Dialect right now

				if( !dialect.SupportsUniqueConstraintInCreateAlterTable )
				{
					foreach( UniqueKey uk in table.UniqueKeyCollection )
					{
						script.Add( uk.SqlCreateString( dialect, mapping, defaultSchema ) );
					}
				}
				*/

				foreach (Index index in table.IndexCollection)
				{
					script.Add(index.SqlCreateString(dialect, mapping, defaultSchema));
				}

				if (dialect.HasAlterTable)
				{
					foreach (ForeignKey fk in table.ForeignKeyCollection)
					{
						script.Add(fk.SqlCreateString(dialect, mapping, defaultSchema));
					}
				}
			}

			foreach (IPersistentIdentifierGenerator idGen in CollectionGenerators(dialect))
			{
				string[] lines = idGen.SqlCreateStrings(dialect);
				script.AddRange(lines);
			}

			foreach (IAuxiliaryDatabaseObject auxDbObj in auxiliaryDatabaseObjects)
			{
				if (auxDbObj.AppliesToDialect(dialect))
				{
					script.Add(auxDbObj.SqlCreateString(dialect, mapping, defaultSchema));
				}
			}

			return ArrayHelper.ToStringArray(script);
		}

		/*
		///<summary>
		/// Generate DDL for altering tables
		///</summary>
		public string[] GenerateSchemaUpdateScript(Dialect.Dialect dialect, DatabaseMetadata databaseMetadata) 
		{
			secondPassCompile();
				
			ArrayList script = new ArrayList(50);
		
			foreach(Table table in TableMappings)
			{
				TableMetadata tableInfo = databaseMetadata.GetTableMetadata( table.Name, table.Schema, null );
				if (tableInfo == null) 
				{
					script.Add( table.SqlCreateString( dialect, this ) );
				}
				else 
				{
					foreach(string alterString in table.SqlAlterStrings(dialect, this, tableInfo))
					{
						script.Add( alterString );
					}
				}
			}
				
			foreach(Table table in TableMappings)
			{
				TableMetadata tableInfo = databaseMetadata.GetTableMetadata( table.Name, table.Schema, null );
					
				if ( dialect.HasAlterTable)
				{
					foreach(ForeignKey fk in table.ForeignKeyCollection)
					{
						bool create = tableInfo == null || ( tableInfo.getForeignKeyMetadata( fk.Name ) == null && (
							// Icky workaround for MySQL bug:
							!( dialect is NHibernate.Dialect.MySQLDialect ) || table.GetIndex( fk.Name ) == null )
						);
						if ( create ) 
						{
							script.Add( fk.SqlCreateString(dialect, mapping) );
						}
					}
				}

				// Broken 'cos we don't generate these with names in SchemaExport
				foreach(Index index in table.IndexCollection)
				{
					if ( tableInfo == null || tableInfo.GetIndexMetadata( index.Name ) == null ) 
					{
						script.Add( index.SqlCreateString( dialect, mapping ) );
					}
				}

				// Broken 'cos we don't generate these with names in SchemaExport
				foreach(UniqueKey uk in table.UniqueKeyCollection)
				{
					if ( tableInfo == null || tableInfo.GetIndexMetadata( uk.Name ) == null ) 
					{
						script.Add( uk.SqlCreateString( dialect, mapping ) );
					}
				}
			}
		
			foreach(IPersistentIdentifierGenerator generator in CollectionGenerators(dialect))
			{
				object key = generator.GeneratorKey();
				if ( !databaseMetadata.IsSequence( key ) && !databaseMetadata.IsTable(key) ) 
				{
					string[] lines = generator.SqlCreateStrings( dialect );
					for (int i = 0; i < lines.Length; i++) script.Add( lines[i] );
				}
			}
				
			return ArrayHelper.ToStringArray(script);
		}
		*/

		private void Validate()
		{
			bool validateProxy = PropertiesHelper.GetBoolean(Environment.UseProxyValidator, properties, true);
			HashedSet allProxyErrors = null;

			foreach (PersistentClass clazz in classes.Values)
			{
				clazz.Validate(mapping);
				if (validateProxy)
				{
					ICollection errors = ValidateProxyInterface(clazz);
					if (errors != null)
					{
						if (allProxyErrors == null)
						{
							allProxyErrors = new HashedSet(errors);
						}
						else
						{
							allProxyErrors.AddAll(errors);
						}
					}
				}
			}

			if (allProxyErrors != null)
			{
				throw new InvalidProxyTypeException(allProxyErrors);
			}

			foreach (NHibernate.Mapping.Collection col in collections.Values)
			{
				col.Validate(mapping);
			}
		}

		private static ICollection ValidateProxyInterface(PersistentClass persistentClass)
		{
			if (!persistentClass.IsLazy)
			{
				// Nothing to validate
				return null;
			}

			if (persistentClass.ProxyInterface == null)
			{
				// Nothing to validate
				return null;
			}

			return ProxyTypeValidator.ValidateType(persistentClass.ProxyInterface);
		}

		/// <remarks>
		/// This method may be called many times!!
		/// </remarks>
		private void SecondPassCompile()
		{
			log.Info("checking mappings queue");

			mappingsQueue.CheckNoUnavailableEntries();

			log.Info("processing one-to-many association mappings");

			foreach (ISecondPass sp in secondPasses)
			{
				sp.DoSecondPass(classes);
			}

			secondPasses.Clear();

			log.Info("processing one-to-one association property references");

			foreach (Mappings.UniquePropertyReference upr in propertyReferences)
			{
				PersistentClass clazz = GetClassMapping(upr.ReferencedClass);
				if (clazz == null)
				{
					throw new MappingException("property-ref to unmapped class: " + upr.ReferencedClass);
				}

				NHibernate.Mapping.Property prop = clazz.GetReferencedProperty(upr.PropertyName);
				((SimpleValue) prop.Value).IsUnique = true;
			}

			//TODO: Somehow add the newly created foreign keys to the internal collection

			log.Info("processing foreign key constraints");

			ISet done = new HashedSet();
			foreach (Table table in TableMappings)
			{
				SecondPassCompileForeignKeys(table, done);
			}
		}

		private void SecondPassCompileForeignKeys(Table table, ISet done)
		{
			foreach (ForeignKey fk in table.ForeignKeyCollection)
			{
				if (!done.Contains(fk))
				{
					done.Add(fk);
					if (log.IsDebugEnabled)
					{
						log.Debug("resolving reference to class: " + fk.ReferencedClass.Name);
					}
					PersistentClass referencedClass = (PersistentClass) classes[fk.ReferencedClass];
					if (referencedClass == null)
					{
						string messageTemplate = "An association from the table {0} refers to an unmapped class: {1}";
						string message = string.Format(messageTemplate, fk.Table.Name, fk.ReferencedClass.Name);

						LogAndThrow(new MappingException(message));
					}

					if (referencedClass.IsJoinedSubclass)
					{
						SecondPassCompileForeignKeys(referencedClass.Superclass.Table, done);
					}

					try
					{
						fk.ReferencedTable = referencedClass.Table;
					}
					catch (MappingException me)
					{
						if (log.IsErrorEnabled)
						{
							log.Error(me);
						}

						// rethrow the error - only caught it for logging purposes
						throw;
					}
				}
			}
		}

		/// <summary>
		/// The named queries
		/// </summary>
		public IDictionary NamedQueries
		{
			get { return namedQueries; }
		}

		private static readonly IInterceptor emptyInterceptor = new EmptyInterceptor();
		private string defaultAssembly;
		private string defaultNamespace;

		/// <summary>
		/// Instantiate a new <see cref="ISessionFactory" />, using the properties and mappings in this
		/// configuration. The <see cref="ISessionFactory" /> will be immutable, so changes made to the
		/// configuration after building the <see cref="ISessionFactory" /> will not affect it.
		/// </summary>
		/// <returns>An <see cref="ISessionFactory" /> instance.</returns>
		public ISessionFactory BuildSessionFactory()
		{
			SecondPassCompile();
			Validate();
			Environment.VerifyProperties(properties);
			Settings settings = BuildSettings();

			// Ok, don't need schemas anymore, so free them
			MappingSchemaCollection = null;
			CfgSchemaCollection = null;

			return new SessionFactoryImpl(this, mapping, settings);
		}

		/// <summary>
		/// Gets or sets the <see cref="IInterceptor"/> to use.
		/// </summary>
		/// <value>The <see cref="IInterceptor"/> to use.</value>
		public IInterceptor Interceptor
		{
			get { return interceptor; }
			set { this.interceptor = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="IDictionary"/> that contains the configuration
		/// properties and their values.
		/// </summary>
		/// <value>
		/// The <see cref="IDictionary"/> that contains the configuration
		/// properties and their values.
		/// </value>
		public IDictionary Properties
		{
			get { return properties; }
			set { this.properties = value; }
		}

		/// <summary>
		/// Set the default assembly to use for the mappings added to the configuration
		/// afterwards.
		/// </summary>
		/// <param name="defaultAssembly">The default assembly name.</param>
		/// <returns>This configuration instance.</returns>
		/// <remarks>
		/// This setting can be overridden for a mapping file by setting <c>default-assembly</c>
		/// attribute of <c>&lt;hibernate-mapping&gt;</c> element.
		/// </remarks>
		public Configuration SetDefaultAssembly(string defaultAssembly)
		{
			this.defaultAssembly = defaultAssembly;
			return this;
		}

		/// <summary>
		/// Set the default namespace to use for the mappings added to the configuration
		/// afterwards.
		/// </summary>
		/// <param name="defaultNamespace">The default namespace.</param>
		/// <returns>This configuration instance.</returns>
		/// <remarks>
		/// This setting can be overridden for a mapping file by setting <c>default-namespace</c>
		/// attribute of <c>&lt;hibernate-mapping&gt;</c> element.
		/// </remarks>
		public Configuration SetDefaultNamespace(string defaultNamespace)
		{
			this.defaultNamespace = defaultNamespace;
			return this;
		}

		/// <summary>
		/// Sets the default interceptor for use by all sessions.
		/// </summary>
		/// <param name="interceptor">The default interceptor.</param>
		/// <returns>This configuration instance.</returns>
		public Configuration SetInterceptor(IInterceptor interceptor)
		{
			this.interceptor = interceptor;
			return this;
		}

		/// <summary>
		/// Specify a completely new set of properties
		/// </summary>
		public Configuration SetProperties(IDictionary properties)
		{
			this.properties = properties;
			return this;
		}

		/// <summary>
		/// Adds an <see cref="IDictionary"/> of configuration properties.  The 
		/// Key is the name of the Property and the Value is the <see cref="String"/>
		/// value of the Property.
		/// </summary>
		/// <param name="properties">An <see cref="IDictionary"/> of configuration properties.</param>
		/// <returns>
		/// This <see cref="Configuration"/> object.
		/// </returns>
		public Configuration AddProperties(IDictionary properties)
		{
			foreach (DictionaryEntry de in properties)
			{
				this.properties.Add(de.Key, de.Value);
			}
			return this;
		}

		/// <summary>
		/// Sets the value of the configuration property.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="value">The value of the property.</param>
		/// <returns>
		/// This configuration object.
		/// </returns>
		public Configuration SetProperty(string name, string value)
		{
			properties[name] = value;
			return this;
		}

		/// <summary>
		/// Gets the value of the configuration property.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <returns>The configured value of the property, or <see langword="null" /> if the property was not specified.</returns>
		public string GetProperty(string name)
		{
			return properties[name] as string;
		}

		private void AddProperties(XmlNode parent, XmlNamespaceManager cfgNamespaceMgr)
		{
			foreach (XmlNode node in parent.SelectNodes(CfgNamespacePrefix + ":property", cfgNamespaceMgr))
			{
				string name = node.Attributes["name"].Value;
				string value = node.InnerText;
				if (log.IsDebugEnabled)
				{
					log.Debug(name + "=" + value);
				}
				properties[name] = value;
				if (!name.StartsWith("hibernate"))
				{
					properties["hibernate." + name] = value;
				}
			}
			Environment.VerifyProperties(properties);
		}

		// TODO - getConfigurationInputStream(String resource)

		/// <summary>
		/// Configure NHibernate using the <c>&lt;hibernate-configuration&gt;</c> section
		/// from the application config file, if found, or the file <c>hibernate.cfg.xml</c>
		/// otherwise.
		/// </summary>
		/// <returns>A configuration object initialized with the file.</returns>
		/// <remarks>
		/// To configure NHibernate explicitly using <c>hibernate.cfg.xml</c>, ignoring
		/// the application configuration file, use this code:
		/// <code>
		///		configuration.Configure("path/to/hibernate.cfg.xml");
		/// </code>
		/// </remarks>
		public Configuration Configure()
		{
			XmlNode configNode = GetAppConfigConfigurationNode();

			if (configNode != null)
			{
				return Configure(configNode);
			}
			else
			{
				return Configure(GetDefaultConfigurationFilePath());
			}
		}

		/// <summary>
		/// Configure NHibernate from an <see cref="XmlNode" /> representing the root
		/// <c>&lt;hibernate-configuration&gt;</c> element.
		/// </summary>
		/// <param name="node">Configuration node</param>
		/// <returns>This Configuration object</returns>
		private Configuration Configure(XmlNode node)
		{
			XmlTextReader reader = new XmlTextReader(node.OuterXml, XmlNodeType.Document, null);
			try
			{
				Configure(reader);
				return this;
			}
			finally
			{
				reader.Close();
			}
		}

		/// <summary>
		/// Configure NHibernate using the file specified.
		/// </summary>
		/// <param name="fileName">The location of the XML file to use to configure NHibernate.</param>
		/// <returns>A Configuration object initialized with the file.</returns>
		/// <remarks>
		/// Calling Configure(string) will overwrite the values set in app.config or web.config
		/// </remarks>
		public Configuration Configure(string fileName)
		{
			XmlTextReader reader = null;
			try
			{
				reader = new XmlTextReader(fileName);
				return Configure(reader);
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}
		}

		/// <summary>
		/// Configure NHibernate using a resource contained in an Assembly.
		/// </summary>
		/// <param name="assembly">The <see cref="Assembly"/> that contains the resource.</param>
		/// <param name="resourceName">The name of the manifest resource being requested.</param>
		/// <returns>A Configuration object initialized from the manifest resource.</returns>
		/// <remarks>
		/// Calling Configure(Assembly, string) will overwrite the values set in app.config or web.config
		/// </remarks>
		public Configuration Configure(Assembly assembly, string resourceName)
		{
			if (assembly == null || resourceName == null)
			{
				throw new HibernateException("Could not configure NHibernate.",
				                             new ArgumentException("A null value was passed in.", "assembly or resourceName"));
			}

			Stream stream = null;
			try
			{
				stream = assembly.GetManifestResourceStream(resourceName);
				if (stream == null)
				{
					// resource does not exist - throw appropriate exception 
					throw new HibernateException("A ManifestResourceStream could not be created for the resource " + resourceName +
					                             " in Assembly " + assembly.FullName);
				}

				return Configure(new XmlTextReader(stream));
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
		}

		/// <summary>
		/// Configure NHibernate using the specified XmlTextReader.
		/// </summary>
		/// <param name="reader">The <see cref="XmlTextReader"/> that contains the Xml to configure NHibernate.</param>
		/// <returns>A Configuration object initialized with the file.</returns>
		/// <remarks>
		/// Calling Configure(XmlTextReader) will overwrite the values set in app.config or web.config
		/// </remarks>
		public Configuration Configure(XmlTextReader reader)
		{
			if (reader == null)
			{
				throw new HibernateException("Could not configure NHibernate.",
				                             new ArgumentException("A null value was passed in.", "reader"));
			}

			XmlDocument doc = new XmlDocument();
			XmlValidatingReader validatingReader = null;

			try
			{
				validatingReader = new XmlValidatingReader(reader);
				validatingReader.ValidationType = ValidationType.Schema;
				validatingReader.Schemas.Add(CfgSchemaCollection);

				doc.Load(validatingReader);
			}
			catch (Exception e)
			{
				log.Error("Problem parsing configuration", e);
				throw new HibernateException("problem parsing configuration : " + e);
			}
			finally
			{
				if (validatingReader != null)
				{
					validatingReader.Close();
				}
			}

			return DoConfigure(doc);
		}

		// Not ported - configure(org.w3c.dom.Document)

		protected Configuration DoConfigure(XmlDocument doc)
		{
			XmlNamespaceManager cfgNamespaceMgr = CreateXmlNamespaceManager(doc);

			XmlNode sfNode =
				doc.DocumentElement.SelectSingleNode("//" + CfgNamespacePrefix + ":session-factory", cfgNamespaceMgr);

			if (sfNode == null)
			{
				throw new MappingException("<session-factory xmlns='" + CfgSchemaXMLNS +
				                           "'> element was not found in the configuration file.");
			}

			XmlAttribute nameNode = sfNode.Attributes["name"];
			string name = nameNode == null ? null : nameNode.Value;

			if (name != null)
			{
				properties[Environment.SessionFactoryName] = name;
			}
			AddProperties(sfNode, cfgNamespaceMgr);

			foreach (XmlNode mapElement in sfNode.ChildNodes)
			{
				string elemname = mapElement.LocalName;
				if ("mapping".Equals(elemname))
				{
					XmlAttribute rsrc = mapElement.Attributes["resource"];
					XmlAttribute file = mapElement.Attributes["file"];
					XmlAttribute assembly = mapElement.Attributes["assembly"];
					if (rsrc != null)
					{
						log.Debug(name + "<-" + rsrc.Value + " in " + assembly.Value);
						AddResource(rsrc.Value, Assembly.Load(assembly.Value));
					}
					else if (assembly != null)
					{
						log.Debug(name + "<-" + assembly.Value);
						AddAssembly(assembly.Value);
					}
					else if (file != null)
					{
						log.Debug(name + "<-" + file.Value);
						AddFile(file.Value);
					}
					else
					{
						throw new MappingException("<mapping> element in configuration specifies no attributes");
					}
				}
				else if ("jcs-class-cache".Equals(elemname) || "class-cache".Equals(elemname))
				{
					string className = mapElement.Attributes["class"].Value;
					System.Type clazz;
					try
					{
						clazz = ReflectHelper.ClassForName(className);
					}
					catch (TypeLoadException tle)
					{
						throw new MappingException("Could not find class: " + className, tle);
					}

					XmlAttribute regionNode = mapElement.Attributes["region"];
					string region = (regionNode == null) ? className : regionNode.Value;
					SetCacheConcurrencyStrategy(clazz, mapElement.Attributes["usage"].Value, region);
				}
				else if ("jcs-collection-cache".Equals(elemname) || "collection-cache".Equals(elemname))
				{
					String role = mapElement.Attributes["collection"].Value;
					NHibernate.Mapping.Collection collection = GetCollectionMapping(role);
					if (collection == null)
					{
						throw new MappingException("Cannot configure cache for unknown collection role " + role);
					}

					XmlAttribute regionNode = mapElement.Attributes["region"];
					string region = (regionNode == null) ? role : regionNode.Value;
					SetCacheConcurrencyStrategy(role, mapElement.Attributes["usage"].Value, region);
				}
			}

			if (name != null)
			{
				log.Info("Configured SessionFactory: " + name);
			}
			log.Debug("properties: " + properties);

			return this;
		}

		internal RootClass GetRootClassMapping(System.Type clazz)
		{
			PersistentClass persistentClass = GetClassMapping(clazz);

			if (persistentClass == null)
			{
				throw new MappingException("Cache specified for unmapped class " + clazz);
			}

			RootClass rootClass = persistentClass as RootClass;

			if (rootClass == null)
			{
				throw new MappingException(
					"You may only specify a cache for root <class> mappings "
					+ "(cache was specified for " + clazz + ")");
			}

			return rootClass;
		}

		/// <summary>
		/// Set up a cache for an entity class
		/// </summary>
		public Configuration SetCacheConcurrencyStrategy(System.Type clazz, string concurrencyStrategy)
		{
			SetCacheConcurrencyStrategy(clazz, concurrencyStrategy, clazz.FullName);
			return this;
		}

		internal void SetCacheConcurrencyStrategy(System.Type clazz, string concurrencyStrategy, string region)
		{
			RootClass rootClass = GetRootClassMapping(clazz);
			rootClass.CacheConcurrencyStrategy = concurrencyStrategy;
			rootClass.CacheRegionName = region;
		}

		/// <summary>
		/// Set up a cache for a collection role
		/// </summary>
		public Configuration SetCacheConcurrencyStrategy(string collectionRole, string concurrencyStrategy)
		{
			SetCacheConcurrencyStrategy(collectionRole, concurrencyStrategy, collectionRole);
			return this;
		}

		internal void SetCacheConcurrencyStrategy(string collectionRole, string concurrencyStrategy, string region)
		{
			NHibernate.Mapping.Collection collection = GetCollectionMapping(collectionRole);
			collection.CacheConcurrencyStrategy = concurrencyStrategy;
			collection.CacheRegionName = region;
		}

		/// <summary>
		/// Get the query language imports
		/// </summary>
		/// <returns></returns>
		public IDictionary Imports
		{
			get { return imports; }
		}

		/// <summary>
		/// Create an object-oriented view of the configuration properties
		/// </summary>
		/// <returns>A <see cref="Settings"/> object initialized from the settings properties.</returns>
		//protected Settings BuildSettings()
		private Settings BuildSettings()
		{
			return SettingsFactory.BuildSettings(properties);
		}

		/// <summary>
		/// The named SQL queries
		/// </summary>
		public IDictionary NamedSQLQueries
		{
			get { return namedSqlQueries; }
		}

		/// <summary>
		/// Naming strategy for tables and columns
		/// </summary>
		public INamingStrategy NamingStrategy
		{
			get { return namingStrategy; }
		}

		/// <summary>
		/// Set a custom naming strategy
		/// </summary>
		/// <param name="namingStrategy">the NamingStrategy to set</param>
		/// <returns></returns>
		public Configuration SetNamingStrategy(INamingStrategy namingStrategy)
		{
			this.namingStrategy = namingStrategy;
			return this;
		}

		public IDictionary SqlResultSetMappings
		{
			get { return sqlResultSetMappings; }
		}

		public IDictionary FilterDefinitions
		{
			get { return filterDefinitions; }
		}

		public void AddFilterDefinition(FilterDefinition definition)
		{
			filterDefinitions.Add(definition.FilterName, definition);
		}

		public void AddAuxiliaryDatabaseObject(IAuxiliaryDatabaseObject obj)
		{
			auxiliaryDatabaseObjects.Add(obj);
		}

		public IDictionary SqlFunctions
		{
			get { return sqlFunctions; }
		}

		public void AddSqlFunction(string functionName, ISQLFunction sqlFunction)
		{
			sqlFunctions[functionName] = sqlFunction;
		}

		#region NHibernate-Specific Members

		/// <summary>
		/// Load and validate the mappings in the <see cref="XmlTextReader" /> against
		/// the nhibernate-mapping-2.2 schema, without adding them to the configuration.
		/// </summary>
		/// <remarks>
		/// This method is made public to be usable from the unit tests. It is not intended
		/// to be called by end users.
		/// </remarks>
		/// <param name="hbmReader">The XmlReader that contains the mapping.</param>
		/// <param name="name">The name of the document, for error reporting purposes.</param>
		/// <returns>NamedXmlDocument containing the validated XmlDocument built from the XmlReader.</returns>
		public NamedXmlDocument LoadMappingDocument(XmlTextReader hbmReader, string name)
		{
			XmlValidatingReader validatingReader = new XmlValidatingReader(hbmReader);

			Debug.Assert(currentDocumentName == null);
			currentDocumentName = name;

			try
			{
				XmlDocument hbmDocument = new XmlDocument();

				validatingReader.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
				validatingReader.ValidationType = ValidationType.Schema;
				validatingReader.Schemas.Add(MappingSchemaCollection);

				hbmDocument.Load(validatingReader);
				return new NamedXmlDocument(name, hbmDocument);
			}
			finally
			{
				currentDocumentName = null;
				validatingReader.Close();
			}
		}

		/// <summary>
		/// Adds the Mappings in the <see cref="XmlTextReader"/> after validating it
		/// against the nhibernate-mapping-2.2 schema.
		/// </summary>
		/// <param name="hbmReader">The XmlTextReader that contains the mapping.</param>
		/// <returns>This Configuration object.</returns>
		public Configuration AddXmlReader(XmlTextReader hbmReader)
		{
			return AddXmlReader(hbmReader, null);
		}

		/// <summary>
		/// Adds the Mappings in the <see cref="XmlTextReader"/> after validating it
		/// against the nhibernate-mapping-2.2 schema.
		/// </summary>
		/// <param name="hbmReader">The XmlTextReader that contains the mapping.</param>
		/// <param name="name">The name of the document to use for error reporting. May be <see langword="null" />.</param>
		/// <returns>This Configuration object.</returns>
		public Configuration AddXmlReader(XmlTextReader hbmReader, string name)
		{
			NamedXmlDocument document = LoadMappingDocument(hbmReader, name);
			AddDocumentThroughQueue(document);
			return this;
		}

		private void AddDocumentThroughQueue(NamedXmlDocument document)
		{
			mappingsQueue.AddDocument(document);
			ProcessMappingsQueue();
		}

		private void ProcessMappingsQueue()
		{
			NamedXmlDocument document;

			while ((document = mappingsQueue.GetNextAvailableResource()) != null)
			{
				AddValidatedDocument(document);
			}
		}

		private void ValidationHandler(object o, ValidationEventArgs args)
		{
			string message =
				string.Format(
					"{0}({1},{2}): XML validation error: {3}",
					currentDocumentName,
					args.Exception.LineNumber,
					args.Exception.LinePosition,
					args.Exception.Message);
			LogAndThrow(new MappingException(message, args.Exception));
		}

		protected XmlNamespaceManager CreateXmlNamespaceManager(XmlDocument doc)
		{
			XmlNamespaceManager cfgNamespaceMgr = new XmlNamespaceManager(doc.NameTable);
			// note that the prefix has absolutely nothing to do with what the user
			// selects as their prefix in the document.  It is the prefix we use to 
			// build the XPath and the nsmgr takes care of translating our prefix into
			// the user defined prefix...
			cfgNamespaceMgr.AddNamespace(CfgNamespacePrefix, CfgSchemaXMLNS);
			return cfgNamespaceMgr;
		}

		private XmlNode GetAppConfigConfigurationNode()
		{
			XmlNode node = ConfigurationSettings.GetConfig("hibernate-configuration") as XmlNode;
			return node;
		}

		private string GetDefaultConfigurationFilePath()
		{
			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
			string binPath = relativeSearchPath == null ? baseDir : Path.Combine(baseDir, relativeSearchPath);
			return Path.Combine(binPath, "hibernate.cfg.xml");
		}

		#endregion
	}
}