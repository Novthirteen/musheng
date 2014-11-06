using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IMiscOrderDetailMgr : IMiscOrderDetailBaseMgr
    {
        #region Customized Methods

        IList<MiscOrderDetail> GetMiscOrderDetail(string orderNo);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IMiscOrderDetailMgrE : com.Sconit.Service.MasterData.IMiscOrderDetailMgr
    {
        
    }
}

#endregion
