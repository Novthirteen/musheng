using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Production;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class LocationMgr : LocationBaseMgr, ILocationMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public INumberControlMgrE numberControlMgrE { get; set; }
        public ILocationTransactionMgrE locationTransactionMgrE { get; set; }
        public IRegionMgrE regionMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IPlannedBillMgrE plannedBillMgrE { get; set; }
        public IStorageBinMgrE storageBinMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IBillMgrE billMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
        public IUserMgrE userMgr { get; set; }


        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public Location CheckAndLoadLocation(string locationCode)
        {
            Location location = this.LoadLocation(locationCode);
            if (location == null)
            {
                throw new BusinessErrorException("Location.Error.LocationCodeNotExist", locationCode);
            }

            return location;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocation(Region region)
        {
            return GetLocation(region.Code, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocation(Region region, bool includeInactive)
        {
            return GetLocation(region.Code, includeInactive);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocation(string regionCode)
        {
            return GetLocation(regionCode, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocation(string regionCode, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Location)).Add(Expression.Eq("Region.Code", regionCode));
            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }
            return criteriaMgrE.FindAll<Location>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocation(string regionCode, bool includeInactive, bool includeReject)
        {
            IList<Location> locationList = GetLocation(regionCode, includeInactive);
            if (includeReject)
            {
                Region region = regionMgrE.LoadRegion(regionCode);
                Location rejectLoc = this.LoadLocation(region.RejectLocation);
                locationList.Add(rejectLoc);
            }
            return locationList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocationByUserCode(string userCode)
        {
            User user = userMgrE.LoadUser(userCode);
            return GetLocation(user);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocation(User user)
        {
            return GetLocation(user, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocation(User user, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Location>();
            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }

            DetachedCriteria[] pCrieteria = SecurityHelper.GetRegionPermissionCriteria(user.Code);

            criteria.Add(
                Expression.Or(
                    Subqueries.PropertyIn("Region.Code", pCrieteria[0]),
                    Subqueries.PropertyIn("Region.Code", pCrieteria[1])));

            criteria.AddOrder(Order.Desc("Region.Code"));
            return criteriaMgrE.FindAll<Location>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocation(string userCode, string type, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Location>();
            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }
            if (type != string.Empty)
            {
                criteria.Add(Expression.Eq("Type", type));
            }

            DetachedCriteria[] pCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);

            criteria.Add(
                Expression.Or(
                    Subqueries.PropertyIn("Region.Code", pCrieteria[0]),
                    Subqueries.PropertyIn("Region.Code", pCrieteria[1])));

            return criteriaMgrE.FindAll<Location>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocation(string userCode, string type)
        {
            return GetLocation(userCode, type, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocation(IList<string> locationCodeList)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Location>();
            if (locationCodeList.Count == 1)
            {
                criteria.Add(Expression.Eq("Code", locationCodeList[0]));
            }
            else
            {
                criteria.Add(Expression.InG<string>("Code", locationCodeList));
            }

            return criteriaMgrE.FindAll<Location>(criteria);
        }

        //[Transaction(TransactionMode.Unspecified)]
        //public Location GetRejectLocation()
        //{
        //    return this.LoadLocation(BusinessConstants.SYSTEM_LOCATION_REJECT);
        //}

        //[Transaction(TransactionMode.Unspecified)]
        //public Location GetInspectLocation()
        //{
        //    return this.LoadLocation(BusinessConstants.SYSTEM_LOCATION_INSPECT);
        //}
        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryOut(InProcessLocationDetail inProcessLocationDetail, User user)
        {
            return InventoryOut(inProcessLocationDetail, user, inProcessLocationDetail.OrderLocationTransaction.Item, (0 - inProcessLocationDetail.Qty * inProcessLocationDetail.OrderLocationTransaction.UnitQty), true);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryOut(InProcessLocationDetail inProcessLocationDetail, User user, Item item, decimal qty, bool fulshToNegative)
        {
            OrderLocationTransaction orderLocationTransaction = inProcessLocationDetail.OrderLocationTransaction;
            OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
            OrderHead orderHead = orderDetail.OrderHead;

            if (inProcessLocationDetail.HuId != null && inProcessLocationDetail.HuId.Trim() != string.Empty
                && inProcessLocationDetail.OrderLocationTransaction.OrderDetail.OrderHead.SubType != BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN)
            {
                #region �ж��Ƿ���Ҫ�¼�
                IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetHuLocationLotDetail(orderLocationTransaction.Location.Code, inProcessLocationDetail.HuId.Trim());
                if (locationLotDetailList != null && locationLotDetailList.Count > 0 && locationLotDetailList[0].StorageBin != null)
                {
                    //�¼�
                    this.InventoryPick(locationLotDetailList[0], user);
                }
                #endregion
            }

            #region ���ҳ����λ
            Location outLoc = null;
            if (orderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                outLoc = orderLocationTransaction.Location;

                //����ǲ��ϸ�Ʒ�˻��������λ�����������൱����⣩ҲΪ���ϸ�Ʒ��λ
                if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_REJ)
                {
                    outLoc = this.LoadLocation(orderLocationTransaction.RejectLocation);
                }
            }
            else
            {
                if (inProcessLocationDetail.HuId != null && inProcessLocationDetail.HuId.Trim() != string.Empty
                    && orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO)
                {
                    //������������������ȡ��λ
                    IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetHuLocationLotDetail(inProcessLocationDetail.HuId);
                    if (locationLotDetailList != null && locationLotDetailList.Count > 0)
                    {
                        LocationLotDetail locationLotDetail = locationLotDetailList[0];
                        outLoc = locationLotDetail.Location;
                    }
                    else
                    {
                        throw new BusinessErrorException("Hu.Error.NoInventory", inProcessLocationDetail.HuId);
                    }
                }
                else
                {
                    outLoc = orderLocationTransaction.Location;
                }
            }
            #endregion

            #region ���¿��
            IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                item,
                outLoc,
                inProcessLocationDetail.HuId,
                inProcessLocationDetail.LotNo,
                qty,
                false,
                null,
                orderLocationTransaction.TransactionType,
                orderHead.ReferenceOrderNo,
                //true,
                user,
                false,
                false,//(orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                fulshToNegative
                );
            #endregion

            #region ��������״̬�Ϳ�λ
            //ֻ����ͨ�Ķ����ż�¼���˻��͵������ÿ��ǣ���Ϊ�˻��͵��������ջ����������������ջ��ļ�¼�������˻�������
            //�������ǲ��ϸ�Ʒ�˻�
            if ((orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML
                || orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_REJ)
                && inProcessLocationDetail.HuId != null && inProcessLocationDetail.HuId.Trim() != string.Empty
                && inProcessLocationDetail.Qty > 0)
            {
                Hu hu = this.huMgrE.LoadHu(inProcessLocationDetail.HuId.Trim());
                if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML)
                {
                    if (orderDetail.DefaultLocationTo != null)
                    {
                        hu.Location = orderDetail.DefaultLocationTo.Code;
                    }
                }
                else
                {
                    if (orderDetail.DefaultRejectLocationTo != null)
                    {
                        hu.Location = orderDetail.DefaultRejectLocationTo;
                    }
                }
                hu.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_INPROCESS;
                this.huMgrE.UpdateHu(hu);
            }
            #endregion

            #region ��¼�������
            foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
            {
                this.locationTransactionMgrE.RecordLocationTransaction(orderLocationTransaction, inventoryTransaction, inProcessLocationDetail.InProcessLocation, user);

                #region �˻����ϼ�
                if ((inProcessLocationDetail.OrderLocationTransaction.OrderDetail.OrderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN
                    || inProcessLocationDetail.OrderLocationTransaction.OrderDetail.OrderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_REJ)
                    && inProcessLocationDetail.ReturnPutAwaySorageBinCode != null && inProcessLocationDetail.ReturnPutAwaySorageBinCode.Trim() != string.Empty)
                {
                    LocationLotDetail locationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransaction.LocationLotDetailId);
                    locationLotDetail.NewStorageBin = this.storageBinMgrE.LoadStorageBin(inProcessLocationDetail.ReturnPutAwaySorageBinCode);
                    inventoryTransaction.StorageBin = locationLotDetail.NewStorageBin;

                    InventoryPut(locationLotDetail, user);
                }
                #endregion
            }
            #endregion

            return inventoryTransactionList;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryOut(MiscOrderDetail miscOrderDetail, User user)
        {
            if (miscOrderDetail.HuId != null && miscOrderDetail.HuId.Trim() != string.Empty)
            {
                #region �ж��Ƿ���Ҫ�¼�
                LocationLotDetail locationLotDetail = this.locationLotDetailMgrE.CheckLoadHuLocationLotDetail(miscOrderDetail.HuId.Trim(), miscOrderDetail.MiscOrder.Location);
                if (locationLotDetail.StorageBin != null)
                {
                    //�¼�
                    this.InventoryPick(locationLotDetail, user);
                }
                #endregion
            }

            #region ���¿��
            IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                miscOrderDetail.Item,
                miscOrderDetail.MiscOrder.Location,
                miscOrderDetail.HuId,
                miscOrderDetail.LotNo,
                (0 - miscOrderDetail.Qty),
                false,
                null,
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_UNP,
                null,
                //true,
                user,
                false,
                false,
                true
                );
            #endregion

            #region ��������״̬�Ϳ�λ
            if (miscOrderDetail.HuId != null && miscOrderDetail.HuId.Trim() != string.Empty
                && miscOrderDetail.Qty > 0)
            {
                Hu hu = this.huMgrE.LoadHu(miscOrderDetail.HuId.Trim());
                hu.Location = null;
                hu.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_CLOSE;
                this.huMgrE.UpdateHu(hu);
            }
            #endregion

            #region ��¼�������
            foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
            {
                this.locationTransactionMgrE.RecordLocationTransaction(miscOrderDetail, inventoryTransaction, user);
            }
            #endregion

            return inventoryTransactionList;
        }

        [Transaction(TransactionMode.Requires)]
        public InventoryTransaction InventoryOut(HuOdd huOdd, ReceiptDetail receiptDetail, User user)
        {
            #region ���¿��
            LocationLotDetail locLotDet = this.locationLotDetailMgrE.LoadLocationLotDetail(huOdd.LocationLotDetail.Id);
            locLotDet.Qty -= huOdd.CurrentCreateQty;
            this.locationLotDetailMgrE.UpdateLocationLotDetail(locLotDet);
            #endregion

            #region ��¼�������
            InventoryTransaction inventoryTransaction = InventoryTransactionHelper.CreateInventoryTransaction(locLotDet, 0 - huOdd.CurrentCreateQty, false);
            this.locationTransactionMgrE.RecordLocationTransaction(receiptDetail.OrderLocationTransaction, inventoryTransaction, receiptDetail.Receipt, user);
            #endregion

            return inventoryTransaction;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryOut(MaterialIn materialIn, User user, Flow ProductLine)
        {
            Hu hu = null;
            LocationLotDetail huLocationLotDetail = null;
            if (materialIn.HuId != null && materialIn.HuId.Trim() != string.Empty)
            {
                hu = this.huMgrE.CheckAndLoadHu(materialIn.HuId);
                IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetHuLocationLotDetail(materialIn.Location.Code, materialIn.HuId);
                if (locationLotDetailList != null && locationLotDetailList.Count > 0)
                {
                    huLocationLotDetail = locationLotDetailList[0];

                    #region �ж��Ƿ���Ҫ�¼�
                    if (huLocationLotDetail.StorageBin != null)
                    {
                        //�¼�
                        this.InventoryPick(huLocationLotDetail, user);
                    }
                    #endregion
                }
                else
                {
                    throw new BusinessErrorException("Hu.Error.NoInventory", hu.HuId);
                }
            }

            #region ���¿��
            IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                materialIn.RawMaterial,
                huLocationLotDetail != null ? huLocationLotDetail.Location : materialIn.Location,
                hu != null ? hu.HuId : null,
                hu != null ? hu.LotNo : null,
                0 - materialIn.Qty,
                false,
                null,
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_TR,
                null,
                //true,
                user,
                false,
                false,
                true
                );
            #endregion

            #region ��������״̬��Ͷ��Ҳ����;
            if (hu != null && materialIn.Qty > 0)
            {
                hu.Location = null;
                hu.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_INPROCESS;
                this.huMgrE.UpdateHu(hu);
            }
            #endregion

            #region ��¼�������
            foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
            {
                this.locationTransactionMgrE.RecordLocationTransaction(inventoryTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_MATERIAL_IN, user, null, null, ProductLine);
            }
            #endregion

            return inventoryTransactionList;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryIn(ReceiptDetail receiptDetail, User user)
        {
            Receipt receipt = receiptDetail.Receipt;

            OrderLocationTransaction orderLocationTransaction = receiptDetail.OrderLocationTransaction;
            OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
            OrderHead orderHead = orderDetail.OrderHead;

            PlannedBill plannedBill = null;
            bool isCS = (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION
                || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING);
            bool isReceiveSettle = (orderDetail.DefaultBillSettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_RECEIVING_SETTLEMENT);

            #region ���ۿ����⣬��¼Planned Bill
            if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION
                || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING)
            {
                #region ��¼������
                plannedBill = this.plannedBillMgrE.CreatePlannedBill(receiptDetail, user);
                #endregion

                #region ί��ӹ���������
                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING)
                {
                    plannedBill.CurrentActingQty = plannedBill.PlannedQty;
                    this.billMgrE.CreateActingBill(plannedBill, user);
                    return null;  //ί��ӹ������ӿ�棬�������������
                }
                #endregion

                #region �жϼ��۸�����Ƿ���������
                bool autoSettleMinusCSInventory = true;
                if (autoSettleMinusCSInventory && isCS &&
                    receiptDetail.ReceivedQty < 0)
                {
                    plannedBill.CurrentActingQty = plannedBill.PlannedQty;
                    this.billMgrE.CreateActingBill(plannedBill, user);
                    isCS = false;
                }
                #endregion

                #region �ж�Ŀ�Ŀ�λΪ�յ���������
                if (isCS && orderLocationTransaction.Location == null && isReceiveSettle)
                {
                    plannedBill.CurrentActingQty = plannedBill.PlannedQty;
                    this.billMgrE.CreateActingBill(plannedBill, user);
                    isCS = false;
                }
                #endregion
            }
            #endregion

            #region ������
            if (orderLocationTransaction.Location != null)
            {
                #region �ջ�LotNo����С�����п���LotNoУ��
                if (orderHead.IsGoodsReceiveFIFO && receiptDetail.HuId != null && receiptDetail.ReceivedQty > 0) //�˻�������
                {
                    Hu hu = this.huMgrE.CheckAndLoadHu(receiptDetail.HuId);

                    #region ����ջ�����С�ڱ����ջ���ϸ���������ڣ�ȡ��С������Ϊ�Ƚ��ȳ��Ļ�׼����
                    DateTime? minManufactureDate = null;

                    if (receipt.ReceiptDetails != null && receipt.ReceiptDetails.Count > 0)
                    {
                        foreach (ReceiptDetail rd in receipt.ReceiptDetails)
                        {
                            if (rd.HuId != null && rd.HuId != string.Empty)
                            {
                                Hu rdHu = this.huMgrE.CheckAndLoadHu(rd.HuId);
                                if ((!minManufactureDate.HasValue || rdHu.ManufactureDate.CompareTo(minManufactureDate) < 0)
                                    //&& rdHu.ManufactureParty.Code == hu.ManufactureParty.Code
                                    && rdHu.Item.Code == hu.Item.Code)
                                {
                                    minManufactureDate = rdHu.ManufactureDate;
                                }
                            }
                        }
                    }
                    #endregion

                    if (!minManufactureDate.HasValue  //�����ջ�Ϊ��һ���ջ���Ҫ����
                        || minManufactureDate.Value.CompareTo(hu.ManufactureDate) > 0)   //�ջ�����С�������ڴ��ڵ��ڱ����ջ���ϸ���������ڲ���ҪУ�飬Ҳ���Ǳ����ջ�����������Ϊ��ǰ��Сֵ
                    {
                        if (!this.locationLotDetailMgrE.CheckGoodsReceiveFIFO(orderLocationTransaction.Location.Code,
                            orderLocationTransaction.Item.Code, hu.ManufactureDate, minManufactureDate))
                        {
                            throw new BusinessErrorException("MasterData.InventoryIn.LotNoIsOld",
                                orderLocationTransaction.Item.Code,
                                hu.HuId,
                                hu.LotNo,
                                orderLocationTransaction.Location.Code);
                        }
                    }
                }
                #endregion

                if (orderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                {
                    #region ����
                    IList<InventoryTransaction> resultInventoryTransactionList = new List<InventoryTransaction>();

                    #region ��Ʒ������
                    if (receiptDetail.ReceivedQty != 0)
                    {
                        Location locIn = orderLocationTransaction.Location; //Ĭ�����λ

                        //����ǲ��ϸ�Ʒ�˻�������λ����������൱�ڳ��⣩����Ϊ���ϸ�Ʒ��λ
                        if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_REJ)
                        {
                            locIn = this.LoadLocation(orderLocationTransaction.RejectLocation);
                        }
                        //#region ����Hu�˻������Hu�ڴ�Ʒ��λ�������λ����Ϊ��Ʒ��λ
                        //if (receiptDetail.HuId != null && receiptDetail.HuId.Trim() != string.Empty
                        //    && orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_REJ)
                        //{
                        //    IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetHuLocationLotDetail(BusinessConstants.SYSTEM_LOCATION_REJECT, receiptDetail.HuId);

                        //    if (locationLotDetailList != null && locationLotDetailList.Count > 0)
                        //    {
                        //        locIn = this.GetRejectLocation();
                        //    }
                        //}
                        //#endregion

                        #region ���¿��
                        IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                            orderLocationTransaction.Item,
                            locIn,
                            receiptDetail.HuId,
                            receiptDetail.LotNo,
                            receiptDetail.ReceivedQty * orderLocationTransaction.UnitQty,
                            isCS ? true : receiptDetail.IsConsignment,                     //�����Ǽ����ƿ�
                            plannedBill != null ? plannedBill : (receiptDetail.PlannedBill.HasValue ? this.plannedBillMgrE.LoadPlannedBill(receiptDetail.PlannedBill.Value) : null),  //��Ҫ��ASNDetail���ҵ�������Ϣ
                            orderLocationTransaction.TransactionType,
                            orderHead.ReferenceOrderNo,
                            //isReceiveSettle,
                            user,
                            orderDetail.NeedInspection && orderHead.NeedInspection,
                            false,
                            true
                            );
                        #endregion

                        IListHelper.AddRange<InventoryTransaction>(resultInventoryTransactionList, inventoryTransactionList);

                        #region ��¼�������
                        foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                        {
                            //������������ȼ���ֵ
                            inventoryTransaction.QualityLevel = BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_1;

                            #region �ɹ������˻�����������
                            if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                                && inventoryTransaction.Qty < 0 && inventoryTransaction.IsConsignment && inventoryTransaction.PlannedBill.HasValue)
                            {
                                PlannedBill pb = this.plannedBillMgrE.LoadPlannedBill(inventoryTransaction.PlannedBill.Value);
                                pb.CurrentActingQty = 0 - inventoryTransaction.Qty;
                                this.billMgrE.CreateActingBill(pb, user);
                            }
                            #endregion

                            this.locationTransactionMgrE.RecordLocationTransaction(orderLocationTransaction, inventoryTransaction, receipt, user);
                        }
                        #endregion
                    }
                    #endregion

                    #region ��Ʒ������
                    if (receiptDetail.RejectedQty > 0)
                    {
                        #region ���¿��
                        IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                            orderLocationTransaction.Item,
                            (orderLocationTransaction.RejectLocation != null && orderLocationTransaction.RejectLocation.Trim() != string.Empty ?
                                this.LoadLocation(orderLocationTransaction.RejectLocation) : orderLocationTransaction.Location),
                            receiptDetail.HuId,
                            receiptDetail.LotNo,
                            receiptDetail.RejectedQty * orderLocationTransaction.UnitQty,
                            isCS,
                            plannedBill,
                            orderLocationTransaction.TransactionType,
                            orderHead.ReferenceOrderNo,
                            //isReceiveSettle,
                            user,
                            false,
                            false,
                            true
                            );
                        #endregion

                        IListHelper.AddRange<InventoryTransaction>(resultInventoryTransactionList, inventoryTransactionList);

                        #region ��¼�������
                        foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                        {
                            //������������ȼ���ֵ
                            inventoryTransaction.QualityLevel = BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_2;

                            this.locationTransactionMgrE.RecordLocationTransaction(orderLocationTransaction, inventoryTransaction, receipt, user);
                        }
                        #endregion
                    }
                    #endregion

                    return resultInventoryTransactionList;
                    #endregion
                }
                else
                {
                    #region ����
                    IList<InventoryTransaction> resultInventoryTransactionList = new List<InventoryTransaction>();

                    #region ��Ʒ������
                    if (receiptDetail.ReceivedQty != 0)
                    {
                        Location locIn = orderLocationTransaction.Location; //Ĭ�����λ

                        //������ò�ʹ�ã�����λ����������൱�ڳ��⣩����Ϊ���ϸ�Ʒ��λ
                        if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RUS)
                        {
                            locIn = this.LoadLocation(orderLocationTransaction.RejectLocation);
                        }

                        #region ���¿��
                        IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                            orderLocationTransaction.Item,
                            locIn,
                            receiptDetail.HuId,
                            receiptDetail.LotNo,
                            receiptDetail.ReceivedQty * orderLocationTransaction.UnitQty,
                            false,
                            null,
                            orderLocationTransaction.TransactionType,
                            orderHead.ReferenceOrderNo,
                            //isReceiveSettle,
                            user,
                            orderDetail.NeedInspection && orderHead.NeedInspection,
                            false,
                            true
                            );
                        #endregion

                        IListHelper.AddRange<InventoryTransaction>(resultInventoryTransactionList, inventoryTransactionList);

                        #region ��¼�������
                        foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                        {
                            //������������ȼ���ֵ
                            inventoryTransaction.QualityLevel = BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_1;

                            this.locationTransactionMgrE.RecordLocationTransaction(orderLocationTransaction, inventoryTransaction, receipt, user);
                        }
                        #endregion
                    }
                    #endregion

                    #region ����Ʒ������
                    if (receiptDetail.RejectedQty > 0)
                    {
                        #region ���¿��
                        IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                            orderLocationTransaction.Item,
                            orderLocationTransaction.Location,  //������Ʒ��λ
                            receiptDetail.HuId,
                            receiptDetail.LotNo,
                            receiptDetail.RejectedQty * orderLocationTransaction.UnitQty,
                            false,
                            null,
                            orderLocationTransaction.TransactionType,
                            orderHead.ReferenceOrderNo,
                            //isReceiveSettle,
                            user,
                            false,
                            false,
                            true
                            );
                        #endregion

                        IListHelper.AddRange<InventoryTransaction>(resultInventoryTransactionList, inventoryTransactionList);

                        #region ��¼�������
                        foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                        {
                            //������������ȼ���ֵ
                            inventoryTransaction.QualityLevel = BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_2;

                            this.locationTransactionMgrE.RecordLocationTransaction(orderLocationTransaction, inventoryTransaction, receipt, user);
                        }
                        #endregion
                    }
                    #endregion

                    return resultInventoryTransactionList;
                    #endregion
                }
            }
            else
            {
                #region ���û�п�λ�ر�Hu
                if (receiptDetail.HuId != null && receiptDetail.HuId.Trim() != string.Empty)
                {
                    Hu hu = this.huMgrE.CheckAndLoadHu(receiptDetail.HuId);
                    hu.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_CLOSE;
                    this.huMgrE.UpdateHu(hu);
                }
                #endregion
            }

            return null;
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryIn(ReceiptDetail receiptDetail, User user, string binCode)
        {
            IList<InventoryTransaction> inventoryTransactionList = InventoryIn(receiptDetail, user);

            if (binCode != null && binCode != string.Empty && inventoryTransactionList != null && inventoryTransactionList.Count > 0)
            {
                foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                {
                    LocationLotDetail locationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransaction.LocationLotDetailId);
                    locationLotDetail.NewStorageBin = this.storageBinMgrE.LoadStorageBin(binCode);
                    inventoryTransaction.StorageBin = locationLotDetail.NewStorageBin;

                    InventoryPut(locationLotDetail, user);
                }
            }

            return inventoryTransactionList;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryIn(Receipt receipt, User user)
        {
            if (receipt.ReceiptDetails != null && receipt.ReceiptDetails.Count > 0)
            {
                IList<InventoryTransaction> resultInventoryTransactionList = new List<InventoryTransaction>();

                foreach (ReceiptDetail receiptDetail in receipt.ReceiptDetails)
                {
                    IList<InventoryTransaction> inventoryTransactionList = InventoryIn(receiptDetail, user);

                    IListHelper.AddRange<InventoryTransaction>(resultInventoryTransactionList, inventoryTransactionList);
                }

                return resultInventoryTransactionList;
            }

            return null;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryIn(MiscOrderDetail miscOrderDetail, User user)
        {
            #region ���¿��
            IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                miscOrderDetail.Item,
                miscOrderDetail.MiscOrder.Location,
                miscOrderDetail.HuId,
                miscOrderDetail.LotNo,
                miscOrderDetail.Qty,
                false,
                null,
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_UNP,
                null,
                //true,
                user,
                false,
                false,
                true
                );
            #endregion

            #region ��¼�������
            foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
            {
                this.locationTransactionMgrE.RecordLocationTransaction(miscOrderDetail, inventoryTransaction, user);
            }

            return inventoryTransactionList;
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryIn(MiscOrderDetail miscOrderDetail, User user, string binCode)
        {
            IList<InventoryTransaction> inventoryTransactionList = InventoryIn(miscOrderDetail, user);

            if (binCode != null && binCode != string.Empty && inventoryTransactionList != null && inventoryTransactionList.Count > 0)
            {
                foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                {
                    LocationLotDetail locationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransaction.LocationLotDetailId);
                    locationLotDetail.NewStorageBin = this.storageBinMgrE.LoadStorageBin(binCode);
                    inventoryTransaction.StorageBin = locationLotDetail.NewStorageBin;

                    InventoryPut(locationLotDetail, user);
                }
            }

            return inventoryTransactionList;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryIn(ProductLineInProcessLocationDetail productLineInProcessLocationDetail, User user)
        {
            Hu hu = null;
            if (productLineInProcessLocationDetail.HuId != null && productLineInProcessLocationDetail.HuId.Trim() != string.Empty)
            {
                hu = this.huMgrE.CheckAndLoadHu(productLineInProcessLocationDetail.HuId);
            }

            #region ���¿��
            IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                productLineInProcessLocationDetail.Item,
                productLineInProcessLocationDetail.LocationFrom,
                hu != null ? hu.HuId : null,
                hu != null ? hu.LotNo : null,
                productLineInProcessLocationDetail.Qty,
                false,
                null,
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_TR,
                null,
                //true,
                user,
                false,
                false,
                true
                );
            #endregion

            #region ��¼�������
            foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
            {
                this.locationTransactionMgrE.RecordLocationTransaction(inventoryTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_TR, user);
            }
            #endregion

            return inventoryTransactionList;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryIn(Location location, StorageBin bin, Item item, string huId, string lotNo, decimal qty, bool isCS, PlannedBill planBill, string transType, User user)
        {
            Hu hu = null;
            if (huId != null && huId.Trim() != string.Empty)
            {
                hu = this.huMgrE.CheckAndLoadHu(huId);
            }

            #region ���¿��
            IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                item,
                location,
                huId,
                lotNo != null && lotNo != string.Empty ? lotNo : (hu != null ? hu.LotNo : null),
                qty,
                isCS,
                planBill,
                transType,
                null,
                //true,
                user,
                false,
                false,
                true
                );
            #endregion

            #region ��¼�������
            foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
            {
                this.locationTransactionMgrE.RecordLocationTransaction(inventoryTransaction, transType, user);
            }
            #endregion

            return inventoryTransactionList;
        }

        [Transaction(TransactionMode.Requires)]
        public void InventoryPick(LocationLotDetail locationLotDetail, User user)
        {
            LocationLotDetail oldLocationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(locationLotDetail.Id);

            #region �����¼ܿ��
            if (oldLocationLotDetail.StorageBin == null)
            {
                throw new BusinessErrorException("Location.Error.PickUp.NotInBin", oldLocationLotDetail.Hu.HuId);
            }
            #endregion

            #region ��¼��������
            //InventoryTransaction inventoryOutTransaction = InventoryTransactionHelper.CreateInventoryTransaction(oldLocationLotDetail, 0 - oldLocationLotDetail.Qty, false);
            //this.locationTransactionMgrE.RecordLocationTransaction(inventoryOutTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_PICK, user);
            #endregion

            #region �¼�
            oldLocationLotDetail.StorageBin = null;
            this.locationLotDetailMgrE.UpdateLocationLotDetail(oldLocationLotDetail);
            #endregion

            #region ��¼�������
            //InventoryTransaction inventoryInTransaction = InventoryTransactionHelper.CreateInventoryTransaction(oldLocationLotDetail, oldLocationLotDetail.Qty, false);
            //this.locationTransactionMgrE.RecordLocationTransaction(inventoryInTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_PICK, user);
            #endregion

            #region ����Hu��λ
            Hu hu = this.huMgrE.LoadHu(oldLocationLotDetail.Hu.HuId);
            hu.StorageBin = null;
            this.huMgrE.UpdateHu(hu);
            #endregion
        }


        /// <summary>
        /// �����������¼�,���ı����
        /// </summary>
        /// <param name="bin"></param>
        /// <param name="user"></param>
        /// <param name="qty"></param>
        /// <param name="itemCode"></param>
        [Transaction(TransactionMode.Requires)]
        public void InventoryPick(StorageBin bin, User user, decimal qty, string itemCode)
        {
            if (bin == null)
            {
                throw new BusinessErrorException("Location.Error.PutAway.BinEmpty");
            }

            //�Ǽ���
            IList<LocationLotDetail> mscLocationLotDetails = locationLotDetailMgrE.GetLocationLotDetail(null, itemCode, false, false, BusinessConstants.PLUS_INVENTORY, false, bin.Code);
            //����
            IList<LocationLotDetail> csmLocationLotDetails = locationLotDetailMgrE.GetLocationLotDetail(null, itemCode, true, false, BusinessConstants.PLUS_INVENTORY, false, bin.Code);

            decimal mscLocationLotDetailsQty = mscLocationLotDetails.Sum(m => m.Qty);

            decimal csmLocationLotDetailsQty = csmLocationLotDetails.Sum(m => m.Qty);

            if (qty > mscLocationLotDetailsQty + csmLocationLotDetailsQty)
            {
                throw new BusinessErrorException("û���㹻���¼ܵĿ��");
            }

            List<LocationLotDetail> locationLotDetails = new List<LocationLotDetail>();

            decimal sumQty = 0;

            foreach (LocationLotDetail locationLotDetail in mscLocationLotDetails)
            {
                sumQty += locationLotDetail.Qty;
                if (sumQty > qty)
                {
                    locationLotDetail.CurrentQty = qty - (sumQty - locationLotDetail.Qty);
                    locationLotDetails.Add(locationLotDetail);
                    break;
                }
                else
                {
                    locationLotDetails.Add(locationLotDetail);
                }
            }

            if (mscLocationLotDetailsQty < qty)
            {
                foreach (LocationLotDetail locationLotDetail in csmLocationLotDetails)
                {
                    sumQty += locationLotDetail.Qty;
                    if (sumQty > qty)
                    {
                        locationLotDetail.CurrentQty = qty - (sumQty - locationLotDetail.Qty);
                        locationLotDetails.Add(locationLotDetail);
                        break;
                    }
                    else
                    {
                        locationLotDetails.Add(locationLotDetail);
                    }
                }
            }

            #region ��¼��������

            //foreach (LocationLotDetail oldLocationLotDetail in locationLotDetails)
            //{
            //    InventoryTransaction inventoryOutTransaction = InventoryTransactionHelper.CreateInventoryTransaction(oldLocationLotDetail, oldLocationLotDetail.CurrentQty == 0 ? -oldLocationLotDetail.Qty : -oldLocationLotDetail.CurrentQty, false);
            //    this.locationTransactionMgrE.RecordLocationTransaction(inventoryOutTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_PICK, user);
            //}

            #endregion

            #region �¼�
            LocationLotDetail lastLocationLotDetail = locationLotDetails.Last();
            LocationLotDetail newLocationLotDetail = null;

            if (lastLocationLotDetail.CurrentQty != 0)
            {
                foreach (LocationLotDetail oldLocationLotDetail in locationLotDetails)
                {
                    if (oldLocationLotDetail.Id != lastLocationLotDetail.Id)
                    {
                        oldLocationLotDetail.StorageBin = null;
                        oldLocationLotDetail.LastModifyDate = DateTime.Now;
                        this.locationLotDetailMgrE.UpdateLocationLotDetail(oldLocationLotDetail);
                    }
                }
                lastLocationLotDetail.Qty = lastLocationLotDetail.Qty - lastLocationLotDetail.CurrentQty;
                locationLotDetailMgrE.UpdateLocationLotDetail(lastLocationLotDetail);

                newLocationLotDetail = new LocationLotDetail();
                newLocationLotDetail.CreateDate = lastLocationLotDetail.LastModifyDate.HasValue ? lastLocationLotDetail.LastModifyDate.Value : DateTime.Now;
                newLocationLotDetail.Qty = lastLocationLotDetail.CurrentQty;
                newLocationLotDetail.IsConsignment = lastLocationLotDetail.IsConsignment;
                newLocationLotDetail.Item = lastLocationLotDetail.Item;
                newLocationLotDetail.Location = lastLocationLotDetail.Location;
                newLocationLotDetail.LotNo = lastLocationLotDetail.LotNo;
                newLocationLotDetail.PlannedBill = lastLocationLotDetail.PlannedBill;
                newLocationLotDetail.RefLocation = lastLocationLotDetail.RefLocation;
                newLocationLotDetail.Hu = lastLocationLotDetail.Hu;
                newLocationLotDetail.StorageBin = null;
                newLocationLotDetail.LastModifyDate = DateTime.Now;
                locationLotDetailMgrE.CreateLocationLotDetail(newLocationLotDetail);
            }
            else
            {
                foreach (LocationLotDetail oldLocationLotDetail in locationLotDetails)
                {
                    oldLocationLotDetail.StorageBin = null;
                    oldLocationLotDetail.LastModifyDate = DateTime.Now;
                    this.locationLotDetailMgrE.UpdateLocationLotDetail(oldLocationLotDetail);
                }
            }
            #endregion

            #region ��¼�������
            //foreach (LocationLotDetail oldLocationLotDetail in locationLotDetails)
            //{
            //    if (oldLocationLotDetail.CurrentQty != 0)
            //    {
            //        break;
            //    }
            //    InventoryTransaction inventoryInTransaction = InventoryTransactionHelper.CreateInventoryTransaction(oldLocationLotDetail, oldLocationLotDetail.Qty, false);
            //    this.locationTransactionMgrE.RecordLocationTransaction(inventoryInTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_PICK, user);
            //}
            //if (newLocationLotDetail != null)
            //{
            //    InventoryTransaction newInventoryInTransaction = InventoryTransactionHelper.CreateInventoryTransaction(newLocationLotDetail, newLocationLotDetail.Qty, false);
            //    this.locationTransactionMgrE.RecordLocationTransaction(newInventoryInTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_PICK, user);
            //}
            #endregion
        }


        [Transaction(TransactionMode.Requires)]
        public void InventoryPick(IList<LocationLotDetail> locationLotDetailList, User user)
        {
            if (locationLotDetailList != null && locationLotDetailList.Count > 0)
            {
                foreach (LocationLotDetail locationLotDetail in locationLotDetailList)
                {
                    this.InventoryPick(locationLotDetail, user);
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void InventoryPick(IList<LocationLotDetail> locationLotDetailList, string userCode)
        {
            User user = userMgrE.LoadUser(userCode);
            InventoryPick(locationLotDetailList, user);
        }


        /// <summary>
        /// �����������ϼ�,���ı����
        /// </summary>
        /// <param name="bin"></param>
        /// <param name="user"></param>
        /// <param name="qty"></param>
        /// <param name="itemCode"></param>
        [Transaction(TransactionMode.Requires)]
        public void InventoryPut(StorageBin bin, User user, decimal qty, string itemCode)
        {
            if (bin == null)
            {
                throw new BusinessErrorException("Location.Error.PutAway.BinEmpty");
            }

            //�Ǽ���
            IList<LocationLotDetail> mscLocationLotDetails = locationLotDetailMgrE.GetLocationLotDetail(bin.Area.Location.Code, itemCode, false, false, BusinessConstants.PLUS_INVENTORY, false, false);
            //����
            IList<LocationLotDetail> csmLocationLotDetails = locationLotDetailMgrE.GetLocationLotDetail(bin.Area.Location.Code, itemCode, true, false, BusinessConstants.PLUS_INVENTORY, false, false);

            decimal mscLocationLotDetailsQty = mscLocationLotDetails.Sum(m => m.Qty);

            decimal csmLocationLotDetailsQty = csmLocationLotDetails.Sum(m => m.Qty);

            if (qty > mscLocationLotDetailsQty + csmLocationLotDetailsQty)
            {
                throw new BusinessErrorException("û���㹻���ϼܵĿ��");
            }

            List<LocationLotDetail> locationLotDetails = new List<LocationLotDetail>();

            decimal sumQty = 0;

            foreach (LocationLotDetail locationLotDetail in mscLocationLotDetails)
            {
                sumQty += locationLotDetail.Qty;
                if (sumQty > qty)
                {
                    locationLotDetail.CurrentQty = qty - (sumQty - locationLotDetail.Qty);
                    locationLotDetails.Add(locationLotDetail);
                    break;
                }
                else
                {
                    locationLotDetails.Add(locationLotDetail);
                }
            }

            if (mscLocationLotDetailsQty < qty)
            {
                foreach (LocationLotDetail locationLotDetail in csmLocationLotDetails)
                {
                    sumQty += locationLotDetail.Qty;
                    if (sumQty > qty)
                    {
                        locationLotDetail.CurrentQty = qty - (sumQty - locationLotDetail.Qty);
                        locationLotDetails.Add(locationLotDetail);
                        break;
                    }
                    else
                    {
                        locationLotDetails.Add(locationLotDetail);
                    }
                }
            }
            #region ��¼��������

            //foreach (LocationLotDetail oldLocationLotDetail in locationLotDetails)
            //{
            //    InventoryTransaction inventoryOutTransaction = InventoryTransactionHelper.CreateInventoryTransaction(oldLocationLotDetail, oldLocationLotDetail.CurrentQty == 0 ? -oldLocationLotDetail.Qty : -oldLocationLotDetail.CurrentQty, false);
            //    this.locationTransactionMgrE.RecordLocationTransaction(inventoryOutTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_PUT, user);
            //}

            #endregion

            #region �ϼ�
            LocationLotDetail lastLocationLotDetail = locationLotDetails.Last();
            LocationLotDetail newLocationLotDetail = null;

            if (lastLocationLotDetail.CurrentQty != 0)
            {
                foreach (LocationLotDetail oldLocationLotDetail in locationLotDetails)
                {
                    if (oldLocationLotDetail.Id != lastLocationLotDetail.Id)
                    {
                        oldLocationLotDetail.StorageBin = bin;
                        lastLocationLotDetail.LastModifyDate = DateTime.Now;
                        this.locationLotDetailMgrE.UpdateLocationLotDetail(oldLocationLotDetail);
                    }
                }
                lastLocationLotDetail.Qty = lastLocationLotDetail.Qty - lastLocationLotDetail.CurrentQty;
                lastLocationLotDetail.LastModifyDate = DateTime.Now;
                locationLotDetailMgrE.UpdateLocationLotDetail(lastLocationLotDetail);

                newLocationLotDetail = new LocationLotDetail();
                newLocationLotDetail.CreateDate = lastLocationLotDetail.LastModifyDate.HasValue ? lastLocationLotDetail.LastModifyDate.Value : DateTime.Now;
                newLocationLotDetail.Qty = lastLocationLotDetail.CurrentQty;
                newLocationLotDetail.IsConsignment = lastLocationLotDetail.IsConsignment;
                newLocationLotDetail.Item = lastLocationLotDetail.Item;
                newLocationLotDetail.Location = lastLocationLotDetail.Location;
                newLocationLotDetail.LotNo = lastLocationLotDetail.LotNo;
                newLocationLotDetail.PlannedBill = lastLocationLotDetail.PlannedBill;
                newLocationLotDetail.RefLocation = lastLocationLotDetail.RefLocation;
                newLocationLotDetail.Hu = lastLocationLotDetail.Hu;
                newLocationLotDetail.StorageBin = bin;
                newLocationLotDetail.LastModifyDate = DateTime.Now;
                locationLotDetailMgrE.CreateLocationLotDetail(newLocationLotDetail);
            }
            else
            {
                foreach (LocationLotDetail oldLocationLotDetail in locationLotDetails)
                {
                    oldLocationLotDetail.StorageBin = bin;
                    lastLocationLotDetail.LastModifyDate = DateTime.Now;
                    this.locationLotDetailMgrE.UpdateLocationLotDetail(oldLocationLotDetail);
                }
            }
            #endregion

            #region ��¼�������
            //foreach (LocationLotDetail oldLocationLotDetail in locationLotDetails)
            //{
            //    if (oldLocationLotDetail.CurrentQty != 0)
            //    {
            //        break;
            //    }
            //    InventoryTransaction inventoryInTransaction = InventoryTransactionHelper.CreateInventoryTransaction(oldLocationLotDetail, oldLocationLotDetail.Qty, false);
            //    this.locationTransactionMgrE.RecordLocationTransaction(inventoryInTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_PUT, user);
            //}
            //if (newLocationLotDetail != null)
            //{
            //    InventoryTransaction newInventoryInTransaction = InventoryTransactionHelper.CreateInventoryTransaction(newLocationLotDetail, newLocationLotDetail.Qty, false);
            //    this.locationTransactionMgrE.RecordLocationTransaction(newInventoryInTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_PUT, user);
            //}
            #endregion
        }


        [Transaction(TransactionMode.Requires)]
        public void InventoryPut(LocationLotDetail locationLotDetail, User user)
        {
            LocationLotDetail oldLocationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(locationLotDetail.Id);

            #region �����ϼܿ��
            if (oldLocationLotDetail.StorageBin != null)
            {
                if (locationLotDetail.NewStorageBin != null
                    && oldLocationLotDetail.StorageBin.Code == locationLotDetail.NewStorageBin.Code)
                {
                    throw new BusinessErrorException("Location.Error.PutAway.AlreadyInBin", oldLocationLotDetail.Hu.HuId, oldLocationLotDetail.StorageBin.Code);
                }

                //�����¼�
                this.InventoryPick(oldLocationLotDetail, user);
            }

            if (locationLotDetail.NewStorageBin == null)
            {
                throw new BusinessErrorException("Location.Error.PutAway.BinEmpty");
            }

            if (locationLotDetail.NewStorageBin.Area.Location.Code.Trim() != oldLocationLotDetail.Location.Code.Trim())
            {
                throw new BusinessErrorException("Location.Error.PutAway.BinNotInLocation", locationLotDetail.NewStorageBin.Code, oldLocationLotDetail.Location.Code);
            }
            #endregion

            #region ��¼��������
            //InventoryTransaction inventoryOutTransaction = InventoryTransactionHelper.CreateInventoryTransaction(oldLocationLotDetail, 0 - oldLocationLotDetail.Qty, false);
            //this.locationTransactionMgrE.RecordLocationTransaction(inventoryOutTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_PUT, user);
            #endregion

            #region �ϼ�
            oldLocationLotDetail.StorageBin = locationLotDetail.NewStorageBin;
            this.locationLotDetailMgrE.UpdateLocationLotDetail(oldLocationLotDetail);
            #endregion

            #region ��¼�������
            //InventoryTransaction inventoryInTransaction = InventoryTransactionHelper.CreateInventoryTransaction(oldLocationLotDetail, oldLocationLotDetail.Qty, false);
            //this.locationTransactionMgrE.RecordLocationTransaction(inventoryInTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_PUT, user);
            #endregion

            #region ����Hu��λ
            Hu hu = this.huMgrE.LoadHu(oldLocationLotDetail.Hu.HuId);
            hu.StorageBin = locationLotDetail.NewStorageBin.Code;
            this.huMgrE.UpdateHu(hu);
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public void InventoryPut(IList<LocationLotDetail> locationLotDetailList, string userCode)
        {
            User user = userMgrE.LoadUser(userCode);
            InventoryPut(locationLotDetailList, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void InventoryPut(IList<LocationLotDetail> locationLotDetailList, User user)
        {
            if (locationLotDetailList != null && locationLotDetailList.Count > 0)
            {
                foreach (LocationLotDetail locationLotDetail in locationLotDetailList)
                {
                    this.InventoryPut(locationLotDetail, user);
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void InventoryRepackIn(RepackDetail repackDetail, User user)
        {
            LocationLotDetail oldLocationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(repackDetail.LocationLotDetail.Id);

            if (oldLocationLotDetail.Qty == 0)
            {
                throw new BusinessErrorException("MasterData.Inventory.Repack.Error.ZeroInRepackDetailQty", oldLocationLotDetail.Hu.HuId);
            }
            else if (repackDetail.Qty > oldLocationLotDetail.Qty)
            {
                throw new BusinessErrorException("MasterData.Inventory.Repack.Error.LocationQtyLessThanInRepackDetailQty", oldLocationLotDetail.Hu.HuId);
            }

            #region �¼�
            if (oldLocationLotDetail.StorageBin != null)
            {
                this.InventoryPick(oldLocationLotDetail, user);
            }
            #endregion

            #region ���㣬ֻ�з�����������
            if (oldLocationLotDetail.IsConsignment == true
                && oldLocationLotDetail.PlannedBill.HasValue
                && repackDetail.Repack.Type == BusinessConstants.CODE_MASTER_REPACK_TYPE_VALUE_REPACK)
            {
                PlannedBill pb = this.plannedBillMgrE.LoadPlannedBill(oldLocationLotDetail.PlannedBill.Value);
                pb.CurrentActingQty = repackDetail.Qty / pb.UnitQty;
                this.billMgrE.CreateActingBill(pb, user);
            }
            #endregion

            #region ����
            oldLocationLotDetail.Qty -= repackDetail.Qty;
            this.locationLotDetailMgrE.UpdateLocationLotDetail(oldLocationLotDetail);
            #endregion

            #region Hu�ر�
            if (oldLocationLotDetail.Hu != null)
            {
                Hu hu = this.huMgrE.LoadHu(oldLocationLotDetail.Hu.HuId);
                hu.Location = null;
                hu.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_CLOSE;

                this.huMgrE.UpdateHu(hu);
            }
            #endregion

            #region ��¼��������
            InventoryTransaction inventoryOutTransaction = InventoryTransactionHelper.CreateInventoryTransaction(oldLocationLotDetail, 0 - repackDetail.Qty, false);
            this.locationTransactionMgrE.RecordLocationTransaction(inventoryOutTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_REPACK, user,repackDetail.Repack.RepackNo);
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public InventoryTransaction InventoryRepackOut(RepackDetail repackDetail, Location location, int? plannedBillId, User user)
        {
            #region ����Hu�ϵ�OrderNo
            Hu hu = null;
            if (repackDetail.Hu != null)
            {
                hu = this.huMgrE.CheckAndLoadHu(repackDetail.Hu.HuId);

                if (repackDetail.Repack.Type == BusinessConstants.CODE_MASTER_REPACK_TYPE_VALUE_REPACK)
                {
                    if (hu.OrderNo != null)
                    {
                        throw new BusinessErrorException("MasterData.Inventory.Repack.Error.OrderNoIsNotEmpty", hu.HuId);
                    }
                    else
                    {
                        hu.OrderNo = repackDetail.Repack.RepackNo;
                        this.huMgrE.UpdateHu(hu);
                    }
                }
            }
            #endregion

            #region ���
            PlannedBill plannedBill = plannedBillId.HasValue ? this.plannedBillMgrE.LoadPlannedBill(plannedBillId.Value) : null;
            IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                hu != null ? hu.Item : this.itemMgrE.CheckAndLoadItem(repackDetail.itemCode),
                location,
                hu != null ? hu.HuId : null,
                hu != null ? hu.LotNo : null,
                repackDetail.Qty,     //��浥λ
                plannedBill != null,
                plannedBill,
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_REPACK,
                null,
                //true,
                user,
                false,
                false,
                true
                );
            #endregion

            #region ��¼��������
            LocationLotDetail inLocationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransactionList[0].LocationLotDetailId);

            InventoryTransaction inventoryInTransaction = InventoryTransactionHelper.CreateInventoryTransaction(inLocationLotDetail, repackDetail.Qty, false);
            this.locationTransactionMgrE.RecordLocationTransaction(inventoryInTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_REPACK, user,repackDetail.Repack.RepackNo);
            #endregion

            #region �ϼ�
            if (hu != null && repackDetail.StorageBinCode != null && repackDetail.StorageBinCode != string.Empty)
            {
                inLocationLotDetail.NewStorageBin = this.storageBinMgrE.CheckAndLoadStorageBin(repackDetail.StorageBinCode);
                this.InventoryPut(inLocationLotDetail, user);
            }
            #endregion

            return inventoryInTransaction;
        }

        [Transaction(TransactionMode.Requires)]
        public InventoryTransaction InspectOut(LocationLotDetail locationLotDetail, User user, bool needSettle, string inspectNo, Location locationTo)
        {
            LocationLotDetail oldLocationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(locationLotDetail.Id);

            if (oldLocationLotDetail.Qty < locationLotDetail.CurrentInspectQty)
            {
                throw new BusinessErrorException("MasterData.Inventory.Inspect.Error.NotEnoughInventory", locationLotDetail.Item.Code);
            }

            #region �¼�
            if (oldLocationLotDetail.StorageBin != null)
            {
                this.InventoryPick(oldLocationLotDetail, user);
            }
            #endregion

            #region ����
            oldLocationLotDetail.Qty -= locationLotDetail.CurrentInspectQty;
            this.locationLotDetailMgrE.UpdateLocationLotDetail(oldLocationLotDetail);
            #endregion

            //�������
            if (needSettle && locationLotDetail.IsConsignment && locationLotDetail.PlannedBill.HasValue)
            {
                PlannedBill plannedBill = this.plannedBillMgrE.LoadPlannedBill(locationLotDetail.PlannedBill.Value);
                if (plannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_INSPECTION)
                {
                    plannedBill.CurrentActingQty = locationLotDetail.CurrentInspectQty / plannedBill.UnitQty;
                    this.billMgrE.CreateActingBill(plannedBill, locationLotDetail, user);
                }
            }

            #region ��¼��������
            InventoryTransaction inventoryOutTransaction = InventoryTransactionHelper.CreateInventoryTransaction(oldLocationLotDetail, 0 - locationLotDetail.CurrentInspectQty, false);
            this.locationTransactionMgrE.RecordLocationTransaction(inventoryOutTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_INP, user, inspectNo, locationTo);
            #endregion

            return inventoryOutTransaction;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InspectOut(Location location, Item item, decimal qty, User user, string inspectNo, Location locationTo)
        {
            #region ���¿��
            IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                item,
                location,
                null,
                null,
                0 - qty,
                false,
                null,
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_INP,
                null,
                user,
                false,
                false,
                true
                );
            #endregion

            #region ��¼�������
            foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
            {
                this.locationTransactionMgrE.RecordLocationTransaction(inventoryTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_INP, user, inspectNo, locationTo);
            }
            #endregion

            return inventoryTransactionList;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InspectIn(LocationLotDetail locationLotDetail, Location locIn, User user, bool needSettle, string inspectNo, string inrNo)
        {
            return InspectIn(locationLotDetail, locIn, null, user, needSettle, inspectNo, inrNo);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InspectIn(LocationLotDetail locationLotDetail, Location locIn, StorageBin bin, User user, bool needSettle, string inspectNo, string inrNo)
        {
            bool isBillSettled = false;  //�Ƿ��Ѿ��������
            if (needSettle && locationLotDetail.IsConsignment && locationLotDetail.PlannedBill.HasValue)
            {
                PlannedBill plannedBill = this.plannedBillMgrE.LoadPlannedBill(locationLotDetail.PlannedBill.Value);
                if (plannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_INSPECTION)
                {
                    isBillSettled = true;
                }
            }

            #region ���
            IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                    locationLotDetail.Item,
                    locIn,
                //�벻�ϸ�Ʒ��λ����Ҫ������
                    locIn.Type != BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT && locationLotDetail.Hu != null ? locationLotDetail.Hu.HuId : null,
                    locationLotDetail.LotNo,
                    locationLotDetail.CurrentInspectQty,     //��浥λ
                    isBillSettled ? false : locationLotDetail.IsConsignment,   //�Ѿ��������ֱ�Ӽ�Ϊ�Ǽ��ۿ��
                    isBillSettled ? null : (locationLotDetail.PlannedBill.HasValue ? this.plannedBillMgrE.LoadPlannedBill(locationLotDetail.PlannedBill.Value) : null),      //�Ѿ��������ֱ�Ӽ�Ϊ�Ǽ��ۿ��
                    BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_INP,
                    null,
                //false,
                    user,
                    false,
                    false,
                    true
                    );
            #endregion

            #region ��¼�������
            if (inventoryTransactionList != null && inventoryTransactionList.Count > 0)
            {
                foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                {
                    this.locationTransactionMgrE.RecordLocationTransaction(inventoryTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_INP, user, inspectNo, locationLotDetail.Location, null, inrNo);
                }
            }
            #endregion

            #region �ϼ�
            if (bin != null)
            {
                LocationLotDetail oldLocationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransactionList[0].LocationLotDetailId);
                this.InventoryPut(oldLocationLotDetail, user);
            }
            #endregion

            #region ��������״̬�Ϳ�λ
            if (locationLotDetail.Hu != null)
            {
                Hu hu = this.huMgrE.LoadHu(locationLotDetail.Hu.HuId);
                if (locIn.Type == BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT)
                {
                    hu.Location = null;
                    hu.Qty = 0;
                    hu.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_CLOSE;
                }
                else
                {
                    if (locIn != null)
                    {
                        hu.Location = locIn.Code;
                    }
                    hu.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_INVENTORY;
                }
                this.huMgrE.UpdateHu(hu);
            }
            #endregion

            return inventoryTransactionList;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InspectIn(Item item, decimal qty, User user, string inspectNo, int? plannedBillId, Location inspectlocation, Location locationFrom)
        {
            #region ���¿��
            PlannedBill plannedBill = plannedBillId.HasValue ? this.plannedBillMgrE.LoadPlannedBill(plannedBillId.Value) : null;
            IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                item,
                inspectlocation,
                null,
                null,
                qty,
                plannedBill != null,
                plannedBill,
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_INP,
                null,
                //true,
                user,
                false,
                false,
                true
                );
            #endregion

            #region ��¼�������
            if (inventoryTransactionList != null && inventoryTransactionList.Count > 0)
            {
                foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                {
                    this.locationTransactionMgrE.RecordLocationTransaction(inventoryTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_INP, user, inspectNo, locationFrom);
                }
            }
            #endregion

            #region ��������״̬�Ϳ�λ
            if (inventoryTransactionList[0].Hu != null)
            {
                Hu hu = this.huMgrE.LoadHu(inventoryTransactionList[0].Hu.HuId);
                hu.Location = inspectlocation.Code;
                hu.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_INVENTORY;
                this.huMgrE.UpdateHu(hu);
            }
            #endregion

            return inventoryTransactionList;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InventoryTransaction> InventoryAdjust(CycleCountResult cycleCountResult, User user)
        {
            #region ���¿��
            IList<InventoryTransaction> inventoryTransactionList = RecordInventory(
                cycleCountResult.Item,
                cycleCountResult.CycleCount.Location,
                cycleCountResult.HuId != null ? cycleCountResult.HuId : null,
                cycleCountResult.LotNo,
                cycleCountResult.DiffQty,
                false,
                null,
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_CYC_CNT,
                null,
                //true,
                user,
                false,
                false,
                true
                );
            #endregion

            #region ��¼�������
            foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
            {
                this.locationTransactionMgrE.RecordLocationTransaction(inventoryTransaction, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_CYC_CNT, user, cycleCountResult.CycleCount.Code, cycleCountResult.CycleCount.EffectiveDate);

                if (cycleCountResult.StorageBin != null)
                {
                    LocationLotDetail locationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransaction.LocationLotDetailId);
                    if (locationLotDetail.StorageBin == null || locationLotDetail.StorageBin.Code != cycleCountResult.StorageBin)
                    {
                        locationLotDetail.NewStorageBin = this.storageBinMgrE.LoadStorageBin(cycleCountResult.StorageBin);
                        this.InventoryPut(locationLotDetail, user);
                    }
                }
            }
            #endregion

            #region ��������״̬
            if (cycleCountResult.HuId != null && cycleCountResult.HuId.Trim() != string.Empty && cycleCountResult.DiffQty < 0)
            {
                Hu hu = this.huMgrE.LoadHu(cycleCountResult.HuId);
                hu.Location = null;
                hu.StorageBin = null;
                hu.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_INPROCESS;

                this.huMgrE.UpdateHu(hu);
            }
            #endregion

            return inventoryTransactionList;
        }

        /**
         * transType ��������
         * loc ��λ
         * effdateStart ������Ч����
         * effDateEnd ������Ч������
         * createDateStart ���񴴽�ʱ��
         * createDateEnd ���񴴽�ʱ����
         * ����ʱ��  --ȥ��
         * ����ʱ���� --ȥ��
         * partyFrom ��Դ��Ӧ��/����
         * partyTo Ŀ�Ŀͻ�/����
         * itemCode �����
         * orderNo ������
         * recNo �ջ���
         * createUser ������
         * ipNo asn��
         * userCode ��ǰ�û�
         */
        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationTransaction> GetLocationTransaction(string[] transType,
           string[] loc, DateTime effdateStart, DateTime effDateEnd, DateTime createDateStart, DateTime createDateEnd,
           string partyFrom, string partyTo, string[] itemCode,
           string[] orderNo, string[] recNo, string createUser, string[] ipNo, string userCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationTransaction));
            //��������
            addDisjunction(transType, "TransactionType", ref criteria);

            IList<Region> regionList = regionMgrE.GetRegion(userCode);
            IList<String> list = new List<String>();
            foreach (string locId in loc)
            {
                Location location = this.LoadLocation(locId);
                if (regionList != null && regionList.Count > 0 && regionList.IndexOf(location.Region) < 0)
                    continue;
                list.Add(locId);
            }

            //��λ
            addDisjunction(list, "Location", ref criteria);

            //������Ч����
            if (effdateStart != null)
            {
                criteria.Add(Expression.Ge("EffectiveDate", effdateStart));
            }

            //������Ч������
            if (effDateEnd != null)
            {
                criteria.Add(Expression.Le("EffectiveDate", effDateEnd));
            }
            //���񴴽�ʱ��
            if (createDateStart != null)
            {
                criteria.Add(Expression.Ge("CreateDate", createDateStart));
            }
            //���񴴽�ʱ����
            if (createDateEnd != null)
            {
                criteria.Add(Expression.Le("CreateDate", createDateEnd));
            }
            //��Դ��Ӧ��/����
            if (partyFrom != null && partyTo.Length > 0)
            {
                criteria.Add(Expression.Eq("PartyFrom", partyFrom));
            }
            //Ŀ�Ŀͻ�/����
            if (partyTo != null && partyTo.Length > 0)
            {
                criteria.Add(Expression.Eq("PartyTo", partyTo));
            }
            //�����
            addDisjunction(itemCode, "Item", ref criteria);


            //������
            addDisjunction(orderNo, "OrderNo", ref criteria);

            //�ջ���recNo
            addDisjunction(recNo, "ReceiptNo", ref criteria);
            //������createUser
            if (createUser != null && createUser.Length > 0)
            {
                criteria.Add(Expression.Eq("CreateUser", createUser));
            }
            //asn��ipNo
            addDisjunction(ipNo, "IpNo", ref criteria);

            //��λ����
            criteria.AddOrder(Order.Asc("Location"));

            return criteriaMgrE.FindAll<LocationTransaction>(criteria);
        }

        public bool IsHuOcuppyByPickList(string huId)
        {
            DetachedCriteria criteria = DetachedCriteria.For<PickListResult>();
            criteria.SetProjection(Projections.Count("Id"));

            criteria.CreateAlias("PickListDetail", "pld");
            criteria.CreateAlias("pld.PickList", "pl");
            criteria.CreateAlias("LocationLotDetail", "lld");
            criteria.CreateAlias("lld.Hu", "hu");

            criteria.Add(Expression.Eq("pl.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
            criteria.Add(Expression.Eq("hu.HuId", huId));

            int count = this.criteriaMgrE.FindAll<int>(criteria)[0];

            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocationByType(string type)
        {
            return GetLocationByType(type, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocationByType(string type, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Location>();
            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }
            criteria.Add(Expression.Eq("Type", type));
            return criteriaMgrE.FindAll<Location>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocationListForMushengRequireForSup(string userCode)
        {
            IList<Flow> purFlow = new List<Flow>();

            User user = this.userMgr.LoadUser(userCode, false, true);

            DetachedCriteria criteria = null;

            List<Permission> suppliers = user.OrganizationPermission.Where(p => (p.Category.Code == BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_SUPPLIER)).ToList();

            if (suppliers != null && suppliers.Count() > 0)
            {
                criteria = DetachedCriteria.For<Flow>();
                criteria.CreateAlias("PartyFrom", "pf");

                criteria.Add(Expression.In("pf.Code", suppliers.Select(p => p.Code).ToList()));

                List<string> flowTypes = new List<string>() 
                    {BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
                        ,BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING};

                criteria.Add(Expression.In("Type", flowTypes));

                criteria.Add(Expression.Eq("IsActive", true));

                purFlow = this.criteriaMgrE.FindAll<Flow>(criteria);
            }

            if (purFlow != null)
            {
                return (from flow in purFlow
                    select flow.LocationTo).Distinct().ToList();
            }
            else
            {
                return null;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Location> GetLocationListForMushengRequireForCust(string userCode)
        {
            IList<Flow> saleFlow = new List<Flow>();

            User user = this.userMgr.LoadUser(userCode, false, true);

            DetachedCriteria criteria = null;

            List<Permission> customers = user.OrganizationPermission.Where(p => (p.Category.Code == BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_CUSTOMER)).ToList();

            if (customers != null && customers.Count() > 0)
            {
                criteria = DetachedCriteria.For<Flow>();
                criteria.CreateAlias("PartyFrom", "pf");

                criteria.Add(Expression.In("pf.Code", customers.Select(p => p.Code).ToList()));

                criteria.Add(Expression.Eq("Type", BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS));

                criteria.Add(Expression.Eq("IsActive", true));

                saleFlow = this.criteriaMgrE.FindAll<Flow>(criteria);
            }

            if (saleFlow != null)
            {
                return (from flow in saleFlow
                        select flow.LocationTo).Distinct().ToList();
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Private Methods
        private void addDisjunction(IList<string> values, string columnLabel, ref DetachedCriteria criteria)
        {
            if (values != null && values.Count > 0)
            {
                Disjunction dis = Expression.Disjunction();
                foreach (String value in values)
                {
                    dis.Add(Expression.Eq(columnLabel, value));
                }
                criteria.Add(dis);
            }
        }

        private void addDisjunction(String[] values, String columnLabel, ref DetachedCriteria criteria)
        {
            if (values != null && values.Length > 0)
            {
                Disjunction dis = Expression.Disjunction();
                foreach (String value in values)
                {
                    dis.Add(Expression.Eq(columnLabel, value));
                }
                criteria.Add(dis);
            }
        }

        private IList<InventoryTransaction> RecordInventory(Item item, Location location, string huId, string lotNo, decimal qty, bool isCS, PlannedBill plannedBill, string transType, string refOrderNo, User user, bool needInspection, bool flushbackIgnoreHu, bool flushToNegative)
        {
            IList<InventoryTransaction> inventoryTransactionList = new List<InventoryTransaction>();

            if (huId != null && huId.Trim() != string.Empty)
            {
                #region ��Hu
                //����/�Ǽ��۴����߼���ͬ
                //��֧�ֶ�����HU���ƻ������
                if (qty > 0)
                {
                    #region ������� > 0
                    IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetHuLocationLotDetail(location.Code, huId);
                    if (locationLotDetailList != null && locationLotDetailList.Count > 0)
                    {
                        //������Ѿ�������ͬ��Hu
                        throw new BusinessErrorException("Hu.Error.HuIdAlreadyExist", huId, location.Code);
                    }
                    else
                    {
                        CreateNewLocationLotDetail(item, location, huId, lotNo, qty, isCS, plannedBill, inventoryTransactionList, user);
                    }
                    #endregion
                }
                else if (qty < 0)
                {
                    #region ������� < 0 / ����
                    IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetHuLocationLotDetail(location.Code, huId);
                    if (locationLotDetailList != null && locationLotDetailList.Count > 0)
                    {
                        LocationLotDetail locationLotDetail = locationLotDetailList[0];  //�����ѯ����¼��ֻ������һ��

                        ////�������Hu�Ƿ񱻼����ռ��
                        //if (this.IsHuOcuppyByPickList(huId))
                        //{
                        //    throw new BusinessErrorException("Order.Error.PickUp.HuOcuppied", huId);
                        //}

                        if (locationLotDetail.Qty + qty < 0)
                        {
                            //Hu��Item������С�ڳ�����
                            throw new BusinessErrorException("Hu.Error.NoEnoughInventory", huId, location.Code);
                        }

                        bool isBillSettled = false;
                        if (locationLotDetail.IsConsignment && locationLotDetail.PlannedBill.HasValue)
                        {
                            PlannedBill pb = this.plannedBillMgrE.LoadPlannedBill(locationLotDetail.PlannedBill.Value);
                            if (
                                //(pb.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_ONLINE_BILLING   //���߽��������������ƿ��������Ž��㣬���Ա����ջ�����ʱ������λ�ͽ���
                                //&& (transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_TR
                                //    || transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_MATERIAL_IN)) 
                                //|| 
                            (pb.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_LINEAR_CLEARING   //���߽�������
                                && transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_WO)
                                //|| (locationLotDetail.PlannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_INSPECTION         //��������������Ӽ����λ���Ⲣ��ISS-INP����
                                //    && locationLotDetail.Location.Code == BusinessConstants.SYSTEM_LOCATION_INSPECT
                                //    && transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_INP)
                                //|| (locationLotDetail.PlannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_INSPECTION         //������㣬�Ӳ��ϸ�Ʒ��λ����
                                //    && locationLotDetail.Location.Code == BusinessConstants.SYSTEM_LOCATION_REJECT)
                            || transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_UNP                                   //����ISS_UNP����CYC_CNT����ǿ�н���
                            || transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_CYC_CNT
                            || transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_SO
                            || transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_WO
                            || transType.StartsWith(BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT))
                            {
                                //����
                                //���ۿ����Ҫ���н��㣬�Ա��س�Ŀ����и�������
                                pb.CurrentActingQty = (0 - qty) / pb.UnitQty; //����������
                                this.billMgrE.CreateActingBill(pb, locationLotDetail, user);
                                isBillSettled = true;
                            }
                        }

                        //��¼���س�ļ�¼
                        inventoryTransactionList.Add(InventoryTransactionHelper.CreateInventoryTransaction(locationLotDetail, qty, isBillSettled));

                        //���¿������
                        locationLotDetail.Qty += qty;
                        this.locationLotDetailMgrE.UpdateLocationLotDetail(locationLotDetail);
                    }
                    else
                    {
                        //û���ҵ�ָ����HU
                        throw new BusinessErrorException("Hu.Error.NoEnoughInventory", huId, location.Code);
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region û��Hu
                if (isCS)
                {
                    #region ���۴���
                    if (qty > 0)
                    {
                        #region ������� > 0

                        //������ջ����������ջ����㣬���س��档
                        if (!(needInspection
                            || plannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_RECEIVING_SETTLEMENT))
                        {
                            //#region �س�ָ���ջ����ŵļ��ۿ��
                            //if (refOrderNo != null && refOrderNo.Trim() != string.Empty)
                            //{
                            //    IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetLocationLotDetail(location.Code, item.Code, true, false, BusinessConstants.MINUS_INVENTORY, false, refOrderNo, flushbackIgnoreHu);
                            //    BackFlushInventory(locationLotDetailList, ref qty, inventoryTransactionList, plannedBill, user, transType);
                            //}
                            //#endregion

                            #region �س���ۿ��
                            if (qty > 0)
                            {
                                IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetLocationLotDetail(location.Code, item.Code, true, false, BusinessConstants.MINUS_INVENTORY, false, flushbackIgnoreHu);
                                BackFlushInventory(locationLotDetailList, ref qty, inventoryTransactionList, plannedBill, user, transType);
                            }
                            #endregion

                            #region �س�Ǽ��ۿ��
                            if (qty > 0)
                            {
                                IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetLocationLotDetail(location.Code, item.Code, false, false, BusinessConstants.MINUS_INVENTORY, false, flushbackIgnoreHu);
                                BackFlushInventory(locationLotDetailList, ref qty, inventoryTransactionList, plannedBill, user, transType);
                            }
                            #endregion
                        }

                        #region ��¼���
                        if (qty > 0)
                        {
                            CreateNewLocationLotDetail(item, location, huId, lotNo, qty, isCS, plannedBill, inventoryTransactionList, user);
                        }
                        #endregion
                        #endregion
                    }
                    else if (qty < 0)
                    {
                        #region ������� < 0

                        //#region �س�ָ���ջ����ŵļ��ۿ��
                        //if (refOrderNo != null && refOrderNo.Trim() != string.Empty)
                        //{
                        //    IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetLocationLotDetail(location.Code, item.Code, true, false, BusinessConstants.PLUS_INVENTORY, false, refOrderNo, flushbackIgnoreHu);
                        //    BackFlushInventory(locationLotDetailList, ref qty, inventoryTransactionList, plannedBill, user, transType);
                        //}
                        //#endregion

                        #region �س���ۿ��
                        if (qty < 0)
                        {
                            IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetLocationLotDetail(location.Code, item.Code, true, false, BusinessConstants.PLUS_INVENTORY, false, flushbackIgnoreHu);
                            BackFlushInventory(locationLotDetailList, ref qty, inventoryTransactionList, plannedBill, user, transType);
                        }
                        #endregion

                        #region �س�Ǽ��ۿ��
                        if (qty < 0)
                        {
                            IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetLocationLotDetail(location.Code, item.Code, false, false, BusinessConstants.PLUS_INVENTORY, false, flushbackIgnoreHu);
                            BackFlushInventory(locationLotDetailList, ref qty, inventoryTransactionList, plannedBill, user, transType);
                        }
                        #endregion

                        #region ��¼���
                        if (qty < 0)
                        {
                            CreateNewLocationLotDetail(item, location, huId, lotNo, qty, isCS, plannedBill, inventoryTransactionList, user);
                        }
                        #endregion
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region �Ǽ��۴���
                    if (qty > 0)
                    {
                        #region ������� > 0

                        //�ջ������Ĳ��ܻس���
                        if (!needInspection)
                        {
                            #region �س�Ǽ��ۿ��
                            IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetLocationLotDetail(location.Code, item.Code, false, false, BusinessConstants.MINUS_INVENTORY, false, flushbackIgnoreHu);
                            BackFlushInventory(locationLotDetailList, ref qty, inventoryTransactionList, null, user, transType);
                            #endregion
                        }

                        #region ��¼���
                        if (qty > 0)
                        {
                            CreateNewLocationLotDetail(item, location, huId, lotNo, qty, isCS, null, inventoryTransactionList, user);
                        }
                        #endregion
                        #endregion
                    }
                    else if (qty < 0)
                    {
                        #region ������� < 0
                        #region �س�Ǽ��ۿ��
                        if (qty < 0)
                        {
                            IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetLocationLotDetail(location.Code, item.Code, false, false, BusinessConstants.PLUS_INVENTORY, false, flushbackIgnoreHu);
                            BackFlushInventory(locationLotDetailList, ref qty, inventoryTransactionList, null, user, transType);
                        }
                        #endregion

                        #region �س���ۿ��
                        if (qty < 0)
                        {
                            IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetLocationLotDetail(location.Code, item.Code, true, false, BusinessConstants.PLUS_INVENTORY, false, flushbackIgnoreHu);
                            BackFlushInventory(locationLotDetailList, ref qty, inventoryTransactionList, null, user, transType);
                        }
                        #endregion

                        #region ��¼���
                        if (qty < 0 && flushToNegative)
                        {
                            CreateNewLocationLotDetail(item, location, huId, lotNo, qty, isCS, null, inventoryTransactionList, user);
                        }
                        #endregion
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }

            return inventoryTransactionList;
        }

        private void BackFlushInventory(IList<LocationLotDetail> backFlushLocLotDetList, ref decimal qtyIn, IList<InventoryTransaction> inventoryTransactionList, PlannedBill plannedBill, User user, string transType)
        {
            if (backFlushLocLotDetList != null && backFlushLocLotDetList.Count > 0)
            {
                foreach (LocationLotDetail backFlushLocLotDet in backFlushLocLotDetList)
                {
                    PlannedBill backFlushPlannedBill = backFlushLocLotDet.IsConsignment ? this.plannedBillMgrE.LoadPlannedBill(backFlushLocLotDet.PlannedBill.Value) : null;

                    #region ֻ�з����ڰ����뷢�����������ջ����������Ϊ��Щ���س�Ŀ���Ѿ�����һ��������������Ͳ������س��ˡ�
                    if (backFlushLocLotDet.Qty == 0)
                    {
                        continue;
                    }
                    #endregion

                    #region �ж��Ƿ�����س�����
                    if (qtyIn == 0)
                    {
                        return;
                    }

                    //������س�����Ķ��Ǽ��ۿ�沢����ͬһ����Ӧ�̣�һ��Ҫ�س�
                    if (backFlushLocLotDet.IsConsignment && plannedBill != null
                        && backFlushPlannedBill.BillAddress.Code == plannedBill.BillAddress.Code)
                    {

                    }
                    else
                    {
                        //���س�ļ��ۿ�棬�����ļ��ۿ�治��һ����Ӧ�̣����ܻس�
                        if (backFlushLocLotDet.IsConsignment && plannedBill != null
                            && backFlushPlannedBill.BillAddress.Code != plannedBill.BillAddress.Code)
                        {
                            return;
                        }

                        //���س�ļ��ۿ��Ľ��㷽ʽ�����߽��㣬�ж��Ƿ�ǰ���������Ƿ���ISS-*�������㲻�ܻس�
                        //if (backFlushLocLotDet.IsConsignment
                        //    && backFlushLocLotDet.PlannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_ONLINE_BILLING)
                        //{
                        //    if (!transType.StartsWith("ISS-"))
                        //    {
                        //        return;
                        //    }
                        //}

                        //���س�ļ��ۿ��Ľ��㷽ʽ�����߽��㣬�ж��Ƿ�ǰ���������Ƿ���ISS-*���Ҳ�����ISS-TR�������㲻�ܻس�
                        //if (backFlushLocLotDet.IsConsignment
                        //   && backFlushLocLotDet.PlannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_LINEAR_CLEARING)
                        //{
                        //    if (!(transType.StartsWith("ISS-") && transType != BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_TR))
                        //    {
                        //        return;
                        //    }
                        //}
                    }
                    #endregion

                    #region �س���
                    decimal currentBFQty = 0; //���λس���
                    if (qtyIn > 0)
                    {
                        if (backFlushLocLotDet.Qty + qtyIn < 0)
                        {
                            //��������� < ���������ȫ���س壬�س��������ڱ��������
                            currentBFQty = qtyIn;
                        }
                        else
                        {
                            //��������� >= ��������������Ŀ�����س�
                            currentBFQty = 0 - backFlushLocLotDet.Qty;
                        }
                    }
                    else
                    {
                        if (backFlushLocLotDet.Qty + qtyIn > 0)
                        {
                            //���γ����� < ���������ȫ���س壬�س��������ڱ��γ�����
                            currentBFQty = qtyIn;
                        }
                        else
                        {
                            //���γ����� >= ��������������Ŀ�����س�
                            currentBFQty = 0 - backFlushLocLotDet.Qty;
                        }
                    }

                    //���¿������
                    backFlushLocLotDet.Qty += currentBFQty;
                    this.locationLotDetailMgrE.UpdateLocationLotDetail(backFlushLocLotDet);

                    #endregion

                    #region ����
                    bool isBillSettled = false;
                    //ֻ�г���(qtyIn < 0 && plannedBill == null)�س���ۿ��(backFlushLocLotDet.IsConsignment == true)�Ű���SettleTerm����
                    //�������������������
                    if (qtyIn < 0 && plannedBill == null && backFlushLocLotDet.IsConsignment)
                    {
                        if (
                            // (backFlushPlannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_ONLINE_BILLING   //���߽��������������ƿ��������Ž��㣬���Ա����ջ�����ʱ������λ�ͽ���
                            //&& (transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_TR
                            //    || transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_MATERIAL_IN)) 
                            //|| 
                            (backFlushPlannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_LINEAR_CLEARING   //���߽�������
                                && transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_WO)
                            //|| (backFlushLocLotDet.PlannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_INSPECTION         //��������������Ӽ����λ���⣬���ҷ���ISS-INP����
                            //    && backFlushLocLotDet.Location.Code == BusinessConstants.SYSTEM_LOCATION_INSPECT
                            //        && transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_INP)
                            //|| (backFlushLocLotDet.PlannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_INSPECTION         //������㣬�Ӳ��ϸ�Ʒ��λ���⣬��������
                            //    && backFlushLocLotDet.Location.Code == BusinessConstants.SYSTEM_LOCATION_REJECT)
                            || transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_UNP                              //�������ISS_UNP����CYC_CNT����ǿ�н���
                            || transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_CYC_CNT
                            || transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_SO
                            || transType == BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_WO
                            || transType.StartsWith(BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT))
                        {
                            //���ۿ����Ҫ���н��㣬�Ա��س�Ŀ����и�������
                            backFlushPlannedBill.CurrentActingQty = (0 - currentBFQty) / backFlushPlannedBill.UnitQty; //����������
                            this.billMgrE.CreateActingBill(backFlushPlannedBill, backFlushLocLotDet, user);
                            isBillSettled = true;
                        }
                    }
                    else
                    {
                        if (backFlushLocLotDet.IsConsignment)
                        {
                            //���ۿ����Ҫ���н��㣬�Ա��س�Ŀ����и�������
                            backFlushPlannedBill.CurrentActingQty = (0 - currentBFQty) / backFlushPlannedBill.UnitQty; //����������
                            this.billMgrE.CreateActingBill(backFlushPlannedBill, backFlushLocLotDet, user);
                            isBillSettled = true;
                        }

                        if (plannedBill != null)
                        {
                            //�����Ŀ����н���
                            plannedBill.CurrentActingQty = currentBFQty / plannedBill.UnitQty;  //����������
                            this.billMgrE.CreateActingBill(plannedBill, user);
                        }
                    }
                    #endregion

                    //��¼���س�ļ�¼
                    inventoryTransactionList.Add(InventoryTransactionHelper.CreateInventoryTransaction(backFlushLocLotDet, currentBFQty, isBillSettled));

                    qtyIn -= currentBFQty;
                }
            }
        }

        private LocationLotDetail CreateNewLocationLotDetail(Item item, Location location, string huId, string lotNo, decimal qty, bool isCS, PlannedBill plannedBill, IList<InventoryTransaction> inventoryTransactionList, User user)
        {
            #region �Ƿ��������
            if (!location.AllowNegativeInventory && qty < 0)
            {
                throw new BusinessErrorException("Location.Error.NotAllowNegativeInventory", item.Code, location.Code);
            }
            #endregion

            #region �������&�ɹ���&�͹�Ʒ�������������
            if (item.Type != BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_P && item.Type != BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_M && item.Type != BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_C)
            {
                throw new BusinessErrorException("Location.Error.ItemTypeNotValid", item.Type);
            }
            #endregion

            bool isBillSettled = false;
            #region �ջ�����/������
            if (isCS && (plannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_RECEIVING_SETTLEMENT
                        || (location.IsSettleConsignment
                            && (plannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_ONLINE_BILLING
                                || plannedBill.SettleTerm == BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_LINEAR_CLEARING))))
            {
                plannedBill.CurrentActingQty = qty / plannedBill.UnitQty;
                this.billMgrE.CreateActingBill(plannedBill, user);

                isCS = false;
                isBillSettled = true;
            }
            #endregion

            DateTime createDate = DateTime.Now;

            LocationLotDetail newLocationLotDetail = new LocationLotDetail();
            newLocationLotDetail.Item = item;
            newLocationLotDetail.Location = location;
            newLocationLotDetail.LotNo = lotNo;
            if (huId != null && huId != string.Empty)
            {
                if (qty < 0)
                {
                    throw new TechnicalException("������������С���㡣");
                }

                huId = huId.ToUpper();
                newLocationLotDetail.Hu = this.huMgrE.LoadHu(huId);

                //��������������ϵ������в��죬���������ϵ�����
                if (newLocationLotDetail.Hu.Qty * newLocationLotDetail.Hu.UnitQty != qty)
                {
                    newLocationLotDetail.Hu.Qty = qty / newLocationLotDetail.Hu.UnitQty;
                }
                newLocationLotDetail.Hu.Location = location.Code;
                newLocationLotDetail.Hu.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_INVENTORY;

                this.huMgrE.UpdateHu(newLocationLotDetail.Hu);
            }
            newLocationLotDetail.CreateDate = createDate;
            newLocationLotDetail.Qty = qty;
            newLocationLotDetail.IsConsignment = isCS;
            if (plannedBill != null)
            {
                newLocationLotDetail.PlannedBill = plannedBill.Id;
            }

            this.locationLotDetailMgrE.CreateLocationLotDetail(newLocationLotDetail);
            inventoryTransactionList.Add(InventoryTransactionHelper.CreateInventoryTransaction(newLocationLotDetail, qty, isBillSettled));

            return newLocationLotDetail;
        }
        #endregion
    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class LocationMgrE : com.Sconit.Service.MasterData.Impl.LocationMgr, ILocationMgrE
    {

    }
}
#endregion