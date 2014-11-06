using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Procurement;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Utility;
using com.Sconit.Service.Distribution;
using com.Sconit.Entity.Distribution;
using NHibernate.Expression;
using com.Sconit.Service.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ReceiptMgr : ReceiptBaseMgr, IReceiptMgr
    {
        private string[] ReceiptDetail2ReceiptDetailCloneFields = new string[] 
            { 
                "Receipt",
                "OrderLocationTransaction",
                "HuId",
                "LotNo",
                "ReceivedQty",
                "ShippedQty",
                "RejectedQty",
                "ScrapQty",
                "IsConsignment",
                "PlannedBill"
            };

        public IReceiptDetailMgrE receiptDetailMgrE { get; set; }
        public INumberControlMgrE numberControlMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IHuOddMgrE huOddMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderLocationTransactionMgrE { get; set; }
        public IInProcessLocationMgrE inProcessLocationMgrE { get; set; }
        public IInProcessLocationDetailMgrE inProcessLocationDetailMgrE { get; set; }
        public IReceiptInProcessLocationMgrE receiptInProcessLocationMgrE { get; set; }
        public IInspectOrderMgrE inspectOrderMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IRegionMgrE regionMgr { get; set; }
        public ICostMgrE costMgr { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Requires)]
        public override void CreateReceipt(Receipt receipt)
        {
            #region 在Receipt上记录参考Asn，为字串
            string ipNo = null;
            
            if (receipt.InProcessLocations != null && receipt.InProcessLocations.Count > 0)
            {
                foreach (InProcessLocation inProcessLocation in receipt.InProcessLocations)
                {
                    if (ipNo == null)
                    {
                        ipNo = inProcessLocation.IpNo;
                    }
                    else
                    {
                        ipNo += ", " + inProcessLocation.IpNo;
                    }
                }
                receipt.Flow = receipt.InProcessLocations[0].Flow;
            }
            receipt.ReferenceIpNo = ipNo;
           
            #endregion

            this.entityDao.CreateReceipt(receipt);

            #region 保存收货和发货关系
            if (receipt.InProcessLocations != null && receipt.InProcessLocations.Count > 0)
            {
                foreach (InProcessLocation inProcessLocation in receipt.InProcessLocations)
                {
                    ReceiptInProcessLocation receiptInProcessLocation = new ReceiptInProcessLocation();
                    receiptInProcessLocation.InProcessLocation = inProcessLocation;
                    receiptInProcessLocation.Receipt = receipt;

                    this.receiptInProcessLocationMgrE.CreateReceiptInProcessLocation(receiptInProcessLocation);
                }
            }
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateReceipt(Receipt receipt, User user)
        {
            CreateReceipt(receipt, user, true);
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateReceipt(Receipt receipt, User user, bool isOddCreateHu)
        {
            #region 查找所有的发货项，收货单打印模板，收货差异处理选项
            string orderType = null;
            Party partyFrom = null;
            Party partyTo = null;
            ShipAddress shipFrom = null;
            ShipAddress shipTo = null;
            string dockDescription = null;
            string receiptTemplate = null;
            string huTemplate = null;
            string grGapTo = null;
            IList<InProcessLocationDetail> inProcessLocationDetailList = new List<InProcessLocationDetail>();
            if (receipt.InProcessLocations != null && receipt.InProcessLocations.Count > 0)
            {
                foreach (InProcessLocation inProcessLocation in receipt.InProcessLocations)
                {
                    InProcessLocation currentIp = inProcessLocationMgrE.LoadInProcessLocation(inProcessLocation.IpNo);
                    if (currentIp.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
                    {
                        throw new BusinessErrorException("InProcessLocation.Error.StatusErrorWhenReceive", currentIp.Status, currentIp.IpNo);
                    }

                    if (orderType == null)
                    {
                        orderType = inProcessLocation.OrderType;
                    }

                    //判断OrderHead的PartyFrom是否一致
                    if (partyFrom == null)
                    {
                        partyFrom = inProcessLocation.PartyFrom;
                    }
                    else if (inProcessLocation.PartyFrom.Code != partyFrom.Code)
                    {
                        throw new BusinessErrorException("Order.Error.ReceiveOrder.PartyFromNotEqual");
                    }

                    //判断OrderHead的PartyFrom是否一致
                    if (partyTo == null)
                    {
                        partyTo = inProcessLocation.PartyTo;
                    }
                    else if (inProcessLocation.PartyTo.Code != partyTo.Code)
                    {
                        throw new BusinessErrorException("Order.Error.ReceiveOrder.PartyToNotEqual");
                    }

                    //判断OrderHead的ShipFrom是否一致
                    if (shipFrom == null)
                    {
                        shipFrom = inProcessLocation.ShipFrom;
                    }
                    else if (!AddressHelper.IsAddressEqual(inProcessLocation.ShipFrom, shipFrom))
                    {
                        throw new BusinessErrorException("Order.Error.ReceiveOrder.ShipFromNotEqual");
                    }

                    //判断OrderHead的ShipTo是否一致
                    if (shipTo == null)
                    {
                        shipTo = inProcessLocation.ShipTo;
                    }
                    else if (!AddressHelper.IsAddressEqual(inProcessLocation.ShipTo, shipTo))
                    {
                        throw new BusinessErrorException("Order.Error.ReceiveOrder.ShipToNotEqual");
                    }

                    if (dockDescription == null)
                    {
                        dockDescription = inProcessLocation.DockDescription;
                    }
                    else if (inProcessLocation.DockDescription != dockDescription)
                    {
                        throw new BusinessErrorException("Order.Error.ReceiveOrder.DockDescriptionNotEqual");
                    }

                    //判断收货单打印模板是否一致
                    if (receiptTemplate == null)
                    {
                        receiptTemplate = inProcessLocation.ReceiptTemplate;
                    }
                    else
                    {
                        if (inProcessLocation.ReceiptTemplate != null && inProcessLocation.ReceiptTemplate != receiptTemplate)
                        {
                            throw new BusinessErrorException("Order.Error.ReceiveOrder.ReceiptTemplateNotEqual");
                        }
                    }

                    //判断条码打印模板是否一致
                    if (huTemplate == null)
                    {
                        huTemplate = inProcessLocation.HuTemplate;
                    }
                    else
                    {
                        if (inProcessLocation.HuTemplate != null && inProcessLocation.HuTemplate != huTemplate)
                        {
                            throw new BusinessErrorException("Order.Error.ReceiveOrder.HuTemplateNotEqual");
                        }
                    }

                    #region 查找收货差异处理选项
                    if (grGapTo == null)
                    {
                        grGapTo = inProcessLocation.GoodsReceiptGapTo;
                    }
                    else
                    {
                        if (inProcessLocation.GoodsReceiptGapTo != null && inProcessLocation.GoodsReceiptGapTo != grGapTo)
                        {
                            throw new BusinessErrorException("Order.Error.ReceiveOrder.GoodsReceiptGapToNotEqual");
                        }
                    }
                    #endregion

                    IListHelper.AddRange<InProcessLocationDetail>(
                        inProcessLocationDetailList, this.inProcessLocationDetailMgrE.GetInProcessLocationDetail(inProcessLocation));
                }
            }
            #endregion

            IList<ReceiptDetail> targetReceiptDetailList = receipt.ReceiptDetails;
            receipt.ReceiptDetails = null;   //清空Asn明细，稍后填充

            #region 创建收货单Head
            receipt.ReceiptNo = numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_RECEIPT);
            receipt.OrderType = orderType;
            receipt.CreateDate = DateTime.Now;
            receipt.CreateUser = user;
            receipt.PartyFrom = partyFrom;
            receipt.PartyTo = partyTo;
            receipt.ShipFrom = shipFrom;
            receipt.ShipTo = shipTo;
            receipt.DockDescription = dockDescription;
            receipt.ReceiptTemplate = receiptTemplate;
            receipt.HuTemplate = huTemplate;

            this.CreateReceipt(receipt);
            #endregion

            #region HU处理/入库操作/创建收货明细
            IList<LocationLotDetail> inspectLocationLotDetailList = new List<LocationLotDetail>();
            IList<LocationLotDetail> rejectInspectLocationLotDetailList = new List<LocationLotDetail>();
            string inspectLocation = null;
            string rejectLocation = null;
            foreach (ReceiptDetail receiptDetail in targetReceiptDetailList)
            {
                OrderLocationTransaction orderLocationTransaction = receiptDetail.OrderLocationTransaction;
                OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
                OrderHead orderHead = orderDetail.OrderHead;

                if (orderHead.CreateHuOption == BusinessConstants.CODE_MASTER_CREATE_HU_OPTION_VALUE_GR
                        && receiptDetail.HuId == null)  //如果订单设置为收货时创建Hu，但是收货时已经扫描过Hu了，按已扫描处理
                {
                    #region 收货时创建Hu
                    #region 生产本次收货+剩余零头生成Hu
                    decimal oddQty = 0;

                    if (!isOddCreateHu && orderDetail.HuLotSize.HasValue
                        && orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION) //只有生产支持零头
                    {
                        #region 查找剩余零头 + 本次收货数是否能够生成Hu
                        Hu oddHu = this.CreateHuFromOdd(receiptDetail, user);
                        if (oddHu != null)
                        {
                            //如果零头生成了Hu，本次收货数会扣减
                            #region 创建Hu
                            IList<Hu> oddHuList = new List<Hu>();
                            oddHuList.Add(oddHu);
                            IList<ReceiptDetail> oddReceiptDetailList = this.receiptDetailMgrE.CreateReceiptDetail(receipt, orderLocationTransaction, oddHuList);
                            #endregion

                            #region 入库
                            IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryIn(oddReceiptDetailList[0], user, receiptDetail.PutAwayBinCode);
                            #endregion

                            #region 是否检验
                            //if (orderDetail.NeedInspection 
                            if (inventoryTransactionList != null && inventoryTransactionList.Count > 0)
                            {
                                foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                                {
                                    if (inventoryTransaction.Qty > 0
                                        && ((orderDetail.Item.NeedInspect.HasValue && orderDetail.Item.NeedInspect.Value
                                        && orderHead.NeedInspection
                                        && orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML)
                                        || (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                                        && inventoryTransaction.QualityLevel != BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_2)))
                                    {
                                        if (inspectLocation == null)
                                        {
                                            inspectLocation = regionMgr.GetDefaultInspectLocation(orderHead.PartyTo.Code, orderLocationTransaction.InspectLocation);
                                        }
                                        else if (inspectLocation != regionMgr.GetDefaultInspectLocation(orderHead.PartyTo.Code, orderLocationTransaction.InspectLocation))
                                        {
                                            throw new BusinessErrorException("InspectOrder.Error.InspectLocationNotEqual");
                                        }

                                        if (rejectLocation == null)
                                        {
                                            rejectLocation = regionMgr.GetDefaultRejectLocation(orderHead.PartyTo.Code, orderLocationTransaction.RejectLocation);
                                        }
                                        else if (rejectLocation != regionMgr.GetDefaultRejectLocation(orderHead.PartyTo.Code, orderLocationTransaction.RejectLocation))
                                        {
                                            throw new BusinessErrorException("InspectOrder.Error.RejectLocationNotEqual");
                                        }

                                        LocationLotDetail locationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransaction.LocationLotDetailId);
                                        locationLotDetail.CurrentInspectQty = locationLotDetail.Qty;
                                        if (inventoryTransaction.QualityLevel != BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_2)
                                        {
                                            inspectLocationLotDetailList.Add(locationLotDetail);
                                        }
                                        else
                                        {
                                            rejectInspectLocationLotDetailList.Add(locationLotDetail);
                                        }
                                    }
                                }
                            }
                            else
                            {

                            }
                            #endregion
                        }
                        #endregion

                        oddQty = orderDetail.HuLotSize.HasValue ?
                            receiptDetail.ReceivedQty % orderDetail.HuLotSize.Value : 0;   //收货零头数

                        receiptDetail.ReceivedQty = receiptDetail.ReceivedQty - oddQty; //收货数量凑整                        
                    }
                    #endregion

                    #region 满包装/零头创建Hu处理
                    //创建Hu
                    IList<Hu> huList = this.huMgrE.CreateHu(receiptDetail, user);

                    //创建收货项
                    IList<ReceiptDetail> receiptDetailList = this.receiptDetailMgrE.CreateReceiptDetail(receipt, orderLocationTransaction, huList);

                    #region 如果还有废品收货，添加到收货列表
                    if (receiptDetail.ScrapQty > 0)
                    {
                        ReceiptDetail scrapReceiptDetail = new ReceiptDetail();
                        CloneHelper.CopyProperty(receiptDetail, scrapReceiptDetail);
                        scrapReceiptDetail.ReceivedQty = 0;
                        scrapReceiptDetail.RejectedQty = 0;
                        scrapReceiptDetail.PutAwayBinCode = null;
                        scrapReceiptDetail.Receipt = receipt;

                        this.receiptDetailMgrE.CreateReceiptDetail(scrapReceiptDetail);

                        receiptDetailList.Add(scrapReceiptDetail);
                        receipt.AddReceiptDetail(scrapReceiptDetail);
                    }
                    #endregion

                    foreach (ReceiptDetail huReceiptDetail in receiptDetailList)
                    {
                        #region 匹配ReceiptDetail和InProcessLocationDetail，Copy相关信息
                        if (orderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                        {
                            IList<InProcessLocationDetail> matchInProcessLocationDetailList = OrderHelper.FindMatchInProcessLocationDetail(receiptDetail, inProcessLocationDetailList);
                            if (matchInProcessLocationDetailList != null && matchInProcessLocationDetailList.Count > 0)
                            {
                                if (matchInProcessLocationDetailList.Count > 1)
                                {
                                    //只有当ASN中包含条码，按数量收货，并收货后创建条码才有可能发生这种情况。
                                    //变态才这么干。
                                    throw new BusinessErrorException("你是变态才这么设置。");
                                }

                                //如果找到匹配项，只可能有一个
                                huReceiptDetail.PlannedBill = matchInProcessLocationDetailList[0].PlannedBill;
                                huReceiptDetail.IsConsignment = matchInProcessLocationDetailList[0].IsConsignment;
                                huReceiptDetail.ShippedQty = matchInProcessLocationDetailList[0].Qty;

                                this.receiptDetailMgrE.UpdateReceiptDetail(huReceiptDetail);
                            }

                            //收货创建HU，分配PlannedAmount，todo：考虑余数
                            //huReceiptDetail.PlannedAmount = receiptDetail.PlannedAmount / receiptDetail.ReceivedQty.Value * huReceiptDetail.ReceivedQty.Value;
                        }
                        #endregion

                        #region 入库
                        IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryIn(huReceiptDetail, user, receiptDetail.PutAwayBinCode);
                        #endregion

                        #region 是否检验
                        //if ((orderDetail.NeedInspection
                        if (inventoryTransactionList != null && inventoryTransactionList.Count > 0
                            && huReceiptDetail.ReceivedQty > 0)
                        {
                            foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                            {
                                if (inventoryTransaction.Qty > 0
                                    && ((orderDetail.Item.NeedInspect.HasValue && orderDetail.Item.NeedInspect.Value
                            && orderHead.NeedInspection
                            && orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML)
                            || (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                                    && inventoryTransaction.QualityLevel == BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_2)))
                                {
                                    if (inspectLocation == null)
                                    {
                                        inspectLocation = regionMgr.GetDefaultInspectLocation(orderHead.PartyTo.Code, orderLocationTransaction.InspectLocation);
                                    }
                                    else if (inspectLocation != regionMgr.GetDefaultInspectLocation(orderHead.PartyTo.Code, orderLocationTransaction.InspectLocation))
                                    {
                                        throw new BusinessErrorException("InspectOrder.Error.InspectLocationNotEqual");
                                    }

                                    if (rejectLocation == null)
                                    {
                                        rejectLocation = regionMgr.GetDefaultRejectLocation(orderHead.PartyTo.Code, orderLocationTransaction.RejectLocation);
                                    }
                                    else if (rejectLocation != regionMgr.GetDefaultRejectLocation(orderHead.PartyTo.Code, orderLocationTransaction.RejectLocation))
                                    {
                                        throw new BusinessErrorException("InspectOrder.Error.RejectLocationNotEqual");
                                    }

                                    LocationLotDetail locationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransaction.LocationLotDetailId);
                                    locationLotDetail.CurrentInspectQty = inventoryTransaction.Qty;
                                    if (inventoryTransaction.QualityLevel != BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_2)
                                    {
                                        inspectLocationLotDetailList.Add(locationLotDetail);
                                    }
                                    else
                                    {
                                        rejectInspectLocationLotDetailList.Add(locationLotDetail);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region 生产剩余零头处理
                    if (oddQty > 0)
                    {
                        ReceiptDetail oddReceiptDetail = new ReceiptDetail();
                        CloneHelper.CopyProperty(receiptDetail, oddReceiptDetail);

                        oddReceiptDetail.ReceivedQty = oddQty;
                        oddReceiptDetail.RejectedQty = 0;
                        oddReceiptDetail.ScrapQty = 0;

                        #region 零头入库
                        oddReceiptDetail.Receipt = receipt;
                        IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryIn(oddReceiptDetail, user, receiptDetail.PutAwayBinCode);
                        #endregion

                        #region 零头创建收货明细
                        this.receiptDetailMgrE.CreateReceiptDetail(oddReceiptDetail);
                        receipt.AddReceiptDetail(oddReceiptDetail);
                        #endregion

                        #region 创建HuOdd
                        LocationLotDetail locationLotDetail = locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransactionList[0].LocationLotDetailId);
                        this.huOddMgrE.CreateHuOdd(oddReceiptDetail, locationLotDetail, user);
                        #endregion
                    }
                    #endregion

                    #endregion
                }
                else
                {
                    #region 收货时不创建Hu

                    #region 更新Hu上的OrderNo、ReceiptNo
                    if (receiptDetail.HuId != null && receiptDetail.HuId.Trim() != string.Empty)
                    {
                        Hu hu = this.huMgrE.LoadHu(receiptDetail.HuId);
                        bool isUpdated = false;

                        if (hu.OrderNo == null || hu.ReceiptNo == null)
                        {
                            if (hu.OrderNo == null)
                            {
                                hu.OrderNo = orderHead.OrderNo;
                            }

                            if (hu.ReceiptNo == null)
                            {
                                hu.ReceiptNo = receipt.ReceiptNo;
                            }

                            isUpdated = true;
                        }

                        if (isUpdated)
                        {
                            this.huMgrE.UpdateHu(hu);
                        }
                    }
                    #endregion

                    IList<ReceiptDetail> noCreateHuReceiptDetailList = new List<ReceiptDetail>();

                    #region 匹配ReceiptDetail和InProcessLocationDetail，Copy相关信息
                    if (orderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                        && orderHead.SubType != BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ)  //收货调整已经匹配过InProcessLocationDetail，不需要在匹配
                    {
                        IList<InProcessLocationDetail> matchInProcessLocationDetailList = OrderHelper.FindMatchInProcessLocationDetail(receiptDetail, inProcessLocationDetailList);

                        if (matchInProcessLocationDetailList != null && matchInProcessLocationDetailList.Count == 1)
                        {
                            //一次收货对应一次发货。
                            receiptDetail.PlannedBill = matchInProcessLocationDetailList[0].PlannedBill;
                            receiptDetail.IsConsignment = matchInProcessLocationDetailList[0].IsConsignment;
                            if (matchInProcessLocationDetailList[0].InProcessLocation.Type == BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_GAP)
                            {
                                receiptDetail.ShippedQty = 0 - matchInProcessLocationDetailList[0].Qty;
                            }
                            else
                            {
                                receiptDetail.ShippedQty = matchInProcessLocationDetailList[0].Qty;
                            }
                            receiptDetail.ReceivedInProcessLocationDetail = matchInProcessLocationDetailList[0];
                            noCreateHuReceiptDetailList.Add(receiptDetail);
                        }
                        else if (matchInProcessLocationDetailList != null && matchInProcessLocationDetailList.Count > 1)
                        {
                            //一次收货对应多次发货。
                            //如：发货按条码，收货按数量。
                            decimal totalRecQty = receiptDetail.ReceivedQty;
                            InProcessLocationDetail lastInProcessLocationDetail = null;
                            foreach (InProcessLocationDetail inProcessLocationDetail in matchInProcessLocationDetailList)
                            {
                                lastInProcessLocationDetail = inProcessLocationDetail; //记录最后一次发货项，供没有对应发货的收货项使用

                                if (Math.Abs(inProcessLocationDetail.ReceivedQty) >= Math.Abs(inProcessLocationDetail.Qty))
                                {
                                    continue;
                                }

                                if (Math.Abs(totalRecQty) > 0)
                                {
                                    ReceiptDetail clonedReceiptDetail = new ReceiptDetail();
                                    CloneHelper.CopyProperty(receiptDetail, clonedReceiptDetail);

                                    clonedReceiptDetail.PlannedBill = inProcessLocationDetail.PlannedBill;
                                    clonedReceiptDetail.IsConsignment = inProcessLocationDetail.IsConsignment;

                                    #region
                                    if (matchInProcessLocationDetailList[0].InProcessLocation.Type == BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_GAP)
                                    {
                                        inProcessLocationDetail.Qty = 0 - inProcessLocationDetail.Qty;
                                    }
                                    #endregion

                                    if (Math.Abs(totalRecQty) > Math.Abs(inProcessLocationDetail.Qty - inProcessLocationDetail.ReceivedQty))
                                    {
                                        clonedReceiptDetail.ReceivedQty = inProcessLocationDetail.Qty - inProcessLocationDetail.ReceivedQty;
                                        clonedReceiptDetail.ShippedQty = inProcessLocationDetail.Qty - inProcessLocationDetail.ReceivedQty;
                                        totalRecQty -= inProcessLocationDetail.Qty - inProcessLocationDetail.ReceivedQty;
                                    }
                                    else
                                    {
                                        clonedReceiptDetail.ReceivedQty = totalRecQty;
                                        clonedReceiptDetail.ShippedQty = totalRecQty;
                                        totalRecQty = 0;
                                    }

                                    //因为去掉了数量，记录已经匹配的发货项，避免差异处理的时候匹配多条而产生差异。
                                    clonedReceiptDetail.ReceivedInProcessLocationDetail = inProcessLocationDetail;

                                    noCreateHuReceiptDetailList.Add(clonedReceiptDetail);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            //超收，没有找到对应的发货项，只记录收货数，发货数记0
                            if (Math.Abs(totalRecQty) > 0)
                            {
                                ReceiptDetail clonedReceiptDetail = new ReceiptDetail();
                                CloneHelper.CopyProperty(receiptDetail, clonedReceiptDetail);

                                clonedReceiptDetail.ShippedQty = 0;
                                clonedReceiptDetail.ReceivedQty = totalRecQty;
                                clonedReceiptDetail.ReceivedInProcessLocationDetail = lastInProcessLocationDetail;

                                noCreateHuReceiptDetailList.Add(clonedReceiptDetail);
                            }
                        }
                        else
                        {
                            noCreateHuReceiptDetailList.Add(receiptDetail);
                        }
                    }
                    else
                    {
                        noCreateHuReceiptDetailList.Add(receiptDetail);
                    }
                    #endregion

                    foreach (ReceiptDetail noCreateHuReceiptDetail in noCreateHuReceiptDetailList)
                    {
                        noCreateHuReceiptDetail.Receipt = receipt;

                        if (noCreateHuReceiptDetail.ReceivedQty != 0 || noCreateHuReceiptDetail.RejectedQty != 0)
                        {
                            #region 入库
                            IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryIn(noCreateHuReceiptDetail, user, noCreateHuReceiptDetail.PutAwayBinCode);
                            #endregion

                            #region 是否检验
                            //if ((orderDetail.NeedInspection 
                            if (inventoryTransactionList != null && inventoryTransactionList.Count > 0)
                            {
                                foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                                {
                                    if (inventoryTransaction.Qty > 0
                                        && ((orderDetail.Item.NeedInspect.HasValue && orderDetail.Item.NeedInspect.Value
                                            && orderHead.NeedInspection && orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML)
                                       || (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                                       && inventoryTransaction.QualityLevel == BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_2)))
                                    {
                                        if (inspectLocation == null)
                                        {
                                            inspectLocation = regionMgr.GetDefaultInspectLocation(orderHead.PartyTo.Code, orderLocationTransaction.InspectLocation);
                                        }
                                        else if (inspectLocation != regionMgr.GetDefaultInspectLocation(orderHead.PartyTo.Code, orderLocationTransaction.InspectLocation))
                                        {
                                            throw new BusinessErrorException("InspectOrder.Error.InspectLocationNotEqual");
                                        }

                                        if (rejectLocation == null)
                                        {
                                            rejectLocation = regionMgr.GetDefaultRejectLocation(orderHead.PartyTo.Code, orderLocationTransaction.RejectLocation);
                                        }
                                        else if (rejectLocation != regionMgr.GetDefaultRejectLocation(orderHead.PartyTo.Code, orderLocationTransaction.RejectLocation))
                                        {
                                            throw new BusinessErrorException("InspectOrder.Error.RejectLocationNotEqual");
                                        }

                                        LocationLotDetail locationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransaction.LocationLotDetailId);
                                        locationLotDetail.CurrentInspectQty = inventoryTransaction.Qty;
                                        if (inventoryTransaction.QualityLevel != BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_2)
                                        {
                                            inspectLocationLotDetailList.Add(locationLotDetail);
                                        }
                                        else
                                        {
                                            rejectInspectLocationLotDetailList.Add(locationLotDetail);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        #region 创建收货明细
                        this.receiptDetailMgrE.CreateReceiptDetail(noCreateHuReceiptDetail);
                        receipt.AddReceiptDetail(noCreateHuReceiptDetail);
                        #endregion
                    }

                    #endregion
                }
            }
            #endregion

            #region 检验
            if (inspectLocationLotDetailList.Count > 0)
            {
                //对于没有Hu的，如果收货时已经回冲了负数库存，也就是库存数量和待检验数量不一致可能会有问题
                //增加ipno，receiptno，isseperated字段
                this.inspectOrderMgrE.CreateInspectOrder(inspectLocationLotDetailList, inspectLocation, rejectLocation, user, receipt.InProcessLocations[0].IpNo, receipt.ReceiptNo, false);
            }

            if (rejectInspectLocationLotDetailList.Count > 0)
            {
                //可疑品
                //对于没有Hu的，如果收货时已经回冲了负数库存，也就是库存数量和待检验数量不一致可能会有问题
                //增加ipno，receiptno，isseperated字段
                this.inspectOrderMgrE.CreateInspectOrder(rejectInspectLocationLotDetailList, inspectLocation, rejectLocation, user, receipt.InProcessLocations[0].IpNo, receipt.ReceiptNo, true);
            }
            #endregion

            //#region 匹配收货发货项，查找差异
            //IList<InProcessLocationDetail> gapInProcessLocationDetailList = new List<InProcessLocationDetail>();

            //#region 发货项不匹配
            //foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocationDetailList)
            //{
            //    if (inProcessLocationDetail.OrderLocationTransaction.OrderDetail.OrderHead.Type
            //        != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)   //生产暂时不支持差异
            //    {
            //        decimal receivedQty = 0;  //发货项的累计收货数

            //        //一条发货项可能对应多条收货项
            //        foreach (ReceiptDetail receiptDetail in receipt.ReceiptDetails)
            //        {
            //            //匹配收货项和发货项
            //            if (receiptDetail.ReceivedInProcessLocationDetail != null)
            //            {
            //                //对于已经匹配的，直接按发货项匹配
            //                if (receiptDetail.ReceivedInProcessLocationDetail.Id == inProcessLocationDetail.Id)
            //                {
            //                    if (receiptDetail.ReceivedQty.HasValue)
            //                    {
            //                        receivedQty += receiptDetail.ReceivedQty.Value;
            //                    }
            //                }
            //            }
            //            else if (OrderHelper.IsInProcessLocationDetailMatchReceiptDetail(
            //                inProcessLocationDetail, receiptDetail))
            //            {
            //                if (receiptDetail.ReceivedQty.HasValue)
            //                {
            //                    receivedQty += receiptDetail.ReceivedQty.Value;
            //                }
            //            }
            //        }

            //        if (receivedQty != inProcessLocationDetail.Qty)
            //        {
            //            #region 收货数量和发货数量不匹配，记录差异
            //            InProcessLocationDetail gapInProcessLocationDetail = new InProcessLocationDetail();
            //            gapInProcessLocationDetail.Qty = receivedQty - inProcessLocationDetail.Qty;
            //            gapInProcessLocationDetail.OrderLocationTransaction = inProcessLocationDetail.OrderLocationTransaction;
            //            //gapInProcessLocationDetail.HuId = inProcessLocationDetail.HuId;
            //            gapInProcessLocationDetail.LotNo = inProcessLocationDetail.LotNo;
            //            gapInProcessLocationDetail.IsConsignment = inProcessLocationDetail.IsConsignment;
            //            gapInProcessLocationDetail.PlannedBill = inProcessLocationDetail.PlannedBill;

            //            gapInProcessLocationDetailList.Add(gapInProcessLocationDetail);
            //            #endregion
            //        }
            //    }
            //}
            //#endregion

            //#region 收货项不匹配
            //foreach (ReceiptDetail receiptDetail in receipt.ReceiptDetails)
            //{
            //    if (receiptDetail.OrderLocationTransaction.OrderDetail.OrderHead.Type
            //        != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)   //生产暂时不支持差异
            //    {
            //        IList<InProcessLocationDetail> matchInProcessLocationDetailList = OrderHelper.FindMatchInProcessLocationDetail(receiptDetail, inProcessLocationDetailList);

            //        if (matchInProcessLocationDetailList == null || matchInProcessLocationDetailList.Count == 0)
            //        {
            //            OrderLocationTransaction outOrderLocationTransaction =
            //                this.orderLocationTransactionMgrE.GetOrderLocationTransaction(receiptDetail.OrderLocationTransaction.OrderDetail, BusinessConstants.IO_TYPE_OUT)[0];
            //            #region 没有找到和收货项对应的发货项
            //            InProcessLocationDetail gapInProcessLocationDetail = new InProcessLocationDetail();
            //            gapInProcessLocationDetail.Qty = receiptDetail.ReceivedQty.Value;
            //            gapInProcessLocationDetail.OrderLocationTransaction = outOrderLocationTransaction;
            //            //gapInProcessLocationDetail.HuId = receiptDetail.HuId;
            //            gapInProcessLocationDetail.LotNo = receiptDetail.LotNo;
            //            gapInProcessLocationDetail.IsConsignment = receiptDetail.IsConsignment;
            //            gapInProcessLocationDetail.PlannedBill = receiptDetail.PlannedBill;

            //            gapInProcessLocationDetailList.Add(gapInProcessLocationDetail);
            //            #endregion
            //        }
            //    }
            //}
            //#endregion
            //#endregion

            #region 关闭InProcessLocation
            if (receipt.InProcessLocations != null && receipt.InProcessLocations.Count > 0)
            {
                foreach (InProcessLocation inProcessLocation in receipt.InProcessLocations)
                {
                    if (inProcessLocation.IsAsnUniqueReceipt)
                    {
                        //不支持多次收货直接关闭                      
                        this.inProcessLocationMgrE.CloseInProcessLocation(inProcessLocation, user);
                    }
                    else
                    {
                        this.inProcessLocationMgrE.TryCloseInProcessLocation(inProcessLocation, user);
                    }
                }
            }
            #endregion

            #region 记录成本消耗成本
            if (receipt.OrderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                this.costMgr.RecordProductionCostTransaction(receipt, user);
            }
            #endregion
        }

        [Transaction(TransactionMode.Unspecified)]
        public Receipt LoadReceipt(string receiptNo, User user)
        {
            return LoadReceipt(receiptNo, user, false, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Receipt LoadReceipt(string receiptNo, bool includeDetail)
        {
            return LoadReceipt(receiptNo, null, includeDetail, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Receipt LoadReceipt(string receiptNo, User user, bool includeDetail)
        {
            return LoadReceipt(receiptNo, user, includeDetail, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Receipt LoadReceipt(string receiptNo, bool includeDetail, bool includeInProcessLocations)
        {
            return LoadReceipt(receiptNo, null, includeDetail, includeInProcessLocations);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Receipt LoadReceipt(string receiptNo, User user, bool includeDetail, bool includeInProcessLocations)
        {
            Receipt receipt = this.LoadReceipt(receiptNo);

            if (user != null)
            {
                CheckReceiptOperationAuthrize(receipt, user);
            }

            if (includeDetail && receipt.ReceiptDetails != null && receipt.ReceiptDetails.Count > 0)
            {
            }
            if (includeInProcessLocations && receipt.InProcessLocations != null && receipt.InProcessLocations.Count > 0)
            {
            }
            return receipt;
        }

        [Transaction(TransactionMode.Unspecified)]
        public Receipt CheckAndLoadReceipt(string receiptNo)
        {
            Receipt receipt = this.LoadReceipt(receiptNo);
            if (receipt == null)
            {
                throw new BusinessErrorException("Common.Business.Error.EntityNotExist", receiptNo);
            }
            return receipt;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Receipt> GetReceipts(string userCode, int firstRow, int maxRows, params string[] orderTypes)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Receipt));
            //todo 权限校验
            //SecurityHelper.SetPartySearchCriteria(criteria, "PartyFrom.Code", userCode);
            //SecurityHelper.SetPartySearchCriteria(criteria, "PartyTo.Code", userCode);
            criteria.Add(Expression.Eq("CreateUser.Code", userCode));
            if (orderTypes.Length == 1)
            {
                criteria.Add(Expression.Eq("OrderType", orderTypes[0]));
            }
            else
            {
                criteria.Add(Expression.In("OrderType", orderTypes));
            }
            criteria.Add(Expression.Ge("CreateDate", DateTime.Today));
            criteria.AddOrder(Order.Desc("ReceiptNo"));
            IList<Receipt> receiptList = criteriaMgrE.FindAll<Receipt>(criteria, firstRow, maxRows);
            if (receiptList.Count > 0)
            {
                return receiptList;
            }
            return null;
        }

        #endregion Customized Methods

        #region Private Methods
        private Hu CreateHuFromOdd(ReceiptDetail receiptDetail, User user)
        {
            OrderLocationTransaction orderLocationTransaction = receiptDetail.OrderLocationTransaction;
            OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
            OrderHead orderHead = orderDetail.OrderHead;
            Receipt receipt = receiptDetail.Receipt;

            int huLotSize = orderDetail.HuLotSize.Value;
            decimal qty = 0; //累计库存Odd数量

            #region 循环获取库存Odd数量
            IList<HuOdd> huOddList = this.huOddMgrE.GetHuOdd(
                orderDetail.Item, orderDetail.UnitCount, orderDetail.Uom,
                orderDetail.DefaultLocationFrom, orderDetail.DefaultLocationTo,
                BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE);

            if (huOddList != null && huOddList.Count > 0)
            {
                foreach (HuOdd huOdd in huOddList)
                {
                    qty += huOdd.OddQty - huOdd.CreatedQty;
                }

                if (qty + receiptDetail.ReceivedQty >= huLotSize)
                {
                    #region 满足批量，创建Hu
                    DateTime dateTimeNow = DateTime.Now;

                    #region 更新库存Odd，出库
                    foreach (HuOdd huOdd in huOddList)
                    {
                        //全部关闭
                        huOdd.CurrentCreateQty = huOdd.OddQty - huOdd.CreatedQty;
                        huOdd.CreatedQty = huOdd.OddQty;
                        huOdd.LastModifyDate = dateTimeNow;
                        huOdd.LastModifyUser = user;
                        huOdd.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;

                        this.huOddMgrE.UpdateHuOdd(huOdd);

                        //出库
                        this.locationMgrE.InventoryOut(huOdd, receiptDetail, user);
                    }
                    #endregion

                    #region 可能还有零头，需要更新receiptDetail收货数量，等于收货数 + 库存Odd数 - Hu批量
                    receiptDetail.ReceivedQty = qty + receiptDetail.ReceivedQty - huLotSize;
                    #endregion

                    #region 创建Hu
                    ReceiptDetail clonedReceiptDetail = new ReceiptDetail();
                    CloneHelper.CopyProperty(receiptDetail, clonedReceiptDetail);
                    clonedReceiptDetail.ReceivedQty = huLotSize;
                    clonedReceiptDetail.RejectedQty = 0;
                    IList<Hu> huList = this.huMgrE.CreateHu(clonedReceiptDetail, user);

                    return huList[0];
                    #endregion

                    #endregion
                }
            }

            return null;

            #endregion
        }

        private void CheckReceiptOperationAuthrize(Receipt receipt, User user)
        {
            if (receipt == null)
            {
                throw new BusinessErrorException("MasterData.Receipt.NotExit");
            }
            //bool partyFromAuthrized = false;
            bool partyToAuthrized = false;
            foreach (Permission permission in user.Permissions)
            {
                //if (permission.Code == receipt.PartyFrom.Code)
                //{
                //    partyFromAuthrized = true;
                //}

                if (permission.Code == receipt.PartyTo.Code)
                {
                    partyToAuthrized = true;
                    break;
                }

                //if (partyFromAuthrized && partyToAuthrized)
                //{
                //    break;
                //}
            }

            //if (!(partyFromAuthrized && partyToAuthrized))
            if (!partyToAuthrized)
            {
                //没有该asn的操作权限
                throw new BusinessErrorException("Receipt.Error.NoAuthrization", receipt.ReceiptNo);

            }
        }

        #endregion
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ReceiptMgrE : com.Sconit.Service.MasterData.Impl.ReceiptMgr, IReceiptMgrE
    {

    }
}
#endregion