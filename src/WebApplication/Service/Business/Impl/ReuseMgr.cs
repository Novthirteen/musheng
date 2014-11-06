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
using com.Sconit.Entity.View;

namespace com.Sconit.Service.Business.Impl
{
    public class ReuseMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IOrderMgrE orderMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderLocationTransactionMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }

        

        protected override void SetBaseInfo(Resolver resolver)
        {
            if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_FLOW)
            {
                setBaseMgrE.FillResolverByFlow(resolver);
                if (resolver.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                {
                    throw new BusinessErrorException("Flow.Error.FlowTypeIsNotProductLine", resolver.OrderType);
                }
            }
            else
            {
                throw new BusinessErrorException("Common.Business.Error.BarCodeInvalid");
            }
        }

        protected override void GetDetail(Resolver resolver)
        {

        }

        protected override void SetDetail(Resolver resolver)
        {
            List<string> flowTypes = new List<string>();
            flowTypes.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION);
            bool isHaslocationLotDetail = locationLotDetailMgrE.CheckHuLocationExist(resolver.Input);
            if (!isHaslocationLotDetail)
            {
                throw new BusinessErrorException("Hu.Error.NoInventory", resolver.Input);
            }
            Hu hu = huMgrE.CheckAndLoadHu(resolver.Input);
            FlowView flowView = null;
            //如果是扫描Bin,根据Hu和Bin匹配出flow
            if (resolver.CodePrefix == null || resolver.CodePrefix.Trim() == string.Empty)
            {
                //确定flow和flowView
                flowView = flowMgrE.CheckAndLoadFlowView(null, resolver.UserCode, string.Empty, null, hu, flowTypes);
                setBaseMgrE.FillResolverByFlow(resolver, flowView.Flow);
            }
            //如果已经确定了Flow
            else
            {
                //根据Flow和Hu匹配出flowView
                flowView = flowMgrE.CheckAndLoadFlowView(resolver.Code, null, null, null, hu, flowTypes);
            }
            setDetailMgrE.MatchHuByFlowView(resolver, flowView, hu);
        }

        [Transaction(TransactionMode.Requires)]
        protected override void ExecuteSubmit(Resolver resolver)
        {
            IList<Hu> huList = new List<Hu>();
            if (resolver.Transformers != null)
            {
                foreach (Transformer transformer in resolver.Transformers)
                {
                    if (transformer.TransformerDetails != null)
                    {
                        foreach (TransformerDetail transformerDetail in transformer.TransformerDetails)
                        {
                            Hu hu = huMgrE.LoadHu(transformerDetail.HuId);
                            huList.Add(hu);
                        }
                    }
                }
            }
            if (huList.Count > 0)
            {
                orderMgrE.CreateOrder(resolver.Code, resolver.UserCode, huList);
                resolver.Result = languageMgrE.TranslateMessage("Order.Reuse.Successfully", resolver.UserCode);
                resolver.Transformers = null;
                resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
            }
            else
            {
                throw new BusinessErrorException("Common.Business.Error.OprationFailed");
            }
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




﻿
#region Extend Class

namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class ReuseMgrE : com.Sconit.Service.Business.Impl.ReuseMgr, IBusinessMgrE
    {
       
    }
}

#endregion
