using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IDiffMgr : IDiffBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface IDiffMgrE : com.Sconit.Service.Cost.IDiffMgr
    {
    }
}

#endregion Extend Interface