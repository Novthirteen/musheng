using System;
using com.Sconit.Entity.Cost;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICostCenterMgr : ICostCenterBaseMgr
    {
        #region Customized Methods

        CostCenter CheckAndLoadCostCenter(string code);
        
        IList<CostCenter> GetCostCenterList(string costGroupCode);

        #endregion Customized Methods
    }
}

#region Extend Interface
namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ICostCenterMgrE : com.Sconit.Service.Cost.ICostCenterMgr
    {

    }
}
#endregion