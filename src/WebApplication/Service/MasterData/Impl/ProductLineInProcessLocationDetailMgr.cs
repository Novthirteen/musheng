using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.Production;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Service.Criteria;
using NHibernate.Expression;
using com.Sconit.Service.Distribution;
using com.Sconit.Entity.Distribution;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Service.Ext.Customize;
using com.Sconit.Entity.Customize;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ProductLineInProcessLocationDetailMgr : ProductLineInProcessLocationDetailBaseMgr, IProductLineInProcessLocationDetailMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public ILocationTransactionMgrE locationTransactionMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderLocationTransactionMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public IOrderPlannedBackflushMgrE orderPlannedBackflushMgrE { get; set; }
        public IInProcessLocationDetailMgrE inProcessLocationDetailMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public ICostMgrE costMgr { get; set; }
        public IPlannedBillMgrE plannedBillMgr { get; set; }
        public IHuMgrE huMgr { get; set; }
        public IOrderHeadMgrE orderHeadMgr { get; set; }
        public IProdutLineFeedSeqenceMgrE produtLineFeedSeqenceMgr { get; set; }
        public ILedColorLevelMgrE ledColorLevelMgr { get; set; }
        public ILedSortLevelMgrE ledSortLevelMgr { get; set; }
        public IProductLineFacilityMgrE productionLineFacilityMgr { get; set; }
        public IProdLineIp2MgrE prodLineIp2Mgr { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public IList<ProductLineInProcessLocationDetail> GetProductLineInProcessLocationDetail(string prodLineCode, string prodLineFacilityCode, string orderNo, string status, string[] items)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ProductLineInProcessLocationDetail>();

            if (prodLineCode != null && prodLineCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("ProductLine.Code", prodLineCode));
            }

            if (prodLineFacilityCode != null && prodLineFacilityCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("ProductLineFacility", prodLineFacilityCode));
            }

            if (orderNo != null && orderNo.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("OrderNo", orderNo));
            }

            if (status != null && status.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Status", status));
            }

            if (items != null && items.Length > 0)
            {
                criteria.CreateAlias("Item", "item");
                criteria.Add(Expression.In("item.Code", items));
            }

            criteria.AddOrder(Order.Asc("Id"));

            return this.criteriaMgrE.FindAll<ProductLineInProcessLocationDetail>(criteria);
        }

        public decimal? GetPLIpQty(string huId)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ProductLineInProcessLocationDetail>();

            criteria.Add(Expression.Eq("HuId", huId));
            criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));

            IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.criteriaMgrE.FindAll<ProductLineInProcessLocationDetail>(criteria);

            if (productLineInProcessLocationDetailList != null && productLineInProcessLocationDetailList.Count > 0)
            {
                if (productLineInProcessLocationDetailList.Count == 1)
                {
                    return productLineInProcessLocationDetailList[0].RemainQty;
                }
                else
                {
                    throw new TechnicalException("条码怎么会出现两次？");
                }
            }

            return null;
        }

        public IList<ProductLineInProcessLocationDetail> GetProductLineInProcessLocationDetailGroupByItem(string prodLineCode, string status)
        {
            IList<ProductLineInProcessLocationDetail> plIpGroupList = new List<ProductLineInProcessLocationDetail>();
            IList<ProductLineInProcessLocationDetail> plIpList = GetProductLineInProcessLocationDetail(prodLineCode, null, null, status, null);
            foreach (ProductLineInProcessLocationDetail plIpDetail in plIpList)
            {
                bool isExist = false;
                foreach (ProductLineInProcessLocationDetail plIpGroupDetail in plIpGroupList)
                {
                    if (plIpGroupDetail.Item.Code == plIpDetail.Item.Code)
                    {
                        isExist = true;
                        plIpGroupDetail.Qty += plIpDetail.Qty;
                        plIpGroupDetail.BackflushQty += plIpGroupDetail.BackflushQty;
                        break;
                    }
                }
                if (!isExist)
                {
                    ProductLineInProcessLocationDetail newPlIpDetail = new ProductLineInProcessLocationDetail();
                    newPlIpDetail.Item = plIpDetail.Item;
                    newPlIpDetail.Qty = plIpDetail.Qty;
                    newPlIpDetail.BackflushQty = plIpDetail.BackflushQty;
                    plIpGroupList.Add(newPlIpDetail);
                }

            }

            return plIpGroupList;
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialIn(Flow prodLine, IList<MaterialIn> materialInList, User user)
        {
            RawMaterialIn(prodLine.Code, materialInList, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialIn(string prodLineCode, IList<MaterialIn> materialInList, User user)
        {
            Flow flow = this.flowMgrE.CheckAndLoadFlow(prodLineCode);
            IList<BomDetail> bomDetailList = this.flowMgrE.GetBatchFeedBomDetail(flow);

            IList<MaterialIn> noneZeroMaterialInList = new List<MaterialIn>();
            DateTime dateTimeNow = DateTime.Now;

            if (materialInList != null && materialInList.Count > 0)
            {
                foreach (MaterialIn materialIn in materialInList)
                {
                    if (materialIn.Qty != 0)
                    {
                        noneZeroMaterialInList.Add(materialIn);
                    }

                    #region 查找物料是否是生产线上投料的
                    if (bomDetailList != null && bomDetailList.Count > 0)
                    {
                        bool findMatch = (from det in bomDetailList
                                          where det.Item.Code == materialIn.RawMaterial.Code
                                          select det).ToList().Count > 0;

                        #region 判断是否后续物料
                        if (!findMatch)
                        {
                            DetachedCriteria criteria = DetachedCriteria.For<ItemDiscontinue>();

                            criteria.Add(Expression.Eq("DiscontinueItem", materialIn.RawMaterial));
                            criteria.Add(Expression.Le("StartDate", dateTimeNow));
                            criteria.Add(Expression.Or(Expression.IsNull("EndDate"), Expression.Ge("EndDate", dateTimeNow)));

                            IList<ItemDiscontinue> disConItems = this.criteriaMgrE.FindAll<ItemDiscontinue>(criteria);
                            if (disConItems != null && disConItems.Count > 0)
                            {
                                findMatch = (from det in bomDetailList
                                             join disConItem in disConItems
                                             on det.Item.Code equals disConItem.Item.Code
                                             where disConItem.Bom == null || disConItem.Bom.Code == det.Bom.Code
                                             select det).ToList().Count > 0;
                            }
                        }
                        #endregion

                        if (!findMatch)
                        {
                            throw new BusinessErrorException("MasterData.Production.Feed.Error.NotContainMaterial", materialIn.RawMaterial.Code, prodLineCode);
                        }
                    }
                    else
                    {
                        throw new BusinessErrorException("MasterData.Feed.Error.NoFeedMaterial", prodLineCode);
                    }
                    #endregion
                }
            }

            if (noneZeroMaterialInList.Count == 0)
            {
                throw new BusinessErrorException("Order.Error.ProductLineInProcessLocationDetailEmpty");
            }

            foreach (MaterialIn materialIn in noneZeroMaterialInList)
            {
                #region 出库
                IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryOut(materialIn, user, flow);
                #endregion

                #region 入生产线物料
                foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                {
                    ProductLineInProcessLocationDetail productLineInProcessLocationDetail = new ProductLineInProcessLocationDetail();
                    productLineInProcessLocationDetail.ProductLine = flow;
                    productLineInProcessLocationDetail.ProductLineFacility = materialIn.ProductLineFacility;
                    productLineInProcessLocationDetail.OrderNo = materialIn.OrderNo;
                    productLineInProcessLocationDetail.Operation = materialIn.Operation;
                    productLineInProcessLocationDetail.Item = inventoryTransaction.Item;
                    productLineInProcessLocationDetail.HuId = inventoryTransaction.Hu != null ? inventoryTransaction.Hu.HuId : null;
                    productLineInProcessLocationDetail.LotNo = inventoryTransaction.Hu != null ? inventoryTransaction.Hu.LotNo : null;
                    productLineInProcessLocationDetail.Qty = 0 - inventoryTransaction.Qty;
                    productLineInProcessLocationDetail.CurrentQty = productLineInProcessLocationDetail.Qty;
                    productLineInProcessLocationDetail.IsConsignment = inventoryTransaction.IsConsignment;
                    productLineInProcessLocationDetail.PlannedBill = inventoryTransaction.PlannedBill;
                    productLineInProcessLocationDetail.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
                    productLineInProcessLocationDetail.LocationFrom = inventoryTransaction.Location;
                    productLineInProcessLocationDetail.CreateDate = dateTimeNow;
                    productLineInProcessLocationDetail.CreateUser = user;
                    //productLineInProcessLocationDetail.LastModifyDate = dateTimeNow;
                    productLineInProcessLocationDetail.LastModifyUser = user;

                    this.CreateProductLineInProcessLocationDetail(productLineInProcessLocationDetail);

                    //记录库存事务
                    this.locationTransactionMgrE.RecordLocationTransaction(productLineInProcessLocationDetail, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_MATERIAL_IN, user, BusinessConstants.IO_TYPE_IN);
                }
                #endregion
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialIn4Order(OrderHead orderHead, IDictionary<string, string> seqHuIdDic, User user)
        {
            this.RawMaterialIn4Order(orderHead.OrderNo, seqHuIdDic, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialIn4Order(string orderNo, IDictionary<string, string> seqHuIdDic, User user)
        {
            OrderHead orderHead = this.orderHeadMgr.LoadOrderHead(orderNo, true, false, true);

            #region 状态检验
            if (orderHead.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
            {
                throw new BusinessErrorException("MasterData.Feed.Error.OrderStatusErrorWhenFeed", orderHead.Status, orderHead.OrderNo);
            }
            #endregion

            #region 生产单明细只能有一条
            if (orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 1)
            {
                throw new BusinessErrorException("MasterData.Feed.Error.OrderDetailGT1", orderHead.OrderNo);
            }
            #endregion

            #region 检查是否已经有上料的工单
            //DetachedCriteria criteria = DetachedCriteria.For<ProductLineInProcessLocationDetail>();

            //criteria.SetProjection(Projections.ProjectionList().Add(Projections.RowCount()));

            //criteria.Add(Expression.Eq("ProductLine.Code", orderHead.Flow));
            //criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
            //if (orderHead.ProductLineFacility != null && orderHead.ProductLineFacility.Trim() != string.Empty)
            //{
            //    criteria.Add(Expression.Eq("ProductLineFacility", orderHead.ProductLineFacility.Trim()));
            //}

            //if (this.criteriaMgrE.FindAll<int>(criteria)[0] > 0)
            //{
            //    //发现生产线已经有投料
            //    if (orderHead.ProductLineFacility != null && orderHead.ProductLineFacility.Trim() != string.Empty)
            //    {
            //        throw new BusinessErrorException("MasterDate.ProductLineFacility.Error.AlreadyFed", orderHead.Flow, orderHead.ProductLineFacility.Trim());
            //    }
            //    else
            //    {
            //        throw new BusinessErrorException("MasterDate.ProductLine.Error.AlreadyFed", orderHead.Flow);
            //    }
            //}
            #endregion

            Item fg = orderHead.OrderDetails[0].Item;
            IList<MaterialIn> noneZeroMaterialInList = new List<MaterialIn>();
            DateTime dateTimeNow = DateTime.Now;

            #region 上料明细不能为空
            if (seqHuIdDic != null && seqHuIdDic.Count > 0)
            {
                foreach (string pos in seqHuIdDic.Keys)
                {
                    string huId = seqHuIdDic[pos];
                    Hu hu = this.huMgr.CheckAndLoadHu(huId);

                    MaterialIn materialIn = new MaterialIn();
                    materialIn.HuId = huId;
                    materialIn.OrderNo = orderNo;
                    materialIn.RawMaterial = hu.Item;
                    materialIn.Qty = hu.Qty;
                    //OrderLocationTransaction orderLocationTransaction = (from orderLoc in orderHead.OrderDetails[0].OrderLocationTransactions
                    //                                                     where orderLoc.Item.Code == hu.Item.Code //&& orderLoc.BackFlushMethod == BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_GOODS_RECEIVE
                    //                                                     && orderLoc.IOType == BusinessConstants.IO_TYPE_OUT
                    //                                                     select orderLoc).SingleOrDefault();
                    materialIn.Location = orderHead.OrderDetails[0].DefaultLocationFrom;
                    materialIn.Position = pos;
                    materialIn.ProductLineFacility = orderHead.ProductLineFacility;
                    noneZeroMaterialInList.Add(materialIn);
                }
            }

            if (noneZeroMaterialInList == null || noneZeroMaterialInList.Count == 0)
            {
                throw new BusinessErrorException("MasterData.Feed.Error.NoFeedMaterial4Order", orderNo);
            }
            #endregion

            #region 上料检验
            //IList<MaterialIn> sortedMaterialInList = (from min in noneZeroMaterialInList
            //                                          orderby min.Sequence ascending
            //                                          select min).ToList();

            bool feedOver = false;
            foreach (MaterialIn materialIn in noneZeroMaterialInList)
            {
                if (CheckAndGetNextProdutLineFeedSeqence(orderNo, materialIn.Position, materialIn.HuId) == null)
                {
                    feedOver = true;
                }
            }

            if (!feedOver)
            {
                throw new BusinessErrorException("MasterData.Feed.Error.FeedNotComplete");
            }
            #endregion

            #region 投料
            //RawMaterialIn(orderHead.Flow, sortedMaterialInList, user);
            #endregion

            #region 仅记录工单和投料关系
            foreach (MaterialIn materialIn in noneZeroMaterialInList)
            {
                ProdLineIp2 ip2 = new ProdLineIp2();

                ip2.ProdLine = orderHead.Flow;
                ip2.ProdLineFact = orderHead.ProductLineFacility;
                ip2.OrderNo = orderHead.OrderNo;
                ip2.Item = materialIn.RawMaterial.Code;
                ip2.ItemDescription = materialIn.RawMaterial.Description;
                ip2.Hu = this.huMgr.LoadHu(materialIn.HuId);
                ip2.CreateDate = dateTimeNow;
                ip2.CreateUser = user.Code;
                ip2.Position = materialIn.Position;
                ip2.Type = "Feed";
                ip2.FG = fg.Code;

                this.prodLineIp2Mgr.CreateProdLineIp2(ip2);
            }
            #endregion

            #region 更新是否上料标记
            orderHead.HasSortAndColor = true;
            this.orderHeadMgr.UpdateOrderHead(orderHead);
            #endregion
        }

        [Transaction(TransactionMode.Unspecified)]
        public ProdutLineFeedSeqence CheckAndGetNextProdutLineFeedSeqence(string orderNo, string position, string huId)
        {

            OrderHead orderHead = this.orderHeadMgr.LoadOrderHead(orderNo, true);
            if (orderHead.OrderDetails.Count > 1)
            {
                throw new BusinessErrorException("MasterData.Feed.Error.OrderDetailGT1", orderNo);
            }
            Item fg = orderHead.OrderDetails[0].Item;

            DetachedCriteria criteria = DetachedCriteria.For<ProdutLineFeedSeqence>();

            criteria.Add(Expression.Eq("ProductLineFacility", orderHead.ProductLineFacility));
            criteria.Add(Expression.Eq("FinishGood.Code", fg.Code));
            if (position != null && position.Trim() != string.Empty)
            {
                criteria.Add(Expression.Ge("Code", position));
            }
            criteria.Add(Expression.Eq("IsActive", true));

            criteria.AddOrder(Order.Asc("Sequence"));

            IList<ProdutLineFeedSeqence> produtLineFeedSeqenceList = this.criteriaMgrE.FindAll<ProdutLineFeedSeqence>(criteria, 0, 2);

            if (position == null || position.Trim() == string.Empty)
            {
                if (produtLineFeedSeqenceList == null || produtLineFeedSeqenceList.Count == 0)
                {
                    throw new BusinessErrorException("MasterData.Feed.Error.FeedSeqNotFound", orderHead.Flow, fg.Code);
                }

                return produtLineFeedSeqenceList[0];
            }
            else
            {
                Hu hu = this.huMgr.CheckAndLoadHu(huId);

                #region 检查工单是否包含该物料
                //criteria = DetachedCriteria.For<OrderLocationTransaction>();

                //criteria.CreateAlias("OrderDetail", "od");
                //criteria.CreateAlias("od.OrderHead", "oh");

                //criteria.Add(Expression.Eq("oh.OrderNo", orderHead.OrderNo));
                //criteria.Add(Expression.Eq("Item", hu.Item));
                //criteria.Add(Expression.Eq("IOType", BusinessConstants.IO_TYPE_OUT));

                //criteria.SetProjection(Projections.ProjectionList().Add(Projections.RowCount()));

                //IList rowCount = this.criteriaMgrE.FindAll(criteria);

                //if (rowCount != null && rowCount[0] != null && (int)(rowCount[0]) > 0)
                //{

                //}
                //else
                //{
                //    throw new BusinessErrorException("Order.Production.RawMaterailNotInWO", orderHead.OrderNo, hu.Item.Code);
                //}
                #endregion

                ProdutLineFeedSeqence currentProdutLineFeedSeqence = produtLineFeedSeqenceList[0];
                if (currentProdutLineFeedSeqence.Code != position)
                {
                    throw new TechnicalException("Material In Sequence not correct.");
                }

                if (currentProdutLineFeedSeqence.RawMaterial.Code != hu.Item.Code)
                {
                    #region 判断是否后续物料
                    DateTime dateTimeNow = DateTime.Now;

                    criteria = DetachedCriteria.For<ItemDiscontinue>();
                    criteria.Add(Expression.Eq("Item", currentProdutLineFeedSeqence.RawMaterial));
                    criteria.Add(Expression.Eq("DiscontinueItem", hu.Item));
                    criteria.Add(Expression.Le("StartDate", dateTimeNow));
                    criteria.Add(Expression.Or(Expression.IsNull("EndDate"), Expression.Ge("EndDate", dateTimeNow)));

                    IList<ItemDiscontinue> disConItems = this.criteriaMgrE.FindAll<ItemDiscontinue>(criteria);
                    if (disConItems == null || disConItems.Count == 0)
                    {
                        throw new BusinessErrorException("MasterData.Feed.Error.FeedRMNotCorrect", orderHead.Flow, fg.Code, currentProdutLineFeedSeqence.Sequence.ToString(), currentProdutLineFeedSeqence.RawMaterial.Code, hu.Item.Code);
                    }
                    #endregion
                }

                #region 分光分色校验
                //IList<OrderLocationTransaction> orderLocationTransactionList = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderHead.OrderDetails[0], BusinessConstants.IO_TYPE_OUT, hu.Item);
                //if (orderLocationTransactionList != null && orderLocationTransactionList.Count == 1)
                //{
                //    OrderLocationTransaction orderLocationTransaction = orderLocationTransactionList[0];
                //    if (orderLocationTransaction.Item.IsSortAndColor.HasValue && orderLocationTransaction.Item.IsSortAndColor.Value)
                //    {
                //        if (orderLocationTransaction.Item.ItemBrand == null)
                //        {
                //            throw new BusinessErrorException("MasterData.Item.Brand.Empty", orderLocationTransaction.Item.Code);
                //        }

                //        if (orderLocationTransaction.Item.ColorLevel1From != null && orderLocationTransaction.Item.ColorLevel1From.Trim() != string.Empty && orderLocationTransaction.Item.ColorLevel1From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                //        {
                //            this.ledColorLevelMgr.CheckLedFeedColorLevel(orderLocationTransaction.Item.Code, orderLocationTransaction.Item.ItemBrand.Code, orderLocationTransaction.Item.ColorLevel1From, orderLocationTransaction.Item.ColorLevel1To, hu.ColorLevel1);
                //        }

                //        if (orderLocationTransaction.Item.SortLevel1From != null && orderLocationTransaction.Item.SortLevel1From.Trim() != string.Empty && orderLocationTransaction.Item.SortLevel1From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                //        {
                //            this.ledSortLevelMgr.CheckLedFeedSortLevel(orderLocationTransaction.Item.Code, orderLocationTransaction.Item.ItemBrand.Code, orderLocationTransaction.Item.SortLevel1From, orderLocationTransaction.Item.SortLevel1To, hu.SortLevel1);
                //        }

                //        if (orderLocationTransaction.Item.ColorLevel2From != null && orderLocationTransaction.Item.ColorLevel2From.Trim() != string.Empty && orderLocationTransaction.Item.ColorLevel2From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                //        {
                //            this.ledColorLevelMgr.CheckLedFeedColorLevel(orderLocationTransaction.Item.Code, orderLocationTransaction.Item.ItemBrand.Code, orderLocationTransaction.Item.ColorLevel2From, orderLocationTransaction.Item.ColorLevel2To, hu.ColorLevel2);
                //        }

                //        if (orderLocationTransaction.Item.SortLevel2From != null && orderLocationTransaction.Item.SortLevel2From.Trim() != string.Empty && orderLocationTransaction.Item.SortLevel2From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                //        {
                //            this.ledSortLevelMgr.CheckLedFeedSortLevel(orderLocationTransaction.Item.Code, orderLocationTransaction.Item.ItemBrand.Code, orderLocationTransaction.Item.SortLevel2From, orderLocationTransaction.Item.SortLevel2To, hu.SortLevel2);
                //        }
                //    }
                //}
                //else
                //{
                //    throw new BusinessErrorException("MasterData.Feed.Error.FeedRMNotFound", orderHead.OrderNo, hu.Item.Code);
                //}
                if (hu.Item.IsSortAndColor.HasValue && hu.Item.IsSortAndColor.Value)
                {
                    if (hu.Item.ItemBrand == null)
                    {
                        throw new BusinessErrorException("MasterData.Item.Brand.Empty", hu.Item.Code);
                    }

                    if (hu.Item.ColorLevel1From != null && hu.Item.ColorLevel1From.Trim() != string.Empty && hu.Item.ColorLevel1From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                    {
                        this.ledColorLevelMgr.CheckLedFeedColorLevel(hu.Item.Code, hu.Item.ItemBrand.Code, hu.Item.ColorLevel1From, hu.Item.ColorLevel1To, hu.ColorLevel1);
                    }

                    if (hu.Item.SortLevel1From != null && hu.Item.SortLevel1From.Trim() != string.Empty && hu.Item.SortLevel1From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                    {
                        this.ledSortLevelMgr.CheckLedFeedSortLevel(hu.Item.Code, hu.Item.ItemBrand.Code, hu.Item.SortLevel1From, hu.Item.SortLevel1To, hu.SortLevel1);
                    }

                    if (hu.Item.ColorLevel2From != null && hu.Item.ColorLevel2From.Trim() != string.Empty && hu.Item.ColorLevel2From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                    {
                        this.ledColorLevelMgr.CheckLedFeedColorLevel(hu.Item.Code, hu.Item.ItemBrand.Code, hu.Item.ColorLevel2From, hu.Item.ColorLevel2To, hu.ColorLevel2);
                    }

                    if (hu.Item.SortLevel2From != null && hu.Item.SortLevel2From.Trim() != string.Empty && hu.Item.SortLevel2From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                    {
                        this.ledSortLevelMgr.CheckLedFeedSortLevel(hu.Item.Code, hu.Item.ItemBrand.Code, hu.Item.SortLevel2From, hu.Item.SortLevel2To, hu.SortLevel2);
                    }
                }
                #endregion

                if (produtLineFeedSeqenceList.Count == 2)
                {
                    return produtLineFeedSeqenceList[1];
                }
                else
                {
                    return null;
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void ExchangeProdutLineFeed(string exchangeHuIdFrom, string exchangeHuIdTo, User user)
        {
            ExchangeProdutLineFeed(exchangeHuIdFrom, exchangeHuIdTo, user, true);
        }

        [Transaction(TransactionMode.Requires)]
        public void ExchangeProdutLineFeed(string exchangeHuIdFrom, string exchangeHuIdTo, User user, bool checkSortAndColor)
        {
            Hu exchangeHuFrom = this.huMgr.CheckAndLoadHu(exchangeHuIdFrom);
            Hu exchangeHuTo = this.huMgr.CheckAndLoadHu(exchangeHuIdTo);

            //DetachedCriteria criteria = DetachedCriteria.For<ProductLineInProcessLocationDetail>();

            //criteria.Add(Expression.Eq("HuId", exchangeHuFrom.HuId));
            //criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));

            //IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.criteriaMgrE.FindAll<ProductLineInProcessLocationDetail>(criteria);

            //if (productLineInProcessLocationDetailList == null || productLineInProcessLocationDetailList.Count == 0)
            //{
            //    throw new BusinessErrorException("MasterData.Feed.Error.HuIdNotFound", exchangeHuFrom.HuId);
            //}
            //else if (productLineInProcessLocationDetailList.Count > 1)
            //{
            //    throw new TechnicalException("HuId found more than 1 time in the productline" + exchangeHuFrom.HuId);
            //}

            #region 分光分色校验
            if (checkSortAndColor)
            {
                if (exchangeHuFrom.Item.Code != exchangeHuTo.Item.Code)
                {
                    throw new BusinessErrorException("MasterData.Feed.Error.ItemNotEqualWhenExchange", exchangeHuFrom.Item.Code, exchangeHuTo.Item.Code);
                }

                if ((exchangeHuFrom.ColorLevel1 != exchangeHuTo.ColorLevel1 && exchangeHuFrom.SortLevel1 != exchangeHuTo.SortLevel1)
                    || (exchangeHuFrom.ColorLevel2 != exchangeHuTo.ColorLevel2 && exchangeHuFrom.SortLevel2 != exchangeHuTo.SortLevel2))
                {
                    throw new BusinessErrorException("MasterData.Feed.Error.SortAndColorBothChangeWhenExchange");
                }
                
                if (exchangeHuFrom.Item.ColorLevel1From != null && exchangeHuFrom.Item.ColorLevel1From.Trim() != string.Empty && exchangeHuFrom.Item.ColorLevel1From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL
                    && exchangeHuFrom.ColorLevel1 != exchangeHuTo.ColorLevel1)
                {
                    this.ledColorLevelMgr.CheckLedFeedColorLevel(exchangeHuTo.Item.Code, exchangeHuTo.Item.ItemBrand.Code, exchangeHuTo.Item.ColorLevel1From, exchangeHuTo.Item.ColorLevel1To, exchangeHuTo.ColorLevel1);

                    if (!this.ledColorLevelMgr.IsNearByLedColorLevel(exchangeHuFrom.Item.Code, exchangeHuFrom.Item.ItemBrand.Code, exchangeHuFrom.ColorLevel1, exchangeHuTo.ColorLevel1))
                    {
                        throw new BusinessErrorException("MasterData.Feed.Error.ColorLevelNotNearByWhenExchange");
                    }
                }

                if (exchangeHuFrom.Item.SortLevel1From != null && exchangeHuFrom.Item.SortLevel1From.Trim() != string.Empty && exchangeHuFrom.Item.SortLevel1From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL
                    && exchangeHuFrom.SortLevel1 != exchangeHuTo.SortLevel1)
                {
                    this.ledSortLevelMgr.CheckLedFeedSortLevel(exchangeHuTo.Item.Code, exchangeHuTo.Item.ItemBrand.Code, exchangeHuTo.Item.SortLevel1From, exchangeHuTo.Item.SortLevel1To, exchangeHuTo.SortLevel1);

                    if (!this.ledSortLevelMgr.IsNearByLedSortLevel(exchangeHuFrom.Item.Code, exchangeHuFrom.Item.ItemBrand.Code, exchangeHuFrom.SortLevel1, exchangeHuTo.SortLevel1))
                    {
                        throw new BusinessErrorException("MasterData.Feed.Error.SortLevelNotNearByWhenExchange");
                    }
                }

                if (exchangeHuFrom.Item.ColorLevel2From != null && exchangeHuFrom.Item.ColorLevel2From.Trim() != string.Empty && exchangeHuFrom.Item.ColorLevel2From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL
                    && exchangeHuFrom.ColorLevel2 != exchangeHuTo.ColorLevel2)
                {
                    this.ledColorLevelMgr.CheckLedFeedColorLevel(exchangeHuTo.Item.Code, exchangeHuTo.Item.ItemBrand.Code, exchangeHuTo.Item.ColorLevel2From, exchangeHuTo.Item.ColorLevel2To, exchangeHuTo.ColorLevel2);

                    if (!this.ledColorLevelMgr.IsNearByLedColorLevel(exchangeHuFrom.Item.Code, exchangeHuFrom.Item.ItemBrand.Code, exchangeHuFrom.ColorLevel2, exchangeHuTo.ColorLevel2))
                    {
                        throw new BusinessErrorException("MasterData.Feed.Error.ColorLevelNotNearByWhenExchange");
                    }
                }

                if (exchangeHuFrom.Item.SortLevel2From != null && exchangeHuFrom.Item.SortLevel2From.Trim() != string.Empty && exchangeHuFrom.Item.SortLevel2From.Trim() != BusinessConstants.SORT_COLOR_IGNORE_LABEL
                    && exchangeHuFrom.SortLevel2 != exchangeHuTo.SortLevel2)
                {
                    this.ledSortLevelMgr.CheckLedFeedSortLevel(exchangeHuTo.Item.Code, exchangeHuTo.Item.ItemBrand.Code, exchangeHuTo.Item.SortLevel2From, exchangeHuTo.Item.SortLevel2To, exchangeHuTo.SortLevel2);

                    if (!this.ledSortLevelMgr.IsNearByLedSortLevel(exchangeHuFrom.Item.Code, exchangeHuFrom.Item.ItemBrand.Code, exchangeHuFrom.SortLevel2, exchangeHuTo.SortLevel2))
                    {
                        throw new BusinessErrorException("MasterData.Feed.Error.SortLevelNotNearByWhenExchange");
                    }
                }
            }
            #endregion

            #region 投料
            //MaterialIn materialIn = new MaterialIn();
            //materialIn.HuId = exchangeHuTo.HuId;
            //materialIn.Location = this.locationMgrE.CheckAndLoadLocation(exchangeHuTo.Location);
            //materialIn.LotNo = exchangeHuTo.LotNo;
            //materialIn.Operation = productLineInProcessLocationDetailList[0].Operation;
            //materialIn.OrderNo = productLineInProcessLocationDetailList[0].OrderNo;
            //materialIn.ProductLineFacility = productLineInProcessLocationDetailList[0].ProductLineFacility;
            //materialIn.Qty = exchangeHuTo.Qty;
            //materialIn.RawMaterial = exchangeHuTo.Item;

            //IList<MaterialIn> materialInList = new List<MaterialIn>();
            //materialInList.Add(materialIn);

            //RawMaterialIn(productLineInProcessLocationDetailList[0].ProductLine.Code, materialInList, user);
            #endregion

            #region 仅记录工单和投料关系
            DetachedCriteria criteria = DetachedCriteria.For<ProdLineIp2>();
            criteria.CreateAlias("Hu", "hu");

            criteria.Add(Expression.Eq("hu.HuId", exchangeHuFrom.HuId));
            criteria.AddOrder(Order.Desc("Id"));

            IList<ProdLineIp2> list = this.criteriaMgrE.FindAll<ProdLineIp2>(criteria, 0, 1);

            if (list == null || list.Count == 0)
            {
                throw new BusinessErrorException("MasterData.Feed.Error.HuIdNotFound", exchangeHuFrom.HuId);
            }
            //else if (list.Count > 1)
            //{
            //    throw new TechnicalException("HuId found more than 1 time in the productline" + exchangeHuFrom.HuId);
            //}

            ProdLineIp2 ip2 = new ProdLineIp2();

            ip2.ProdLine = list[0].ProdLine;
            ip2.ProdLineFact = list[0].ProdLineFact;
            ip2.OrderNo = list[0].OrderNo;
            ip2.Item = list[0].Item;
            ip2.ItemDescription = list[0].ItemDescription;
            ip2.Hu = this.huMgr.LoadHu(exchangeHuIdTo);
            ip2.CreateDate = DateTime.Now;
            ip2.CreateUser = user.Code;
            ip2.Position = list[0].Position;
            ip2.Type = "Exchange";
            ip2.FG = list[0].FG;

            this.prodLineIp2Mgr.CreateProdLineIp2(ip2);
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialBackflush(string prodLineCode, User user)
        {
            this.RawMaterialBackflush(prodLineCode, null, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialBackflush(string prodLineCode, IDictionary<string, decimal> itemQtydic, User user)
        {

            if (itemQtydic == null || itemQtydic.Count == 0)
            {
                throw new BusinessErrorException("MasterData.Production.Feed.Error.NoSelectFeed");
            }

            Flow flow = this.flowMgrE.CheckAndLoadFlow(prodLineCode);
            DateTime dateTimeNow = DateTime.Now;

            IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList =
                this.GetProductLineInProcessLocationDetail(prodLineCode, null, null, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, itemQtydic.Keys.ToArray<string>());

            IList<ProductLineInProcessLocationDetail> targetProductLineInProcessLocationDetailList = new List<ProductLineInProcessLocationDetail>();

            #region 根据剩余数量计算回冲零件数量，添加到待处理列表
            if (itemQtydic != null && itemQtydic.Count > 0)
            {
                foreach (string itemCode in itemQtydic.Keys)
                {
                    decimal remainQty = itemQtydic[itemCode];   //剩余投料量
                    decimal inQty = 0;                     //总投料量
                    IList<ProductLineInProcessLocationDetail> currentProductLineInProcessLocationDetailList = new List<ProductLineInProcessLocationDetail>();
                    foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
                    {
                        if (productLineInProcessLocationDetail.Item.Code == itemCode)
                        {
                            inQty += (productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty);
                            currentProductLineInProcessLocationDetailList.Add(productLineInProcessLocationDetail);
                        }
                    }

                    if (remainQty > inQty)
                    {
                        throw new BusinessErrorException("MasterData.Production.Feed.Error.RemainQtyGtFeedQty", itemCode);
                    }

                    decimal backflushQty = inQty - remainQty;  //本次回冲量

                    #region 设定本次回冲数量
                    if (backflushQty > 0)
                    {
                        foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in currentProductLineInProcessLocationDetailList)
                        {
                            if (backflushQty - (productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty) > 0)
                            {
                                productLineInProcessLocationDetail.CurrentBackflushQty = productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty;
                                backflushQty -= productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty;
                                productLineInProcessLocationDetail.BackflushQty = productLineInProcessLocationDetail.Qty;
                                targetProductLineInProcessLocationDetailList.Add(productLineInProcessLocationDetail);
                            }
                            else
                            {
                                productLineInProcessLocationDetail.CurrentBackflushQty = backflushQty;
                                productLineInProcessLocationDetail.BackflushQty += backflushQty;
                                backflushQty = 0;
                                targetProductLineInProcessLocationDetailList.Add(productLineInProcessLocationDetail);
                                break;
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion

            //为了多次回冲，注掉此处代码
            //#region 处理未填写剩余数量的投料，全部添加到待处理列表
            //foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
            //{
            //    bool isUsed = false;
            //    foreach (string itemCode in itemQtydic.Keys)
            //    {
            //        if (productLineInProcessLocationDetail.Item.Code == itemCode)
            //        {
            //            isUsed = true;
            //            break;
            //        }
            //    }

            //    //未填写剩余数量的全部回冲
            //    if (!isUsed)
            //    {
            //        productLineInProcessLocationDetail.CurrentBackflushQty = productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty;
            //        productLineInProcessLocationDetail.BackflushQty = productLineInProcessLocationDetail.Qty;
            //        targetProductLineInProcessLocationDetailList.Add(productLineInProcessLocationDetail);
            //    }
            //}
            //#endregion

            if (targetProductLineInProcessLocationDetailList != null && targetProductLineInProcessLocationDetailList.Count > 0)
            {
                #region 更新生产线上的物料
                foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in targetProductLineInProcessLocationDetailList)
                {
                    if (productLineInProcessLocationDetail.Qty == productLineInProcessLocationDetail.BackflushQty)
                    {
                        productLineInProcessLocationDetail.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                    }
                    //productLineInProcessLocationDetail.LastModifyDate = dateTimeNow;
                    productLineInProcessLocationDetail.LastModifyUser = user;

                    this.UpdateProductLineInProcessLocationDetail(productLineInProcessLocationDetail);

                    //记录库存事务
                    //this.locationTransactionMgrE.RecordLocationTransaction(productLineInProcessLocationDetail, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_TR, user, BusinessConstants.IO_TYPE_OUT);
                    //this.locationTransactionMgrE.RecordLocationTransaction(productLineInProcessLocationDetail, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_TR, user, BusinessConstants.IO_TYPE_OUT);
                }
                #endregion

                IList<OrderPlannedBackflush> orderPlannedBackflushList = this.orderPlannedBackflushMgrE.GetActiveOrderPlannedBackflush(prodLineCode, itemQtydic.Keys.ToArray<string>());
                if (orderPlannedBackflushList == null || orderPlannedBackflushList.Count == 0)
                {
                    throw new BusinessErrorException("MasterData.Production.Feed.Error.NoWO", prodLineCode);
                }

                var mainOrderPlannedBackflushList = from opb in orderPlannedBackflushList
                                                    select new
                                                    {
                                                        Item = opb.OrderLocationTransaction.Item,
                                                        PlannedQty = opb.PlannedQty,
                                                        OrderPlannedBackflush = opb
                                                    };
                #region 查找替换物料
                DateTime minCreateDate = (from opb in orderPlannedBackflushList
                                          orderby opb.CreateDate ascending
                                          select opb.CreateDate).First();

                DateTime maxCreateDate = (from opb in orderPlannedBackflushList
                                          orderby opb.CreateDate descending
                                          select opb.CreateDate).First();


                DetachedCriteria criteria = DetachedCriteria.For<ItemDiscontinue>();

                criteria.Add(Expression.Ge("StartDate", maxCreateDate));
                criteria.Add(Expression.Or(Expression.Le("EndDate", minCreateDate), Expression.IsNull("EndDate")));
                criteria.Add(Expression.In("Item.Code", itemQtydic.Keys.ToArray<string>()));

                IList<ItemDiscontinue> itemDiscontinueList = this.criteriaMgrE.FindAll<ItemDiscontinue>(criteria);

                if (itemDiscontinueList != null && itemDiscontinueList.Count > 0)
                {
                    var disOrderPlannedBackflushList = from i in itemDiscontinueList
                                                       join m in mainOrderPlannedBackflushList on i.Item.Code equals m.Item.Code
                                                       select new
                                                       {
                                                           Item = i.DiscontinueItem,
                                                           PlannedQty = m.PlannedQty * i.UnitQty,
                                                           OrderPlannedBackflush = m.OrderPlannedBackflush
                                                       };

                    if (disOrderPlannedBackflushList != null && disOrderPlannedBackflushList.Count() > 0)
                    {
                        mainOrderPlannedBackflushList = mainOrderPlannedBackflushList.Concat(disOrderPlannedBackflushList);
                    }
                }
                #endregion

                var productLineInProcessLocationDetailDic = from plIp in targetProductLineInProcessLocationDetailList
                                                            group plIp by new
                                                            {
                                                                Item = plIp.Item.Code,
                                                                Operation = plIp.Operation,
                                                                HuId = plIp.HuId,
                                                                LotNo = plIp.LotNo,
                                                                LocationFrom = plIp.LocationFrom,
                                                                IsConsignment = plIp.IsConsignment,
                                                                PlannedBill = plIp.PlannedBill
                                                            } into result
                                                            select new
                                                            {
                                                                Item = result.Key.Item,
                                                                Operation = result.Key.Operation,
                                                                HuId = result.Key.HuId,
                                                                LotNo = result.Key.LotNo,
                                                                LocationFrom = result.Key.LocationFrom,
                                                                IsConsignment = result.Key.IsConsignment,
                                                                PlannedBill = result.Key.PlannedBill,
                                                                BackflushQty = result.Sum(plIp => plIp.CurrentBackflushQty)
                                                            };

                foreach (var productLineInProcessLocationDetail in productLineInProcessLocationDetailDic)
                {
                    var planList = mainOrderPlannedBackflushList.Where(p => p.Item.Code == productLineInProcessLocationDetail.Item
                        && (!productLineInProcessLocationDetail.Operation.HasValue || productLineInProcessLocationDetail.Operation == p.OrderPlannedBackflush.OrderLocationTransaction.Operation)).ToList();

                    var totalBaseQty = planList.Sum(p => p.PlannedQty); //回冲分配基数

                    if (planList.Count > 0)
                    {
                        EntityPreference entityPreference = this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
                        int amountDecimalLength = int.Parse(entityPreference.Value);

                        decimal remainTobeBackflushQty = productLineInProcessLocationDetail.BackflushQty;  //剩余待回冲数量
                        decimal unitQty = remainTobeBackflushQty / totalBaseQty;  //单位基数的回冲数量

                        for (int i = 0; i < planList.Count; i++)
                        {
                            #region 物料回冲
                            #region 更新匹配的OrderLocationTransaction
                            Item matchedItem = planList[i].Item;
                            OrderPlannedBackflush matchedOrderPlannedBackflush = planList[i].OrderPlannedBackflush;
                            OrderLocationTransaction matchedOrderLocationTransaction = matchedOrderPlannedBackflush.OrderLocationTransaction;

                            bool isLastestRecord = (i == (planList.Count - 1));
                            decimal currentTotalBackflushQty = 0;

                            if (!matchedOrderLocationTransaction.AccumulateQty.HasValue)
                            {
                                matchedOrderLocationTransaction.AccumulateQty = 0;
                            }

                            if (!isLastestRecord)
                            {
                                decimal currentBackflushQty = Math.Round(planList[i].PlannedQty * unitQty, amountDecimalLength, MidpointRounding.AwayFromZero);
                                currentTotalBackflushQty += currentBackflushQty;
                                matchedOrderLocationTransaction.AccumulateQty += currentBackflushQty;
                                remainTobeBackflushQty -= currentBackflushQty;
                            }
                            else
                            {
                                currentTotalBackflushQty += remainTobeBackflushQty;
                                matchedOrderLocationTransaction.AccumulateQty += remainTobeBackflushQty;
                                remainTobeBackflushQty = 0;
                            }

                            this.orderLocationTransactionMgrE.UpdateOrderLocationTransaction(matchedOrderLocationTransaction);
                            #endregion

                            #region 新增/更新AsnDetail
                            InProcessLocationDetail inProcessLocationDetail = null;
                            if (productLineInProcessLocationDetail.HuId == null || productLineInProcessLocationDetail.HuId.Trim() == string.Empty)
                            {
                                inProcessLocationDetail = this.inProcessLocationDetailMgrE.GetNoneHuAndIsConsignmentInProcessLocationDetail(matchedOrderPlannedBackflush.InProcessLocation, matchedOrderPlannedBackflush.OrderLocationTransaction);
                                if (inProcessLocationDetail != null)
                                {
                                    inProcessLocationDetail.Qty += currentTotalBackflushQty;

                                    this.inProcessLocationDetailMgrE.UpdateInProcessLocationDetail(inProcessLocationDetail);
                                }
                            }

                            if (inProcessLocationDetail == null)
                            {
                                inProcessLocationDetail = new InProcessLocationDetail();
                                inProcessLocationDetail.InProcessLocation = matchedOrderPlannedBackflush.InProcessLocation;
                                inProcessLocationDetail.OrderLocationTransaction = matchedOrderPlannedBackflush.OrderLocationTransaction;
                                inProcessLocationDetail.HuId = productLineInProcessLocationDetail.HuId;
                                inProcessLocationDetail.LotNo = productLineInProcessLocationDetail.LotNo;
                                inProcessLocationDetail.IsConsignment = productLineInProcessLocationDetail.IsConsignment;
                                inProcessLocationDetail.PlannedBill = productLineInProcessLocationDetail.PlannedBill;
                                inProcessLocationDetail.Qty = currentTotalBackflushQty;
                                inProcessLocationDetail.Item = matchedItem;

                                this.inProcessLocationDetailMgrE.CreateInProcessLocationDetail(inProcessLocationDetail);

                                matchedOrderPlannedBackflush.InProcessLocation.AddInProcessLocationDetail(inProcessLocationDetail);
                            }

                            #endregion

                            #region 新增库存事务
                            this.locationTransactionMgrE.RecordWOBackflushLocationTransaction(
                                matchedOrderPlannedBackflush.OrderLocationTransaction, matchedItem,
                                productLineInProcessLocationDetail.HuId, productLineInProcessLocationDetail.LotNo, currentTotalBackflushQty,
                                matchedOrderPlannedBackflush.InProcessLocation.IpNo, user, productLineInProcessLocationDetail.LocationFrom);
                            #endregion

                            #region 记录回冲成本事务
                            this.costMgr.RecordProductionBackFlushCostTransaction(matchedOrderPlannedBackflush.OrderLocationTransaction, matchedItem, -1 * currentTotalBackflushQty, user);
                            #endregion
                            #endregion

                            #region 关闭OrderPlannedBackflush
                            if (matchedOrderPlannedBackflush.IsActive)
                            {
                                matchedOrderPlannedBackflush.IsActive = false;
                                this.orderPlannedBackflushMgrE.UpdateOrderPlannedBackflush(matchedOrderPlannedBackflush);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region 没有匹配的OrderLocationTransaction
                        //退回原库位
                        throw new BusinessErrorException("MasterData.BackFlush.NotFoundResources", productLineInProcessLocationDetail.Item);
                        //this.locationMgrE.InventoryIn(productLineInProcessLocationDetail, user);
                        #endregion
                    }
                }
            }
            else
            {
                throw new BusinessErrorException("MasterData.Production.Feed.Error.NoFeed", prodLineCode);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialBackflush(Flow prodLine, User user)
        {
            this.RawMaterialBackflush(prodLine.Code, null, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialBackflush(Flow prodLine, IDictionary<string, decimal> itemQtydic, User user)
        {
            this.RawMaterialBackflush(prodLine.Code, itemQtydic, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void BackflushRawMaterial(string prodLineCode, Item item, ref decimal qty, OrderLocationTransaction orderLocationTransaction, string ipNo, User user)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ProductLineInProcessLocationDetail>();

            criteria.Add(Expression.Eq("ProductLine.Code", prodLineCode));
            criteria.Add(Expression.Eq("Item", item));
            criteria.Add(Expression.Eq("OrderNo", orderLocationTransaction.OrderDetail.OrderHead.OrderNo));
            criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));

            if (orderLocationTransaction.OrderDetail.OrderHead.ProductLineFacility != null
                && orderLocationTransaction.OrderDetail.OrderHead.ProductLineFacility.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("ProductLineFacility", orderLocationTransaction.OrderDetail.OrderHead.ProductLineFacility));
            }

            criteria.AddOrder(Order.Asc("Id"));

            IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.criteriaMgrE.FindAll<ProductLineInProcessLocationDetail>(criteria);

            if (productLineInProcessLocationDetailList != null && productLineInProcessLocationDetailList.Count > 0)
            {
                foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
                {
                    if (productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty > 0)
                    {
                        decimal currentTotalBackflushQty = 0;
                        if ((productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty) > qty)
                        {
                            currentTotalBackflushQty = qty;
                            productLineInProcessLocationDetail.BackflushQty += currentTotalBackflushQty;
                            qty = 0;
                        }
                        else
                        {
                            currentTotalBackflushQty = productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty;
                            qty -= currentTotalBackflushQty;
                            productLineInProcessLocationDetail.BackflushQty = productLineInProcessLocationDetail.Qty;
                            productLineInProcessLocationDetail.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                        }

                        #region 新增库存事务
                        this.locationTransactionMgrE.RecordWOBackflushLocationTransaction(
                            orderLocationTransaction, item, productLineInProcessLocationDetail.HuId,
                            productLineInProcessLocationDetail.LotNo, currentTotalBackflushQty,
                            ipNo, user, productLineInProcessLocationDetail.LocationFrom);
                        #endregion

                        if (qty == 0)
                        {
                            break;
                        }
                    }
                }
            }
        }


        [Transaction(TransactionMode.Requires)]
        public void RawMaterialReturnByProductLine(string prodLineCode, User user)
        {
            Flow flow = this.flowMgrE.CheckAndLoadFlow(prodLineCode);
            #region 检查生产线下面是否有实例
            if (this.productionLineFacilityMgr.HasProductLineFacility(prodLineCode))
            {
                //生产线有设备，需要扫描设备条码
                throw new BusinessErrorException("MasterDate.ProductLineFacility.Error.ScanFacilityFirst", prodLineCode);
            }
            #endregion

            DateTime dateTimeNow = DateTime.Now;

            IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.GetProductLineInProcessLocationDetail(flow.Code, null, null, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, null);
            foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
            {
                doReturnRawMaterial(productLineInProcessLocationDetail, productLineInProcessLocationDetail.RemainQty, dateTimeNow, user);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialReturnByProductLineFacility(string prodLineFacilityCode, User user)
        {
            ProductLineFacility productLineFacility = this.productionLineFacilityMgr.CheckAndLoadProductLineFacility(prodLineFacilityCode);
            DateTime dateTimeNow = DateTime.Now;

            IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.GetProductLineInProcessLocationDetail(null, productLineFacility.Code, null, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, null);
            foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
            {
                doReturnRawMaterial(productLineInProcessLocationDetail, productLineInProcessLocationDetail.RemainQty, dateTimeNow, user);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialReturnByOrderNo(string orderNo, User user)
        {
            OrderHead orderHead = this.orderHeadMgr.CheckAndLoadOrderHead(orderNo);
            DateTime dateTimeNow = DateTime.Now;

            IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.GetProductLineInProcessLocationDetail(null, null, orderHead.OrderNo, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, null);
            foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
            {
                doReturnRawMaterial(productLineInProcessLocationDetail, productLineInProcessLocationDetail.RemainQty, dateTimeNow, user);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialReturnByProductLine(string prodLineCode, IDictionary<string, decimal> returnHuQty, User user)
        {
            if (returnHuQty != null && returnHuQty.Count > 0)
            {
                RawMaterialReturn(prodLineCode, null, null, returnHuQty, user);
            }
            else
            {
                IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.GetProductLineInProcessLocationDetail(prodLineCode, null, null, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, null);

                IDictionary<string, decimal> returnHuQty2 = new Dictionary<string, decimal>();
                if (productLineInProcessLocationDetailList != null && productLineInProcessLocationDetailList.Count > 0)
                {
                    foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
                    {
                        returnHuQty2.Add(productLineInProcessLocationDetail.HuId, productLineInProcessLocationDetail.RemainQty);
                    }
                }

                RawMaterialReturn(prodLineCode, null, null, returnHuQty2, user);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialReturnByProductLineFacility(string prodLineFacilityCode, IDictionary<string, decimal> returnHuQty, User user)
        {
            if (returnHuQty != null && returnHuQty.Count > 0)
            {
                RawMaterialReturn(null, prodLineFacilityCode, null, returnHuQty, user);
            }
            else
            {
                IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.GetProductLineInProcessLocationDetail(null, prodLineFacilityCode, null, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, null);

                IDictionary<string, decimal> returnHuQty2 = new Dictionary<string, decimal>();
                if (productLineInProcessLocationDetailList != null && productLineInProcessLocationDetailList.Count > 0)
                {
                    foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
                    {
                        returnHuQty2.Add(productLineInProcessLocationDetail.HuId, productLineInProcessLocationDetail.RemainQty);
                    }
                }

                RawMaterialReturn(null, prodLineFacilityCode, null, returnHuQty2, user);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialReturnByOrderNo(string orderNo, IDictionary<string, decimal> returnHuQty, User user)
        {
            if (returnHuQty != null && returnHuQty.Count > 0)
            {
                RawMaterialReturn(null, null, orderNo, returnHuQty, user);
            }
            else
            {
                IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.GetProductLineInProcessLocationDetail(null, null, orderNo, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, null);

                IDictionary<string, decimal> returnHuQty2 = new Dictionary<string, decimal>();
                if (productLineInProcessLocationDetailList != null && productLineInProcessLocationDetailList.Count > 0)
                {
                    foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
                    {
                        returnHuQty2.Add(productLineInProcessLocationDetail.HuId, productLineInProcessLocationDetail.RemainQty);
                    }
                }

                RawMaterialReturn(null, null, orderNo, returnHuQty2, user);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialReturnByHuIdQty(IDictionary<string, decimal> returnHuQty, User user)
        {
            DateTime dateTimeNow = DateTime.Now;

            foreach (string huId in returnHuQty.Keys)
            {
                DetachedCriteria criteria = DetachedCriteria.For<ProductLineInProcessLocationDetail>();
                criteria.Add(Expression.Eq("HuId", huId));
                criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));

                IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.criteriaMgrE.FindAll<ProductLineInProcessLocationDetail>(criteria);

                if (productLineInProcessLocationDetailList != null && productLineInProcessLocationDetailList.Count > 0)
                {
                    doReturnRawMaterial(productLineInProcessLocationDetailList[0], returnHuQty[huId], dateTimeNow, user);
                }
                else
                {
                    throw new BusinessErrorException("MasterData.Return.Error.HuIdNotExist", huId);
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialReturnByHuId(IList<string> returnHu, User user)
        {
            DateTime dateTimeNow = DateTime.Now;

            foreach (string huId in returnHu)
            {
                DetachedCriteria criteria = DetachedCriteria.For<ProductLineInProcessLocationDetail>();
                criteria.Add(Expression.Eq("HuId", huId));
                criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));

                IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.criteriaMgrE.FindAll<ProductLineInProcessLocationDetail>(criteria);

                if (productLineInProcessLocationDetailList != null && productLineInProcessLocationDetailList.Count > 0)
                {
                    doReturnRawMaterial(productLineInProcessLocationDetailList[0], productLineInProcessLocationDetailList[0].RemainQty, dateTimeNow, user);
                }
                else
                {
                    throw new BusinessErrorException("MasterData.Return.Error.HuIdNotExist", huId);
                }
            }
        }

        #endregion Customized Methods

        #region Private Methods

        private void RawMaterialReturn(string prodLineCode, string prodLineFacilityCode, string orderNo, IDictionary<string, decimal> returnHuQty, User user)
        {
            if (returnHuQty == null || returnHuQty.Count == 0)
            {
                throw new BusinessErrorException("MasterData.Return.Error.NotEmpty");
            }

            DateTime dateTimeNow = DateTime.Now;
            foreach (string huId in returnHuQty.Keys)
            {
                decimal returnQty = returnHuQty[huId];
                DetachedCriteria criteria = DetachedCriteria.For<ProductLineInProcessLocationDetail>();

                if (prodLineCode != null && prodLineCode.Trim() != string.Empty)
                {
                    criteria.Add(Expression.Eq("ProductLine.Code", prodLineCode));
                }

                if (prodLineFacilityCode != null && prodLineFacilityCode.Trim() != string.Empty)
                {
                    criteria.Add(Expression.Eq("ProductLineFacility", prodLineFacilityCode));
                }

                if (orderNo != null && orderNo.Trim() != string.Empty)
                {
                    criteria.Add(Expression.Eq("OrderNo", orderNo));
                }

                criteria.Add(Expression.Eq("HuId", huId));
                criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));

                IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList = this.criteriaMgrE.FindAll<ProductLineInProcessLocationDetail>(criteria);

                if (productLineInProcessLocationDetailList == null || productLineInProcessLocationDetailList.Count == 0)
                {
                    throw new BusinessErrorException("MasterData.Return.Error.HuIdNotExist", huId);
                }

                ProductLineInProcessLocationDetail productLineInProcessLocationDetail = productLineInProcessLocationDetailList[0];
                //if (productLineInProcessLocationDetail.RemainQty < returnQty)
                //{
                //    throw new BusinessErrorException("MasterData.Return.Error.ReturnHuQtyGeRemianQty", huId);
                //}

                doReturnRawMaterial(productLineInProcessLocationDetail, returnQty, dateTimeNow, user);
            }
        }

        private void doReturnRawMaterial(ProductLineInProcessLocationDetail productLineInProcessLocationDetail, decimal returnQty, DateTime dateTimeNow, User user)
        {
            #region 校验退料数量不能大于生产线剩余数量
            if (productLineInProcessLocationDetail.RemainQty < returnQty)
            {
                if (productLineInProcessLocationDetail.HuId != null && productLineInProcessLocationDetail.HuId != string.Empty)
                {
                    throw new BusinessErrorException("MasterData.Return.Error.ReturnHuQtyGeRemianQty", productLineInProcessLocationDetail.HuId);
                }
                else
                {
                    throw new BusinessErrorException("MasterData.Feed.Error.NoReturnQty");
                }
            }
            #endregion

            #region 生产线退料
            productLineInProcessLocationDetail.CurrentQty = 0 - returnQty;

            productLineInProcessLocationDetail.Qty -= returnQty;
            if (productLineInProcessLocationDetail.RemainQty == 0)
            {
                productLineInProcessLocationDetail.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
            }
            productLineInProcessLocationDetail.LastModifyDate = dateTimeNow;
            productLineInProcessLocationDetail.LastModifyUser = user;

            this.UpdateProductLineInProcessLocationDetail(productLineInProcessLocationDetail);

            this.locationTransactionMgrE.RecordLocationTransaction(productLineInProcessLocationDetail, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_MATERIAL_IN, user, BusinessConstants.IO_TYPE_IN);
            #endregion

            #region 线边库位收货
            PlannedBill plannedBill = null;
            if (productLineInProcessLocationDetail.PlannedBill.HasValue)
            {
                this.plannedBillMgr.LoadPlannedBill(productLineInProcessLocationDetail.PlannedBill.Value);
            }
            this.locationMgrE.InventoryIn(productLineInProcessLocationDetail.LocationFrom, null, productLineInProcessLocationDetail.Item, productLineInProcessLocationDetail.HuId, productLineInProcessLocationDetail.LotNo, returnQty, productLineInProcessLocationDetail.IsConsignment, plannedBill, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_TR, user);
            #endregion

            //#region 更新条码数量
            //if (productLineInProcessLocationDetail.HuId != null && productLineInProcessLocationDetail.HuId != string.Empty)
            //{
            //    Hu hu = this.huMgr.CheckAndLoadHu(productLineInProcessLocationDetail.HuId);
            //    hu.Qty -= returnQty;

            //    this.huMgr.UpdateHu(hu);
            //}
            //#endregion
        }
        #endregion
    }
}


#region 扩展


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ProductLineInProcessLocationDetailMgrE : com.Sconit.Service.MasterData.Impl.ProductLineInProcessLocationDetailMgr, IProductLineInProcessLocationDetailMgrE
    {

    }
}


#endregion