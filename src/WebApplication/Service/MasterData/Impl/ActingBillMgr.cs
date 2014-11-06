using System;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ActingBillMgr : ActingBillBaseMgr, IActingBillMgr
    {

        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        //public IBillTransactionMgrE billTransactionMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IPriceListDetailMgrE priceListDetailMgrE { get; set; }
        public ICostMgrE costMgr { get; set; }
        public ITaxRateMgrE taxRateMgr { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public void ReverseUpdateActingBill(BillDetail oldBillDetail, BillDetail newBillDetail, User user)
        {
            if (oldBillDetail != null && newBillDetail != null
                && oldBillDetail.ActingBill.Id != newBillDetail.ActingBill.Id)
            {
                throw new TechnicalException("oldBillDetail.ActingBill.Id != newBillDetail.ActingBill.Id when ReverseUpdateActingBill");
            }

            DateTime dateTimeNow = DateTime.Now;
            #region 扣减旧BillDetail的数量和金额
            if (oldBillDetail != null)
            {
                //todo 校验数量、金额
                ActingBill actingBill = this.LoadActingBill(oldBillDetail.ActingBill.Id);
                actingBill.BilledQty -= oldBillDetail.BilledQty;
                actingBill.BilledAmount -= oldBillDetail.Amount;
                actingBill.LastModifyDate = dateTimeNow;
                actingBill.LastModifyUser = user;

                if ((actingBill.BillQty > 0 && actingBill.BillQty < actingBill.BilledQty)
                    || (actingBill.BillQty < 0 && actingBill.BillQty > actingBill.BilledQty))
                {
                    throw new BusinessErrorException("ActingBill.Error.CurrentBillQtyGeRemainQty");
                }

                if ((actingBill.BillAmount > 0 && actingBill.BillAmount < actingBill.BilledAmount)
                    || (actingBill.BillAmount < 0 && actingBill.BillAmount > actingBill.BilledAmount))
                {
                    throw new BusinessErrorException("ActingBill.Error.CurrentBillAmountGeRemainAmount");
                }

                //if ((actingBill.BillQty == actingBill.BillQty && actingBill.BillAmount != actingBill.BilledAmount)
                //    || (actingBill.BillQty != actingBill.BillQty && actingBill.BillAmount == actingBill.BilledAmount))
                //{
                //    throw new BusinessErrorException("ActingBill.Error.CurrentBillQtyAndBillAmountMustBeZero");
                //}

                if (actingBill.BillQty == actingBill.BilledQty)
                {
                    actingBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                }
                else
                {
                    actingBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
                }
                this.UpdateActingBill(actingBill);
            }
            #endregion

            #region 增加新BillDetail的数量和金额
            if (newBillDetail != null)
            {
                //todo 校验数量、金额
                ActingBill actingBill = this.LoadActingBill(newBillDetail.ActingBill.Id);             

                actingBill.BilledQty += newBillDetail.BilledQty;
                actingBill.BilledAmount += newBillDetail.Amount;
                actingBill.LastModifyDate = dateTimeNow;
                actingBill.LastModifyUser = user;
                if ((actingBill.BillQty > 0 && actingBill.BillQty < actingBill.BilledQty)
                    || (actingBill.BillQty < 0 && actingBill.BillQty > actingBill.BilledQty))
                {
                    throw new BusinessErrorException("ActingBill.Error.CurrentBillQtyGeRemainQty");
                }

                //if ((actingBill.BillAmount > 0 && actingBill.BillAmount < actingBill.BilledAmount)
                //   || (actingBill.BillAmount < 0 && actingBill.BillAmount > actingBill.BilledAmount))
                //{
                //    throw new BusinessErrorException("ActingBill.Error.CurrentBillAmountGeRemainAmount");
                //}


                //if ((actingBill.BillQty == actingBill.BillQty && actingBill.BillAmount != actingBill.BilledAmount)
                //   || (actingBill.BillQty != actingBill.BillQty && actingBill.BillAmount == actingBill.BilledAmount))
                //{
                //    throw new BusinessErrorException("ActingBill.Error.CurrentBillQtyAndBillAmountMustBeZero");
                //}

                if (actingBill.BillQty == actingBill.BilledQty)
                {
                    actingBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                }
                else
                {
                    actingBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
                }
                this.UpdateActingBill(actingBill);
            }
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public void RecalculatePrice(IList<ActingBill> actingBillList, User user)
        {
            this.RecalculatePrice(actingBillList, user, null);
        }

        [Transaction(TransactionMode.Requires)]
        public void RecalculatePrice(IList<ActingBill> actingBillList, User user, DateTime? efftiveDate)
        {
            if (actingBillList != null && actingBillList.Count > 0)
            {
                DateTime dateTimeNow = DateTime.Now;

                //if (!efftiveDate.HasValue)
                //{
                //    #region 查找结算日期
                //    //DetachedCriteria criteria = DetachedCriteria.For<BillTransaction>();
                //    //criteria.Add(Expression.Eq("ActingBill", actingBill.Id));

                //    //IList<BillTransaction> billTransactionList = billTransactionMgrE.GetAllBillTransaction();
                //    //if (billTransactionList != null && billTransactionList.Count > 0)
                //    //{
                //    //    efftiveDate = billTransactionList[0].EffectiveDate;
                //    //}
                //    //else
                //    //{
                //        //没有找到结算事务，用当前时间去找价格
                //        //efftiveDate = DateTime.Now;
                //    //}
                //    #endregion
                //}

                foreach (ActingBill actingBill in actingBillList)
                {
                    PriceListDetail priceListDetail = null;
                    if (efftiveDate.HasValue)
                    {
                        priceListDetail = this.priceListDetailMgrE.GetLastestPriceListDetail(actingBill.PriceList, actingBill.Item, efftiveDate.Value, actingBill.Currency, actingBill.Uom);
                    }
                    else
                    {
                        priceListDetail = this.priceListDetailMgrE.GetLastestPriceListDetail(actingBill.PriceList, actingBill.Item, actingBill.EffectiveDate, actingBill.Currency, actingBill.Uom);
                    }

                    if (priceListDetail != null)
                    {
                        actingBill.IsIncludeTax = false; //待开票明细都是不含税金额
                        actingBill.TaxCode = priceListDetail.TaxCode;
                        actingBill.ListPrice = priceListDetail.UnitPrice;
                        actingBill.UnitPrice = priceListDetail.UnitPrice;
                        actingBill.IsProvisionalEstimate = priceListDetail.IsProvisionalEstimate;
                        actingBill.LastModifyDate = dateTimeNow;
                        actingBill.LastModifyUser = user;
                        //不计折扣
                        decimal oldBillAmount = actingBill.BillAmount;                        
                        actingBill.BillAmount = actingBill.UnitPrice * actingBill.BillQty;

                        if (priceListDetail.IsIncludeTax)   //如果价格单含税，待开票金额要转为不含税金额
                        {
                            EntityPreference entityPreference = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
                            int decimalLength = Convert.ToInt32(entityPreference.Value);
                            TaxRate taxRate = this.taxRateMgr.CheckAndLoadTaxRate(actingBill.TaxCode);
                            //actingBill.BillAmount = Math.Round(actingBill.BillAmount / (1 + taxRate.TaxRate), decimalLength, MidpointRounding.AwayFromZero);
                            actingBill.ListPrice = Math.Round(actingBill.ListPrice / (1 + taxRate.TaxRate), decimalLength, MidpointRounding.AwayFromZero);
                            actingBill.UnitPrice = Math.Round(actingBill.UnitPrice / (1 + taxRate.TaxRate), decimalLength, MidpointRounding.AwayFromZero);
                            actingBill.BillAmount = actingBill.UnitPrice * actingBill.BillQty;
                        }

                        if (actingBill.BillAmount != oldBillAmount)  //记录重新计价产生的价差
                        {
                            this.costMgr.RecordDiffProcurementCostTransaction(actingBill, actingBill.BillAmount - oldBillAmount, user);
                        }
                        this.UpdateActingBill(actingBill);
                    }
                }
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ActingBill> GetUnBilledActingBill(OrderHead orderHead)
        {
            return GetUnBilledActingBill(orderHead.OrderNo);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ActingBill> GetUnBilledActingBill(string orderNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ActingBill>();
            criteria.Add(Expression.Eq("OrderNo", orderNo));
            criteria.Add(Expression.NotEqProperty("BillQty", "BilledQty"));
            return this.criteriaMgrE.FindAll<ActingBill>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ActingBill> GetActingBill(string partyCode, string receiver, DateTime? effDateFrom, DateTime? effDateTo, string itemCode, string currency, string transType, string exceptBillNo)
        {
            return GetActingBill(partyCode, receiver, effDateFrom, effDateTo, itemCode, currency, transType, exceptBillNo, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ActingBill> GetActingBill(string partyCode, string receiver, DateTime? effDateFrom, DateTime? effDateTo, string itemCode, string currency, string transType, string exceptBillNo, bool? isProvisionalEstimate)
        {
            return GetActingBill(partyCode, receiver, effDateFrom, effDateTo, itemCode, currency, transType, exceptBillNo, isProvisionalEstimate, null, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ActingBill> GetActingBill(string partyCode, string receiver, DateTime? effDateFrom, DateTime? effDateTo, string itemCode, string currency, string transType, string exceptBillNo, bool? isProvisionalEstimate, string flowCode, string billAddress)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ActingBill>();

            criteria.CreateAlias("BillAddress", "ba");

            if (partyCode != null && partyCode != string.Empty)
            {
                criteria.Add(Expression.Eq("ba.Party.Code", partyCode));
            }

            if (receiver != null && receiver != string.Empty)
            {
                if (transType == BusinessConstants.BILL_TRANS_TYPE_PO)
                {
                    //采购查询收货单号
                    criteria.Add(Expression.Like("ReceiptNo", receiver, MatchMode.Anywhere));
                }
                else
                {
                    //销售查询客户回单号
                    criteria.Add(Expression.Like("ExternalReceiptNo", receiver, MatchMode.Anywhere));
                }
            }

            if (effDateFrom.HasValue)
            {
                criteria.Add(Expression.Ge("EffectiveDate", effDateFrom.Value));
            }

            if (effDateTo.HasValue)
            {
                criteria.Add(Expression.Le("EffectiveDate", effDateTo.Value));
            }

            if (itemCode != null && itemCode != string.Empty)
            {
                criteria.CreateAlias("Item", "i");
                criteria.Add(
                  Expression.Like("i.Code", itemCode, MatchMode.Anywhere) ||
                  Expression.Like("i.Desc1", itemCode, MatchMode.Anywhere) ||
                  Expression.Like("i.Desc2", itemCode, MatchMode.Anywhere)
                  );
                
            }

            if (currency != null && currency != string.Empty)
            {
                criteria.Add(Expression.Eq("Currency.Code", currency));
            }

            if (exceptBillNo != null && exceptBillNo != string.Empty)
            {
                DetachedCriteria bCriteria = DetachedCriteria.For<BillDetail>();
                bCriteria.Add(Expression.Eq("Bill.BillNo", exceptBillNo));

                IList<BillDetail> billDetailList = this.criteriaMgrE.FindAll<BillDetail>(bCriteria);

                if (billDetailList != null && billDetailList.Count > 0)
                {
                    List<int> idList = new List<int>();

                    foreach (BillDetail billDetail in billDetailList)
                    {
                        idList.Add(billDetail.ActingBill.Id);
                    }

                    criteria.Add(Expression.Not(Expression.In("Id", idList)));
                }
            }

            criteria.Add(Expression.Eq("TransactionType", transType));

            if (isProvisionalEstimate.HasValue)
            {
                criteria.Add(Expression.Eq("IsProvisionalEstimate", isProvisionalEstimate));   //非暂估价
            }

            criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));

            if (flowCode != null && flowCode != string.Empty)
            {
                criteria.Add(Expression.Eq("FlowCode", flowCode));
            }

            if (billAddress != null && billAddress != string.Empty)
            {
                criteria.Add(Expression.Eq("ba.Code", billAddress));
            }
            return this.criteriaMgrE.FindAll<ActingBill>(criteria);
        }

        #endregion Customized Methods
    }
}


#region Extend Class



namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ActingBillMgrE : com.Sconit.Service.MasterData.Impl.ActingBillMgr, IActingBillMgrE
    {


    }
}
#endregion
