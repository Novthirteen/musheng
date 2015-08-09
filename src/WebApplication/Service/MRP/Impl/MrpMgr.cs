using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Service.Ext.MRP;
using NHibernate.Expression;
using com.Sconit.Entity.MRP;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.Hql;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.Cost;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Utility;
using com.Sconit.Entity.Exception;
using System.Collections;
using NHibernate.SqlCommand;

namespace com.Sconit.Service.MRP.Impl
{
    public class MrpMgr : IMrpMgr
    {
        public IMrpRunLogMgrE mrpRunLogMgr { get; set; }
        public ICriteriaMgrE criteriaMgr { get; set; }
        public IHqlMgrE hqlMgr { get; set; }
        public IFinanceCalendarMgrE financeCalendarMgr { get; set; }
        public IMrpShipPlanMgrE mrpShipPlanMgr { get; set; }
        public IUomConversionMgrE uomConversionMgr { get; set; }
        public ICustomerScheduleDetailMgrE customerScheduleDetailMgr { get; set; }
        public IItemMgrE itemMgr { get; set; }
        public IBomMgrE bomMgr { get; set; }
        public IBomDetailMgrE bomDetailMgr { get; set; }
        public IRoutingDetailMgrE routingDetailMgr { get; set; }
        public IMrpReceivePlanMgrE mrpReceivePlanMgr { get; set; }
        public IExpectTransitInventoryMgrE expectTransitInventoryMgr { get; set; }

        public static bool isRunMrp = false;

        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.MRP");

