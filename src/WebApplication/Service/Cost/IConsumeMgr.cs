using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IConsumeMgr : IConsumeBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface IConsumeMgrE : com.Sconit.Service.Cost.IConsumeMgr
    {
    }
}

#endregion Extend Interface