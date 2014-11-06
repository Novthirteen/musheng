using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Dss;
using com.Sconit.Entity.Dss;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss.Impl
{
    [Transactional]
    public class DssExportHistoryMgr : DssExportHistoryBaseMgr, IDssExportHistoryMgr
    {
        public IDssExportHistoryDetailMgrE dssExportHistoryDetailMgrE { get; set; }

        

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public void CreateDssExportHistory(IList<DssExportHistory> dssExportHistoryList)
        {
            if (dssExportHistoryList != null && dssExportHistoryList.Count > 0)
            {
                foreach (DssExportHistory dssExportHistory in dssExportHistoryList)
                {
                    this.CreateDssExportHistory(dssExportHistory);
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public override void CreateDssExportHistory(DssExportHistory dssExportHistory)
        {
            base.CreateDssExportHistory(dssExportHistory);

            #region Create details
            if (dssExportHistory.DssExportHistoryDetails != null && dssExportHistory.DssExportHistoryDetails.Count > 0)
            {
                foreach (var dssExportHistoryDetail in dssExportHistory.DssExportHistoryDetails)
                {
                    dssExportHistoryDetail.MstrId = dssExportHistory.Id;
                    this.dssExportHistoryDetailMgrE.CreateDssExportHistoryDetail(dssExportHistoryDetail);
                }
            }
            #endregion
        }
        #endregion Customized Methods
    }
}



#region Extend Class
namespace com.Sconit.Service.Ext.Dss.Impl
{
    [Transactional]
    public partial class DssExportHistoryMgrE : com.Sconit.Service.Dss.Impl.DssExportHistoryMgr, IDssExportHistoryMgrE
    {
        
    }
}
#endregion
