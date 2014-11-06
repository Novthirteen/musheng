using System;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IItemTypeMgr : IItemTypeBaseMgr
    {
        #region Customized Methods

        IList<ItemType> GetItemType(int level, bool includeEmpty);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IItemTypeMgrE : com.Sconit.Service.MasterData.IItemTypeMgr
    {
    }
}

#endregion Extend Interface