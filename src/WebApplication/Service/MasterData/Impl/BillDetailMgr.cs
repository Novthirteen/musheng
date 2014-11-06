using com.Sconit.Service.Ext.MasterData;


using System;
using System.Linq;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using System.Collections.Generic;
using com.Sconit.Service.Ext.Hql;
using System.Text;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class BillDetailMgr : BillDetailBaseMgr, IBillDetailMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public IHqlMgrE hqlMgrE { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public override void CreateBillDetail(BillDetail entity)
        {
            entity.Amount = Math.Round(entity.Amount, 2, MidpointRounding.AwayFromZero);
            base.CreateBillDetail(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BillDetail> GetBillDetail(string billNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For<BillDetail>();
            criteria.Add(Expression.Eq("Bill.BillNo", billNo));

            return this.criteriaMgrE.FindAll<BillDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BillDetail> GetBillDetailOrderByItem(string billNo)
        {
            return this.GetBillDetailOrderByItem(billNo, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BillDetail> GetBillDetailOrderByItem(string billNo, bool isGroup)
        {
            StringBuilder hql = new StringBuilder();

            hql.Append(@"SELECT   bd from BillDetail bd,InProcessLocation pl ");
            hql.Append(@"WHERE    pl.IpNo = bd.IpNo ");
            hql.Append(@"AND      bd.Bill.BillNo = :BillNo ");
            hql.Append(@"ORDER BY bd.ActingBill.Item.Code ASC , ");
            hql.Append(@"         pl.CreateDate ASC ");

            IDictionary<string, object> param = new Dictionary<string, object>();
            param.Add("BillNo", billNo);

            IList<BillDetail> billDetails = hqlMgrE.FindAll<BillDetail>(hql.ToString(), param);
            if (isGroup)
            {
                billDetails = GroupBillDetail(billDetails);
            }
            return billDetails;
        }

        [Transaction(TransactionMode.Unspecified)]
        public BillDetail TransferAtingBill2BillDetail(ActingBill actingBill)
        {
            EntityPreference entityPreference = this.entityPreferenceMgrE.LoadEntityPreference(
                BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
            int amountDecimalLength = int.Parse(entityPreference.Value);
            BillDetail billDetail = new BillDetail();
            billDetail.ActingBill = actingBill;

            billDetail.Currency = actingBill.Currency;
            billDetail.IsIncludeTax = actingBill.IsIncludeTax;
            billDetail.TaxCode = actingBill.TaxCode;
            billDetail.UnitPrice = actingBill.UnitPrice;
            billDetail.ListPrice = actingBill.ListPrice;
            billDetail.BilledQty = actingBill.CurrentBillQty;
            billDetail.Amount = actingBill.CurrentBillAmount;
            billDetail.Discount = actingBill.CurrentDiscount;
            billDetail.HeadDiscount = actingBill.CurrentHeadDiscount;
            billDetail.LocationFrom = actingBill.LocationFrom;
            billDetail.IpNo = actingBill.IpNo;
            billDetail.ReferenceItemCode = actingBill.ReferenceItemCode;
            billDetail.FlowCode = actingBill.FlowCode;
            billDetail.CostCenter = actingBill.CostCenter;
            billDetail.CostGroup = actingBill.CostGroup;
            if (actingBill.CurrentBillQty != (actingBill.BillQty - actingBill.BilledQty))
            {
                //本次开票数量大于剩余数量
                if (actingBill.CurrentBillQty > (actingBill.BillQty - actingBill.BilledQty))
                {
                    throw new BusinessErrorException("ActingBill.Error.CurrentBillQtyGeRemainQty");
                }

            }

            if (actingBill.CurrentBillAmount != (actingBill.BillAmount - actingBill.BilledAmount))
            {
                //本次开票金额大于剩余金额
                if (actingBill.CurrentBillAmount > (actingBill.BillAmount - actingBill.BilledAmount))
                {
                    throw new BusinessErrorException("ActingBill.Error.CurrentBillAmountGeRemainAmount");
                }

            }

            return billDetail;
        }
        [Transaction(TransactionMode.Unspecified)]
        public IList<BillDetail> GroupBillDetail(IList<BillDetail> billDetails)
        {
            return (from b in billDetails
                    group b by new { b.ActingBill.Item, ReferenceItemCode = (b.ActingBill.ReferenceItemCode == null ? string.Empty : b.ActingBill.ReferenceItemCode), b.ActingBill.Uom, b.UnitPrice, b.Currency } into g
                    select new BillDetail
                    {
                        Item = g.Key.Item,
                        ReferenceItemCode = g.Key.ReferenceItemCode,
                        Uom = g.Key.Uom,
                        UnitPrice = g.Key.UnitPrice,
                        Currency = g.Key.Currency,
                        BillQty = g.Sum(b => b.ActingBill.BillQty),
                        BilledQty = g.Sum(b => b.BilledQty),
                        GroupAmount = g.Sum(b => b.Amount) - g.Sum(b => b.Discount.HasValue ? b.Discount.Value : 0)
                    }).ToList();
        }

        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class BillDetailMgrE : com.Sconit.Service.MasterData.Impl.BillDetailMgr, IBillDetailMgrE
    {

    }
}
#endregion
