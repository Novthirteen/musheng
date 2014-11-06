using System;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICostElementMgr : ICostElementBaseMgr
    {
        #region Customized Methods

        CostElement CheckAndLoadCostElement(string code);

        #endregion Customized Methods
    }
}
#region Extend Interface
namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ICostElementMgrE : com.Sconit.Service.Cost.ICostElementMgr
    {

    }
}
#endregion