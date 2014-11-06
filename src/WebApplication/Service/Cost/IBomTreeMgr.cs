using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IBomTreeMgr : IBomTreeBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface IBomTreeMgrE : com.Sconit.Service.Cost.IBomTreeMgr
    {
    }
}

#endregion Extend Interface