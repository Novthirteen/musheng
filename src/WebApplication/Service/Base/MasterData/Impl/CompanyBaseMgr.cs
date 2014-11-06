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
    public class CompanyBaseMgr : SessionBase, ICompanyBaseMgr
    {
        public ICompanyDao entityDao { get; set; }
        
        

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCompany(Company entity)
        {
            entityDao.CreateCompany(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual Company LoadCompany(String code)
        {
            return entityDao.LoadCompany(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Company> GetAllCompany()
        {
            return entityDao.GetAllCompany(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Company> GetAllCompany(bool includeInactive)
        {
            return entityDao.GetAllCompany(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCompany(Company entity)
        {
            entityDao.UpdateCompany(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCompany(String code)
        {
            entityDao.DeleteCompany(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCompany(Company entity)
        {
            entityDao.DeleteCompany(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCompany(IList<String> pkList)
        {
            entityDao.DeleteCompany(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCompany(IList<Company> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCompany(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}


