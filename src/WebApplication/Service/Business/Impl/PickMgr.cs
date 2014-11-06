using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using Castle.Services.Transaction;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.Report;


namespace com.Sconit.Service.Business.Impl
{
    public class PickMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IPickListMgrE pickListMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IPickListDetailMgrE pickListDetailMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IReportMgrE reportMgrE { get; set; }

        
        protected override void SetBaseInfo(Resolver resolver)
        {
            if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_PICKLIST)
            {
                setBaseMgrE.FillResolverByPickList(resolver);

                if (resolver.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
                {
                    throw new BusinessErrorException("Common.Business.Error.StatusError", resolver.Code, resolver.Status);
                }
            }
            else
            {
                throw new BusinessErrorException("Common.Business.Error.BarCodeInvalid");
            }
        }

        protected override void GetDetail(Resolver resolver)
        {
            PickList pickList = pickListMgrE.LoadPickList(resolver.Input, true, true);

            resolver.Transformers = TransformerHelper.ConvertPickListDetailsToTransformers(pickList.PickListDetails);
            resolver.Result = languageMgrE.TranslateMessage("Common.Business.PickList", resolver.UserCode) + ":" + resolver.Code;
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
        }

        protected override void SetDetail(Resolver resolver)
        {
            setDetailMgrE.MatchShip(resolver);
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            this.PickList(resolver);
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
            executeMgrE.CancelOperation(resolver);
        }

        /// <summary>
        /// 拣货
        /// </summary>
        /// <param name="resolver"></param>
        [Transaction(TransactionMode.Unspecified)]
        public void PickList(Resolver resolver)
        {
            PickList pickList = pickListMgrE.CheckAndLoadPickList(resolver.Code);
            pickList.PickListDetails = new List<PickListDetail>();
            if (resolver.Transformers != null)
            {
                foreach (Transformer transformer in resolver.Transformers)
                {
                    PickListDetail pickListDetail = pickListDetailMgrE.LoadPickListDetail(transformer.Id, true);
                    if (transformer != null)
                    {
                        foreach (TransformerDetail transformerDetail in transformer.TransformerDetails)
                        {
                            if (transformerDetail != null && transformerDetail.HuId != null && transformerDetail.HuId != string.Empty
                                && transformerDetail.CurrentQty != 0)
                            {
                                PickListResult pickListResult = new PickListResult();
                                //pickListResult.LocationLotDetail = locationLotDetailMgrE.LoadLocationLotDetail(transformerDetail.LocationLotDetId);
                                pickListResult.LocationLotDetail = locationLotDetailMgrE.CheckLoadHuLocationLotDetail(transformerDetail.HuId);
                                pickListResult.PickListDetail = pickListDetail;
                                pickListResult.Qty = transformerDetail.CurrentQty * pickListDetail.OrderLocationTransaction.UnitQty;
                                pickListDetail.AddPickListResult(pickListResult);
                            }
                        }
                    }
                    pickList.AddPickListDetail(pickListDetail);
                }
            }
            pickListMgrE.DoPick(pickList, resolver.UserCode);
            resolver.Result = languageMgrE.TranslateMessage("MasterData.PickList.Pick.Successfully", resolver.UserCode, resolver.Code);
            resolver.Transformers = null;
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void ExecutePrint(Resolver resolver)
        {
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void GetReceiptNotes(Resolver resolver)
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
            resolver.ReceiptNotes = receiptNotes;
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
    }
}




﻿
#region Extend Class




namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class PickMgrE : com.Sconit.Service.Business.Impl.PickMgr, IBusinessMgrE
    {
        
    }
}

#endregion
