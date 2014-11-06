using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ICustomerGoodsPriceListBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateCustomerGoodsPriceList(CustomerGoodsPriceList entity);

        CustomerGoodsPriceList LoadCustomerGoodsPriceList(String code);

        IList<CustomerGoodsPriceList> GetAllCustomerGoodsPriceList();
    
        IList<CustomerGoodsPriceList> GetAllCustomerGoodsPriceList(bool includeInactive);
      
        void UpdateCustomerGoodsPriceList(CustomerGoodsPriceList entity);

        void DeleteCustomerGoodsPriceList(String code);
    
        void DeleteCustomerGoodsPriceList(CustomerGoodsPriceList entity);
    
        void DeleteCustomerGoodsPriceList(IList<String> pkList);
    
        void DeleteCustomerGoodsPriceList(IList<CustomerGoodsPriceList> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
