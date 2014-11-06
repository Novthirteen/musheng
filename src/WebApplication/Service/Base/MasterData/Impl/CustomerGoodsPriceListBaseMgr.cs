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
    public class CustomerGoodsPriceListBaseMgr : SessionBase, ICustomerGoodsPriceListBaseMgr
    {
        public ICustomerGoodsPriceListDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCustomerGoodsPriceList(CustomerGoodsPriceList entity)
        {
            entityDao.CreateCustomerGoodsPriceList(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual CustomerGoodsPriceList LoadCustomerGoodsPriceList(String code)
        {
            return entityDao.LoadCustomerGoodsPriceList(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CustomerGoodsPriceList> GetAllCustomerGoodsPriceList()
        {
            return entityDao.GetAllCustomerGoodsPriceList(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CustomerGoodsPriceList> GetAllCustomerGoodsPriceList(bool includeInactive)
        {
            return entityDao.GetAllCustomerGoodsPriceList(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCustomerGoodsPriceList(CustomerGoodsPriceList entity)
        {
            entityDao.UpdateCustomerGoodsPriceList(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCustomerGoodsPriceList(String code)
        {
            entityDao.DeleteCustomerGoodsPriceList(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCustomerGoodsPriceList(CustomerGoodsPriceList entity)
        {
            entityDao.DeleteCustomerGoodsPriceList(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCustomerGoodsPriceList(IList<String> pkList)
        {
            entityDao.DeleteCustomerGoodsPriceList(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCustomerGoodsPriceList(IList<CustomerGoodsPriceList> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCustomerGoodsPriceList(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
