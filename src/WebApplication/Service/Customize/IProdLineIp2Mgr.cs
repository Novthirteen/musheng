using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize
{
    public interface IProdLineIp2Mgr : IProdLineIp2BaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Customize
{
    public partial interface IProdLineIp2MgrE : com.Sconit.Service.Customize.IProdLineIp2Mgr
    {
    }
}

#endregion Extend Interface