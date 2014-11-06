using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class MenuIndustryMgr : MenuIndustryBaseMgr, IMenuIndustryMgr
    {
       

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class MenuIndustryMgrE : com.Sconit.Service.MasterData.Impl.MenuIndustryMgr, IMenuIndustryMgrE
    {
    }
}

#endregion Extend Class