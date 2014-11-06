using System;
using System.Data;
using NHibernate.SqlTypes;

namespace NHibernate.Type
{
	/// <summary>
	/// Common base class for <see cref="CharType" /> and <see cref="AnsiCharType" />.
	/// </summary>
	[Serializable]
	public abstract class BaseCharType : ValueTypeType, IDiscriminatorType
	{
		public BaseCharType(SqlType sqlType) : base(sqlType)
		{
		}

		public override object Get(IDataReader rs, int index)
		{
			string dbValue = Convert.ToString(rs[index]);
			if (dbValue == null)
			{
				return null;
			}
			else
			{
				return dbValue[0];
			}
		}

		public override object Get(IDataReader rs, string name)
		{
			return Get(rs, rs.GetOrdinal(name));
		}

		public override System.Type ReturnedClass
		{
			get { return typeof(char); }
		}

		public override void Set(IDbCommand cmd, object value, int index)
		{
			((IDataParameter) cmd.Parameters[index]).Value = (char) value;
		}

		public override string ObjectToSQLString(object value)
		{
			return '\'' + value.ToString() + '\'';
		}

		public virtual object StringToObject(string xml)
		{
			if (xml.Length != 1)
			{
				throw new MappingException("multiple or zero characters found parsing string");
			}
			return xml[0];
		}

		public override object FromStringValue(string xml)
		{
			return xml[0];
		}
	}
}