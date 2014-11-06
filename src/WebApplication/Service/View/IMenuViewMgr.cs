using System;
using com.Sconit.Entity.View;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface IMenuViewMgr : IMenuViewBaseMgr
    {
        #region Customized Methods

        IList<MenuView> GetMenuViewByIsActive(bool isActive);
        MenuView GetMenuView(string key);

        #endregion Customized Methods
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.View
{
    public partial interface IMenuViewMgrE : com.Sconit.Service.View.IMenuViewMgr
    {
        
    }
}

#endregion
