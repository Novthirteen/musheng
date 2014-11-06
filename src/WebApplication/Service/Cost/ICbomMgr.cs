using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICbomMgr : ICbomBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ICbomMgrE : com.Sconit.Service.Cost.ICbomMgr
    {
    }
}

#endregion Extend Interface