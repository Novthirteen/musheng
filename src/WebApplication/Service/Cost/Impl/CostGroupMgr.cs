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
    public class CostGroupMgr : CostGroupBaseMgr, ICostGroupMgr
    {

        public ICostCenterMgr costCenterMgrE;

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public CostGroup CheckAndLoadCostGroup(string code)
        {
            CostGroup cg = this.LoadCostGroup(code);
            if (cg == null)
            {
                throw new BusinessErrorException("CostGroup.Error.CodeNotExist", code);
            }

            return cg;
        }

        [Transaction(TransactionMode.Unspecified)]
        public override void DeleteCostGroup(string code)
        {
            IList<CostCenter> costCenterList = costCenterMgrE.GetCostCenterList(code);
            if (costCenterList != null && costCenterList.Count > 0)
            {
                costCenterMgrE.DeleteCostCenter(costCenterList);
            }

            base.Delete(code);
        }

        #endregion Customized Methods
    }
}

#region Extend Class
namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class CostGroupMgrE : com.Sconit.Service.Cost.Impl.CostGroupMgr, ICostGroupMgrE
    {

    }
}
#endregion