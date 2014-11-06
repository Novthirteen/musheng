using com.Sconit.Service.Ext.Business;

using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.View;
using System.Collections.Generic;

namespace com.Sconit.Service.Business.Impl
{
    public class ReturnMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public IStorageBinMgrE storageBinMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }

        

        protected override void SetBaseInfo(Resolver resolver)
        {
            if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_BIN)
            {
                setBaseMgrE.FillResolverByBin(resolver);
            }
            else if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_LOCATION)
            {
                setBaseMgrE.FillResolverByLocation(resolver);
            }
            else if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_FLOW)
            {
                setBaseMgrE.FillResolverByFlow(resolver);
                if (resolver.OrderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                    && resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_SHIPRETURN)
                {
                    throw new BusinessErrorException("Flow.ShipReturn.Error.FlowTypeIsProcurement", resolver.OrderType);
                }
                if (resolver.OrderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION
                    && resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_RECEIVERETURN)
                {
                    throw new BusinessErrorException("Flow.ReceiveReturn.Error.FlowTypeIsDistribution", resolver.OrderType);
                }
            }
            else
            {
                throw new BusinessErrorException("Common.Business.Error.BarCodeInvalid");
            }
        }

        protected override void GetDetail(Resolver resolver)
        {
            //setBaseMgrE.FillDetailByFlow(resolver);
        }

        protected override void SetDetail(Resolver resolver)
        {
            List<string> flowTypes = new List<string>();
            flowTypes.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER);
            flowTypes.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT);
            flowTypes.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION);
            flowTypes.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_CUSTOMERGOODS);
            Hu hu = huMgrE.CheckAndLoadHu(resolver.Input);
            if (this.locationMgrE.IsHuOcuppyByPickList(resolver.Input))
            {
                throw new BusinessErrorException("Order.Error.PickUp.HuOcuppied", resolver.Input);
            }

            FlowView flowView = null;
            //移库路线类型退货(退库)可以跟据库格和库位找出相对应的移库路线
            if (resolver.CodePrefix == null || resolver.CodePrefix.Trim() == string.Empty)
            {
                if (resolver.BinCode == null || resolver.BinCode.Trim() == string.Empty)
                {
                    throw new BusinessErrorException("Common.Business.Error.ScanFlowOrStorageBinFirst");
                }
                if (resolver.LocationFormCode == null || resolver.LocationFormCode.Trim() == string.Empty)
                {
                    throw new BusinessErrorException("Common.Business.Error.ScanFlowOrLocationFirst");
                }
                if (hu.Location != null)
                {
                    if (hu.Location != resolver.LocationFormCode)
                    {
                        throw new BusinessErrorException("Common.Business.Error.HuNoInventory", resolver.LocationFormCode, hu.HuId);
                    }
                }
                //确定flow和flowView
                List<string> transferType = new List<string>();
                transferType.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER);
                flowView = flowMgrE.CheckAndLoadFlowView(null, resolver.UserCode, resolver.LocationToCode, resolver.LocationFormCode, hu, transferType);
                setBaseMgrE.FillResolverByFlow(resolver, flowView.Flow);
                resolver.Result = resolver.LocationFormCode + " => " + resolver.LocationToCode;
            }

            //已经确定了Flow,匹配新的Hu
            if (resolver.CodePrefix != null && resolver.CodePrefix.Trim() != string.Empty)
            {
                flowView = flowMgrE.CheckAndLoadFlowView(resolver.Code, null, null, null, hu, null);
                //退库检查库存
                if ((resolver.OrderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER
                     || resolver.OrderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                     || resolver.OrderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_CUSTOMERGOODS)
                    && flowView.Flow.IsReceiptScanHu
                    )
                {
                    LocationLotDetail locationLotDetail = locationLotDetailMgrE.CheckLoadHuLocationLotDetail(hu.HuId);
                    hu.Qty = locationLotDetail.Qty / hu.UnitQty;
                }
            }
            else
            {
                throw new BusinessErrorException("Common.Business.Error.ScanFlowFirst");
            }
            setDetailMgrE.MatchHuByFlowView(resolver, flowView, hu);
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            executeMgrE.OrderReturn(resolver);
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
    public partial class ReturnMgrE : com.Sconit.Service.Business.Impl.ReturnMgr, IBusinessMgrE
    {
        
    }
}

#endregion
