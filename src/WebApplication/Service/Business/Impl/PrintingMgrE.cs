using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using NHibernate.Expression;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.Report;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Service.Ext.Distribution;

namespace com.Sconit.Service.Business.Impl
{
    [Transactional]
    public class PrintingMgr : AbstractBusinessMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IReportMgrE reportMgrE { get; set; }
        public IPickListMgrE pickListMgrE { get; set; }
        public IOrderHeadMgrE orderHeadMgrE { get; set; }
        public IInspectOrderMgrE inspectOrderMgrE { get; set; }
        public IInProcessLocationMgrE inProcessLocationMgrE { get; set; }

        protected override void GetReceiptNotes(Resolver resolver)
        {
            try
            {
                if (resolver.Input != null && resolver.Input.Trim() != string.Empty)
                {
                    //清空，不然老是重打
                    resolver.ReceiptNotes = null;
                    
                    string[] printTargets = resolver.Input.Split('|');

                    foreach (string printTarget in printTargets)
                    {
                        if (printTarget == "Purchase")
                        {
                            PrintPurchase(resolver);
                        } 
                        else if (printTarget == "Production")
                        {
                            PrintProduction(resolver);
                        }
                        else if (printTarget == "Inspect")
                        {
                            PrintInspect(resolver);
                        }
                        else if (printTarget == "Picklist")
                        {
                            PrintPickList(resolver);
                        }
                        else if (printTarget == "ASN")
                        {
                            PrintASN(resolver);
                        }
                    }
                }

                this.criteriaMgrE.FlushSession();
            } 
            catch(Exception ex) 
            {

            }
        }

        protected override void SetBaseInfo(Resolver resolver)
        {

        }
        protected override void GetDetail(Resolver resolver)
        {

        }
        protected override void SetDetail(Resolver resolver)
        {

        }
        protected override void ExecuteSubmit(Resolver resolver)
        {

        }
        protected override void ExecuteCancel(Resolver resolver)
        {

        }
        protected override void ExecutePrint(Resolver resolver)
        {

        }

