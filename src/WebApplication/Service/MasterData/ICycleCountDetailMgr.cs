
//TODO: Add other using statements here.

using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
namespace com.Sconit.Service.MasterData
{
    public interface ICycleCountDetailMgr : ICycleCountDetailBaseMgr
    {
        #region Customized Methods

        IList<CycleCountDetail> GetCycleCountDetail(string orderNo);

        void CreateCycleCountDetail(CycleCount cycleCount, IList<CycleCountDetail> cycleCountDetailList);

        #endregion Customized Methods
    }
}


#region Extend Interface


namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ICycleCountDetailMgrE : com.Sconit.Service.MasterData.ICycleCountDetailMgr
    {

    }
}

#endregion