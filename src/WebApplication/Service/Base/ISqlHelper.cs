using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace com.Sconit.Service
{
    public interface ISqlHelperMgr
    {
        int Create(string sql, SqlParameter[] commandParameters);

        int Update(string sql, SqlParameter[] commandParameters);

        int Delete(string sql, SqlParameter[] commandParameters);

        int SearchCount(string sql, SqlParameter[] commandParameters);

        int ExecuteSql(string commandText, SqlParameter[] commandParameters);

        int ExecuteStoredProcedure(string commandText, SqlParameter[] commandParameters);

        DataSet GetDatasetBySql(string commandText, SqlParameter[] commandParameters);

        DataSet GetDatasetByStoredProcedure(string commandText, SqlParameter[] commandParameters);

        DataSet GetDatasetBySql(string commandText);
    }
}

#region

namespace com.Sconit.Service.Ext
{
    public partial interface ISqlHelperMgrE : ISqlHelperMgr
    {

    }
}

#endregion