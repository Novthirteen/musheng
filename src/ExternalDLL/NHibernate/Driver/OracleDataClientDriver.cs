using System;
using System.Data;
using NHibernate.SqlTypes;
using NHibernate.Impl;
using NHibernate.Engine;

namespace NHibernate.Driver
{
	/// <summary>
	/// A NHibernate Driver for using the Oracle.DataAccess DataProvider
	/// </summary>
	/// <remarks>
	/// Code was contributed by <a href="http://sourceforge.net/users/jemcalgary/">James Mills</a>
	/// on the NHibernate forums in this 
	/// <a href="http://sourceforge.net/forum/message.php?msg_id=2952662">post</a>.
	/// </remarks>
	public class OracleDataClientDriver : ReflectionBasedDriver
	{
		/// <summary>
		/// Initializes a new instance of <see cref="OracleDataClientDriver"/>.
		/// </summary>
		/// <exception cref="HibernateException">
		/// Thrown when the <c>Oracle.DataAccess</c> assembly can not be loaded.
		/// </exception>
		public OracleDataClientDriver() : base(
			"Oracle.DataAccess",
			"Oracle.DataAccess.Client.OracleConnection",
			"Oracle.DataAccess.Client.OracleCommand")
		{
		}

        /// <summary>
        /// Create an instance of <see cref="IBatcher"/> according to the configuration 
        /// and the capabilities of the driver
        /// </summary>
        /// <remarks>
        /// By default, .Net doesn't have any batching capabilities, drivers that does have
        /// batching support need to override this method and return their own batcher.
        /// </remarks>
        public override IBatcher CreateBatcher(ConnectionManager connectionManager)
        {
            if (connectionManager.Factory.IsBatchUpdateEnabled)
                return new OracleDataClientBatchingBatcher(connectionManager);
            else
                return new NonBatchingBatcher(connectionManager);
        }

		/// <summary></summary>
		public override bool UseNamedPrefixInSql
		{
			get { return true; }
		}

		/// <summary></summary>
		public override bool UseNamedPrefixInParameter
		{
			get { return true; }
		}

		/// <summary></summary>
		public override string NamedPrefix
		{
			get { return ":"; }
		}

		/// <remarks>
		/// This adds logic to ensure that a DbType.Boolean parameter is not created since
		/// ODP.NET doesn't support it.
		/// </remarks>
		protected override void InitializeParameter(IDbDataParameter dbParam, string name, SqlType sqlType)
		{
			// if the parameter coming in contains a boolean then we need to convert it 
			// to another type since ODP.NET doesn't support DbType.Boolean
			if (sqlType.DbType == DbType.Boolean)
			{
				sqlType = SqlTypeFactory.Int16;
			}
			base.InitializeParameter(dbParam, name, sqlType);
		}
	}
}