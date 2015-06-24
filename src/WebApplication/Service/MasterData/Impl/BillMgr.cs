using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Cost;
using System.Linq;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{

    [Transactional]
    public class BillMgr : BillBaseMgr, IBillMgr
    {
        private string[] BillCloneField = new string[] 
            { 
                "ExternalBillNo",
                "TransactionType",
                "BillAddress",
                "Currency",
                "IsIncludeTax",
                "TaxCode"
            };

        private string[] BillDetailCloneField = new string[] 
            { 
                "ActingBill",
                "UnitPrice",
                "Currency",
                "IsIncludeTax",
                "TaxCode"
            };

        private string[] PlannedBill2ActingBillCloneField = new string[] {
            "OrderNo",
            "ExternalReceiptNo",
            "ReceiptNo",
            "TransactionType",
            "Item",
            "BillAddress",
            "Uom",
            "UnitCount",
            "UnitPrice",
            "PriceList",
            "Currency",
            "IsIncludeTax",
            "TaxCode",
            "IsProvisionalEstimate",
            "ReferenceItemCode",
            "LocationFrom",
            "IpNo",
            "FlowCode",
            "CostCenter",
            "CostGroup",
            "ListPrice"
        };

        public IActingBillMgrE actingBillMgrE { get; set; }
        public INumberControlMgrE numberControlMgrE { get; set; }
        public IBillDetailMgrE billDetailMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public IPlannedBillMgrE plannedBillMgrE { get; set; }
        public IBillTransactionMgrE billTransactionMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public ICostMgrE costMgr { get; set; }


        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public Bill CheckAndLoadBill(string billNo)
        {
            Bill bill = this.LoadBill(billNo);
            if (bill != null)
            {
                return bill;
            }
            else
            {
                throw new BusinessErrorException("Bill.Error.BillNoNotExist", billNo);
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public Bill CheckAndLoadBill(string billNo, bool includeBillDetail)
        {
            Bill bill = this.CheckAndLoadBill(billNo);

            if (includeBillDetail)
            {
                if (bill.BillDetails != null && bill.BillDetails.Count > 0)
                {
                }
            }

            return bill;

        }
        [Transaction(TransactionMode.Unspecified)]
        public Bill LoadBill(string billNo, bool includeBillDetail)
        {
            return LoadBill(billNo, includeBillDetail, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Bill LoadBill(string billNo, bool includeBillDetail, bool isGroup)
        {
            Bill bill = this.LoadBill(billNo);

            if (includeBillDetail)
            {
                if (bill.BillDetails != null && bill.BillDetails.Count > 0)
                {
                    if (isGroup)
                    {
                        bill.BillDetails = this.billDetailMgrE.GroupBillDetail(bill.BillDetails);
                    }
                }
            }

            return bill;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Bill> CreateBill(IList<ActingBill> actingBillList, User user)
        {
            return this.CreateBill(actingBillList, user, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, 0);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Bill> CreateBill(IList<ActingBill> actingBillList, User user, DateTime startDate, DateTime endDate)
        {
            return this.CreateBill(actingBillList, user, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, 0, startDate, endDate);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Bill> CreateBill(IList<ActingBill> actingBillList, string userCode)
        {
            return this.CreateBill(actingBillList, userCode, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, 0);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Bill> CreateBill(IList<ActingBill> actingBillList, User user, string status)
        {
            return this.CreateBill(actingBillList, user, status, 0);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Bill> CreateBill(IList<ActingBill> actingBillList, string userCode, string status)
        {
            return this.CreateBill(actingBillList, userCode, status, 0);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Bill> CreateBill(IList<ActingBill> actingBillList, User user, string status, decimal headDiscount)
        {
            DateTime now = DateTime.Now;
            return this.CreateBill(actingBillList, user, status, headDiscount, now, now);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Bill> CreateBill(IList<ActingBill> actingBillList, User user, string status, decimal headDiscount, DateTime startDate, DateTime endDate)
        {
            if (status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE
                && status != BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT
                && status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
            {
                throw new TechnicalException("status specified is not valided");
            }

            if (actingBillList == null || actingBillList.Count == 0)
            {
                throw new BusinessErrorException("Bill.Error.EmptyBillDetails");
            }

            DateTime dateTimeNow = DateTime.Now;
            IList<Bill> billList = new List<Bill>();

            #region 计算头折扣分配率
            decimal totalAmount = actingBillList.Sum(p => p.CurrentBillAmount);
            decimal headDiscountAssignRate = totalAmount != 0 ? (headDiscount / totalAmount) : 0;
            #endregion

            decimal remainDiscount = headDiscount;
            foreach (ActingBill actingBill in actingBillList)
            {
                ActingBill oldActingBill = this.actingBillMgrE.LoadActingBill(actingBill.Id);
                oldActingBill.CurrentBillQty = actingBill.CurrentBillQty;
                oldActingBill.CurrentBillAmount = actingBill.CurrentBillAmount;
                oldActingBill.CurrentDiscount = actingBill.CurrentDiscount;
                if (actingBillList.IndexOf(actingBill) != (actingBillList.Count - 1))
                {
                    oldActingBill.CurrentHeadDiscount = headDiscountAssignRate * actingBill.CurrentBillAmount;
                    remainDiscount -= oldActingBill.CurrentHeadDiscount;
                }
                else
                {
                    oldActingBill.CurrentHeadDiscount = remainDiscount;
                    remainDiscount = 0;
                }

                ////检查ActingBill的剩余待开数量是否为0
                //if (oldActingBill.Qty - oldActingBill.BilledQty == 0)
                //{
                //    throw new BusinessErrorException("Bill.Create.Error.ZeroActingBillRemainQty");
                //}

                Bill bill = null;

                #region 查找和待开明细的transactionType、billAddress、currency一致的BillMstr
                foreach (Bill thisBill in billList)
                {
                    if (thisBill.TransactionType == oldActingBill.TransactionType
                        && thisBill.BillAddress.Code == oldActingBill.BillAddress.Code
                        && thisBill.Currency.Code == oldActingBill.Currency.Code)
                    {
                        bill = thisBill;
                        break;
                    }
                }
                #endregion

                #region 没有找到匹配的Bill，新建
                if (bill == null)
                {
                    #region 检查权限
                    bool hasPermission = false;
                    foreach (Permission permission in user.OrganizationPermission)
                    {
                        if (permission.Code == oldActingBill.BillAddress.Party.Code)
                        {
                            hasPermission = true;
                            break;
                        }
                    }

                    if (!hasPermission)
                    {
                        throw new BusinessErrorException("Bill.Create.Error.NoAuthrization", oldActingBill.BillAddress.Party.Code);
                    }
                    #endregion

                    #region 创建Bill
                    bill = new Bill();
                    bill.BillNo = numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_BILL);
                    bill.Status = status;
                    bill.TransactionType = oldActingBill.TransactionType;
                    if (bill.TransactionType == BusinessConstants.BILL_TRANS_TYPE_PO)
                    {
                        bill.StartDate = startDate;
                        bill.EndDate = endDate;
                    }
                    bill.BillAddress = oldActingBill.BillAddress;
                    bill.Currency = oldActingBill.Currency;
                    bill.Discount = headDiscount;
                    bill.BillType = BusinessConstants.CODE_MASTER_BILL_TYPE_VALUE_NORMAL;
                    bill.CreateDate = dateTimeNow;
                    bill.CreateUser = user;
                    bill.LastModifyDate = dateTimeNow;
                    bill.LastModifyUser = user;

                    this.CreateBill(bill);
                    billList.Add(bill);
                    #endregion
                }
                #endregion

                BillDetail billDetail = this.billDetailMgrE.TransferAtingBill2BillDetail(oldActingBill);
                billDetail.Bill = bill;
                bill.AddBillDetail(billDetail);

                this.billDetailMgrE.CreateBillDetail(billDetail);
                //扣减ActingBill数量和金额
                this.actingBillMgrE.ReverseUpdateActingBill(null, billDetail, user);
            }

            return billList;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Bill> CreateBill(IList<ActingBill> actingBillList, string userCode, string status, decimal headDiscount)
        {
            return this.CreateBill(actingBillList, this.userMgrE.CheckAndLoadUser(userCode), status, headDiscount);
        }

        [Transaction(TransactionMode.Requires)]
        public void AddBillDetail(string billNo, IList<ActingBill> actingBillList, string userCode)
        {
            this.AddBillDetail(billNo, actingBillList, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void AddBillDetail(string billNo, IList<ActingBill> actingBillList, User user)
        {
            Bill oldBill = this.CheckAndLoadBill(billNo, true);

            #region 检查状态
            if (oldBill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                throw new BusinessErrorException("Bill.Error.StatusErrorWhenAddDetail", oldBill.Status, oldBill.BillNo);
            }
            #endregion

            if (actingBillList != null && actingBillList.Count > 0)
            {
                foreach (ActingBill actingBill in actingBillList)
                {
                    ActingBill oldActingBill = this.actingBillMgrE.LoadActingBill(actingBill.Id);
                    oldActingBill.CurrentBillQty = actingBill.CurrentBillQty;
                    oldActingBill.CurrentDiscount = actingBill.CurrentDiscount;
                    oldActingBill.CurrentBillAmount = actingBill.CurrentBillAmount;
                    BillDetail billDetail = this.billDetailMgrE.TransferAtingBill2BillDetail(oldActingBill);
                    billDetail.Bill = oldBill;
                    oldBill.AddBillDetail(billDetail);

                    this.billDetailMgrE.CreateBillDetail(billDetail);
                    //扣减ActingBill数量和金额
                    this.actingBillMgrE.ReverseUpdateActingBill(null, billDetail, user);
                }

                oldBill.LastModifyDate = DateTime.Now;
                oldBill.LastModifyUser = user;

                this.UpdateBill(oldBill);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void AddBillDetail(Bill bill, IList<ActingBill> actingBillList, string userCode)
        {
            this.AddBillDetail(bill.BillNo, actingBillList, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void AddBillDetail(Bill bill, IList<ActingBill> actingBillList, User user)
        {
            this.AddBillDetail(bill.BillNo, actingBillList, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteBillDetail(IList<BillDetail> billDetailList, string userCode)
        {
            this.DeleteBillDetail(billDetailList, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteBillDetail(IList<BillDetail> billDetailList, User user)
        {
            if (billDetailList != null && billDetailList.Count > 0)
            {
                IDictionary<string, Bill> cachedBillDic = new Dictionary<string, Bill>();

                foreach (BillDetail billDetail in billDetailList)
                {
                    BillDetail oldBillDetail = this.billDetailMgrE.LoadBillDetail(billDetail.Id);
                    Bill bill = oldBillDetail.Bill;

                    #region 缓存Bill
                    if (!cachedBillDic.ContainsKey(bill.BillNo))
                    {
                        cachedBillDic.Add(bill.BillNo, bill);

                        #region 检查状态
                        if (bill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
                        {
                            throw new BusinessErrorException("Bill.Error.StatusErrorWhenDeleteDetail", bill.Status, bill.BillNo);
                        }
                        #endregion
                    }
                    #endregion

                    //扣减ActingBill数量和金额
                    this.actingBillMgrE.ReverseUpdateActingBill(oldBillDetail, null, user);

                    this.billDetailMgrE.DeleteBillDetail(oldBillDetail);
                    //bill.RemoveBillDetail(oldBillDetail);
                }

                #region 更新Bill
                DateTime dateTimeNow = DateTime.Now;
                foreach (Bill bill in cachedBillDic.Values)
                {
                    bill.LastModifyDate = dateTimeNow;
                    bill.LastModifyUser = user;

                    this.UpdateBill(bill);
                }
                #endregion
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteBill(string billNo, string userCode)
        {
            this.DeleteBill(billNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteBill(string billNo, User user)
        {
            Bill oldBill = this.CheckAndLoadBill(billNo, true);

            #region 检查状态
            if (oldBill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                throw new BusinessErrorException("Bill.Error.StatusErrorWhenDelete", oldBill.Status, oldBill.BillNo);
            }
            #endregion

            if (oldBill.BillDetails != null && oldBill.BillDetails.Count > 0)
            {
                foreach (BillDetail billDetail in oldBill.BillDetails)
                {
                    BillDetail oldBillDetail = this.billDetailMgrE.LoadBillDetail(billDetail.Id);
                    //扣减ActingBill数量和金额
                    this.actingBillMgrE.ReverseUpdateActingBill(oldBillDetail, null, user);

                    this.billDetailMgrE.DeleteBillDetail(oldBillDetail);
                    //oldBill.RemoveBillDetail(billDetail);
                }
            }

            this.DeleteBill(oldBill);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteBill(Bill bill, string userCode)
        {
            this.DeleteBill(bill.BillNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteBill(Bill bill, User user)
        {
            this.DeleteBill(bill.BillNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateBill(Bill bill, string userCode)
        {
            this.UpdateBill(bill, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateBill(Bill bill, User user)
        {
            Bill oldBill = this.CheckAndLoadBill(bill.BillNo, true);


            #region 检查状态
            if (oldBill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                throw new BusinessErrorException("Bill.Error.StatusErrorWhenUpdate", oldBill.Status, oldBill.BillNo);
            }
            #endregion

            oldBill.Discount = bill.Discount;
            oldBill.ExternalBillNo = bill.ExternalBillNo;
            oldBill.StartDate = bill.StartDate;
            oldBill.EndDate = bill.EndDate;

            #region 更新明细
            if (bill.BillDetails != null && bill.BillDetails.Count > 0)
            {
                foreach (BillDetail newBillDetail in bill.BillDetails)
                {
                    BillDetail oldBillDetail = this.billDetailMgrE.LoadBillDetail(newBillDetail.Id);
                    newBillDetail.ActingBill = oldBillDetail.ActingBill;
                    //newBillDetail.Amount = oldBillDetail.Amount;
                    newBillDetail.UnitPrice = oldBillDetail.UnitPrice;
                    newBillDetail.Currency = oldBillDetail.Currency;
                    newBillDetail.IsIncludeTax = oldBillDetail.IsIncludeTax;
                    newBillDetail.TaxCode = oldBillDetail.TaxCode;

                    //反向更新ActingBill，会重新计算开票金额
                    if (oldBillDetail.BilledQty != newBillDetail.BilledQty)
                    {
                        this.actingBillMgrE.ReverseUpdateActingBill(oldBillDetail, newBillDetail, user);
                    }

                    oldBillDetail.Amount = newBillDetail.Amount;
                    oldBillDetail.BilledQty = newBillDetail.BilledQty;
                    oldBillDetail.Discount = newBillDetail.Discount;

                    this.billDetailMgrE.UpdateBillDetail(oldBillDetail);
                }
            }
            #endregion

            oldBill.LastModifyUser = user;
            oldBill.LastModifyDate = DateTime.Now;

            this.UpdateBill(oldBill);
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseBill(string billNo, string userCode)
        {
            this.ReleaseBill(billNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseBill(string billNo, User user)
        {
            //modify by ljz start
            //Bill oldBill = this.CheckAndLoadBill(billNo);
            string[] group = billNo.Split(',');
            string billno = group[0];
            decimal taxamount = decimal.Parse(group[1]);
            Bill oldBill = this.CheckAndLoadBill(billno);

            #region 检查状态
            if (oldBill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                throw new BusinessErrorException("Bill.Error.StatusErrorWhenRelease", oldBill.Status, oldBill.BillNo);
            }
            #endregion

            #region 检查明细不能为空
            if (oldBill.BillDetails == null || oldBill.BillDetails.Count == 0)
            {
                throw new BusinessErrorException("Bill.Error.EmptyBillDetail", oldBill.BillNo);
            }
            #endregion

            #region 记录开票事务
            foreach (BillDetail billDetail in oldBill.BillDetails)
            {
                this.billTransactionMgrE.RecordBillTransaction(billDetail, user, false);
            }
            #endregion

            oldBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
            oldBill.LastModifyUser = user;
            oldBill.LastModifyDate = DateTime.Now;
            oldBill.TaxAmount = taxamount; //add by ljz

            this.UpdateBill(oldBill);

            this.SendBillEmail(oldBill, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseBill(Bill bill, string userCode)
        {
            this.ReleaseBill(bill.BillNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseBill(Bill bill, User user)
        {
            this.ReleaseBill(bill.BillNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void CancelBill(string billNo, string userCode)
        {
            this.CancelBill(billNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void CancelBill(string billNo, User user)
        {
            Bill oldBill = this.CheckAndLoadBill(billNo);

            #region 检查状态
            if (oldBill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                throw new BusinessErrorException("Bill.Error.StatusErrorWhenCancel", oldBill.Status, oldBill.BillNo);
            }
            #endregion

            if (oldBill.BillDetails != null && oldBill.BillDetails.Count > 0)
            {
                foreach (BillDetail newBillDetail in oldBill.BillDetails)
                {
                    BillDetail oldBillDetail = this.billDetailMgrE.LoadBillDetail(newBillDetail.Id);

                    //反向更新ActingBill
                    this.actingBillMgrE.ReverseUpdateActingBill(oldBillDetail, null, user);

                    #region 记录开票事务
                    this.billTransactionMgrE.RecordBillTransaction(oldBillDetail, user, true);
                    #endregion
                }
            }

            oldBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL;
            oldBill.LastModifyUser = user;
            oldBill.LastModifyDate = DateTime.Now;

            this.UpdateBill(oldBill);
        }

        [Transaction(TransactionMode.Requires)]
        public void CancelBill(Bill bill, string userCode)
        {
            this.CancelBill(bill.BillNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void CancelBill(Bill bill, User user)
        {
            this.CancelBill(bill.BillNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void CloseBill(string billNo, string userCode)
        {
            this.CloseBill(billNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void CloseBill(string billNo, User user)
        {
            Bill oldBill = this.CheckAndLoadBill(billNo);

            #region 检查状态
            if (oldBill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                throw new BusinessErrorException("Bill.Error.StatusErrorWhenClose", oldBill.Status, oldBill.BillNo);
            }
            #endregion

            oldBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
            oldBill.LastModifyUser = user;
            oldBill.LastModifyDate = DateTime.Now;

            this.UpdateBill(oldBill);
        }

        [Transaction(TransactionMode.Requires)]
        public void CloseBill(Bill bill, string userCode)
        {
            this.CloseBill(bill.BillNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void CloseBill(Bill bill, User user)
        {
            this.CloseBill(bill.BillNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public Bill VoidBill(string billNo, string userCode)
        {
            return this.VoidBill(billNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public Bill VoidBill(string billNo, User user)
        {
            Bill oldBill = this.CheckAndLoadBill(billNo, true);
            DateTime dateTimeNow = DateTime.Now;

            #region 检查状态
            if (oldBill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
            {
                throw new BusinessErrorException("Bill.Error.StatusErrorWhenVoid", oldBill.Status, oldBill.BillNo);
            }
            #endregion

            #region 创建作废账单
            Bill voidBill = new Bill();
            CloneHelper.CopyProperty(oldBill, voidBill, this.BillCloneField);

            voidBill.BillNo = this.numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_BILL_RED);
            voidBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
            if (oldBill.Discount.HasValue)
            {
                voidBill.Discount = 0 - oldBill.Discount.Value;
            }
            voidBill.ReferenceBillNo = oldBill.BillNo;
            voidBill.BillType = BusinessConstants.CODE_MASTER_BILL_TYPE_VALUE_CANCEL;
            voidBill.CreateDate = dateTimeNow;
            voidBill.CreateUser = user;
            voidBill.LastModifyDate = dateTimeNow;
            voidBill.LastModifyUser = user;

            this.CreateBill(voidBill);
            #endregion

            #region 创建作废账单明细
            foreach (BillDetail oldBillDetail in oldBill.BillDetails)
            {
                BillDetail voidBillDetail = new BillDetail();
                CloneHelper.CopyProperty(oldBillDetail, voidBillDetail, this.BillDetailCloneField);
                voidBillDetail.BilledQty = 0 - oldBillDetail.BilledQty;
                voidBillDetail.Discount = 0 - oldBillDetail.Discount;
                voidBillDetail.Amount = 0 - oldBillDetail.Amount;
                voidBillDetail.Bill = voidBill;

                this.billDetailMgrE.CreateBillDetail(voidBillDetail);
                voidBill.AddBillDetail(voidBillDetail);

                //反向更新ActingBill
                this.actingBillMgrE.ReverseUpdateActingBill(null, voidBillDetail, user);
            }
            #endregion

            #region 记录开票事务
            foreach (BillDetail billDetail in oldBill.BillDetails)
            {
                this.billTransactionMgrE.RecordBillTransaction(billDetail, user, true);
            }
            #endregion

            #region 更新原账单
            oldBill.ReferenceBillNo = voidBill.BillNo;
            oldBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID;
            oldBill.LastModifyDate = dateTimeNow;
            oldBill.LastModifyUser = user;

            this.UpdateBill(oldBill);
            #endregion

            return voidBill;
        }

        [Transaction(TransactionMode.Requires)]
        public Bill VoidBill(Bill bill, string userCode)
        {
            return this.VoidBill(bill.BillNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public Bill VoidBill(Bill bill, User user)
        {
            return this.VoidBill(bill.BillNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<ActingBill> ManualCreateActingBill(IList<PlannedBill> plannedBillList, User user)
        {
            if (plannedBillList != null && plannedBillList.Count > 0)
            {
                IList<ActingBill> actingBillList = new List<ActingBill>();

                foreach (PlannedBill plannedBill in plannedBillList)
                {
                    ActingBill actingBill = ManualCreateActingBill(plannedBill, user);
                    actingBillList.Add(actingBill);
                    if (plannedBill.IsAutoBill)
                    {
                        actingBill.CurrentBillQty = actingBill.BillQty;
                        actingBill.CurrentDiscount = actingBill.UnitPrice * actingBill.BillQty - actingBill.BillAmount;
                        actingBillList.Add(actingBill);

                    }
                }

                //if (actingBillList.Count > 0)
                //{
                //    this.CreateBill(actingBillList, user);
                //}

                return actingBillList;
            }

            return null;
        }

        [Transaction(TransactionMode.Requires)]
        public ActingBill ManualCreateActingBill(PlannedBill plannedBill, User user)
        {
            return ManualCreateActingBill(plannedBill, null, user);
        }

        [Transaction(TransactionMode.Requires)]
        public ActingBill ManualCreateActingBill(PlannedBill plannedBill, LocationLotDetail locationLotDetail, User user)
        {
            IList<LocationLotDetail> locationLotDetailList = this.locationLotDetailMgrE.GetLocationLotDetail(plannedBill);

            if (locationLotDetailList != null && locationLotDetailList.Count > 0)
            {
                decimal actingQty = Math.Round(plannedBill.CurrentActingQty * plannedBill.UnitQty, 8);

                foreach (LocationLotDetail currentLocationLotDetail in locationLotDetailList)
                {
                    if (actingQty > 0)
                    {
                        #region 更新库存寄售标记
                        if (actingQty - currentLocationLotDetail.Qty >= 0)
                        {
                            actingQty -= currentLocationLotDetail.Qty;
                            currentLocationLotDetail.IsConsignment = false;
                            currentLocationLotDetail.PlannedBill = null;
                        }
                        else
                        {
                            //不支持同一库存记录进行部分结算
                            throw new BusinessErrorException("Location.Error.PlannedBill.CantSplitInventory");
                        }

                        this.locationLotDetailMgrE.UpdateLocationLotDetail(currentLocationLotDetail);
                        #endregion
                    }
                    else
                    {
                        break;
                    }
                }
            }

            #region 创建ActBill
            ActingBill actingBill = this.CreateActingBill(plannedBill, locationLotDetail, user);
            #endregion

            return actingBill;
        }

        [Transaction(TransactionMode.Requires)]
        public ActingBill CreateActingBill(PlannedBill plannedBill, User user)
        {
            return CreateActingBill(plannedBill, null, user);
        }

        [Transaction(TransactionMode.Requires)]
        public ActingBill CreateActingBill(PlannedBill plannedBill, LocationLotDetail locationLotDetail, User user)
        {

            PlannedBill oldPlannedBill = plannedBillMgrE.LoadPlannedBill(plannedBill.Id);
            oldPlannedBill.CurrentActingQty = plannedBill.CurrentActingQty;

            //检验，已结算数+本次结算数不能大于总结算数量，可能有负数结算，所以要用绝对值比较
            if (!oldPlannedBill.ActingQty.HasValue)
            {
                oldPlannedBill.ActingQty = 0;
            }
            if (Math.Abs(oldPlannedBill.ActingQty.Value + oldPlannedBill.CurrentActingQty) > Math.Abs(oldPlannedBill.PlannedQty))
            {
                throw new BusinessErrorException("PlannedBill.Error.ActingQtyExceed");
            }

            DateTime dateTimeNow = DateTime.Now;

            ActingBill actingBill = this.RetriveActingBill(oldPlannedBill, dateTimeNow, user);

            #region 计算结算金额
            if (Math.Abs(oldPlannedBill.ActingQty.Value + oldPlannedBill.CurrentActingQty) < Math.Abs(oldPlannedBill.PlannedQty))
            {
                //总结算数小于计划数，用实际单价计算待开票金额
                plannedBill.CurrentBillAmount = oldPlannedBill.UnitPrice * oldPlannedBill.CurrentActingQty;
            }
            else
            {
                plannedBill.CurrentBillAmount = oldPlannedBill.PlannedAmount - (oldPlannedBill.ActingAmount.HasValue ? oldPlannedBill.ActingAmount.Value : 0);
            }
            actingBill.BillAmount += plannedBill.CurrentBillAmount;
            #endregion

            #region 更新Planed Bill的已结算数量和金额
            if (!oldPlannedBill.ActingQty.HasValue)
            {
                oldPlannedBill.ActingQty = 0;
            }
            oldPlannedBill.ActingQty += oldPlannedBill.CurrentActingQty;

            if (!oldPlannedBill.ActingAmount.HasValue)
            {
                oldPlannedBill.ActingAmount = 0;
            }
            oldPlannedBill.ActingAmount += plannedBill.CurrentBillAmount;
            oldPlannedBill.LastModifyDate = dateTimeNow;
            oldPlannedBill.LastModifyUser = user;

            this.plannedBillMgrE.UpdatePlannedBill(oldPlannedBill);
            #endregion

            if (actingBill.Id == 0)
            {
                actingBillMgrE.CreateActingBill(actingBill);
            }
            else
            {
                actingBillMgrE.UpdateActingBill(actingBill);
            }

            #region 记BillTransaction
            billTransactionMgrE.RecordBillTransaction(plannedBill, actingBill, locationLotDetail, user);
            #endregion

            #region 记成本Trans
            if (plannedBill.TransactionType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                //只有采购才需要记录
                this.costMgr.RecordProcurementCostTransaction(plannedBill, user);
            }
            #endregion

            return actingBill;
        }

        #endregion Customized Methods

        #region Private Methods
        private ActingBill RetriveActingBill(PlannedBill plannedBill, DateTime dateTimeNow, User user)
        {

            DateTime effectiveDate = DateTime.Parse(dateTimeNow.ToShortDateString());   //仅保留年月日

            DetachedCriteria criteria = DetachedCriteria.For<ActingBill>();

            criteria.Add(Expression.Eq("OrderNo", plannedBill.OrderNo));
            if (plannedBill.ExternalReceiptNo != null)
            {
                criteria.Add(Expression.Eq("ExternalReceiptNo", plannedBill.ExternalReceiptNo));
            }
            else
            {
                criteria.Add(Expression.IsNull("ExternalReceiptNo"));
            }
            criteria.Add(Expression.Eq("ReceiptNo", plannedBill.ReceiptNo));
            criteria.Add(Expression.Eq("TransactionType", plannedBill.TransactionType));
            criteria.Add(Expression.Eq("Item", plannedBill.Item));
            criteria.Add(Expression.Eq("BillAddress", plannedBill.BillAddress));
            criteria.Add(Expression.Eq("Uom", plannedBill.Uom));
            criteria.Add(Expression.Eq("UnitCount", plannedBill.UnitCount));
            criteria.Add(Expression.Eq("PriceList", plannedBill.PriceList));
            criteria.Add(Expression.Eq("UnitPrice", plannedBill.UnitPrice));
            criteria.Add(Expression.Eq("Currency", plannedBill.Currency));
            criteria.Add(Expression.Eq("IsIncludeTax", plannedBill.IsIncludeTax));
            if (plannedBill.TaxCode != null)
            {
                criteria.Add(Expression.Eq("TaxCode", plannedBill.TaxCode));
            }
            else
            {
                criteria.Add(Expression.IsNull("TaxCode"));
            }

            if (plannedBill.LocationFrom != null)
            {
                criteria.Add(Expression.Eq("LocationFrom", plannedBill.LocationFrom));
            }
            else
            {
                criteria.Add(Expression.IsNull("LocationFrom"));
            }

            criteria.Add(Expression.Eq("IsProvisionalEstimate", plannedBill.IsProvisionalEstimate));
            criteria.Add(Expression.Eq("EffectiveDate", effectiveDate));

            IList<ActingBill> actingBillList = this.criteriaMgrE.FindAll<ActingBill>(criteria);

            ActingBill actingBill = null;
            if (actingBillList.Count == 0)
            {
                actingBill = new ActingBill();
                CloneHelper.CopyProperty(plannedBill, actingBill, PlannedBill2ActingBillCloneField);
                actingBill.EffectiveDate = effectiveDate;
                actingBill.CreateUser = user;
                actingBill.CreateDate = dateTimeNow;
            }
            else if (actingBillList.Count == 1)
            {
                actingBill = actingBillList[0];
            }
            else
            {
                throw new TechnicalException("Acting bill record consolidate error, find target acting bill number great than 1.");
            }


            actingBill.BillQty += plannedBill.CurrentActingQty;
            if (actingBill.BillQty != actingBill.BilledQty)
            {
                actingBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
            }
            else
            {
                actingBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
            }
            actingBill.LastModifyUser = user;
            actingBill.LastModifyDate = dateTimeNow;
            return actingBill;
        }

        private void SendBillEmail(Bill bill, User user)
        {
            #region 发送邮件通知

            string supplierEmail = string.Empty;
            if (bill.BillAddress != null && bill.BillAddress.Email != null)
            {
                supplierEmail = bill.BillAddress.Email.Trim();
            }
            if (supplierEmail != string.Empty && (bill.TransactionType == BusinessConstants.BILL_TRANS_TYPE_PO))
            {
                IList<EntityPreference> entityPreferences = entityPreferenceMgrE.GetAllEntityPreference();
                string EnableMailToSupplier = this.GetEntityPreference(entityPreferences, BusinessConstants.ENTITY_PREFERENCE_CODE_ENABLEMAILBILL);
                if (EnableMailToSupplier.ToLower() == "true")
                {
                    try
                    {
                        string emailFrom = this.GetEntityPreference(entityPreferences, BusinessConstants.ENTITY_PREFERENCE_CODE_SMTPEMAILADDR);
                        string userMail = emailFrom;
                        if (user.Email != null && user.Email.Trim() != string.Empty)
                        {
                            userMail = user.Email.Trim();
                        }
                        string subject = string.Empty;
                        string companyName = this.GetEntityPreference(entityPreferences, BusinessConstants.ENTITY_PREFERENCE_CODE_COMPANYNAME);

                        subject = companyName + "给您的开票通知:" + bill.BillNo;

                        string mailBody = "尊敬的" + bill.BillAddress.Party.Name + ":<br />";
                        mailBody += companyName + "给您的开票通知:" + bill.BillNo;
                        mailBody += "<br />此开票通知的开票期间为:" + (bill.StartDate.HasValue ? bill.StartDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                            + "～" + (bill.EndDate.HasValue ? bill.EndDate.Value.ToString("yyyy-MM-dd") : string.Empty);
//                        mailBody += "<br />合计:" + bill.TotalBillAmount.ToString("0.########");
//                        mailBody += "<br />您的材料结算清单如下:";
//                        mailBody += "<table cellspacing='0' cellpadding='4' rules='all' border='1' style='border-collapse:collapse;font-size:12px;'>";
//                        mailBody += "<tr style='color:#FFFFFF;background-color:#666666;font-weight:bold;line-height:150%;'>";
//                        mailBody += @"<th scope='col'>物料号</th><th scope='col'>物料描述</th><th scope='col'>单位</th><th scope='col'>数量</th>
//                                      <th scope='col'>单价</th><th scope='col'>金额</th>
//                                      <th scope='col'>采购单号</th><th scope='col'>送货单号</th><th scope='col'>入库单号</th>
//                                      <th scope='col'>入库日期</th></tr>";

//                        foreach (var billDetail in bill.BillDetails)
//                        {
//                            mailBody += "<tr><td>" + billDetail.ActingBill.Item.Code + "</td><td>" + billDetail.ActingBill.Item.Description + "</td><td>"
//                                + billDetail.ActingBill.Uom.Name + "</td><td>" + billDetail.BilledQty.ToString("0.########") +
//                                "</td><td>" + billDetail.UnitPrice.ToString("0.########") +
//                                "</td><td>" + billDetail.Amount.ToString("0.########") +
//                                "</td><td>" + billDetail.ActingBill.OrderNo +
//                                "</td><td>" + billDetail.IpNo +
//                                "</td><td>" + billDetail.ActingBill.ReceiptNo +
//                                "</td><td>" + billDetail.ActingBill.EffectiveDate.ToString("yyyy-MM-dd") +
//                                @"</td></tr>";
//                        }
//                        mailBody += "</table>";

                        mailBody += @"<br />说明:
                                    <br/>1.所有单价均为不含税价;
                                    <br/>2.必须把结算清单号标注在发票备注栏内;
                                    <br/>3.发票合计金额必须与清单合计金额一致;";
                        mailBody += "<br /><br />您可以登录到我公司的网站查看此开票通知单:" + bill.BillNo;
                        mailBody += "<br />谢谢合作!<br /><br />";
                        mailBody += "<div style='color:red'><b>此邮箱为系统服务邮箱，请勿直接回复，如有任何问题请直接联系对口业务员。</b></div><br />";
                        mailBody += "系统服务<br />";
                        mailBody += "日期:" + DateTime.Now.ToString();

                        string SMTPEmailHost = this.GetEntityPreference(entityPreferences, BusinessConstants.ENTITY_PREFERENCE_CODE_SMTPEMAILHOST);
                        string SMTPEmailPasswd = this.GetEntityPreference(entityPreferences, BusinessConstants.ENTITY_PREFERENCE_CODE_SMTPEMAILPASSWD);

                        SMTPHelper.SendSMTPEMail(subject, mailBody, emailFrom, supplierEmail, SMTPEmailHost, SMTPEmailPasswd, userMail);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            #endregion
        }

        private string GetEntityPreference(IList<EntityPreference> entityPreferences, string code)
        {
            var q = entityPreferences.Where(en => StringHelper.Eq(en.Code, code));
            if (q != null && q.Count() > 0)
            {
                return q.First().Value;
            }
            return string.Empty;
        }
        #endregion
    }
}


#region Extend Class




namespace com.Sconit.Service.Ext.MasterData.Impl
{

    [Transactional]
    public partial class BillMgrE : com.Sconit.Service.MasterData.Impl.BillMgr, IBillMgrE
    {
    }
}
#endregion
