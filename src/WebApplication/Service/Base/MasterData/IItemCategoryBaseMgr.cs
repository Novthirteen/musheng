using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IItemCategoryBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateItemCategory(ItemCategory entity);

        ItemCategory LoadItemCategory(String code);

        IList<ItemCategory> GetAllItemCategory();
    
        void UpdateItemCategory(ItemCategory entity);

        void DeleteItemCategory(String code);
    
        void DeleteItemCategory(ItemCategory entity);
    
        void DeleteItemCategory(IList<String> pkList);
    
        void DeleteItemCategory(IList<ItemCategory> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
