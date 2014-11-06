using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MRP;
using com.Sconit.Entity.MRP;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.Criteria;
using System.Linq;
using System.Reflection;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MRP;
using com.Sconit.Entity.Exception;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MRP.Impl
{
    [Transactional]
    public class CustomerScheduleDetailMgr : CustomerScheduleDetailBaseMgr, ICustomerScheduleDetailMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }
        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public IList<CustomerScheduleDetail> GetCustomerScheduleDetails(string flowCode, DateTime currentDate)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CustomerScheduleDetail));
            criteria.CreateAlias("CustomerSchedule", "cs");
            criteria.Add(Expression.Eq("cs.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
            if (flowCode != null && flowCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("cs.Flow", flowCode));
            }
            //criteria.Add(Expression.Le("cs.ReleaseDate", currentDate.Date.AddDays(1)));
            criteria.Add(Expression.Ge("DateFrom", currentDate));
            criteria.AddOrder(Order.Asc("StartTime"));
            return criteriaMgr.FindAll<CustomerScheduleDetail>(criteria, 0, 15000);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CustomerScheduleDetail> GetCustomerScheduleDetails(int customerScheduleId)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CustomerScheduleDetail));
            criteria.CreateAlias("CustomerSchedule", "cs");
            criteria.Add(Expression.Le("cs.Id", customerScheduleId));
            return criteriaMgr.FindAll<CustomerScheduleDetail>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CustomerScheduleDetail> GetEffectiveCustomerScheduleDetail(IList<CustomerScheduleDetail> customerScheduleDetailList)
        {
            return GetEffectiveCustomerScheduleDetail(customerScheduleDetailList, DateTime.Now);
        }

        /* �����Ϸ���
        [Transaction(TransactionMode.Requires)]
        public IList<CustomerScheduleDetail> GetEffectiveCustomerScheduleDetail(IList<CustomerScheduleDetail> customerScheduleDetailList, DateTime effectiveDate)
        {
            IList<CustomerScheduleDetail> effectiveCustomerScheduleDetailList = new List<CustomerScheduleDetail>();

            if (customerScheduleDetailList != null && customerScheduleDetailList.Count > 0)
            {
                #region ����Flow����
                var groupedCustomerScheduleDetailList = from det in customerScheduleDetailList
                                                        where det.DateFrom >= effectiveDate
                                                        group det by det.CustomerSchedule.Flow into result
                                                        select new
                                                        {
                                                            Flow = result.Key,
                                                            List = result.ToList()
                                                        };
                #endregion

                if (groupedCustomerScheduleDetailList != null && groupedCustomerScheduleDetailList.Count() > 0)
                {
                    foreach (var groupedCustomerScheduleDetail in groupedCustomerScheduleDetailList)
                    {
                        IList<CustomerScheduleDetail> detList = groupedCustomerScheduleDetail.List;

                        #region ����Id����(���ͻ��ճ̷���)
                        var groupedDetailList = from det in detList
                                                group det by det.CustomerSchedule.Id into result
                                                select new
                                                {
                                                    Id = result.Key,
                                                    List = result.ToList()
                                                };
                        #endregion

                        #region �ٸ���Id����
                        var orderedAndGroupedDetailList = from det in groupedDetailList
                                                          orderby det.Id descending
                                                          select det;
                        #endregion

                        #region ѭ����ȡ��Ч�ճ�
                        DateTime? minDateFrom = null;  //�����ճ̵���С��ʼ���ڣ��ɵ��ճ�ȡ�ȿ�ʼ����С����
                        foreach (var orderedAndGroupedDetail in orderedAndGroupedDetailList)
                        {
                            if (!minDateFrom.HasValue)
                            {
                                //�����ճ̣�ȫ������Ч��
                                IListHelper.AddRange<CustomerScheduleDetail>(effectiveCustomerScheduleDetailList, orderedAndGroupedDetail.List);
                            }
                            else
                            {
                                #region ���ճ̣�ֻ��С����С��ʼ��������Ч��
                                var effDetail = (from det in orderedAndGroupedDetail.List
                                                 where det.DateFrom < minDateFrom
                                                 select det);

                                if (effDetail != null && effDetail.Count() > 0)
                                {
                                    IListHelper.AddRange<CustomerScheduleDetail>(effectiveCustomerScheduleDetailList, effDetail.ToList());
                                }
                                else
                                {
                                    continue;
                                }
                                #endregion
                            }

                            //��С��ʼ���ڸ�ֵ
                            minDateFrom = (from det in orderedAndGroupedDetail.List
                                           orderby det.DateFrom ascending
                                           select det.DateFrom).FirstOrDefault();
                        }
                        #endregion
                    }
                }
            }

            return effectiveCustomerScheduleDetailList;
        }
        */

        [Transaction(TransactionMode.Requires)]
        public IList<CustomerScheduleDetail> GetEffectiveCustomerScheduleDetail(IList<CustomerScheduleDetail> customerScheduleDetailList, DateTime effectiveDate)
        {
            IList<CustomerScheduleDetail> effectiveCustomerScheduleDetailList = new List<CustomerScheduleDetail>();

            if (customerScheduleDetailList != null && customerScheduleDetailList.Count > 0)
            {
                #region ����Flow����
                var groupedCustomerScheduleDetailList = from det in customerScheduleDetailList
                                                        //where det.DateTo >= effectiveDate
                                                        group det by det.CustomerSchedule.Flow into result
                                                        select new
                                                        {
                                                            Flow = result.Key,
                                                            List = result.ToList()
                                                        };
                #endregion

                if (groupedCustomerScheduleDetailList != null && groupedCustomerScheduleDetailList.Count() > 0)
                {
                    foreach (var groupedCustomerScheduleDetail in groupedCustomerScheduleDetailList)
                    {
                        foreach (CustomerScheduleDetail det in groupedCustomerScheduleDetail.List)
                        {
                            var q = effectiveCustomerScheduleDetailList.Where(c => //c.CustomerSchedule.Id < det.CustomerSchedule.Id &&
                                  StringHelper.Eq(c.Item, det.Item) && StringHelper.Eq(c.Type, det.Type) &&
                                  StringHelper.Eq(c.Uom, det.Uom) && c.UnitCount == det.UnitCount &&
                                  c.StartTime.Date == det.StartTime.Date);

                            if (q.Count() == 0)
                            {
                                effectiveCustomerScheduleDetailList.Add(det);
                            }
                            else if (q.Count() == 1)
                            {
                                if (q.Single().Id < det.Id)
                                {
                                    //q.Single().Qty = det.Qty;
                                    effectiveCustomerScheduleDetailList.Remove(q.Single());
                                    effectiveCustomerScheduleDetailList.Add(det);
                                }
                            }
                            else
                            {
                                //log.Error("Have Same CustomerScheduleDetail");
                                throw new TechnicalException("Have Same CustomerScheduleDetail");
                            }
                        }
                    }
                }
            }
            return effectiveCustomerScheduleDetailList;
        }

        [Transaction(TransactionMode.Requires)]
        public ScheduleView TransferCustomerScheduleDetails2ScheduleView(IList<CustomerScheduleDetail> customerScheduleDetails, DateTime effDate)
        {
            customerScheduleDetails = this.GetEffectiveCustomerScheduleDetail(customerScheduleDetails, effDate).ToList();

            #region ͷ
            List<ScheduleHead> scheduleHeads =
               (from det in customerScheduleDetails
                group det by new { det.DateFrom, det.DateTo, det.Type, det.CustomerSchedule.ReleaseDate } into result
                select new ScheduleHead
            {
                DateFrom = result.Key.DateFrom,
                DateTo = result.Key.DateTo,
                Type = result.Key.Type,
                ReleaseDate = result.Key.ReleaseDate
            }).ToList();
            scheduleHeads = scheduleHeads.OrderBy(c => c.DateFrom).Take(41).ToList();
            #endregion

            #region ��ϸ
            List<ScheduleBody> scheduleBodys =
                (from det in customerScheduleDetails
                 group det by new { det.Item, det.Uom, det.UnitCount, det.Location, det.ItemDescription, det.ItemReference } into result
                 select new ScheduleBody
                 {
                     Item = result.Key.Item,
                     Uom = result.Key.Uom,
                     UnitCount = result.Key.UnitCount,
                     Location = result.Key.Location,
                     ItemDescription = result.Key.ItemDescription,
                     ItemReference = result.Key.ItemReference
                 }).ToList();
            #endregion

            #region ��ֵ
            #region ����1
            if (false)
            {
                foreach (CustomerScheduleDetail customerScheduleDetail in customerScheduleDetails)
                {
                    var q_scheduleHeads = scheduleHeads.Where(c => c.DateFrom == customerScheduleDetail.DateFrom
                        && c.DateTo == customerScheduleDetail.DateTo && StringHelper.Eq(c.Type, customerScheduleDetail.Type));
                    if (q_scheduleHeads.Count() == 1)
                    {
                        int index = scheduleHeads.IndexOf(q_scheduleHeads.Single());
                        string qtyIndex = "Qty" + index.ToString();
                        var q_scheduleBodys = scheduleBodys.Where(c => StringHelper.Eq(c.Item, customerScheduleDetail.Item) && c.UnitCount == customerScheduleDetail.UnitCount
                             && StringHelper.Eq(c.Uom, customerScheduleDetail.Uom) && StringHelper.Eq(c.Location, customerScheduleDetail.Location));
                        if (q_scheduleBodys.Count() == 1)
                        {
                            ScheduleBody scheduleBody = q_scheduleBodys.Single();
                            PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                            foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                            {
                                if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), qtyIndex))
                                {
                                    pi.SetValue(scheduleBody, customerScheduleDetail.Qty, null);
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            #endregion
            #region ����2
            foreach (ScheduleBody scheduleBody in scheduleBodys)
            {
                int i = 0;
                foreach (ScheduleHead scheduleHead in scheduleHeads)
                {
                    string qty = "Qty" + i.ToString();
                    var q = customerScheduleDetails
                        .Where(c => StringHelper.Eq(c.Item, scheduleBody.Item) && StringHelper.Eq(c.Uom, scheduleBody.Uom) && c.UnitCount == scheduleBody.UnitCount &&
                            c.DateFrom == scheduleHead.DateFrom && c.DateTo == scheduleHead.DateTo && c.Type == scheduleHead.Type);
                    if (q.Count() == 1)
                    {
                        PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                        foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                        {
                            if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), qty))
                            {
                                pi.SetValue(scheduleBody, q.Single().Qty, null);
                                break;
                            }
                        }
                    }
                    i++;
                }
            }
            #endregion
            #endregion

            #region ����ȫ����0��
            scheduleBodys = scheduleBodys.Where(s => s.TotalQty > 0).ToList();
            #endregion

            ScheduleView scheduleView = new ScheduleView();
            scheduleView.ScheduleHeads = scheduleHeads;
            scheduleView.ScheduleBodys = scheduleBodys;
            return scheduleView;
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MRP.Impl
{
    [Transactional]
    public partial class CustomerScheduleDetailMgrE : com.Sconit.Service.MRP.Impl.CustomerScheduleDetailMgr, ICustomerScheduleDetailMgrE
    {
    }
}

#endregion Extend Class