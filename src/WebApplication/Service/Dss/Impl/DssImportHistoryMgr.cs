using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Dss;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.Dss;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss.Impl
{
    [Transactional]
    public class DssImportHistoryMgr : DssImportHistoryBaseMgr, IDssImportHistoryMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }



        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<DssImportHistory> GetActiveDssImportHistory(int dssInboundCtrlId)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(DssImportHistory));
            criteria.Add(Expression.Eq("IsActive", true));
            criteria.Add(Expression.Eq("DssInboundCtrl.Id", dssInboundCtrlId));
            criteria.Add(Expression.Lt("ErrorCount", 3));

            return criteriaMgrE.FindAll<DssImportHistory>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public void CreateDssImportHistory(IList<DssImportHistory> dssImportHistoryList)
        {
            if (dssImportHistoryList != null && dssImportHistoryList.Count > 0)
            {
                foreach (var dssImportHistory in dssImportHistoryList)
                {
                    //#region 过滤掉已经导入的文件
                    //DetachedCriteria criteria = DetachedCriteria.For<DssImportHistory>();
                    //criteria.SetProjection(Projections.Count("Id"));
                    //criteria.CreateAlias("DssInboundControl", "dic");
                    //criteria.Add(Expression.Eq("dic.Id", item.DssInboundCtrl.Id));
                    //criteria.Add(Expression.Eq("KeyCode", item.KeyCode));
                    //IList<int> count = this.criteriaMgrE.FindAll<int>(criteria);
                    //if (count[0] == 0) 
                    //{
                    this.CreateDssImportHistory(dssImportHistory);
                    //}
                    //#endregion
                }
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public void UpdateDssImportHistory(IList<DssImportHistory> dssImportHistoryList)
        {
            if (dssImportHistoryList != null && dssImportHistoryList.Count > 0)
            {
                foreach (var dssImportHistory in dssImportHistoryList)
                {
                    this.UpdateDssImportHistory(dssImportHistory);
                }
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public void UpdateDssImportHistory(IList<DssImportHistory> dssImportHistoryList, bool isActive)
        {
            if (dssImportHistoryList != null && dssImportHistoryList.Count > 0)
            {
                foreach (var dssImportHistory in dssImportHistoryList)
                {
                    dssImportHistory.IsActive = isActive;
                    this.UpdateDssImportHistory(dssImportHistory);
                }
            }
        }

        #endregion Customized Methods
    }
}



#region Extend Class


namespace com.Sconit.Service.Ext.Dss.Impl
{
    [Transactional]
    public partial class DssImportHistoryMgrE : com.Sconit.Service.Dss.Impl.DssImportHistoryMgr, IDssImportHistoryMgrE
    {
       
    }
}
#endregion
