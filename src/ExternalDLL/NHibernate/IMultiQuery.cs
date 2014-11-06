using System;
using System.Collections;
using NHibernate.Transform;
using NHibernate.Type;

namespace NHibernate
{
	/// <summary>
	/// Combines sevaral queries into a single database call
	/// </summary>
	public interface IMultiQuery
	{
		/// <summary>
		/// Get all the 
		/// </summary>
		IList List();

		/// <summary>
		/// Add the specified HQL query to the multi query
		/// </summary>
		IMultiQuery Add(IQuery query);

		/// <summary>
		/// Add the specified HQL query to the multi query
		/// </summary>
		IMultiQuery Add(string hql);

		/// <summary>
		/// Add a named query to the multi query
		/// </summary>
		IMultiQuery AddNamedQuery(string namedQuery);

		/// <summary>
		/// Enable caching of this query result set.
		/// </summary>
		/// <param name="cacheable">Should the query results be cacheable?</param>
		IMultiQuery SetCacheable(bool cacheable);

		/// Set the name of the cache region.
		/// <param name="cacheRegion">The name of a query cache region, or <see langword="null" />
		/// for the default query cache</param>
		IMultiQuery SetCacheRegion(string cacheRegion);

		/// Should the query force a refresh of the specified query cache region?
		/// This is particularly useful in cases where underlying data may have been
		/// updated via a seperate process (i.e., not modified through Hibernate) and
		/// allows the application to selectively refresh the query cache regions
		/// based on its knowledge of those events.
		/// <param name="forceCacheRefresh">Should the query result in a forceable refresh of
		/// the query cache?</param>
		IMultiQuery SetForceCacheRefresh(bool forceCacheRefresh);

		/// <summary>
		/// The timeout for the underlying ADO query
		/// </summary>
		/// <param name="timeout"></param>
		IMultiQuery SetTimeout(int timeout);

		/// <summary>
		/// Bind a value to a named query parameter
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">The possibly null parameter value</param>
		/// <param name="type">The NHibernate <see cref="IType"/>.</param>
		IMultiQuery SetParameter(string name, object val, IType type);


		/// <summary>
		/// Bind a value to a named query parameter, guessing the NHibernate <see cref="IType"/>
		/// from the class of the given object.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">The non-null parameter value</param>
		IMultiQuery SetParameter(string name, object val);

		/// <summary>
		/// Bind multiple values to a named query parameter. This is useful for binding a list
		/// of values to an expression such as <c>foo.bar in (:value_list)</c>
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="vals">A collection of values to list</param>
		/// <param name="type">The Hibernate type of the values</param>
		IMultiQuery SetParameterList(string name, ICollection vals, IType type);

		/// <summary>
		/// Bind multiple values to a named query parameter, guessing the Hibernate
		/// type from the class of the first object in the collection. This is useful for binding a list
		/// of values to an expression such as <c>foo.bar in (:value_list)</c>
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="vals">A collection of values to list</param>
		IMultiQuery SetParameterList(string name, ICollection vals);

		/// <summary>
		/// Bind an instance of a <see cref="String" /> to a named parameter
		/// using an NHibernate <see cref="AnsiStringType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="String"/>.</param>
		IMultiQuery SetAnsiString(string name, string val);

		/// <summary>
		/// Bind an instance of a <see cref="Byte" /> array to a named parameter
		/// using an NHibernate <see cref="BinaryType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Byte"/> array.</param>
		IMultiQuery SetBinary(string name, byte[] val);

		/// <summary>
		/// Bind an instance of a <see cref="Boolean" /> to a named parameter
		/// using an NHibernate <see cref="BooleanType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Boolean"/>.</param>
		IMultiQuery SetBoolean(string name, bool val);

		/// <summary>
		/// Bind an instance of a <see cref="Byte" /> to a named parameter
		/// using an NHibernate <see cref="ByteType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Byte"/>.</param>
		IMultiQuery SetByte(string name, byte val);

		/// <summary>
		/// Bind an instance of a <see cref="Char" /> to a named parameter
		/// using an NHibernate <see cref="CharType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Char"/>.</param>
		IMultiQuery SetCharacter(string name, char val);

		/// <summary>
		/// Bind an instance of a <see cref="DateTime" /> to a named parameter
		/// using an NHibernate <see cref="DateTimeType"/>.
		/// </summary>
		/// <param name="val">A non-null instance of a <see cref="DateTime"/>.</param>
		/// <param name="name">The name of the parameter</param>
		IMultiQuery SetDateTime(string name, DateTime val);

		/// <summary>
		/// Bind an instance of a <see cref="Decimal" /> to a named parameter
		/// using an NHibernate <see cref="DecimalType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Decimal"/>.</param>
		IMultiQuery SetDecimal(string name, decimal val);

		/// <summary>
		/// Bind an instance of a <see cref="Double" /> to a named parameter
		/// using an NHibernate <see cref="DoubleType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Double"/>.</param>
		IMultiQuery SetDouble(string name, double val);

		/// <summary>
		/// Bind an instance of a mapped persistent class to a named parameter.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a persistent class</param>
		IMultiQuery SetEntity(string name, object val);

		/// <summary>
		/// Bind an instance of a persistent enumeration class to a named parameter
		/// using an NHibernate <see cref="PersistentEnumType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a persistent enumeration</param>
		IMultiQuery SetEnum(string name, Enum val);

		/// <summary>
		/// Bind an instance of a <see cref="Int16" /> to a named parameter
		/// using an NHibernate <see cref="Int16Type"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Int16"/>.</param>
		IMultiQuery SetInt16(string name, short val);

		/// <summary>
		/// Bind an instance of a <see cref="Int32" /> to a named parameter
		/// using an NHibernate <see cref="Int32Type"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Int32"/>.</param>
		IMultiQuery SetInt32(string name, int val);

		/// <summary>
		/// Bind an instance of a <see cref="Int64" /> to a named parameter
		/// using an NHibernate <see cref="Int64Type"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Int64"/>.</param>
		IMultiQuery SetInt64(string name, long val);

		/// <summary>
		/// Bind an instance of a <see cref="Single" /> to a named parameter
		/// using an NHibernate <see cref="SingleType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="Single"/>.</param>
		IMultiQuery SetSingle(string name, float val);

		/// <summary>
		/// Bind an instance of a <see cref="String" /> to a named parameter
		/// using an NHibernate <see cref="StringType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="String"/>.</param>
		IMultiQuery SetString(string name, string val);


		/// <summary>
		/// Bind an instance of a <see cref="DateTime" /> to a named parameter
		/// using an NHibernate <see cref="DateTimeType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="DateTime"/>.</param>
		IMultiQuery SetTime(string name, DateTime val);

		/// <summary>
		/// Bind an instance of a <see cref="DateTime" /> to a named parameter
		/// using an NHibernate <see cref="TimestampType"/>.
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="val">A non-null instance of a <see cref="DateTime"/>.</param>
		IMultiQuery SetTimestamp(string name, DateTime val);

		/// <summary>
		/// Override the current session flush mode, just for this query.
		/// </summary>
		IMultiQuery SetFlushMode(FlushMode flushMode);

		/// <summary>
		/// Set a strategy for handling the query results. This can be used to change
		/// "shape" of the query result.
		/// </summary>
		IMultiQuery SetResultTransformer(IResultTransformer transformer);
	}
}