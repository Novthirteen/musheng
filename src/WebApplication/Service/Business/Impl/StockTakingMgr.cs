using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.Criteria;
using Castle.Services.Transaction;
using NHibernate.Expression;

namespace com.Sconit.Service.Business.Impl
{
    public class StockTakingMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public ICycleCountMgrE cycleCountMgrE { get; set; }
        public ICycleCountDetailMgrE cycleCountDetailMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IStorageBinMgrE storageBinMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }


        protected override void SetBaseInfo(Resolver resolver)
        {
            if (resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_HUSTATUS)
            {
                return;
            }
            if (resolver.BarcodeHead == BusinessConstants.CODE_PREFIX_CYCCNT)
            {
                CycleCount cycleCount = cycleCountMgrE.CheckAndLoadCycleCount(resolver.Input);

                resolver.Code = cycleCount.Code;
                resolver.Description = StringHelper.GetCodeDescriptionString(cycleCount.Location.Code, cycleCount.Location.Name);
                resolver.Status = cycleCount.Status;
                resolver.OrderType = cycleCount.Type;
                resolver.IsScanHu = cycleCount.IsScanHu;
                resolver.LocationCode = cycleCount.Location.Code;
                resolver.Result = resolver.Description;
                resolver.IsPickFromBin = cycleCount.Location.EnableAdvWM;
                if (!resolver.IsScanHu)
                {
                    throw new BusinessErrorException("CycCnt.Error.IsNotScanHu");
                }

                resolver.WorkingHours = new List<string[]>();
                if (cycleCount.Items != null && cycleCount.Items != string.Empty)
                {
                    resolver.WorkingHours.Add(cycleCount.Items.Split('|'));
                }
                else
                {
                    resolver.WorkingHours.Add(new string[] { });
                }
                if (cycleCount.Bins != null && cycleCount.Bins != string.Empty)
                {
                    resolver.WorkingHours.Add(cycleCount.Bins.Split('|'));
                }
                else
                {
                    resolver.WorkingHours.Add(new string[] { });
                }

                resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;

                if (resolver.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
                {
                    throw new BusinessErrorException("Common.Business.Error.StatusError", resolver.Code, resolver.Status);
                }
            }
            else if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_BIN)
            {
                if (resolver.Code == null || resolver.Code == string.Empty)
                {
                    throw new BusinessErrorException("CycCnt.Error.ScanCycCountFirst");
                }
                //if (!resolver.IsPickFromBin)
                //{
                //    //没有启用高位货架,不用扫描库格
                //    throw new BusinessErrorException("CycCnt.Error.NotEnableAdvWM");
                //}
                IList<StorageBin> storageBinList = storageBinMgrE.GetStorageBinByLocation(resolver.LocationCode);
                setBaseMgrE.FillResolverByBin(resolver);

                //CycleCount cycleCount = cycleCountMgrE.CheckAndLoadCycleCount(resolver.Code);
                var bins = storageBinList.Select(s => s.Code);
                if (!bins.Contains(resolver.BinCode))
                {
                    //库格没有在盘点库位上面
                    throw new BusinessErrorException("CycCnt.Error.BinNotInLocation");
                }
                if (resolver.WorkingHours != null && resolver.WorkingHours.Count == 2)
                {
                    if (resolver.WorkingHours[1] != null && resolver.WorkingHours[1].Length > 0 &&
                        !resolver.WorkingHours[1].Contains(resolver.BinCode))
                    {
                        if (resolver.WorkingHours[1].Length > 0)
                        {
                            throw new BusinessErrorException("CycCnt.Error.NotContainTheBin");
                        }
                    }
                }
                else
                {
                    throw new BusinessErrorException("CycCnt.Error");
                }
            }

            //else if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_LOCATION)
            //{
            //    if (resolver.LocationCode != null && resolver.LocationCode.Trim() != string.Empty)
            //    {
            //        throw new BusinessErrorException("CycCnt.Please.ScanHu");
            //    }
            //    setBaseMgrE.FillResolverByLocation(resolver);
            //    resolver.IsScanHu = true;
            //    resolver.BinCode = null;
            //}
            else
            {
                throw new BusinessErrorException("Common.Business.Error.BarCodeInvalid");
            }
        }

        protected override void GetDetail(Resolver resolver)
        {
            //IList<CycleCountDetail> cycleCountDetailList = cycleCountDetailMgrE.GetCycleCountDetail(resolver.Code);
            resolver.Transformers = null;
        }

        protected override void SetDetail(Resolver resolver)
        {
            #region Hu状态查询
            if (resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_HUSTATUS)
            {
                Hu hu = null;
                //检查库存
                LocationLotDetail locationLotDetail = locationLotDetailMgrE.LoadHuLocationLotDetail(resolver.Input);
                if (locationLotDetail == null)//库存中没有,检查HuDet
                {
                    hu = huMgrE.CheckAndLoadHu(resolver.Input);
                    //修正状态
                    if (hu.Status == BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_INVENTORY)
                    {
                        hu.Status = "ERROR";
                    }
                }
                else
                {
                    hu = locationLotDetail.Hu;
                    //修正库位 数量
                    hu.Location = locationLotDetail.Location.Code;
                    hu.Qty = locationLotDetail.Qty / hu.UnitQty;
                    if (hu.Status == BusinessConstants.CODE_MASTER_HU_STATUS_VALUE_INVENTORY)
                    {
                        if (this.locationMgrE.IsHuOcuppyByPickList(resolver.Input))
                        {
                            hu.Status += languageMgrE.TranslateMessage("MasterDate.PickList.Ocuppied", resolver.UserCode);
                        }
                    }
                }
                TransformerDetail transformerDetail = TransformerHelper.ConvertHuToTransformerDetail(hu);
                if (locationLotDetail != null && locationLotDetail.StorageBin != null)
                {
                    transformerDetail.StorageBinCode = locationLotDetail.StorageBin.Code;
                }
                resolver.Transformers = null;
                resolver.AddTransformerDetail(transformerDetail);
            }
            #endregion

            #region 盘点
            else
            {
                if (resolver.WorkingHours == null || resolver.WorkingHours.Count != 2)
                {
                    //出错
                    throw new BusinessErrorException("CycCnt.Error.ScanCycCountFirst");
                }
                if (resolver.LocationCode == null || resolver.LocationCode == string.Empty)
                {
                    throw new BusinessErrorException("CycCnt.Error.HasNoLocation");
                }
                //如果指定了库格或启用了高级仓库管理,需要先扫描库格.
                if ((resolver.BinCode == null || resolver.BinCode == string.Empty)
                    && (resolver.WorkingHours[1].Length > 0 || resolver.IsPickFromBin))
                {
                    throw new BusinessErrorException("CycCnt.Error.ScanStorageBinFirst");
                }

                //检查当月盘点重复扫描
                this.cycleCountMgrE.CheckHuExistThisCount(resolver.Code, resolver.Input);
                Hu hu = huMgrE.CheckAndLoadHu(resolver.Input);

                if (resolver.WorkingHours[0].Length > 0 && !resolver.WorkingHours[0].Contains(hu.Item.Code))
                {
                    if (resolver.WorkingHours[0].Length > 0)
                    {
                        throw new BusinessErrorException("CycCnt.Error.NotContainTheItem");
                    }
                    else
                    {
                        //nothing todo
                    }
                }

                TransformerDetail transformerDetail = Utility.TransformerHelper.ConvertHuToTransformerDetail(hu);
                transformerDetail.Sequence = setDetailMgrE.FindMaxSeq(resolver.Transformers);
                transformerDetail.StorageBinCode = resolver.BinCode;
                resolver.AddTransformerDetail(transformerDetail);
            }
            #endregion
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            User user = userMgrE.LoadUser(resolver.UserCode, false, true);
            IList<CycleCountDetail> cycleCountDetailList = this.ConvertResolverToCycleCountDetail(resolver);
            if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_CYCCNT)
            {
                cycleCountMgrE.RecordCycleCountDetail(resolver.Code, cycleCountDetailList, user);
                resolver.Result = languageMgrE.TranslateMessage("CycCnt.Process.Successfully", resolver.UserCode);
                resolver.Transformers = null;
                resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
                resolver.BinCode = null;
                resolver.LocationCode = null;
            }
            //else if (resolver.LocationCode != null && resolver.LocationCode.Trim() != string.Empty)
            //{
            //    if (resolver.BinCode != null && resolver.BinCode.Trim() != string.Empty)
            //    {
            //        StorageBin bin = storageBinMgrE.CheckAndLoadStorageBin(resolver.BinCode);
            //        cycleCountMgrE.CreateCycleCount(bin, cycleCountDetailList, user);
            //    }
            //    else
            //    {
            //        Location location = locationMgrE.CheckAndLoadLocation(resolver.LocationCode);
            //        cycleCountMgrE.CreateCycleCount(location, cycleCountDetailList, user);
            //    }
            //}
            else
            {
                throw new BusinessErrorException("Common.Business.Error.ScanFlowOrStorageBinFirst");
            }
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
            executeMgrE.CancelOperation(resolver);
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void ExecutePrint(Resolver resolver)
        {
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void GetReceiptNotes(Resolver resolver)
        {
        }

        #region Private Method

        [Transaction(TransactionMode.Unspecified)]
        private IList<CycleCountDetail> ConvertResolverToCycleCountDetail(Resolver resolver)
        {
            if (resolver.Transformers == null || resolver.Transformers.Count == 0)
                return null;

            IList<CycleCountDetail> cycleCountDetailList = new List<CycleCountDetail>();
            foreach (Transformer transformer in resolver.Transformers)
            {
                if (transformer.TransformerDetails != null && resolver.IsScanHu)
                {
                    #region 条码
                    foreach (TransformerDetail transformerDetail in transformer.TransformerDetails)
                    {
                        if (transformerDetail.Id == 0 ||
                            (transformerDetail.HuId != null && transformerDetail.HuId.Trim() != string.Empty))
                        {
                            //Hu hu = huMgrE.LoadHu(transformerDetail.HuId);
                            CycleCountDetail cycleCountDetail = new CycleCountDetail();
                            cycleCountDetail.Id = transformerDetail.Id;
                            cycleCountDetail.HuId = transformerDetail.HuId;
                            cycleCountDetail.LotNo = transformerDetail.LotNo;
                            cycleCountDetail.Qty = transformerDetail.CurrentQty;
                            cycleCountDetail.Item = itemMgrE.LoadItem(transformerDetail.ItemCode);
                            if (transformerDetail.StorageBinCode != null && transformerDetail.StorageBinCode.Trim() != string.Empty)
                                cycleCountDetail.StorageBin = storageBinMgrE.LoadStorageBin(transformerDetail.StorageBinCode).Code;
                            cycleCountDetailList.Add(cycleCountDetail);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Item
                    if (transformer.Id == 0 ||
                        (transformer.ItemCode != null && transformer.ItemCode.Trim() != string.Empty))
                    {
                        CycleCountDetail cycleCountDetail = new CycleCountDetail();
                        cycleCountDetail.Id = transformer.Id;
                        cycleCountDetail.Item = itemMgrE.LoadItem(transformer.ItemCode);
                        cycleCountDetail.Qty = transformer.CurrentQty;
                        cycleCountDetailList.Add(cycleCountDetail);
                    }
                    #endregion
                }
            }
            return cycleCountDetailList;
        }
        private List<Transformer> ConvertCycleCountDetailToTransformers(IList<CycleCountDetail> cycleCountDetailList)
        {
            List<Transformer> transformerList = new List<Transformer>();

            #region Item
            var qItem =
                from cycleCountDetail in cycleCountDetailList
                where cycleCountDetail.HuId == null
                select cycleCountDetail;
            foreach (var item in qItem)
            {
                transformerList.Add(this.ConvertCycleCountDetailToTransformer(item));
            }
            #endregion

            #region Hu
            var qHu =
                from cycleCountDetail in cycleCountDetailList
                where cycleCountDetail.HuId != null
                select cycleCountDetail;
            #endregion

            foreach (CycleCountDetail cycleCountDetail in cycleCountDetailList)
            {
                if (cycleCountDetail.HuId == null)
                {

                }
                else
                {

                }
            }

            return transformerList;
        }

        private Transformer ConvertCycleCountDetailToTransformer(CycleCountDetail cycleCountDetail)
        {
            Transformer transformer = TransformerHelper.ConvertItemToTransformer(cycleCountDetail.Item);
            transformer.Id = cycleCountDetail.Id;
            transformer.OrderNo = cycleCountDetail.CycleCount.Code;
            transformer.Qty = cycleCountDetail.Qty;

            return transformer;
        }

        #endregion
    }
}



﻿
#region Extend Class



namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class StockTakingMgrE : com.Sconit.Service.Business.Impl.StockTakingMgr, IBusinessMgrE
    {
        
    }
}

#endregion
