using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface IBalanceBaseDao
    {
        #region Method Created By CodeSmith

        void CreateBalance(Balance entity);

        Balance LoadBalance(Int32 id);
  
        IList<Balance> GetAllBalance();
  
        void UpdateBalance(Balance entity);
        
        void DeleteBalance(Int32 id);
    
        void DeleteBalance(Balance entity);
    
        void DeleteBalance(IList<Int32> pkList);
    
        void DeleteBalance(IList<Balance> entityList);    
        #endregion Method Created By CodeSmith
    }
}
