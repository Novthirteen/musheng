using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Service.Ext.Business;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Report;
using com.Sconit.Entity.MasterData;

namespace com.Sconit.Service.Business.Impl
{
    public class UnitizationMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IReportMgrE reportMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IRepackMgrE repackMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
            if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_LOCATION)
            {
                setBaseMgrE.FillResolverByLocation(resolver);
                if (resolver.FlowCode == null || resolver.FlowCode == string.Empty)
                {
                    resolver.Result = languageMgrE.TranslateMessage("Business.Unitization.CurrentLocation.ScanFlow", resolver.UserCode, resolver.LocationCode);
                }
                else
                {
                    resolver.Result = languageMgrE.TranslateMessage("Business.Unitization.CurrentLocation", resolver.UserCode, resolver.LocationCode);
                }
            }
            else if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_FLOW)
            {
                setBaseMgrE.FillResolverByFlow(resolver);

                if (resolver.LocationCode == null || resolver.LocationCode == string.Empty)
                {
                    throw new BusinessErrorException("Business.Unitization.CurrentFlow.ScanLocationFirst");
                }
                else
                {
                    resolver.Result = languageMgrE.TranslateMessage("Business.Unitization.CurrentLocation", resolver.UserCode, resolver.LocationCode);
                }

            }
            else
            {
                throw new BusinessErrorException("Common.Business.Error.BarCodeInvalid");
            }
        }

        protected override void GetDetail(Resolver resolver)
        {
            #region 查找没有条码化的库存
            setBaseMgrE.FillDetailByFlow(resolver);
            List<string> itemCodes = new List<string>();
            if (resolver.Transformers != null)
            {
                foreach (Transformer transformer in resolver.Transformers)
                {
                    itemCodes.Add(transformer.ItemCode);
                }
            }
            else
            {
                throw new BusinessErrorException("Common.Business.Warn.DetailEmpty");
                //return;
            }
            IList<LocationLotDetail> locationLotDetails = locationLotDetailMgrE.GetLocationLotDetail(resolver.LocationCode, itemCodes, false, false, BusinessConstants.PLUS_INVENTORY, null, false);

            if (locationLotDetails != null)
            {
                foreach (Transformer transformer in resolver.Transformers)
                {
                    foreach (LocationLotDetail locationLotDetail in locationLotDetails)
                    {
                        if (string.Equals(locationLotDetail.Item.Code, transformer.ItemCode, StringComparison.OrdinalIgnoreCase))
                        {
                            Uom uom = new Uom();
                            uom.Code = transformer.UomCode;
                            transformer.Qty += uomConversionMgrE.ConvertUomQty(transformer.ItemCode, locationLotDetail.Item.Uom, locationLotDetail.Qty, uom);
                        }
                    }
                }
            }
            List<Transformer> transformers = new List<Transformer>();
            if (resolver.Transformers != null)
            {
                foreach (Transformer transformer in resolver.Transformers)
                {
                    if (transformer.Qty > 0)
                    {
                        transformers.Add(transformer);
                    }
                }
            }
            resolver.Transformers = transformers;
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
            #endregion
        }

        protected override void SetDetail(Resolver resolver)
        {
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void ExecuteSubmit(Resolver resolver)
        {
            #region 生产条码
            Flow flow = flowMgrE.LoadFlow(resolver.FlowCode, true);
            if (flow.FlowDetails != null)
            {
                foreach (FlowDetail flowDetail in flow.FlowDetails)
                {
                    if (resolver.Transformers != null)
                    {
                        foreach (Transformer transformer in resolver.Transformers)
                        {
                            if (string.Equals(flowDetail.Item.Code, transformer.ItemCode, StringComparison.OrdinalIgnoreCase)
                                && string.Equals(flowDetail.Uom.Code, transformer.UomCode, StringComparison.OrdinalIgnoreCase)
                                && flowDetail.UnitCount == transformer.UnitCount)
                            {
                                flowDetail.OrderedQty = transformer.CurrentQty;
                            }
                        }
                    }
                }
            }

            IList<FlowDetail> targetFlowDetailList = new List<FlowDetail>();

            if (flow.FlowDetails != null && flow.FlowDetails.Count > 0)
            {
                foreach (FlowDetail fd in flow.FlowDetails)
                {
                    if (fd.OrderedQty > 0)
                    {
                        targetFlowDetailList.Add(fd);
                    }
                }
            }

            if (targetFlowDetailList.Count == 0)
            {
                throw new BusinessErrorException("Inventory.Error.PrintHu.FlowDetail.Required");
            }

            string packageType = BusinessConstants.CODE_MASTER_PACKAGETYPE_OUTER;
            User user = userMgrE.LoadUser(resolver.UserCode, false, true);

            EntityPreference entityPreference = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COMPANY_ID_MARK);
            IList<Hu> huList = huMgrE.CreateHu(targetFlowDetailList, user, entityPreference.Value, packageType);

            if (huList.Count > 0)
            {
                IList<object> huDetailObj = new List<object>();
                huDetailObj.Add(huList);
                huDetailObj.Add(resolver.UserCode);
                resolver.PrintUrl = reportMgrE.WriteToFile(huList[0].HuTemplate, huDetailObj, huList[0].HuTemplate);
            }
            #endregion

            #region 数量变条码
            IList<RepackDetail> repackDetailList = new List<RepackDetail>();
            decimal totalQty = 0M;
            foreach (Hu hu in huList)
            {
                RepackDetail outRepackDetail = new RepackDetail();

                outRepackDetail.IOType = BusinessConstants.IO_TYPE_OUT;

                outRepackDetail.Hu = hu;
                outRepackDetail.Qty = outRepackDetail.Hu.Qty * outRepackDetail.Hu.UnitQty;
                totalQty += outRepackDetail.Qty;
                repackDetailList.Add(outRepackDetail);

                if (repackDetailList.Count > 0)
                {
                    IList<LocationLotDetail> locationLotDetailList = locationLotDetailMgrE.GetLocationLotDetail(resolver.LocationCode, hu.Item.Code, false, false, BusinessConstants.PLUS_INVENTORY, null, false);
                    if (locationLotDetailList == null || locationLotDetailList.Count == 0)
                    {
                        throw new BusinessErrorException("MasterData.Inventory.Repack.LocationLotDetail.Empty");
                    }

                    foreach (LocationLotDetail locationLotDetail in locationLotDetailList)
                    {
                        RepackDetail inRepackDetail = new RepackDetail();
                        inRepackDetail.LocationLotDetail = locationLotDetail;
                        inRepackDetail.IOType = BusinessConstants.IO_TYPE_IN;
                        repackDetailList.Add(inRepackDetail);
                        if (locationLotDetail.Qty < totalQty)
                        {

                            inRepackDetail.Qty = locationLotDetail.Qty;
                            totalQty -= inRepackDetail.Qty;
                        }
                        else
                        {
                            inRepackDetail.Qty = totalQty;
                            break;
                        }
                    }
                }
            }
            Repack repack = repackMgrE.CreateRepack(repackDetailList, user);
            #endregion
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
            resolver.Result = languageMgrE.TranslateMessage("Business.Unitization.Successfull", resolver.UserCode);
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
        }

        protected override void ExecutePrint(Resolver resolver)
        {
        }

        protected override void GetReceiptNotes(Resolver resolver)
        {
        }
    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class UnitizationMgrE : com.Sconit.Service.Business.Impl.UnitizationMgr, IBusinessMgrE
    {
    }
}

#endregion