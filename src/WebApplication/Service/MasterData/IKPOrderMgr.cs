using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IKPOrderMgr : IKPOrderBaseMgr
    {
        #region Customized Methods

        KPOrder LoadKPOrder(Decimal orderId, bool includeDetail);

        #endregion Customized Methods
    }
}



#region Extend Interface




namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IKPOrderMgrE : com.Sconit.Service.MasterData.IKPOrderMgr
    {
        
    }
}

#endregion

#region Extend Interface




namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IKPOrderMgrE : com.Sconit.Service.MasterData.IKPOrderMgr
    {
        
    }
}

#endregion
