using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.Procurement;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class PlannedBillMgr : PlannedBillBaseMgr, IPlannedBillMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public ICostCenterMgrE costCenterMgr { get; set; }
        public IOrderLocationTransactionMgrE orderLocationTransactionMgr { get; set; }
        public IPriceListDetailMgrE priceListDetailMgrE { get; set; }
        public IBomDetailMgrE bomDetailMgrE { get; set; }
        public IItemReferenceMgrE itemReferenceMgrE { get; set; }
        public ITaxRateMgrE taxRateMgr { get; set; }

        [Transaction(TransactionMode.Unspecified)]
        public IList<PlannedBill> GetUnSettledPlannedBill(OrderHead orderHead)
        {
            return this.GetUnSettledPlannedBill(orderHead.OrderNo);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<PlannedBill> GetUnSettledPlannedBill(string orderNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For<PlannedBill>();
            criteria.Add(Expression.Eq("OrderNo", orderNo));
            criteria.Add(Expression.NotEqProperty("PlannedQty", "ActingQty"));
            return this.criteriaMgrE.FindAll<PlannedBill>(criteria);
        }

        //[Transaction(TransactionMode.Requires)]
        //public IList<PlannedBill> CreatePlannedBill(ReceiptDetail receiptDetail, User user)
        //{
        //    Receipt receipt = receiptDetail.Receipt;

        //    OrderLocationTransaction orderLocationTransaction = receiptDetail.OrderLocationTransaction;
        //    OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
        //    OrderHead orderHead = orderDetail.OrderHead;

        //    IList<PlannedBill> plannedBills = new List<PlannedBill>();

        //    //if (orderDetail.Bom != null)
        //    //{
        //    //    IList<BomDetail> bomDetails = bomDetailMgrE.GetFlatBomDetail(orderDetail.Bom.Code, orderHead.CreateDate); // orderDetail.Bom.BomDetails; 
        //    //    foreach (BomDetail bomDetail in bomDetails)
        //    //    {
        //    //        PlannedBill plannedBill = CreatePlannedBill(receiptDetail, user, bomDetail);
        //    //        plannedBills.Add(plannedBill);
        //    //    }
        //    //}
        //    //else
        //    //{
        //        PlannedBill plannedBill = CreatePlannedBill(receiptDetail, user, null);
        //        plannedBills.Add(plannedBill);
        //    //}
        //    return plannedBills;
        //}

        #region Customized Methods
        [Transaction(TransactionMode.Requires)]
        public PlannedBill CreatePlannedBill(ReceiptDetail receiptDetail, User user)
        {
            Receipt receipt = receiptDetail.Receipt;

            OrderLocationTransaction orderLocationTransaction = receiptDetail.OrderLocationTransaction;
            OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
            OrderHead orderHead = orderDetail.OrderHead;

            //DateTime dateTimeNow = DateTime.Now;
            //decimal plannedAmount = 0;

            PlannedBill plannedBill = new PlannedBill();
            plannedBill.OrderNo = orderHead.OrderNo;
            #region 
            if (orderHead.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION
                && orderDetail.Remark != null
                && orderDetail.Remark.Trim() != string.Empty)
            {
                plannedBill.ExternalReceiptNo = orderDetail.Remark;
            }
            else
            {
                if (receipt.ExternalReceiptNo != null && receipt.ExternalReceiptNo.Trim() != string.Empty)
                {
                    plannedBill.ExternalReceiptNo = receipt.ExternalReceiptNo;        //记录客户回单号
                }
                else if (orderHead.ExternalOrderNo != null && orderHead.ExternalOrderNo.Trim() != string.Empty)
                {
                    plannedBill.ExternalReceiptNo = orderHead.ExternalOrderNo;
                }
                else
                {
                    plannedBill.ExternalReceiptNo = orderHead.ReferenceOrderNo;
                }
            }
            #endregion
            plannedBill.ReceiptNo = receipt.ReceiptNo;
            plannedBill.Item = orderDetail.Item;
            plannedBill.SettleTerm = orderDetail.DefaultBillSettleTerm;
            plannedBill.PlannedQty = receiptDetail.ReceivedQty;         //设置待结算数量默认值
            plannedBill.Uom = orderDetail.Uom;                                                  //单位为订单单位
            plannedBill.UnitCount = orderDetail.UnitCount;
            plannedBill.UnitQty = orderLocationTransaction.UnitQty;                                 //UnitQty沿用OrderLocationTransaction
            plannedBill.CreateDate = receipt.CreateDate;
            plannedBill.CreateUser = user;
            plannedBill.LastModifyDate = receipt.CreateDate;
            plannedBill.LastModifyUser = user;
            plannedBill.IsAutoBill = orderHead.IsAutoBill;
            plannedBill.HuId = receiptDetail.HuId;
            plannedBill.LotNo = receiptDetail.LotNo;
            plannedBill.FlowCode = orderHead.Flow;            
            plannedBill.IpNo = receipt.ReferenceIpNo;
            plannedBill.ReferenceItemCode = orderDetail.ReferenceItemCode;

            plannedBill.BillAddress = orderDetail.DefaultBillAddress;
            plannedBill.PriceList = orderDetail.DefaultPriceList;
            plannedBill.IsProvisionalEstimate =      //暂估价格处理，没有找到价格也认为是暂估价格
                     orderDetail.UnitPrice.HasValue ? orderDetail.IsProvisionalEstimate : true;
            plannedBill.ListPrice = orderDetail.UnitPrice.HasValue ? orderDetail.UnitPrice.Value : 0;
            plannedBill.UnitPrice = orderDetail.UnitPriceAfterDiscount.HasValue ? orderDetail.UnitPriceAfterDiscount.Value : 0;
            plannedBill.PlannedAmount = plannedBill.UnitPrice * plannedBill.PlannedQty;
            if (orderDetail.IsIncludeTax)
            {
                EntityPreference entityPreference = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
                int decimalLength = Convert.ToInt32(entityPreference.Value);
                TaxRate taxRate = this.taxRateMgr.CheckAndLoadTaxRate(orderDetail.TaxCode);
                plannedBill.ListPrice = Math.Round(plannedBill.ListPrice / (1 + taxRate.TaxRate), decimalLength, MidpointRounding.AwayFromZero);
                plannedBill.UnitPrice = Math.Round(plannedBill.UnitPrice / (1 + taxRate.TaxRate), decimalLength, MidpointRounding.AwayFromZero);
                plannedBill.PlannedAmount = plannedBill.UnitPrice * plannedBill.PlannedQty;
                //plannedBill.PlannedAmount = Math.Round(plannedBill.PlannedAmount / (1 + taxRate.TaxRate), decimalLength, MidpointRounding.AwayFromZero);
            }
            plannedBill.Currency = orderDetail.OrderHead.Currency;
            plannedBill.IsIncludeTax = false;
            plannedBill.TaxCode = orderDetail.TaxCode;
            if (orderDetail.OrderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                || orderDetail.OrderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING)
            {

                plannedBill.TransactionType = BusinessConstants.BILL_TRANS_TYPE_PO;
               
                #region 采购记录入库的成本中心
                plannedBill.LocationFrom = orderLocationTransaction.Location.Code;
                plannedBill.CostCenter = orderLocationTransaction.Location.Region.CostCenter;
                plannedBill.CostGroup = costCenterMgr.CheckAndLoadCostCenter(plannedBill.CostCenter).CostGroup.Code;
                #endregion
            }
            else if (orderDetail.OrderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                plannedBill.TransactionType = BusinessConstants.BILL_TRANS_TYPE_SO;
               
                #region 销售记录出库的成本中心
                OrderLocationTransaction outOrderLocTrans = this.orderLocationTransactionMgr.GetOrderLocationTransaction(orderLocationTransaction.OrderDetail.Id, BusinessConstants.IO_TYPE_OUT)[0];
                plannedBill.LocationFrom = outOrderLocTrans.Location.Code;
                plannedBill.CostCenter = outOrderLocTrans.Location.Region.CostCenter;
                plannedBill.CostGroup = costCenterMgr.CheckAndLoadCostCenter(plannedBill.CostCenter).CostGroup.Code;
                #endregion
            }
            else
            {
                throw new TechnicalException("Only SO and PO/SubContract can create planned bill.");
            }

            ////////////////////////
            //if (bomDetail != null)
            //{
            //    plannedBill.Item = bomDetail.Item;
            //    decimal receivedQty = receiptDetail.ReceivedQty.HasValue ? receiptDetail.ReceivedQty.Value : 0;         //设置待结算数量默认值
            //    plannedBill.Uom = bomDetail.Uom == null ? bomDetail.Item.Uom : bomDetail.Uom;                   //单位为订单单位
            //    plannedBill.UnitCount = bomDetail.Item.UnitCount;
            //    plannedBill.UnitQty = bomDetail.RateQty;                //UnitQty沿用OrderLocationTransaction
            //    plannedBill.PlannedQty = receivedQty * plannedBill.UnitQty;
            //    plannedBill.ReferenceItemCode = itemReferenceMgrE.GetItemReferenceByItem(plannedBill.Item.Code, orderHead.PartyTo.Code, orderHead.PartyFrom.Code);
            //}
            
            //if (orderDetail.Uom.Code != plannedBill.Uom.Code)
            //{
            //    //订单单位和采购单位不一致，需要更改UnitQty和PlannedQty值
            //    plannedBill.UnitQty = this.uomConversionMgrE.ConvertUomQty(orderDetail.Item, orderDetail.Uom, plannedBill.UnitQty, plannedBill.Uom);
            //    plannedBill.PlannedQty = plannedBill.PlannedQty * plannedBill.UnitQty;
            //}

            this.CreatePlannedBill(plannedBill);

            return plannedBill;
        }

        #endregion Customized Methods

    }
}


#region 扩展










namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class PlannedBillMgrE : com.Sconit.Service.MasterData.Impl.PlannedBillMgr, IPlannedBillMgrE
    {

    }
}
#endregion
