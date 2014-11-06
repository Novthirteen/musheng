using Castle.Services.Transaction;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using System.Collections.Generic;
using com.Sconit.Service.Ext.Business;
using com.Sconit.Service.Ext.Customize;
using com.Sconit.Entity.Customize;
using com.Sconit.Utility;

namespace com.Sconit.Service.Business.Impl
{
    public class ReturnMaterialMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IProductLineInProcessLocationDetailMgrE productLineInProcessLocationDetailMgrE { get; set; }
        public IProductLineFacilityMgrE productLineFacilityMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
            resolver.Code = resolver.Input;
            if (resolver.CodePrefix == BusinessConstants.BARCODE_HEAD_FLOW_FACILITY)
            {
                ProductLineFacility productLineFacility = productLineFacilityMgrE.CheckAndLoadProductLineFacility(resolver.Input);
                Flow flow = flowMgrE.LoadFlow(productLineFacility.ProductLine);
                setBaseMgrE.FillResolverByFlow(resolver, flow);
                resolver.Code = productLineFacility.Code;
                resolver.FlowFacility = productLineFacility.Code;
                //resolver.CodePrefix = BusinessConstants.BARCODE_HEAD_FLOW_FACILITY;
            }
            else if (resolver.CodePrefix == BusinessConstants.BARCODE_HEAD_FLOW)
            {
                setBaseMgrE.FillResolverByFlow(resolver);
            }
            else
            {
                throw new BusinessErrorException("ReturnMaterial.BarCode.Type.Error");
            }

            IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList =
                productLineInProcessLocationDetailMgrE.GetProductLineInProcessLocationDetail(resolver.FlowCode, resolver.FlowFacility, null, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, null);
            foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
            {

                Hu hu = huMgrE.LoadHu(productLineInProcessLocationDetail.HuId);
                TransformerDetail transformerDetail = TransformerHelper.ConvertHuToTransformerDetail(hu);
                transformerDetail.CurrentQty = productLineInProcessLocationDetail.RemainQty;
                resolver.AddTransformerDetail(transformerDetail);
            }
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMERDETAIL;
        }

        protected override void GetDetail(Resolver resolver)
        {

        }

        protected override void SetDetail(Resolver resolver)
        {
            Hu hu = huMgrE.CheckAndLoadHu(resolver.Input.Trim());
            TransformerDetail transformerDetail = TransformerHelper.ConvertHuToTransformerDetail(hu);
            decimal? currentQty = this.productLineInProcessLocationDetailMgrE.GetPLIpQty(hu.HuId);
            transformerDetail.CurrentQty = currentQty.HasValue ? currentQty.Value : 0M;
            resolver.AddTransformerDetail(transformerDetail);
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMERDETAIL;
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            if (resolver.CodePrefix == BusinessConstants.BARCODE_HEAD_FLOW_FACILITY)
            {
                productLineInProcessLocationDetailMgrE.RawMaterialReturnByProductLineFacility(resolver.Code, null, userMgrE.LoadUser(resolver.UserCode));
            }
            else if (resolver.CodePrefix == BusinessConstants.BARCODE_HEAD_FLOW)
            {
                productLineInProcessLocationDetailMgrE.RawMaterialReturnByProductLine(resolver.Code, null, userMgrE.LoadUser(resolver.UserCode));
            }
            else if (resolver.CodePrefix == null || resolver.CodePrefix == string.Empty)
            {
                IList<string> huList = new List<string>();
                if (resolver.Transformers != null)
                {
                    foreach (Transformer transformer in resolver.Transformers)
                    {
                        if (transformer.TransformerDetails != null)
                        {
                            foreach (TransformerDetail transformerDetail in transformer.TransformerDetails)
                            {
                                if (transformerDetail.CurrentQty > 0)
                                {
                                    huList.Add(transformerDetail.HuId);
                                }
                            }
                        }
                    }
                }

                productLineInProcessLocationDetailMgrE.RawMaterialReturnByHuId(huList, userMgrE.LoadUser(resolver.UserCode));
            }
            else
            {
                throw new BusinessErrorException("ReturnMaterial.BarCode.Type.Error");
            }
            resolver.Result = languageMgrE.TranslateMessage("ReturnMaterial.Successfully", resolver.UserCode);
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMERDETAIL;
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
            executeMgrE.CancelOperation(resolver);
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
    public partial class ReturnMaterialMgrE : com.Sconit.Service.Business.Impl.ReturnMaterialMgr, IBusinessMgrE
    {


    }
}

#endregion
