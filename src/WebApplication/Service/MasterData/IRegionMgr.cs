using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IRegionMgr : IRegionBaseMgr
    {
        #region Customized Methods

        //IList FindAllRegion();

        Region CheckAndLoadRegion(string regionCode);

        IList<Region> GetRegion(string userCode);

        IList<Region> GetRegion(string userCode,bool includeInactive);

        void CreateRegion(Region entity, User currentUser);

        string GetDefaultInspectLocation(string regionCode);

        string GetDefaultInspectLocation(string regionCode, string defaultInspectLocation);

        string GetDefaultRejectLocation(string regionCode);

        string GetDefaultRejectLocation(string regionCode, string defaultRejectLocation);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IRegionMgrE : com.Sconit.Service.MasterData.IRegionMgr
    {
        
    }
}

#endregion
