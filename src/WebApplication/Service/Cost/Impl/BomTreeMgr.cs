using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class BomTreeMgr : BomTreeBaseMgr, IBomTreeMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        /*
        private void GenAllocation(IList<Consume> consumes, IList<Cbom> cboms, IList<FgCost> fgCosts, IList<LocationTransaction> fgOutPuts, IList<Balance> balances)
        {

            var groupBom = from b in cboms
                           group b by b.Bom into result
                           select new
                           {
                               Bom = result.Key,
                               BomDetails = cboms.Where(c => c.Bom == result.Key)
                           };

            foreach (var bom in groupBom)
            {
                var fgOutPut = fgOutPuts.SingleOrDefault(f => f.Item == bom.Bom);
                fgOutPut = fgOutPut == null ? new LocationTransaction() : fgOutPut;
                //��Ʒ
                var consume = consumes.SingleOrDefault(f => f.Item == bom.Bom);
                consume = consume == null ? new Consume() : consume;
                foreach (var bomdetail in bom.BomDetails)
                {
                    if (bomdetail.ItemCategory.Contains("RM"))
                    {
                        //Ͷ����
                        bomdetail.InQty = bomdetail.AccumQty * (double)fgOutPut.Qty;
                        //��Ʒ�������ɢ��ԭ�������¼���
                        bomdetail.ScraptQty += (bomdetail.AccumQty * (-(double)consume.Qty));
                        cbomMgr.UpdateCbom(bomdetail);
                    }
                }
            }

            #region ���ԭ���Ϸ�̯
            var rmConsumes = consumes.Where(c => c.ItemCategory.Contains("RM"));
            double rmCost = 0;
            foreach (var consume in rmConsumes)
            {
                var rmCboms = cboms.Where(c => c.Item == consume.Item);
                double sumInQty = rmCboms.Sum(c => c.InQty);
                if (sumInQty > 0)//��ʾ���������Ĵ�ԭ���ϵĳ�Ʒ����,��Ȩƽ����̯����Ӧ�ĳ�Ʒ��ԭ������ȥ
                {
                    foreach (var cbom in rmCboms)
                    {
                        cbom.ScraptQty += ((cbom.InQty / sumInQty) * (-consume.Qty));
                        cbom.Allocation += (cbom.ScraptQty / cbom.InQty) * cbom.Cost;
                        //�ۼӵ�fgCost.Allocation��ȥ
                        fgCosts.SingleOrDefault(f => f.Item == cbom.Bom).Allocation += (cbom.ScraptQty / cbom.InQty) * cbom.Cost;
                    }
                }
                else//��ʾ����û�������Ĵ�ԭ���ϵĳ�Ʒ����,���ۼ�����
                {
                    rmCost += consume.Cost;
                }
            }

            //�ټ�Ȩƽ����̯�����еĳ�Ʒ��ȥ
            double sumFgAmount = fgCosts.Sum(f => f.Amount);
            foreach (var fgCost in fgCosts)
            {
                var fgOutPut = fgOutPuts.SingleOrDefault(f => f.Item == fgCost.Item);
                fgOutPut = fgOutPut == null ? new LocationTransaction() : fgOutPut;

                fgCost.Allocation += (fgCost.Amount / sumFgAmount) * rmCost;
                fgCost.OutQty = (double)fgOutPut.Qty;
                fgCostMgr.UpdateFgCost(fgCost);
            }
            log.Info("10.���ԭ���Ϸ�̯");
            #endregion


            #region ��Ʒ�ɱ� GenCBom And FGCost
            
            #region bomtree           
            criteria = DetachedCriteria.For(typeof(Item));
            criteria.Add(Expression.Like("ItemCategory.Code", "FG", MatchMode.Start));
            IList<Item> items = criteriaMgr.FindAll<Item>(criteria);

            foreach (Item item in items)
            {
                IList<BomDetail> newBomDetails = this.bomDetailMgr.GetTreeBomDetail(item.Code, financeCalendar.EndDate);
                if (newBomDetails != null)
                {
                    string fgUom = newBomDetails.First(b => b.Bom.Code == item.Code).Bom.Uom.Code;

                    foreach (BomDetail bomDetail in newBomDetails)
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
                        bomTreeMgr.CreateBomTree(bomtree);
                    }
                }
            }
            #endregion

            IList<BomTree> bomTrees = bomTreeMgr.GetAllBomTree();
            var groupBom = from b in bomTrees
                           group b by b.Bom into result
                           select new
                           {
                               Bom = result.Key,
                               BomDetails = bomTrees.Where(c => c.Bom == result.Key)
                           };

            foreach (var bomItem in groupBom)
            {
                Item item = itemMgr.LoadItem(bomItem.Bom);
                if (item != null)
                {
                    var fgOutPut = fgOutPuts.SingleOrDefault(f => f.Item == bomItem.Bom);
                    fgOutPut = fgOutPut == null ? new LocationTransaction() : fgOutPut;
                    var consume = consumes.SingleOrDefault(f => f.Item == bomItem.Bom);
                    consume = consume == null ? new Consume() : consume;
                    FgCost fgCost = new FgCost();
                    foreach (var bomdetail in bomItem.BomDetails)
                    {
                        if (bomdetail.ItemCategoryCode.Contains("RM"))
                        {
                            var balance = balances.FirstOrDefault(b => b.Item == bomdetail.Item);
                            balance = balance == null ? new Balance() : balance;
                            Cbom cbom = new Cbom();
                            cbom.Bom = bomItem.Bom;
                            cbom.BomLevel = bomdetail.BomLevel;
                            cbom.AccumQty = (double)bomdetail.AccumQty;
                            cbom.Cost = cbom.AccumQty * balance.Cost;
                            cbom.CreateDate = DateTime.Now;
                            cbom.CreateUser = userCode;
                            cbom.FinanceCalendar = fc;
                            cbom.Item = bomdetail.Item;
                            cbom.ItemCategory = bomdetail.ItemCategoryCode;
                            cbom.Uom = bomdetail.Uom;
                            //Ͷ����
                            cbom.InQty = cbom.AccumQty * (double)fgOutPut.Qty;
                            //��Ʒ�������ɢ��ԭ�������¼���
                            cbom.ScraptQty += cbom.AccumQty * (double)consume.Qty;
                            cboms.Add(cbom);
                            cbomMgr.CreateCbom(cbom);
                            fgCost.Cost += cbom.Cost;
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
                    fgCostMgr.CreateFgCost(fgCost);
                }
            }
            log.Info("8.��Ʒ�ɱ� GenCBom And FGCost");
            
            #endregion
        }

        private void GenPurchaseAndGenConsume(FinanceCalendar financeCalendar, string userCode)
        {
            string fc = getfc(financeCalendar);
            #region ���²ɹ� GenPurchase
            //ɾ�����е�
            SqlParameter[] sqlParam = new SqlParameter[1];
            sqlParam[0] = new SqlParameter("@p0", fc);
            sqlHelperMgr.ExecuteSql(" delete from C_Purchase where FinanceCalendar = @p0", sqlParam);

            //�����µ�
            sqlParam = new SqlParameter[4];
            sqlParam[0] = new SqlParameter("@p0", fc);
            sqlParam[1] = new SqlParameter("@p1", financeCalendar.StartDate);
            sqlParam[2] = new SqlParameter("@p2", financeCalendar.EndDate);
            sqlParam[3] = new SqlParameter("@p3", userCode);

            string sql = @"insert into dbo.C_Purchase
                select 
                item as Item,actbill.uom as Uom,party.code as Supplier,
                sum(BillQty) as Qty,
                sum(BillAmount) as Balance,
                case when sum(BillQty)<>0 then sum(BillAmount) / sum(BillQty) else 0 end as Cost,
                @p0 as FinanceCalendar,getdate() as CreateDate,@p3 as CreateUser
                from actbill 
                left join partyaddr on partyaddr.code = actbill.billaddr
                left join party on partyaddr.partycode = party.code                         
                where transtype='PO' 
                and effdate>=@p1 and effdate <=@p2
                group by party.code,party.name,item,actbill.uom
                order by party.code";
            sqlHelperMgr.ExecuteSql(sql, sqlParam);
            #endregion

            #region �������� GenConsume
            //ɾ�����е�
            sqlParam = new SqlParameter[1];
            sqlParam[0] = new SqlParameter("@p0", fc);
            sqlHelperMgr.ExecuteSql(" delete from C_Consume where FinanceCalendar = @p0", sqlParam);

            //�����µ�
            sqlParam = new SqlParameter[4];
            sqlParam[0] = new SqlParameter("@p0", fc);
            sqlParam[1] = new SqlParameter("@p1", financeCalendar.StartDate);
            sqlParam[2] = new SqlParameter("@p2", financeCalendar.EndDate);
            sqlParam[3] = new SqlParameter("@p3", userCode);

            sql = @"insert into C_Consume
                select Item, LocTrans.Uom,TransType,
                Item.Category as ItemCategory,
                sum(Qty) as Qty,0 as Balance,0  as Cost,
                @p0 as FinanceCalendar, getdate() as CreateDate,@p3 as CreateUser
                from LocTrans 
                left join Location on Location.code = LocTrans.loc
                left join Item on Item.code = LocTrans.Item
                where Effdate>=@p1 and Effdate <=@p2 and 
                TransType in('ISS-UNP','RCT-UNP','CYC-CNT') and item.code is not null
                group by Item,LocTrans.Uom,Item.Category,TransType";

            sqlHelperMgr.ExecuteSql(sql, sqlParam);
            #endregion

        }

        public void DeleteData(FinanceCalendar financeCalendar)
        {
            string fc = getfc(financeCalendar);
            #region delete C_Balance & C_Bom & C_FGCost
            SqlParameter[] sqlParam = new SqlParameter[1];
            sqlParam[0] = new SqlParameter("@p0", fc);
            string sql = @"delete from C_Balance where FinanceCalendar = @p0
                           delete from C_FGCost where FinanceCalendar = @p0";
            sqlHelperMgr.ExecuteSql(sql, sqlParam);
            #endregion
        }

        public void GenData(FinanceCalendar financeCalendar, string userCode)
        {
            string fc = getfc(financeCalendar);
            SqlParameter[] sqlParam = new SqlParameter[4];
            sqlParam[0] = new SqlParameter("@fc", fc);
            sqlParam[1] = new SqlParameter("@startTime", financeCalendar.StartDate);
            sqlParam[2] = new SqlParameter("@endTime", financeCalendar.EndDate);
            sqlParam[3] = new SqlParameter("@userCode", userCode);
            #region ���뱾�²ɹ� GenPurchase

            #region ���¼Ƽ�
            IList<ActingBill> actingBill = actingBillMgr.GetActingBill(null, null, financeCalendar.StartDate, financeCalendar.EndDate, null, null,
                BusinessConstants.BILL_TRANS_TYPE_PO, null);

            actingBillMgr.RecalculatePrice(actingBill, userMgr.LoadUser(userCode), financeCalendar.EndDate);
            #endregion

            sqlHelperMgr.ExecuteStoredProcedure("GenPurchase", sqlParam);
            log.Info("1.���뱾�²ɹ� GenPurchase �ɹ�");
            #endregion

            #region ���뱾����� GenConsume
            sqlHelperMgr.ExecuteStoredProcedure("GenConsume", sqlParam);
            log.Info("2.���뱾����� GenConsume �ɹ�");
            #endregion

            #region GenCbom
            sqlHelperMgr.ExecuteStoredProcedure("GenCbom", sqlParam);
            log.Info("3.���뱾�³ɱ�Bom�ɹ�");
            #endregion
        }

        public IList<LocationTransaction> GetFgOutPuts(FinanceCalendar financeCalendar)
        {
            #region ��ȡ���³�Ʒ���� => fgOutPuts
            DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationTransaction));
            criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
            criteria.Add(Expression.Le("EffectiveDate", financeCalendar.EndDate));
            criteria.Add(Expression.Eq("TransactionType", "RCT-WO"));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("Item").As("Item"))
                .Add(Projections.Sum("Qty").As("Qty")));

            criteria.SetResultTransformer(Transformers.AliasToBean(typeof(LocationTransaction)));
            log.Info("4.��ȡ���³�Ʒ���� => fgOutPuts RCT-WO �ɹ�");
            return criteriaMgr.FindAll<LocationTransaction>(criteria);
            #endregion
        }

        public IList<Balance> GetLastRmBalances(FinanceCalendar financeCalendar)
        {
            #region ��ȡ���µĿ������
            FinanceCalendar lastFinanceCalendar = financeCalendarMgr.GetFinanceCalendar(financeCalendar.FinanceYear, financeCalendar.FinanceMonth, -1);
            string lastfc = getfc(lastFinanceCalendar);

            DetachedCriteria criteria = DetachedCriteria.For(typeof(Balance));
            criteria.Add(Expression.Eq("FinanceCalendar", lastfc));
            criteria.Add(Expression.Like("ItemCategory", "RM", MatchMode.Anywhere));
            log.Info("5.��ȡ���µĿ������ Balance");
            return criteriaMgr.FindAll<Balance>(criteria);
            #endregion
        }

        public IList<Purchase> GetPurchases(FinanceCalendar financeCalendar)
        {
            #region ��ȡ���µĲɹ��������
            string fc = getfc(financeCalendar);
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Purchase));
            criteria.Add(Expression.Eq("FinanceCalendar", fc));
            log.Info("6.��ȡ���µĲɹ�������� Purchase");
            return criteriaMgr.FindAll<Purchase>(criteria);
            #endregion
        }

        public IList<Consume> GetConsumes(FinanceCalendar financeCalendar)
        {
            #region ������� => Consumes
            string fc = getfc(financeCalendar);
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Consume));
            criteria.Add(Expression.Eq("FinanceCalendar", fc));
            log.Info("8.������� => Consumes");
            return criteriaMgr.FindAll<Consume>(criteria);
            #endregion
        }

        public IList<Cbom> GetCboms(FinanceCalendar financeCalendar)
        {
            string fc = getfc(financeCalendar);
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Cbom));
            criteria.Add(Expression.Eq("FinanceCalendar", fc));
            return criteriaMgr.FindAll<Cbom>(criteria);
        }

        public void GenRawBalance(IList<Balance> lastBalances, IList<Purchase> purchases, FinanceCalendar financeCalendar, string userCode)
        {
            string fc = getfc(financeCalendar);
            #region RM�Ĳ��ϳɱ�=(���ڲɹ����+�ڳ������)/(�ɹ�����+�ڳ�����)
            foreach (Balance lastBalance in lastBalances)
            {
                Balance newBalance = new Balance();
                CloneHelper.CopyProperty(lastBalance, newBalance);
                var item_Purchase = purchases.Where(p => p.Item == lastBalance.Item);
                double purchaseAmount = item_Purchase.Sum(p => p.Amount);
                double purchaseQty = 0;
                double purchaseCost = 0;
                foreach (var purchase in item_Purchase)
                {
                    purchaseQty += (double)uomConversionMgr.ConvertUomQty(purchase.Item, purchase.Uom, (decimal)purchase.Qty, lastBalance.Uom);
                    newBalance.IsProvEst = purchase.IsProvEst;
                    purchaseCost = purchase.AvgPrice;
                }

                if (lastBalance.Qty + purchaseQty != 0)
                {
                    newBalance.Cost = (lastBalance.Amount + purchaseAmount) / (lastBalance.Qty + purchaseQty);
                }
                newBalance.Amount = newBalance.Cost * newBalance.Qty;
                newBalance.FinanceCalendar = fc;
                newBalance.CreateDate = DateTime.Now;
                newBalance.CreateUser = userCode;
                this.CreateBalance(newBalance);

                RawIOB rawIOB = new RawIOB();
                rawIOB.CreateDate = DateTime.Now;
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
                rawIOBMgr.CreateRawIOB(rawIOB);
            }

            //�³�û�е����ϳɱ�
            foreach (Purchase purchase in purchases)
            {
                if (lastBalances.Count(l => l.Item == purchase.Item) == 0)
                {
                    Item item = itemMgr.LoadItem(purchase.Item);
                    if (item != null)
                    {
                        double purchaseQty = (double)uomConversionMgr.ConvertUomQty(purchase.Item, purchase.Uom, (decimal)purchase.Qty, item.Uom.Code);
                        Balance newBalance = new Balance();
                        newBalance.Amount = purchase.Amount;
                        newBalance.Cost = purchase.AvgPrice;
                        newBalance.CreateDate = DateTime.Now;
                        newBalance.CreateUser = userCode;
                        newBalance.FinanceCalendar = fc;
                        newBalance.Item = purchase.Item;
                        newBalance.ItemCategory = item.ItemCategory.Code;
                        newBalance.Qty = purchaseQty;
                        newBalance.Uom = item.Uom.Code;
                        newBalance.IsProvEst = purchase.IsProvEst;
                        this.CreateBalance(newBalance);

                        RawIOB rawIOB = new RawIOB();
                        rawIOB.CreateDate = DateTime.Now;
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
                        rawIOBMgr.CreateRawIOB(rawIOB);
                    }
                }
            }
            log.Info("7.RM�Ĳ��ϳɱ�=(���ڲɹ����+�ڳ������)/(�ɹ�����+�ڳ�����)");
            #endregion
        }

        public void GenFGCost(IList<Cbom> cboms, IList<LocationTransaction> fgOutPuts, IList<Consume> consumes, IList<Balance> balances, IList<FgCost> fgCosts, FinanceCalendar financeCalendar, string userCode)
        {
            var groupBom = from b in cboms
                           group b by b.Bom into result
                           select new
                           {
                               Bom = result.Key,
                               BomDetails = cboms.Where(c => c.Bom == result.Key)
                           };

            foreach (var bomItem in groupBom)
            {
                Item item = itemMgr.LoadItem(bomItem.Bom);
                if (item != null)
                {
                    var fgOutPut = fgOutPuts.SingleOrDefault(f => f.Item == bomItem.Bom);
                    fgOutPut = fgOutPut == null ? new LocationTransaction() : fgOutPut;
                    double fgOutQty = Convert.ToDouble(fgOutPut.Qty);
                    //��Ʒ��ķ�̯
                    var consume = consumes.SingleOrDefault(f => f.Item == bomItem.Bom);
                    consume = consume == null ? new Consume() : consume;
                    FgCost fgCost = new FgCost();
                    foreach (var bomdetail in bomItem.BomDetails)
                    {
                        if (bomdetail.ItemCategory.Contains("RM"))
                        {
                            var balance = balances.FirstOrDefault(b => b.Item == bomdetail.Item);
                            balance = balance == null ? new Balance() : balance;
                            bomdetail.IsProvEst = balance.IsProvEst;
                            //�ܳɱ�
                            bomdetail.Cost = bomdetail.AccumQty * balance.Cost;
                            //Ͷ������
                            bomdetail.InQty = bomdetail.AccumQty * fgOutQty;
                            //��Ʒ�������ɢ��ԭ�������¼���
                            bomdetail.ScraptQty += (bomdetail.AccumQty * (-consume.Qty));
                            //
                            bomdetail.Allocation += (bomdetail.AccumQty * (-consume.Qty) * bomdetail.Cost);
                            cbomMgr.UpdateCbom(bomdetail);
                            fgCost.Cost += bomdetail.Cost;
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
                    fgCostMgr.CreateFgCost(fgCost);
                }
            }
            log.Info("9.��Ʒ�ɱ� GenCBom And FGCost");
        }
*/
        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class BomTreeMgrE : com.Sconit.Service.Cost.Impl.BomTreeMgr, IBomTreeMgrE
    {
    }
}

#endregion Extend Class