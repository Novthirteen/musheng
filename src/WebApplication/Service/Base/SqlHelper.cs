using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Persistence;
using System.Data.SqlClient;
using System.Data;

namespace com.Sconit.Service.Impl
{
    public class SqlHelperMgr : ISqlHelperMgr
    {
        public ISqlHelperDao baseSqlHelperDao { get; set; }

        public int Delete(string sqlString, SqlParameter[] commandParameters)
        {
            return baseSqlHelperDao.Delete(sqlString, commandParameters);
        }

        public int Create(string sqlString, SqlParameter[] commandParameters)
        {
            return baseSqlHelperDao.Create(sqlString, commandParameters);
        }

        public int Update(string sqlString, SqlParameter[] commandParameters)
        {
            return baseSqlHelperDao.Update(sqlString, commandParameters);
        }

        public int SearchCount(string sqlString, SqlParameter[] commandParameters)
        {
            return baseSqlHelperDao.SearchCount(sqlString, commandParameters);
        }

        public int ExecuteSql(string commandText, SqlParameter[] commandParameters)
        {
            return baseSqlHelperDao.ExecuteSql(commandText, commandParameters);
        }

        public int ExecuteStoredProcedure(string commandText, SqlParameter[] commandParameters)
        {
            return baseSqlHelperDao.ExecuteStoredProcedure(commandText, commandParameters);
        }

        public DataSet GetDatasetBySql(string commandText, SqlParameter[] commandParameters)
        {
            return baseSqlHelperDao.GetDatasetBySql(commandText, commandParameters);
        }

        public DataSet GetDatasetByStoredProcedure(string commandText, SqlParameter[] commandParameters)
        {
            return baseSqlHelperDao.GetDatasetByStoredProcedure(commandText, commandParameters);
        }

        public DataSet GetDatasetBySql(string commandText)
        {
            return GetDatasetBySql(commandText, null);
        }
    }
}



#region Extend Class

namespace com.Sconit.Service.Ext.Impl
{
    public partial class SqlHelperMgrE : com.Sconit.Service.Impl.SqlHelperMgr, ISqlHelperMgrE
    {
    }
}

#endregion