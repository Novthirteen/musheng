using System;
using System.Collections.Generic;
using com.Sconit.Entity.Dss;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss
{
    public interface IDssObjectMappingMgr : IDssObjectMappingBaseMgr
    {
        #region Customized Methods

        IList<DssObjectMapping> GetDssObjectMapping(string entity, string externalSystem, string code, string externalEntity);

        string GetExternalCode(string entity, string externalSystem, string code, string defaultResult);

        string GetExternalCode(string entity, string externalSystem, string code, string externalEntity, string defaultResult);

        #endregion Customized Methods
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.Dss
{
    public partial interface IDssObjectMappingMgrE : com.Sconit.Service.Dss.IDssObjectMappingMgr
    {
        
    }
}

#endregion