        private void PrintPurchase(Resolver resolver)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderHead));

            criteria.CreateAlias("PartyFrom", "pf");
            criteria.CreateAlias("PartyTo", "pt");

            criteria.Add(Expression.Eq("IsPrinted", false));

            OrderHelper.SetOpenOrderStatusCriteria(criteria, "Status");//订单状态
            SecurityHelper.SetPartySearchCriteria(criteria, "PartyFrom.Code", resolver.UserCode); //区域或者供应商权限

            criteria.Add(Expression.In("Type", new string[] { 
                    BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT, 
                    BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER }));

            IList<OrderHead> orderHeadList = criteriaMgrE.FindAll<OrderHead>(criteria);

            List<ReceiptNote> receiptNotes = new List<ReceiptNote>();
            if (orderHeadList != null && orderHeadList.Count > 0)
            {
                foreach (OrderHead orderHead in orderHeadList)
                {
                    string newUrl = reportMgrE.WriteToFile(orderHead.OrderTemplate, orderHead.OrderNo);
                    orderHead.IsPrinted = true;//to be refactored
                    orderHeadMgrE.UpdateOrderHead(orderHead);
                    ReceiptNote receiptNote = Order2ReceiptNote(orderHead);
                    receiptNote.PrintUrl = newUrl;
                    receiptNotes.Add(receiptNote);
                }
            }

            if (resolver.ReceiptNotes == null)
            {
                resolver.ReceiptNotes = receiptNotes;
            }
            else
            {
                IListHelper.AddRange<ReceiptNote>(resolver.ReceiptNotes, receiptNotes);
            }
        }

        private void PrintProduction(Resolver resolver)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderHead));

            criteria.CreateAlias("PartyFrom", "pf");
            criteria.CreateAlias("PartyTo", "pt");

            criteria.Add(Expression.Eq("IsPrinted", false));

            OrderHelper.SetOpenOrderStatusCriteria(criteria, "Status");//订单状态
            SecurityHelper.SetPartySearchCriteria(criteria, "PartyFrom.Code", resolver.UserCode); //区域或者供应商权限
            criteria.Add(Expression.Eq("Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION));

            IList<OrderHead> orderHeadList = criteriaMgrE.FindAll<OrderHead>(criteria);

            List<ReceiptNote> receiptNotes = new List<ReceiptNote>();
            if (orderHeadList != null && orderHeadList.Count > 0)
            {
                foreach (OrderHead orderHead in orderHeadList)
                {
                    string newUrl = reportMgrE.WriteToFile(orderHead.OrderTemplate, orderHead.OrderNo);
                    orderHead.IsPrinted = true;//to be refactored
                    orderHeadMgrE.UpdateOrderHead(orderHead);
                    ReceiptNote receiptNote = Order2ReceiptNote(orderHead);
                    receiptNote.PrintUrl = newUrl;
                    receiptNotes.Add(receiptNote);
                }
            }

            if (resolver.ReceiptNotes == null)
            {
                resolver.ReceiptNotes = receiptNotes;
            }
            else
            {
                IListHelper.AddRange<ReceiptNote>(resolver.ReceiptNotes, receiptNotes);
            }
        }

        private void PrintInspect(Resolver resolver)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(InspectOrder));

            criteria.Add(Expression.Eq("IsPrinted", false));
            criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));

            IList<InspectOrder> inspectOrderList = criteriaMgrE.FindAll<InspectOrder>(criteria);

            List<ReceiptNote> receiptNotes = new List<ReceiptNote>();
            if (inspectOrderList != null && inspectOrderList.Count > 0)
            {
                foreach (InspectOrder inspectOrder in inspectOrderList)
                {

                    string newUrl = reportMgrE.WriteToFile("InspectOrder.xls", inspectOrder.InspectNo);
                    inspectOrder.IsPrinted = true;//to be refactored
                    inspectOrderMgrE.UpdateInspectOrder(inspectOrder);
                    ReceiptNote receiptNote = InspectOrder2ReceiptNote(inspectOrder);
                    receiptNote.PrintUrl = newUrl;
                    receiptNotes.Add(receiptNote);
                }
            }

            if (resolver.ReceiptNotes == null)
            {
                resolver.ReceiptNotes = receiptNotes;
            }
            else
            {
                IListHelper.AddRange<ReceiptNote>(resolver.ReceiptNotes, receiptNotes);
            }
        }

        private void PrintPickList(Resolver resolver)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(PickList));
            criteria.Add(Expression.Eq("IsPrinted", false));
            OrderHelper.SetOpenOrderStatusCriteria(criteria, "Status");//订单状态
            SecurityHelper.SetRegionSearchCriteria(criteria, "PartyFrom.Code", resolver.UserCode); //区域权限

            IList<PickList> pickList = criteriaMgrE.FindAll<PickList>(criteria, 0, 5);

            List<ReceiptNote> receiptNotes = new List<ReceiptNote>();
            if (pickList != null && pickList.Count > 0)
            {
                foreach (PickList pl in pickList)
                {
                    string newUrl = reportMgrE.WriteToFile("PickList.xls", pl.PickListNo);
                    pl.IsPrinted = true;//to be refactored
                    pickListMgrE.UpdatePickList(pl);
                    ReceiptNote receiptNote = PickList2ReceiptNote(pl);
                    receiptNote.PrintUrl = newUrl;
                    receiptNotes.Add(receiptNote);
                }
            }

            if (resolver.ReceiptNotes == null)
            {
                resolver.ReceiptNotes = receiptNotes;
            }
            else
            {
                IListHelper.AddRange<ReceiptNote>(resolver.ReceiptNotes, receiptNotes);
            }
        }

        private void PrintASN(Resolver resolver)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(InProcessLocation));

            criteria.Add(Expression.Eq("IsPrinted", false));
            criteria.Add(Expression.Eq("NeedPrintAsn", true));
            criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
            criteria.Add(Expression.Eq("AsnTemplate", "ASN.xls"));
            criteria.Add(Expression.Or(Expression.Eq("OrderType", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION),
                                       Expression.Eq("OrderType", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER)));

            SecurityHelper.SetRegionSearchCriteria(criteria, "PartyFrom.Code", resolver.UserCode); //区域权限

            IList<InProcessLocation> inProcessLocationList = criteriaMgrE.FindAll<InProcessLocation>(criteria);

            List<ReceiptNote> receiptNotes = new List<ReceiptNote>();
            if (inProcessLocationList != null && inProcessLocationList.Count > 0)
            {
                foreach (InProcessLocation inProcessLocation in inProcessLocationList)
                {
                    IList<object> list = new List<object>();
                    list.Add(inProcessLocation);
                    list.Add(inProcessLocation.InProcessLocationDetails);

                    string newUrl = reportMgrE.WriteToFile(inProcessLocation.AsnTemplate, list);
                    inProcessLocation.IsPrinted = true;//to be refactored
                    inProcessLocationMgrE.UpdateInProcessLocation(inProcessLocation);
                    ReceiptNote receiptNote = InProcessLocation2ReceiptNote(inProcessLocation);
                    receiptNote.PrintUrl = newUrl;
                    receiptNotes.Add(receiptNote);
                }
            }

            if (resolver.ReceiptNotes == null)
            {
                resolver.ReceiptNotes = receiptNotes;
            }
            else
            {
                IListHelper.AddRange<ReceiptNote>(resolver.ReceiptNotes, receiptNotes);
            }
        }

        private ReceiptNote Order2ReceiptNote(OrderHead orderHead)
        {
            ReceiptNote receiptNote = new ReceiptNote();
            receiptNote.OrderNo = orderHead.OrderNo;
            receiptNote.CreateDate = orderHead.CreateDate;
            receiptNote.CreateUser = orderHead.CreateUser == null ? string.Empty : orderHead.CreateUser.Code;
            receiptNote.Status = orderHead.Status;

            return receiptNote;
        }

        private ReceiptNote InspectOrder2ReceiptNote(InspectOrder inspectOrder)
        {
            ReceiptNote receiptNote = new ReceiptNote();
            receiptNote.OrderNo = inspectOrder.InspectNo;
            receiptNote.CreateDate = inspectOrder.CreateDate;
            receiptNote.CreateUser = inspectOrder.CreateUser == null ? string.Empty : inspectOrder.CreateUser.Code;
            receiptNote.Status = inspectOrder.Status;

            return receiptNote;
        }

        private ReceiptNote PickList2ReceiptNote(PickList pickList)
        {
            ReceiptNote receiptNote = new ReceiptNote();
            receiptNote.OrderNo = pickList.PickListNo;
            receiptNote.CreateDate = pickList.CreateDate;
            receiptNote.CreateUser = pickList.CreateUser == null ? string.Empty : pickList.CreateUser.Code;
            receiptNote.Status = pickList.Status;

            return receiptNote;
        }

        private ReceiptNote InProcessLocation2ReceiptNote(InProcessLocation inProcessLocation)
        {
            ReceiptNote receiptNote = new ReceiptNote();
            receiptNote.OrderNo = inProcessLocation.IpNo;
            receiptNote.CreateDate = inProcessLocation.CreateDate;
            receiptNote.CreateUser = inProcessLocation.CreateUser == null ? string.Empty : inProcessLocation.CreateUser.Code;
            receiptNote.Status = inProcessLocation.Status;

            return receiptNote;
        }
    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class PrintingMgrE : com.Sconit.Service.Business.Impl.PrintingMgr, IBusinessMgrE
    {
    }
}

#endregion
