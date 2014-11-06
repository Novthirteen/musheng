using System;
using System.Collections;
using System.Xml;
using log4net;
using NHibernate.Engine;
using NHibernate.Util;

namespace NHibernate.Cfg
{
	public class NamedSQLQuerySecondPass : ResultSetMappingBinder, IQuerySecondPass
	{
		private static ILog log = LogManager.GetLogger(typeof(NamedSQLQuerySecondPass));
		private readonly XmlNode queryElem;
		private readonly string path;
		private readonly Mappings mappings;

		public NamedSQLQuerySecondPass(XmlNode queryElem, string path, Mappings mappings, XmlNamespaceManager nsmgr)
			: base(nsmgr)
		{
			this.queryElem = queryElem;
			this.path = path;
			this.mappings = mappings;
		}

		public void DoSecondPass(IDictionary persistentClasses)
		{
			String queryName = queryElem.Attributes["name"].Value;
			if (path != null)
			{
				queryName = path + '.' + queryName;
			}

			bool cacheable = "true".Equals(XmlHelper.GetAttributeValue(queryElem, "cacheable"));
			string region = XmlHelper.GetAttributeValue(queryElem, "cache-region");
			XmlAttribute tAtt = queryElem.Attributes["timeout"];
			int timeout = tAtt == null ? -1 : int.Parse(tAtt.Value);
			XmlAttribute fsAtt = queryElem.Attributes["fetch-size"];
			int fetchSize = fsAtt == null ? -1 : int.Parse(fsAtt.Value);
			XmlAttribute roAttr = queryElem.Attributes["read-only"];
			bool readOnly = roAttr != null && "true".Equals(roAttr.Value);
			XmlAttribute cacheModeAtt = queryElem.Attributes["cache-mode"];
			string cacheMode = cacheModeAtt == null ? null : cacheModeAtt.Value;
			XmlAttribute cmAtt = queryElem.Attributes["comment"];
			string comment = cmAtt == null ? null : cmAtt.Value;

			IList synchronizedTables = new ArrayList();

			foreach (XmlNode item in queryElem.SelectNodes(HbmConstants.nsSynchronize, nsmgr))
			{
				synchronizedTables.Add(XmlHelper.GetAttributeValue(item, "table"));
			}
			bool callable = "true".Equals(XmlHelper.GetAttributeValue(queryElem, "callable"));

			NamedSQLQueryDefinition namedQuery;
			XmlAttribute @ref = queryElem.Attributes["resultset-ref"];
			string resultSetRef = @ref == null ? null : @ref.Value;
			if (StringHelper.IsNotEmpty(resultSetRef))
			{
				namedQuery = new NamedSQLQueryDefinition(
					queryElem.InnerText,
					resultSetRef,
					synchronizedTables,
					cacheable,
					region,
					timeout,
					fetchSize,
					HbmBinder.GetFlushMode(XmlHelper.GetAttributeValue(queryElem, "flush-mode")),
					//HbmBinder.GetCacheMode(cacheMode),
					readOnly,
					comment,
					HbmBinder.GetParameterTypes(queryElem),
					callable
					);
				//TODO check there is no actual definition elemnents when a ref is defined
			}
			else
			{
				ResultSetMappingDefinition definition = BuildResultSetMappingDefinition(queryElem, path, mappings);
				namedQuery = new NamedSQLQueryDefinition(
					queryElem.InnerText,
					definition.GetQueryReturns(),
					synchronizedTables,
					cacheable,
					region,
					timeout,
					fetchSize,
					HbmBinder.GetFlushMode(XmlHelper.GetAttributeValue(queryElem, "flush-mode")),
					//HbmBinder.GetCacheMode(cacheMode),
					readOnly,
					comment,
					HbmBinder.GetParameterTypes(queryElem),
					callable
					);
			}

			log.Debug("Named SQL query: " + queryName + " -> " + namedQuery.QueryString);
			mappings.AddSQLQuery(queryName, namedQuery);
		}
	}
}