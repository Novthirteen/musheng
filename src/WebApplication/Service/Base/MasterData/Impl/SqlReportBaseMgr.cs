using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class SqlReportBaseMgr : SessionBase, ISqlReportBaseMgr
    {
        public ISqlReportDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateSqlReport(SqlReport entity)
        {
            entityDao.CreateSqlReport(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual SqlReport LoadSqlReport(Int32 id)
        {
            return entityDao.LoadSqlReport(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<SqlReport> GetAllSqlReport()
        {
            return entityDao.GetAllSqlReport();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateSqlReport(SqlReport entity)
        {
            entityDao.UpdateSqlReport(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSqlReport(Int32 id)
        {
            entityDao.DeleteSqlReport(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSqlReport(SqlReport entity)
        {
            entityDao.DeleteSqlReport(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSqlReport(IList<Int32> pkList)
        {
            entityDao.DeleteSqlReport(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSqlReport(IList<SqlReport> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteSqlReport(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
