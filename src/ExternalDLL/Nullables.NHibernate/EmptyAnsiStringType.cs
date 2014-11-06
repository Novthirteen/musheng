using System;
using System.Data;

using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.UserTypes;

namespace Nullables.NHibernate
{
	/// <summary>
	/// An <see cref="IUserType"/> that reads a <see langword="null" /> value from an <c>ansi string</c>
	/// column in the database as a <see cref="String.Empty">String.Empty</see>
	/// and writes a <see cref="String.Empty">String.Empty</see> to the database
	/// as <see langword="null" />.
	/// </summary>
	/// <remarks>
	/// This is intended to help with Windows Forms DataBinding and the problems associated
	/// with binding a null value.  See <a href="http://jira.nhibernate.org/browse/NH-279">
	/// NH-279</a> for the origin of this code.
	/// </remarks>
	public class EmptyAnsiStringType : IUserType
	{
		private AnsiStringType stringType;

		public EmptyAnsiStringType()
		{
			stringType = (AnsiStringType) NHibernateUtil.AnsiString;
		}

		#region IUserType Members

		bool IUserType.Equals(object x, object y)
		{
			if (x == y)
			{
				return true;
			}
			if ((x == null) || (y == null))
			{
				return false;
			}
			return x.Equals(y);
		}

		public int GetHashCode(object x)
		{
			return (x == null) ? 0 : x.GetHashCode();
		}

		public SqlType[] SqlTypes
		{
			get { return new SqlType[] {stringType.SqlType}; }
		}

		public DbType[] DbTypes
		{
			get { return new DbType[] {stringType.SqlType.DbType}; }
		}

		public object DeepCopy(object value)
		{
			return value;
		}

		public void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			if (value == null || value.Equals(string.Empty))
			{
				((IDbDataParameter) cmd.Parameters[index]).Value = DBNull.Value;
			}
			else
			{
				stringType.Set(cmd, value, index);
			}
		}

		public Type ReturnedType
		{
			get { return typeof(string); }
		}

		public object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			int index = rs.GetOrdinal(names[0]);

			if (rs.IsDBNull(index))
			{
				return string.Empty;
			}
			else
			{
				return rs[index];
			}
		}

		public bool IsMutable
		{
			get { return false; }
		}

		public object Replace(object original, object target, object owner)
		{
			return original;
		}

		public object Assemble(object cached, object owner)
		{
			return cached;
		}

		public object Disassemble(object value)
		{
			return value;
		}

		#endregion
	}
}