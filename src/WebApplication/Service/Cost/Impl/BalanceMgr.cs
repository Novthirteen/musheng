using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Entity.Cost;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.Cost;
using com.Sconit.Service.Ext;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using NHibernate.Expression;
using NHibernate.Transform;
using com.Sconit.Entity;
using System.Data;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class BalanceMgr : BalanceBaseMgr, IBalanceMgr
    {
        #region Customized Methods

        public ICriteriaMgrE criteriaMgr { get; set; }
        public ISqlHelperMgrE sqlHelperMgr { get; set; }
        public IBomTreeMgrE bomTreeMgr { get; set; }
        public IFinanceCalendarMgrE financeCalendarMgr { get; set; }
        //public IItemMgrE itemMgr { get; set; }
        public IUomConversionMgrE uomConversionMgr { get; set; }
        public ICbomMgrE cbomMgr { get; set; }
        public IFgCostMgrE fgCostMgr { get; set; }
        public IBomDetailMgrE bomDetailMgr { get; set; }
        public IConsumeMgrE consumeMgr { get; set; }
        public IActingBillMgrE actingBillMgr { get; set; }
        public IUserMgrE userMgr { get; set; }
        public IRawIOBMgrE rawIOBMgr { get; set; }
        public IItemMgrE itemMgr { get; set; }

        //by ljz
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        //by ljz

        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.MRP");


        [Transaction(TransactionMode.Requires)]
        public Balance GetBalance(string fc, string item)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Balance));
            criteria.Add(Expression.Eq("FinanceCalendar", fc));
            criteria.Add(Expression.Eq("Item", item));
            IList<Balance> balances = criteriaMgr.FindAll<Balance>(criteria);
            if(balances != null && balances.Count() > 0)
            {
                return balances[0];
            }
            return new Balance();
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateRawIOB(RawIOB rawIOB)
        {
            this.rawIOBMgr.UpdateRawIOB(rawIOB);
            Balance balance = this.GetBalance(rawIOB.FinanceCalendar, rawIOB.Item);
            balance.Amount = rawIOB.EndAmount;
            balance.Cost = rawIOB.EndCost;
            balance.Qty = rawIOB.EndQty;
            this.UpdateBalance(balance);

            DetachedCriteria criteria = DetachedCriteria.For(typeof(Cbom));
            criteria.Add(Expression.Eq("Item", rawIOB.Item));
            criteria.Add(Expression.Eq("FinanceCalendar", rawIOB.FinanceCalendar));
            var cboms = criteriaMgr.FindAll<Cbom>(criteria);

            foreach(var cbom in cboms)
            {
                var cost = cbom.Cost;
                cbom.Cost = rawIOB.EndCost * cbom.AccumQty;
                this.cbomMgr.UpdateCbom(cbom);

                var fgDiffCost = cbom.Cost - cost;

                criteria = DetachedCriteria.For(typeof(FgCost));
                criteria.Add(Expression.Eq("Item", cbom.Bom));
                criteria.Add(Expression.Eq("FinanceCalendar", rawIOB.FinanceCalendar));
                var fgCostList = criteriaMgr.FindAll<FgCost>(criteria);
                if(fgCostList != null && fgCostList.Count() > 0)
                {
                    foreach(var fgCost in fgCostList)
                    {
                        fgCost.Cost += fgDiffCost;
                        fgCost.ScrapCost += fgDiffCost;
                        fgCost.Diff += fgDiffCost;
                        this.fgCostMgr.UpdateFgCost(fgCost);

                        #region 再上一级
                        NextLevelBom(fgCost.Item, fgCost.FinanceCalendar, fgCost.Cost);
                        #endregion
                    }
                }
            }
        }

        private void NextLevelBom(string item, string financeCalendar, double endCost)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Cbom));
            criteria.Add(Expression.Eq("Item", item));
            criteria.Add(Expression.Eq("FinanceCalendar", financeCalendar));
            var cboms = criteriaMgr.FindAll<Cbom>(criteria);

            foreach(var cbom in cboms)
            {
                var cost = cbom.Cost;
                cbom.Cost = endCost * cbom.AccumQty;
                this.cbomMgr.UpdateCbom(cbom);

                var fgDiffCost = cbom.Cost - cost;

                criteria = DetachedCriteria.For(typeof(FgCost));
                criteria.Add(Expression.Eq("Item", cbom.Bom));
                criteria.Add(Expression.Eq("FinanceCalendar", financeCalendar));
                var fgCostList = criteriaMgr.FindAll<FgCost>(criteria);
                if(fgCostList != null && fgCostList.Count() > 0)
                {
                    foreach(var fgCost in fgCostList)
                    {
                        fgCost.Cost += fgDiffCost;
                        fgCost.ScrapCost += fgDiffCost;
                        fgCost.Diff += fgDiffCost;
                        this.fgCostMgr.UpdateFgCost(fgCost);
                    }
                }
            }
        }

        private static object genBalanceLock = new object();
        [Transaction(TransactionMode.Requires)]
        public void GenBalance(FinanceCalendar financeCalendar, string userCode, bool isGenRm, bool isGenCbom)
        {
            lock (genBalanceLock)
            {
                string fc = Getfc(financeCalendar);

                #region delete C_FGCost
                SqlParameter[] sqlParam = new SqlParameter[1];
                sqlParam[0] = new SqlParameter("@p0", fc);
                string sql = @"delete from C_FGCost where FinanceCalendar = @p0";
                sqlHelperMgr.ExecuteSql(sql, sqlParam);
                #endregion

                DetachedCriteria criteria;
                IList<Balance> balances = new List<Balance>();

                #region delete
                sql = @"delete from C_Balance where FinanceCalendar = @p0
                        delete from C_RawIOB where FinanceCalendar = @p0";
                sqlHelperMgr.ExecuteSql(sql, sqlParam);
                #endregion

                sqlParam = new SqlParameter[4];
                sqlParam[0] = new SqlParameter("@fc", fc);
                sqlParam[1] = new SqlParameter("@startTime", financeCalendar.StartDate);
                sqlParam[2] = new SqlParameter("@endTime", financeCalendar.EndDate);
                sqlParam[3] = new SqlParameter("@userCode", userCode);

                #region 插入本月采购 GenPurchase

                #region 重新计价
                IList<ActingBill> actingBill = actingBillMgr.GetActingBill(null, null, financeCalendar.StartDate, financeCalendar.EndDate, null, null,
                    BusinessConstants.BILL_TRANS_TYPE_PO, null);

                actingBillMgr.RecalculatePrice(actingBill, userMgr.LoadUser(userCode), financeCalendar.EndDate);
                #endregion

                sqlHelperMgr.ExecuteStoredProcedure("GenPurchase", sqlParam);
                log.Info("1.插入本月采购 GenPurchase 成功");
                #endregion

                #region 插入本月损耗 GenConsume
                sqlHelperMgr.ExecuteStoredProcedure("GenConsume", sqlParam);
                log.Info("2.插入本月损耗 GenConsume 成功");
                #endregion

                #region bomtree and GenCbom
                if (isGenCbom)
                {
                    this.GenBomTree(financeCalendar, userCode);
                    this.GenCbom(financeCalendar, userCode);
                }
                log.Info("3.插入本月成本Bom成功");
                #endregion

                #region GenRM in Balance

                #region 获取上月的库存结存金额
                FinanceCalendar lastFinanceCalendar = financeCalendarMgr.GetFinanceCalendar(financeCalendar.FinanceYear, financeCalendar.FinanceMonth, -1);
                string lastfc = Getfc(lastFinanceCalendar);

                //DetachedCriteria criteria = DetachedCriteria.For(typeof(Balance));
                //criteria.Add(Expression.Eq("FinanceCalendar", lastfc));
                //criteria.Add(Expression.Like("ItemCategory", "RM", MatchMode.Anywhere));
                IList<Balance> lastBalances = this.GetBalances(lastfc);
                var items = itemMgr.GetAllItem(true);
                lastBalances = (from l in lastBalances
                                join i in items on l.Item equals i.Code
                                group l by new
                                {
                                    Item = l.Item,
                                    Uom = i.Uom.Code,
                                    FinanceCalendar = l.FinanceCalendar,
                                    ItemCategory = i.ItemCategory.Code
                                } into g
                                select new Balance
                                {
                                    Amount = g.Max(f => f.Amount),
                                    Cost = g.Max(f => f.Cost),
                                    FinanceCalendar = g.Key.FinanceCalendar,
                                    IsProvEst = g.Last().IsProvEst,
                                    Item = g.Key.Item,
                                    ItemCategory = g.Key.ItemCategory,
                                    Qty = g.Max(f => f.Qty),
                                    Uom = g.Key.Uom
                                }).ToList();

                log.Info("5.获取上月的库存结存金额 Balance");

                #endregion

                #region 获取本月的采购入库总数
                criteria = DetachedCriteria.For(typeof(Purchase));
                criteria.Add(Expression.Eq("FinanceCalendar", fc));
                IList<Purchase> purchases = criteriaMgr.FindAll<Purchase>(criteria);
                log.Info("6.获取本月的采购入库总数 Purchase");
                #endregion

                #region 获得本月月末的原材料库存
                IList<Balance> rmBalances = this.GetHisInv(financeCalendar, "RM");
                #endregion

                #region RM的材料成本=(本期采购金额+期初库存金额)/(采购数量+期初数量)

                foreach (var rmBalance in rmBalances)
                {
                    Balance lastBalance = lastBalances.FirstOrDefault(l => l.Item.Trim().ToLower() == rmBalance.Item.Trim().ToLower());
                    if (lastBalance != null)
                    {
                        Balance newBalance = new Balance();
                        CloneHelper.CopyProperty(lastBalance, newBalance);
                        var item_Purchase = purchases.Where(p => p.Item.Trim().ToLower() == lastBalance.Item.Trim().ToLower());
                        double purchaseAmount = item_Purchase.Sum(p => p.Amount);
                        double purchaseQty = 0;
                        double purchaseCost = 0;
                        foreach (var purchase in item_Purchase)
                        {
                            purchaseQty += (double)uomConversionMgr.ConvertUomQty(purchase.Item, purchase.Uom, (decimal)purchase.Qty, lastBalance.Uom);
                            newBalance.IsProvEst = purchase.IsProvEst;
                            purchaseCost = purchase.AvgPrice;
                        }
                        if ((lastBalance.Qty + purchaseQty) != 0)
                        {
                            newBalance.Cost = (lastBalance.Amount + purchaseAmount) / (lastBalance.Qty + purchaseQty);
                        }

                        newBalance.Qty = (double)uomConversionMgr.ConvertUomQty(rmBalance.Item, rmBalance.Uom, (decimal)rmBalance.Qty, lastBalance.Uom);
                        newBalance.Amount = newBalance.Cost * newBalance.Qty;

                        newBalance.FinanceCalendar = fc;
                        newBalance.CreateDate = DateTime.Now;
                        newBalance.CreateUser = userCode;
                        this.CreateBalance(newBalance);
                        balances.Add(newBalance);

                        RawIOB rawIOB = new RawIOB();
                        rawIOB.CreateTime = DateTime.Now;
                        rawIOB.CreateUser = userCode;
                        rawIOB.FinanceCalendar = fc;
                        rawIOB.InAmount = purchaseAmount;
                        rawIOB.InCost = purchaseCost;
                        rawIOB.InQty = purchaseQty;
                        rawIOB.Item = lastBalance.Item;
                        rawIOB.StartAmount = lastBalance.Amount;
                        rawIOB.StartCost = lastBalance.Cost;
                        rawIOB.StartQty = lastBalance.Qty;
                        rawIOB.Uom = lastBalance.Uom;
                        rawIOB.EndAmount = newBalance.Amount;
                        rawIOB.EndCost = newBalance.Cost;
                        rawIOB.EndQty = newBalance.Qty;
                        rawIOB.LastModifyTime = DateTime.Now;
                        rawIOB.LastModifyUser = userCode;
                        rawIOBMgr.CreateRawIOB(rawIOB);
                    }
                    else
                    {
                        //月初没有的原材料RM成本
                        var purchase = purchases.FirstOrDefault(l => l.Item.Trim().ToLower() == rmBalance.Item.Trim().ToLower());
                        if (purchase == null)
                        {
                            purchase = new Purchase();
                            purchase.Item = rmBalance.Item;
                            purchase.Qty = rmBalance.Qty;
                            purchase.Uom = rmBalance.Uom;

                            CgPrice cgPrice = GetCgPrice(purchase.Item, financeCalendar.EndDate);
                            purchase.AvgPrice = (double)uomConversionMgr.ConvertUomQty(purchase.Item, purchase.Uom, (decimal)cgPrice.UnitPrice, cgPrice.Uom);
                            purchase.Amount = purchase.Qty * purchase.AvgPrice;
                            purchase.IsProvEst = cgPrice.IsProvisionalEstimate;
                        }

                        object[] item = this.LoadItem(purchase.Item);
                        if (item != null)
                        {
                            double rate = (double)uomConversionMgr.ConvertUomQty(purchase.Item, purchase.Uom, 1M, (string)item[1]);
                            double purchaseQty = purchase.Qty * rate;
                            Balance newBalance = new Balance();
                            newBalance.Amount = purchase.Amount;
                            newBalance.Cost = purchase.AvgPrice / rate;
                            newBalance.CreateDate = DateTime.Now;
                            newBalance.CreateUser = userCode;
                            newBalance.FinanceCalendar = fc;
                            newBalance.Item = purchase.Item;
                            newBalance.ItemCategory = (string)item[2];
                            newBalance.Qty = purchaseQty;
                            newBalance.Uom = (string)item[1];
                            newBalance.IsProvEst = purchase.IsProvEst;
                            balances.Add(newBalance);
                            this.CreateBalance(newBalance);

                            RawIOB rawIOB = new RawIOB();
                            rawIOB.CreateTime = DateTime.Now;
                            rawIOB.CreateUser = userCode;
                            rawIOB.FinanceCalendar = fc;
                            rawIOB.InAmount = purchase.Amount;
                            rawIOB.InCost = purchase.AvgPrice;
                            rawIOB.InQty = purchaseQty;
                            rawIOB.Item = purchase.Item;
                            rawIOB.StartAmount = 0;
                            rawIOB.StartCost = 0;
                            rawIOB.StartQty = 0;
                            rawIOB.Uom = purchase.Uom;
                            rawIOB.EndAmount = newBalance.Amount;
                            rawIOB.EndCost = newBalance.Cost;
                            rawIOB.EndQty = newBalance.Qty;
                            rawIOB.LastModifyTime = DateTime.Now;
                            rawIOB.LastModifyUser = userCode;
                            rawIOBMgr.CreateRawIOB(rawIOB);
                        }
                    }
                }

                log.Info("7.RM的材料成本=(本期采购金额+期初库存金额)/(采购数量+期初数量)");
                #endregion

                #endregion


                ////////////////////////////
                #region 获取本月成品产出 => fgOutPuts
                criteria = DetachedCriteria.For(typeof(LocationTransaction));
                criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
                criteria.Add(Expression.Le("EffectiveDate", financeCalendar.EndDate));
                criteria.Add(Expression.Eq("TransactionType", "RCT-WO"));

                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.GroupProperty("Item").As("Item"))
                    .Add(Projections.GroupProperty("Uom").As("Uom"))
                    .Add(Projections.Sum("Qty").As("Qty")));

                //criteria.SetResultTransformer(Transformers.AliasToBean(typeof(LocationTransaction)));
                IList<object[]> objfgOutPuts = criteriaMgr.FindAll<object[]>(criteria);

                var fgOutPuts = from f in objfgOutPuts
                                select new FgOutPut
                                {
                                    Item = (string)f[0],
                                    Uom = (string)f[1],
                                    Qty = (decimal)f[2]
                                };

                log.Info("8.获取本月成品产出 => fgOutPuts RCT-WO 成功");
                #endregion

                #region 本月损耗 => Consumes
                criteria = DetachedCriteria.For(typeof(Consume));
                criteria.Add(Expression.Eq("FinanceCalendar", fc));
                IList<Consume> consumes = criteriaMgr.FindAll<Consume>(criteria);
                log.Info("9.本月损耗 => Consumes");
                #endregion

                IList<FgCost> fgCosts = new List<FgCost>();
                #region 成品成本 GenCBom And FGCost
                criteria = DetachedCriteria.For(typeof(Cbom));
                criteria.Add(Expression.Eq("FinanceCalendar", fc));
                IList<Cbom> cboms = criteriaMgr.FindAll<Cbom>(criteria);

                var groupBom = from b in cboms
                               group b by b.Bom into result
                               select new
                               {
                                   Bom = result.Key,
                                   BomDetails = result
                               };

                foreach (var bomItem in groupBom)
                {
                    Item item = items.FirstOrDefault(p => p.Code == bomItem.Bom);
                    //object[] item = this.LoadItem(bomItem.Bom);

                    if (item != null)
                    {
                        //尚未考虑单位转换
                        FgOutPut fgOutPut = fgOutPuts.SingleOrDefault(f => f.Item.Trim().ToLower() == bomItem.Bom.Trim().ToLower());

                        fgOutPut = fgOutPut == null ? new FgOutPut() : fgOutPut;
                        double fgOutQty = Convert.ToDouble(fgOutPut.Qty);
                        //成品损耗分摊
                        var q_consumes = consumes.Where(f => f.Item.Trim().ToLower() == bomItem.Bom.Trim().ToLower());
                        double consumeQty = -(q_consumes.Sum(c => c.Qty));
                        //var consume = consumes.SingleOrDefault(f => f.Item == bomItem.Bom);
                        //consume = consume == null ? new Consume() : consume;
                        FgCost fgCost = new FgCost();

                        foreach (var bomdetail in bomItem.BomDetails)
                        {
                            if (bomdetail.ItemCategory.Contains("RM") || bomdetail.ItemCategory.Contains("CG"))
                            {
                                var balance = balances.FirstOrDefault(b => b.Item.Trim().ToLower() == bomdetail.Item.Trim().ToLower());
                                //balance = balance == null ? new Balance() : balance;
                                if (bomdetail.ItemCategory.Contains("RM"))
                                {
                                    //总成本
                                    if (balance != null)
                                    {
                                        //单位转换
                                        double _accumQty1 = (double)uomConversionMgr.ConvertUomQty(bomdetail.Item, bomdetail.Uom, (decimal)bomdetail.AccumQty, balance.Uom);
                                        bomdetail.Cost = _accumQty1 * balance.Cost;
                                        bomdetail.IsProvEst = balance.IsProvEst;
                                    }
                                    else
                                    {
                                        CgPrice cgPrice = this.GetCgPrice(bomdetail.Item, financeCalendar.EndDate);
                                        double _accumQty2 = (double)uomConversionMgr.ConvertUomQty(bomdetail.Item, bomdetail.Uom, (decimal)bomdetail.AccumQty, cgPrice.Uom);
                                        bomdetail.Cost = _accumQty2 * cgPrice.UnitPrice;
                                        bomdetail.IsProvEst = cgPrice.IsProvisionalEstimate;
                                    }
                                    fgCost.Cost += bomdetail.Cost;
                                }
                                else
                                {
                                    CgPrice cgPrice = this.GetCgPrice(bomdetail.Item, financeCalendar.EndDate);
                                    double _accumQty3 = (double)uomConversionMgr.ConvertUomQty(bomdetail.Item, bomdetail.Uom, (decimal)bomdetail.AccumQty, cgPrice.Uom);
                                    bomdetail.Cost = _accumQty3 * cgPrice.UnitPrice;
                                    bomdetail.IsProvEst = cgPrice.IsProvisionalEstimate;
                                }
                                //投入数量
                                bomdetail.InQty = bomdetail.AccumQty * fgOutQty;
                                //成品损耗量打散成原材料重新计算
                                double _scraptQty = bomdetail.AccumQty * consumeQty;
                                bomdetail.ScraptQty += _scraptQty;
                                //
                                bomdetail.Allocation += (_scraptQty * bomdetail.Cost);
                                //cbomMgr.UpdateCbom(bomdetail);//lazy update

                                fgCost.ScrapCost += bomdetail.Cost;
                                if (bomdetail.IsProvEst)
                                {
                                    fgCost.IsProvEst = true;
                                }
                            }
                        }
                        fgCost.CreateTime = DateTime.Now;
                        fgCost.CreateUser = userCode;
                        fgCost.ItemCategory = item.ItemCategory.Code;
                        fgCost.FinanceCalendar = fc;
                        fgCost.Item = bomItem.Bom;
                        fgCost.Uom = item.Uom.Code;
                        fgCost.OutQty = (double)fgOutPut.Qty;
                        fgCosts.Add(fgCost);
                        //fgCostMgr.CreateFgCost(fgCost);
                        foreach (var consume in q_consumes)
                        {
                            consume.Cost = fgCost.ScrapCost;
                            consume.Amount = consume.Qty * consume.Cost;
                        }
                    }
                }
                log.Info("10.成品成本 GenCBom And FGCost");
                #endregion

                #region 损耗原材料分摊 && 获取消耗成本
                var cgConsumes = consumes.Where(c => c.ItemCategory.Contains("CG"));
                foreach (var consume in cgConsumes)
                {
                    //criteria = DetachedCriteria.For(typeof(PriceListDetail));
                    //criteria.Add(Expression.Eq("Item.Code", consume.Item));
                    //criteria.Add(Expression.Le("StartDate", financeCalendar.EndDate));
                    //criteria.AddOrder(Order.Desc("StartDate")); //按StartDate降序，取最新的价格

                    //IList<PriceListDetail> priceListDetail = criteriaMgr.FindAll<PriceListDetail>(criteria, 0, 1);
                    //if (priceListDetail != null && priceListDetail.Count > 0)
                    //{
                    CgPrice cgPrice = this.GetCgPrice(consume.Item, financeCalendar.EndDate);
                    consume.Cost = (double)uomConversionMgr.ConvertUomQty(consume.Item, consume.Uom, (decimal)cgPrice.UnitPrice, cgPrice.Uom);
                    consume.Amount = consume.Cost * consume.Qty;
                    consume.IsProvEst = cgPrice.IsProvisionalEstimate;
                    //consumeMgr.UpdateConsume(consume);
                    //consumeAmount += (-consume.Amount);
                    //}
                }

                var rmConsumes = consumes.Where(c => c.ItemCategory.Contains("RM") || c.ItemCategory.Contains("CG"));
                double consumeAmount = 0;
                foreach (var consume in rmConsumes)
                {
                    //只分摊到最终的产成品上去
                    var rmCboms = cboms.Where(c => c.Item.Trim().ToLower() == consume.Item.Trim().ToLower() && c.FGCategory == "FG");

                    if (rmCboms.Count() > 0)
                    {
                        consume.Cost = rmCboms.First().Cost;
                        consume.Amount = consume.Cost * consume.Qty;
                        consume.IsProvEst = rmCboms.First().IsProvEst;
                        //consumeMgr.UpdateConsume(consume);
                    }

                    double sumInQty = rmCboms.Sum(c => c.InQty);
                    if (sumInQty != 0)//表示本月有消耗此原材料的成品产出,加权平均分摊到对应的成品的原材料上去
                    {
                        foreach (var cbom in rmCboms)
                        {
                            if (cbom.InQty != 0)
                            {
                                //(此物料的用量/总用量)*报废数
                                cbom.ScraptQty += ((cbom.InQty / sumInQty) * (-consume.Qty));
                                cbom.Allocation += ((cbom.InQty / sumInQty) * (-consume.Amount));
                                //lazy update
                                //cbomMgr.UpdateCbom(cbom);
                                //累加到fgCost.Allocation上去
                                fgCosts.SingleOrDefault(f => f.Item.Trim().ToLower() == cbom.Bom.Trim().ToLower()).Allocation += ((cbom.InQty / sumInQty) * (-consume.Amount));
                            }
                        }
                    }
                    else//表示本月没有有消耗此原材料的成品产出,先累加起来
                    {
                        consumeAmount += (-consume.Amount);
                    }
                }

                //再加权平均分摊到所有的成品上去
                var fg_Costs = fgCosts.Where(f => f.ItemCategory == "FG");
                double sumFgAmount = fg_Costs.Sum(f => f.Amount);
                if (sumFgAmount != 0)
                {
                    foreach (var fgCost in fg_Costs)
                    {
                        if (fgCost.Amount != 0)
                        {
                            fgCost.Allocation += ((fgCost.Amount / sumFgAmount) * consumeAmount);
                            //fgCostMgr.UpdateFgCost(fgCost);
                        }
                    }
                }
                log.Info("11.损耗分摊");
                #endregion

                #region 插入本月成品月结库存 GenFG in Balance
                IList<Balance> fgBalances = this.GetHisInv(financeCalendar, "FG");

                foreach (var fgBalance in fgBalances)
                {
                    //balances
                    var fgCost = fgCosts.FirstOrDefault(f => f.Item.Trim().ToLower() == fgBalance.Item.Trim().ToLower());
                    if (fgCost != null)
                    {
                        var rmBalance = balances.FirstOrDefault(b => b.Item.Trim().ToLower() == fgBalance.Item.Trim().ToLower());

                        if (rmBalance != null)
                        {
                            if (rmBalance.ItemCategory.Contains("FG"))
                            {
                                rmBalance.Cost = fgCost.Cost;
                                rmBalance.Qty = fgBalance.Qty;
                                rmBalance.IsProvEst = fgBalance.IsProvEst;
                                rmBalance.Amount = fgBalance.Cost * fgBalance.Qty;
                            }
                        }
                        else
                        {
                            fgBalance.Cost = fgCost.Cost;
                            fgBalance.Amount = fgBalance.Cost * fgBalance.Qty;
                            fgBalance.CreateDate = DateTime.Now;
                            fgBalance.CreateUser = userCode;
                            fgBalance.FinanceCalendar = fc;
                            fgBalance.IsProvEst = fgCost.IsProvEst;
                            this.CreateBalance(fgBalance);
                        }
                    }
                }

                log.Info("12.插入本月成品生产数 GenFG in Balance");
                #endregion

                fgCostMgr.CreateFgCost(fgCosts);

                #region 发送邮件
                IList<EntityPreference> entityPreferences = entityPreferenceMgrE.GetAllEntityPreference();
                if (entityPreferences != null && entityPreferences.Count > 0)
                {
                    string enableSendMail = string.Empty;
                    foreach (EntityPreference ep in entityPreferences)
                    {
                        if (ep.Code == "EnableSendMailOfCostCalculate")
                        {
                            enableSendMail = ep.Value;
                            break;
                        }
                    }
                    if (enableSendMail.ToLower() == "true")
                    {
                        string subject = string.Empty; //主题
                        string emailFrom = string.Empty; //发件人
                        string userMail = string.Empty; //
                        string SMTPEmailHost = string.Empty;
                        string SMTPEmailPasswd = string.Empty;
                        string supplierEmail = string.Empty;
                        string mailBody = string.Empty;

                        subject = DateTime.Now.ToString() + " - 成本计算执行完成";
                        foreach (EntityPreference ep in entityPreferences)
                        {
                            if (ep.Code == BusinessConstants.ENTITY_PREFERENCE_CODE_SMTPEMAILADDR)
                            {
                                emailFrom = ep.Value;
                                break;
                            }
                        }
                        userMail = emailFrom;
                        //if (user.Email != null && user.Email.Trim() != string.Empty)
                        //{
                        //    userMail = user.Email.Trim();
                        //}

                        foreach (EntityPreference ep in entityPreferences)
                        {
                            if (ep.Code == BusinessConstants.ENTITY_PREFERENCE_CODE_SMTPEMAILHOST)
                            {
                                SMTPEmailHost = ep.Value;
                                break;
                            }
                        }
                        foreach (EntityPreference ep in entityPreferences)
                        {
                            if (ep.Code == BusinessConstants.ENTITY_PREFERENCE_CODE_SMTPEMAILPASSWD)
                            {
                                SMTPEmailPasswd = ep.Value;
                                break;
                            }
                        }
                        foreach (EntityPreference ep in entityPreferences)
                        {
                            if (ep.Code == "MailAddrOfCostCalculate")
                            {
                                supplierEmail = ep.Value;
                                break;
                            }
                        }
                        mailBody = "您好,<br />";
                        mailBody += "成本计算已经执行完成!<br />";
                        mailBody += "日期:" + DateTime.Now.ToString();
                        SMTPHelper.SendSMTPEMail(subject, mailBody, emailFrom, supplierEmail, SMTPEmailHost, SMTPEmailPasswd, userMail);
                    }
                }
                #endregion
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void GenBomTree(FinanceCalendar financeCalendar, string userCode)
        {
            sqlHelperMgr.ExecuteSql(" truncate table bomtree", null);
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
            criteria.Add(Expression.Like("ItemCategory.Code", "FG", MatchMode.Anywhere));
            IList<Item> items = criteriaMgr.FindAll<Item>(criteria);
            foreach(Item item in items)
            {
                IList<BomDetail> newBomDetails = this.bomDetailMgr.GetTreeBomDetail(item.Code, financeCalendar.EndDate);
                if(newBomDetails != null)
                {
                    string fgUom = item.Uom.Code;
                    var q = newBomDetails.First(b => b.Bom.Code.Trim().ToLower() == item.Code.Trim().ToLower()).Bom.Uom;
                    if(q != null)
                    {
                        fgUom = q.Code;
                    }

                    foreach(BomDetail bomDetail in newBomDetails)
                    {
                        BomTree bomtree = new BomTree();
                        bomtree.Bom = bomDetail.Bom.Code;
                        bomtree.BomDesc = bomDetail.Bom.Description;
                        bomtree.Item = bomDetail.Item.Code;
                        bomtree.ItemDesc = bomDetail.Item.Description;
                        bomtree.RateQty = bomDetail.RateQty;
                        bomtree.Uom = bomDetail.Uom.Code;
                        bomtree.BomLevel = bomDetail.BomLevel;
                        bomtree.ItemCategoryCode = bomDetail.Item.ItemCategory.Code;
                        bomtree.ItemCategoryDesc = bomDetail.Item.ItemCategory.Description;
                        bomtree.AccumQty = bomDetail.AccumQty;
                        bomtree.CreateTime = DateTime.Now;
                        bomtree.FG = item.Code;
                        bomtree.FGDesc = item.Description;
                        bomtree.FGUom = fgUom;
                        bomtree.FGCategory = item.ItemCategory.Code;
                        bomTreeMgr.CreateBomTree(bomtree);
                    }
                }
            }
        }

        public void GenCbom(FinanceCalendar financeCalendar, string userCode)
        {
            string fc = financeCalendar.FinanceYear.ToString() + "-" + financeCalendar.FinanceMonth.ToString();
            SqlParameter[] sqlParam = new SqlParameter[4];
            sqlParam[0] = new SqlParameter("@fc", fc);
            sqlParam[1] = new SqlParameter("@startTime", financeCalendar.StartDate);
            sqlParam[2] = new SqlParameter("@endTime", financeCalendar.EndDate);
            sqlParam[3] = new SqlParameter("@userCode", userCode);
            sqlHelperMgr.ExecuteStoredProcedure("GenCbom", sqlParam);
        }

        public IList<Balance> GetHisInv(FinanceCalendar financeCalendar)
        {
            return GetHisInv(financeCalendar, null);
        }

        public IList<Balance> GetHisInv(FinanceCalendar financeCalendar, string itemCategory)
        {
            string fc = Getfc(financeCalendar);

            #region 查找当前库存
            DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationLotDetail));
            //criteria.Add(Expression.Not(Expression.Eq("Qty", 0M)));
            if(!string.IsNullOrEmpty(itemCategory))
            {
                criteria.CreateAlias("Item", "i");
                criteria.CreateAlias("i.ItemCategory", "ic");
                criteria.Add(Expression.Like("ic.Code", itemCategory, MatchMode.Anywhere));
            }
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("Item").As("Item"))
                .Add(Projections.Sum("Qty").As("Qty")));

            criteria.SetResultTransformer(Transformers.AliasToBean(typeof(LocationLotDetail)));
            IList<LocationLotDetail> locationLotDetails = this.criteriaMgr.FindAll<LocationLotDetail>(criteria);
            #endregion

            #region LocTrans
            criteria = DetachedCriteria.For(typeof(LocationTransaction));
            criteria.Add(Expression.Gt("EffectiveDate", financeCalendar.EndDate));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("Item").As("Item"))
                .Add(Projections.Sum("Qty").As("Qty")));

            criteria.SetResultTransformer(Transformers.AliasToBean(typeof(LocationTransaction)));
            IList<LocationTransaction> locTrans = criteriaMgr.FindAll<LocationTransaction>(criteria);
            #endregion

            IList<Balance> balances = new List<Balance>();
            foreach(var ld in locationLotDetails)
            {
                Balance balance = new Balance();
                decimal transQty = locTrans.Where(l => l.Item.Trim().ToLower() == ld.Item.Code.Trim().ToLower()).Sum(l => l.Qty);

                //历史库存
                balance.FinanceCalendar = fc;
                balance.Item = ld.Item.Code;
                balance.ItemCategory = ld.Item.ItemCategory.Code;
                balance.Qty = (double)(ld.Qty - transQty);
                balance.Uom = ld.Item.Uom.Code;
                balances.Add(balance);
            }
            return balances;
        }

        #endregion Customized Methods

        private string Getfc(FinanceCalendar financeCalendar)
        {
            return financeCalendar.FinanceYear.ToString() + "-" + financeCalendar.FinanceMonth.ToString();
        }

        private object[] LoadItem(string itemCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
            criteria.Add(Expression.Eq("Code", itemCode));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("Code").As("Code"))
                .Add(Projections.GroupProperty("Uom.Code").As("Uom"))
                .Add(Projections.GroupProperty("ItemCategory.Code").As("ItemCategory")));

            IList<object[]> objs = criteriaMgr.FindAll<object[]>(criteria);
            if(objs != null && objs.Count() > 0)
            {
                return objs[0];
            }
            return null;
        }

        private CgPrice GetCgPrice(string itemCode, DateTime effDate)
        {
            string sql = @"select top 1 UnitPrice,Uom ,IsProvEst as IsProvisionalEstimate  from pricelistdet
                        left join pricelistmstr on pricelistmstr.Code = pricelistdet.pricelist
                        where item = @p0 and pricelistmstr.Type = 'Purchase'
                        and startdate<= @p1 and (enddate is null or enddate> @p1)
                        order by StartDate desc";

            SqlParameter[] sqlParam = new SqlParameter[2];

            sqlParam[0] = new SqlParameter("@p0", itemCode);
            sqlParam[1] = new SqlParameter("@p1", effDate);

            DataSet dataSet = sqlHelperMgr.GetDatasetBySql(sql, sqlParam);

            List<CgPrice> priceList = IListHelper.DataTableToList<CgPrice>(dataSet.Tables[0]);

            if(priceList != null && priceList.Count() > 0)
            {
                return priceList.First();
            }
            return new CgPrice();
        }

        private IList<Balance> GetBalances(string fc)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Balance));
            criteria.Add(Expression.Eq("FinanceCalendar", fc));
            //criteria.Add(Expression.Like("ItemCategory", "RM", MatchMode.Anywhere));
            return criteriaMgr.FindAll<Balance>(criteria);
        }
    }

    class FgOutPut
    {
        public string Item { get; set; }
        public string Uom { get; set; }
        public decimal Qty { get; set; }
    }

    class CgPrice
    {
        public double UnitPrice { get; set; }
        public string Uom { get; set; }
        public bool IsProvisionalEstimate { get; set; }
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class BalanceMgrE : com.Sconit.Service.Cost.Impl.BalanceMgr, IBalanceMgrE
    {
    }
}

#endregion Extend Class