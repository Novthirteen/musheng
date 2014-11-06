using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IItemBrandBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateItemBrand(ItemBrand entity);

        ItemBrand LoadItemBrand(String code);

        IList<ItemBrand> GetAllItemBrand();
    
        void UpdateItemBrand(ItemBrand entity);

        void DeleteItemBrand(String code);
    
        void DeleteItemBrand(ItemBrand entity);
    
        void DeleteItemBrand(IList<String> pkList);
    
        void DeleteItemBrand(IList<ItemBrand> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
