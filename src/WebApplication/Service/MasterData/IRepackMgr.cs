using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IRepackMgr : IRepackBaseMgr
    {
        #region Customized Methods

        Repack LoadRepack(String repackNo, bool includeDetail);

        Repack CreateRepack(IList<RepackDetail> repackDetailList, User user);

        Repack CreateDevanning(IList<RepackDetail> repackDetailList, User user);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IRepackMgrE : com.Sconit.Service.MasterData.IRepackMgr
    {
        
    }
}

#endregion
