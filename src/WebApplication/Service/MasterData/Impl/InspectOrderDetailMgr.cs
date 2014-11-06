using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class InspectOrderDetailMgr : InspectOrderDetailBaseMgr, IInspectOrderDetailMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public IList<InspectOrderDetail> GetInspectOrderDetail(string inspectOrderNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InspectOrderDetail>();

            criteria.Add(Expression.Eq("InspectOrder.InspectNo", inspectOrderNo));

            return this.criteriaMgrE.FindAll<InspectOrderDetail>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<InspectOrderDetail> GetInspectOrderDetail(InspectOrder inspectOrder)
        {
            return GetInspectOrderDetail(inspectOrder.InspectNo);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InspectOrderDetail> ConvertTransformerToInspectDetail(IList<Transformer> transformerList)
        {
            return this.ConvertTransformerToInspectDetail(transformerList, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InspectOrderDetail> ConvertTransformerToInspectDetail(IList<Transformer> transformerList, bool includeZero)
        {
            IList<InspectOrderDetail> inspectDetailList = new List<InspectOrderDetail>();
            InspectOrderDetail inspectDetail = new InspectOrderDetail();
            if (transformerList != null && transformerList.Count > 0)
            {
                foreach (Transformer transformer in transformerList)
                {
                    if (transformer.TransformerDetails != null && transformer.TransformerDetails.Count > 0)
                    {
                        foreach (TransformerDetail transformerDetail in transformer.TransformerDetails)
                        {
                            InspectOrderDetail inspectOrderDetail = this.LoadInspectOrderDetail(transformerDetail.Id);

                            inspectOrderDetail.CurrentQualifiedQty = transformerDetail.CurrentQty;
                            inspectOrderDetail.CurrentRejectedQty = transformerDetail.CurrentRejectQty;

                            if (inspectOrderDetail.CurrentQualifiedQty != 0 || inspectOrderDetail.CurrentRejectedQty != 0 || includeZero)
                            {
                                inspectDetailList.Add(inspectOrderDetail);
                            }
                        }
                    }
                    else
                    {
                        InspectOrderDetail inspectOrderDetail = this.LoadInspectOrderDetail(transformer.Id);

                        inspectOrderDetail.CurrentQualifiedQty = transformer.CurrentQty;
                        inspectOrderDetail.CurrentRejectedQty = transformer.CurrentRejectQty;

                        if (inspectOrderDetail.CurrentQualifiedQty != 0 || inspectOrderDetail.CurrentRejectedQty != 0 || includeZero)
                        {
                            inspectDetailList.Add(inspectOrderDetail);
                        }
                    }
                }
            }

            return inspectDetailList;
        }

        #endregion Customized Methods
    }
}




#region Extend Interface


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class InspectOrderDetailMgrE : com.Sconit.Service.MasterData.Impl.InspectOrderDetailMgr, IInspectOrderDetailMgrE
    {
        
    }
}
#endregion
