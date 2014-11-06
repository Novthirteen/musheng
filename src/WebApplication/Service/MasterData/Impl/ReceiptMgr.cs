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
            #region ��Receipt�ϼ�¼�ο�Asn��Ϊ�ִ�
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

            #region �����ջ��ͷ�����ϵ
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
            #region �������еķ�����ջ�����ӡģ�壬�ջ����촦��ѡ��
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

                    //�ж�OrderHead��PartyFrom�Ƿ�һ��
                    if (partyFrom == null)
                    {
                        partyFrom = inProcessLocation.PartyFrom;
                    }
                    else if (inProcessLocation.PartyFrom.Code != partyFrom.Code)
                    {
                        throw new BusinessErrorException("Order.Error.ReceiveOrder.PartyFromNotEqual");
                    }

                    //�ж�OrderHead��PartyFrom�Ƿ�һ��
                    if (partyTo == null)
                    {
                        partyTo = inProcessLocation.PartyTo;
                    }
                    else if (inProcessLocation.PartyTo.Code != partyTo.Code)
                    {
                        throw new BusinessErrorException("Order.Error.ReceiveOrder.PartyToNotEqual");
                    }

                    //�ж�OrderHead��ShipFrom�Ƿ�һ��
                    if (shipFrom == null)
                    {
                        shipFrom = inProcessLocation.ShipFrom;
                    }
                    else if (!AddressHelper.IsAddressEqual(inProcessLocation.ShipFrom, shipFrom))
                    {
                        throw new BusinessErrorException("Order.Error.ReceiveOrder.ShipFromNotEqual");
                    }

                    //�ж�OrderHead��ShipTo�Ƿ�һ��
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

                    //�ж��ջ�����ӡģ���Ƿ�һ��
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

                    //�ж������ӡģ���Ƿ�һ��
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

                    #region �����ջ����촦��ѡ��
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
            receipt.ReceiptDetails = null;   //���Asn��ϸ���Ժ����

            #region �����ջ���Head
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

            #region HU����/������/�����ջ���ϸ
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
                        && receiptDetail.HuId == null)  //�����������Ϊ�ջ�ʱ����Hu�������ջ�ʱ�Ѿ�ɨ���Hu�ˣ�����ɨ�账��
                {
                    #region �ջ�ʱ����Hu
                    #region ���������ջ�+ʣ����ͷ����Hu
                    decimal oddQty = 0;

                    if (!isOddCreateHu && orderDetail.HuLotSize.HasValue
                        && orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION) //ֻ������֧����ͷ
                    {
                        #region ����ʣ����ͷ + �����ջ����Ƿ��ܹ�����Hu
                        Hu oddHu = this.CreateHuFromOdd(receiptDetail, user);
                        if (oddHu != null)
                        {
                            //�����ͷ������Hu�������ջ�����ۼ�
                            #region ����Hu
                            IList<Hu> oddHuList = new List<Hu>();
                            oddHuList.Add(oddHu);
                            IList<ReceiptDetail> oddReceiptDetailList = this.receiptDetailMgrE.CreateReceiptDetail(receipt, orderLocationTransaction, oddHuList);
                            #endregion

                            #region ���
                            IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryIn(oddReceiptDetailList[0], user, receiptDetail.PutAwayBinCode);
                            #endregion

                            #region �Ƿ����
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
                            receiptDetail.ReceivedQty % orderDetail.HuLotSize.Value : 0;   //�ջ���ͷ��

                        receiptDetail.ReceivedQty = receiptDetail.ReceivedQty - oddQty; //�ջ���������                        
                    }
                    #endregion

                    #region ����װ/��ͷ����Hu����
                    //����Hu
                    IList<Hu> huList = this.huMgrE.CreateHu(receiptDetail, user);

                    //�����ջ���
                    IList<ReceiptDetail> receiptDetailList = this.receiptDetailMgrE.CreateReceiptDetail(receipt, orderLocationTransaction, huList);

                    #region ������з�Ʒ�ջ�����ӵ��ջ��б�
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
                        #region ƥ��ReceiptDetail��InProcessLocationDetail��Copy�����Ϣ
                        if (orderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                        {
                            IList<InProcessLocationDetail> matchInProcessLocationDetailList = OrderHelper.FindMatchInProcessLocationDetail(receiptDetail, inProcessLocationDetailList);
                            if (matchInProcessLocationDetailList != null && matchInProcessLocationDetailList.Count > 0)
                            {
                                if (matchInProcessLocationDetailList.Count > 1)
                                {
                                    //ֻ�е�ASN�а������룬�������ջ������ջ��󴴽�������п��ܷ������������
                                    //��̬����ô�ɡ�
                                    throw new BusinessErrorException("���Ǳ�̬����ô���á�");
                                }

                                //����ҵ�ƥ���ֻ������һ��
                                huReceiptDetail.PlannedBill = matchInProcessLocationDetailList[0].PlannedBill;
                                huReceiptDetail.IsConsignment = matchInProcessLocationDetailList[0].IsConsignment;
                                huReceiptDetail.ShippedQty = matchInProcessLocationDetailList[0].Qty;

                                this.receiptDetailMgrE.UpdateReceiptDetail(huReceiptDetail);
                            }

                            //�ջ�����HU������PlannedAmount��todo����������
                            //huReceiptDetail.PlannedAmount = receiptDetail.PlannedAmount / receiptDetail.ReceivedQty.Value * huReceiptDetail.ReceivedQty.Value;
                        }
                        #endregion

                        #region ���
                        IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryIn(huReceiptDetail, user, receiptDetail.PutAwayBinCode);
                        #endregion

                        #region �Ƿ����
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

                    #region ����ʣ����ͷ����
                    if (oddQty > 0)
                    {
                        ReceiptDetail oddReceiptDetail = new ReceiptDetail();
                        CloneHelper.CopyProperty(receiptDetail, oddReceiptDetail);

                        oddReceiptDetail.ReceivedQty = oddQty;
                        oddReceiptDetail.RejectedQty = 0;
                        oddReceiptDetail.ScrapQty = 0;

                        #region ��ͷ���
                        oddReceiptDetail.Receipt = receipt;
                        IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryIn(oddReceiptDetail, user, receiptDetail.PutAwayBinCode);
                        #endregion

                        #region ��ͷ�����ջ���ϸ
                        this.receiptDetailMgrE.CreateReceiptDetail(oddReceiptDetail);
                        receipt.AddReceiptDetail(oddReceiptDetail);
                        #endregion

                        #region ����HuOdd
                        LocationLotDetail locationLotDetail = locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransactionList[0].LocationLotDetailId);
                        this.huOddMgrE.CreateHuOdd(oddReceiptDetail, locationLotDetail, user);
                        #endregion
                    }
                    #endregion

                    #endregion
                }
                else
                {
                    #region �ջ�ʱ������Hu

                    #region ����Hu�ϵ�OrderNo��ReceiptNo
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

                    #region ƥ��ReceiptDetail��InProcessLocationDetail��Copy�����Ϣ
                    if (orderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                        && orderHead.SubType != BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ)  //�ջ������Ѿ�ƥ���InProcessLocationDetail������Ҫ��ƥ��
                    {
                        IList<InProcessLocationDetail> matchInProcessLocationDetailList = OrderHelper.FindMatchInProcessLocationDetail(receiptDetail, inProcessLocationDetailList);

                        if (matchInProcessLocationDetailList != null && matchInProcessLocationDetailList.Count == 1)
                        {
                            //һ���ջ���Ӧһ�η�����
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
                            //һ���ջ���Ӧ��η�����
                            //�磺���������룬�ջ���������
                            decimal totalRecQty = receiptDetail.ReceivedQty;
                            InProcessLocationDetail lastInProcessLocationDetail = null;
                            foreach (InProcessLocationDetail inProcessLocationDetail in matchInProcessLocationDetailList)
                            {
                                lastInProcessLocationDetail = inProcessLocationDetail; //��¼���һ�η������û�ж�Ӧ�������ջ���ʹ��

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

                                    //��Ϊȥ������������¼�Ѿ�ƥ��ķ����������촦���ʱ��ƥ��������������졣
                                    clonedReceiptDetail.ReceivedInProcessLocationDetail = inProcessLocationDetail;

                                    noCreateHuReceiptDetailList.Add(clonedReceiptDetail);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            //���գ�û���ҵ���Ӧ�ķ����ֻ��¼�ջ�������������0
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
                            #region ���
                            IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryIn(noCreateHuReceiptDetail, user, noCreateHuReceiptDetail.PutAwayBinCode);
                            #endregion

                            #region �Ƿ����
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

                        #region �����ջ���ϸ
                        this.receiptDetailMgrE.CreateReceiptDetail(noCreateHuReceiptDetail);
                        receipt.AddReceiptDetail(noCreateHuReceiptDetail);
                        #endregion
                    }

                    #endregion
                }
            }
            #endregion

            #region ����
            if (inspectLocationLotDetailList.Count > 0)
            {
                //����û��Hu�ģ�����ջ�ʱ�Ѿ��س��˸�����棬Ҳ���ǿ�������ʹ�����������һ�¿��ܻ�������
                //����ipno��receiptno��isseperated�ֶ�
                this.inspectOrderMgrE.CreateInspectOrder(inspectLocationLotDetailList, inspectLocation, rejectLocation, user, receipt.InProcessLocations[0].IpNo, receipt.ReceiptNo, false);
            }

            if (rejectInspectLocationLotDetailList.Count > 0)
            {
                //����Ʒ
                //����û��Hu�ģ�����ջ�ʱ�Ѿ��س��˸�����棬Ҳ���ǿ�������ʹ�����������һ�¿��ܻ�������
                //����ipno��receiptno��isseperated�ֶ�
                this.inspectOrderMgrE.CreateInspectOrder(rejectInspectLocationLotDetailList, inspectLocation, rejectLocation, user, receipt.InProcessLocations[0].IpNo, receipt.ReceiptNo, true);
            }
            #endregion

            //#region ƥ���ջ���������Ҳ���
            //IList<InProcessLocationDetail> gapInProcessLocationDetailList = new List<InProcessLocationDetail>();

            //#region �����ƥ��
            //foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocationDetailList)
            //{
            //    if (inProcessLocationDetail.OrderLocationTransaction.OrderDetail.OrderHead.Type
            //        != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)   //������ʱ��֧�ֲ���
            //    {
            //        decimal receivedQty = 0;  //��������ۼ��ջ���

            //        //һ����������ܶ�Ӧ�����ջ���
            //        foreach (ReceiptDetail receiptDetail in receipt.ReceiptDetails)
            //        {
            //            //ƥ���ջ���ͷ�����
            //            if (receiptDetail.ReceivedInProcessLocationDetail != null)
            //            {
            //                //�����Ѿ�ƥ��ģ�ֱ�Ӱ�������ƥ��
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
            //            #region �ջ������ͷ���������ƥ�䣬��¼����
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

            //#region �ջ��ƥ��
            //foreach (ReceiptDetail receiptDetail in receipt.ReceiptDetails)
            //{
            //    if (receiptDetail.OrderLocationTransaction.OrderDetail.OrderHead.Type
            //        != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)   //������ʱ��֧�ֲ���
            //    {
            //        IList<InProcessLocationDetail> matchInProcessLocationDetailList = OrderHelper.FindMatchInProcessLocationDetail(receiptDetail, inProcessLocationDetailList);

            //        if (matchInProcessLocationDetailList == null || matchInProcessLocationDetailList.Count == 0)
            //        {
            //            OrderLocationTransaction outOrderLocationTransaction =
            //                this.orderLocationTransactionMgrE.GetOrderLocationTransaction(receiptDetail.OrderLocationTransaction.OrderDetail, BusinessConstants.IO_TYPE_OUT)[0];
            //            #region û���ҵ����ջ����Ӧ�ķ�����
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

            #region �ر�InProcessLocation
            if (receipt.InProcessLocations != null && receipt.InProcessLocations.Count > 0)
            {
                foreach (InProcessLocation inProcessLocation in receipt.InProcessLocations)
                {
                    if (inProcessLocation.IsAsnUniqueReceipt)
                    {
                        //��֧�ֶ���ջ�ֱ�ӹر�                      
                        this.inProcessLocationMgrE.CloseInProcessLocation(inProcessLocation, user);
                    }
                    else
                    {
                        this.inProcessLocationMgrE.TryCloseInProcessLocation(inProcessLocation, user);
                    }
                }
            }
            #endregion

            #region ��¼�ɱ����ĳɱ�
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
            //todo Ȩ��У��
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
            decimal qty = 0; //�ۼƿ��Odd����

            #region ѭ����ȡ���Odd����
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
                    #region ��������������Hu
                    DateTime dateTimeNow = DateTime.Now;

                    #region ���¿��Odd������
                    foreach (HuOdd huOdd in huOddList)
                    {
                        //ȫ���ر�
                        huOdd.CurrentCreateQty = huOdd.OddQty - huOdd.CreatedQty;
                        huOdd.CreatedQty = huOdd.OddQty;
                        huOdd.LastModifyDate = dateTimeNow;
                        huOdd.LastModifyUser = user;
                        huOdd.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;

                        this.huOddMgrE.UpdateHuOdd(huOdd);

                        //����
                        this.locationMgrE.InventoryOut(huOdd, receiptDetail, user);
                    }
                    #endregion

                    #region ���ܻ�����ͷ����Ҫ����receiptDetail�ջ������������ջ��� + ���Odd�� - Hu����
                    receiptDetail.ReceivedQty = qty + receiptDetail.ReceivedQty - huLotSize;
                    #endregion

                    #region ����Hu
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
                //û�и�asn�Ĳ���Ȩ��
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