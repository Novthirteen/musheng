using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ISalesMgr : ISalesBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ISalesMgrE : com.Sconit.Service.Cost.ISalesMgr
    {
    }
}

#endregion Extend Interface