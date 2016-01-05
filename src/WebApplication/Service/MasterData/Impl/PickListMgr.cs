using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class PickListMgr : PickListBaseMgr, IPickListMgr
    {
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public INumberControlMgrE numberControlMgrE { get; set; }
        public IPickListDetailMgrE pickListDetailMgrE { get; set; }
        public IPickListResultMgrE pickListResultMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderLocationTransactionMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public ICodeMasterMgrE codeMasterMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }



        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public PickList LoadPickList(string pickListNo, bool includePickListDetail)
        {
            PickList pickList = base.LoadPickList(pickListNo);
            if (includePickListDetail && pickList != null && pickList.PickListDetails != null && pickList.PickListDetails.Count > 0)
            {
            }
            return pickList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public PickList LoadPickList(string pickListNo, bool includePickListDetail, bool includePickListResult)
        {
            PickList pickList = base.LoadPickList(pickListNo);
            if (includePickListDetail && pickList != null && pickList.PickListDetails != null && pickList.PickListDetails.Count > 0)
            {
                foreach (PickListDetail pickListDetail in pickList.PickListDetails)
                {
                    if (pickListDetail.PickListResults != null && pickListDetail.PickListResults.Count > 0)
                    {
                    }
                }
            }
            return pickList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public PickList CheckAndLoadPickList(string pickListNo)
        {
            PickList pickList = this.LoadPickList(pickListNo, true);
            if (pickList == null)
            {
                throw new BusinessErrorException("Order.Error.PickUp.PickListNoNotExist", pickListNo);
            }
            return pickList;
        }


        [Transaction(TransactionMode.Unspecified)]
        public PickList CreatePickList(List<string> orderNoList, User user)
        {
            IList<OrderLocationTransaction> orderLocTransList = orderLocationTransactionMgrE.GetOrderLocationTransaction(orderNoList, BusinessConstants.IO_TYPE_OUT);
            if (orderLocTransList != null && orderLocTransList.Count > 0)
            {
                foreach (OrderLocationTransaction orderLocTrans in orderLocTransList)
                {
                    orderLocTrans.CurrentShipQty = orderLocTrans.OrderDetail.RemainShippedQty * orderLocTrans.UnitQty;//转换成基本单位
                }
            }
            return this.CreatePickList(orderLocTransList, user);
        }

        [Transaction(TransactionMode.Requires)]
        public PickList CreatePickList(List<Transformer> transformerList, User user)
        {
            if (transformerList == null || transformerList.Count == 0)
            {
                throw new BusinessErrorException("Common.Business.Empty");
            }

            IList<OrderLocationTransaction> orderLocationTransactionList = new List<OrderLocationTransaction>();
            foreach (Transformer transformer in transformerList)
            {
                if (transformer.CurrentQty > 0)
                {
                    OrderLocationTransaction orderLocationTransaction = orderLocationTransactionMgrE.LoadOrderLocationTransaction(transformer.OrderLocTransId);
                    orderLocationTransaction.CurrentShipQty = transformer.CurrentQty;
                    orderLocationTransactionList.Add(orderLocationTransaction);
                }
            }

            return this.CreatePickList(orderLocationTransactionList, user);
        }
        [Transaction(TransactionMode.Requires)]
        public PickList CreatePickList(IList<OrderLocationTransaction> orderLocationTransactionList, User user)
        {
            List<OrderLocationTransaction> targetOrderLocationTransactionList = new List<OrderLocationTransaction>();
            OrderLocationTransactionComparer orderLocationTransactionComparer = new OrderLocationTransactionComparer();

            if (orderLocationTransactionList != null && orderLocationTransactionList.Count > 0)
            {
                var flowItem = flowMgrE.GetFlowItem(orderLocationTransactionList[0].OrderDetail.OrderHead.Flow, null);

                foreach (OrderLocationTransaction orderLocationTransaction in orderLocationTransactionList)
                {
                    if (orderLocationTransaction.CurrentShipQty > 0)
                    {
                        targetOrderLocationTransactionList.Add(orderLocationTransaction);

                        if (!flowItem.Contains(orderLocationTransaction.Item.Code))
                        {
                            throw new BusinessErrorException("Order.OrderHead.Error.ItemNotInFlowDetail",
                                orderLocationTransaction.Item.Code,
                                orderLocationTransaction.OrderDetail.OrderHead.Flow);
                        }
                    }
                }
            }

            if (targetOrderLocationTransactionList.Count == 0)
            {
                throw new BusinessErrorException("Order.Error.PickUp.DetailEmpty");
            }
            else
            {
                //按FromLocation、零件号、单位、单包装排序
                targetOrderLocationTransactionList.Sort(orderLocationTransactionComparer);
            }

            string orderType = null;
            Party partyFrom = null;
            Party partyTo = null;
            ShipAddress shipFrom = null;
            ShipAddress shipTo = null;
            string dockDescription = null;
            bool? isShipScanHu = null;
            bool? isReceiptScanHu = null;
            bool? isAutoReceive = null;
            decimal? completeLatency = null;
            string grGapTo = null;
            string asnTemplate = null;
            string receiptTemplate = null;
            string flow = null;
            DateTime? windowTime = null;
            bool? isAsnUniqueReceipt = null;
            Location locTo = null;

            #region 判断OrderHead的PartyFrom, PartyTo, ShipFrom, ShipTo, DockDescription是否一致
            foreach (OrderLocationTransaction orderLocationTransaction in targetOrderLocationTransactionList)
            {
                OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
                OrderHead orderHead = orderDetail.OrderHead;

                //判断OrderHead的Type是否一致
                if (orderType == null)
                {
                    orderType = orderHead.Type;
                }
                else if (orderHead.Type != orderType)
                {
                    throw new BusinessErrorException("Order.Error.PickUp.OrderTypeNotEqual");
                }

                //判断OrderHead的PartyFrom是否一致
                if (partyFrom == null)
                {
                    partyFrom = orderHead.PartyFrom;
                }
                else if (orderHead.PartyFrom.Code != partyFrom.Code)
                {
                    throw new BusinessErrorException("Order.Error.PickUp.PartyFromNotEqual");
                }

                //判断OrderHead的PartyFrom是否一致
                if (partyTo == null)
                {
                    partyTo = orderHead.PartyTo;
                }
                else if (orderHead.PartyTo.Code != partyTo.Code)
                {
                    throw new BusinessErrorException("Order.Error.PickUp.PartyToNotEqual");
                }

                //判断OrderHead的ShipFrom是否一致
                if (shipFrom == null)
                {
                    shipFrom = orderHead.ShipFrom;
                }
                else if (!AddressHelper.IsAddressEqual(orderHead.ShipFrom, shipFrom))
                {
                    throw new BusinessErrorException("Order.Error.PickUp.ShipFromNotEqual");
                }

                //判断OrderHead的ShipTo是否一致
                if (shipTo == null)
                {
                    shipTo = orderHead.ShipTo;
                }
                else if (!AddressHelper.IsAddressEqual(orderHead.ShipTo, shipTo))
                {
                    throw new BusinessErrorException("Order.Error.PickUp.ShipToNotEqual");
                }

                //判断OrderHead的DockDescription是否一致
                if (dockDescription == null)
                {
                    dockDescription = orderHead.DockDescription;
                }
                else if (orderHead.DockDescription != dockDescription)
                {
                    throw new BusinessErrorException("Order.Error.PickUp.DockDescriptionNotEqual");
                }

                //判断OrderHead的IsShipScanHu是否一致
                if (isShipScanHu == null)
                {
                    isShipScanHu = orderHead.IsShipScanHu;
                }
                else if (orderHead.IsShipScanHu != isShipScanHu)
                {
                    throw new BusinessErrorException("Order.Error.PickUp.IsShipScanHuNotEqual");
                }

                //判断OrderHead的IsReceiptScanHu是否一致
                if (isReceiptScanHu == null)
                {
                    isReceiptScanHu = orderHead.IsReceiptScanHu;
                }
                else if (orderHead.IsReceiptScanHu != isReceiptScanHu)
                {
                    throw new BusinessErrorException("Order.Error.PickUp.IsReceiptScanHuNotEqual");
                }

                //判断OrderHead的IsAutoReceipt是否一致
                if (isAutoReceive == null)
                {
                    isAutoReceive = orderHead.IsAutoReceive;
                }
                else if (orderHead.IsAutoReceive != isAutoReceive)
                {
                    throw new BusinessErrorException("Order.Error.PickUp.IsAutoReceiveNotEqual");
                }

                //判断OrderHead的CompleteLatency是否一致
                if (completeLatency == null)
                {
                    completeLatency = orderHead.CompleteLatency;
                }
                else
                {
                    if (orderHead.CompleteLatency.HasValue && orderHead.CompleteLatency != completeLatency)
                    {
                        throw new BusinessErrorException("Order.Error.PickUp.CompleteLatencyNotEqual");
                    }
                }

                //判断OrderHead的GoodsReceiptGapTo是否一致
                if (grGapTo == null)
                {
                    grGapTo = orderHead.GoodsReceiptGapTo;
                }
                else
                {
                    if (orderHead.GoodsReceiptGapTo != null && orderHead.GoodsReceiptGapTo != grGapTo)
                    {
                        throw new BusinessErrorException("Order.Error.PickUp.GoodsReceiptGapToNotEqual");
                    }
                }

                //判断OrderHead的AsnTemplate是否一致
                if (asnTemplate == null)
                {
                    asnTemplate = orderHead.AsnTemplate;
                }
                else
                {
                    if (orderHead.AsnTemplate != null && orderHead.AsnTemplate != asnTemplate)
                    {
                        throw new BusinessErrorException("Order.Error.PickUp.AsnTemplateNotEqual");
                    }
                }

                //判断OrderHead的ReceiptTemplate是否一致
                if (receiptTemplate == null)
                {
                    receiptTemplate = orderHead.ReceiptTemplate;
                }
                else
                {
                    if (orderHead.ReceiptTemplate != null && orderHead.ReceiptTemplate != receiptTemplate)
                    {
                        throw new BusinessErrorException("Order.Error.PickUp.ReceiptTemplateNotEqual");
                    }
                }

                //判断OrderHead的Flow是否一致
                if (flow == null)
                {
                    flow = orderHead.Flow;
                }
                else if (orderHead.Flow != flow)
                {
                    //throw new BusinessErrorException("Order.Error.PickUp.FlowNotEqual");
                }

                //寻找最小的WindowTime
                if (!windowTime.HasValue)
                {
                    windowTime = orderHead.WindowTime;
                }
                else if (windowTime.Value > orderHead.WindowTime)
                {
                    windowTime = orderHead.WindowTime;
                }

                //判断OrderHead的IsAsnUniqueReceipt是否一致
                if (isAsnUniqueReceipt == null)
                {
                    isAsnUniqueReceipt = orderHead.IsAsnUniqueReceipt;
                }
                else if (orderHead.IsAsnUniqueReceipt != isAsnUniqueReceipt)
                {
                    throw new BusinessErrorException("Order.Error.PickUp.IsAsnUniqueReceiptNotEqual");
                }

                //判断OrderDetail的DefaultLocTo是否一致
                if (locTo == null)
                {
                    locTo = orderDetail.DefaultLocationTo;
                }
                else
                {
                    if (orderDetail.DefaultLocationTo != null && orderDetail.DefaultLocationTo.Code != locTo.Code)
                    {
                        throw new BusinessErrorException("Order.Error.PickUp.LocationTo");
                    }
                }
            }
            #endregion

            #region 创建捡货单头
            DateTime dateTimeNow = DateTime.Now;

            PickList pickList = new PickList();

            pickList.PickListNo = numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_PICKLIST);
            pickList.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
            pickList.PickBy = this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_PICK_BY).Value;
            pickList.OrderType = orderType;
            pickList.PartyFrom = partyFrom;
            pickList.PartyTo = partyTo;
            pickList.ShipFrom = shipFrom;
            pickList.ShipTo = shipTo;
            pickList.DockDescription = dockDescription;
            pickList.CreateDate = dateTimeNow;
            pickList.CreateUser = user;
            pickList.LastModifyDate = dateTimeNow;
            pickList.LastModifyUser = user;
            pickList.IsShipScanHu = isShipScanHu.Value;
            pickList.IsReceiptScanHu = isReceiptScanHu.Value;
            pickList.IsAutoReceive = isAutoReceive.Value;
            pickList.CompleteLatency = completeLatency;
            pickList.GoodsReceiptGapTo = grGapTo;
            pickList.AsnTemplate = asnTemplate;
            pickList.ReceiptTemplate = receiptTemplate;
            pickList.Flow = flow;
            pickList.WindowTime = windowTime.Value;
            pickList.IsAsnUniqueReceipt = isAsnUniqueReceipt.Value;
            pickList.Location = locTo;

            this.CreatePickList(pickList);
            #endregion

            #region 创建捡货单明细
            int index = 0;
            IList<LocationLotDetail> locationLotDetailList = null;
            IList<LocationLotDetail> occupiedLocationLotDetailList = null; //捡货占用库存
            for (int i = 0; i < targetOrderLocationTransactionList.Count; i++)
            {
                OrderLocationTransaction orderLocationTransaction = targetOrderLocationTransactionList[i];  //本次循环OrderLocationTransaction
                OrderLocationTransaction lastOrderLocationTransaction = i == 0 ? null : targetOrderLocationTransactionList[i - 1];  //上次OrderLocationTransaction
                List<PickListDetail> pickListDetailList = new List<PickListDetail>();   //本次生成的PickListDetail列表

                OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
                OrderHead orderHead = orderDetail.OrderHead;
                decimal shipQty = orderLocationTransaction.CurrentShipQty;      //库存单位

                #region 过量拣货判断
                decimal pickedQty = 0; //其它拣货单的待拣货数量，只考虑Submit和InProcess状态
                IList<PickListDetail> pickedPickListDetailList = this.pickListDetailMgrE.GetPickedPickListDetail(orderLocationTransaction.Id);
                if (pickedPickListDetailList != null && pickedPickListDetailList.Count > 0)
                {
                    foreach (PickListDetail pickListDetail in pickedPickListDetailList)
                    {
                        if (pickListDetail.PickList.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT
                            || pickListDetail.PickList.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
                        {
                            pickedQty += pickListDetail.Qty;
                        }
                    }
                }

                //累计发货数量 + 待捡货数量 + 本次拣货数量 不能大于 订单数量
                if ((orderLocationTransaction.AccumulateQty.HasValue ? orderLocationTransaction.AccumulateQty.Value : 0) + shipQty + pickedQty > orderLocationTransaction.OrderedQty)
                {
                    throw new BusinessErrorException("MasterData.PickList.Error.PickExcceed", orderLocationTransaction.Item.Code);
                }
                #endregion

                //比较本次OrderLocationTransaction和上次OrderLocationTransaction，如果不相同需重新查找locationLotDetailList和重置index
                //为了处理订单合并捡货时，相同零件推荐的Hu/LotNo不重复问题
                if (lastOrderLocationTransaction == null
                    || orderLocationTransactionComparer.Compare(lastOrderLocationTransaction, orderLocationTransaction) == -1)
                {
                    index = 0;

                    #region 零头发货选项查询待拣货列表
                    string oddShipOption = orderDetail.OddShipOption;

                    if (oddShipOption == null || oddShipOption.Trim() == string.Empty)
                    {
                        CodeMaster codeMaster = this.codeMasterMgrE.GetDefaultCodeMaster(BusinessConstants.CODE_MASTER_ODD_SHIP_OPTION);

                        oddShipOption = codeMaster.Value;
                    }

                    if (oddShipOption == BusinessConstants.CODE_MASTER_ODD_SHIP_OPTION_VALUE_SHIP_FIRST)
                    {
                        //零头优先发、LotnNo先进先出、货架、包装    
                        if (orderHead.IsPickFromBin)
                        {
                            locationLotDetailList = this.locationLotDetailMgrE.GetHuLocationLotDetail(orderLocationTransaction.Location.Code, null, null, null, orderDetail.Item.Code, null, false, null, orderDetail.Uom.Code, new string[] { "hu.ManufactureDate;Asc", "sb.Sequence;Asc", "Qty;Asc", "Id;Asc" }, orderHead.IsPickFromBin, true, null, null);
                        }
                        else
                        {
                            locationLotDetailList = this.locationLotDetailMgrE.GetHuLocationLotDetail(orderLocationTransaction.Location.Code, null, null, null, orderDetail.Item.Code, null, false, null, orderDetail.Uom.Code, new string[] { "hu.ManufactureDate;Asc", "Qty;Asc", "Id;Asc" }, orderHead.IsPickFromBin, false, null, null);
                        }
                        //,orderLocationTransaction.SortLevel1From, orderLocationTransaction.SortLevel1To, orderLocationTransaction.ColorLevel1From, orderLocationTransaction.ColorLevel1To, orderLocationTransaction.SortLevel2From, orderLocationTransaction.SortLevel2To, orderLocationTransaction.ColorLevel2From, orderLocationTransaction.ColorLevel2To);  //添加分光分色校验
                        #region 重新排序，把零头放在前面
                        if (locationLotDetailList != null && locationLotDetailList.Count > 0)
                        {
                            IList<LocationLotDetail> oddLocationLotDetailList = new List<LocationLotDetail>();
                            IList<LocationLotDetail> noOddLocationLotDetailList = new List<LocationLotDetail>();
                            foreach (LocationLotDetail locationLotDetail in locationLotDetailList)
                            {
                                if (!this.locationMgrE.IsHuOcuppyByPickList(locationLotDetail.Hu.HuId))
                                {
                                    if (locationLotDetail.Hu.Qty < orderDetail.UnitCount)
                                    {
                                        oddLocationLotDetailList.Add(locationLotDetail);
                                        //shipQty += locationLotDetail.Qty;  //零头一定要先发走，不占用待拣货数量
                                    }
                                    else
                                    {
                                        noOddLocationLotDetailList.Add(locationLotDetail);
                                    }
                                }
                            }
                            locationLotDetailList = oddLocationLotDetailList;
                            IListHelper.AddRange<LocationLotDetail>(locationLotDetailList, noOddLocationLotDetailList);
                        }
                        #endregion
                    }
                    else if (oddShipOption == BusinessConstants.CODE_MASTER_ODD_SHIP_OPTION_VALUE_NOT_SHIP)
                    {
                        //零头不发
                        if (orderHead.IsPickFromBin)
                        {
                            locationLotDetailList = this.locationLotDetailMgrE.GetHuLocationLotDetail(orderLocationTransaction.Location.Code, null, null, null, orderDetail.Item.Code, null, false, orderDetail.UnitCount, orderDetail.Uom.Code, new string[] { "hu.ManufactureDate;Asc", "sb.Sequence;Asc", "Id;Asc" }, orderHead.IsPickFromBin, true, null, null);
                        }
                        else
                        {
                            locationLotDetailList = this.locationLotDetailMgrE.GetHuLocationLotDetail(orderLocationTransaction.Location.Code, null, null, null, orderDetail.Item.Code, null, false, orderDetail.UnitCount, orderDetail.Uom.Code, new string[] { "hu.ManufactureDate;Asc", "Id;Asc" }, orderHead.IsPickFromBin, false, null, null);
                        }

                        //,orderLocationTransaction.SortLevel1From, orderLocationTransaction.SortLevel1To, orderLocationTransaction.ColorLevel1From, orderLocationTransaction.ColorLevel1To, orderLocationTransaction.SortLevel2From, orderLocationTransaction.SortLevel2To, orderLocationTransaction.ColorLevel2From, orderLocationTransaction.ColorLevel2To);  //添加分光分色校验
                    }
                    #endregion

                    IList<PickListDetail> submitPickListDetailList = this.pickListDetailMgrE.GetPickListDetail(orderLocationTransaction.Location.Code, orderDetail.Item.Code, orderDetail.UnitCount, orderDetail.Uom.Code, new string[] { BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT, BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS });
                    //IList<PickListResult> inprocessPickListResultList = this.pickListResultMgrE.GetPickListResult(orderLocationTransaction.Location.Code, orderDetail.Item.Code, orderDetail.UnitCount, orderDetail.Uom.Code, new string[] { BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS });

                    occupiedLocationLotDetailList = this.Convert2OccupiedLocationLotDetail(submitPickListDetailList, pickList.PickBy);
                }

                if (locationLotDetailList != null && locationLotDetailList.Count > 0)
                {
                    PickListDetail lastestPickListDetail = null;
                    for (; index < locationLotDetailList.Count; index++)
                    {
                        LocationLotDetail locationLotDetail = locationLotDetailList[index];
                        decimal locQty = locationLotDetail.Qty;

                        if (pickList.PickBy == BusinessConstants.CODE_MASTER_PICKBY_HU)
                        {
                            #region 按Hu捡货

                            #region 过滤掉已经被推荐的库存
                            if (occupiedLocationLotDetailList != null && occupiedLocationLotDetailList.Count > 0)
                            {
                                bool findMatch = false;
                                foreach (LocationLotDetail occupiedLocationLotDetail in occupiedLocationLotDetailList)
                                {
                                    if (occupiedLocationLotDetail.Hu.HuId == locationLotDetail.Hu.HuId)
                                    {
                                        findMatch = true;
                                        continue;
                                    }
                                }

                                if (findMatch)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            shipQty -= locQty;

                            PickListDetail pickListDetail = new PickListDetail();

                            pickListDetail.PickList = pickList;
                            pickListDetail.OrderLocationTransaction = orderLocationTransaction;
                            pickListDetail.Item = orderLocationTransaction.Item;
                            pickListDetail.UnitCount = orderDetail.UnitCount;
                            pickListDetail.Uom = orderDetail.Uom;
                            pickListDetail.HuId = locationLotDetail.Hu.HuId;
                            pickListDetail.LotNo = locationLotDetail.Hu.ManufactureDate.ToString("yyyyMMdd");
                            pickListDetail.Location = locationLotDetail.Location;
                            if (locationLotDetail.StorageBin != null)
                            {
                                pickListDetail.StorageArea = locationLotDetail.StorageBin.Area;
                                pickListDetail.StorageBin = locationLotDetail.StorageBin;
                            }
                            pickListDetail.Qty = locQty / orderLocationTransaction.UnitQty; //订单单位
                            this.pickListDetailMgrE.CreatePickListDetail(pickListDetail);
                            pickList.AddPickListDetail(pickListDetail);
                            pickListDetailList.Add(pickListDetail);

                            if (shipQty <= 0)
                            {
                                index++;
                                break;
                            }
                            #endregion
                        }
                        else if (pickList.PickBy == BusinessConstants.CODE_MASTER_PICKBY_LOTNO)
                        {
                            #region 按LotNo捡货

                            #region 过滤掉已经被推荐的库存
                            if (occupiedLocationLotDetailList != null && occupiedLocationLotDetailList.Count > 0)
                            {
                                foreach (LocationLotDetail occupiedLocationLotDetail in occupiedLocationLotDetailList)
                                {
                                    if (occupiedLocationLotDetail.Item.Code == locationLotDetail.Item.Code
                                        && occupiedLocationLotDetail.LotNo == locationLotDetail.LotNo
                                        && occupiedLocationLotDetail.Location.Code == locationLotDetail.Location.Code
                                        && StorageBinHelper.IsStorageBinEqual(occupiedLocationLotDetail.StorageBin, locationLotDetail.StorageBin))
                                    {
                                        if (locationLotDetail.Hu.Qty < orderDetail.UnitCount)
                                        {
                                            //shipQty -= locationLotDetail.Qty;  //如果零头被占用，需要扣减发货数量
                                        }

                                        if (occupiedLocationLotDetail.Qty == 0)
                                        {
                                            continue;
                                        }

                                        if (occupiedLocationLotDetail.Qty - locQty >= 0)
                                        {
                                            occupiedLocationLotDetail.Qty -= locQty;
                                            locQty = 0;
                                            continue;
                                        }
                                        else
                                        {
                                            occupiedLocationLotDetail.Qty = 0;
                                            locQty -= occupiedLocationLotDetail.Qty;
                                            break;
                                        }
                                    }
                                }

                                if (locQty == 0)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            shipQty -= locQty;

                            if (shipQty < 0)
                            {
                                if (locQty < orderDetail.UnitCount)
                                {
                                    //零头，发走
                                }
                                else
                                {
                                    //非零头，只扣减包装数
                                    shipQty += locQty;
                                    locQty = orderDetail.UnitCount * Math.Ceiling(shipQty / orderDetail.UnitCount);
                                    shipQty -= locQty;
                                }
                            }

                            if (lastestPickListDetail != null
                                && lastestPickListDetail.LotNo == locationLotDetail.Hu.ManufactureDate.ToString("yyyyMMdd")
                                && StorageBinHelper.IsStorageBinEqual(lastestPickListDetail.StorageBin, locationLotDetail.StorageBin))
                            {
                                #region 合并捡货数量
                                lastestPickListDetail.Qty += locQty / orderLocationTransaction.UnitQty; //订单单位
                                this.pickListDetailMgrE.UpdatePickListDetail(lastestPickListDetail);
                                #endregion
                            }
                            else
                            {
                                #region 新增捡货明细
                                lastestPickListDetail = new PickListDetail();

                                lastestPickListDetail.PickList = pickList;
                                lastestPickListDetail.OrderLocationTransaction = orderLocationTransaction;
                                lastestPickListDetail.Item = orderLocationTransaction.Item;
                                lastestPickListDetail.UnitCount = orderDetail.UnitCount;                    //可能拣货的包装和订单明细包装不一致，所以使用Hu上的单包装
                                lastestPickListDetail.Uom = orderDetail.Uom;
                                lastestPickListDetail.LotNo = locationLotDetail.Hu.ManufactureDate.ToString("yyyyMMdd");
                                lastestPickListDetail.Location = locationLotDetail.Location;
                                lastestPickListDetail.SortLevel1From = orderLocationTransaction.SortLevel1From;
                                lastestPickListDetail.SortLevel1To = orderLocationTransaction.SortLevel1To;
                                lastestPickListDetail.ColorLevel1From = orderLocationTransaction.ColorLevel1From;
                                lastestPickListDetail.ColorLevel1To = orderLocationTransaction.ColorLevel1To;
                                lastestPickListDetail.SortLevel2From = orderLocationTransaction.SortLevel2From;
                                lastestPickListDetail.SortLevel2To = orderLocationTransaction.SortLevel2To;
                                lastestPickListDetail.ColorLevel2From = orderLocationTransaction.ColorLevel2From;
                                lastestPickListDetail.ColorLevel2To = orderLocationTransaction.ColorLevel2To;
                                if (locationLotDetail.StorageBin != null)
                                {
                                    lastestPickListDetail.StorageArea = locationLotDetail.StorageBin.Area;
                                    lastestPickListDetail.StorageBin = locationLotDetail.StorageBin;
                                }
                                lastestPickListDetail.Qty = locQty / orderLocationTransaction.UnitQty; //订单单位

                                this.pickListDetailMgrE.CreatePickListDetail(lastestPickListDetail);
                                pickList.AddPickListDetail(lastestPickListDetail);
                                pickListDetailList.Add(lastestPickListDetail);
                                #endregion
                            }

                            if (shipQty <= 0)
                            {
                                index++;
                                break;
                            }
                            #endregion
                        }
                        else
                        {
                            throw new TechnicalException("Invalied PickBy value:" + pickList.PickBy);
                        }
                    }
                }

                //if (pickListDetailList.Count == 0)
                //{
                //    throw new BusinessErrorException("MasterData.PickList.Error.NotEnoughInventory");
                //}

                if (shipQty > 0)
                {
                    PickListDetail pickListDetail = new PickListDetail();

                    pickListDetail.PickList = pickList;
                    pickListDetail.OrderLocationTransaction = orderLocationTransaction;
                    pickListDetail.Item = orderLocationTransaction.Item;
                    pickListDetail.UnitCount = orderDetail.UnitCount;
                    pickListDetail.Uom = orderDetail.Uom;
                    pickListDetail.Location = orderLocationTransaction.Location;
                    pickListDetail.Qty = shipQty / orderLocationTransaction.UnitQty; //订单单位
                    pickListDetail.Memo = this.languageMgrE.TranslateMessage("MasterData.PickList.NotEnoughInventory", user); //设置Memo为库存不足

                    pickList.AddPickListDetail(pickListDetail);

                    this.pickListDetailMgrE.CreatePickListDetail(pickListDetail);
                }

                #region 设置多批号
                if (pickListDetailList.Count > 0 && pickList.PickBy == BusinessConstants.CODE_MASTER_PICKBY_LOTNO)
                {
                    string lotNo = string.Empty;
                    bool hasMultiLotNo = false;
                    foreach (PickListDetail pickListDetail in pickListDetailList)
                    {
                        if (lotNo == string.Empty)
                        {
                            lotNo = pickListDetail.LotNo;
                        }
                        else if (lotNo != pickListDetail.LotNo)
                        {
                            hasMultiLotNo = true;
                            break;
                        }
                    }

                    //设置Memo为多批号
                    if (hasMultiLotNo)
                    {
                        foreach (PickListDetail pickListDetail in pickListDetailList)
                        {
                            if (pickListDetail.Memo == null || pickListDetail.Memo.Trim() == string.Empty)
                            {
                                pickListDetail.Memo = this.languageMgrE.TranslateMessage("MasterData.PickList.MultiLotNo", user);
                            }
                            else
                            {
                                pickListDetail.Memo += "; " + this.languageMgrE.TranslateMessage("MasterData.PickList.MultiLotNo", user);
                            }
                            this.pickListDetailMgrE.UpdatePickListDetail(pickListDetail);
                        }
                    }
                }
                #endregion
            }
            #endregion

            //if (pickList.PickListDetails == null || pickList.PickListDetails.Count == 0)
            //{
            //    throw new BusinessErrorException("MasterData.PickList.Error.NotEnoughInventory");
            //}

            #region 按分光分色过滤

            #endregion

            return pickList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public void DoPick(PickList pickList, string userCode)
        {
            DoPick(pickList, userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void DoPick(PickList pickList, User user)
        {
            PickList oldPickList = this.LoadPickList(pickList.PickListNo);

            PickListHelper.CheckAuthrize(oldPickList, user);

            if (oldPickList.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
            {
                throw new BusinessErrorException("Order.Error.PickUp.StatusErrorWhenPick", oldPickList.Status, oldPickList.PickListNo);
            }

            //检查是否有pickListResult
            int resultCount = 0;
            foreach (PickListDetail pickListDetail in pickList.PickListDetails)
            {
                foreach (PickListResult pickListResult in pickListDetail.PickListResults)
                {
                    if (pickListResult.Id == 0)
                    {
                        resultCount++;
                        break;
                    }
                }
            }

            if (resultCount == 0)
            {
                throw new BusinessErrorException("MasterData.No.PickListResult");
            }

            foreach (PickListDetail pickListDetail in pickList.PickListDetails)
            {
                foreach (PickListResult pickListResult in pickListDetail.PickListResults)
                {
                    if (pickListResult.Id > 0)
                    {
                        continue;
                    }

                    #region 检查捡货项是否已经被其它捡货单占用
                    if (this.locationMgrE.IsHuOcuppyByPickList(pickListResult.LocationLotDetail.Hu.HuId))
                    {
                        throw new BusinessErrorException("Order.Error.PickUp.HuOcuppied", pickListResult.LocationLotDetail.Hu.HuId);
                    }
                    #endregion

                    pickListResultMgrE.CreatePickListResult(pickListResult);

                    #region 下架
                    if (pickListResult.LocationLotDetail.StorageBin != null)
                    {
                        this.locationMgrE.InventoryPick(pickListResult.LocationLotDetail, user);
                    }
                    #endregion
                }
            }

            oldPickList.LastModifyDate = DateTime.Now;
            oldPickList.LastModifyUser = user;

            this.UpdatePickList(oldPickList);
        }

        [Transaction(TransactionMode.Requires)]
        public void ManualClosePickList(PickList pickList, User user)
        {
            ManualClosePickList(pickList.PickListNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void ManualClosePickList(string pickListNo, User user)
        {
            PickList pickList = this.LoadPickList(pickListNo);

            PickListHelper.CheckAuthrize(pickList, user);

            if (pickList.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
            {
                throw new BusinessErrorException("Order.Error.PickUp.StatusErrorWhenClose", pickList.Status, pickList.PickListNo);
            }

            pickList.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
            pickList.LastModifyDate = DateTime.Now;
            pickList.LastModifyUser = user;

            this.UpdatePickList(pickList);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeletePickList(PickList pickList, User user)
        {
            DeletePickList(pickList.PickListNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeletePickList(string pickListNo, User user)
        {
            PickList oldPickList = this.LoadPickList(pickListNo, true);

            PickListHelper.CheckAuthrize(oldPickList, user);

            if (oldPickList.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                throw new BusinessErrorException("Order.Error.PickUp.StatusErrorWhenDelete", oldPickList.Status, oldPickList.PickListNo);
            }

            foreach (PickListDetail pickListDetail in oldPickList.PickListDetails)
            {
                pickListDetailMgrE.DeletePickListDetail(pickListDetail);
            }

            this.DeletePickList(pickListNo);
        }

        [Transaction(TransactionMode.Requires)]
        public void CancelPickList(PickList pickList, User user)
        {
            CancelPickList(pickList.PickListNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void CancelPickList(string pickListNo, User user)
        {
            PickList pickList = this.LoadPickList(pickListNo);

            PickListHelper.CheckAuthrize(pickList, user);

            if (pickList.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                throw new BusinessErrorException("Order.Error.PickUp.StatusErrorWhenCancel", pickList.Status, pickList.PickListNo);
            }

            pickList.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL;
            pickList.LastModifyDate = DateTime.Now;
            pickList.LastModifyUser = user;

            this.UpdatePickList(pickList);
        }

        [Transaction(TransactionMode.Requires)]
        public void StartPickList(PickList pickList, User user)
        {
            StartPickList(pickList.PickListNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void StartPickList(string pickListNo, User user)
        {
            PickList pickList = this.CheckAndLoadPickList(pickListNo);

            PickListHelper.CheckAuthrize(pickList, user);

            if (pickList.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                throw new BusinessErrorException("Order.Error.PickUp.StatusErrorWhenStart", pickList.Status, pickList.PickListNo);
            }

            #region 检查MaxOnlineQty
            Flow flow = this.flowMgrE.LoadFlow(pickList.Flow);
            if (flow != null && flow.MaxOnlineQty > 0
                && this.GetInPorcessPickListCount(pickList.Flow, user) >= flow.MaxOnlineQty)
            {
                throw new BusinessErrorException("Order.Error.PickUp.ExcceedMaxOnlineQty");
            }
            #endregion

            DateTime dateTimeNow = DateTime.Now;
            pickList.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS;
            pickList.StartDate = dateTimeNow;
            pickList.StartUser = user;
            pickList.LastModifyDate = dateTimeNow;
            pickList.LastModifyUser = user;

            this.UpdatePickList(pickList);
        }
        #endregion Customized Methods

        #region Private Methods
        IList<LocationLotDetail> Convert2OccupiedLocationLotDetail(IList<PickListDetail> pickListDetailList, string pickBy)
        {
            IList<LocationLotDetail> locationLotDetailList = new List<LocationLotDetail>();

            #region 转换PickListDetail至LocationLotDetail
            if (pickListDetailList != null && pickListDetailList.Count > 0)
            {
                foreach (PickListDetail pickListDetail in pickListDetailList)
                {
                    if (pickBy == BusinessConstants.CODE_MASTER_PICKBY_HU)
                    {
                        if (pickListDetail.HuId != null && pickListDetail.HuId.Trim() != string.Empty)
                        {
                            LocationLotDetail newLocationLotDetail = new LocationLotDetail();
                            newLocationLotDetail.Location = pickListDetail.Location;
                            newLocationLotDetail.StorageBin = pickListDetail.StorageBin;
                            newLocationLotDetail.Item = pickListDetail.Item;
                            newLocationLotDetail.Hu = new Hu();
                            newLocationLotDetail.Hu.HuId = pickListDetail.HuId;
                            newLocationLotDetail.LotNo = pickListDetail.LotNo;
                            newLocationLotDetail.Qty = pickListDetail.Qty * pickListDetail.OrderLocationTransaction.UnitQty; //库存单位

                            locationLotDetailList.Add(newLocationLotDetail);
                        }
                    }
                    else if (pickBy == BusinessConstants.CODE_MASTER_PICKBY_LOTNO)
                    {
                        bool matchLocationLotDetail = false;

                        foreach (LocationLotDetail locationLotDetail in locationLotDetailList)
                        {
                            if (locationLotDetail.Item.Code == pickListDetail.Item.Code
                                && locationLotDetail.LotNo == pickListDetail.LotNo
                                && locationLotDetail.Location.Code == pickListDetail.Location.Code
                                && StorageBinHelper.IsStorageBinEqual(locationLotDetail.StorageBin, pickListDetail.StorageBin))
                            {
                                locationLotDetail.Qty += pickListDetail.Qty * pickListDetail.OrderLocationTransaction.UnitQty; //库存单位
                                matchLocationLotDetail = true;
                                break;
                            }
                        }

                        if (!matchLocationLotDetail)
                        {
                            LocationLotDetail newLocationLotDetail = new LocationLotDetail();
                            newLocationLotDetail.Location = pickListDetail.Location;
                            newLocationLotDetail.StorageBin = pickListDetail.StorageBin;
                            newLocationLotDetail.Item = pickListDetail.Item;
                            newLocationLotDetail.LotNo = pickListDetail.LotNo;
                            newLocationLotDetail.Qty = pickListDetail.Qty * pickListDetail.OrderLocationTransaction.UnitQty; //库存单位

                            locationLotDetailList.Add(newLocationLotDetail);
                        }
                    }
                    else
                    {
                        throw new TechnicalException("invalided pick by:" + pickBy);
                    }
                }
            }
            #endregion

            return locationLotDetailList;
        }

        private int GetInPorcessPickListCount(string flowCode, User user)
        {
            DetachedCriteria criteria = DetachedCriteria.For<PickList>();
            criteria.SetProjection(Projections.Count("PickListNo"));

            criteria.Add(Expression.Eq("Flow", flowCode));
            criteria.Add(Expression.Eq("StartUser", user));
            criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));

            return this.criteriaMgrE.FindAll<int>(criteria)[0];
        }

        #endregion Private Methods
    }

    class OrderLocationTransactionComparer : IComparer<OrderLocationTransaction>
    {
        public int Compare(OrderLocationTransaction x, OrderLocationTransaction y)
        {
            if (x.Location.Code == y.Location.Code
                && x.OrderDetail.Item.Code == y.OrderDetail.Item.Code
                && x.OrderDetail.Uom.Code == y.OrderDetail.Uom.Code
                && x.OrderDetail.UnitCount == y.OrderDetail.UnitCount)
            {
                return 0;
            }
            else if (x.Location.Code == y.Location.Code
                && x.OrderDetail.Item.Code == y.OrderDetail.Item.Code
                && x.OrderDetail.Uom.Code == y.OrderDetail.Uom.Code
                && x.OrderDetail.UnitCount > y.OrderDetail.UnitCount)
            {
                return 1;
            }
            else if (x.Location.Code == y.Location.Code
                && x.OrderDetail.Item.Code == y.OrderDetail.Item.Code
                && x.OrderDetail.Uom.Code == y.OrderDetail.Uom.Code
                && x.OrderDetail.UnitCount < y.OrderDetail.UnitCount)
            {
                return -1;
            }
            else if (x.Location.Code == y.Location.Code
                && x.OrderDetail.Item.Code == y.OrderDetail.Item.Code)
            {
                return string.Compare(x.OrderDetail.Uom.Code, y.OrderDetail.Uom.Code);
            }
            else if (x.Location.Code == y.Location.Code)
            {
                return string.Compare(x.OrderDetail.Item.Code, y.OrderDetail.Item.Code);
            }
            else
            {
                return string.Compare(x.Location.Code, y.Location.Code);
            }
        }
    }
}


#region Extend Class





namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class PickListMgrE : com.Sconit.Service.MasterData.Impl.PickListMgr, IPickListMgrE
    {

    }


}
#endregion
