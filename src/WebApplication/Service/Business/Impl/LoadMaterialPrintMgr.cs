using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Production;
using com.Sconit.Service.Ext.Business;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity.Customize;
using com.Sconit.Service.Ext.Customize;
using com.Sconit.Service.Ext.Customize.Impl;
using com.Sconit.Service.Ext.Report;

namespace com.Sconit.Service.Business.Impl
{
    public class LoadMaterialPrintMgr : AbstractBusinessMgr
    {
        public IReportMgrE reportMgrE { get; set; }
        public ISetBaseMgrE setBaseMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
            setBaseMgrE.FillResolverByOrder(resolver);
            resolver.PrintUrl = reportMgrE.WriteToFile("ProdutLineFeedSeqence.xls", resolver.OrderNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolver"></param>
        protected override void GetDetail(Resolver resolver)
        {
        }

        protected override void SetDetail(Resolver resolver)
        {
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
           
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void ExecutePrint(Resolver resolver)
        {
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void GetReceiptNotes(Resolver resolver)
        {
        }
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class LoadMaterialPrintMgrE : com.Sconit.Service.Business.Impl.LoadMaterialPrintMgr, IBusinessMgrE
    {

    }
}

#endregion
