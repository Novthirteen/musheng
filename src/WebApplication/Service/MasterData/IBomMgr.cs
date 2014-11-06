using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IBomMgr : IBomBaseMgr
    {
        #region Customized Methods

        Bom LoadBom(String code, bool includeDetail);

        string FindBomCode(string itemCode);

        string FindBomCode(Item item);

        Bom CheckAndLoadBom(string bomCode);

        #endregion Customized Methods
    }
}





#region Extend Interface




namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IBomMgrE : com.Sconit.Service.MasterData.IBomMgr
    {
        
    }
}

#endregion
