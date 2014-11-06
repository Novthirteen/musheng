using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.MasterData
{
    public interface IItemDiscontinueBaseDao
    {
        #region Method Created By CodeSmith

        void CreateItemDiscontinue(ItemDiscontinue entity);

        ItemDiscontinue LoadItemDiscontinue(Int32 id);
  
        IList<ItemDiscontinue> GetAllItemDiscontinue();
  
        void UpdateItemDiscontinue(ItemDiscontinue entity);
        
        void DeleteItemDiscontinue(Int32 id);
    
        void DeleteItemDiscontinue(ItemDiscontinue entity);
    
        void DeleteItemDiscontinue(IList<Int32> pkList);
    
        void DeleteItemDiscontinue(IList<ItemDiscontinue> entityList);    
        #endregion Method Created By CodeSmith
    }
}
