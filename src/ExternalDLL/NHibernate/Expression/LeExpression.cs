using System;

namespace NHibernate.Expression
{
	/// <summary>
	/// An <see cref="ICriterion"/> that represents an "less than or equal" constraint.
	/// </summary>
	[Serializable]
	public class LeExpression : SimpleExpression
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="LeExpression" /> class for a named
		/// Property and its value.
		/// </summary>
		/// <param name="propertyName">The name of the Property in the class.</param>
		/// <param name="value">The value for the Property.</param>
		public LeExpression(string propertyName, object value) : base(propertyName, value)
		{
		}

		/// <summary>
		/// Get the Sql operator to use for the <see cref="LeExpression"/>.
		/// </summary>
		/// <value>The string "<c> &lt;= </c>"</value>
		protected override string Op
		{
			get { return " <= "; }
		}
	}
}