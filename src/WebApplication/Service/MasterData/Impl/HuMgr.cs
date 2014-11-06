using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Procurement;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Utility;
using NHibernate.Expression;
using NPOI.HSSF.UserModel;
using System.IO;
using com.Sconit.Service.Ext.Customize;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class HuMgr : HuBaseMgr, IHuMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public INumberControlMgrE numberControlMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
        public IUomMgrE uomMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IShiftDetailMgrE shiftDetailMgrE { get; set; }
        public ILedSortLevelMgrE ledSortLevelMgr { get; set; }
        public ILedColorLevelMgrE ledColorLevelMgr { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgr { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(IList<FlowDetail> flowDetailList, User user)
        {
            return CreateHu(flowDetailList, user, null);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(IList<FlowDetail> flowDetailList, User user, string idMark)
        {
            return CreateHu(flowDetailList, user, idMark, null);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(IList<FlowDetail> flowDetailList, User user, string idMark, string packageType)
        {
            if (flowDetailList != null && flowDetailList.Count > 0)
            {
                IList<Hu> huList = new List<Hu>();
                int? huLotSize = null;
                foreach (FlowDetail flowDetail in flowDetailList)
                {
                    Flow flow = flowDetail.Flow;
                    if (packageType == BusinessConstants.CODE_MASTER_PACKAGETYPE_INNER)
                    {
                        huLotSize = Convert.ToInt32(flowDetail.UnitCount);
                    }
                    else
                    {
                        huLotSize = flowDetail.HuLotSize;
                    }

                    IListHelper.AddRange<Hu>(huList,
                    CreateHu(flowDetail.Item, flowDetail.OrderedQty, flowDetail.HuLotNo, flowDetail.Uom, flowDetail.UnitCount, huLotSize,
                        null, null, null, flowDetail.Flow.PartyFrom, BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_1, user, idMark, flowDetail.CustomerItemCode, flow.HuTemplate,
                        flowDetail.HuSupplierLotNo, flowDetail.HuSortLevel1, flowDetail.HuColorLevel1, flowDetail.HuSortLevel2, flowDetail.HuColorLevel2, null));
                }

                return huList;
            }

            return null;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(IList<OrderDetail> orderDetailList, User user)
        {
            return CreateHu(orderDetailList, user, null);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(IList<OrderDetail> orderDetailList, User user, string idMark)
        {
            if (orderDetailList != null && orderDetailList.Count > 0)
            {
                IList<Hu> huList = new List<Hu>();
                foreach (OrderDetail orderDetail in orderDetailList)
                {

                    string lotNo = orderDetail.HuLotNo != null && orderDetail.HuLotNo.Trim().Length != 0 ? orderDetail.HuLotNo.Trim() : LotNoHelper.GenerateLotNo(orderDetail.OrderHead.WindowTime);
                    IListHelper.AddRange<Hu>(huList,
                        CreateHu(orderDetail.Item, orderDetail.OrderedQty, lotNo, orderDetail.Uom, orderDetail.UnitCount, orderDetail.HuLotSize,
                        null, null, null, orderDetail.OrderHead.PartyFrom, BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_1, user, idMark, orderDetail.CustomerItemCode, orderDetail.OrderHead.HuTemplate,
                        orderDetail.HuSupplierLotNo, orderDetail.HuSortLevel1, orderDetail.HuColorLevel1, orderDetail.HuSortLevel2, orderDetail.HuColorLevel2, null));
                }

                return huList;
            }

            return null;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(IList<OrderDetail> orderDetailList, User user, string idMark, string packageType)
        {
            if (orderDetailList != null && orderDetailList.Count > 0)
            {
                IList<Hu> huList = new List<Hu>();
                int? huLotSize = null;
                foreach (OrderDetail orderDetail in orderDetailList)
                {
                    if (packageType == BusinessConstants.CODE_MASTER_PACKAGETYPE_INNER)
                    {
                        huLotSize = Convert.ToInt32(orderDetail.UnitCount);
                    }
                    else
                    {
                        huLotSize = orderDetail.HuLotSize;
                    }
                    string lotNo = orderDetail.HuLotNo != null && orderDetail.HuLotNo.Trim().Length != 0 ? orderDetail.HuLotNo.Trim() : LotNoHelper.GenerateLotNo(orderDetail.OrderHead.WindowTime);
                    IListHelper.AddRange<Hu>(huList,
                        CreateHu(orderDetail.Item, orderDetail.OrderedQty, lotNo, orderDetail.Uom, orderDetail.UnitCount, huLotSize,
                        null, null, null, orderDetail.OrderHead.PartyFrom, BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_1, user, idMark, orderDetail.CustomerItemCode, orderDetail.OrderHead.HuTemplate,
                        orderDetail.HuSupplierLotNo, orderDetail.HuSortLevel1, orderDetail.HuColorLevel1, orderDetail.HuSortLevel2, orderDetail.HuColorLevel2, null));
                }

                return huList;
            }

            return null;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(IList<InProcessLocationDetail> inProcessLocationDetailList, User user, string idMark, string packageType)
        {
            if (inProcessLocationDetailList != null && inProcessLocationDetailList.Count > 0)
            {
                IList<Hu> huList = new List<Hu>();
                int? huLotSize = null;
                foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocationDetailList)
                {
                    if (packageType == BusinessConstants.CODE_MASTER_PACKAGETYPE_INNER)
                    {
                        huLotSize = Convert.ToInt32(inProcessLocationDetail.OrderLocationTransaction.OrderDetail.UnitCount);
                    }
                    else
                    {
                        huLotSize = inProcessLocationDetail.OrderLocationTransaction.OrderDetail.HuLotSize;
                    }
                    string lotNo = inProcessLocationDetail.HuLotNo != null && inProcessLocationDetail.HuLotNo.Trim().Length != 0 ? inProcessLocationDetail.HuLotNo.Trim() : LotNoHelper.GenerateLotNo(inProcessLocationDetail.OrderLocationTransaction.OrderDetail.OrderHead.WindowTime);
                    IListHelper.AddRange<Hu>(huList,
                        CreateHu(inProcessLocationDetail.OrderLocationTransaction.Item, inProcessLocationDetail.HuQty, lotNo, inProcessLocationDetail.OrderLocationTransaction.OrderDetail.Uom, inProcessLocationDetail.OrderLocationTransaction.OrderDetail.UnitCount, huLotSize,
                        null, null, null, inProcessLocationDetail.OrderLocationTransaction.OrderDetail.OrderHead.PartyFrom, BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_1, user, idMark, inProcessLocationDetail.OrderLocationTransaction.OrderDetail.CustomerItemCode, inProcessLocationDetail.OrderLocationTransaction.OrderDetail.OrderHead.HuTemplate,
                        inProcessLocationDetail.HuSupplierLotNo, inProcessLocationDetail.HuSortLevel1, inProcessLocationDetail.HuColorLevel1, inProcessLocationDetail.HuSortLevel2, inProcessLocationDetail.HuColorLevel2, null));
                }

                return huList;
            }

            return null;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(Item item, string qtyType, decimal papers, string lotNo, string huSupplierLotNo, int? huLotSize,
            string huSortLevel1, string huColorLevel1, string huSortLevel2, string huColorLevel2, string huTemplate, User user,
            string idMark, string packageType, string oldHuId)
        {
            return CreateHu(item, qtyType, papers, lotNo, huSupplierLotNo, (decimal)huLotSize,
               huSortLevel1, huColorLevel1, huSortLevel2, huColorLevel2, huTemplate, user,
               idMark, packageType, oldHuId);
        }


        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(Item item, string qtyType, decimal papers, string lotNo, string huSupplierLotNo, decimal? huLotSize,
            string huSortLevel1, string huColorLevel1, string huSortLevel2, string huColorLevel2, string huTemplate, User user,
            string idMark, string packageType, string oldHuId)
        {
            if (item != null)
            {
                IList<Hu> huList = new List<Hu>();

                if (qtyType == "0")
                {
                    IListHelper.AddRange<Hu>(huList,
                            CreateHu(item, decimal.Parse(papers.ToString()), lotNo, item.Uom, item.UnitCount, huLotSize,
                            null, null, null, null, BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_1, user, idMark, string.Empty, huTemplate,
                            huSupplierLotNo, huSortLevel1, huColorLevel1, huSortLevel2, huColorLevel2, oldHuId));
                }
                else if (qtyType == "1")
                {
                    for (int i = 0; i < (int)papers; i++)
                    {

                        IListHelper.AddRange<Hu>(huList,
                            CreateHu(item, decimal.Parse(huLotSize.Value.ToString()), lotNo, item.Uom, item.UnitCount, huLotSize,
                            null, null, null, null, BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_1, user, idMark, string.Empty, huTemplate,
                            huSupplierLotNo, huSortLevel1, huColorLevel1, huSortLevel2, huColorLevel2, oldHuId));
                    }
                }

                return huList;
            }

            return null;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(OrderHead orderHead, User user)
        {
            if (orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
            {
                IList<Hu> huList = new List<Hu>();
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    IListHelper.AddRange<Hu>(huList,
                        CreateHu(orderDetail.Item, orderDetail.OrderedQty, orderDetail.HuLotNo, orderDetail.Uom, orderDetail.UnitCount, orderDetail.HuLotSize,
                        null, null, null, orderDetail.OrderHead.PartyFrom, BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_1, user, null, orderDetail.CustomerItemCode, orderDetail.OrderHead.HuTemplate,
                        orderDetail.HuSupplierLotNo, orderDetail.HuSortLevel1, orderDetail.HuColorLevel1, orderDetail.HuSortLevel2, orderDetail.HuColorLevel2, null));
                }

                return huList;
            }

            return null;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(InProcessLocationDetail inProcessLocationDetail, User user)
        {
            if (inProcessLocationDetail.HuId != null)
            {
                throw new TechnicalException("HuId already exist.");
            }

            OrderLocationTransaction orderLocationTransaction = inProcessLocationDetail.OrderLocationTransaction;
            OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
            OrderHead orderHead = orderDetail.OrderHead;

            return CreateHu(orderLocationTransaction.Item, inProcessLocationDetail.Qty, inProcessLocationDetail.LotNo, orderDetail.Uom, orderDetail.UnitCount, orderDetail.HuLotSize,
                    orderHead.OrderNo, null, null, orderHead.PartyFrom, BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_1, user, null, orderDetail.CustomerItemCode, orderHead.HuTemplate,
                    inProcessLocationDetail.HuSupplierLotNo, inProcessLocationDetail.HuSortLevel1, inProcessLocationDetail.HuColorLevel1, inProcessLocationDetail.HuSortLevel2, inProcessLocationDetail.HuColorLevel2, null);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CreateHu(ReceiptDetail receiptDetail, User user)
        {
            if (receiptDetail.HuId != null)
            {
                throw new TechnicalException("HuId already exist.");
            }

            OrderLocationTransaction orderLocationTransaction = receiptDetail.OrderLocationTransaction;
            OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
            OrderHead orderHead = orderDetail.OrderHead;
            IList<Hu> huList = new List<Hu>();
            string lotNo = null;

            if (orderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                lotNo = receiptDetail.LotNo;
            }

            #region 为正品创建Hu
            if (receiptDetail.ReceivedQty != 0)
            {
                huList = CreateHu(orderLocationTransaction.Item, receiptDetail.ReceivedQty, lotNo, orderDetail.Uom, orderDetail.UnitCount, orderDetail.HuLotSize,
                    orderHead.OrderNo, receiptDetail.Receipt.ReceiptNo, null, orderHead.PartyFrom, BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_1, user, null, orderDetail.CustomerItemCode, orderHead.HuTemplate,
                    receiptDetail.HuSupplierLotNo, receiptDetail.HuSortLevel1, receiptDetail.HuColorLevel1, receiptDetail.HuSortLevel2, receiptDetail.HuColorLevel2, null);
            }
            #endregion

            #region 为次品创建Hu
            if (receiptDetail.RejectedQty != 0)
            {
                IList<Hu> rejHuList = CreateHu(orderLocationTransaction.Item, receiptDetail.RejectedQty, lotNo, orderDetail.Uom, orderDetail.UnitCount, orderDetail.HuLotSize,
                    orderHead.OrderNo, receiptDetail.Receipt.ReceiptNo, null, orderHead.PartyFrom, BusinessConstants.CODE_MASTER_ITEM_QUALITY_LEVEL_VALUE_2, user, null, orderDetail.CustomerItemCode, orderHead.HuTemplate,
                    receiptDetail.HuSupplierLotNo, receiptDetail.HuSortLevel1, receiptDetail.HuColorLevel1, receiptDetail.HuSortLevel2, receiptDetail.HuColorLevel2, null);
                IListHelper.AddRange<Hu>(huList, rejHuList);
            }
            #endregion

            return huList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public Hu CheckAndLoadHu(string huId)
        {
            Hu hu = this.LoadHu(huId);
            if (hu == null)
            {
                throw new BusinessErrorException("Hu.Error.HuIdNotExist", huId);
            }

            return hu;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Hu> GetHuList(string userCode, int firstRow, int maxRows, params string[] orderTypes)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Hu));

            //criteria.CreateAlias("OrderNo","order");
            criteria.Add(Expression.Eq("CreateUser.Code", userCode));
            //criteria.Add(Expression.In("order.OrderType", orderTypes));
            criteria.Add(Expression.Ge("CreateDate", DateTime.Today));//直显示当天的条码
            criteria.AddOrder(Order.Desc("CreateDate"));
            IList<Hu> huList = criteriaMgrE.FindAll<Hu>(criteria, firstRow, maxRows);
            if (huList.Count > 0)
            {
                return huList;
            }
            return null;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CloneHu(Hu hu, decimal uintCount, int count, User user)
        {
            return CloneHu(hu.HuId, uintCount, count, user);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Hu> CloneHu(string huId, decimal uintCount, int count, User user)
        {
            IList<Hu> huList = new List<Hu>();
            Hu oldHu = this.LoadHu(huId);
            DateTime dateTimeNow = DateTime.Now;
            string sortAndColor = string.Empty;
            if (oldHu.SortLevel1 != null && oldHu.SortLevel1.Trim() != string.Empty)
            {
                sortAndColor += oldHu.SortLevel1 + "*" + oldHu.ColorLevel1;
            }
            if (oldHu.SortLevel2 != null && oldHu.SortLevel2.Trim() != string.Empty)
            {
                sortAndColor += "*" + oldHu.SortLevel2 + "*" + oldHu.ColorLevel2;
            }

            int i = 0;
            while (i < count)
            {
                Hu huTemplate = new Hu();
                CloneHelper.CopyProperty(oldHu, huTemplate);
                //CloneHelper.DeepClone<Hu>(oldHu);
                huTemplate.LotSize = uintCount;
                huTemplate.UnitCount = uintCount;
                huTemplate.Qty = uintCount;
                huTemplate.HuId = this.numberControlMgrE.GenerateHuId(oldHu.Item.Id, sortAndColor);
                huTemplate.CreateDate = dateTimeNow;
                huTemplate.CreateUser = user;
                huTemplate.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_CREATE;

                this.CreateHu(huTemplate);
                huList.Add(huTemplate);
                i++;
            }

            return huList;
        }
        #endregion Customized Methods

        #region Private Methods
        private IList<Hu> CreateHu(Item item, decimal qty, string lotNo, Uom uom, decimal unitCount, decimal? huLotSize,
            string orderNo, string recNo, DateTime? manufactureDate, Party manufactureParty, string qualityLevel,
            User user, string idMark, string customerItemCode, string huTemplate,
            string supplierLotNo, string sortLevel1, string colorLevel1, string sortLevel2, string colorLevel2, string oldHuId)
        {
            string sortAndColor = string.Empty;
            #region 分光分色校验
            if (item.IsSortAndColor.HasValue && item.IsSortAndColor.Value)
            {
                if (item.SortLevel1From != null && item.SortLevel1From != string.Empty && item.SortLevel1From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                {
                    if (sortLevel1 == null || sortLevel1.Trim() == string.Empty)
                    {
                        throw new BusinessErrorException("MasterData.Hu.SortLevel1Empty", item.Code);
                    }
                    else
                    {
                        ledSortLevelMgr.CheckLedFeedSortLevel(item.Code, item.ItemBrand.Code, item.SortLevel1From, item.SortLevel1To, sortLevel1);
                    }

                    sortAndColor = sortLevel1 + "*" + colorLevel1;
                }

                if (item.ColorLevel1From != null && item.ColorLevel1From != string.Empty && item.ColorLevel1From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                {
                    if (colorLevel1 == null || colorLevel1.Trim() == string.Empty)
                    {
                        throw new BusinessErrorException("MasterData.Hu.ColorLevel1Empty", item.Code);
                    }
                    else
                    {
                        ledColorLevelMgr.CheckLedFeedColorLevel(item.Code, item.ItemBrand.Code, item.ColorLevel1From, item.ColorLevel1To, colorLevel1);
                    }
                }

                if (item.SortLevel2From != null && item.SortLevel2From != string.Empty && item.SortLevel2From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                {
                    if (sortLevel2 == null || sortLevel2.Trim() == string.Empty)
                    {
                        throw new BusinessErrorException("MasterData.Hu.SortLevel2Empty", item.Code);
                    }
                    else
                    {
                        ledSortLevelMgr.CheckLedFeedSortLevel(item.Code, item.ItemBrand.Code, item.SortLevel2From, item.SortLevel2To, sortLevel2);
                    }

                    sortAndColor += "*" + sortLevel2 + "*" + colorLevel2;
                }

                if (item.ColorLevel2From != null && item.ColorLevel2From != string.Empty && item.ColorLevel2From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                {
                    if (colorLevel2 == null || colorLevel2.Trim() == string.Empty)
                    {
                        throw new BusinessErrorException("MasterData.Hu.ColorLevel2Empty", item.Code);
                    }
                    else
                    {
                        ledColorLevelMgr.CheckLedFeedColorLevel(item.Code, item.ItemBrand.Code, item.ColorLevel2From, item.ColorLevel2To, colorLevel2);
                    }
                }
            }
            #endregion

            IList<Hu> huList = new List<Hu>();

            #region 根据Hu批量创建Hu
            decimal remainHuQty = qty;                                        //剩余量
            decimal currentHuQty = GetNextHuQty(ref remainHuQty, huLotSize);  //本次量
            DateTime dateTimeNow = DateTime.Now;
            if (lotNo == null || lotNo == string.Empty)
            {
                lotNo = LotNoHelper.GenerateLotNo();
            }

            if (!manufactureDate.HasValue)
            {
                manufactureDate = LotNoHelper.ResolveLotNo(lotNo);
            }

            while (currentHuQty > 0)
            {
                #region 创建Hu
                Hu hu = new Hu();
                #region HuId生成
                hu.HuId = this.numberControlMgrE.GenerateHuId(item.Id, sortAndColor);
                #endregion
                hu.Item = item;
                hu.OrderNo = orderNo;
                hu.ReceiptNo = recNo;
                hu.Uom = uom;   //用订单单位
                hu.UnitCount = unitCount;
                #region 单位用量
                //如果是OrderDetail，应该等于inOrderLocationTransaction.UnitQty，现在暂时直接用单位换算
                if (item.Uom.Code != uom.Code)
                {
                    hu.UnitQty = this.uomConversionMgrE.ConvertUomQty(item, uom, 1, item.Uom);   //单位用量
                }
                else
                {
                    hu.UnitQty = 1;
                }
                #endregion
                hu.QualityLevel = qualityLevel;
                hu.Qty = currentHuQty;
                hu.LotNo = supplierLotNo;
                hu.ManufactureDate = manufactureDate.Value;
                hu.ManufactureParty = manufactureParty;
                hu.CreateUser = user;
                hu.CreateDate = dateTimeNow;
                hu.LotSize = huLotSize.HasValue ? huLotSize.Value : currentHuQty;
                hu.Status = BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_CREATE;
                hu.CustomerItemCode = customerItemCode;
                hu.HuTemplate = huTemplate;
                hu.SupplierLotNo = supplierLotNo;
                hu.SortLevel1 = sortLevel1;
                hu.ColorLevel1 = colorLevel1;
                hu.SortLevel2 = sortLevel2;
                hu.ColorLevel2 = colorLevel2;
                hu.Remark = oldHuId;

                this.CreateHu(hu);
                #endregion

                huList.Add(hu);
                currentHuQty = GetNextHuQty(ref remainHuQty, huLotSize);
            }
            #endregion

            return huList;
        }

        private decimal GetNextHuQty(ref decimal remainHuQty, decimal? huLotSize)
        {
            #region 设置下次Hu批量
            decimal currentHuQty = 0;
            if (huLotSize.HasValue && huLotSize > 0)
            {
                if (remainHuQty - huLotSize.Value > 0)
                {
                    remainHuQty -= huLotSize.Value;
                    currentHuQty = huLotSize.Value;
                }
                else
                {
                    currentHuQty = remainHuQty;
                    remainHuQty = 0;
                }
            }
            else
            {
                currentHuQty = remainHuQty;
                remainHuQty = 0;
            }

            return currentHuQty;
            #endregion
        }

        #endregion
    }
}


#region Extend Class








namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class HuMgrE : com.Sconit.Service.MasterData.Impl.HuMgr, IHuMgrE
    {

    }
}
#endregion
