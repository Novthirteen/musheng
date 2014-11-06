using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Cost;
using com.Sconit.Entity.Cost;
using com.Sconit.Entity.Exception;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class CostCenterMgr : CostCenterBaseMgr, ICostCenterMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public CostCenter CheckAndLoadCostCenter(string code)
        {
            CostCenter cc = this.LoadCostCenter(code);
            if (cc == null)
            {
                throw new BusinessErrorException("CostCenter.Error.CodeNotExist", code);
            }

            return cc;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<CostCenter> GetCostCenterList(string costGroupCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<CostCenter>();
            criteria.Add(Expression.Eq("CostGroup.Code",costGroupCode));
            return criteriaMgrE.FindAll<CostCenter>(criteria);
        }

        #endregion Customized Methods
    }
}
#region Extend Class
namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class CostCenterMgrE : com.Sconit.Service.Cost.Impl.CostCenterMgr, ICostCenterMgrE
    {

    }
}
#endregion