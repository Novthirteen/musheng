using System;

namespace NHibernate.Expression
{
	/// <summary>
	/// An <see cref="ICriterion"/> that represents non-empty association constraint.
	/// </summary>
	[Serializable]
	public class IsNotEmptyExpression : AbstractEmptinessExpression
	{
		public IsNotEmptyExpression(string propertyName)
			: base(propertyName)
		{
		}

		protected override bool ExcludeEmpty
		{
			get { return true; }
		}
	}
}