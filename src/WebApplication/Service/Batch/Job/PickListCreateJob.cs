using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using System.Collections;
using Castle.Services.Transaction;
using com.Sconit.Utility;

namespace com.Sconit.Service.Batch.Job
{
    [Transactional]
    public class PickListCreateJob : IJob
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.PickList");
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IPickListMgrE pickListMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public ILocationDetailMgrE locationDetailMgrE { get; set; }
        public IOrderHeadMgrE orderHeadMgrE { get; set; }

        #region IJob Members


        public void Execute(JobRunContext context)
        {
            log.Info("----------------------------------Invincible's dividing line---------------------------------------");
            log.Info("job run start.");
            DetachedCriteria criteria = DetachedCriteria.For<OrderHead>();

            criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
            //criteria.Add(Expression.Le("StartTime", DateTime.Now));  //为了周五把周六周日的要货单全部发完，故注释掉该行
            criteria.Add(Expression.Eq("IsAutoCreatePickList", true));
            //criteria.Add(Expression.Eq("IsPickListCreated", false));

            //查找自动创建拣货单并且正在执行中的订单
            IList<OrderHead> orderHeadList = this.criteriaMgrE.FindAll<OrderHead>(criteria);
            if (orderHeadList != null && orderHeadList.Count > 0)
            {

                string productLine = context.JobDataMap.GetStringValue("FlowCode");
                if (productLine != null && productLine != string.Empty)
                {
                    string[] productLineArray = productLine.Split('|');
                    foreach (string seperatedProductLineArray in productLineArray)
                    {
                        log.Info("----------------------------------Start Create PickList for " + seperatedProductLineArray + "---------------------------------------");
                        string[] plArray = seperatedProductLineArray.Split(',');

                        List<OrderHead> odList = new List<OrderHead>();

                        foreach (string pl in plArray)
                        {
                            IList<OrderHead> q = orderHeadList.Where(o => pl == o.Flow).ToList();
                            if (q != null && q.Count > 0)
                            {
                                IListHelper.AddRange<OrderHead>(odList, q);
                            }
                        }

                        if (odList != null && odList.Count > 0)
                        {
                            try
                            {
                                GenertatePickList(odList);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                this.criteriaMgrE.FlushSession();
                                this.criteriaMgrE.CleanSession();
                            }
                        }
                        log.Info("----------------------------------End Create PickList for " + seperatedProductLineArray + "---------------------------------------");
                    }
                }
            }

            log.Info("job run end.");
        }

        //public void Execute(JobRunContext context)
        //{
        //    DetachedCriteria criteria = DetachedCriteria.For<OrderHead>();

        //    criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
        //    criteria.Add(Expression.Eq("IsAutoCreatePickList", true));

        //    //查找自动创建拣货单并且正在执行中的订单
        //    IList<OrderHead> orderHeadList = this.criteriaMgrE.FindAll<OrderHead>(criteria);
        //    if (orderHeadList != null && orderHeadList.Count > 0)
        //    {
        //        foreach(OrderHead orderHead in orderHeadList) 
        //        {
        //           GenertatePickList(orderHead);
        //        }
        //    }
        //}

        #endregion

        [Transaction(TransactionMode.RequiresNew)]
        public virtual void GenertatePickList(IList<OrderHead> orderHeadList)
        {
            DetachedCriteria criteria = DetachedCriteria.For<OrderLocationTransaction>();
            criteria.CreateAlias("OrderDetail", "od");

            criteria.Add(Expression.In("od.OrderHead", orderHeadList.ToArray()));
            criteria.Add(Expression.Eq("IOType", BusinessConstants.IO_TYPE_OUT));

            IList<OrderLocationTransaction> orderLocationTransactionList = this.criteriaMgrE.FindAll<OrderLocationTransaction>(criteria);

            IList<OrderLocationTransaction> unPickedOrderLocationTransactionList = new List<OrderLocationTransaction>();
            foreach (OrderLocationTransaction orderLocationTransaction in orderLocationTransactionList)
            {
                //查找未全部发货的OrderLocationTransaction
                if (!orderLocationTransaction.AccumulateQty.HasValue
                    || orderLocationTransaction.AccumulateQty.Value < orderLocationTransaction.OrderedQty)
                {
                    #region 查找该OrderLocationTransaction是否有未关闭的拣货单
                    criteria = DetachedCriteria.For<PickListDetail>();
                    criteria.SetProjection(Projections.RowCount());
                    criteria.CreateAlias("OrderLocationTransaction", "olt");
                    criteria.CreateAlias("PickList", "pl");

                    criteria.Add(Expression.Eq("olt.Id", orderLocationTransaction.Id));
                    criteria.Add(Expression.In("pl.Status", new string[] { BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT, BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS }));

                    IList result = this.criteriaMgrE.FindAll(criteria);
                    #endregion

                    if (result != null && result[0] != null && (int)result[0] == 0)
                    {
                        orderLocationTransaction.CurrentShipQty = orderLocationTransaction.OrderedQty - (orderLocationTransaction.AccumulateQty.HasValue ? orderLocationTransaction.AccumulateQty.Value : 0);
                        unPickedOrderLocationTransactionList.Add(orderLocationTransaction);
                        log.Debug("OrderNo[" + orderLocationTransaction.OrderDetail.OrderHead.OrderNo + "], Item[" + orderLocationTransaction.Item.Code + "], Qty[" + orderLocationTransaction.CurrentShipQty + "] will generate pickList");
                    }
                }
            }

            if (unPickedOrderLocationTransactionList != null && unPickedOrderLocationTransactionList.Count > 0)
            {
                PickList pickList = this.pickListMgrE.CreatePickList(unPickedOrderLocationTransactionList, this.userMgrE.GetMonitorUser());
                log.Debug("PickList " + pickList .PickListNo + " Created");
            }
        }

