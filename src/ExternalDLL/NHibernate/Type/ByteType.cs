using System;
using System.Collections;
using System.Data;
using NHibernate.Engine;
using NHibernate.SqlTypes;

namespace NHibernate.Type
{
	/// <summary>
	/// Maps a <see cref="System.Byte"/> property 
	/// to a <see cref="DbType.Byte"/> column.
	/// </summary>
	[Serializable]
	public class ByteType : ValueTypeType, IDiscriminatorType, IVersionType
	{
		internal ByteType() : base(SqlTypeFactory.Byte)
		{
		}

		public override object Get(IDataReader rs, int index)
		{
			return Convert.ToByte(rs[index]);
		}

		public override object Get(IDataReader rs, string name)
		{
			return Convert.ToByte(rs[name]);
		}

		public override System.Type ReturnedClass
		{
			get { return typeof(byte); }
		}

		public override void Set(IDbCommand cmd, object value, int index)
		{
			((IDataParameter) cmd.Parameters[index]).Value = (byte) value;
		}

		public override string Name
		{
			get { return "Byte"; }
		}

		public override string ObjectToSQLString(object value)
		{
			return value.ToString();
		}

		public virtual object StringToObject(string xml)
		{
			return FromString(xml);
		}

		public override object FromStringValue(string xml)
		{
			return byte.Parse(xml);
		}

		public virtual object Next(object current, ISessionImplementor session)
		{
			return (byte) ((byte) current + (byte) 1);
		}

		public virtual object Seed(ISessionImplementor session)
		{
			return (byte) 1;
		}

		public IComparer Comparator
		{
			get { return Comparer.DefaultInvariant; }
		}
	}
}