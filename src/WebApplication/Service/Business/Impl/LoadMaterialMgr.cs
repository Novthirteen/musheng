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

namespace com.Sconit.Service.Business.Impl
{
    public class LoadMaterialMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IProductLineInProcessLocationDetailMgrE productLineInProcessLocationDetailMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderLocTransMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
            if (resolver.CodePrefix != BusinessConstants.CODE_PREFIX_WORK_ORDER)
            {
                throw new BusinessErrorException("LoadMaterial.ScanOrderNo.First");
                // return;
            }

            setBaseMgrE.FillResolverByOrder(resolver);

            resolver.Input = null;
            if (resolver.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                throw new BusinessErrorException("Flow.ShipReturn.Error.FlowTypeIsNotProduction", resolver.OrderType);
            }

            if (resolver.Transformers == null || resolver.Transformers.Count != 2)
            {
                resolver.Transformers = new List<Transformer>();
                resolver.Transformers.Add(new Transformer());
                resolver.Transformers.Add(new Transformer());
            }
            resolver.Transformers[0] = new Transformer();
            resolver.Transformers[1] = new Transformer();
            resolver.Result = languageMgrE.TranslateMessage("LoadMaterial.Order.Message", resolver.UserCode, resolver.OrderNo, resolver.FlowCode);
            this.SetDetail(resolver);
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
            if (resolver.CodePrefix == null || resolver.CodePrefix == string.Empty)
            {
                throw new BusinessErrorException("LoadMaterial.ScanOrderNo.First");
            }

            string seq = null;
            if (resolver.Input != null)
            {
                Hu hu = huMgrE.CheckAndLoadHu(resolver.Input.Trim());
                TransformerDetail oldTransformerDetail = TransformerHelper.ConvertHuToTransformerDetail(hu);
                oldTransformerDetail.Position = resolver.Transformers[0].TransformerDetails[0].Position;
                if (resolver.Transformers[1].TransformerDetails == null)
                {
                    resolver.Transformers[1].TransformerDetails = new List<TransformerDetail>();
                }
                oldTransformerDetail.Sequence = setDetailMgrE.FindMaxSeq(resolver.Transformers[1]) + 1;
                resolver.Transformers[1].TransformerDetails.Add(oldTransformerDetail);
                seq = oldTransformerDetail.Position;
            }

            ProdutLineFeedSeqence produtLineFeedSeqence =
                productLineInProcessLocationDetailMgrE.CheckAndGetNextProdutLineFeedSeqence(resolver.OrderNo, seq, resolver.Input);

            resolver.Transformers[0].TransformerDetails = new List<TransformerDetail>();
            if (produtLineFeedSeqence == null)
            {
                resolver.Result = languageMgrE.TranslateMessage("LoadMaterial.Business.Warning.LastHu", resolver.UserCode);
            }
            else
            {
                TransformerDetail transformerDetail = TransformerHelper.ConvertProdutLineFeedSeqenceToTransformerDetail(produtLineFeedSeqence);

                resolver.Transformers[0].TransformerDetails.Add(transformerDetail);
            }
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMERDETAIL;
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            if (resolver.Transformers == null || resolver.Transformers.Count != 2
                || resolver.Transformers[1].TransformerDetails == null || resolver.Transformers[1].TransformerDetails.Count == 0)
            {
                throw new BusinessErrorException("LoadMaterial.Error.No.Details");
            }

            IDictionary<string, string> seqHuIdDic = new Dictionary<string, string>();

            foreach (TransformerDetail transformerDetail in resolver.Transformers[1].TransformerDetails)
            {
                seqHuIdDic.Add(transformerDetail.Position, transformerDetail.HuId);
            }

            productLineInProcessLocationDetailMgrE.RawMaterialIn4Order(resolver.OrderNo, seqHuIdDic, userMgrE.LoadUser(resolver.UserCode));

            resolver.Result = languageMgrE.TranslateMessage("LoadMaterial.Successfully", resolver.UserCode, resolver.Code);

            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMERDETAIL;
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
            executeMgrE.CancelRepackOperation(resolver);
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
    public partial class LoadMaterialMgrE : com.Sconit.Service.Business.Impl.LoadMaterialMgr, IBusinessMgrE
    {

    }
}

#endregion
