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
    public class IndustryBaseMgr : SessionBase, IIndustryBaseMgr
    {
        public IIndustryDao entityDao { get; set; }


        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateIndustry(Industry entity)
        {
            entityDao.CreateIndustry(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual Industry LoadIndustry(String code)
        {
            return entityDao.LoadIndustry(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Industry> GetAllIndustry()
        {
            return entityDao.GetAllIndustry(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Industry> GetAllIndustry(bool includeInactive)
        {
            return entityDao.GetAllIndustry(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateIndustry(Industry entity)
        {
            entityDao.UpdateIndustry(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteIndustry(String code)
        {
            entityDao.DeleteIndustry(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteIndustry(Industry entity)
        {
            entityDao.DeleteIndustry(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteIndustry(IList<String> pkList)
        {
            entityDao.DeleteIndustry(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteIndustry(IList<Industry> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteIndustry(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}




