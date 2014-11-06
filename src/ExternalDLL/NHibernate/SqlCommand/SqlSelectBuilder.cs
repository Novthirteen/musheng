using System;
using log4net;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.SqlCommand
{
	/// <summary>
	/// Builds a <c>SELECT</c> SQL statement.
	/// </summary>
	public class SqlSelectBuilder : SqlBaseBuilder, ISqlStringBuilder
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(SqlSelectBuilder));

		private string selectClause;
		private string fromClause;
		private SqlString outerJoinsAfterFrom;
		private SqlString whereClause;
		private SqlString outerJoinsAfterWhere;
		private string orderByClause;
		private string groupByClause;
		private LockMode lockMode;

		public readonly Dialect.Dialect dialect;

		public SqlSelectBuilder(ISessionFactoryImplementor factory) : base(factory)
		{
			this.dialect = factory.Dialect;
		}

		/// <summary>
		/// Sets the text that should appear after the FROM 
		/// </summary>
		/// <param name="fromClause">The fromClause to set</param>
		/// <returns>The SqlSelectBuilder</returns>
		public SqlSelectBuilder SetFromClause(string fromClause)
		{
			this.fromClause = fromClause;
			return this;
		}

		/// <summary>
		/// Sets the text that should appear after the FROM 
		/// </summary>
		/// <param name="tableName">The name of the Table to get the data from</param>
		/// <param name="alias">The Alias to use for the table name.</param>
		/// <returns>The SqlSelectBuilder</returns>
		public SqlSelectBuilder SetFromClause(string tableName, string alias)
		{
			this.fromClause = tableName + " " + alias;
			return this;
		}

		/// <summary>
		/// Sets the text that should appear after the FROM
		/// </summary>
		/// <param name="fromClause">The fromClause in a SqlString</param>
		/// <returns>The SqlSelectBuilder</returns>
		public SqlSelectBuilder SetFromClause(SqlString fromClause)
		{
			// it is safe to do this because a fromClause will have no
			// parameters
			return SetFromClause(fromClause.ToString());
		}

		/// <summary>
		/// Sets the text that should appear after the ORDER BY.
		/// </summary>
		/// <param name="orderByClause">The orderByClause to set</param>
		/// <returns>The SqlSelectBuilder</returns>
		public SqlSelectBuilder SetOrderByClause(string orderByClause)
		{
			this.orderByClause = orderByClause;
			return this;
		}

		/// <summary>
		/// Sets the text that should appear after the GROUP BY.
		/// </summary>
		/// <param name="groupByClause">The groupByClause to set</param>
		/// <returns>The SqlSelectBuilder</returns>
		public SqlSelectBuilder SetGroupByClause(string groupByClause)
		{
			this.groupByClause = groupByClause;
			return this;
		}

		/// <summary>
		/// Sets the SqlString for the OUTER JOINs.  
		/// </summary>
		/// <remarks>
		/// All of the Sql needs to be included in the SELECT.  No OUTER JOINS will automatically be
		/// added.
		/// </remarks>
		/// <param name="outerJoinsAfterFrom">The outerJoinsAfterFrom to set</param>
		/// <param name="outerJoinsAfterWhere">The outerJoinsAfterWhere to set</param>
		/// <returns>The SqlSelectBuilder</returns>
		public SqlSelectBuilder SetOuterJoins(SqlString outerJoinsAfterFrom, SqlString outerJoinsAfterWhere)
		{
			this.outerJoinsAfterFrom = outerJoinsAfterFrom;

			SqlString tmpOuterJoinsAfterWhere = outerJoinsAfterWhere.Trim();
			if (tmpOuterJoinsAfterWhere.StartsWithCaseInsensitive("and"))
			{
				tmpOuterJoinsAfterWhere = tmpOuterJoinsAfterWhere.Substring(4);
			}

			this.outerJoinsAfterWhere = tmpOuterJoinsAfterWhere;
			return this;
		}

		/// <summary>
		/// Sets the text for the SELECT
		/// </summary>
		/// <param name="selectClause">The selectClause to set</param>
		/// <returns>The SqlSelectBuilder</returns>
		public SqlSelectBuilder SetSelectClause(string selectClause)
		{
			this.selectClause = selectClause;
			return this;
		}

		/// <summary>
		/// Sets the criteria to use for the WHERE.  It joins all of the columnNames together with an AND.
		/// </summary>
		/// <param name="tableAlias"></param>
		/// <param name="columnNames">The names of the columns</param>
		/// <param name="whereType">The Hibernate Type</param>
		/// <returns>The SqlSelectBuilder</returns>
		public SqlSelectBuilder SetWhereClause(string tableAlias, string[] columnNames, IType whereType)
		{
			return SetWhereClause(ToWhereString(tableAlias, columnNames));
		}

		/// <summary>
		/// Sets the prebuilt SqlString to the Where clause
		/// </summary>
		/// <param name="whereSqlString">The SqlString that contains the sql and parameters to add to the WHERE</param>
		/// <returns>This SqlSelectBuilder</returns>
		public SqlSelectBuilder SetWhereClause(SqlString whereSqlString)
		{
			whereClause = whereSqlString;
			return this;
		}

		public SqlSelectBuilder SetLockMode(LockMode lockMode)
		{
			this.lockMode = lockMode;
			return this;
		}

		#region ISqlStringBuilder Members

		/// <summary></summary>
		public SqlString ToSqlString()
		{
			// 4 = the "SELECT", selectClause, "FROM", fromClause are straight strings
			// plus the number of parts in outerJoinsAfterFrom SqlString.
			// 1 = the "WHERE" 
			// plus the number of parts in outerJoinsAfterWhere SqlString.
			// 1 = the whereClause
			// 2 = the "ORDER BY" and orderByClause
			int initialCapacity = 4 + outerJoinsAfterFrom.Count + 1 + outerJoinsAfterWhere.Count + 1 + 2;

			SqlStringBuilder sqlBuilder = new SqlStringBuilder(initialCapacity + 2);

			sqlBuilder.Add("SELECT ")
				.Add(selectClause)
				.Add(" FROM ")
				.Add(fromClause)
				.Add(outerJoinsAfterFrom);

			if (StringHelper.IsNotEmpty(whereClause) || StringHelper.IsNotEmpty(outerJoinsAfterWhere))
			{
				sqlBuilder.Add(" WHERE ");
				// the outerJoinsAfterWhere needs to come before where clause to properly
				// handle dynamic filters
				if (StringHelper.IsNotEmpty(outerJoinsAfterWhere))
				{
					sqlBuilder.Add(outerJoinsAfterWhere);
					if (StringHelper.IsNotEmpty(whereClause))
					{
						sqlBuilder.Add(" AND ");
					}
				}

				if (StringHelper.IsNotEmpty(whereClause))
				{
					sqlBuilder.Add(whereClause);
				}
			}

			if (StringHelper.IsNotEmpty(groupByClause))
			{
				sqlBuilder.Add(" GROUP BY ")
					.Add(groupByClause);
			}
			if (StringHelper.IsNotEmpty(orderByClause))
			{
				sqlBuilder.Add(" ORDER BY ")
					.Add(orderByClause);
			}

			if (lockMode != null)
			{
				sqlBuilder.Add(dialect.GetForUpdateString(lockMode));
			}

			if (log.IsDebugEnabled)
			{
				if (initialCapacity < sqlBuilder.Count)
				{
					log.Debug(
						"The initial capacity was set too low at: " + initialCapacity + " for the SelectSqlBuilder " +
						"that needed a capacity of: " + sqlBuilder.Count + " for the table " + fromClause);
				}
				else if (initialCapacity > 16 && ((float) initialCapacity / sqlBuilder.Count) > 1.2)
				{
					log.Debug(
						"The initial capacity was set too high at: " + initialCapacity + " for the SelectSqlBuilder " +
						"that needed a capacity of: " + sqlBuilder.Count + " for the table " + fromClause);
				}
			}

			return sqlBuilder.ToSqlString();
		}

		#endregion
	}
}