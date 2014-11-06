using System;
using System.Collections;
using NHibernate.Collection;
using NHibernate.Impl;
using NHibernate.Proxy;
using NHibernate.Type;
using NHibernate.UserTypes;

namespace NHibernate
{
	/// <summary>
	/// Provides access to the full range of NHibernate built-in types.
	/// IType instances may be used to bind values to query parameters.
	/// Also a factory for new Blobs and Clobs.
	/// </summary>
	public sealed class NHibernateUtil
	{
		/// <summary>
		/// NHibernate Ansi String type
		/// </summary>
		public static readonly NullableType AnsiString = new AnsiStringType();

		/// <summary>
		/// NHibernate binary type
		/// </summary>
		public static readonly NullableType Binary = new BinaryType();

		/// <summary>
		/// NHibernate binary blob type
		/// </summary>
		public static readonly NullableType BinaryBlob = new BinaryBlobType();

		/// <summary>
		/// NHibernate boolean type
		/// </summary>
		public static readonly NullableType Boolean = new BooleanType();

		/// <summary>
		/// NHibernate byte type
		/// </summary>
		public static readonly NullableType Byte = new ByteType();

		/// <summary>
		/// NHibernate character type
		/// </summary>
		public static readonly NullableType Character = new CharType();

		/// <summary>
		/// NHibernate Culture Info type
		/// </summary>
		public static readonly NullableType CultureInfo = new CultureInfoType();

		/// <summary>
		/// NHibernate date type
		/// </summary>
		public static readonly NullableType DateTime = new DateTimeType();

		/// <summary>
		/// NHibernate date type
		/// </summary>
		public static readonly NullableType Date = new DateType();

		/// <summary>
		/// NHibernate decimal type
		/// </summary>
		public static readonly NullableType Decimal = new DecimalType();

		/// <summary>
		/// NHibernate double type
		/// </summary>
		public static readonly NullableType Double = new DoubleType();

		/// <summary>
		/// NHibernate Guid type.
		/// </summary>
		public static readonly NullableType Guid = new GuidType();

		/// <summary>
		/// NHibernate System.Int16 (short in C#) type
		/// </summary>
		public static readonly NullableType Int16 = new Int16Type();

		/// <summary>
		/// NHibernate System.Int32 (int in C#) type
		/// </summary>
		public static readonly NullableType Int32 = new Int32Type();

		/// <summary>
		/// NHibernate System.Int64 (long in C#) type
		/// </summary>
		public static readonly NullableType Int64 = new Int64Type();

		/// <summary>
		/// NHibernate System.SByte type
		/// </summary>
		public static readonly NullableType SByte = new SByteType();

		/// <summary>
		/// NHibernate System.UInt16 (ushort in C#) type
		/// </summary>
		public static readonly NullableType UInt16 = new UInt16Type();

		/// <summary>
		/// NHibernate System.UInt32 (uint in C#) type
		/// </summary>
		public static readonly NullableType UInt32 = new UInt32Type();

		/// <summary>
		/// NHibernate System.UInt64 (ulong in C#) type
		/// </summary>
		public static readonly NullableType UInt64 = new UInt64Type();

		/// <summary>
		/// NHIbernate System.Single (float in C#) Type
		/// </summary>
		public static readonly NullableType Single = new SingleType();

		/// <summary>
		/// NHibernate String type
		/// </summary>
		public static readonly NullableType String = new StringType();

		/// <summary>
		/// NHibernate string clob type
		/// </summary>
		public static readonly NullableType StringClob = new StringClobType();

		/// <summary>
		/// NHibernate Time type
		/// </summary>
		public static readonly NullableType Time = new TimeType();

		/// <summary>
		/// NHibernate Ticks type
		/// </summary>
		public static readonly NullableType Ticks = new TicksType();

		/// <summary>
		/// NHibernate Ticks type
		/// </summary>
		public static readonly NullableType TimeSpan = new TimeSpanType();

		/// <summary>
		/// NHibernate Timestamp type
		/// </summary>
		public static readonly NullableType Timestamp = new TimestampType();

