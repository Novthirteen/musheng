using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using Castle.Services.Transaction;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Report;

namespace com.Sconit.Service.Business.Impl
{
    public class DevanningMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IRepackMgrE repackMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IReportMgrE reportMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
        }

        protected override void GetDetail(Resolver resolver)
        {
        }

        protected override void SetDetail(Resolver resolver)
        {
            setDetailMgrE.MatchRepack(resolver);
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            this.CreateDevanning(resolver);
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
            executeMgrE.CancelRepackOperation(resolver);
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void ExecutePrint(Resolver resolver)
        {
            Repack repack = repackMgrE.LoadRepack(resolver.Code);
            Hu hu = repack.RepackDetails.Where(r => r.IOType == BusinessConstants.IO_TYPE_OUT).OrderBy(r => r.Id).Last().Hu;
            IList<Hu> huList = new List<Hu>();
            huList.Add(hu);
            IList<object> huDetailObj = new List<object>();
            huDetailObj.Add(huList);
            huDetailObj.Add(resolver.UserCode);
            resolver.PrintUrl = reportMgrE.WriteToFile(huList[0].HuTemplate, huDetailObj, huList[0].HuTemplate);
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void GetReceiptNotes(Resolver resolver)
        {
        }
        /// <summary>
        /// 拆箱
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        [Transaction(TransactionMode.Unspecified)]
        public void CreateDevanning(Resolver resolver)
        {
            IList<RepackDetail> repackDetailList = executeMgrE.ConvertTransformerListToRepackDetail(resolver.Transformers);
            if (repackDetailList.Count == 0)
            {
                throw new BusinessErrorException("MasterData.Inventory.Repack.Error.RepackDetailEmpty");
            }
            //KSS 客户化需求, 拆箱并上架
            foreach (RepackDetail repackDetail in repackDetailList)
            {
                repackDetail.StorageBinCode = resolver.BinCode;
            }
            decimal inQty = repackDetailList.Where(r => r.IOType == BusinessConstants.IO_TYPE_IN).Sum(r => r.Qty);
            decimal outQty = repackDetailList.Where(r => r.IOType == BusinessConstants.IO_TYPE_OUT).Sum(r => r.Qty);

            Repack repack = repackMgrE.CreateDevanning(repackDetailList, this.userMgrE.LoadUser(resolver.UserCode, false, true));
            resolver.Code = repack.RepackNo;
            resolver.Result = languageMgrE.TranslateMessage("MasterData.Inventory.Devanning.Successfully", resolver.UserCode, resolver.Transformers[0].TransformerDetails[0].HuId);
            resolver.Transformers = null;

            if (inQty != outQty)
            {
                Hu hu = repack.RepackDetails.Where(r => r.IOType == BusinessConstants.IO_TYPE_OUT).OrderBy(r => r.Id).Last().Hu;

                IList<Hu> huList = new List<Hu>();
                huList.Add(hu);
                IList<object> huDetailObj = new List<object>();
                huDetailObj.Add(huList);
                huDetailObj.Add(resolver.UserCode);
                resolver.PrintUrl = reportMgrE.WriteToFile(huList[0].HuTemplate, huDetailObj, huList[0].HuTemplate);
            }
        }
    }
}



#region Extend Class

namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class DevanningMgrE : com.Sconit.Service.Business.Impl.DevanningMgr, IBusinessMgrE
    {


    }
}

#endregion
