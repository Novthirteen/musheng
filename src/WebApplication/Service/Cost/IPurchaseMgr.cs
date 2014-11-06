using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IPurchaseMgr : IPurchaseBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface IPurchaseMgrE : com.Sconit.Service.Cost.IPurchaseMgr
    {
    }
}

#endregion Extend Interface