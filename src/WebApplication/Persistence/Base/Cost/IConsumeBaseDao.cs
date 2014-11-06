using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface IConsumeBaseDao
    {
        #region Method Created By CodeSmith

        void CreateConsume(Consume entity);

        Consume LoadConsume(Int32 id);
  
        IList<Consume> GetAllConsume();
  
        void UpdateConsume(Consume entity);
        
        void DeleteConsume(Int32 id);
    
        void DeleteConsume(Consume entity);
    
        void DeleteConsume(IList<Int32> pkList);
    
        void DeleteConsume(IList<Consume> entityList);    
        #endregion Method Created By CodeSmith
    }
}
