using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Entity.Cost;
using com.Sconit.Entity.Distribution;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.Cost.Impl;
using com.Sconit.Service.Ext.MasterData.Impl;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using System.Collections;
using com.Sconit.Service.Ext.Hql;
using NHibernate.Transform;

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class CostMgr : SessionBase, ICostMgr
    {
        public ICostCenterMgrE costCenterMgr { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgr { get; set; }
        public ICostElementMgrE costElementMgr { get; set; }
        public ICurrencyMgrE currencyMgr { get; set; }
        public IReceiptInProcessLocationMgrE receiptInProcessLocationMgr { get; set; }
        public IStandardCostMgrE standardCostMgr { get; set; }
        public ICostTransactionMgrE costTransacationMgr { get; set; }
        public ICriteriaMgrE criteriaMgr { get; set; }
        public IOrderHeadMgrE orderHeadMgr { get; set; }
        public IFinanceCalendarMgrE financeCalendarMgr { get; set; }
        public ICurrencyExchangeMgrE currencyExchangeMgr { get; set; }
        public ITaxRateMgrE taxRateMgr { get; set; }
        public ICostAllocateTransactionMgrE costAllocateTransactionMgr { get; set; }
        public ICostBalanceMgrE costBalanceMgr { get; set; }
        public ICostDetailMgrE costDetailMgr { get; set; }
        public IInventoryBalanceMgrE inventoryBalanceMgr { get; set; }
        public IItemMgrE itemMgr { get; set; }
        public ICostGroupMgrE costGroupMgr { get; set; }
        public ICostInventoryBalanceMgrE costInventoryBalanceMgr { get; set; }

        #region old Cost Method
        private string[] CostDetailCloneFields = new string[] 
            { 
                "Item",
                "ItemCategory",
                "CostGroup",
                "CostElement",
                "Cost"
            };

        private string[] CostTransactionCloneFields = new string[] 
            { 
                "Item",
                "ItemCategory",
                "OrderNo",
                "ReceiptNo",
                "CostGroup",
                "CostCenter",
                "CostElement",
                "Currency",
                "BaseCurrency",
                "ExchangeRate",
                "ReferenceItem",
                "ReferenceQty"
            };

        [Transaction(TransactionMode.Requires)]
        public void RecordProductionCostTransaction(Receipt receipt, User user)
        {
            if (receipt.ReceiptDetails == null || receipt.ReceiptDetails.Count == 0)
            {
                throw new BusinessErrorException("Cost.Transaction.Error.ReceiptEmpty");
            }

            if (receipt.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                throw new BusinessErrorException("Cost.Transaction.Error.ReceiptTypeWrong");
            }

            #region 从PartyTo取成本中心
            string costCenterCode = ((Region)receipt.PartyTo).CostCenter;
            if (costCenterCode == null || costCenterCode == string.Empty)
            {
                throw new BusinessErrorException("Cost.Transaction.Error.NotCostCenterFindForRegion", receipt.PartyTo.Code);
            }
            CostCenter regionCC = costCenterMgr.CheckAndLoadCostCenter(costCenterCode);
            #endregion

            #region 取企业币种
            EntityPreference baseCurrencyPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_BASE_CURRENCY);
            Currency baseCurrency = currencyMgr.CheckAndLoadCurrency(baseCurrencyPreference.Value);
            #endregion

            #region 取小数位数
            EntityPreference decimalLengthPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
            int decimalLength = int.Parse(decimalLengthPreference.Value);
            #endregion

            #region 取当前时间
            DateTime dateTimeNow = DateTime.Now;
            #endregion

            #region 记录材料成本事务

            #region 查找直接物料成本要素代码
            EntityPreference materialPerference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_ELEMENT_MATERIAL);
            CostElement materialCostElement = costElementMgr.LoadCostElement(materialPerference.Value);
            #endregion

            #region 取得所有原材料
            IList<InProcessLocationDetail> inProcessLocationDetailList = new List<InProcessLocationDetail>();
            IList<ReceiptInProcessLocation> receiptInProcessLocationList = receiptInProcessLocationMgr.GetReceiptInProcessLocation(null, receipt.ReceiptNo);
            var ipDetResult = from receiptInProcessLocation in receiptInProcessLocationList
                              select receiptInProcessLocation.InProcessLocation.InProcessLocationDetails;

            foreach (IList<InProcessLocationDetail> list in ipDetResult)
            {
                IListHelper.AddRange<InProcessLocationDetail>(inProcessLocationDetailList, list);
            }
            #endregion

            //if (receipt.ReceiptDetails.Count == 1)
            //{
            //    var p = from ipDet in inProcessLocationDetailList
            //            select new
            //            {
            //                ReceiptDetail = receipt.ReceiptDetails[0],
            //                InProcessLocationDetail = ipDet
            //            };
            //}
            //else
            //{

            #region 计算产品针对每个物料的消耗因子（产品收货数量 * 单个产品的物料消耗率 => ReceivedQty * UnitQty）
            var recOrderTrans = from recDet in receipt.ReceiptDetails
                                select recDet.OrderLocationTransaction.OrderDetail.OrderLocationTransactions;

            IList<OrderLocationTransaction> orderLocationTransactionList = new List<OrderLocationTransaction>();
            foreach (IList<OrderLocationTransaction> list in recOrderTrans)
            {
                IListHelper.AddRange(orderLocationTransactionList, list);
            }

            var recDetOrderTrans = from orderLocationTransaction in orderLocationTransactionList
                                   join recDet in receipt.ReceiptDetails on orderLocationTransaction.OrderDetail.Id equals recDet.OrderLocationTransaction.OrderDetail.Id
                                   where orderLocationTransaction.IOType == BusinessConstants.IO_TYPE_OUT
                                   select new { recDet, orderLocationTransaction };

            var consumeFact = from v in recDetOrderTrans
                              group v by new { Order = v.recDet.OrderLocationTransaction.OrderDetail.OrderHead, ReceiptDet = v.recDet, Material = v.orderLocationTransaction.Item } into PM
                              select new
                              {
                                  Order = PM.Key.Order,
                                  ReceiptDet = PM.Key.ReceiptDet,
                                  Material = PM.Key.Material,
                                  ConsumeFact = PM.Sum(t => (t.recDet.ReceivedQty
                                      + t.recDet.RejectedQty
                                      + t.recDet.ScrapQty) * t.orderLocationTransaction.UnitQty)  //消耗因子需要考虑次品和废品
                              };
            #endregion

            #region 计算消耗比率
            var itemConsume = from ipDet in inProcessLocationDetailList
                              group ipDet by ipDet.OrderLocationTransaction.Item into i
                              select new
                              {
                                  Material = i.Key,
                                  ConsumedQty = i.Sum(ipDet => ipDet.Qty * ipDet.OrderLocationTransaction.UnitQty)
                              };

            //分母
            var consumeDenominator = from fact in consumeFact
                                     group fact by fact.Material into f
                                     select new
                                     {
                                         Material = f.Key,
                                         ConsumeDenominator = f.Sum(fact => fact.ConsumeFact)
                                     };

            var itemConsumeRate = from consume in itemConsume
                                  join denominator in consumeDenominator on consume.Material equals denominator.Material
                                  select new
                                  {
                                      Material = consume.Material,
                                      ConsumeRate = consume.ConsumedQty / denominator.ConsumeDenominator
                                  };
            #endregion

            #region 计算消耗量
            //第一次计算消耗量，可能有差异
            var consumeQty = from fact in consumeFact
                             join rate in itemConsumeRate on fact.Material.Code equals rate.Material.Code
                             select new
                             {
                                 Order = fact.Order,
                                 ReceiptDet = fact.ReceiptDet,
                                 Material = fact.Material,
                                 ConsumeQty = Math.Round(fact.ConsumeFact * rate.ConsumeRate, decimalLength, MidpointRounding.AwayFromZero)
                             };

            //查找差异
            var calQty = from consume in consumeQty
                         group consume by consume.Material into g
                         where g.Sum(consume => consume.ConsumeQty) != 0
                         select new
                         {
                             Material = g.Key,
                             CalQty = g.Sum(consume => consume.ConsumeQty)
                         };

            var gap = from cal in calQty
                      join tol in itemConsume on cal.Material.Code equals tol.Material.Code
                      where (tol.ConsumedQty - cal.CalQty != 0)
                      select new
                      {
                          Material = cal.Material,
                          GapQty = tol.ConsumedQty - cal.CalQty
                      };

            #endregion

            #region 记录材料成本事务
            HashSet<string> gapAllocate = new HashSet<string>();
            foreach (var consume in consumeQty)
            {
                CostTransaction costTransaction = new CostTransaction();
                costTransaction.Item = consume.ReceiptDet.OrderLocationTransaction.Item.Code;
                costTransaction.ItemCategory = consume.ReceiptDet.OrderLocationTransaction.Item.ItemCategory.Code;
                costTransaction.OrderNo = consume.Order.OrderNo;
                costTransaction.ReceiptNo = receipt.ReceiptNo;
                costTransaction.CostGroup = regionCC.CostGroup;
                costTransaction.CostCenter = regionCC;
                costTransaction.CostElement = materialCostElement;
                costTransaction.Currency = baseCurrency.Code;          //生产的币种和本位币一样，汇率为1
                costTransaction.BaseCurrency = baseCurrency.Code;
                costTransaction.ExchangeRate = 1;
                costTransaction.Qty = (consume.ReceiptDet.ReceivedQty
                    + consume.ReceiptDet.RejectedQty
                    + consume.ReceiptDet.ScrapQty) * consume.ReceiptDet.OrderLocationTransaction.UnitQty;  //收货单位为订单单位，要乘以单位用量
                costTransaction.ReferenceItem = consume.Material.Code;
                costTransaction.ReferenceItemType = consume.Material.Type;
                costTransaction.ReferenceQty = consume.ConsumeQty;                 //物料消耗数量
                #region 查找是否有差异，有差异分摊到消耗该物料的第一项
                if (gap != null && gap.Count() > 0 && !gapAllocate.Contains(consume.Material.Code))
                {
                    var a = gap.Where(g => g.Material.Code == costTransaction.ReferenceItem).SingleOrDefault();
                    if (a != null)
                    {
                        costTransaction.ReferenceQty += a.GapQty;
                    }

                    gapAllocate.Add(costTransaction.ReferenceItem);
                }
                #endregion

                #region 查找标准成本，没有记为0
                decimal? standardCost = standardCostMgr.SumStandardCost(consume.Material, regionCC.CostGroup);
                if (standardCost.HasValue)
                {
                    costTransaction.StandardAmount = Math.Round(costTransaction.ReferenceQty.Value * standardCost.Value, decimalLength, MidpointRounding.AwayFromZero);
                }
                else
                {
                    costTransaction.StandardAmount = 0;
                }
                #endregion

                if (consume.Material.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_C
                    && (consume.ReceiptDet.OrderLocationTransaction.OrderDetail.OrderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ
                || consume.ReceiptDet.OrderLocationTransaction.OrderDetail.OrderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO))
                {
                    //客供品报废计入成品
                    Item customerGoods = consume.Material;
                    if (!customerGoods.ScrapPrice.HasValue)
                    {
                        throw new BusinessErrorException("Cost.CostAllocateTransaction.Error.NoPriceForCustomerGoods", customerGoods.Code);
                    }
                    else
                    {
                        costTransaction.ActualAmount = customerGoods.ScrapPrice.Value * costTransaction.ReferenceQty.Value;
                    }
                }
                else
                {
                    costTransaction.ActualAmount = 0;
                }
                costTransaction.EffectiveDate = dateTimeNow;
                costTransaction.CreateDate = dateTimeNow;
                costTransaction.CreateUser = user.Code;

                this.costTransacationMgr.CreateCostTransaction(costTransaction);
            }
            #endregion

            //}
            #endregion

            #region 记录人工成本事务

            #region 查找直接人工成本要素代码
            EntityPreference laborPerference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_ELEMENT_LABOR);
            CostElement laborCostElement = costElementMgr.LoadCostElement(laborPerference.Value);
            #endregion
            var recGroup = from recDet in receipt.ReceiptDetails
                           group recDet by new { recDet.OrderLocationTransaction.OrderDetail.OrderHead, recDet.OrderLocationTransaction.Item } into o
                           select new
                           {
                               OrderHead = o.Key.OrderHead,
                               Item = o.Key.Item,
                               MaterialNumber = o.Sum(recDet => recDet.OrderLocationTransaction.MaterailNumber),
                               ReceivedQty = o.Sum(recDet =>
                                   recDet.ReceivedQty
                                   + recDet.RejectedQty
                                   + recDet.ScrapQty) //次品、废品全部要计算人工                     
                           };

            var recList = from recDet in recGroup
                          group recDet by recDet.OrderHead into result
                          select new
                          {
                              OrderHead = result.Key,
                              ItemList = result.Select(p => p.Item),
                              MaterialNumberList = result.Select(p => p.MaterialNumber),
                              QtyList = result.Select(p => p.ReceivedQty)
                          };

            foreach (var rec in recList)
            {
                OrderHead orderHead = rec.OrderHead;
                if (orderHead.OrderOperations != null && orderHead.OrderOperations.Count > 0)
                {
                    #region OrderOperation根据WC的成本中心分组，因为要按照工作中心对应的CostCenter记录事务
                    var ccOperationGroup = from operation in orderHead.OrderOperations
                                           group operation by operation.WorkCenter.DefaultCostCenter into result
                                           select new
                                           {
                                               CostCenter = result.Key,
                                               Operations = result.ToList()
                                           };
                    #endregion

                    #region 循环收货，计算人工成本
                    for (int i = 0; i < rec.ItemList.Count(); i++)
                    {
                        Item product = rec.ItemList.ElementAt(i);
                        int? materialNumber = rec.MaterialNumberList.ElementAt(i);
                        int laborHourFact = materialNumber.HasValue ? (materialNumber.Value > 1 ? materialNumber.Value - 1 : 0) : 0; //工时计算因子，根据物料数量-1计算
                        Decimal? receivedQty = rec.QtyList.ElementAt(i);

                        if (!receivedQty.HasValue || receivedQty.Value == 0)
                        {
                            continue;
                        }

                        decimal laborHour = decimal.Zero;
                        decimal laborCost = decimal.Zero;

                        foreach (var ccOp in ccOperationGroup)
                        {
                            foreach (var operation in ccOp.Operations)
                            {
                                #region 运行成本 = 运行小时数 /单位 x WC 人工费率
                                if (operation.RunTime.HasValue)
                                {
                                    laborHour += operation.RunTime.Value / 60 * receivedQty.Value * laborHourFact;
                                    if (operation.WorkCenter.LaborRate.HasValue)
                                    {
                                        laborCost += operation.RunTime.Value / 60 * operation.WorkCenter.LaborRate.Value * receivedQty.Value * laborHourFact;
                                    }
                                }
                                #endregion

                                #region 附加成本 = 运行小时数 x WC 人工附加费率
                                if (operation.RunTime.HasValue && operation.WorkCenter.LaborBurdenRate.HasValue)
                                {
                                    laborCost += operation.RunTime.Value / 60 * operation.WorkCenter.LaborBurdenRate.Value * receivedQty.Value * laborHourFact;
                                }
                                #endregion

                                #region 附加成本% = 运行小时数 x WC人工费率 x WC人工附加%
                                if (operation.RunTime.HasValue && operation.WorkCenter.LaborBurdenPercent.HasValue)
                                {
                                    laborCost += operation.RunTime.Value / 60 * operation.WorkCenter.LaborRate.Value * operation.WorkCenter.LaborBurdenPercent.Value * receivedQty.Value * laborHourFact / 100;
                                }
                                #endregion

                                //占不考虑每工序机器附加成本
                                //还没有搞明白
                                //运行 = 运行 小时数 x 机器附加费率 
                            }

                            #region 记录人工成本事务
                            CostTransaction costTransaction = new CostTransaction();
                            costTransaction.Item = product.Code;
                            costTransaction.ItemCategory = product.ItemCategory.Code;
                            costTransaction.OrderNo = orderHead.OrderNo;
                            costTransaction.ReceiptNo = receipt.ReceiptNo;
                            CostCenter wcCC = costCenterMgr.CheckAndLoadCostCenter(ccOp.CostCenter);
                            costTransaction.CostGroup = wcCC.CostGroup;
                            costTransaction.CostCenter = wcCC;
                            costTransaction.CostElement = laborCostElement;
                            costTransaction.Currency = baseCurrency.Code;          //生产的币种和本位币一样，汇率为1
                            costTransaction.BaseCurrency = baseCurrency.Code;
                            costTransaction.ExchangeRate = 1;
                            costTransaction.Qty = laborHour;
                            #region 记录成本
                            //工作中心的Rate作为标准成本
                            costTransaction.StandardAmount = Math.Round(laborCost, decimalLength, MidpointRounding.AwayFromZero);
                            costTransaction.ActualAmount = 0;
                            #endregion
                            costTransaction.EffectiveDate = dateTimeNow;
                            costTransaction.CreateDate = dateTimeNow;
                            costTransaction.CreateUser = user.Code;

                            this.costTransacationMgr.CreateCostTransaction(costTransaction);
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public void RecordProductionBackFlushCostTransaction(OrderLocationTransaction orderLocationTransaction, Item item, decimal qty, User user)
        {
            CostTransaction costTransaction = new CostTransaction();

            costTransaction.Item = orderLocationTransaction.OrderDetail.Item.Code;
            costTransaction.ItemCategory = orderLocationTransaction.OrderDetail.Item.ItemCategory.Code;
            costTransaction.OrderNo = orderLocationTransaction.OrderDetail.OrderHead.OrderNo;
            costTransaction.CostCenter = this.costCenterMgr.CheckAndLoadCostCenter(orderLocationTransaction.Location.Region.CostCenter);
            costTransaction.CostGroup = costTransaction.CostCenter.CostGroup;

            EntityPreference materialPerference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_ELEMENT_MATERIAL);
            CostElement materialCostElement = costElementMgr.LoadCostElement(materialPerference.Value);
            costTransaction.CostElement = materialCostElement;

            EntityPreference baseCurrencyPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_BASE_CURRENCY);
            Currency baseCurrency = currencyMgr.CheckAndLoadCurrency(baseCurrencyPreference.Value);
            costTransaction.Currency = baseCurrency.Code;
            costTransaction.BaseCurrency = baseCurrency.Code;
            costTransaction.ExchangeRate = 1;
            costTransaction.Qty = 0;

            decimal? standardCost = standardCostMgr.SumStandardCost(item, costTransaction.CostGroup);
            if (standardCost.HasValue)
            {
                EntityPreference decimalLengthPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
                int decimalLength = int.Parse(decimalLengthPreference.Value);

                costTransaction.StandardAmount = Math.Round(qty * standardCost.Value, decimalLength, MidpointRounding.AwayFromZero);
            }
            else
            {
                costTransaction.StandardAmount = 0;
            }

            costTransaction.ActualAmount = 0;
            costTransaction.ReferenceItem = item.Code;
            costTransaction.ReferenceItemType = item.Type;
            costTransaction.ReferenceQty = qty;

            costTransaction.EffectiveDate = DateTime.Now;
            costTransaction.CreateDate = DateTime.Now;
            costTransaction.CreateUser = user.Code;

            this.costTransacationMgr.CreateCostTransaction(costTransaction);
        }

        [Transaction(TransactionMode.Requires)]
        public void RecordProductionSettingCostTransaction(OrderHead orderHead, User user)
        {
            OrderHead oldOrderHead = this.orderHeadMgr.LoadOrderHead(orderHead.OrderNo);
            if (oldOrderHead.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE)
            {
                throw new BusinessErrorException("Cost.Transaction.Error.OrderStatusNotValided", orderHead.OrderNo, oldOrderHead.Status);
            }

            if (oldOrderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                throw new BusinessErrorException("Cost.Transaction.Error.OrderTypeNotValided", orderHead.OrderNo, oldOrderHead.Type);
            }

            #region 如果没有工序，可以不用记录设置成本
            if (oldOrderHead.OrderOperations == null || oldOrderHead.OrderOperations.Count == 0)
            {
                return;
            }
            #endregion

            #region 如果没有收货，可以不用记录设置成本
            var receivedDet = from orderDet in oldOrderHead.OrderDetails
                              where orderDet.ReceivedQty > 0 || orderDet.RejectedQty > 0 || orderDet.ScrapQty > 0
                              select orderDet;

            if (receivedDet == null || receivedDet.Count() == 0)
            {
                return;
            }
            #endregion

            #region 查找直接人工成本要素代码
            EntityPreference laborPerference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_ELEMENT_LABOR);
            CostElement laborCostElement = costElementMgr.LoadCostElement(laborPerference.Value);
            #endregion

            #region 取小数位数
            EntityPreference decimalLengthPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
            int decimalLength = int.Parse(decimalLengthPreference.Value);
            #endregion

            #region 取当前时间
            DateTime dateTimeNow = DateTime.Now;
            #endregion

            #region OrderOperation根据成本中心分组
            var ccOpGroup = from operation in oldOrderHead.OrderOperations
                            group operation by operation.WorkCenter.CostCenter into result
                            select new
                            {
                                CostCenter = result.Key,
                                Operations = result.ToList()
                            };
            #endregion

            FinanceCalendar financeCalendar = financeCalendarMgr.GetLastestOpenFinanceCalendar();
            foreach (var ccOp in ccOpGroup)
            {
                CostCenter costCenter = this.costCenterMgr.CheckAndLoadCostCenter(ccOp.CostCenter);
                IList<OrderOperation> operations = ccOp.Operations;

                DetachedCriteria criteria = DetachedCriteria.For<CostTransaction>();
                criteria.Add(Expression.Eq("OrderNo", oldOrderHead.OrderNo));
                criteria.Add(Expression.Eq("CostCenter", costCenter));
                criteria.Add(Expression.Eq("CostElement", laborCostElement));
                criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));

                IList<CostTransaction> costTransactionList = this.criteriaMgr.FindAll<CostTransaction>(criteria);
                if (costTransactionList == null || costTransactionList.Count == 0)
                {
                    #region 如果没有找到原工单人工成本Trans，就分摊给所有工单
                    criteria = DetachedCriteria.For<CostTransaction>();
                    criteria.Add(Expression.Eq("CostCenter", costCenter));
                    criteria.Add(Expression.Eq("CostElement", laborCostElement));
                    criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));

                    costTransactionList = this.criteriaMgr.FindAll<CostTransaction>(criteria);
                    #endregion
                }

                if (costTransactionList != null && costTransactionList.Count > 0)
                {
                    #region 根据工时分摊
                    //计算总工时
                    decimal totoalQty = costTransactionList.Where(trans => trans.Qty > 0).Sum(trans => trans.Qty);

                    if (totoalQty == 0)
                    {
                        throw new BusinessErrorException("Cost.Transaction.Error.AssignWONotFound");
                    }

                    #region 计算设置成本
                    //设置成本 = 设置小时数x WC 设置 费率
                    var setupCost = operations.Sum(op => op.SetupTime / 60 * op.WorkCenter.SetupRate);
                    var setupTime = operations.Sum(op => op.SetupTime / 60);

                    //设置附加成本
                    //附加成本 = [(设置 小时数) x WC 人工附加费率]
                    //附加成本% = [(设置 小时数/订单数量) x WC 设置 费率 x WC 人工附加%]
                    var setupBdnCost = operations.Sum(op => op.SetupTime / 60 * op.WorkCenter.LaborBurdenRate);
                    var setupBdnPctCost = operations.Sum(op => op.SetupTime / 60 * op.WorkCenter.SetupRate * op.WorkCenter.LaborBurdenPercent / 100);

                    decimal totalSetupCost = (setupCost.HasValue ? setupCost.Value : 0)
                        + (setupBdnCost.HasValue ? setupBdnCost.Value : 0)
                        + (setupBdnPctCost.HasValue ? setupBdnPctCost.Value : 0);
                    decimal remainSetupCost = totalSetupCost;  //剩余成本分配金额
                    #endregion

                    #region 计算分配率
                    decimal costRate = totalSetupCost / totoalQty;
                    decimal remainSetupTime = setupTime.HasValue ? setupTime.Value : 0;
                    decimal timeRate = remainSetupTime / totoalQty;
                    #endregion

                    foreach (CostTransaction costTransaction in costTransactionList)
                    {
                        #region 记录人工成本事务
                        CostTransaction setupCostTransaction = new CostTransaction();
                        CloneHelper.CopyProperty(costTransaction, setupCostTransaction);
                        //工时设置为0，因为收货时已经在运行成本中设置过工时了
                        setupCostTransaction.Id = 0;

                        #region 设置成本分摊，以Rate作为标准成本
                        if (costTransactionList.IndexOf(costTransaction) != (costTransactionList.Count - 1))
                        {
                            //setupCostTransaction.DifferenceAmount = currentSharedCost;
                            setupCostTransaction.StandardAmount = Math.Round(costRate * costTransaction.Qty, decimalLength, MidpointRounding.AwayFromZero);
                            setupCostTransaction.Qty = Math.Round(timeRate * costTransaction.Qty, decimalLength, MidpointRounding.AwayFromZero); ;
                            remainSetupCost -= setupCostTransaction.StandardAmount;
                            remainSetupTime -= setupCostTransaction.Qty;
                        }
                        else
                        {
                            //最后一次，剩余的成本全部分摊到这里
                            setupCostTransaction.StandardAmount = remainSetupCost;
                            setupCostTransaction.Qty = remainSetupTime;
                        }
                        #endregion

                        setupCostTransaction.ActualAmount = 0;
                        setupCostTransaction.EffectiveDate = dateTimeNow;
                        setupCostTransaction.CreateDate = dateTimeNow;
                        setupCostTransaction.CreateUser = user.Code;

                        this.costTransacationMgr.CreateCostTransaction(setupCostTransaction);
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    throw new BusinessErrorException("Cost.Transaction.Error.AssignWONotFound");
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RecordProcurementCostTransaction(PlannedBill plannedBill, User user)
        {
            //Location location = this.locationMgr.LoadLocation(plannedBill.LocationFrom);
            CostCenter costCenter = this.costCenterMgr.CheckAndLoadCostCenter(plannedBill.CostCenter);
            OrderHead orderHead = this.orderHeadMgr.LoadOrderHead(plannedBill.OrderNo);

            #region 查找直接物料成本要素代码
            EntityPreference materialPerference = entityPreferenceMgr.LoadEntityPreference(
                orderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING ?
                BusinessConstants.ENTITY_PREFERENCE_CODE_COST_ELEMENT_MATERIAL : BusinessConstants.ENTITY_PREFERENCE_CODE_COST_ELEMENT_SUBCONTRACT);
            CostElement materialCostElement = costElementMgr.LoadCostElement(materialPerference.Value);
            #endregion

            #region 取企业币种
            EntityPreference baseCurrencyPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_BASE_CURRENCY);
            Currency baseCurrency = currencyMgr.CheckAndLoadCurrency(baseCurrencyPreference.Value);
            #endregion

            #region 取小数位数
            EntityPreference decimalLengthPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
            int decimalLength = int.Parse(decimalLengthPreference.Value);
            #endregion

            #region 取当前时间
            DateTime dateTimeNow = DateTime.Now;
            #endregion

            CostTransaction costTransaction = new CostTransaction();
            costTransaction.Item = plannedBill.Item.Code;
            costTransaction.ItemCategory = plannedBill.Item.ItemCategory.Code;
            costTransaction.OrderNo = orderHead.OrderNo;
            costTransaction.ReceiptNo = plannedBill.ReceiptNo;
            costTransaction.CostCenter = costCenter;
            costTransaction.CostGroup = costCenter.CostGroup;
            costTransaction.CostElement = materialCostElement;
            costTransaction.Currency = orderHead.Currency.Code;
            costTransaction.BaseCurrency = baseCurrency.Code;
            if (orderHead.Currency.Code != baseCurrency.Code)
            {
                costTransaction.ExchangeRate = this.currencyExchangeMgr.FindLastestExchangeRate(orderHead.Currency.Code, baseCurrency.Code);
            }
            else
            {
                costTransaction.ExchangeRate = 1;
            }
            costTransaction.Qty = plannedBill.CurrentActingQty;
            costTransaction.ActualAmount = 0;

            #region 查找标准成本，没有记为0
            Decimal? standardCost = standardCostMgr.SumStandardCost(plannedBill.Item, materialCostElement, costCenter.CostGroup);
            if (standardCost != null)
            {
                costTransaction.StandardAmount = Math.Round(costTransaction.Qty * standardCost.Value, decimalLength, MidpointRounding.AwayFromZero);
            }
            else
            {
                costTransaction.StandardAmount = 0;
            }
            #endregion

            #region 计算实际价格，如果含税要去掉税
            if (plannedBill.IsIncludeTax && plannedBill.TaxCode != null && plannedBill.TaxCode.Trim() != string.Empty)
            {
                TaxRate taxRate = taxRateMgr.CheckAndLoadTaxRate(plannedBill.TaxCode);
                costTransaction.ActualAmount =
                    Math.Round(plannedBill.CurrentBillAmount * costTransaction.ExchangeRate / (1 + taxRate.TaxRate), decimalLength, MidpointRounding.AwayFromZero);
            }
            else
            {
                costTransaction.ActualAmount = plannedBill.CurrentBillAmount * costTransaction.ExchangeRate;
            }
            #endregion

            costTransaction.EffectiveDate = dateTimeNow;
            costTransaction.CreateDate = dateTimeNow;
            costTransaction.CreateUser = user.Code;

            this.costTransacationMgr.CreateCostTransaction(costTransaction);
        }

        [Transaction(TransactionMode.Requires)]
        public void RecordDiffProcurementCostTransaction(ActingBill actingBill, decimal diffAmount, User user)
        {
            CostTransaction costTransaction = new CostTransaction();

            costTransaction.Item = actingBill.Item.Code;
            costTransaction.ItemCategory = actingBill.Item.ItemCategory.Code;
            costTransaction.OrderNo = actingBill.OrderNo;
            costTransaction.ReceiptNo = actingBill.ReceiptNo;
            costTransaction.CostCenter = this.costCenterMgr.CheckAndLoadCostCenter(actingBill.CostCenter);
            costTransaction.CostGroup = this.costGroupMgr.CheckAndLoadCostGroup(actingBill.CostGroup);

            EntityPreference materialPerference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_ELEMENT_MATERIAL);
            CostElement materialCostElement = costElementMgr.LoadCostElement(materialPerference.Value);
            costTransaction.CostElement = materialCostElement;

            EntityPreference baseCurrencyPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_BASE_CURRENCY);
            Currency baseCurrency = currencyMgr.CheckAndLoadCurrency(baseCurrencyPreference.Value);
            costTransaction.Currency = actingBill.Currency.Code;
            costTransaction.BaseCurrency = baseCurrency.Code;
            if (actingBill.Currency.Code != baseCurrency.Code)
            {
                costTransaction.ExchangeRate = this.currencyExchangeMgr.FindLastestExchangeRate(actingBill.Currency.Code, baseCurrency.Code);
            }
            else
            {
                costTransaction.ExchangeRate = 1;
            }
            costTransaction.Qty = 0;
            costTransaction.ActualAmount = diffAmount;

            costTransaction.EffectiveDate = DateTime.Now;
            costTransaction.CreateDate = DateTime.Now;
            costTransaction.CreateUser = user.Code;

            this.costTransacationMgr.CreateCostTransaction(costTransaction);
        }

        [Transaction(TransactionMode.Requires)]
        public void RecordCostAllocationTransaction(CostAllocateTransaction costAllocateTransaction, User user)
        {
            RecordCostAllocationTransaction(costAllocateTransaction, user, DateTime.Now);
        }

        [Transaction(TransactionMode.Requires)]
        public void RecordCostAllocationTransaction(CostAllocateTransaction costAllocateTransaction, User user, DateTime effectiveDate)
        {
            #region 检查EffectiveDate是否合法
            FinanceCalendar financeCalendar = this.financeCalendarMgr.GetLastestOpenFinanceCalendar();

            if (financeCalendar.StartDate < effectiveDate)
            {
                throw new BusinessErrorException("Cost.Transaction.Error.EffDateLtStartDateFinanceCalendar");
            }
            #endregion

            costAllocateTransaction.EffectiveDate = effectiveDate;
            costAllocateTransaction.CreateDate = DateTime.Now;
            costAllocateTransaction.CreateUser = user.Code;

            costAllocateTransactionMgr.CreateCostAllocateTransaction(costAllocateTransaction);
        }

        [Transaction(TransactionMode.Requires)]
        public void CloseFinanceMonth(User user)
        {
            FinanceCalendar financeCalendar = this.financeCalendarMgr.GetLastestOpenFinanceCalendar();
            int lastFinanceYear = financeCalendar.FinanceMonth != 1 ? financeCalendar.FinanceYear : financeCalendar.FinanceYear - 1;
            int lastFinanceMonth = financeCalendar.FinanceMonth != 1 ? financeCalendar.FinanceMonth - 1 : 12;

            if (financeCalendar.EndDate < DateTime.Now)
            {
                throw new BusinessErrorException("Cost.Transaction.Error.Error.CanNotBeginCloseFinanceCalendar", financeCalendar.FinanceYear.ToString(), financeCalendar.FinanceMonth.ToString(), financeCalendar.EndDate.ToLongDateString());
            }

            #region 取企业币种
            EntityPreference baseCurrencyPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_BASE_CURRENCY);
            Currency baseCurrency = currencyMgr.CheckAndLoadCurrency(baseCurrencyPreference.Value);
            #endregion

            #region 取小数位数
            EntityPreference decimalLengthPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
            int decimalLength = int.Parse(decimalLengthPreference.Value);
            #endregion

            #region 取当前时间
            DateTime dateTimeNow = DateTime.Now;
            #endregion

            #region 成本分摊
            AollcateCost(user, financeCalendar, baseCurrency, decimalLength);
            #endregion

            #region 查询所有待处理的零件，并按CostGroup分组
            DetachedCriteria criteria = DetachedCriteria.For<CostTransaction>();
            criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
            criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
            criteria.SetProjection(Projections.Distinct(
                Projections.ProjectionList()
                    .Add(Projections.Property("CostGroup.Code"))
                            .Add(Projections.Property("Item"))));

            IList<object[]> allCostGroupAndItems = this.criteriaMgr.FindAll<object[]>(criteria);
            //IList costTransItems = this.criteriaMgr.FindAll(criteria);

            //IList<object[]> allCostGroupAndItems = new List<object[]>();
            //if (costTransItems != null && costTransItems.Count > 0)
            //{
            //    allCostGroupAndItems = (IList<object[]>)costTransItems;
            //}
            #endregion

            #region 成本余额表复制
            IList<string> costBalaItems = new List<string>();  //存储上期的零件代码

            criteria = DetachedCriteria.For<CostBalance>();

            criteria.Add(Expression.Eq("FinanceYear", lastFinanceYear));
            criteria.Add(Expression.Eq("FinanceMonth", lastFinanceMonth));

            IList<CostBalance> prevCostBalanceList = this.criteriaMgr.FindAll<CostBalance>(criteria);

            if (prevCostBalanceList != null && prevCostBalanceList.Count > 0)
            {
                foreach (CostBalance prevCostBalance in prevCostBalanceList)
                {
                    CostBalance currCostBalance = new CostBalance();
                    CloneHelper.CopyProperty(prevCostBalance, currCostBalance);
                    currCostBalance.Id = 0;
                    currCostBalance.FinanceYear = financeCalendar.FinanceYear;
                    currCostBalance.FinanceMonth = financeCalendar.FinanceMonth;
                    currCostBalance.CreateUser = user.Code;
                    currCostBalance.CreateDate = dateTimeNow;

                    this.costBalanceMgr.CreateCostBalance(currCostBalance);

                    #region 待处理列表中增加当月没有发生成本事务但是上月有成本明细的零件
                    var intersectCostGroupAndItem = from costGroupAndItem in allCostGroupAndItems
                                                    where costGroupAndItem[0] == currCostBalance.CostGroup.Code
                                                        && costGroupAndItem[1] == currCostBalance.Item
                                                    select costGroupAndItem.ToList();

                    if (intersectCostGroupAndItem.Count() == 0)
                    {
                        allCostGroupAndItems.Add(new object[] { currCostBalance.CostGroup.Code, currCostBalance.Item });
                    }
                    #endregion
                }
            }
            #endregion

            #region 递归计算成本明细
            if (allCostGroupAndItems != null && allCostGroupAndItems.Count > 0)
            {
                IList<object[]> calculatedCostGroupAndItems = new List<object[]>(); //存储已经完成成本计算的零件代码
                DateTime effectiveDate = financeCalendar.EndDate.AddMilliseconds(-1);

                #region 客供品成本归集
                criteria = DetachedCriteria.For<CostTransaction>();

                criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
                criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
                criteria.Add(Expression.Eq("ReferenceItemType", BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_C));
                criteria.Add(Expression.Not(Expression.Eq("ActualAmount", decimal.Zero)));

                criteria.SetProjection(Projections.ProjectionList()
                     .Add(Projections.GroupProperty("Item"))
                     .Add(Projections.GroupProperty("CostGroup.Code"))
                     .Add(Projections.GroupProperty("CostElement.Code"))
                     .Add(Projections.Sum("ActualAmount"))
                   );

                IList custCostTransList = this.criteriaMgr.FindAll(criteria);

                if (custCostTransList != null && custCostTransList.Count > 0)
                {
                    foreach (object[] custCostTrans in custCostTransList)
                    {
                        this.costBalanceMgr.ChangeCostBalance((string)custCostTrans[0], (string)custCostTrans[1], (string)custCostTrans[2], (decimal)custCostTrans[3],
                           financeCalendar.FinanceYear, financeCalendar.FinanceMonth, user);
                    }
                }
                #endregion

                #region 按CostGroup分组循环计算成本明细

                int lastCalItemCount = -1;                              //统计上次已经处理完的零件的数量

                while (true)
                {
                    IList<object[]> tobeCalItems = allCostGroupAndItems.Except(calculatedCostGroupAndItems, new CostGroupAndItemComparer()).ToList();
                    if (tobeCalItems == null || tobeCalItems.Count == 0)
                    {
                        //没有待处理的零件，退出循环
                        break;
                    }

                    RecursionCalculateCost(financeCalendar, effectiveDate, baseCurrency, user, decimalLength, tobeCalItems, ref calculatedCostGroupAndItems);

                    if (lastCalItemCount == calculatedCostGroupAndItems.Count)
                    {
                        //本次没有处理任何零件，如果继续会陷入死循环
                        throw new BusinessErrorException("Cost.Transaction.Error.CanNotCalSubItemCost", tobeCalItems.ToString());
                    }
                    else
                    {
                        lastCalItemCount = calculatedCostGroupAndItems.Count;
                    }
                }
                #endregion
            }
            #endregion

            #region 扣减成本余额
            #region 统计本期销售出库事务
            criteria = DetachedCriteria.For<BillTransaction>();
            criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
            criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
            criteria.Add(Expression.Eq("TransactionType", BusinessConstants.BILL_TRANS_TYPE_SO));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("Item"))
                .Add(Projections.GroupProperty("CostGroup"))
                .Add(Projections.Sum("Qty"))
                );

            IList<object[]> saleInvList = this.criteriaMgr.FindAll<object[]>(criteria);
            #endregion

            #region 统计本期生产出库事务
            criteria = DetachedCriteria.For<LocationTransaction>();
            criteria.Add(Expression.In("TransactionType",
                new string[] { BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_WO, 
                                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_WO_BF}));
            criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
            criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("Item"))
                .Add(Projections.GroupProperty("CostGroupFrom"))
                .Add(Projections.Sum("Qty"))
                );

            IList<object[]> prodInvList = this.criteriaMgr.FindAll<object[]>(criteria);
            #endregion

            #region 统计跨CostGroup出库事务
            criteria = DetachedCriteria.For<LocationTransaction>();
            criteria.Add(Expression.Eq("TransactionType", BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_TR));
            criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
            criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
            criteria.Add(Expression.NotEqProperty("CostGroupFrom", "CostGroupTo"));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("Item"))
                .Add(Projections.GroupProperty("CostGroupFrom"))
                .Add(Projections.Sum("Qty"))
                );

            IList<object[]> transInvList = this.criteriaMgr.FindAll<object[]>(criteria);
            #endregion

            #region 循环扣减成本余额
            IList<object[]> invOutList = new List<object[]>();
            if (saleInvList != null)
            {
                IListHelper.AddRange<object[]>(invOutList, saleInvList);
            }

            if (prodInvList != null)
            {
                IListHelper.AddRange<object[]>(invOutList, prodInvList);
            }

            if (transInvList != null)
            {
                IListHelper.AddRange<object[]>(invOutList, transInvList);
            }

            if (invOutList != null && invOutList.Count > 0)
            {
                foreach (object[] invOut in invOutList)
                {
                    criteria = DetachedCriteria.For<CostDetail>();
                    criteria.Add(Expression.Eq("FinanceYear", financeCalendar.FinanceYear));
                    criteria.Add(Expression.Eq("FinanceMonth", financeCalendar.FinanceMonth));
                    criteria.Add(Expression.Eq("Item", invOut[0]));
                    criteria.Add(Expression.Eq("CostGroup.Code", invOut[1]));

                    IList<CostDetail> itemCostDetailList = this.criteriaMgr.FindAll<CostDetail>(criteria);
                    if (itemCostDetailList != null && itemCostDetailList.Count > 0)
                    {
                        foreach (CostDetail itemCostDetail in itemCostDetailList)
                        {
                            //扣减库存成本余额
                            this.costBalanceMgr.ChangeCostBalance(itemCostDetail.Item, itemCostDetail.CostGroup.Code, itemCostDetail.CostElement.Code, (decimal)invOut[2] * itemCostDetail.Cost,
                                financeCalendar.FinanceYear, financeCalendar.FinanceMonth, user);
                        }
                    }
                    else
                    {
                        if (this.itemMgr.CheckAndLoadItem((string)invOut[0]).Type != BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_C)
                        {
                            throw new TechnicalException("不应该找不到本期成本明细的呀，肯定是哪里错了。");
                        }
                    }
                }

                this.FlushSession();
            }
            #endregion
            #endregion

            #region 计算财务库存余额明细
            #region 财务库存余额表复制
            criteria = DetachedCriteria.For<CostInventoryBalance>();

            criteria.Add(Expression.Eq("FinanceYear", lastFinanceYear));
            criteria.Add(Expression.Eq("FinanceMonth", lastFinanceMonth));

            IList<CostInventoryBalance> prevCostInventoryBalanceList = this.criteriaMgr.FindAll<CostInventoryBalance>(criteria);

            if (prevCostInventoryBalanceList != null && prevCostInventoryBalanceList.Count > 0)
            {
                foreach (CostInventoryBalance prevCostInventoryBalance in prevCostInventoryBalanceList)
                {
                    CostInventoryBalance currCostInventoryBalance = new CostInventoryBalance();
                    CloneHelper.CopyProperty(prevCostInventoryBalance, currCostInventoryBalance);
                    currCostInventoryBalance.Id = 0;
                    currCostInventoryBalance.FinanceYear = financeCalendar.FinanceYear;
                    currCostInventoryBalance.FinanceMonth = financeCalendar.FinanceMonth;
                    currCostInventoryBalance.CreateUser = user.Code;
                    currCostInventoryBalance.CreateDate = dateTimeNow;

                    this.costInventoryBalanceMgr.CreateCostInventoryBalance(currCostInventoryBalance);
                }
            }
            #endregion

            #region 账单事务统计
            criteria = DetachedCriteria.For<BillTransaction>();
            criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
            criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
            criteria.Add(Expression.In("TransactionType", new string[] {
                BusinessConstants.BILL_TRANS_TYPE_PO,   //采购
                BusinessConstants.BILL_TRANS_TYPE_SO    //销售
            }));

            criteria.SetProjection(Projections.ProjectionList()
               .Add(Projections.GroupProperty("Item"))
               .Add(Projections.GroupProperty("LocationFrom"))
               .Add(Projections.Sum("Qty"))
               );

            IList<object[]> billTransList = this.criteriaMgr.FindAll<object[]>(criteria);
            #endregion

            #region 库存事务统计
            criteria = DetachedCriteria.For<LocationTransaction>();
            criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
            criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
            criteria.Add(Expression.In("TransactionType", new string[] {
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_WO,   //原材料发货
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_WO,   //成品收货
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_WO_BF, //投料回冲
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_CYC_CNT,   //盘点
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_TR,    //移库
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_TR,    //移库
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_INP,   //检验
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_INP,   //检验
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_UNP,   //计划外入库
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_UNP    //计划外出库
            }));

            criteria.SetProjection(Projections.ProjectionList()
               .Add(Projections.GroupProperty("Item"))
               .Add(Projections.GroupProperty("Location"))
               .Add(Projections.Sum("Qty"))
               );

            IList<object[]> costLocTransList = this.criteriaMgr.FindAll<object[]>(criteria);
            #endregion

            #region 更新库存余额
            IList<object[]> transList = new List<object[]>();

            if (billTransList == null && billTransList.Count > 0 && billTransList[0][0] != null)
            {
                transList = costLocTransList;
            }
            else if (costLocTransList == null && costLocTransList.Count > 0 && costLocTransList[0][0] != null)
            {
                transList = billTransList;
            }
            else
            {
                transList = billTransList;
                IListHelper.AddRange<object[]>(transList, costLocTransList);
            }

            if (transList != null && transList.Count > 0 && transList[0][0] != null)
            {
                var transList2 = from trans in transList
                                 group trans by new { Item = (string)trans[0], Location = (string)trans[1] } into result
                                 select new
                                 {
                                     Item = result.Key.Item,
                                     Location = result.Key.Location,
                                     Qty = result.Sum(trans => (decimal)trans[2])
                                 };

                foreach (var trans in transList2)
                {
                    criteria = DetachedCriteria.For<CostInventoryBalance>();

                    criteria.Add(Expression.Eq("FinanceYear", financeCalendar.FinanceYear));
                    criteria.Add(Expression.Eq("FinanceMonth", financeCalendar.FinanceMonth));
                    criteria.Add(Expression.Eq("Item", trans.Item));
                    criteria.Add(Expression.Eq("Location", trans.Location));

                    IList<CostInventoryBalance> costInvBalanceList = this.criteriaMgr.FindAll<CostInventoryBalance>(criteria);
                    if (costInvBalanceList != null && costInvBalanceList.Count > 0)
                    {
                        CostInventoryBalance costInventoryBalance = costInvBalanceList[0];
                        costInventoryBalance.Qty += trans.Qty;

                        this.costInventoryBalanceMgr.UpdateCostInventoryBalance(costInventoryBalance);
                    }
                    else
                    {
                        criteria = DetachedCriteria.For<Location>();

                        criteria.Add(Expression.Eq("Code", trans.Location));
                        Location location = this.criteriaMgr.FindAll<Location>(criteria)[0];
                        CostGroup costGroup = this.costCenterMgr.LoadCostCenter(location.Region.CostCenter).CostGroup;

                        CostInventoryBalance costInventoryBalance = new CostInventoryBalance();
                        costInventoryBalance.Item = trans.Item;
                        costInventoryBalance.ItemCategory = this.itemMgr.CheckAndLoadItem(trans.Item).ItemCategory.Code;
                        costInventoryBalance.CostGroup = costGroup;
                        costInventoryBalance.Location = location.Code;
                        costInventoryBalance.Qty = trans.Qty;
                        costInventoryBalance.FinanceYear = financeCalendar.FinanceYear;
                        costInventoryBalance.FinanceMonth = financeCalendar.FinanceMonth;
                        costInventoryBalance.CreateDate = dateTimeNow;
                        costInventoryBalance.CreateUser = user.Code;

                        this.costInventoryBalanceMgr.CreateCostInventoryBalance(costInventoryBalance);
                    }
                }
            }
            #endregion
            #endregion

            #region 计算实际库存余额明细
            #region 库存余额表复制
            criteria = DetachedCriteria.For<InventoryBalance>();

            criteria.Add(Expression.Eq("FinanceYear", lastFinanceYear));
            criteria.Add(Expression.Eq("FinanceMonth", lastFinanceMonth));

            IList<InventoryBalance> prevInventoryBalanceList = this.criteriaMgr.FindAll<InventoryBalance>(criteria);

            if (prevInventoryBalanceList != null && prevInventoryBalanceList.Count > 0)
            {
                foreach (InventoryBalance prevInventoryBalance in prevInventoryBalanceList)
                {
                    InventoryBalance currInventoryBalance = new InventoryBalance();
                    CloneHelper.CopyProperty(prevInventoryBalance, currInventoryBalance);
                    currInventoryBalance.Id = 0;
                    currInventoryBalance.FinanceYear = financeCalendar.FinanceYear;
                    currInventoryBalance.FinanceMonth = financeCalendar.FinanceMonth;
                    currInventoryBalance.CreateUser = user.Code;
                    currInventoryBalance.CreateDate = dateTimeNow;

                    this.inventoryBalanceMgr.CreateInventoryBalance(currInventoryBalance);
                }
            }
            #endregion

            #region 库存事务统计
            criteria = DetachedCriteria.For<LocationTransaction>();
            criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));

            criteria.SetProjection(Projections.ProjectionList()
               .Add(Projections.GroupProperty("Item"))
               .Add(Projections.GroupProperty("Location"))
               .Add(Projections.Sum("Qty"))
               );

            IList<object[]> locTransList = this.criteriaMgr.FindAll<object[]>(criteria);
            #endregion

            #region 更新库存余额
            if (locTransList != null && locTransList.Count > 0 && locTransList[0][0] != null)
            {
                var transList2 = from trans in locTransList
                                 group trans by new { Item = (string)trans[0], Location = (string)trans[1] } into result
                                 select new
                                 {
                                     Item = result.Key.Item,
                                     Location = result.Key.Location,
                                     Qty = result.Sum(trans => (decimal)trans[2])
                                 };

                foreach (var trans in transList2)
                {
                    criteria = DetachedCriteria.For<InventoryBalance>();

                    criteria.Add(Expression.Eq("FinanceYear", financeCalendar.FinanceYear));
                    criteria.Add(Expression.Eq("FinanceMonth", financeCalendar.FinanceMonth));
                    criteria.Add(Expression.Eq("Item", trans.Item));
                    criteria.Add(Expression.Eq("Location", trans.Location));

                    IList<InventoryBalance> invBalanceList = this.criteriaMgr.FindAll<InventoryBalance>(criteria);
                    if (invBalanceList != null && invBalanceList.Count > 0)
                    {
                        InventoryBalance inventoryBalance = invBalanceList[0];
                        inventoryBalance.Qty += trans.Qty;

                        this.inventoryBalanceMgr.UpdateInventoryBalance(inventoryBalance);
                    }
                    else
                    {
                        criteria = DetachedCriteria.For<Location>();

                        criteria.Add(Expression.Eq("Code", trans.Location));
                        Location location = this.criteriaMgr.FindAll<Location>(criteria)[0];
                        CostGroup costGroup = this.costCenterMgr.LoadCostCenter(location.Region.CostCenter).CostGroup;

                        InventoryBalance inventoryBalance = new InventoryBalance();
                        inventoryBalance.Item = trans.Item;
                        inventoryBalance.Location = location.Code;
                        inventoryBalance.Qty = trans.Qty;
                        inventoryBalance.FinanceYear = financeCalendar.FinanceYear;
                        inventoryBalance.FinanceMonth = financeCalendar.FinanceMonth;
                        inventoryBalance.CreateDate = dateTimeNow;
                        inventoryBalance.CreateUser = user.Code;

                        this.inventoryBalanceMgr.CreateInventoryBalance(inventoryBalance);
                    }
                }
            }
            #endregion
            #endregion

            #region 财政月关闭
            financeCalendar.IsClosed = true;
            this.financeCalendarMgr.UpdateFinanceCalendar(financeCalendar);
            #endregion
            //throw new TechnicalException();
        }

        [Transaction(TransactionMode.Requires)]
        public void AollcateCost(User user)
        {
            FinanceCalendar financeCalendar = this.financeCalendarMgr.GetLastestOpenFinanceCalendar();

            if (financeCalendar.EndDate > DateTime.Now)
            {
                throw new BusinessErrorException("Cost.Transaction.Error.CanNotBeginCloseFinanceCalendar", financeCalendar.FinanceYear.ToString(), financeCalendar.FinanceMonth.ToString(), financeCalendar.EndDate.ToLongDateString());
            }

            #region 取企业币种
            EntityPreference baseCurrencyPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_BASE_CURRENCY);
            Currency baseCurrency = currencyMgr.CheckAndLoadCurrency(baseCurrencyPreference.Value);
            #endregion

            #region 取小数位数
            EntityPreference decimalLengthPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
            int decimalLength = int.Parse(decimalLengthPreference.Value);
            #endregion

            AollcateCost(user, financeCalendar, baseCurrency, decimalLength);
        }

        #region Private Methods
        private void AollcateCost(User user, FinanceCalendar financeCalendar, Currency baseCurrency, int decimalLength)
        {
            #region 取得最大生效日期
            DateTime effectiveDate = financeCalendar.EndDate.AddMilliseconds(-1);
            //DetachedCriteria criteria = DetachedCriteria.For<CostTransaction>().SetProjection(Projections.Max("EffectiveDate"));
            //criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
            //criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
            //IList maxEffDateList = this.criteriaMgr.FindAll(criteria);
            //if (maxEffDateList != null && maxEffDateList.Count > 0 && maxEffDateList[0] != null)
            //{
            //    effectiveDate = (DateTime)maxEffDateList[0];
            //}
            #endregion

            #region 成本分摊
            #region 取得最大成本事务Id
            DetachedCriteria criteria = DetachedCriteria.For<CostTransaction>().SetProjection(
                Projections.ProjectionList().Add(Projections.Max("Id")).Add(Projections.Min("Id")));
            criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
            criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
            IList maxTransIdList = this.criteriaMgr.FindAll(criteria);
            int maxTransId = -1;
            int minTransId = -1;
            if (maxTransIdList != null && maxTransIdList.Count > 0)
            {
                object[] objList = (object[])maxTransIdList[0];
                if (objList[0] != null)
                {
                    maxTransId = (Int32)objList[0];
                    minTransId = (Int32)objList[1];
                }
            }
            #endregion

            if (maxTransId > 0)
            {
                #region 查找待分摊成本事务
                criteria = DetachedCriteria.For<CostAllocateTransaction>();
                criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
                criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));

                IList<CostAllocateTransaction> costAllocateTransactionList = this.criteriaMgr.FindAll<CostAllocateTransaction>(criteria);
                #endregion

                #region 循环分摊
                if (costAllocateTransactionList != null && costAllocateTransactionList.Count > 0)
                {
                    foreach (CostAllocateTransaction costAllocateTransaction in costAllocateTransactionList)
                    {
                        #region 查找分摊依赖的成本事务
                        criteria = DetachedCriteria.For<CostTransaction>();

                        #region 查询条件
                        criteria.Add(Expression.Between("Id", minTransId, maxTransId));
                        criteria.Add(Expression.Eq("CostElement", costAllocateTransaction.DependCostElement));
                        criteria.Add(Expression.Eq("CostCenter", costAllocateTransaction.CostCenter));

                        if (costAllocateTransaction.Items != null && costAllocateTransaction.Items.Trim() != string.Empty)
                        {
                            criteria.Add(Expression.In("Item", costAllocateTransaction.Items.Split('|')));
                        }

                        if (costAllocateTransaction.Orders != null && costAllocateTransaction.Orders.Trim() != string.Empty)
                        {
                            criteria.Add(Expression.In("OrderNo", costAllocateTransaction.Orders.Split('|')));
                        }

                        if (costAllocateTransaction.ItemCategorys != null && costAllocateTransaction.ItemCategorys.Trim() != string.Empty)
                        {
                            criteria.Add(Expression.In("ItemCategory", costAllocateTransaction.ItemCategorys.Split('|')));
                        }

                        if (costAllocateTransaction.ReferenceItems != null && costAllocateTransaction.ReferenceItems.Trim() != string.Empty)
                        {
                            criteria.Add(Expression.In("ReferenceItem", costAllocateTransaction.ReferenceItems.Split('|')));
                        }
                        #endregion

                        #region 投影
                        ProjectionList projectionList = Projections.ProjectionList()
                            .Add(Projections.GroupProperty("OrderNo").As("OrderNo"))
                            .Add(Projections.GroupProperty("ReceiptNo").As("ReceiptNo"))
                            .Add(Projections.GroupProperty("Item").As("Item"))
                            .Add(Projections.Sum("Qty").As("Qty"))
                            .Add(Projections.Sum("StandardAmount").As("StandardAmount"))
                            .Add(Projections.GroupProperty("ReferenceItem").As("ReferenceItem"))
                            .Add(Projections.GroupProperty("ReferenceItemType").As("ReferenceItemType"))
                            .Add(Projections.Sum("ReferenceQty").As("ReferenceQty"));

                        criteria.SetProjection(projectionList);
                        #endregion

                        #region 查询
                        IList<object[]> translist = this.criteriaMgr.FindAll<object[]>(criteria);

                        if (translist == null || translist.Count == 0 || translist[0] == null || translist[0][2] == null)
                        {
                            throw new BusinessErrorException("Cost.Transaction.Error.CantFindAssignCostTrans", costAllocateTransaction.CostCenter.Code, costAllocateTransaction.DependCostElement.Code);
                        }
                        #endregion
                        #endregion

                        #region 开始分摊

                        #region 计算分摊因子
                        //分母
                        decimal baseAmount = 0;
                        if (costAllocateTransaction.AllocateBy == BusinessConstants.CODE_MASTER_COST_ALLOCATE_BY_AMOUNT)
                        {
                            baseAmount = translist.Sum(trans => (decimal)trans[4]);
                        }
                        else if (costAllocateTransaction.AllocateBy == BusinessConstants.CODE_MASTER_COST_ALLOCATE_BY_QTY)
                        {
                            baseAmount = translist.Sum(trans => (decimal)trans[3]);
                        }
                        else
                        {
                            throw new TechnicalException("Allocate By value : " + costAllocateTransaction.AllocateBy + " is not valided");
                        }

                        if (baseAmount <= 0)
                        {
                            throw new BusinessErrorException("Cost.Transaction.Error.CantFindAssignCostTrans", costAllocateTransaction.CostCenter.Code, costAllocateTransaction.DependCostElement.Code);
                        }

                        //分子
                        var amount = costAllocateTransaction.Amount;
                        decimal remainAmount = amount;

                        //分摊因子
                        decimal allocateFact = amount / baseAmount;
                        #endregion

                        foreach (object[] trans in translist)
                        {
                            #region 分摊
                            CostTransaction costTransaction = new CostTransaction();

                            costTransaction.Item = (string)trans[2];
                            costTransaction.ItemCategory = itemMgr.CheckAndLoadItem(costTransaction.Item).ItemCategory.Code;
                            costTransaction.OrderNo = (string)trans[0];
                            costTransaction.ReceiptNo = (string)trans[1];
                            costTransaction.CostGroup = costAllocateTransaction.CostCenter.CostGroup;
                            costTransaction.CostCenter = costAllocateTransaction.CostCenter;
                            costTransaction.CostElement = costAllocateTransaction.CostElement;
                            costTransaction.Currency = baseCurrency.Code;
                            costTransaction.BaseCurrency = baseCurrency.Code;
                            costTransaction.ExchangeRate = 1;
                            costTransaction.Qty = 0;
                            costTransaction.StandardAmount = 0;
                            if (translist.IndexOf(trans) != translist.Count - 1)
                            {
                                decimal allocateAmount = 0;
                                if (costAllocateTransaction.AllocateBy == BusinessConstants.CODE_MASTER_COST_ALLOCATE_BY_AMOUNT)
                                {
                                    allocateAmount = Math.Round(allocateFact * (decimal)trans[4], decimalLength, MidpointRounding.AwayFromZero);
                                }
                                else if (costAllocateTransaction.AllocateBy == BusinessConstants.CODE_MASTER_COST_ALLOCATE_BY_QTY)
                                {
                                    allocateAmount = Math.Round(allocateFact * (decimal)trans[3], decimalLength, MidpointRounding.AwayFromZero);
                                }
                                costTransaction.ActualAmount = allocateAmount;
                                remainAmount -= allocateAmount;
                            }
                            else
                            {
                                costTransaction.ActualAmount = remainAmount;
                            }

                            costTransaction.EffectiveDate = effectiveDate;
                            costTransaction.CreateDate = DateTime.Now;
                            costTransaction.CreateUser = user.Code;
                            costTransaction.CostAllocateTransaction = costAllocateTransaction.Id;
                            costTransaction.ReferenceItem = trans[5] != null ? (string)trans[5] : null;
                            costTransaction.ReferenceItemType = trans[6] != null ? (string)trans[6] : null;
                            costTransaction.ReferenceQty = trans[7] != null ? (decimal?)trans[7] : null;

                            this.costTransacationMgr.CreateCostTransaction(costTransaction);
                            #endregion
                        }
                        #endregion
                    }

                    this.FlushSession();
                }
                #endregion
            }
            #endregion
        }

        private void RecursionCalculateCost(FinanceCalendar financeCalendar, DateTime effectiveDate, Currency baseCurrency, User user, int decimalLength, IList<object[]> tobeCalGroupAndItems, ref IList<object[]> calculatedGroupAndItems)
        {
            #region 变量定义
            DateTime dateTimeNow = DateTime.Now;

            int lastFinanceYear = financeCalendar.FinanceMonth != 1 ? financeCalendar.FinanceYear : financeCalendar.FinanceYear - 1;
            int lastFinanceMonth = financeCalendar.FinanceMonth != 1 ? financeCalendar.FinanceMonth - 1 : 12;
            #endregion

            #region 循环处理零件
            foreach (object[] tobeCalGroupAndItem in tobeCalGroupAndItems)
            {
                string costGroupCode = (string)tobeCalGroupAndItem[0];
                string itemCode = (string)tobeCalGroupAndItem[1];
                Item item = this.itemMgr.CheckAndLoadItem(itemCode);

                CostGroup costGroup = this.costGroupMgr.CheckAndLoadCostGroup(costGroupCode);

                #region 检查是否可以计算当期成本
                bool pass = false;

                DetachedCriteria criteria = DetachedCriteria.For<CostTransaction>();
                criteria.Add(Expression.Eq("Item", itemCode));
                criteria.Add(Expression.Eq("CostGroup", costGroup));
                criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
                criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
                criteria.Add(Expression.NotEqProperty("Item", "ReferenceItem"));   //过滤掉返工的成品投料
                criteria.Add(Expression.Not(Expression.Eq("ReferenceItemType", BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_C)));  //过滤掉客供品消耗

                criteria.SetProjection(Projections.Distinct(
                            Projections.ProjectionList()
                            .Add(Projections.Property("Item"))
                            .Add(Projections.Property("ReferenceItem"))
                            .Add(Projections.Property("CostGroup.Code"))));

                IList<object[]> costTransList = this.criteriaMgr.FindAll<object[]>(criteria);

                if (costTransList == null || costTransList.Count == 0)
                {
                    //没有成本事务发生，可以计算当期成本
                    pass = true;
                }
                else if (costTransList.Count == 1 && costTransList[0][1] == null)
                {
                    //没有RefItem，可以计算当期成本
                    pass = true;
                }
                else
                {
                    var refCostGroupItems = from trans in costTransList
                                            //join tobe表 过滤掉不需要计算当期成本的零件，如客供品                                           
                                            //join tobe in tobeCalGroupAndItems on new { RefItem = trans[1], RefCG = trans[2] } equals new { RefItem = tobe[1], RefCG = tobe[0] } 
                                            where trans[1] != null
                                            group trans by new { RefItem = trans[1], RefCG = trans[2] } into result
                                            select new object[]
                                       { 
                                           result.Key.RefCG,
                                           result.Key.RefItem
                                       };

                    if (calculatedGroupAndItems.Intersect(refCostGroupItems.ToList(), new CostGroupAndItemComparer()).Count()
                        == refCostGroupItems.Count())
                    {
                        //所有的RefItem都在calculatedItems中，可以计算当期成本
                        pass = true;
                    }
                }
                #endregion

                #region 计算当期成本
                if (pass)
                {
                    #region 把零件号加入完成列表中
                    calculatedGroupAndItems.Add(tobeCalGroupAndItem);
                    #endregion

                    #region 增加当期成本余额
                    criteria = DetachedCriteria.For<CostTransaction>();
                    criteria.CreateAlias("CostElement", "ce");

                    criteria.Add(Expression.Eq("Item", itemCode));
                    criteria.Add(Expression.Eq("CostGroup", costGroup));
                    criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
                    criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
                    criteria.Add(Expression.Or(
                        Expression.Not(Expression.Eq("ReferenceItemType", BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_C)),
                        Expression.IsNull("ReferenceItemType")));   //因为客供品已经汇总过，这里需要过滤掉

                    criteria.SetProjection(Projections.ProjectionList()
                        .Add(Projections.Sum("ActualAmount"))
                        .Add(Projections.GroupProperty("ce.Code")));

                    IList tobeCalCostTransList = this.criteriaMgr.FindAll(criteria);

                    if (tobeCalCostTransList != null && tobeCalCostTransList.Count > 0)
                    {
                        foreach (object[] tobeCalCostTrans in tobeCalCostTransList)
                        {
                            this.costBalanceMgr.ChangeCostBalance(itemCode, costGroupCode, (string)tobeCalCostTrans[1], (decimal)tobeCalCostTrans[0],
                               financeCalendar.FinanceYear, financeCalendar.FinanceMonth, user);
                        }
                    }
                    #endregion

                    #region 查找上月库存结余
                    decimal itemInvOpenBalance = 0;   //期初库存
                    criteria = DetachedCriteria.For<CostInventoryBalance>();
                    criteria.Add(Expression.Eq("Item", itemCode));
                    criteria.Add(Expression.Eq("CostGroup", costGroup));
                    criteria.Add(Expression.Eq("FinanceYear", financeCalendar.FinanceMonth != 1 ? financeCalendar.FinanceYear : (financeCalendar.FinanceYear - 1)));
                    criteria.Add(Expression.Eq("FinanceMonth", financeCalendar.FinanceMonth != 1 ? (financeCalendar.FinanceMonth - 1) : 12));

                    IList<CostInventoryBalance> costInvOpenBalanceList = this.criteriaMgr.FindAll<CostInventoryBalance>();

                    if (costInvOpenBalanceList != null && costInvOpenBalanceList.Count > 0)
                    {
                        itemInvOpenBalance = costInvOpenBalanceList[0].Qty;
                    }
                    #endregion

                    #region 统计本期采购入库事务
                    decimal itemPurInv = 0;   //采购入库
                    criteria = DetachedCriteria.For<BillTransaction>();
                    criteria.Add(Expression.Eq("Item", itemCode));
                    criteria.Add(Expression.Eq("CostGroup", costGroupCode));
                    criteria.Add(Expression.Eq("TransactionType", BusinessConstants.BILL_TRANS_TYPE_PO));
                    criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
                    criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));

                    criteria.SetProjection(Projections.ProjectionList().Add(Projections.Sum("Qty")));

                    IList purInvList = this.criteriaMgr.FindAll(criteria);
                    if (purInvList != null && purInvList.Count > 0 && purInvList[0] != null)
                    {
                        itemPurInv = (decimal)purInvList[0];
                    }
                    #endregion

                    #region 统计本期生产入库事务 + 计划外入库事务 + 盘点(盘亏/盘赢)
                    //计划外出库/盘亏，库存减少了，减少的库存成本要分摊掉
                    //盘赢，库存增加，增加的库存要摊掉一部分成本
                    decimal itemInvIncr = 0;   //生产入库
                    criteria = DetachedCriteria.For<LocationTransaction>();
                    criteria.Add(Expression.Eq("Item", itemCode));
                    criteria.Add(Expression.Or(Expression.In("TransactionType",
                        new string[] { BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_WO, 
                            BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_UNP
                                //BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_UNP,    //计划外出库要扣减库存金额，不会影响库存单价
                                //BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_CYC_CNT     //盘亏同理
                        }), Expression.And(Expression.Eq("TransactionType", BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_CYC_CNT), Expression.Gt("Qty", decimal.Zero))));
                    criteria.Add(Expression.Eq("CostGroupFrom", costGroupCode));
                    criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
                    criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));

                    criteria.SetProjection(Projections.ProjectionList().Add(Projections.Sum("Qty")));

                    IList prodInvList = this.criteriaMgr.FindAll(criteria);
                    if (prodInvList != null && prodInvList.Count > 0 && prodInvList[0] != null)
                    {
                        itemInvIncr = (decimal)prodInvList[0];
                    }
                    #endregion

                    #region 统计本期不同成本单元移库入库(CostGroupFrom != CostGroupTo)
                    //不同CostGroup之间移库，要增加库存成本余额，所以也要考虑这部分库存增加数量
                    //
                    decimal itemTransIncr = 0;
                    //criteria = DetachedCriteria.For<LocationTransaction>();
                    //criteria.Add(Expression.Eq("Item", itemCode));
                    //criteria.Add(Expression.Eq("TransactionType", BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_TR));
                    //criteria.Add(Expression.Eq("CostGroupFrom", costGroupCode));
                    //criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
                    //criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
                    //criteria.Add(Expression.NotEqProperty("CostGroupFrom", "CostGroupTo"));

                    //criteria.SetProjection(Projections.ProjectionList()
                    //    .Add(Projections.Sum("Qty")));

                    //IList transInvList = this.criteriaMgr.FindAll(criteria);
                    //if (transInvList != null && transInvList.Count > 0 && transInvList[0] != null)
                    //{
                    //    itemTransIncr = (decimal)transInvList[0];
                    //}
                    #endregion

                    #region 计算成本单价
                    decimal itemUnitCost = 0;
                    decimal itemCostInvBase = itemInvOpenBalance + itemPurInv + itemInvIncr + itemTransIncr;

                    HashSet<string> completedCostElements = new HashSet<string>();  //记录已经完成计算的成本元素                   

                    #region 循环根据CostElement计算成本单价
                    if (tobeCalCostTransList != null && tobeCalCostTransList.Count > 0)
                    {
                        foreach (object[] tobeCalCostTrans in tobeCalCostTransList)
                        {
                            string costElement = (string)tobeCalCostTrans[1];

                            if (itemCostInvBase != 0)  //如果库存余额等于0，成本单价等于0
                            {
                                CostBalance costBalance = this.costBalanceMgr.GetCostBalance(itemCode, costGroupCode, costElement, financeCalendar.FinanceYear, financeCalendar.FinanceMonth);
                                itemUnitCost = costBalance.Balance / itemCostInvBase;
                            }

                            CostDetail costDetail = new CostDetail();
                            costDetail.Item = itemCode;
                            costDetail.ItemCategory = this.itemMgr.CheckAndLoadItem(itemCode).ItemCategory.Code;
                            costDetail.CostGroup = costGroup;
                            costDetail.CostElement = this.costElementMgr.CheckAndLoadCostElement(costElement);
                            costDetail.FinanceYear = financeCalendar.FinanceYear;
                            costDetail.FinanceMonth = financeCalendar.FinanceMonth;
                            costDetail.Cost = itemUnitCost;
                            costDetail.CreateDate = dateTimeNow;
                            costDetail.CreateUser = user.Code;

                            this.costDetailMgr.CreateCostDetail(costDetail);

                            completedCostElements.Add(costElement);
                        }
                    }
                    #endregion

                    #region 当月没有发生成本事务，成本余额表中有记录的，单位成本直接等于上月的
                    #region 查找上期的成本明细
                    criteria = DetachedCriteria.For<CostDetail>();

                    criteria.Add(Expression.Eq("Item", itemCode));
                    criteria.Add(Expression.Eq("CostGroup", costGroup));
                    criteria.Add(Expression.Eq("FinanceYear", lastFinanceYear));
                    criteria.Add(Expression.Eq("FinanceMonth", lastFinanceMonth));

                    IList<CostDetail> prevCostDetailList = this.criteriaMgr.FindAll<CostDetail>(criteria);
                    #endregion

                    #region 复制上期成本明细等于当期成本明细
                    if (prevCostDetailList != null && prevCostDetailList.Count > 0)
                    {
                        foreach (CostDetail prevCostDetail in prevCostDetailList)
                        {
                            if (!completedCostElements.Contains(prevCostDetail.CostElement.Code))
                            {
                                CostDetail currCostDetail = new CostDetail();
                                CloneHelper.CopyProperty(prevCostDetail, currCostDetail);
                                currCostDetail.Id = 0;
                                currCostDetail.FinanceYear = financeCalendar.FinanceYear;
                                currCostDetail.FinanceMonth = financeCalendar.FinanceMonth;
                                currCostDetail.CreateDate = dateTimeNow;
                                currCostDetail.CreateUser = user.Code;

                                this.costDetailMgr.CreateCostDetail(currCostDetail);

                                completedCostElements.Add(prevCostDetail.CostElement.Code);
                            }
                        }
                    }
                    #endregion
                    #endregion

                    #endregion

                    #region 新增零件作为子件的成本事务
                    criteria = DetachedCriteria.For<CostTransaction>();

                    criteria.Add(Expression.Eq("ReferenceItem", itemCode));
                    criteria.Add(Expression.Eq("CostGroup", costGroup));
                    //criteria.Add(Expression.Eq("ReferenceCostGroup", costGroupCode));
                    criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
                    criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));

                    IList<CostTransaction> refCostTransactionList = this.criteriaMgr.FindAll<CostTransaction>(criteria);
                    if (refCostTransactionList != null && refCostTransactionList.Count > 0)
                    {

                        #region 查找本期Total成本
                        decimal? unitCost = this.costDetailMgr.CalculateItemUnitCost(itemCode, costGroup.Code, financeCalendar.FinanceYear, financeCalendar.FinanceMonth);
                        if (unitCost == null)
                        {
                            throw new BusinessErrorException("Cost.Transaction.Error.CantFindCostTrans", itemCode, costGroup.Code, financeCalendar.FinanceYear.ToString(), financeCalendar.FinanceMonth.ToString());
                        }
                        #endregion

                        #region 循环插入成本事务
                        foreach (CostTransaction refCostTrans in refCostTransactionList)
                        {

                            CostTransaction costTransaction = new CostTransaction();
                            CloneHelper.CopyProperty(refCostTrans, costTransaction);
                            costTransaction.Id = 0;
                            costTransaction.StandardAmount = 0;
                            costTransaction.ActualAmount = Math.Round(unitCost.Value * refCostTrans.ReferenceQty.Value, decimalLength, MidpointRounding.AwayFromZero);
                            costTransaction.ReferenceQty = 0;
                            costTransaction.Qty = 0;
                            costTransaction.EffectiveDate = effectiveDate;
                            costTransaction.CreateDate = dateTimeNow;
                            costTransaction.CreateUser = user.Code;

                            this.costTransacationMgr.CreateCostTransaction(costTransaction);
                        }
                        #endregion
                    }
                    #endregion

                    #region 新增零件作为移库的入库成本事务
                    criteria = DetachedCriteria.For<LocationTransaction>();
                    criteria.Add(Expression.Eq("Item", itemCode));
                    criteria.Add(Expression.Eq("TransactionType", BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_TR));
                    criteria.Add(Expression.Eq("CostGroupFrom", costGroupCode));
                    criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
                    criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));
                    criteria.Add(Expression.NotEqProperty("CostGroupFrom", "CostGroupTo"));

                    criteria.SetProjection(Projections.ProjectionList()
                        .Add(Projections.GroupProperty("OrderNo"))
                        .Add(Projections.GroupProperty("ReceiptNo"))
                        .Add(Projections.GroupProperty("CostGroupTo"))
                        .Add(Projections.GroupProperty("CostCenterFrom"))
                        .Add(Projections.GroupProperty("CostCenterTo"))
                        .Add(Projections.Sum("Qty")));

                    IList transInvList = this.criteriaMgr.FindAll(criteria);

                    if (transInvList != null && transInvList.Count > 0)
                    {
                        IList<CostDetail> costDetailList = this.costDetailMgr.GetCostDetail(itemCode, costGroupCode, financeCalendar.FinanceYear, financeCalendar.FinanceMonth);

                        if (costDetailList != null && costDetailList.Count > 0)
                        {
                            foreach (CostDetail costDetail in costDetailList)
                            {
                                //转出标准成本
                                Decimal? outStandardCost = this.standardCostMgr.SumStandardCost(costDetail.Item, costDetail.CostElement.Code, costDetail.CostGroup.Code);

                                string oldCostGroupTo = null;
                                Decimal? inStandardCost = null;   //转入标准成本
                                foreach (object[] transInv in transInvList)
                                {
                                    if (oldCostGroupTo != (string)transInv[2])
                                    {
                                        oldCostGroupTo = (string)transInv[2];
                                        inStandardCost = this.standardCostMgr.SumStandardCost(costDetail.Item, costDetail.CostElement.Code, oldCostGroupTo);
                                    }

                                    #region 新增CostGroupFrom出库成本事务
                                    CostTransaction outCostTransaction = new CostTransaction();

                                    outCostTransaction.Item = itemCode;
                                    outCostTransaction.ItemCategory = item.ItemCategory.Code;
                                    outCostTransaction.OrderNo = (string)transInv[0];
                                    outCostTransaction.ReceiptNo = (string)transInv[1];
                                    outCostTransaction.CostGroup = costGroup;
                                    outCostTransaction.CostCenter = this.costCenterMgr.CheckAndLoadCostCenter((string)transInv[3]);
                                    outCostTransaction.CostElement = costDetail.CostElement;
                                    outCostTransaction.Currency = baseCurrency.Code;
                                    outCostTransaction.BaseCurrency = baseCurrency.Code;
                                    outCostTransaction.ExchangeRate = 1;
                                    outCostTransaction.Qty = -(decimal)transInv[5];
                                    outCostTransaction.StandardAmount = outStandardCost.HasValue ? outStandardCost.Value * outCostTransaction.Qty : 0;
                                    outCostTransaction.ActualAmount = costDetail.Cost * outCostTransaction.Qty;
                                    outCostTransaction.EffectiveDate = effectiveDate;
                                    outCostTransaction.CreateDate = dateTimeNow;
                                    outCostTransaction.CreateUser = user.Code;

                                    this.costTransacationMgr.CreateCostTransaction(outCostTransaction);
                                    #endregion

                                    #region 新增CostGroupFrom入库成本事务
                                    CostTransaction inCostTransaction = new CostTransaction();

                                    inCostTransaction.Item = itemCode;
                                    inCostTransaction.ItemCategory = item.ItemCategory.Code;
                                    inCostTransaction.OrderNo = (string)transInv[0];
                                    inCostTransaction.ReceiptNo = (string)transInv[1];
                                    inCostTransaction.CostGroup = this.costGroupMgr.CheckAndLoadCostGroup((string)transInv[2]);
                                    inCostTransaction.CostCenter = this.costCenterMgr.CheckAndLoadCostCenter((string)transInv[4]);
                                    inCostTransaction.CostElement = costDetail.CostElement;
                                    inCostTransaction.Currency = baseCurrency.Code;
                                    inCostTransaction.BaseCurrency = baseCurrency.Code;
                                    inCostTransaction.ExchangeRate = 1;
                                    inCostTransaction.Qty = (decimal)transInv[5];
                                    inCostTransaction.StandardAmount = inStandardCost.HasValue ? inStandardCost.Value * inCostTransaction.Qty : 0;
                                    inCostTransaction.ActualAmount = costDetail.Cost * inCostTransaction.Qty;
                                    inCostTransaction.EffectiveDate = effectiveDate;
                                    inCostTransaction.CreateDate = dateTimeNow;
                                    inCostTransaction.CreateUser = user.Code;

                                    this.costTransacationMgr.CreateCostTransaction(inCostTransaction);
                                    #endregion
                                }
                            }
                        }
                    }
                    #endregion

                    #region 扣减计划外出库/盘亏库存金额
                    criteria = DetachedCriteria.For<LocationTransaction>();
                    criteria.Add(Expression.Eq("Item", itemCode));
                    criteria.Add(Expression.Or(Expression.Eq("TransactionType", BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_UNP),
                        Expression.And(Expression.Eq("TransactionType", BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_CYC_CNT), Expression.Lt("Qty", decimal.Zero))));
                    criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
                    criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));

                    criteria.SetProjection(Projections.ProjectionList()
                        .Add(Projections.GroupProperty("OrderNo"))
                        .Add(Projections.GroupProperty("ReceiptNo"))
                        .Add(Projections.GroupProperty("CostCenterFrom"))
                        .Add(Projections.Sum("Qty"))
                        .Add(Projections.GroupProperty("TransactionType")));

                    IList miscOrderList = this.criteriaMgr.FindAll(criteria);

                    if (miscOrderList != null && transInvList.Count > 0)
                    {
                        IList<CostDetail> costDetailList = this.costDetailMgr.GetCostDetail(itemCode, costGroupCode, financeCalendar.FinanceYear, financeCalendar.FinanceMonth);

                        if (costDetailList != null && costDetailList.Count > 0)
                        {
                            foreach (CostDetail costDetail in costDetailList)
                            {
                                Decimal? outStandardCost = this.standardCostMgr.SumStandardCost(costDetail.Item, costDetail.CostElement.Code, costDetail.CostGroup.Code);

                                foreach (object[] transInv in transInvList)
                                {
                                    CostTransaction outCostTransaction = new CostTransaction();

                                    outCostTransaction.Item = itemCode;
                                    outCostTransaction.ItemCategory = item.ItemCategory.Code;
                                    outCostTransaction.OrderNo = (string)transInv[0];
                                    outCostTransaction.ReceiptNo = (string)transInv[1];
                                    outCostTransaction.CostGroup = costGroup;
                                    outCostTransaction.CostCenter = this.costCenterMgr.CheckAndLoadCostCenter((string)transInv[2]);
                                    outCostTransaction.CostElement = costDetail.CostElement;
                                    outCostTransaction.Currency = baseCurrency.Code;
                                    outCostTransaction.BaseCurrency = baseCurrency.Code;
                                    outCostTransaction.ExchangeRate = 1;
                                    outCostTransaction.Qty = -(decimal)transInv[3];
                                    outCostTransaction.StandardAmount = outStandardCost.HasValue ? outStandardCost.Value * outCostTransaction.Qty : 0;
                                    outCostTransaction.ActualAmount = costDetail.Cost * outCostTransaction.Qty;
                                    outCostTransaction.EffectiveDate = effectiveDate;
                                    outCostTransaction.CreateDate = dateTimeNow;
                                    outCostTransaction.CreateUser = user.Code;

                                    this.costTransacationMgr.CreateCostTransaction(outCostTransaction);
                                }
                            }
                        }
                    }
                    #endregion

                    this.FlushSession();
                }
                #endregion
            }

            #endregion
        }
        #endregion
        #endregion

        #region new Cost Method 
        [Transaction(TransactionMode.Unspecified)]
        public void GenBomTree()
        {
            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Item));
            selectCriteria.Add(Expression.Like("ItemCategory.Code", "FG", MatchMode.Start));
            IList<Item> items = criteriaMgr.FindAll<Item>(selectCriteria);


        
        }




        /// <summary>
        /// 历史库存
        /// </summary>
        /// <param name="effDate"></param>
        /// <returns></returns>
        public IList<Balance> GetHisInv(DateTime effDate)
        {
            #region 查找当前库存
            DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationLotDetail));
            criteria.Add(Expression.Not(Expression.Eq("Qty", 0M)));
            criteria.CreateAlias("Item", "i");

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("i.Code").As("Item"))
                .Add(Projections.GroupProperty("i.ItemCategory").As("ItemCategory"))
                .Add(Projections.Sum("Qty").As("Qty")));

            criteria.SetResultTransformer(Transformers.AliasToBean(typeof(Balance)));
            IList<Balance> balances = this.criteriaMgr.FindAll<Balance>(criteria);
            #endregion

            #region LocTrans
            criteria = DetachedCriteria.For(typeof(LocationTransaction));
            criteria.Add(Expression.Gt("EffectiveDate", effDate));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("Item").As("Item"))
                .Add(Projections.Sum("Qty").As("Qty")));

            criteria.SetResultTransformer(Transformers.AliasToBean(typeof(LocationTransaction)));
            IList<LocationTransaction> locTrans = criteriaMgr.FindAll<LocationTransaction>(criteria);
            #endregion

            foreach (var ld in balances)
            {
                decimal transQty = locTrans.Where(l => l.Item == ld.Item).Sum(l => l.Qty);

                //历史库存
                ld.Qty = ld.Qty - (double)transQty;
            }
            return balances;
        }
        #endregion

    }

    class CostGroupAndItemComparer : IEqualityComparer<object[]>
    {
        public bool Equals(object[] x, object[] y)
        {
            return (string)x[0] == (string)y[0] && (string)x[1] == (string)y[1];
        }

        public int GetHashCode(object[] obj)
        {
            string hCode = obj[0] + "|" + obj[1];
            return hCode.GetHashCode();
        }
    }

    class CostComparer : IEqualityComparer<object[]>
    {
        public bool Equals(object[] x, object[] y)
        {
            return x[0] == y[0] && x[1] == y[1] && x[2] == y[2];
        }

        public int GetHashCode(object[] obj)
        {
            string hCode = obj[0] + "|" + obj[1] + "|" + obj[2];
            return hCode.GetHashCode();
        }
    }
}
#region Extend Class
namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class CostMgrE : com.Sconit.Service.Cost.Impl.CostMgr, ICostMgrE
    {

    }
}
#endregion