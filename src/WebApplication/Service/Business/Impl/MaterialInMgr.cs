using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.Production;
using Castle.Services.Transaction;

namespace com.Sconit.Service.Business.Impl
{
    public class MaterialInMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public IProductLineInProcessLocationDetailMgrE productLineInProcessLocationDetailMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IBomDetailMgrE bomDetailMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public IRoutingDetailMgrE routingDetailMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderLocTransMgrE { get; set; }
        public IOrderMgrE orderMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
            if (!(resolver.CodePrefix == BusinessConstants.BARCODE_HEAD_FLOW ||
                resolver.CodePrefix == BusinessConstants.CODE_PREFIX_WORK_ORDER))
            {
                return;
            }

            IList<OrderLocationTransaction> orderLocTransList = new List<OrderLocationTransaction>();

            if (resolver.CodePrefix == BusinessConstants.BARCODE_HEAD_FLOW)
            {
                setBaseMgrE.FillResolverByFlow(resolver);
            }
            else if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_WORK_ORDER)
            {
                string orderNo = resolver.Input;
                orderLocTransList = orderLocTransMgrE.GetOrderLocationTransaction(orderNo, BusinessConstants.IO_TYPE_OUT);
                if (orderLocTransList == null && orderLocTransList.Count == 0)
                {
                    return;
                }
                if (orderLocTransList[0].OrderDetail.OrderHead.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
                {
                    throw new BusinessErrorException("Common.Business.Error.StatusError", orderNo, BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT);
                }
                resolver.Input = orderLocTransList[0].OrderDetail.OrderHead.Flow;
                resolver.CodePrefix = BusinessConstants.BARCODE_HEAD_FLOW;
                resolver.OrderNo = orderNo;
                setBaseMgrE.FillResolverByFlow(resolver);
            }

            if (resolver.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                throw new BusinessErrorException("Flow.ShipReturn.Error.FlowTypeIsNotDistribution", resolver.OrderType);
            }

            setDetailMgrE.SetMateria(resolver);

            if (orderLocTransList.Count > 0)
            {
                foreach (Transformer transformer in resolver.Transformers)
                {
                    foreach (OrderLocationTransaction orderLocTrans in orderLocTransList)
                    {
                        if (transformer.ItemCode == orderLocTrans.Item.Code && transformer.UomCode == orderLocTrans.Uom.Code)
                        {
                            transformer.CurrentQty += orderLocTrans.OrderedQty;
                        }
                    }
                }
            }
            //if (orderNo != string.Empty)
            //{
            //    orderMgrE.StartOrder(orderNo, resolver.UserCode);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolver"></param>
        protected override void GetDetail(Resolver resolver)
        {
            //setDetailMgrE.SetMateria(resolver);
        }

        /// <summary>
        /// 仅校验投料的物料号,库位是否一致,不校验单位单包装等信息
        /// todo:不允许投入的又有数量又有Hu //可以前台控制
        /// </summary>
        /// <param name="resolver"></param>
        protected override void SetDetail(Resolver resolver)
        {
            if (resolver.CodePrefix == string.Empty)
            {
                throw new BusinessErrorException("Common.Business.Error.ScanProductLineFirst");
            }
            LocationLotDetail locationLotDetail = locationLotDetailMgrE.CheckLoadHuLocationLotDetail(resolver.Input, resolver.UserCode);
            TransformerDetail transformerDetail = TransformerHelper.ConvertLocationLotDetailToTransformerDetail(locationLotDetail, false);
            var query = resolver.Transformers.Where
                    (t => (t.ItemCode == transformerDetail.ItemCode && t.LocationCode == transformerDetail.LocationCode));
            if (query.Count() < 1)
            {
                throw new BusinessErrorException("Warehouse.HuMatch.NotMatch", transformerDetail.HuId);
            }
            resolver.AddTransformerDetail(transformerDetail);
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            IList<MaterialIn> materialInList = executeMgrE.ConvertTransformersToMaterialIns(resolver.Transformers);
            productLineInProcessLocationDetailMgrE.RawMaterialIn(resolver.Code, materialInList, userMgrE.CheckAndLoadUser(resolver.UserCode));
            resolver.Transformers = null;
            if (resolver.OrderNo != null && resolver.OrderNo != string.Empty)
            {
                orderMgrE.StartOrder(resolver.OrderNo, resolver.UserCode);
                resolver.Result = languageMgrE.TranslateMessage("MasterData.MaterialInAndStartOrder.Successfully", resolver.UserCode, resolver.OrderNo);
            }
            else
            {
                resolver.Result = languageMgrE.TranslateMessage("MasterData.MaterialIn.Successfully", resolver.UserCode, resolver.Code);
            }
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
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
    public partial class MaterialInMgrE : com.Sconit.Service.Business.Impl.MaterialInMgr, IBusinessMgrE
    {

    }
}

#endregion
