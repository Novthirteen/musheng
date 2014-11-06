using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IRawIOBMgr : IRawIOBBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface IRawIOBMgrE : com.Sconit.Service.Cost.IRawIOBMgr
    {
    }
}

#endregion Extend Interface