using System;
using System.Collections;
using System.Data;
using log4net;
using NHibernate.Cache;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Hql;
using NHibernate.Transaction;
using NHibernate.Util;

namespace NHibernate.Cfg
{
	/// <summary>
	/// Reads configuration properties and configures a <see cref="Settings"/> instance. 
	/// </summary>
	public sealed class SettingsFactory
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(SettingsFactory));
		private static readonly string DefaultCacheProvider = typeof(NoCacheProvider).AssemblyQualifiedName;

		public static Settings BuildSettings(IDictionary properties)
		{
			Settings settings = new Settings();

			Dialect.Dialect dialect = null;
			try
			{
				dialect = Dialect.Dialect.GetDialect(properties);
				IDictionary temp = new Hashtable();

				foreach (DictionaryEntry de in dialect.DefaultProperties)
				{
					temp[de.Key] = de.Value;
				}
				foreach (DictionaryEntry de in properties)
				{
					temp[de.Key] = de.Value;
				}
				properties = temp;
			}
			catch (HibernateException he)
			{
				log.Warn("No dialect set - using GenericDialect: " + he.Message);
				dialect = new GenericDialect();
			}

			// TODO: SQLExceptionConverter

			// TODO: should this be enabled?
//			int statementFetchSize = PropertiesHelper.GetInt32( Environment.StatementFetchSize, properties, -1 );
//			if( statementFetchSize != -1 )
//			{
//				log.Info( "JDBC result set fetch size: " + statementFetchSize );
//			}

			int maxFetchDepth = PropertiesHelper.GetInt32(Environment.MaxFetchDepth, properties, -1);
			if (maxFetchDepth != -1)
			{
				log.Info("Maximum outer join fetch depth: " + maxFetchDepth);
			}

			IConnectionProvider connectionProvider = ConnectionProviderFactory.NewConnectionProvider(properties);
			ITransactionFactory transactionFactory = CreateTransactionFactory(properties);
			// TransactionManagerLookup transactionManagerLookup = TransactionManagerLookupFactory.GetTransactionManagerLookup( properties );

			// Not ported: useGetGeneratedKeys, useScrollableResultSets

			bool useMinimalPuts = PropertiesHelper.GetBoolean(Environment.UseMinimalPuts, properties, false);
			log.Info("Optimize cache for minimal puts: " + useMinimalPuts);

			string releaseModeName = PropertiesHelper.GetString(Environment.ReleaseConnections, properties, "auto");
			log.Info("Connection release mode: " + releaseModeName);
			ConnectionReleaseMode releaseMode;
			if ("auto".Equals(releaseModeName))
			{
				releaseMode = ConnectionReleaseMode.AfterTransaction; //transactionFactory.DefaultReleaseMode;
			}
			else
			{
				releaseMode = ParseConnectionReleaseMode(releaseModeName);
			}
			settings.ConnectionReleaseMode = releaseMode;

			string defaultSchema = properties[Environment.DefaultSchema] as string;
			if (defaultSchema != null)
			{
				log.Info("Default schema set to: " + defaultSchema);
			}

			bool showSql = PropertiesHelper.GetBoolean(Environment.ShowSql, properties, false);
			if (showSql)
			{
				log.Info("echoing all SQL to stdout");
			}

			// queries:

			settings.QueryTranslatorFactory = CreateQueryTranslatorFactory(properties);

			IDictionary querySubstitutions =
				PropertiesHelper.ToDictionary(Environment.QuerySubstitutions, " ,=;:\n\t\r\f", properties);
			if (log.IsInfoEnabled)
			{
				log.Info("Query language substitutions: " + CollectionPrinter.ToString(querySubstitutions));
			}

			string autoSchemaExport = properties[Environment.Hbm2ddlAuto] as string;
			if ("update" == autoSchemaExport)
			{
				settings.IsAutoUpdateSchema = true;
			}
			if ("create" == autoSchemaExport)
			{
				settings.IsAutoCreateSchema = true;
			}
			if ("create-drop" == autoSchemaExport)
			{
				settings.IsAutoCreateSchema = true;
				settings.IsAutoDropSchema = true;
			}

			bool useSecondLevelCache = PropertiesHelper.GetBoolean(Environment.UseSecondLevelCache, properties, true);
			bool useQueryCache = PropertiesHelper.GetBoolean(Environment.UseQueryCache, properties);

			if (useSecondLevelCache || useQueryCache)
			{
				// The cache provider is needed when we either have second-level cache enabled
				// or query cache enabled.  Note that useSecondLevelCache is enabled by default
				settings.CacheProvider = CreateCacheProvider(properties);
			}
			else
			{
				settings.CacheProvider = new NoCacheProvider();
			}

			string cacheRegionPrefix = PropertiesHelper.GetString(Environment.CacheRegionPrefix, properties, null);
			if (StringHelper.IsEmpty(cacheRegionPrefix)) cacheRegionPrefix = null;
			if (cacheRegionPrefix != null) log.Info("Cache region prefix: " + cacheRegionPrefix);


			if (useQueryCache)
			{
				string queryCacheFactoryClassName =
					PropertiesHelper.GetString(Environment.QueryCacheFactory, properties, "NHibernate.Cache.StandardQueryCacheFactory");
				log.Info("query cache factory: " + queryCacheFactoryClassName);
				try
				{
					settings.QueryCacheFactory = (IQueryCacheFactory) Activator.CreateInstance(
					                                                  	ReflectHelper.ClassForName(queryCacheFactoryClassName));
				}
				catch (Exception cnfe)
				{
					throw new HibernateException("could not instantiate IQueryCacheFactory: " + queryCacheFactoryClassName, cnfe);
				}
			}

			string sessionFactoryName = (string) properties[Environment.SessionFactoryName];

			// TODO: Environment.BatchVersionedData
			// TODO: wrapResultSets/DataReaders

			string isolationString = PropertiesHelper.GetString(Environment.Isolation, properties, String.Empty);
			IsolationLevel isolation = IsolationLevel.Unspecified;
			if (isolationString.Length > 0)
			{
				try
				{
					isolation = (IsolationLevel) Enum.Parse(typeof(IsolationLevel), isolationString);
					log.Info("Using Isolation Level: " + isolation.ToString());
				}
				catch (ArgumentException ae)
				{
					log.Error("error configuring IsolationLevel " + isolationString, ae);
					throw new HibernateException(
						"The isolation level of " + isolationString + " is not a valid IsolationLevel.  Please " +
						"use one of the Member Names from the IsolationLevel.", ae);
				}
			}

			// Not ported - settings.StatementFetchSize = statementFetchSize;
			// Not ported - ScrollableResultSetsEnabled
			// Not ported - GetGeneratedKeysEnabled
			settings.BatchSize = PropertiesHelper.GetInt32(Environment.BatchSize, properties, 0);
			settings.DefaultSchemaName = defaultSchema;
			settings.IsShowSqlEnabled = showSql;
			settings.Dialect = dialect;
			settings.ConnectionProvider = connectionProvider;
			settings.QuerySubstitutions = querySubstitutions;
			settings.TransactionFactory = transactionFactory;
			// Not ported - TransactionManagerLookup
			settings.SessionFactoryName = sessionFactoryName;
			settings.MaximumFetchDepth = maxFetchDepth;
			settings.IsQueryCacheEnabled = useQueryCache;
			settings.IsSecondLevelCacheEnabled = useSecondLevelCache;
			settings.CacheRegionPrefix = cacheRegionPrefix;
			settings.IsMinimalPutsEnabled = useMinimalPuts;
			// Not ported - JdbcBatchVersionedData
			// TODO: SQLExceptionConverter
			// TODO: WrapResultSetsEnabled

			// NHibernate-specific:
			settings.IsolationLevel = isolation;

			return settings;
		}

		private static ICacheProvider CreateCacheProvider(IDictionary properties)
		{
			string cacheClassName = PropertiesHelper.GetString(Environment.CacheProvider, properties, DefaultCacheProvider);
			log.Info("cache provider: " + cacheClassName);
			try
			{
				return (ICacheProvider) Activator.CreateInstance(ReflectHelper.ClassForName(cacheClassName));
			}
			catch (Exception e)
			{
				throw new HibernateException("could not instantiate CacheProvider: " + cacheClassName, e);
			}
		}

		private SettingsFactory()
		{
			//should not be publically creatable
		}

		private static ConnectionReleaseMode ParseConnectionReleaseMode(string name)
		{
			switch (name)
			{
				case "after_statement":
					throw new HibernateException("aggressive connection release (after_statement) not supported by NHibernate");
				case "after_transaction":
					return ConnectionReleaseMode.AfterTransaction;
				case "on_close":
					return ConnectionReleaseMode.OnClose;
				default:
					throw new HibernateException("could not determine appropriate connection release mode [" + name + "]");
			}
		}

		// visibility changed and static modifier added until complete H3.2 porting of SettingsFactory
		private static IQueryTranslatorFactory CreateQueryTranslatorFactory(IDictionary properties)
		{
			string className = PropertiesHelper.GetString(
				Environment.QueryTranslator, properties, "NHibernate.Hql.Classic.ClassicQueryTranslatorFactory");
			log.Info("Query translator: " + className);
			try
			{
				return (IQueryTranslatorFactory) Activator.CreateInstance(ReflectHelper.ClassForName(className));
			}
			catch (Exception cnfe)
			{
				throw new HibernateException("could not instantiate QueryTranslatorFactory: " + className, cnfe);
			}
		}

		private static ITransactionFactory CreateTransactionFactory(IDictionary properties)
		{
			string className = PropertiesHelper.GetString(
				Environment.TransactionStrategy, properties, "NHibernate.Transaction.AdoNetTransactionFactory");
			log.Info("Transaction factory: " + className);

			try
			{
				return (ITransactionFactory)Activator.CreateInstance(ReflectHelper.ClassForName(className));
			}
			catch (Exception cnfe)
			{
				throw new HibernateException("could not instantiate TransactionFactory: " + className, cnfe);
			}
		}
	}
}
