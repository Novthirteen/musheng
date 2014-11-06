using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class CycleCountResultMgr : CycleCountResultBaseMgr, ICycleCountResultMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }


        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<CycleCountResult> GetCycleCountResult(string orderNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For<CycleCountResult>();
            criteria.CreateAlias("CycleCount", "cc");
            criteria.Add(Expression.Eq("cc.Code", orderNo));

            return this.criteriaMgrE.FindAll<CycleCountResult>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public void ClearOldCycleCountResult(string orderNo)
        {
            IList<CycleCountResult> cycleCountResultList = this.GetCycleCountResult(orderNo);
            if (cycleCountResultList != null && cycleCountResultList.Count > 0)
            {
                foreach (var item in cycleCountResultList)
                {
                    this.DeleteCycleCountResult(item.Id);
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateCycleCountResult(IList<CycleCountResult> cycleCountResultList)
        {
            if (cycleCountResultList != null && cycleCountResultList.Count > 0)
            {
                foreach (var item in cycleCountResultList)
                {
                    this.CreateCycleCountResult(item);
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void SaveCycleCountResult(string orderNo, IList<CycleCountResult> cycleCountResultList)
        {
            this.ClearOldCycleCountResult(orderNo);
            this.CreateCycleCountResult(cycleCountResultList);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateCycleCountResult(IList<CycleCountResult> cycleCountResultList)
        {
            if (cycleCountResultList != null && cycleCountResultList.Count > 0)
            {
                foreach (var item in cycleCountResultList)
                {
                    this.UpdateCycleCountResult(item);
                }
            }
        }

        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class CycleCountResultMgrE : com.Sconit.Service.MasterData.Impl.CycleCountResultMgr, ICycleCountResultMgrE
    {
        
    }
}
#endregion
