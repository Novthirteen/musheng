using System;
using NHibernate.Type;

namespace NHibernate.Persister.Entity
{
	/// <summary>
	/// Abstraction of all mappings that define properties: entities, collection elements.
	/// </summary>
	public interface IPropertyMapping
	{
		// TODO: It would be really, really nice to use this to also model components!

		/// <summary>
		/// Given a component path expression, get the type of the property
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		IType ToType(string propertyName);

		/// <summary>
		/// Given a query alias and a property path, return the qualified column name
		/// </summary>
		/// <param name="alias"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		string[] ToColumns(string alias, string propertyName);

		/// <summary>
		/// Get the type of the thing containing the properties
		/// </summary>
		IType Type { get; }
	}
}