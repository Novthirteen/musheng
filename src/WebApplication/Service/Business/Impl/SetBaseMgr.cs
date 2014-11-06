using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.Exception;
using Castle.Services.Transaction;
using com.Sconit.Entity.Distribution;
using com.Sconit.Service.Distribution;
using com.Sconit.Entity;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.Distribution;

namespace com.Sconit.Service.Business.Impl
{
    public class SetBaseMgr : ISetBaseMgr
    {
        public IUserMgrE userMgrE { get; set; }
        public IOrderHeadMgrE orderHeadMgrE { get; set; }
        public IPickListMgrE pickListMgrE { get; set; }
        public IStorageBinMgrE storageBinMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IInProcessLocationMgrE inProcessLocationMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }

        [Transaction(TransactionMode.Unspecified)]
        public void FillResolverByOrder(Resolver resolver)
        {
            User user = userMgrE.CheckAndLoadUser(resolver.UserCode);
            OrderHead orderHead = orderHeadMgrE.CheckAndLoadOrderHead(resolver.Input);
            Flow flow = this.flowMgrE.LoadFlow(orderHead.Flow);

            //if (!user.HasPermission(orderHead.PartyFrom.Code) ||
            //    !user.HasPermission(orderHead.PartyTo.Code))
            //{
            //    throw new BusinessErrorException("Common.Business.Error.NoPermission");
            //}
            #region CopyProperty from OrderHead
            resolver.Code = orderHead.OrderNo;
            resolver.Description = flow.Description;
            resolver.Status = orderHead.Status;
            resolver.OrderType = orderHead.Type;
            resolver.OrderNo = orderHead.OrderNo;
            resolver.NeedPrintAsn = orderHead.NeedPrintAsn;
            resolver.NeedPrintReceipt = orderHead.NeedPrintReceipt;
            resolver.NeedInspection = orderHead.NeedInspection;
            resolver.AutoPrintHu = orderHead.AutoPrintHu && (orderHead.CreateHuOption != BusinessConstants.CODE_MASTER_CREATE_HU_OPTION_VALUE_NONE);
            resolver.AllowExceed = orderHead.AllowExceed;
            resolver.IsOddCreateHu = orderHead.IsOddCreateHu;
            resolver.IsPickFromBin = orderHead.IsPickFromBin;
            resolver.FulfillUnitCount = orderHead.FulfillUnitCount;
            resolver.IsShipByOrder = orderHead.IsShipByOrder;
            resolver.FlowCode = orderHead.Flow;
            if (resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_RECEIVE)
            {
                resolver.IsScanHu = orderHead.IsReceiptScanHu;
            }
            else if (resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_SHIP
                || resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_SHIPORDER)
            {
                resolver.IsScanHu = orderHead.IsShipScanHu;
            }
            #endregion
        }

