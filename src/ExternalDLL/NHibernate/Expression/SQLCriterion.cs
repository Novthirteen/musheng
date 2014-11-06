using System;
using System.Collections;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.Expression
{
	/// <summary>
	/// An <see cref="ICriterion"/> that creates a SQLExpression.
	/// The string {alias} will be replaced by the alias of the root entity.
	/// </summary>
	/// <remarks>
	/// This allows for database specific Expressions at the cost of needing to 
	/// write a correct <see cref="SqlString"/>.
	/// </remarks>
	[Serializable]
	public class SQLCriterion : AbstractCriterion
	{
		private readonly SqlString _sql;
		private readonly TypedValue[] _typedValues;

		public SQLCriterion(SqlString sql, object[] values, IType[] types)
		{
			_sql = sql;
			_typedValues = new TypedValue[values.Length];
			for (int i = 0; i < _typedValues.Length; i++)
			{
				_typedValues[i] = new TypedValue(types[i], values[i]);
			}
		}

		public override SqlString ToSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary enabledFilters)
		{
			return _sql.Replace("{alias}", criteriaQuery.GetSQLAlias(criteria));
		}

		public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return _typedValues;
		}

		/// <summary></summary>
		public override string ToString()
		{
			return _sql.ToString();
		}
	}
}