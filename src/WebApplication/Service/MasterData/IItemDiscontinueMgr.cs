using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IItemDiscontinueMgr : IItemDiscontinueBaseMgr
    {
        #region Customized Methods

        IList<ItemDiscontinue> GetItemDiscontinue(Item item, Bom bom, DateTime effectiveDate);

        IList<ItemDiscontinue> GetItemDiscontinue(string itemCode, string bomCode, string effectiveDate);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IItemDiscontinueMgrE : com.Sconit.Service.MasterData.IItemDiscontinueMgr
    {
    }
}

#endregion Extend Interface