        //[Transaction(TransactionMode.RequiresNew)]
        //public virtual void GenertatePickList(OrderHead orderHead)
        //{
        //    IList<OrderLocationTransaction> unPickedOrderLocationTransactionList = new List<OrderLocationTransaction>();
        //    foreach (OrderDetail orderDetail in orderHead.OrderDetails)
        //    {
        //        foreach (OrderLocationTransaction orderLocationTransaction in orderDetail.OrderLocationTransactions)
        //        {
        //            //查找未全部发货的OrderLocationTransaction
        //            if (orderLocationTransaction.IOType == BusinessConstants.IO_TYPE_OUT
        //                && (!orderLocationTransaction.AccumulateQty.HasValue || orderLocationTransaction.AccumulateQty < orderLocationTransaction.OrderedQty))
        //            {
        //                #region 查找该OrderLocationTransaction是否有未关闭的拣货单
        //                DetachedCriteria criteria2 = DetachedCriteria.For<PickListDetail>();
        //                criteria2.SetProjection(Projections.Count("Id"));
        //                criteria2.CreateAlias("OrderLocationTransaction", "olt");
        //                criteria2.CreateAlias("PickList", "pl");

        //                criteria2.Add(Expression.Eq("olt.Id", orderLocationTransaction.Id));
        //                criteria2.Add(Expression.Eq("pl.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));

        //                IList result = this.criteriaMgrE.FindAll(criteria2);
        //                #endregion

        //                if (((int)result[0]) == 0)
        //                {
        //                    orderLocationTransaction.CurrentShipQty = orderLocationTransaction.OrderedQty - (orderLocationTransaction.AccumulateQty.HasValue ? orderLocationTransaction.AccumulateQty.Value : 0);
        //                    unPickedOrderLocationTransactionList.Add(orderLocationTransaction);
        //                }
        //            }
        //        }
        //    }

        //    #region 有未创建拣货单的订单，自动创建拣货单
        //    if (unPickedOrderLocationTransactionList != null && unPickedOrderLocationTransactionList.Count > 0)
        //    {
        //        bool hasInventory = false;


        //        foreach (OrderLocationTransaction orderLocationTransaction in unPickedOrderLocationTransactionList)
        //        {
        //            //查找是否有可用库存
        //            LocationDetail locationDetail = this.locationDetailMgrE.GetLocationDetail(orderLocationTransaction.Location.Code, orderLocationTransaction.Item.Code);
        //            if (locationDetail != null && locationDetail.Qty > 0)
        //            {
        //                //查找拣货单占用库存数量
        //                DetachedCriteria criteria3 = DetachedCriteria.For<PickListDetail>();
        //                criteria3.CreateAlias("Item", "i");
        //                criteria3.CreateAlias("Location", "l");
        //                criteria3.CreateAlias("PickList", "pl");

        //                criteria3.Add(Expression.Eq("i.Code", orderLocationTransaction.Item.Code));
        //                criteria3.Add(Expression.Eq("l.Code", orderLocationTransaction.Location.Code));
        //                criteria3.Add(Expression.Not(Expression.Eq("pl.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)));
        //                criteria3.Add(Expression.Not(Expression.Eq("pl.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL)));

        //                decimal pickedQty = 0;
        //                IList<PickListDetail> pickListDetailList = this.criteriaMgrE.FindAll<PickListDetail>(criteria3);
        //                if (pickListDetailList != null && pickListDetailList.Count > 0)
        //                {
        //                    foreach (PickListDetail pickListDetail in pickListDetailList)
        //                    {
        //                        pickedQty += pickListDetail.OrderLocationTransaction.UnitQty * pickListDetail.Qty;
        //                    }
        //                }

        //                if (pickedQty < locationDetail.Qty)
        //                {
        //                    hasInventory = true;
        //                }
        //            }
        //        }

        //        if (hasInventory)
        //        {
        //            this.pickListMgrE.CreatePickList(unPickedOrderLocationTransactionList, this.userMgrE.GetMonitorUser());
        //        }
        //    }
        //    #endregion
        //}
    }
}



#region Extend Class


namespace com.Sconit.Service.Ext.Batch.Job
{
    [Transactional]
    public partial class PickListCreateJob : com.Sconit.Service.Batch.Job.PickListCreateJob
    {
        public PickListCreateJob()
        {
        }
    }
}

#endregion
