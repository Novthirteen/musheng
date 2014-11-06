using com.Sconit.Service.Ext.MasterData;
using System.Collections;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IItemCategoryMgr : IItemCategoryBaseMgr
    {
        #region Customized Methods

        IList GetItemCategory(string code, string desc1);
        IList GetItemCategory(string code, string desc1, string desc2);


        IList<ItemCategory> GetCacheAllItemCategory();

        #endregion Customized Methods
    }
}

#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IItemCategoryMgrE : com.Sconit.Service.MasterData.IItemCategoryMgr
    {

    }
}

#endregion
