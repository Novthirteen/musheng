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
    public class LanguageBaseMgr : SessionBase, ILanguageBaseMgr
    {
        public ILanguageDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateLanguage(Language entity)
        {
            entityDao.CreateLanguage(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual Language LoadLanguage(String code)
        {
            return entityDao.LoadLanguage(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Language> GetAllLanguage()
        {
            return entityDao.GetAllLanguage();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateLanguage(Language entity)
        {
            entityDao.UpdateLanguage(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLanguage(String code)
        {
            entityDao.DeleteLanguage(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLanguage(Language entity)
        {
            entityDao.DeleteLanguage(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLanguage(IList<String> pkList)
        {
            entityDao.DeleteLanguage(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLanguage(IList<Language> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteLanguage(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
