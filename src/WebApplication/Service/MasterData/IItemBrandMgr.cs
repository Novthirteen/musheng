using System;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IItemBrandMgr : IItemBrandBaseMgr
    {
        #region Customized Methods

        IList<ItemBrand> GetItemBrandIncludeEmpty();

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IItemBrandMgrE : com.Sconit.Service.MasterData.IItemBrandMgr
    {
    }
}

#endregion Extend Interface