		/// <summary>
		/// NHibernate TrueFalse type
		/// </summary>
		public static readonly NullableType TrueFalse = new TrueFalseType();

		/// <summary>
		/// NHibernate YesNo type
		/// </summary>
		public static readonly NullableType YesNo = new YesNoType();

		/// <summary>
		/// NHibernate class type
		/// </summary>
		public static readonly NullableType Class = new TypeType();

		/// <summary>
		/// NHibernate serializable type
		/// </summary>
		public static readonly NullableType Serializable = new SerializableType();

		/// <summary>
		/// NHibernate System.Object type
		/// </summary>
		public static readonly IType Object = new AnyType();


//		/// <summary>
//		/// NHibernate blob type
//		/// </summary>
//		public static readonly NullableType Blob = new BlobType();
//		/// <summary>
//		/// NHibernate clob type
//		/// </summary>
//		public static readonly NullableType Clob = new ClobType();
		/// <summary>
		/// Cannot be instantiated.
		/// </summary>
		private NHibernateUtil()
		{
			throw new NotSupportedException();
		}

		public static readonly NullableType AnsiChar = new AnsiCharType();

		/// <summary>
		/// A NHibernate persistent enum type
		/// </summary>
		/// <param name="enumClass"></param>
		/// <returns></returns>
		public static IType Enum(System.Type enumClass)
		{
			return new PersistentEnumType(enumClass);
		}

		/// <summary>
		/// A NHibernate serializable type
		/// </summary>
		/// <param name="serializableClass"></param>
		/// <returns></returns>
		public static IType GetSerializable(System.Type serializableClass)
		{
			return new SerializableType(serializableClass);
		}

		/// <summary>
		/// A NHibernate serializable type
		/// </summary>
		/// <param name="metaType">a type mapping <see cref="IType"/> to a single column</param>
		/// <param name="identifierType">the entity identifier type</param>
		/// <returns></returns>
		public static IType Any(IType metaType, IType identifierType)
		{
			return new AnyType(metaType, identifierType);
		}

		/// <summary>
		/// A NHibernate persistent object (entity) type
		/// </summary>
		/// <param name="persistentClass">a mapped entity class</param>
		/// <returns></returns>
		[Obsolete("use NHibernate.Entity instead")]
		public static IType Association(System.Type persistentClass)
		{
			// not really a many-to-one association *necessarily*
			return new ManyToOneType(persistentClass);
		}

		/// <summary>
		/// A NHibernate persistent object (entity) type
		/// </summary>
		/// <param name="persistentClass">a mapped entity class</param>
		/// <returns></returns>
		public static IType Entity(System.Type persistentClass)
		{
			// not really a many-to-one association *necessarily*
			return new ManyToOneType(persistentClass);
		}

		/// <summary>
		/// A NHibernate custom type
		/// </summary>
		/// <param name="userTypeClass">a class that implements UserType</param>
		/// <returns></returns>
		public static IType Custom(System.Type userTypeClass)
		{
			if (typeof(ICompositeUserType).IsAssignableFrom(userTypeClass))
			{
				return new CompositeCustomType(userTypeClass, null);
			}
			else
			{
				return new CustomType(userTypeClass, null);
			}
		}


		/// <summary>
		/// Force initialization of a proxy or persistent collection.
		/// </summary>
		/// <param name="proxy">a persistable object, proxy, persistent collection or null</param>
		/// <exception cref="HibernateException">if we can't initialize the proxy at this time, eg. the Session was closed</exception>
		public static void Initialize(object proxy)
		{
			if (proxy == null)
			{
				return;
			}
			else if (proxy is INHibernateProxy)
			{
				NHibernateProxyHelper.GetLazyInitializer((INHibernateProxy) proxy).Initialize();
			}
			else if (proxy is IPersistentCollection)
			{
				((IPersistentCollection) proxy).ForceInitialization();
			}
		}

