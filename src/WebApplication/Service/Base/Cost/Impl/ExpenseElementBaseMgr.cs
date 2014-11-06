using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.Cost;
using com.Sconit.Persistence.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class ExpenseElementBaseMgr : SessionBase, IExpenseElementBaseMgr
    {
        public IExpenseElementDao entityDao { get; set; }

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateExpenseElement(ExpenseElement entity)
        {
            entityDao.CreateExpenseElement(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual ExpenseElement LoadExpenseElement(String code)
        {
            return entityDao.LoadExpenseElement(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ExpenseElement> GetAllExpenseElement()
        {
            return entityDao.GetAllExpenseElement();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateExpenseElement(ExpenseElement entity)
        {
            entityDao.UpdateExpenseElement(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteExpenseElement(String code)
        {
            entityDao.DeleteExpenseElement(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteExpenseElement(ExpenseElement entity)
        {
            entityDao.DeleteExpenseElement(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteExpenseElement(IList<String> pkList)
        {
            entityDao.DeleteExpenseElement(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteExpenseElement(IList<ExpenseElement> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteExpenseElement(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
