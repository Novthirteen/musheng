using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.Distribution;
using com.Sconit.Service.Criteria;
using com.Sconit.Service.MasterData;
using NHibernate.Expression;
using com.Sconit.Utility;
using NHibernate.Transform;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Criteria;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Distribution.Impl
{
    [Transactional]
    public class InProcessLocationDetailMgr : InProcessLocationDetailBaseMgr, IInProcessLocationDetailMgr
    {
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public void CreateInProcessLocationDetail(InProcessLocation inProcessLocation, OrderLocationTransaction outOrderLocationTransaction, IList<InventoryTransaction> inventoryTransactionList)
        {
            if (inventoryTransactionList != null && inventoryTransactionList.Count > 0)
            {
                InProcessLocationDetail mergedInProcessLocationDetail = null;

                foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                {
                    if (inventoryTransaction.Hu == null && !inventoryTransaction.IsConsignment)
                    {
                        #region �ϲ���������
                        //û��HU && �Ǽ��۲ſ��Ժϲ�
                        if (mergedInProcessLocationDetail == null)
                        {
                            mergedInProcessLocationDetail = new InProcessLocationDetail();
                            mergedInProcessLocationDetail.InProcessLocation = inProcessLocation;
                            mergedInProcessLocationDetail.OrderLocationTransaction = outOrderLocationTransaction;
                            mergedInProcessLocationDetail.Item = outOrderLocationTransaction.OrderDetail.Item;
                            mergedInProcessLocationDetail.ReferenceItemCode = outOrderLocationTransaction.OrderDetail.ReferenceItemCode;
                            mergedInProcessLocationDetail.CustomerItemCode = outOrderLocationTransaction.OrderDetail.CustomerItemCode;
                            mergedInProcessLocationDetail.Uom = outOrderLocationTransaction.OrderDetail.Uom;
                            mergedInProcessLocationDetail.UnitCount = outOrderLocationTransaction.OrderDetail.UnitCount;
                            mergedInProcessLocationDetail.LocationFrom = outOrderLocationTransaction.OrderDetail.LocationFrom;
                            mergedInProcessLocationDetail.LocationTo = outOrderLocationTransaction.OrderDetail.LocationTo;
                        }

                        mergedInProcessLocationDetail.Qty += (0 - inventoryTransaction.Qty) / outOrderLocationTransaction.UnitQty;
                        #endregion
                    }
                    else
                    {
                        InProcessLocationDetail inProcessLocationDetail = new InProcessLocationDetail();
                        inProcessLocationDetail.InProcessLocation = inProcessLocation;
                        inProcessLocationDetail.OrderLocationTransaction = outOrderLocationTransaction;
                        if (inventoryTransaction.Hu != null)
                        {
                            inProcessLocationDetail.HuId = inventoryTransaction.Hu.HuId;
                            inProcessLocationDetail.LotNo = inventoryTransaction.Hu.LotNo;
                        }
                        inProcessLocationDetail.IsConsignment = inventoryTransaction.IsConsignment;
                        inProcessLocationDetail.PlannedBill = inventoryTransaction.PlannedBill;
                        inProcessLocationDetail.Qty = (0 - inventoryTransaction.Qty) / outOrderLocationTransaction.UnitQty;
                        inProcessLocationDetail.Item = outOrderLocationTransaction.OrderDetail.Item;
                        inProcessLocationDetail.ReferenceItemCode = outOrderLocationTransaction.OrderDetail.ReferenceItemCode;
                        inProcessLocationDetail.CustomerItemCode = outOrderLocationTransaction.OrderDetail.CustomerItemCode;
                        inProcessLocationDetail.Uom = outOrderLocationTransaction.OrderDetail.Uom;
                        inProcessLocationDetail.UnitCount = outOrderLocationTransaction.OrderDetail.UnitCount;
                        inProcessLocationDetail.LocationFrom = outOrderLocationTransaction.OrderDetail.LocationFrom;
                        inProcessLocationDetail.LocationTo = outOrderLocationTransaction.OrderDetail.LocationTo;

                        inProcessLocation.AddInProcessLocationDetail(inProcessLocationDetail);

                        this.CreateInProcessLocationDetail(inProcessLocationDetail);
                    }
                }

                if (mergedInProcessLocationDetail != null)
                {
                    inProcessLocation.AddInProcessLocationDetail(mergedInProcessLocationDetail);

                    this.CreateInProcessLocationDetail(mergedInProcessLocationDetail);
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateInProcessLocationDetail(InProcessLocation inProcessLocation, OrderLocationTransaction outOrderLocationTransaction, IList<Hu> huList)
        {
            if (huList != null && huList.Count > 0)
            {
                foreach (Hu hu in huList)
                {
                    InProcessLocationDetail inProcessLocationDetail = new InProcessLocationDetail();
                    inProcessLocationDetail.InProcessLocation = inProcessLocation;
                    inProcessLocationDetail.OrderLocationTransaction = outOrderLocationTransaction;
                    inProcessLocationDetail.HuId = hu.HuId;
                    inProcessLocationDetail.LotNo = hu.LotNo;
                    inProcessLocationDetail.Qty = hu.Qty * hu.UnitQty / outOrderLocationTransaction.UnitQty;  //�ȳ�Hu.UnitQtyתΪ������λ���ڳ�outOrderLocationTransaction.UnitQtyתΪ������λ��

                    inProcessLocation.AddInProcessLocationDetail(inProcessLocationDetail);

                    this.CreateInProcessLocationDetail(inProcessLocationDetail);
                }
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> GetInProcessLocationDetail(string ipNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InProcessLocationDetail>();
            criteria.Add(Expression.Eq("InProcessLocation.IpNo", ipNo));

            return this.criteriaMgrE.FindAll<InProcessLocationDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> GetInProcessLocationDetail(InProcessLocation inProcessLocation)
        {
            return GetInProcessLocationDetail(inProcessLocation.IpNo);
        }

        [Transaction(TransactionMode.Unspecified)]
        public InProcessLocationDetail GetNoneHuAndIsConsignmentInProcessLocationDetail(InProcessLocation inProcessLocation, OrderLocationTransaction orderLocationTransaction)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InProcessLocationDetail>();

            criteria.Add(Expression.Eq("InProcessLocation.IpNo", inProcessLocation.IpNo));
            criteria.Add(Expression.Eq("OrderLocationTransaction.Id", orderLocationTransaction.Id));
            criteria.Add(Expression.Or(Expression.IsNull("HuId"), Expression.Eq("HuId", "")));
            criteria.Add(Expression.Eq("IsConsignment", false));

            IList<InProcessLocationDetail> list = this.criteriaMgrE.FindAll<InProcessLocationDetail>(criteria);

            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> GetInProcessLocationDetail(OrderLocationTransaction orderLocationTransaction)
        {
            return GetInProcessLocationDetail(orderLocationTransaction, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> GetInProcessLocationDetail(OrderLocationTransaction orderLocationTransaction, string asnType)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InProcessLocationDetail>();
            criteria.CreateAlias("OrderLocationTransaction", "olt");
            criteria.Add(Expression.Eq("olt.Id", orderLocationTransaction.Id));

            if (asnType != null)
            {
                criteria.CreateAlias("InProcessLocation", "ip");
                criteria.Add(Expression.Eq("ip.Type", asnType));
            }

            return this.criteriaMgrE.FindAll<InProcessLocationDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> GetInProcessLocationDetail(OrderDetail orderDetail)
        {
            return this.GetInProcessLocationDetail(orderDetail, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> GetInProcessLocationDetail(OrderDetail orderDetail, bool includeGap)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InProcessLocationDetail>();
            criteria.CreateAlias("OrderLocationTransaction", "olt");
            criteria.CreateAlias("olt.OrderDetail", "od");
            criteria.CreateAlias("InProcessLocation", "ip");
            criteria.Add(Expression.Eq("od.Id", orderDetail.Id));
            if (!includeGap)
                criteria.Add(Expression.Eq("ip.Type", BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_NORMAL));

            return this.criteriaMgrE.FindAll<InProcessLocationDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> GetInProcessLocationDetail(OrderHead orderHead)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InProcessLocationDetail>();
            criteria.CreateAlias("OrderLocationTransaction", "olt");
            criteria.CreateAlias("olt.OrderDetail", "od");
            criteria.CreateAlias("od.OrderHead", "oh");
            criteria.Add(Expression.Eq("oh.OrderNo", orderHead.OrderNo));

            return this.criteriaMgrE.FindAll<InProcessLocationDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> GetInProcessLocationDetail(OrderHead orderHead, string status)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InProcessLocationDetail>();
            criteria.CreateAlias("OrderLocationTransaction", "olt");
            criteria.CreateAlias("olt.OrderDetail", "od");
            criteria.CreateAlias("od.OrderHead", "oh");
            criteria.CreateAlias("InProcessLocation", "ip");
            criteria.Add(Expression.Eq("oh.OrderNo", orderHead.OrderNo));
            criteria.Add(Expression.Eq("ip.Status", status));

            return this.criteriaMgrE.FindAll<InProcessLocationDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> GetInProcessLocationDetail(string ipNo, string huId)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InProcessLocationDetail>();
            criteria.Add(Expression.Eq("InProcessLocation.IpNo", ipNo));
            criteria.Add(Expression.Eq("HuId", huId));

            return this.criteriaMgrE.FindAll<InProcessLocationDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public  IList<InProcessLocationDetail> GetInProcessLocationDetail(string ipNo, int outOrderLocationTransactionId)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InProcessLocationDetail>();
            criteria.Add(Expression.Eq("InProcessLocation.IpNo", ipNo));
            criteria.Add(Expression.Eq("OrderLocationTransaction.Id", outOrderLocationTransactionId));

            return this.criteriaMgrE.FindAll<InProcessLocationDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> SummarizeInProcessLocationDetails(IList<InProcessLocationDetail> inProcessLocationDetailList)
        {
            IList<InProcessLocationDetail> ipDetList = new List<InProcessLocationDetail>();
            if (inProcessLocationDetailList != null && inProcessLocationDetailList.Count > 0)
            {
                foreach (InProcessLocationDetail ipDetail in inProcessLocationDetailList)
                {
                    if (ipDetail.HuId == null)
                    {
                        //��֧��Hu,����Ҫ����
                        return inProcessLocationDetailList;
                    }

                    bool isExist = false;
                    foreach (InProcessLocationDetail ipDet in ipDetList)
                    {
                        //OrderLocationTransaction��ͬ�Ļ���
                        if (ipDetail.OrderLocationTransaction.Id == ipDet.OrderLocationTransaction.Id)
                        {
                            ipDet.Qty += ipDetail.Qty;
                            ipDet.AddHuInProcessLocationDetails(ipDetail);
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        InProcessLocationDetail inProcessLocationDetail = new InProcessLocationDetail();
                        CloneHelper.CopyProperty(ipDetail, inProcessLocationDetail, new string[] { "Id", "HuId" }, true);
                        inProcessLocationDetail.AddHuInProcessLocationDetails(ipDetail);
                        ipDetList.Add(inProcessLocationDetail);
                    }
                }
            }

            return ipDetList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> GetInProcessLocationDetailOut(IList<string> itemList, IList<string> locList)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(InProcessLocationDetail));
            criteria.CreateAlias("InProcessLocation", "ip");
            criteria.Add(Expression.Eq("ip.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
            criteria.Add(Expression.Eq("ip.Type", BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_NORMAL));
            criteria.CreateAlias("OrderLocationTransaction", "olt");
            if (itemList.Count == 1)
            {
                criteria.Add(Expression.Eq("olt.Item.Code", itemList[0]));
            }
            else
            {
                criteria.Add(Expression.InG<string>("olt.Item.Code", itemList));
            }
            if (locList.Count == 1)
            {
                criteria.Add(Expression.Eq("olt.Location.Code", locList[0]));
            }
            else
            {
                criteria.Add(Expression.InG<string>("olt.Location.Code", locList));
            }

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("olt.Location.Code").As("LocationCode"))
                .Add(Projections.GroupProperty("olt.Item.Code").As("ItemCode"))
                .Add(Projections.Sum("Qty").As("Qty")));

            criteria.SetResultTransformer(Transformers.AliasToBean(typeof(InProcessLocationDetail)));
            return this.criteriaMgrE.FindAll<InProcessLocationDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> GetInProcessLocationDetailIn(IList<string> itemList, IList<string> locList)
        {
            DetachedCriteria subCriteria = DetachedCriteria.For(typeof(OrderLocationTransaction));
            subCriteria.CreateAlias("OrderDetail", "od");
            subCriteria.CreateAlias("od.OrderHead", "oh");
            OrderHelper.SetOpenOrderStatusCriteria(subCriteria, "oh.Status");
            subCriteria.Add(Expression.Eq("IOType", BusinessConstants.IO_TYPE_IN));
            if (itemList != null && itemList.Count > 0)
            {
                if (itemList.Count == 1)
                {
                    subCriteria.Add(Expression.Eq("Item.Code", itemList[0]));
                }
                else
                {
                    subCriteria.Add(Expression.InG<string>("Item.Code", itemList));
                }
            }
            if (locList != null && locList.Count > 0)
            {
                if (locList.Count == 1)
                {
                    subCriteria.Add(Expression.Eq("Location.Code", locList[0]));
                }
                else
                {
                    subCriteria.Add(Expression.InG<string>("Location.Code", locList));
                }
            }
            subCriteria.SetProjection(Projections.ProjectionList().Add(Projections.GroupProperty("OrderDetail.Id")));

            DetachedCriteria criteria = DetachedCriteria.For(typeof(InProcessLocationDetail));
            criteria.CreateAlias("InProcessLocation", "ip");
            criteria.Add(Expression.Eq("ip.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
            criteria.Add(Expression.Eq("ip.Type", BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_NORMAL));
            criteria.CreateAlias("OrderLocationTransaction", "olt");
            criteria.Add(Subqueries.PropertyIn("olt.OrderDetail.Id", subCriteria));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("olt.Location.Code").As("LocationCode"))
                .Add(Projections.GroupProperty("olt.Item.Code").As("ItemCode"))
                .Add(Projections.Sum("Qty").As("Qty")));

            criteria.SetResultTransformer(Transformers.AliasToBean(typeof(InProcessLocationDetail)));
            return this.criteriaMgrE.FindAll<InProcessLocationDetail>(criteria);
        }
        #endregion Customized Methods
    }
}

#region Extend Interface
namespace com.Sconit.Service.Ext.Distribution.Impl
{
    [Transactional]
    public partial class InProcessLocationDetailMgrE : com.Sconit.Service.Distribution.Impl.InProcessLocationDetailMgr, IInProcessLocationDetailMgrE
    {

    }
}
#endregion
