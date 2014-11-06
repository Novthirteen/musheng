using com.Sconit.Service.Ext.MasterData;


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
    public class IndustryMgr : IndustryBaseMgr, IIndustryMgr
    {
        

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}




#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class IndustryMgrE : com.Sconit.Service.MasterData.Impl.IndustryMgr, IIndustryMgrE
    {
       
    }
}
#endregion
