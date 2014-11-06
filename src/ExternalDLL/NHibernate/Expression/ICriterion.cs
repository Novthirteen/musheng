using System.Collections;
using NHibernate.Engine;
using NHibernate.SqlCommand;

namespace NHibernate.Expression
{
	/// <summary>
	/// An object-oriented representation of a query criterion that may be used as a constraint
	/// in a <see cref="ICriteria"/> query.
	/// </summary>
	/// <remarks>
	/// Built-in criterion types are provided by the <c>Expression</c> factory class.
	/// This interface might be implemented by application classes but, more commonly, application 
	/// criterion types would extend <c>AbstractCriterion</c>.
	/// </remarks>
	public interface ICriterion
	{
		/// <summary>
		/// Render a SqlString fragment for the expression.
		/// </summary>
		/// <returns>A SqlString that contains a valid Sql fragment.</returns>
		SqlString ToSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary enabledFilters);

		/// <summary>
		/// Return typed values for all parameters in the rendered SQL fragment
		/// </summary>
		/// <returns>An array of TypedValues for the Expression.</returns>
		TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery);
	}
}