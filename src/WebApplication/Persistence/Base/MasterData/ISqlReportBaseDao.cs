using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.MasterData
{
    public interface ISqlReportBaseDao
    {
        #region Method Created By CodeSmith

        void CreateSqlReport(SqlReport entity);

        SqlReport LoadSqlReport(Int32 id);
  
        IList<SqlReport> GetAllSqlReport();
  
        void UpdateSqlReport(SqlReport entity);
        
        void DeleteSqlReport(Int32 id);
    
        void DeleteSqlReport(SqlReport entity);
    
        void DeleteSqlReport(IList<Int32> pkList);
    
        void DeleteSqlReport(IList<SqlReport> entityList);    
        #endregion Method Created By CodeSmith
    }
}
