using System;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICostGroupMgr : ICostGroupBaseMgr
    {
        #region Customized Methods

        CostGroup CheckAndLoadCostGroup(string code);

        #endregion Customized Methods
    }
}
#region Extend Interface
namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ICostGroupMgrE : com.Sconit.Service.Cost.ICostGroupMgr
    {

    }
}
#endregion