        [Transaction(TransactionMode.Unspecified)]
        public void FillResolverByASN(Resolver resolver)
        {
            User user = userMgrE.CheckAndLoadUser(resolver.UserCode);
            InProcessLocation inProcessLocation = inProcessLocationMgrE.CheckAndLoadInProcessLocation(resolver.Input);

            //if (!user.HasPermission(inProcessLocation.PartyFrom.Code) ||
            //    !user.HasPermission(inProcessLocation.PartyTo.Code))
            //{
            //    throw new BusinessErrorException("Common.Business.Error.NoPermission");
            //}

            #region CopyProperty from ASN
            resolver.Code = inProcessLocation.IpNo;
            resolver.Status = inProcessLocation.Status;
            //resolver.IsDetailContainHu = inProcessLocation.IsDetailContainHu;
            resolver.IsScanHu = inProcessLocation.IsReceiptScanHu;
            if (inProcessLocation.InProcessLocationDetails != null && inProcessLocation.InProcessLocationDetails.Count > 0)
            {
                OrderHead orderHead =  inProcessLocation.InProcessLocationDetails[0].OrderLocationTransaction.OrderDetail.OrderHead;
                resolver.FulfillUnitCount = orderHead.FulfillUnitCount;
                resolver.AllowExceed = orderHead.AllowExceed;
                //
                resolver.NeedPrintAsn = orderHead.NeedPrintAsn;
                resolver.NeedPrintReceipt = orderHead.NeedPrintReceipt;
                resolver.NeedInspection = orderHead.NeedInspection;
                resolver.AutoPrintHu = orderHead.AutoPrintHu && (orderHead.CreateHuOption != BusinessConstants.CODE_MASTER_CREATE_HU_OPTION_VALUE_NONE);
                resolver.IsOddCreateHu = orderHead.IsOddCreateHu;
                resolver.IsPickFromBin = orderHead.IsPickFromBin;
                resolver.FulfillUnitCount = orderHead.FulfillUnitCount;
                resolver.IsShipByOrder = orderHead.IsShipByOrder;
                resolver.FlowCode = orderHead.Flow;
                resolver.IsScanHu = orderHead.IsReceiptScanHu;
            }
            resolver.PickBy = inProcessLocation.IsDetailContainHu ? BusinessConstants.CODE_MASTER_PICKBY_HU : BusinessConstants.CODE_MASTER_PICKBY_ITEM;
            //resolver.PickBy = inProcessLocation.IsReceiptScanHu ? BusinessConstants.CODE_MASTER_PICKBY_HU : BusinessConstants.CODE_MASTER_PICKBY_ITEM;
            resolver.OrderType = inProcessLocation.OrderType;

            #endregion
        }

        [Transaction(TransactionMode.Unspecified)]
        public void FillResolverByPickList(Resolver resolver)
        {
            PickList pickList = pickListMgrE.CheckAndLoadPickList(resolver.Input);
            User user = userMgrE.CheckAndLoadUser(resolver.UserCode);

            PickListHelper.CheckAuthrize(pickList, user);

            resolver.Code = pickList.PickListNo;
            resolver.Status = pickList.Status;
            resolver.PickBy = pickList.PickBy;
            if (pickList.PickListDetails != null && pickList.PickListDetails.Count > 0)
            {
                resolver.NeedPrintAsn = pickList.PickListDetails[0].OrderLocationTransaction.OrderDetail.OrderHead.NeedPrintAsn;
                resolver.IsPickFromBin = pickList.PickListDetails[0].OrderLocationTransaction.OrderDetail.OrderHead.IsPickFromBin;
            }
            //resolver.IsDetailContainHu = true;
            resolver.IsScanHu = true;//目前只有支持Hu才支持拣货
            resolver.OrderType = pickList.OrderType;
        }

        [Transaction(TransactionMode.Unspecified)]
        public void FillResolverByBin(Resolver resolver)
        {
            User user = userMgrE.CheckAndLoadUser(resolver.UserCode);
            StorageBin storageBin = storageBinMgrE.CheckAndLoadStorageBin(resolver.Input);

            #region 校验
            if (!storageBin.IsActive)
            {
                throw new BusinessErrorException("Common.Business.Error.EntityInActive", storageBin.Code);
            }

            //if (!user.HasPermission(storageBin.Area.Location.Region.Code))
            //{
            //    throw new BusinessErrorException("Common.Business.Error.NoPermission");
            //}
            #endregion

            resolver.Description = storageBin.Description;
            resolver.BinCode = storageBin.Code;

            //库格一定为目的(操作)
            resolver.LocationCode = storageBin.Area.Location.Code;
            resolver.LocationToCode = storageBin.Area.Location.Code;
            resolver.Result = languageMgrE.TranslateMessage("Warehouse.CurrentBinCode", resolver.UserCode, storageBin.Code);
        }


        /// <summary>
        ///  目前仅用于退库
        /// </summary>
        /// <param name="resolver"></param>
        [Transaction(TransactionMode.Unspecified)]
        public void FillResolverByLocation(Resolver resolver)
        {
            User user = userMgrE.CheckAndLoadUser(resolver.UserCode);
            Location location = locationMgrE.CheckAndLoadLocation(resolver.Input);

            #region 校验
            if (!location.IsActive)
            {
                throw new BusinessErrorException("Common.Business.Error.EntityInActive", location.Code);
            }

            //if (!user.HasPermission(location.Region.Code))
            //{
            //    throw new BusinessErrorException("Common.Business.Error.NoPermission");
            //}
            #endregion

            resolver.Description = location.Name;
            //库位一定为来源(操作)
            resolver.LocationCode = location.Code;
            resolver.LocationFormCode = location.Code;

            resolver.Result = languageMgrE.TranslateMessage("Warehouse.LocationCode", resolver.UserCode, location.Name);
        }