        [Transaction(TransactionMode.Requires)]
        public void RunMrp(User user)
        {
            if (isRunMrp)
            {
                log.Info("run mrp.Occupy");
            }
            else
            {
                isRunMrp = true;
                RunMrp(DateTime.Now, user);
                isRunMrp = false;
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RunMrp(DateTime effectiveDate, User user)
        {
            DateTime dateTimeNow = DateTime.Now;
            IList<MrpShipPlan> mrpShipPlanList = new List<MrpShipPlan>();
            #region EffectiveDate格式化
            effectiveDate = effectiveDate.Date;
            #endregion

            log.Info("----------------------------------Invincible's dividing line---------------------------------------");
            log.Info("Start run mrp effectivedate:" + effectiveDate.ToLongDateString());

            #region 删除有效期相同的ShipPlan、ReceivePlan、TransitInventory
            string hql = @"from MrpShipPlan entity where entity.EffectiveDate = ?";
            hqlMgr.Delete(hql, new object[] { effectiveDate }, new IType[] { NHibernateUtil.DateTime });

            hql = @"from MrpReceivePlan entity where entity.EffectiveDate = ?";
            hqlMgr.Delete(hql, new object[] { effectiveDate }, new IType[] { NHibernateUtil.DateTime });

            hql = @"from ExpectTransitInventory entity where entity.EffectiveDate = ?";
            hqlMgr.Delete(hql, new object[] { effectiveDate }, new IType[] { NHibernateUtil.DateTime });

            this.hqlMgr.FlushSession();
            this.hqlMgr.CleanSession();
            #endregion

            #region 获取实时库存和在途
            #region 查询
            #region 订单待收

            string sql = @"select oh.OrderNo, oh.Type, oh.Flow, olt.Loc, ISNULL(im.MapItem, olt.Item) as Item, olt.Uom, od.UC, oh.StartTime, oh.WindowTime, od.OrderQty, od.ShipQty, od.RecQty, olt.UnitQty
                    from OrderLocTrans as olt 
                    inner join OrderDet as od on olt.OrderDetId = od.Id
                    inner join OrderMstr as oh on od.OrderNo = oh.OrderNo
                    left join ItemMap as im on im.Item = olt.Item
                    where oh.Status in (?, ?) and oh.SubType = ? and not oh.Type = ? and olt.IOType = ?";

            IDictionary<String, IType> columns = new Dictionary<String, IType>();
            columns.Add("OrderNo", NHibernateUtil.String);
            columns.Add("Type", NHibernateUtil.String);
            columns.Add("Flow", NHibernateUtil.String);
            columns.Add("Loc", NHibernateUtil.String);
            columns.Add("Item", NHibernateUtil.String);
            columns.Add("Uom", NHibernateUtil.String);
            columns.Add("UC", NHibernateUtil.Decimal);
            columns.Add("StartTime", NHibernateUtil.DateTime);
            columns.Add("WindowTime", NHibernateUtil.DateTime);
            columns.Add("OrderQty", NHibernateUtil.Decimal);
            columns.Add("ShipQty", NHibernateUtil.Decimal);
            columns.Add("RecQty", NHibernateUtil.Decimal);
            columns.Add("UnitQty", NHibernateUtil.Decimal);


            IList<object[]> expectTransitInvList = hqlMgr.FindAllWithNativeSql<object[]>(sql,
                new Object[] {
                    BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT, 
                    BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS, 
                    BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML, 
                    BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION,
                    BusinessConstants.IO_TYPE_IN
                }, columns);
            #endregion

            #region 安全库存
            hql = @"select fl.Code, fdl.Code, i.Code, fd.SafeStock from FlowDetail as fd 
                                        join fd.Flow as f 
                                        left join fd.LocationTo as fdl 
                                        left join f.LocationTo as fl
                                        join fd.Item as i
                                        where fd.LocationTo is not null 
                                        or f.LocationTo is not null";
            IList<object[]> safeQtyList = hqlMgr.FindAll<object[]>(hql);
            #endregion

            #region 实时库存
            sql = @"select l.Code, ISNULL(im.MapItem, lld.Item) as Item, sum(lld.Qty) as Qty
                    from LocationLotDet as lld 
                    inner join Location as l on lld.Location = l.Code
                    left join ItemMap as im on lld.Item = im.Item
                    where lld.Qty <> 0 and l.Type = ? and l.IsMRP = 1
                    group by l.Code, im.MapItem, lld.Item";

            columns = new Dictionary<String, IType>();
            columns.Add("Code", NHibernateUtil.String);
            columns.Add("Item", NHibernateUtil.String);
            columns.Add("Qty", NHibernateUtil.Decimal);

            IList<object[]> invList = hqlMgr.FindAllWithNativeSql<object[]>(
                sql, BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_NORMAL, columns);
            #endregion

            #region 发运在途

            sql = @"select od.LocTo, ISNULL(im.MapItem, olt.Item) as Item, SUM(ipd.Qty) as Qty, SUM(ipd.RecQty) as RecQty, ip.ArriveTime, om.LocTo as MLocTo
                    from IpDet as ipd inner join IpMstr as ip on ipd.IpNo = ip.IpNo
                    inner join OrderLocTrans as olt on ipd.OrderLocTransId = olt.Id
                    inner join OrderDet as od on olt.OrderDetId = od.Id
                    inner join OrderMstr as om on od.OrderNo = om.OrderNo
                    left join ItemMap as im on olt.Item = im.Item
                    where ip.Status = ? and om.SubType = ? and ip.OrderType in (?, ?)
                    group by od.LocTo, im.MapItem, olt.Item, ip.ArriveTime, om.LocTo";

            columns = new Dictionary<String, IType>();
            columns.Add("LocTo", NHibernateUtil.String);
            columns.Add("Item", NHibernateUtil.String);
            columns.Add("Qty", NHibernateUtil.Decimal);
            columns.Add("RecQty", NHibernateUtil.Decimal);
            columns.Add("ArriveTime", NHibernateUtil.DateTime);
            columns.Add("MLocTo", NHibernateUtil.String);

            IList<object[]> ipDetList = hqlMgr.FindAllWithNativeSql<object[]>(sql,
                new Object[] {
                    BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, 
                    BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML, 
                    BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING, 
                    BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER
                }, columns);
            #endregion

            #region 检验在途
            sql = @"select iod.LocTo, ISNULL(im.MapItem, lld.Item) as Item, SUM(lld.Qty) as Qty, io.EstInspectDate 
                    from InspectDet as iod 
                    inner join InspectMstr as io on iod.InspNo = io.InspNo
                    inner join LocationLotdet as lld on iod.LocLotDetId = lld.Id
                    left join ItemMap as im on lld.Item = im.Item
                    where io.IsSeperated = ? and io.Status = ?
                    group by iod.LocTo, im.MapItem, lld.Item, io.EstInspectDate";

            columns = new Dictionary<String, IType>();
            columns.Add("LocTo", NHibernateUtil.String);
            columns.Add("Item", NHibernateUtil.String);
            columns.Add("Qty", NHibernateUtil.Decimal);
            columns.Add("EstInspectDate", NHibernateUtil.DateTime);

            IList<object[]> inspLocList = hqlMgr.FindAllWithNativeSql<object[]>(sql,
                new Object[] {
                    false, 
                    BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE
                }, columns);
            #endregion
            #endregion

            #region 处理数据
            #region 获取所有库位的安全库存
            IList<SafeInventory> locationSafeQtyList = new List<SafeInventory>();
            if (safeQtyList != null && safeQtyList.Count > 0)
            {
                var unGroupSafeQtyList = from safeQty in safeQtyList
                                         select new
                                         {
                                             Location = (safeQty[1] != null ? (string)safeQty[1] : (string)safeQty[0]),
                                             Item = (string)safeQty[2],
                                             SafeQty = safeQty[3] != null ? (decimal)safeQty[3] : 0
                                         };

                var groupSafeQtyList = from g in unGroupSafeQtyList
                                       group g by new { g.Location, g.Item } into result
                                       select new SafeInventory
                                       {
                                           Location = result.Key.Location,
                                           Item = result.Key.Item,
                                           SafeQty = result.Max(g => g.SafeQty)
                                       };

                locationSafeQtyList = groupSafeQtyList != null ? groupSafeQtyList.ToList() : new List<SafeInventory>();
            }
            #endregion

            #region 获取实时库存
            IList<MrpLocationLotDetail> inventoryBalanceList = new List<MrpLocationLotDetail>();
            if (invList != null && invList.Count > 0)
            {
                IListHelper.AddRange<MrpLocationLotDetail>(inventoryBalanceList, (from inv in invList
                                                                                  select new MrpLocationLotDetail
                                                                                  {
                                                                                      Location = (string)inv[0],
                                                                                      Item = (string)inv[1],
                                                                                      Qty = (decimal)inv[2],
                                                                                      SafeQty = (from g in locationSafeQtyList
                                                                                                 where g.Location == (string)inv[0]
                                                                                                    && g.Item == (string)inv[1]
                                                                                                 select g.SafeQty).FirstOrDefault()
                                                                                  }).ToList());
            }
            #endregion

            #region 没有库存的安全库存全部转换为InventoryBalance
            if (locationSafeQtyList != null && locationSafeQtyList.Count > 0)
            {
                var eqSafeQtyList = from sq in locationSafeQtyList
                                    join inv in inventoryBalanceList on new { Location = sq.Location, Item = sq.Item } equals new { Location = inv.Location, Item = inv.Item }
                                    select sq;

                IList<SafeInventory> lackSafeQtyList = null;
                if (eqSafeQtyList != null && eqSafeQtyList.Count() > 0)
                {
                    lackSafeQtyList = locationSafeQtyList.Except(eqSafeQtyList.ToList(), new SafeInventoryComparer()).ToList();
                }
                else
                {
                    lackSafeQtyList = locationSafeQtyList;
                }

                if (lackSafeQtyList != null && lackSafeQtyList.Count > 0)
                {
                    var mlldList = from sq in lackSafeQtyList
                                   where sq.SafeQty > 0
                                   select new MrpLocationLotDetail
                                   {
                                       Location = sq.Location,
                                       Item = sq.Item,
                                       Qty = 0,
                                       SafeQty = sq.SafeQty
                                   };

                    if (mlldList != null && mlldList.Count() > 0)
                    {
                        if (inventoryBalanceList == null)
                        {
                            inventoryBalanceList = mlldList.ToList();
                        }
                        else
                        {
                            IListHelper.AddRange<MrpLocationLotDetail>(inventoryBalanceList, mlldList.ToList());
                        }
                    }
                }
            }
            #endregion

            #region 发运在途 ASN
            IList<TransitInventory> transitInventoryList = new List<TransitInventory>();

            if (ipDetList != null && ipDetList.Count > 0)
            {
                foreach (object[] ipDet in ipDetList)
                {
                    //记录在途库存
                    TransitInventory transitInventory = new TransitInventory();
                    transitInventory.Location = ipDet[0] != null ? ((Location)ipDet[0]).Code : (ipDet[5] != null ? ((Location)ipDet[5]).Code : null);
                    transitInventory.Item = (string)ipDet[1];
                    transitInventory.Qty = (decimal)ipDet[2] - (decimal)ipDet[3];
                    transitInventory.EffectiveDate = (DateTime)ipDet[4];

                    transitInventoryList.Add(transitInventory);
                }
            }
            #endregion

            #region 检验在途
            if (inspLocList != null && inspLocList.Count > 0)
            {
                foreach (object[] inspLoc in inspLocList)
                {
                    //记录在途库存
                    TransitInventory transitInventory = new TransitInventory();
                    transitInventory.Location = (string)inspLoc[0];
                    transitInventory.Item = (string)inspLoc[1];
                    transitInventory.Qty = (decimal)inspLoc[2];
                    transitInventory.EffectiveDate = (DateTime)inspLoc[3];

                    transitInventoryList.Add(transitInventory);
                    log.Debug("In-Process inspect order detail records as transit inventory. location[" + transitInventory.Location + "], item[" + transitInventory.Item + "], qty[" + transitInventory.Qty + "], effectiveDate[" + transitInventory.EffectiveDate + "]");
                }
            }
            #endregion

            #region Snapshot 订单待收
            if (expectTransitInvList != null)
            {
                //            select oh.OrderNo, oh.Type, oh.Flow, olt.Location.Code, olt.Item.Code, olt.Uom.Code, od.UnitCount, oh.StartTime, oh.WindowTime, od.OrderedQty, od.ShippedQty, od.ReceivedQty, olt.UnitQty
                //select oh.OrderNo, oh.Type, oh.Flow, olt.Loc, ISNULL(im.MapItem, olt.Item) as Item, olt.Uom, od.UC, oh.StartTime, oh.WindowTime, od.OrderQty, od.ShipQty, od.RecQty, olt.UnitQty
                                var expTransInvListSnapShot = from inv in expectTransitInvList
                                              select new ExpectTransitInventory
                                              {
                                                  OrderNo = (string)inv[0],
                                                  Flow = (string)inv[2],
                                                  Location = (string)inv[3],
                                                  Item = (string)inv[4],
                                                  Uom = (string)inv[5],
                                                  UnitCount = (decimal)inv[6],
                                                  StartTime = (DateTime)inv[7],
                                                  WindowTime = (DateTime)inv[8],
                                                  TransitQty = (string)inv[1] != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                                                               && (string)inv[1] != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT ?
                                                                                ((decimal)inv[9] - (inv[10] != null ? (decimal)inv[10] : 0) * (decimal)inv[12])
                                                                                : ((decimal)inv[9] - (inv[11] != null ? (decimal)inv[11] : 0) * (decimal)inv[12]),
                                                  EffectiveDate = effectiveDate
                                              };

                foreach (ExpectTransitInventory snapShot in expTransInvListSnapShot)
                {
                    if (snapShot.TransitQty != 0)
                    {
                        this.expectTransitInventoryMgr.CreateExpectTransitInventory(snapShot);
                    }
                }
            }
            #endregion
            #endregion
            #endregion

            #region 获取所有替代物料
            DetachedCriteria criteria = DetachedCriteria.For<ItemDiscontinue>();

            criteria.Add(Expression.Le("StartDate", effectiveDate));
            criteria.Add(Expression.Or(Expression.IsNull("EndDate"), Expression.Ge("EndDate", effectiveDate)));

            IList<ItemDiscontinue> itemDiscontinueList = this.criteriaMgr.FindAll<ItemDiscontinue>(criteria);
            #endregion

            #region 获取所有映射物料
            criteria = DetachedCriteria.For<ItemMap>();

            criteria.Add(Expression.Le("StartDate", effectiveDate));
            criteria.Add(Expression.Or(Expression.IsNull("EndDate"), Expression.Ge("EndDate", effectiveDate)));

            IList<ItemMap> itemMapList = this.criteriaMgr.FindAll<ItemMap>(criteria);
            #endregion

            #region 根据客户需求/销售订单生成发货计划

            #region 获取所有销售路线明细
            criteria = DetachedCriteria.For<Flow>();

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("Code"))
                .Add(Projections.GroupProperty("MRPOption")));

            criteria.Add(Expression.Eq("IsActive", true));
            criteria.Add(Expression.Eq("Type", BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION));

            IList<object[]> flowList = this.criteriaMgr.FindAll<object[]>(criteria);
            #endregion

            #region 获取客户需求
            criteria = DetachedCriteria.For<CustomerScheduleDetail>();
            criteria.CreateAlias("CustomerSchedule", "cs");

            criteria.Add(Expression.Eq("cs.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
            //criteria.Add(Expression.Ge("StartTime", effectiveDate));
            criteria.Add(Expression.Ge("DateFrom", effectiveDate));

            IList<CustomerScheduleDetail> customerScheduleDetailList = this.criteriaMgr.FindAll<CustomerScheduleDetail>(criteria);

            #region 取得有效的CustomerScheduleDetail
            IList<CustomerScheduleDetail> effectiveCustomerScheduleDetailList = customerScheduleDetailMgr.GetEffectiveCustomerScheduleDetail(customerScheduleDetailList, effectiveDate);
            #endregion
            #endregion

            #region 获取所有销售定单明细
            criteria = DetachedCriteria.For<OrderDetail>();
            criteria.CreateAlias("OrderHead", "od");

            criteria.Add(Expression.Eq("od.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION));
            criteria.Add(Expression.Eq("od.SubType", BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML));
            criteria.Add(Expression.In("od.Status", new string[] { BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT, BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS }));
            criteria.Add(Expression.Or(Expression.GtProperty("OrderedQty", "ShippedQty"), Expression.IsNull("ShippedQty")));
            //criteria.Add(Expression.Ge("od.StartTime", effectiveDate));
            criteria.AddOrder(Order.Asc("od.StartTime"));

            IList<OrderDetail> salesOrderDetailList = this.criteriaMgr.FindAll<OrderDetail>(criteria);
            #endregion

            #region 循环销售路线生成发货计划
            if (flowList != null && flowList.Count > 0)
            {
                foreach (object[] flow in flowList)
                {
                    string flowCode = (string)flow[0];
                    string mrpOption = (string)flow[1];

                    var targetSalesOrderDetailList = from det in salesOrderDetailList
                                                     where det.OrderHead.Flow == flowCode
                                                     select det;

                    var targetCustomerScheduleDetailList = from det in effectiveCustomerScheduleDetailList
                                                           where det.CustomerSchedule.Flow == flowCode
                                                           select det;

                    if (mrpOption == BusinessConstants.CODE_MASTER_MRP_OPTION_VALUE_ORDER_BEFORE_PLAN)
                    {
                        IListHelper.AddRange(mrpShipPlanList, TransferSalesOrderAndCustomerPlan2ShipPlan(targetSalesOrderDetailList != null ? targetSalesOrderDetailList.ToList() : null,
                            targetCustomerScheduleDetailList != null ? targetCustomerScheduleDetailList.ToList() : null,
                            effectiveDate, dateTimeNow, user, itemMapList));
                    }
                    else if (mrpOption == BusinessConstants.CODE_MASTER_MRP_OPTION_VALUE_PLAN_ONLY)
                    {
                        IListHelper.AddRange(mrpShipPlanList, TransferCustomerPlan2ShipPlan(targetCustomerScheduleDetailList != null ? targetCustomerScheduleDetailList.ToList() : null, effectiveDate, dateTimeNow, user));
                    }
                    else if (mrpOption == BusinessConstants.CODE_MASTER_MRP_OPTION_VALUE_ORDER_ONLY)
                    {
                        IListHelper.AddRange(mrpShipPlanList, TransferSalesOrder2ShipPlan(targetSalesOrderDetailList != null ? targetSalesOrderDetailList.ToList() : null, effectiveDate, dateTimeNow, user, itemMapList));
                    }
                    else
                    {
                        throw new TechnicalException("MRP option " + mrpOption + " is not valid.");
                    }
                }
            }
            #endregion
            #endregion

            #region 查询并缓存所有FlowDetail
            criteria = DetachedCriteria.For<FlowDetail>();
            criteria.CreateAlias("Flow", "f");
            criteria.CreateAlias("Item", "i");
            criteria.CreateAlias("i.Uom", "iu");
            criteria.CreateAlias("Uom", "u");
            criteria.CreateAlias("i.Location", "il", JoinType.LeftOuterJoin);
            criteria.CreateAlias("i.Bom", "ib", JoinType.LeftOuterJoin);
            criteria.CreateAlias("i.Routing", "ir", JoinType.LeftOuterJoin);
            criteria.CreateAlias("LocationFrom", "lf", JoinType.LeftOuterJoin);
            criteria.CreateAlias("LocationTo", "lt", JoinType.LeftOuterJoin);
            criteria.CreateAlias("f.LocationFrom", "flf", JoinType.LeftOuterJoin);
            criteria.CreateAlias("f.LocationTo", "flt", JoinType.LeftOuterJoin);
            criteria.CreateAlias("Bom", "b", JoinType.LeftOuterJoin);
            criteria.CreateAlias("Routing", "r", JoinType.LeftOuterJoin);
            criteria.CreateAlias("f.Routing", "fr", JoinType.LeftOuterJoin);

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("f.Code").As("Flow"))
                .Add(Projections.GroupProperty("f.Type").As("FlowType"))
                .Add(Projections.GroupProperty("i.Code").As("Item"))
                .Add(Projections.GroupProperty("lf.Code").As("LocationFrom"))
                .Add(Projections.GroupProperty("lt.Code").As("LocationTo"))
                .Add(Projections.GroupProperty("flf.Code").As("FlowLocationFrom"))
                .Add(Projections.GroupProperty("flt.Code").As("FlowLocationTo"))
                .Add(Projections.GroupProperty("MRPWeight").As("MRPWeight"))
                .Add(Projections.GroupProperty("b.Code").As("Bom"))
                .Add(Projections.GroupProperty("r.Code").As("Routing"))
                .Add(Projections.GroupProperty("fr.Code").As("FlowRouting"))
                .Add(Projections.GroupProperty("iu.Code").As("ItemUom"))
                .Add(Projections.GroupProperty("u.Code").As("Uom"))
                .Add(Projections.GroupProperty("f.LeadTime").As("LeadTime"))
                .Add(Projections.GroupProperty("ib.Code").As("ItemBom"))
                .Add(Projections.GroupProperty("ir.Code").As("ItemRouting"))
                .Add(Projections.GroupProperty("il.Code").As("ItemLocation"))
                .Add(Projections.GroupProperty("UnitCount").As("UnitCount"))
                .Add(Projections.GroupProperty("i.Desc1").As("ItemDesc1"))
                .Add(Projections.GroupProperty("i.Desc2").As("ItemDesc2"))
                .Add(Projections.GroupProperty("Id").As("Id"))
                .Add(Projections.GroupProperty("StartDate").As("StartDate"))
                .Add(Projections.GroupProperty("EndDate").As("EndDate"))
                );

            criteria.Add(Expression.Eq("f.IsActive", true));
            //criteria.Add(Expression.Not(Expression.Eq("f.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)));
            criteria.Add(Expression.Gt("MRPWeight", 0));
            //criteria.Add(Expression.Or(Expression.Le("StartDate", dateTimeNow), Expression.IsNull("StartDate")));
            //criteria.Add(Expression.Or(Expression.Ge("EndDate", dateTimeNow), Expression.IsNull("EndDate")));
            criteria.Add(Expression.Eq("f.IsMRP", true));

            IList<object[]> flowDetailList = this.criteriaMgr.FindAll<object[]>(criteria);

            var targetFlowDetailList = from fd in flowDetailList
                                       select new FlowDetailSnapShot
                                       {
                                           Flow = (string)fd[0],
                                           FlowType = (string)fd[1],
                                           Item = (string)fd[2],
                                           LocationFrom = fd[3] != null ? (string)fd[3] : fd[5] != null ? (string)fd[5] : (string)fd[16],
                                           LocationTo = fd[4] != null ? (string)fd[4] : (string)fd[6],
                                           MRPWeight = (int)fd[7],
                                           Bom = (string)fd[1] != BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION ? null : fd[8] != null ? (string)fd[8] : fd[14] != null ? (string)fd[14] : (string)fd[2],  //FlowDetail --> Item.Bom --> Item.Code
                                           Routing = (string)fd[1] != BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION ? null : fd[9] != null ? (string)fd[9] : fd[10] != null ? (string)fd[10] : fd[15] != null ? (string)fd[15] : null, //FlowDetail --> Flow --> Item.Routing
                                           BaseUom = (string)fd[11],
                                           Uom = (string)fd[12],
                                           LeadTime = fd[13] != null ? (decimal)fd[13] : 0,
                                           UnitCount = (decimal)fd[17],
                                           ItemDescription = ((fd[18] != null ? fd[18] : string.Empty) + ((fd[19] != null && fd[19] != string.Empty) ? "[" + fd[19] + "]" : string.Empty)),
                                           Id = (int)fd[20],
                                           StartDate = fd[21] == null ? DateTime.MinValue : (DateTime)fd[21],
                                           EndDate = fd[22] == null ? DateTime.MaxValue : (DateTime)fd[22]
                                       };

            IList<FlowDetailSnapShot> flowDetailSnapShotList = new List<FlowDetailSnapShot>();
            if (targetFlowDetailList != null && targetFlowDetailList.Count() > 0)
            {
                flowDetailSnapShotList = targetFlowDetailList.ToList();
            }

            #region 处理引用
            if (flowDetailSnapShotList != null && flowDetailSnapShotList.Count > 0)
            {
                criteria = DetachedCriteria.For<Flow>();

                criteria.CreateAlias("LocationFrom", "flf", JoinType.LeftOuterJoin);
                criteria.CreateAlias("LocationTo", "flt", JoinType.LeftOuterJoin);
                criteria.CreateAlias("Routing", "fr", JoinType.LeftOuterJoin);

                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.GroupProperty("Code").As("Flow"))
                    .Add(Projections.GroupProperty("Type").As("FlowType"))
                    .Add(Projections.GroupProperty("ReferenceFlow").As("ReferenceFlow"))
                    .Add(Projections.GroupProperty("flf.Code").As("FlowLocationFrom"))
                    .Add(Projections.GroupProperty("flt.Code").As("FlowLocationTo"))
                    .Add(Projections.GroupProperty("fr.Code").As("FlowRouting"))
                    );

                criteria.Add(Expression.Eq("IsActive", true));
                criteria.Add(Expression.IsNotNull("ReferenceFlow"));
                criteria.Add(Expression.Eq("IsMRP", true));
                criteria.Add(Expression.Not(Expression.Eq("Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)));

                IList<object[]> refFlowList = this.criteriaMgr.FindAll<object[]>(criteria);

                if (refFlowList != null && refFlowList.Count > 0)
                {
                    foreach (object[] refFlow in refFlowList)
                    {
                        var refFlowDetailList = from fd in flowDetailSnapShotList
                                                where string.Compare(fd.Flow, (string)refFlow[2]) == 0
                                                select fd;

                        if (refFlowDetailList != null && refFlowDetailList.Count() > 0)
                        {
                            IListHelper.AddRange(flowDetailSnapShotList, (from fd in refFlowDetailList
                                                                          select new FlowDetailSnapShot
                                                                          {
                                                                              Flow = (string)refFlow[0],
                                                                              FlowType = (string)refFlow[1],
                                                                              Item = fd.Item,
                                                                              LocationFrom = (string)refFlow[3],
                                                                              LocationTo = (string)refFlow[4],
                                                                              MRPWeight = fd.MRPWeight,
                                                                              Bom = fd.Bom,
                                                                              Routing = (string)refFlow[5],
                                                                              BaseUom = fd.BaseUom,
                                                                              Uom = fd.Uom,
                                                                              LeadTime = fd.LeadTime,
                                                                              UnitCount = fd.UnitCount,
                                                                              ItemDescription = fd.ItemDescription,
                                                                              StartDate = fd.StartDate,
                                                                              EndDate = fd.EndDate
                                                                          }).ToList());
                        }
                    }
                }
            }
            #endregion
            #endregion

            #region 补充安全库存
            if (inventoryBalanceList != null && inventoryBalanceList.Count > 0)
            {
                var lackInventoryList = from inv in inventoryBalanceList
                                        where inv.ActiveQty < 0  //可用库存小于0，要补充安全库存
                                        select inv;

                if (lackInventoryList != null && lackInventoryList.Count() > 0)
                {
                    foreach (MrpLocationLotDetail lackInventory in lackInventoryList)
                    {
                        #region 扣减在途，不考虑在途的到货时间
                        var transitConsumed = from trans in transitInventoryList
                                              where trans.Location == lackInventory.Location
                                                  && trans.Item == lackInventory.Item && trans.Qty > 0
                                              select trans;

                        if (transitConsumed != null && transitConsumed.Count() > 0)
                        {
                            foreach (TransitInventory inventory in transitConsumed)
                            {
                                if ((-lackInventory.ActiveQty) > inventory.Qty)
                                {
                                    lackInventory.Qty += inventory.Qty;
                                    inventory.Qty = 0;
                                }
                                else
                                {
                                    inventory.Qty += lackInventory.ActiveQty;
                                    lackInventory.Qty = lackInventory.SafeQty;

                                    break;
                                }
                            }
                        }

                        if (lackInventory.ActiveQty == 0)
                        {
                            //在途满足库存短缺
                            continue;
                        }
                        else
                        {
                            //在途不满足库存短缺
                            Item item = this.itemMgr.CheckAndLoadItem(lackInventory.Item);

                            MrpReceivePlan mrpReceivePlan = new MrpReceivePlan();
                            mrpReceivePlan.Item = lackInventory.Item;
                            mrpReceivePlan.Uom = item.Uom.Code;
                            mrpReceivePlan.Location = lackInventory.Location;
                            mrpReceivePlan.Qty = -lackInventory.ActiveQty;
                            mrpReceivePlan.UnitCount = item.UnitCount;
                            mrpReceivePlan.ReceiveTime = effectiveDate;
                            mrpReceivePlan.SourceType = BusinessConstants.CODE_MASTER_MRP_SOURCE_TYPE_VALUE_SAFE_STOCK;
                            mrpReceivePlan.SourceDateType = BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_DAY;
                            mrpReceivePlan.SourceId = lackInventory.Location;
                            mrpReceivePlan.SourceUnitQty = 1;
                            mrpReceivePlan.SourceItemCode = item.Code;
                            mrpReceivePlan.SourceItemDescription = item.Description1;
                            mrpReceivePlan.EffectiveDate = effectiveDate;
                            mrpReceivePlan.CreateDate = dateTimeNow;
                            mrpReceivePlan.CreateUser = user.Code;
                            mrpReceivePlan.ItemDescription = item.Description;

                            //this.mrpReceivePlanMgr.CreateMrpReceivePlan(mrpReceivePlan);

                            log.Debug("Create receive plan for safe stock, location[" + mrpReceivePlan.Location + "], item[" + mrpReceivePlan.Item + "], qty[" + mrpReceivePlan.Qty + "], sourceType[" + mrpReceivePlan.SourceType + "], sourceId[" + (mrpReceivePlan.SourceId != null ? mrpReceivePlan.SourceId : string.Empty) + "]");

                            CalculateNextShipPlan(mrpReceivePlan, inventoryBalanceList, transitInventoryList, flowDetailSnapShotList, itemDiscontinueList, effectiveDate, dateTimeNow, user);
                        }
                        #endregion
                    }
                }
            }
            #endregion

            #region 循环生成入库计划/发货计划
            if (mrpShipPlanList != null && mrpShipPlanList.Count > 0)
            {
                var sortedMrpShipPlanList = from plan in mrpShipPlanList
                                            orderby plan.StartTime ascending
                                            select plan;

                foreach (MrpShipPlan mrpShipPlan in sortedMrpShipPlanList)
                {
                    NestCalculateMrpShipPlanAndReceivePlan(mrpShipPlan, inventoryBalanceList, transitInventoryList, flowDetailSnapShotList, itemDiscontinueList, effectiveDate, dateTimeNow, user);
                }
            }
            #endregion

            #region 记录MRP Run日志
            MrpRunLog currLog = new MrpRunLog();
            currLog.RunDate = effectiveDate;
            currLog.StartTime = dateTimeNow;
            currLog.EndTime = DateTime.Now;
            currLog.CreateDate = dateTimeNow;
            currLog.CreateUser = user.Code;

            this.mrpRunLogMgr.CreateMrpRunLog(currLog);
            #endregion

            log.Info("End run mrp effectivedate:" + effectiveDate.ToLongDateString());
        }

        #region Private Methods
        private void ProcessEffectiveInventoryBalance(ref IList<MrpLocationLotDetail> inventoryBalanceList, object[] invLoc, IList<SafeInventory> safeQtyList, DateTime effectiveDate, DateTime dateTimeNow, User user)
        {
            MrpLocationLotDetail matchedInv = (from g in inventoryBalanceList
                                               where g.Location == ((string)invLoc[0])
                                                  && g.Item == ((string)invLoc[1])
                                               select g).FirstOrDefault();

            if (matchedInv != null)
            {
                matchedInv.Qty += (decimal)invLoc[2];
            }
            else
            {
                MrpLocationLotDetail locationLotDetail = new MrpLocationLotDetail();
                locationLotDetail.Location = (string)invLoc[0];
                locationLotDetail.Item = (string)invLoc[1];
                locationLotDetail.Qty = (decimal)invLoc[2];
                locationLotDetail.SafeQty = (from g in safeQtyList
                                             where g.Location == locationLotDetail.Location
                                                && g.Item == locationLotDetail.Item
                                             select g.SafeQty).FirstOrDefault();

                inventoryBalanceList.Add(locationLotDetail);
            }
        }

        private IList<MrpShipPlan> TransferSalesOrder2ShipPlan(IList<OrderDetail> salesOrderDetailList, DateTime effectiveDate, DateTime dateTimeNow, User user, IList<ItemMap> itemMapList)
        {
            IList<MrpShipPlan> mrpShipPlanList = new List<MrpShipPlan>();

            if (salesOrderDetailList != null && salesOrderDetailList.Count > 0)
            {
                foreach (OrderDetail salesOrderDetail in salesOrderDetailList)
                {
                    OrderHead orderHead = salesOrderDetail.OrderHead;
                    MrpShipPlan mrpShipPlan = new MrpShipPlan();

                    string itemCode = itemMapList != null && itemMapList.Where(i=>i.Item == salesOrderDetail.Item.Code).Count() > 0 ? 
                        itemMapList.Where(i=>i.Item == salesOrderDetail.Item.Code).First().Item : salesOrderDetail.Item.Code;

                    if (salesOrderDetail.OrderHead.StartTime < effectiveDate)
                    {
                        mrpShipPlan.IsExpire = true;
                        mrpShipPlan.ExpireStartTime = salesOrderDetail.OrderHead.StartTime;
                    }
                    else
                    {
                        mrpShipPlan.IsExpire = false;
                    }
                    mrpShipPlan.Flow = salesOrderDetail.OrderHead.Flow;
                    mrpShipPlan.FlowType = BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION;
                    mrpShipPlan.Item = itemCode;
                    mrpShipPlan.ItemDescription = salesOrderDetail.Item.Description;
                    if (mrpShipPlan.IsExpire)
                    {
                        mrpShipPlan.StartTime = DateTime.Now;
                    }
                    else
                    {
                        mrpShipPlan.StartTime = salesOrderDetail.OrderHead.StartTime;
                    }
                    if (salesOrderDetail.OrderHead.WindowTime < effectiveDate)
                    {
                        mrpShipPlan.WindowTime = DateTime.Now;
                    }
                    else
                    {
                        mrpShipPlan.WindowTime = salesOrderDetail.OrderHead.WindowTime;
                    }
                    mrpShipPlan.LocationFrom = salesOrderDetail.DefaultLocationFrom.Code;
                    mrpShipPlan.SourceType = BusinessConstants.CODE_MASTER_MRP_SOURCE_TYPE_VALUE_ORDER;
                    mrpShipPlan.SourceDateType = BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_DAY;
                    mrpShipPlan.SourceId = salesOrderDetail.OrderHead.OrderNo;
                    mrpShipPlan.SourceUnitQty = 1;
                    mrpShipPlan.SourceItemCode = itemCode;
                    mrpShipPlan.SourceItemDescription = salesOrderDetail.Item.Description1;
                    mrpShipPlan.EffectiveDate = effectiveDate;
                    mrpShipPlan.Qty = (salesOrderDetail.OrderedQty - (salesOrderDetail.ShippedQty.HasValue ? salesOrderDetail.ShippedQty.Value : 0)); ;
                    mrpShipPlan.Uom = salesOrderDetail.Uom.Code;
                    mrpShipPlan.BaseUom = salesOrderDetail.Item.Uom.Code;
                    mrpShipPlan.UnitCount = salesOrderDetail.UnitCount;
                    if (mrpShipPlan.Uom != mrpShipPlan.BaseUom)
                    {
                        mrpShipPlan.UnitQty = this.uomConversionMgr.ConvertUomQty(mrpShipPlan.Item, mrpShipPlan.Uom, 1, mrpShipPlan.BaseUom);
                    }
                    else
                    {
                        mrpShipPlan.UnitQty = 1;
                    }
                    mrpShipPlan.CreateDate = dateTimeNow;
                    mrpShipPlan.CreateUser = user.Code;

                    this.mrpShipPlanMgr.CreateMrpShipPlan(mrpShipPlan);
                    mrpShipPlanList.Add(mrpShipPlan);

                    log.Debug("Create ship plan for sales order, flow[" + mrpShipPlan.Flow + "], item[" + mrpShipPlan.Item + "], qty[" + mrpShipPlan.Qty + "], sourceType[" + mrpShipPlan.SourceType + "], sourceId[" + (mrpShipPlan.SourceId != null ? mrpShipPlan.SourceId : string.Empty) + "]");
                }
            }

            return mrpShipPlanList;
        }

        private IList<MrpShipPlan> TransferCustomerPlan2ShipPlan(IList<CustomerScheduleDetail> customerScheduleDetaillList, DateTime effectiveDate, DateTime dateTimeNow, User user)
        {
            IList<MrpShipPlan> mrpShipPlanList = new List<MrpShipPlan>();

            if (customerScheduleDetaillList != null && customerScheduleDetaillList.Count() > 0)
            {
                foreach (CustomerScheduleDetail customerScheduleDetail in customerScheduleDetaillList)
                {
                    Item item = this.itemMgr.LoadItem(customerScheduleDetail.Item);
                    MrpShipPlan mrpShipPlan = new MrpShipPlan();

                    mrpShipPlan.Flow = customerScheduleDetail.CustomerSchedule.Flow;
                    mrpShipPlan.FlowType = BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION;
                    mrpShipPlan.Item = customerScheduleDetail.Item;
                    mrpShipPlan.ItemDescription = customerScheduleDetail.ItemDescription;
                    mrpShipPlan.ItemReference = customerScheduleDetail.ItemReference;
                    mrpShipPlan.StartTime = customerScheduleDetail.StartTime;
                    mrpShipPlan.WindowTime = customerScheduleDetail.DateFrom;
                    mrpShipPlan.LocationFrom = customerScheduleDetail.Location;
                    mrpShipPlan.SourceType = BusinessConstants.CODE_MASTER_MRP_SOURCE_TYPE_VALUE_CUSTOMER_PLAN;
                    mrpShipPlan.SourceDateType = customerScheduleDetail.Type;
                    mrpShipPlan.SourceId = customerScheduleDetail.CustomerSchedule.ReferenceScheduleNo;
                    mrpShipPlan.SourceUnitQty = 1;
                    mrpShipPlan.SourceItemCode = item.Code;
                    mrpShipPlan.SourceItemDescription = item.Description1;
                    mrpShipPlan.EffectiveDate = effectiveDate;
                    mrpShipPlan.Qty = customerScheduleDetail.Qty;
                    mrpShipPlan.Uom = customerScheduleDetail.Uom;
                    mrpShipPlan.UnitCount = customerScheduleDetail.UnitCount;
                    mrpShipPlan.BaseUom = item.Uom.Code;
                    if (mrpShipPlan.Uom != mrpShipPlan.BaseUom)
                    {
                        mrpShipPlan.UnitQty = this.uomConversionMgr.ConvertUomQty(mrpShipPlan.Item, mrpShipPlan.Uom, 1, mrpShipPlan.BaseUom);
                    }
                    else
                    {
                        mrpShipPlan.UnitQty = 1;
                    }
                    mrpShipPlan.CreateDate = dateTimeNow;
                    mrpShipPlan.CreateUser = user.Code;

                    this.mrpShipPlanMgr.CreateMrpShipPlan(mrpShipPlan);
                    mrpShipPlanList.Add(mrpShipPlan);

                    log.Debug("Create ship plan for customer schedule, flow[" + mrpShipPlan.Flow + "], item[" + mrpShipPlan.Item + "], qty[" + mrpShipPlan.Qty + "], sourceType[" + mrpShipPlan.SourceType + "], sourceId[" + (mrpShipPlan.SourceId != null ? mrpShipPlan.SourceId : string.Empty) + "]");
                }
            }

            return mrpShipPlanList;
        }

        private IList<MrpShipPlan> TransferSalesOrderAndCustomerPlan2ShipPlan(IList<OrderDetail> salesOrderDetailList, IList<CustomerScheduleDetail> customerScheduleDetaillList, DateTime effectiveDate, DateTime dateTimeNow, User user, IList<ItemMap> itemMapList)
        {
            IList<MrpShipPlan> mrpShipPlanList = TransferSalesOrder2ShipPlan(salesOrderDetailList, effectiveDate, dateTimeNow, user, itemMapList);

            IList<CustomerScheduleDetail> newDetails = new List<CustomerScheduleDetail>();

            if (mrpShipPlanList != null && mrpShipPlanList.Count > 0)
            {
                if (customerScheduleDetaillList != null && customerScheduleDetaillList.Count > 0)
                {
                    #region new
                    var gDetails = from p in customerScheduleDetaillList
                                   group p by new
                                   {
                                       Flow = p.CustomerSchedule.Flow,
                                       Item = p.Item
                                   } into g
                                   select new
                                   {
                                       Flow = g.Key.Flow,
                                       Item = g.Key.Item,
                                       List = g
                                   };

                    foreach (var gDetail in gDetails)
                    {
                        string hql = @"select m.StartTime from OrderDetail as d
                                join d.OrderHead as m
                                where m.Flow = ? and d.Item = ? order by  m.StartTime desc";

                        IList<object> lastStartTime = hqlMgr.FindAll<object>(hql, new object[] { gDetail.Flow, gDetail.Item }, 0, 1);

                        DateTime maxStartTime = effectiveDate;
                        if (lastStartTime != null && lastStartTime.Count > 0)
                        {
                            maxStartTime = (DateTime)(lastStartTime.First());
                        }

                        foreach (var detail in gDetail.List)
                        {
                            if (detail.DateFrom > maxStartTime)
                            {
                                newDetails.Add(detail);
                            }
                        }
                    }
                    #endregion
                }
            }
            else
            {
                newDetails = customerScheduleDetaillList;
            }

            IListHelper.AddRange(mrpShipPlanList, TransferCustomerPlan2ShipPlan(newDetails, effectiveDate, dateTimeNow, user));

            return mrpShipPlanList;
        }

        private void NestCalculateMrpShipPlanAndReceivePlan(MrpShipPlan mrpShipPlan, IList<MrpLocationLotDetail> inventoryBalanceList, IList<TransitInventory> transitInventoryList, IList<FlowDetailSnapShot> flowDetailSnapShotList, IList<ItemDiscontinue> itemDiscontinueList, DateTime effectiveDate, DateTime dateTimeNow, User user)
        {
            //if (mrpShipPlan.IsExpire)
            //{
            //    return; //过期需求不往下传递
            //}

            if (mrpShipPlan.LocationFrom != null && mrpShipPlan.LocationFrom.Trim() != string.Empty)
            {
                #region 消耗本机物料
                if (mrpShipPlan.Qty == 0)
                {
                    return;
                }
                else if (mrpShipPlan.Qty < 0)
                {
                    throw new TechnicalException("Mrp Ship Plan Qty Can't < 0");
                }

                //回冲库存
                BackFlushInventory(mrpShipPlan, mrpShipPlan.Item, mrpShipPlan.UnitQty, inventoryBalanceList);

                //回冲在途
                BackFlushTransitInventory(mrpShipPlan, mrpShipPlan.Item, mrpShipPlan.UnitQty, transitInventoryList);
                //if (mrpShipPlan.StartTime >= effectiveDate      //只有StartTime>= EffectiveDate才能回冲
                //|| mrpShipPlan.SourceType == BusinessConstants.CODE_MASTER_MRP_SOURCE_TYPE_VALUE_SAFE_STOCK)  //或者回冲安全库存
                //{

                //}
                #endregion

                #region 消耗替代物料
                if (false && itemDiscontinueList != null && itemDiscontinueList.Count > 0 && mrpShipPlan.Qty > 0)
                {
                    var discontinuedItemList = from itemDis in itemDiscontinueList
                                               where itemDis.Item.Code == mrpShipPlan.Item
                                               orderby itemDis.Priority ascending
                                               select itemDis;

                    if (discontinuedItemList != null && discontinuedItemList.Count() > 0)
                    {
                        foreach (ItemDiscontinue itemDis in discontinuedItemList)
                        {
                            //回冲库存
                            BackFlushInventory(mrpShipPlan, itemDis.DiscontinueItem.Code, mrpShipPlan.UnitQty * itemDis.UnitQty, inventoryBalanceList);

                            //回冲在途
                            if (itemDis.EndDate >= mrpShipPlan.StartTime)
                            {
                                BackFlushTransitInventory(mrpShipPlan, itemDis.DiscontinueItem.Code, mrpShipPlan.UnitQty * itemDis.UnitQty, transitInventoryList);
                            }
                        }
                    }
                }
                #endregion

                #region 生成入库计划
                if (mrpShipPlan.Qty == 0)
                {
                    return;
                }

                IList<MrpReceivePlan> currMrpReceivePlanList = new List<MrpReceivePlan>();
                if (mrpShipPlan.FlowType != BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
                {
                    #region 非生产直接从发运计划变为入库计划
                    MrpReceivePlan mrpReceivePlan = new MrpReceivePlan();
                    mrpReceivePlan.IsExpire = mrpShipPlan.IsExpire;
                    mrpReceivePlan.ExpireStartTime = mrpShipPlan.ExpireStartTime;
                    mrpReceivePlan.Item = mrpShipPlan.Item;
                    mrpReceivePlan.ItemDescription = mrpReceivePlan.ItemDescription;
                    mrpReceivePlan.ItemReference = mrpReceivePlan.ItemReference;
                    mrpReceivePlan.Location = mrpShipPlan.LocationFrom;
                    mrpReceivePlan.Qty = mrpShipPlan.Qty * mrpShipPlan.UnitQty;  //转换为库存单位
                    mrpReceivePlan.UnitCount = mrpShipPlan.UnitCount;
                    mrpReceivePlan.Uom = mrpShipPlan.BaseUom;
                    mrpReceivePlan.ReceiveTime = mrpShipPlan.StartTime;
                    mrpReceivePlan.SourceId = mrpShipPlan.SourceId;
                    mrpReceivePlan.SourceDateType = mrpShipPlan.SourceDateType;
                    mrpReceivePlan.SourceType = mrpShipPlan.SourceType;
                    mrpReceivePlan.SourceUnitQty = mrpShipPlan.SourceUnitQty * mrpShipPlan.UnitQty;
                    mrpReceivePlan.SourceItemCode = mrpShipPlan.SourceItemCode;
                    mrpReceivePlan.SourceItemDescription = mrpShipPlan.SourceItemDescription;
                    mrpReceivePlan.EffectiveDate = effectiveDate;
                    mrpReceivePlan.CreateDate = dateTimeNow;
                    mrpReceivePlan.CreateUser = user.Code;
                    mrpReceivePlan.FlowDetailIdList = mrpShipPlan.FlowDetailIdList;

                    //this.mrpReceivePlanMgr.CreateMrpReceivePlan(mrpReceivePlan);

                    currMrpReceivePlanList.Add(mrpReceivePlan);

                    log.Debug("Transfer ship plan flow[" + mrpShipPlan.Flow + "], qty[" + mrpShipPlan.Qty + "] to receive plan location[" + mrpReceivePlan.Location + "], item[" + mrpReceivePlan.Item + "], qty[" + mrpReceivePlan.Qty + "], sourceType[" + mrpReceivePlan.SourceType + "], sourceId[" + (mrpReceivePlan.SourceId != null ? mrpReceivePlan.SourceId : string.Empty) + "]");
                    #endregion
                }
                else
                {
                    #region 生产，需要分解Bom
                    log.Debug("Production flow start resolve bom");
                    Bom bom = this.bomMgr.LoadBom(mrpShipPlan.Bom);
                    IList<BomDetail> bomDetailList = this.bomDetailMgr.GetFlatBomDetail(mrpShipPlan.Bom, mrpShipPlan.StartTime);

                    if (bom != null && bomDetailList != null && bomDetailList.Count > 0)
                    {
                        IList<RoutingDetail> routingDetailList = null;
                        if (mrpShipPlan.Routing != null && mrpShipPlan.Routing.Trim() != null)
                        {
                            routingDetailList = this.routingDetailMgr.GetRoutingDetail(mrpShipPlan.Routing, mrpShipPlan.StartTime);
                        }

                        foreach (BomDetail bomDetail in bomDetailList)
                        {
                            log.Debug("Find bomDetail FG[" + mrpShipPlan.Item + "], RM[" + bomDetail.Item.Code + "]");

                            #region 创建MrpReceivePlan
                            MrpReceivePlan mrpReceivePlan = new MrpReceivePlan();
                            mrpReceivePlan.IsExpire = mrpShipPlan.IsExpire;
                            mrpReceivePlan.ExpireStartTime = mrpShipPlan.ExpireStartTime;
                            mrpReceivePlan.Item = bomDetail.Item.Code;
                            mrpReceivePlan.UnitCount = bomDetail.Item.UnitCount;
                            mrpReceivePlan.ItemDescription = bomDetail.Item.Description;
                            #region 取库位
                            mrpReceivePlan.Location = mrpShipPlan.LocationFrom;  //默认库位
                            if (bomDetail.Location != null)
                            {
                                mrpReceivePlan.Location = bomDetail.Location.Code;
                            }
                            else
                            {
                                if (routingDetailList != null)
                                {
                                    Location location = (from det in routingDetailList
                                                         where det.Operation == bomDetail.Operation
                                                         && det.Reference == bomDetail.Reference
                                                         select det.Location).FirstOrDefault();

                                    if (location != null)
                                    {
                                        mrpReceivePlan.Location = location.Code;
                                    }
                                }
                            }
                            #endregion
                            decimal fgQty = mrpShipPlan.Qty;
                            decimal fgSourceUnitQty = mrpShipPlan.SourceUnitQty;
                            if (mrpShipPlan.Uom != bom.Uom.Code)
                            {
                                //成品数量转换为Bom单位
                                fgQty = this.uomConversionMgr.ConvertUomQty(mrpShipPlan.Item, mrpShipPlan.Uom, fgQty, bom.Uom.Code);
                                fgSourceUnitQty = this.uomConversionMgr.ConvertUomQty(mrpShipPlan.Item, mrpShipPlan.Uom, fgSourceUnitQty, bom.Uom.Code);
                            }
                            mrpReceivePlan.Uom = bomDetail.Item.Uom.Code;
                            #region 计算用量
                            //BomDetail上的单位
                            mrpReceivePlan.Qty = fgQty //成品用量                                  
                                * bomDetail.RateQty //乘以单位用量
                                * (1 + bomDetail.DefaultScrapPercentage);  //乘以损耗
                            mrpReceivePlan.SourceUnitQty = fgSourceUnitQty
                                * bomDetail.RateQty //乘以单位用量
                                * (1 + bomDetail.DefaultScrapPercentage);  //乘以损耗
                            if (mrpReceivePlan.Uom != bomDetail.Uom.Code)
                            {
                                //转换为库存单位
                                mrpReceivePlan.Qty = this.uomConversionMgr.ConvertUomQty(mrpReceivePlan.Item, bomDetail.Uom.Code, mrpReceivePlan.Qty, mrpReceivePlan.Uom);
                                mrpReceivePlan.SourceUnitQty = this.uomConversionMgr.ConvertUomQty(mrpReceivePlan.Item, bomDetail.Uom.Code, mrpReceivePlan.SourceUnitQty, mrpReceivePlan.Uom);
                            }

                            #region 消耗本级物料
                            #region 扣减线边库位库存
                            BackFlushInventory(mrpReceivePlan, mrpReceivePlan.Item, 1, inventoryBalanceList);
                            #endregion

                            #region 扣减线边在途库存
                            BackFlushTransitInventory(mrpReceivePlan, mrpReceivePlan.Item, 1, transitInventoryList);
                            #endregion
                            #endregion

                            #region 消耗替代物料
                            if (false && itemDiscontinueList != null && itemDiscontinueList.Count > 0 && mrpReceivePlan.Qty > 0)
                            {
                                var discontinuedItemList = from itemDis in itemDiscontinueList
                                                           where itemDis.Item.Code == mrpReceivePlan.Item
                                                           orderby itemDis.Priority ascending
                                                           select itemDis;

                                if (discontinuedItemList != null && discontinuedItemList.Count() > 0)
                                {
                                    foreach (ItemDiscontinue itemDis in discontinuedItemList)
                                    {
                                        //回冲库存
                                        BackFlushInventory(mrpReceivePlan, itemDis.DiscontinueItem.Code, itemDis.UnitQty, inventoryBalanceList);

                                        //回冲在途
                                        if (itemDis.EndDate >= mrpReceivePlan.ReceiveTime)
                                        {
                                            BackFlushTransitInventory(mrpReceivePlan, itemDis.DiscontinueItem.Code, itemDis.UnitQty, transitInventoryList);
                                        }
                                    }
                                }
                            }
                            #endregion
                            #endregion

                            mrpReceivePlan.ReceiveTime = mrpShipPlan.StartTime;
                            mrpReceivePlan.SourceId = mrpShipPlan.SourceId;
                            mrpReceivePlan.SourceDateType = mrpShipPlan.SourceDateType;
                            mrpReceivePlan.SourceType = mrpShipPlan.SourceType;
                            mrpReceivePlan.SourceItemCode = mrpShipPlan.SourceItemCode;
                            mrpReceivePlan.SourceItemDescription = mrpShipPlan.SourceItemDescription;
                            mrpReceivePlan.EffectiveDate = effectiveDate;
                            mrpReceivePlan.CreateDate = dateTimeNow;
                            mrpReceivePlan.CreateUser = user.Code;
                            mrpReceivePlan.FlowDetailIdList = mrpShipPlan.FlowDetailIdList;

                            //this.mrpReceivePlanMgr.CreateMrpReceivePlan(mrpReceivePlan);
                            currMrpReceivePlanList.Add(mrpReceivePlan);
                            #endregion
                        }
                    }
                    else
                    {
                        log.Error("Can't find bom detial for code " + mrpShipPlan.Bom);
                    }
                    log.Debug("Production flow end resolve bom");
                    #endregion
                }
                #endregion

                #region 计算下游发运计划
                foreach (MrpReceivePlan mrpReceivePlan in currMrpReceivePlanList)
                {
                    log.Debug("Transfer ship plan flow[" + mrpShipPlan.Flow + "], qty[" + mrpShipPlan.Qty + "] to receive plan location[" + mrpReceivePlan.Location + "], item[" + mrpReceivePlan.Item + "], qty[" + mrpReceivePlan.Qty + "], sourceType[" + mrpReceivePlan.SourceType + "], sourceId[" + (mrpReceivePlan.SourceId != null ? mrpReceivePlan.SourceId : string.Empty) + "]");
                    CalculateNextShipPlan(mrpReceivePlan, inventoryBalanceList, transitInventoryList, flowDetailSnapShotList, itemDiscontinueList, effectiveDate, dateTimeNow, user);
                }
                #endregion
            }
        }

        private void CalculateNextShipPlan(MrpReceivePlan mrpReceivePlan, IList<MrpLocationLotDetail> inventoryBalanceList, IList<TransitInventory> transitInventoryList, IList<FlowDetailSnapShot> flowDetailSnapShotList, IList<ItemDiscontinue> itemDiscontinueList, DateTime effectiveDate, DateTime dateTimeNow, User user)
        {
            if (mrpReceivePlan.ReceiveTime < effectiveDate)
            {
                //如果窗口时间小于effectivedate，不往下计算
                //return;
            }

            var nextFlowDetailList = from det in flowDetailSnapShotList
                                     where det.LocationTo == mrpReceivePlan.Location
                                    && det.Item == mrpReceivePlan.Item
                                    && det.StartDate <= mrpReceivePlan.ReceiveTime && det.EndDate >= mrpReceivePlan.ReceiveTime
                                     select det;

            //#region 如果有多条下游路线，根据Item.DefaultFlow过滤
            //if (nextFlowDetailList != null && nextFlowDetailList.Count() > 1)
            //{
            //    Item item = this.itemMgr.LoadItem(mrpReceivePlan.Item);

            //    if (item.DefaultFlow != null && item.DefaultFlow.Trim() != string.Empty)
            //    {
            //        var defaultFlow = from det in nextFlowDetailList
            //                          where det.Flow == item.DefaultFlow
            //                          select det;

            //        if (defaultFlow != null && defaultFlow.Count() > 0)
            //        {
            //            nextFlowDetailList = defaultFlow;
            //        }
            //    }
            //}
            //#endregion

            if (nextFlowDetailList != null && nextFlowDetailList.Count() > 0)
            {
                int mrpWeight = nextFlowDetailList.Sum(p => p.MRPWeight);
                decimal rate = mrpReceivePlan.Qty / mrpWeight;
                decimal remainQty = mrpReceivePlan.Qty;

                for (int i = 0; i < nextFlowDetailList.Count(); i++)
                {
                    FlowDetailSnapShot flowDetail = nextFlowDetailList.ElementAt(i);

                    MrpShipPlan mrpShipPlan = new MrpShipPlan();

                    if (mrpReceivePlan.ContainFlowDetailId(flowDetail.Id))
                    {
                        log.Error("Cycle Flow Detail Find when transfer receive plan location[" + mrpReceivePlan.Location + "], item[" + mrpReceivePlan.Item + "], qty[" + mrpReceivePlan.Qty + "], sourceType[" + mrpReceivePlan.SourceType + "], sourceId[" + (mrpReceivePlan.SourceId != null ? mrpReceivePlan.SourceId : string.Empty) + "] to ship plan flow[" + flowDetail.Flow + "]");
                        //continue;
                    }
                    else
                    {
                        mrpShipPlan.FlowDetailIdList = mrpReceivePlan.FlowDetailIdList;
                        mrpShipPlan.AddFlowDetailId(flowDetail.Id);
                    }

                    mrpShipPlan.Flow = flowDetail.Flow;
                    mrpShipPlan.FlowType = flowDetail.FlowType;
                    mrpShipPlan.Item = flowDetail.Item;
                    mrpShipPlan.ItemDescription = flowDetail.ItemDescription;
                    if (mrpReceivePlan.SourceDateType != BusinessConstants.CODE_MASTER_MRP_SOURCE_TYPE_VALUE_SAFE_STOCK)
                    {
                        mrpShipPlan.StartTime = mrpReceivePlan.ReceiveTime.AddHours(-Convert.ToDouble(flowDetail.LeadTime));
                    }
                    else
                    {
                        mrpShipPlan.StartTime = mrpReceivePlan.ReceiveTime;
                    }
                    if (mrpShipPlan.StartTime < effectiveDate)
                    {
                        mrpShipPlan.IsExpire = true;
                        mrpShipPlan.ExpireStartTime = mrpShipPlan.StartTime;
                        mrpShipPlan.StartTime = dateTimeNow;
                    }
                    else
                    {
                        mrpShipPlan.IsExpire = false;
                    }
                    mrpShipPlan.WindowTime = mrpReceivePlan.ReceiveTime;
                    mrpShipPlan.LocationFrom = flowDetail.LocationFrom;
                    mrpShipPlan.LocationTo = flowDetail.LocationTo;
                    mrpShipPlan.SourceType = mrpReceivePlan.SourceType;
                    mrpShipPlan.SourceDateType = mrpReceivePlan.SourceDateType;
                    mrpShipPlan.SourceId = mrpReceivePlan.SourceId;
                    mrpShipPlan.SourceItemCode = mrpReceivePlan.SourceItemCode;
                    mrpShipPlan.SourceItemDescription = mrpReceivePlan.SourceItemDescription;
                    mrpShipPlan.EffectiveDate = effectiveDate;
                    mrpShipPlan.Uom = flowDetail.Uom;
                    mrpShipPlan.BaseUom = flowDetail.BaseUom;
                    if (mrpShipPlan.Uom != mrpShipPlan.BaseUom)
                    {
                        mrpShipPlan.UnitQty = this.uomConversionMgr.ConvertUomQty(mrpShipPlan.Item, mrpShipPlan.Uom, 1, mrpShipPlan.BaseUom);
                    }
                    else
                    {
                        mrpShipPlan.UnitQty = 1;
                    }
                    if (i != nextFlowDetailList.Count() - 1)
                    {
                        remainQty -= rate * flowDetail.MRPWeight;
                        mrpShipPlan.Qty = rate * flowDetail.MRPWeight / mrpShipPlan.UnitQty;   //转换为定单单位                        
                    }
                    else
                    {
                        mrpShipPlan.Qty = remainQty / mrpShipPlan.UnitQty;   //转换为定单单位
                    }
                    mrpShipPlan.SourceUnitQty = mrpReceivePlan.SourceUnitQty / mrpWeight * flowDetail.MRPWeight / mrpShipPlan.UnitQty;
                    mrpShipPlan.UnitCount = flowDetail.UnitCount;
                    mrpShipPlan.Bom = flowDetail.Bom;
                    mrpShipPlan.Routing = flowDetail.Routing;
                    //mrpShipPlan.IsExpire = mrpReceivePlan.IsExpire;
                    //mrpShipPlan.ExpireStartTime = mrpReceivePlan.ExpireStartTime;
                    mrpShipPlan.CreateDate = dateTimeNow;
                    mrpShipPlan.CreateUser = user.Code;

                    this.mrpShipPlanMgr.CreateMrpShipPlan(mrpShipPlan);

                    log.Debug("Transfer receive plan location[" + mrpReceivePlan.Location + "], item[" + mrpReceivePlan.Item + "], qty[" + mrpReceivePlan.Qty + "], sourceType[" + mrpReceivePlan.SourceType + "], sourceId[" + (mrpReceivePlan.SourceId != null ? mrpReceivePlan.SourceId : string.Empty) + "] to ship plan flow[" + mrpShipPlan.Flow + "], qty[" + mrpShipPlan.Qty + "]");

                    NestCalculateMrpShipPlanAndReceivePlan(mrpShipPlan, inventoryBalanceList, transitInventoryList, flowDetailSnapShotList, itemDiscontinueList, effectiveDate, dateTimeNow, user);
                }
            }
            else
            {
                log.Warn("Can't find next flow for location[" + mrpReceivePlan.Location + "], item[" + mrpReceivePlan.Item + "]");
            }
        }

        private void BackFlushInventory(MrpShipPlan mrpShipPlan, string itemCode, decimal unitQty, IList<MrpLocationLotDetail> inventoryBalanceList)
        {
            #region 先消耗库存
            if (mrpShipPlan.Qty == 0)
            {
                return;
            }

            var inventoryConsumed = from inv in inventoryBalanceList
                                    where inv.Location == mrpShipPlan.LocationFrom
                                    && inv.Item == itemCode && inv.Qty > inv.SafeQty
                                    select inv;

            if (inventoryConsumed != null && inventoryConsumed.Count() > 0)
            {
                foreach (MrpLocationLotDetail inventory in inventoryConsumed)
                {
                    if (mrpShipPlan.Qty * unitQty > inventory.ActiveQty)
                    {
                        log.Debug("Backflush inventory for mrpShipPlan flow[" + mrpShipPlan.Flow + "], item[" + itemCode + "], qty[" + mrpShipPlan.Qty + "], sourceType[" + mrpShipPlan.SourceType + "], sourceId[" + (mrpShipPlan.SourceId != null ? mrpShipPlan.SourceId : string.Empty) + "], backflushQty[" + inventory.ActiveQty / unitQty + "]");
                        mrpShipPlan.Qty -= inventory.ActiveQty / unitQty;
                        inventory.Qty = inventory.SafeQty;
                    }
                    else
                    {
                        log.Debug("Backflush inventory for mrpShipPlan flow[" + mrpShipPlan.Flow + "], item[" + itemCode + "], qty[" + mrpShipPlan.Qty + "], sourceType[" + mrpShipPlan.SourceType + "], sourceId[" + (mrpShipPlan.SourceId != null ? mrpShipPlan.SourceId : string.Empty) + "], backflushQty[" + mrpShipPlan.Qty * unitQty + "]");
                        inventory.Qty -= mrpShipPlan.Qty * unitQty;
                        mrpShipPlan.Qty = 0;

                        break;
                    }
                }
            }
            #endregion
        }

        private void BackFlushTransitInventory(MrpShipPlan mrpShipPlan, string itemCode, decimal unitQty, IList<TransitInventory> transitInventoryList)
        {
            #region 再根据ShipPlan的StartTime < 在途库存的EffectiveDate消耗在途库存
            if (mrpShipPlan.Qty == 0)
            {
                return;
            }

            var transitConsumed = from trans in transitInventoryList
                                  where trans.Location == mrpShipPlan.LocationFrom
                                      && trans.Item == itemCode && trans.Qty > 0
                                      && trans.EffectiveDate <= mrpShipPlan.StartTime
                                  select trans;

            if (transitConsumed != null && transitConsumed.Count() > 0)
            {
                foreach (TransitInventory inventory in transitConsumed)
                {
                    if (mrpShipPlan.Qty * unitQty > inventory.Qty)
                    {
                        log.Debug("Backflush transit inventory for mrpShipPlan flow[" + mrpShipPlan.Flow + "], item[" + itemCode + "], qty[" + mrpShipPlan.Qty + "], sourceType[" + mrpShipPlan.SourceType + "], sourceId[" + (mrpShipPlan.SourceId != null ? mrpShipPlan.SourceId : string.Empty) + "], effectiveDate[" + inventory.EffectiveDate + "], backflushQty[" + inventory.Qty / unitQty + "]");
                        mrpShipPlan.Qty -= inventory.Qty / unitQty;
                        inventory.Qty = 0;
                    }
                    else
                    {
                        log.Debug("Backflush transit inventory for mrpShipPlan flow[" + mrpShipPlan.Flow + "], item[" + itemCode + "], qty[" + mrpShipPlan.Qty + "], sourceType[" + mrpShipPlan.SourceType + "], sourceId[" + (mrpShipPlan.SourceId != null ? mrpShipPlan.SourceId : string.Empty) + "], effectiveDate[" + inventory.EffectiveDate + "], backflushQty[" + mrpShipPlan.Qty * unitQty + "]");
                        inventory.Qty -= mrpShipPlan.Qty * unitQty;
                        mrpShipPlan.Qty = 0;

                        break;
                    }
                }
            }
            #endregion
        }

        private void BackFlushInventory(MrpReceivePlan mrpReceivePlan, string itemCode, decimal unitQty, IList<MrpLocationLotDetail> inventoryBalanceList)
        {
            #region 先消耗库存
            if (mrpReceivePlan.Qty == 0)
            {
                return;
            }

            var wipInventoryConsumed = from inv in inventoryBalanceList
                                       where inv.Location == mrpReceivePlan.Location
                                       && inv.Item == mrpReceivePlan.Item && inv.Qty > inv.SafeQty
                                       select inv;

            if (wipInventoryConsumed != null && wipInventoryConsumed.Count() > 0)
            {
                foreach (MrpLocationLotDetail inventory in wipInventoryConsumed)
                {
                    if (mrpReceivePlan.Qty * unitQty > inventory.ActiveQty)
                    {
                        log.Debug("Backflush inventory for mrpReceivePlan location[" + mrpReceivePlan.Location + "], item[" + itemCode + "], qty[" + mrpReceivePlan.Qty + "], sourceType[" + mrpReceivePlan.SourceType + "], sourceId[" + (mrpReceivePlan.SourceId != null ? mrpReceivePlan.SourceId : string.Empty) + "], backflushQty[" + inventory.ActiveQty / unitQty + "]");
                        mrpReceivePlan.Qty -= inventory.ActiveQty / unitQty;
                        inventory.Qty = inventory.SafeQty;
                    }
                    else
                    {
                        log.Debug("Backflush inventory for mrpReceivePlan location[" + mrpReceivePlan.Location + "], item[" + itemCode + "], qty[" + mrpReceivePlan.Qty + "], sourceType[" + mrpReceivePlan.SourceType + "], sourceId[" + (mrpReceivePlan.SourceId != null ? mrpReceivePlan.SourceId : string.Empty) + "], backflushQty[" + mrpReceivePlan.Qty * unitQty + "]");

                        inventory.Qty -= mrpReceivePlan.Qty * unitQty;
                        mrpReceivePlan.Qty = 0;

                        break;
                    }
                }
            }
            #endregion
        }

        private void BackFlushTransitInventory(MrpReceivePlan mrpReceivePlan, string itemCode, decimal unitQty, IList<TransitInventory> transitInventoryList)
        {
            #region 再根据ShipPlan的StartTime < 在途库存的EffectiveDate消耗在途库存
            if (mrpReceivePlan.Qty == 0)
            {
                return;
            }

            var transitConsumed = from trans in transitInventoryList
                                  where trans.Location == mrpReceivePlan.Location
                                      && trans.Item == itemCode && trans.Qty > 0
                                      && trans.EffectiveDate <= mrpReceivePlan.ReceiveTime
                                  select trans;

            if (transitConsumed != null && transitConsumed.Count() > 0)
            {
                foreach (TransitInventory inventory in transitConsumed)
                {
                    if (mrpReceivePlan.Qty * unitQty > inventory.Qty)
                    {
                        log.Debug("Backflush transit inventory for mrpReceivePlan location[" + mrpReceivePlan.Location + "], item[" + itemCode + "], qty[" + mrpReceivePlan.Qty + "], sourceType[" + mrpReceivePlan.SourceType + "], sourceId[" + (mrpReceivePlan.SourceId != null ? mrpReceivePlan.SourceId : string.Empty) + "], effectiveDate[" + inventory.EffectiveDate + "], backflushQty[" + inventory.Qty / unitQty + "]");
                        mrpReceivePlan.Qty -= inventory.Qty / unitQty;
                        inventory.Qty = 0;
                    }
                    else
                    {
                        log.Debug("Backflush transit inventory for mrpReceivePlan location[" + mrpReceivePlan.Location + "], item[" + itemCode + "], qty[" + mrpReceivePlan.Qty + "], sourceType[" + mrpReceivePlan.SourceType + "], sourceId[" + (mrpReceivePlan.SourceId != null ? mrpReceivePlan.SourceId : string.Empty) + "], effectiveDate[" + inventory.EffectiveDate + "], backflushQty[" + mrpReceivePlan.Qty * unitQty + "]");
                        inventory.Qty -= mrpReceivePlan.Qty * unitQty;
                        mrpReceivePlan.Qty = 0;

                        break;
                    }
                }
            }
            #endregion
        }
        #endregion
    }

    class SafeInventory
    {
        public string Location { get; set; }
        public string Item { get; set; }
        public decimal SafeQty { get; set; }
    }

    class TransitInventory
    {
        public string Location { get; set; }
        public string Item { get; set; }
        public decimal Qty { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    class FlowDetailSnapShot
    {
        public string Flow { get; set; }
        public string FlowType { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string Item { get; set; }
        public int MRPWeight { get; set; }
        public string Bom { get; set; }
        public string Routing { get; set; }
        public string BaseUom { get; set; }
        public string Uom { get; set; }
        public decimal LeadTime { get; set; }
        public decimal UnitCount { get; set; }
        public string ItemDescription { get; set; }
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    class SafeInventoryComparer : IEqualityComparer<SafeInventory>
    {
        public bool Equals(SafeInventory x, SafeInventory y)
        {
            return x.Location == y.Location && x.Item == y.Item;
        }

        public int GetHashCode(SafeInventory obj)
        {
            string hCode = obj.Location + "|" + obj.Item;
            return hCode.GetHashCode();
        }
    }

    class LastActionQty
    {
        public string Flow { get; set; }
        public string Item { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitQty { get; set; }
    }

    //class MrpReceivePlan
    //{
    //    private string _location;
    //    public string Location
    //    {
    //        get
    //        {
    //            return _location;
    //        }
    //        set
    //        {
    //            _location = value;
    //        }
    //    }
    //    private string _item;
    //    public string Item
    //    {
    //        get
    //        {
    //            return _item;
    //        }
    //        set
    //        {
    //            _item = value;
    //        }
    //    }
    //    private Decimal _qty;
    //    public Decimal Qty
    //    {
    //        get
    //        {
    //            return _qty;
    //        }
    //        set
    //        {
    //            _qty = value;
    //        }
    //    }
    //    private Decimal _unitCount;
    //    public Decimal UnitCount
    //    {
    //        get
    //        {
    //            return _unitCount;
    //        }
    //        set
    //        {
    //            _unitCount = value;
    //        }
    //    }
    //    private DateTime _receiveTime;
    //    public DateTime ReceiveTime
    //    {
    //        get
    //        {
    //            return _receiveTime;
    //        }
    //        set
    //        {
    //            _receiveTime = value;
    //        }
    //    }
    //    private string _sourceDateType;
    //    public string SourceDateType
    //    {
    //        get
    //        {
    //            return _sourceDateType;
    //        }
    //        set
    //        {
    //            _sourceDateType = value;
    //        }
    //    }
    //    private string _sourceType;
    //    public string SourceType
    //    {
    //        get
    //        {
    //            return _sourceType;
    //        }
    //        set
    //        {
    //            _sourceType = value;
    //        }
    //    }
    //    private string _sourceId;
    //    public string SourceId
    //    {
    //        get
    //        {
    //            return _sourceId;
    //        }
    //        set
    //        {
    //            _sourceId = value;
    //        }
    //    }
    //    private Boolean _isExpire;
    //    public Boolean IsExpire
    //    {
    //        get
    //        {
    //            return _isExpire;
    //        }
    //        set
    //        {
    //            _isExpire = value;
    //        }
    //    }
    //    private string _uom;
    //    public string Uom
    //    {
    //        get
    //        {
    //            return _uom;
    //        }
    //        set
    //        {
    //            _uom = value;
    //        }
    //    }
    //    private DateTime? _expireStartTime;
    //    public DateTime? ExpireStartTime
    //    {
    //        get
    //        {
    //            return _expireStartTime;
    //        }
    //        set
    //        {
    //            _expireStartTime = value;
    //        }
    //    }
    //    public string ItemDescription { get; set; }
    //    public string ItemReference { get; set; }
    //}

    class MrpLocationLotDetail
    {
        private string _location;
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        private string _item;
        public string Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }
        private Decimal _safeQty;
        public Decimal SafeQty
        {
            get
            {
                return _safeQty;
            }
            set
            {
                _safeQty = value;
            }
        }
        private Decimal _qty;
        public Decimal Qty
        {
            get
            {
                return _qty;
            }
            set
            {
                _qty = value;
            }
        }

        public decimal ActiveQty
        {
            get
            {
                return Qty - SafeQty;
            }
        }
    }

}

#region Extend Class

namespace com.Sconit.Service.Ext.MRP.Impl
{
    [Transactional]
    public partial class MrpMgrE : com.Sconit.Service.MRP.Impl.MrpMgr, IMrpMgrE
    {
    }
}

#endregion Extend Class