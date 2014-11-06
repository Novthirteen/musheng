using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Cost;
using com.Sconit.Entity.Cost;
using com.Sconit.Entity.Exception;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class CostElementMgr : CostElementBaseMgr, ICostElementMgr
    {
        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public CostElement CheckAndLoadCostElement(string code)
        {
            CostElement ce = this.LoadCostElement(code);
            if (ce == null)
            {
                throw new BusinessErrorException("CostElement.Error.CodeNotExist", code);
            }

            return ce;
        }

        #endregion Customized Methods
    }
}
#region Extend Class
namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class CostElementMgrE : com.Sconit.Service.Cost.Impl.CostElementMgr, ICostElementMgrE
    {

    }
}
#endregion