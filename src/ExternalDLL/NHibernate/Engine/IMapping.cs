using NHibernate.Type;

namespace NHibernate.Engine
{
	/// <summary>
	/// Defines operations common to "compiled" mappings (ie. <c>SessionFactory</c>) and
	/// "uncompiled" mappings (ie <c>Configuration</c> that are used by implementors of <c>IType</c>
	/// </summary>
	public interface IMapping
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="persistentClass"></param>
		/// <returns></returns>
		IType GetIdentifierType(System.Type persistentClass);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="persistentClass"></param>
		/// <returns></returns>
		string GetIdentifierPropertyName(System.Type persistentClass);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="persistentClass"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		IType GetPropertyType(System.Type persistentClass, string propertyName);
	}
}