        [Transaction(TransactionMode.Unspecified)]
        public void FillResolverByFlow(Resolver resolver)
        {
            Flow flow = flowMgrE.CheckAndLoadFlow(resolver.Input);
            FillResolverByFlow(resolver, flow);
        }

        [Transaction(TransactionMode.Unspecified)]
        public void FillResolverByFlow(Resolver resolver, Flow flow)
        {
            User user = userMgrE.CheckAndLoadUser(resolver.UserCode);
            //Flow flow = flowMgrE.CheckAndLoadFlow(resolver.Input);

            #region 校验
            if (!flow.IsActive)
            {
                throw new BusinessErrorException("Common.Business.Error.EntityInActive", flow.Code);
            }

            //if (!user.HasPermission(flow.PartyFrom.Code)
            //|| !user.HasPermission(flow.PartyTo.Code))
            //{
            //    throw new BusinessErrorException("Common.Business.Error.NoPermission");
            //}
            #endregion

            resolver.Code = flow.Code;
            resolver.Description = flow.Description;
            resolver.IsScanHu = flow.IsShipScanHu || flow.IsReceiptScanHu;
            resolver.OrderType = flow.Type;
            resolver.AllowCreateDetail = flow.AllowCreateDetail;
            resolver.NeedPrintAsn = flow.NeedPrintAsn;
            resolver.NeedPrintReceipt = flow.NeedPrintReceipt;
            resolver.AutoPrintHu = flow.AutoPrintHu;
            resolver.AllowExceed = flow.AllowExceed;
            resolver.IsOddCreateHu = flow.IsOddCreateHu;
            resolver.IsPickFromBin = flow.IsPickFromBin;
            //resolver.CodePrefix = BusinessConstants.BARCODE_HEAD_FLOW;
            resolver.LocationFormCode = flow.LocationFrom == null ? string.Empty : flow.LocationFrom.Code;
            resolver.LocationToCode = flow.LocationTo == null ? string.Empty : flow.LocationTo.Code;
            resolver.FulfillUnitCount = flow.FulfillUnitCount;
            resolver.FlowCode = flow.Code;

            resolver.Result = languageMgrE.TranslateMessage("Common.Business.Message.Flow", resolver.UserCode) + resolver.Description;
        }

        [Transaction(TransactionMode.Unspecified)]
        public void FillDetailByFlow(Resolver resolver)
        {
            Flow flow = flowMgrE.CheckAndLoadFlow(resolver.Code, true);
            FillDetailByFlow(resolver, flow);
        }

        [Transaction(TransactionMode.Unspecified)]
        public void FillDetailByFlow(Resolver resolver, Flow flow)
        {
            foreach (FlowDetail flowDetail in flow.FlowDetails)
            {
                flowDetail.LocationFrom = flowDetail.LocationFrom == null ? flow.LocationFrom : flowDetail.LocationFrom;
                flowDetail.LocationTo = flowDetail.LocationTo == null ? flow.LocationTo : flowDetail.LocationTo;
                //if (resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_RECEIVERETURN
                //    || resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_SHIPRETURN)
                //{
                //    Location tempLocation = flowDetail.LocationFrom;
                //    flowDetail.LocationFrom = flowDetail.LocationTo;
                //    flowDetail.LocationTo = tempLocation;
                //}
            }
            resolver.Transformers = TransformerHelper.ConvertFlowDetailsToTransformers(flow.FlowDetails);
            //resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
        }

    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class SetBaseMgrE : com.Sconit.Service.Business.Impl.SetBaseMgr, ISetBaseMgrE
    {
       
    }
}

#endregion