		/// <summary>
		/// Is the proxy or persistent collection initialized?
		/// </summary>
		/// <param name="proxy">a persistable object, proxy, persistent collection or null</param>
		/// <returns>true if the argument is already initialized, or is not a proxy or collection</returns>
		public static bool IsInitialized(object proxy)
		{
			if (proxy is INHibernateProxy)
			{
				return !NHibernateProxyHelper.GetLazyInitializer((INHibernateProxy) proxy).IsUninitialized;
			}
			else if (proxy is IPersistentCollection)
			{
				return ((IPersistentCollection) proxy).WasInitialized;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Get the true, underlying class of a proxied persistent class. This operation
		/// will initialize a proxy by side-effect.
		/// </summary>
		/// <param name="proxy">a persistable object or proxy</param>
		/// <returns>the true class of the instance</returns>
		public static System.Type GetClass(object proxy)
		{
			if (proxy is INHibernateProxy)
			{
				return NHibernateProxyHelper.GetLazyInitializer((INHibernateProxy) proxy).GetImplementation().GetType();
			}
			else
			{
				return proxy.GetType();
			}
		}

		/*
		/// <summary>
		/// Create a new Blob. The returned object will be initially immutable.
		/// </summary>
		/// <param name="bytes">a byte array</param>
		/// <returns></returns>
		public static Blob CreateBlob(byte[] bytes) {
			return new BlobImpl(bytes);
		}
	
		/// <summary>
		/// Create a new Blob. The returned object will be initially immutable.
		/// </summary>
		/// <param name="stream">a binary stream</param>
		/// <param name="length">the number of bytes in the stream</param>
		/// <returns></returns>
		public static Blob CreateBlob(TextReader stream, int length) {
			return new BlobImpl(stream, length);
		}
		
		/// <summary>
		/// Create a new Blob. The returned object will be initially immutable.
		/// </summary>
		/// <param name="stream">a binary stream</param>
		/// <param name="length">the number of bytes in the stream</param>
		/// <returns></returns>
		public static Blob CreateBlob(BinaryReader stream, int length) 
		{
			return new BlobImpl(stream, length);
		}

		/// <summary>
		/// Create a new Blob. The returned object will be initially immutable.
		/// </summary>
		/// <param name="stream">a binary stream</param>
		/// <returns></returns>
		public static Blob CreateBlob(StreamReader stream) 
		{
			return new BlobImpl( stream, stream.available() );
		}
		
		/// <summary>
		/// Create a new Blob. The returned object will be initially immutable.
		/// </summary>
		/// <param name="stream">a binary stream</param>
		/// <returns></returns>
		public static Blob CreateBlob(BinaryReader stream) 
		{
			return new BlobImpl( stream, stream.available() );
		}

		/// <summary>
		/// Create a new Clob. The returned object will be
		/// initially immutable.
		/// </summary>
		/// <param name="str">a String</param>
		/// <returns></returns>
		public static Clob CreateClob(string str) 
		{
			return new ClobImpl(str);
		}

		/// <summary>
		/// Create a new Clob. The returned object will be initially immutable.
		/// </summary>
		/// <param name="reader">a character stream</param>
		/// <param name="length">the number of characters in the stream</param>
		/// <returns></returns>
		public static Clob CreateClob(TextReader reader, int length) {
			return new ClobImpl(reader, length);
		}
		*/

		/// <summary>
		/// Close an <see cref="IEnumerator" /> obtained from an <see cref="IEnumerable" />
		/// returned by NHibernate immediately, instead of waiting until the session is
		/// closed or disconnected.
		/// </summary>
		public static void Close(IEnumerator enumerator)
		{
			EnumerableImpl hibernateEnumerator = enumerator as EnumerableImpl;
			if (hibernateEnumerator == null)
			{
				throw new ArgumentException("Not a NHibernate enumerator", "enumerator");
			}
			hibernateEnumerator.Dispose();
		}

		/// <summary>
		/// Close an <see cref="IEnumerable" /> returned by NHibernate immediately,
		/// instead of waiting until the session is closed or disconnected.
		/// </summary>
		public static void Close(IEnumerable enumerable)
		{
			EnumerableImpl hibernateEnumerable = enumerable as EnumerableImpl;
			if (hibernateEnumerable == null)
			{
				throw new ArgumentException("Not a NHibernate enumerable", "enumerable");
			}
			hibernateEnumerable.Dispose();
		}
	}
}