using System;
using System.Collections;

namespace NHibernate.Transform
{
	public sealed class Transformers
	{
		private Transformers()
		{
		}

		/// <summary>
		/// Each row of results is a map (<see cref="IDictionary" />) from alias to values/entities
		/// </summary>
		public static readonly IResultTransformer AliasToEntityMap = new AliasToEntityMapResultTransformer();

		/// <summary>
		/// Creates a resulttransformer that will inject aliased values into instances
		/// of <paramref name="target"/> via property methods or fields.
		/// </summary>
		public static IResultTransformer AliasToBean(System.Type target)
		{
			return new AliasToBeanResultTransformer(target);
		}
	}
}