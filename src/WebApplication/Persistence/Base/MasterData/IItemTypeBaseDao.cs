using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.MasterData
{
    public interface IItemTypeBaseDao
    {
        #region Method Created By CodeSmith

        void CreateItemType(ItemType entity);

        ItemType LoadItemType(String code);
  
        IList<ItemType> GetAllItemType();
  
        void UpdateItemType(ItemType entity);
        
        void DeleteItemType(String code);
    
        void DeleteItemType(ItemType entity);
    
        void DeleteItemType(IList<String> pkList);
    
        void DeleteItemType(IList<ItemType> entityList);    
        #endregion Method Created By CodeSmith
    }
}
