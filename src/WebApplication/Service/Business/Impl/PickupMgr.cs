using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using Castle.Services.Transaction;

namespace com.Sconit.Service.Business.Impl
{
    public class PickupMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
            throw new BusinessErrorException("Common.Business.Error.BarCodeInvalid");
        }

        protected override void GetDetail(Resolver resolver)
        {
        }

        protected override void SetDetail(Resolver resolver)
        {
            //setDetailMgrE.CheckHuInTransformerDetails(resolver);
            LocationLotDetail locationLotDetail = locationLotDetailMgrE.CheckLoadHuLocationLotDetail(resolver.Input, resolver.UserCode);
            //Hu hu = locationLotDetail.Hu;
            //hu.Qty = locationLotDetail.Qty / hu.UnitQty;
            if (this.locationMgrE.IsHuOcuppyByPickList(resolver.Input))
            {
                throw new BusinessErrorException("Order.Error.PickUp.HuOcuppied", resolver.Input);
            }

            //已经下架
            if (locationLotDetail.StorageBin == null)
            {
                throw new BusinessErrorException("Warehouse.PickUp.Error.IsAlreadyPickUp", resolver.Input);
            }
            TransformerDetail transformerDetail = TransformerHelper.ConvertLocationLotDetailToTransformerDetail(locationLotDetail, false);
            resolver.AddTransformerDetail(transformerDetail);
            //transformerDetail.CurrentQty = transformerDetail.Qty;
            //int maxSeq = setDetailMgrE.FindMaxSeq(resolver.Transformers);
            //transformerDetail.Sequence = maxSeq + 1;
            //resolver.Transformers[0].TransformerDetails.Add(transformerDetail);
            //resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMERDETAIL;
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            if (resolver.Transformers == null || resolver.Transformers.Count < 1)
            {
                throw new BusinessErrorException("PickUp.Error.HuDetailEmpty");
            }

            IList<LocationLotDetail> locationLotDetailList = executeMgrE.ConvertTransformersToLocationLotDetails(resolver.Transformers, false);
            locationMgrE.InventoryPick(locationLotDetailList, resolver.UserCode);
            resolver.Result = languageMgrE.TranslateMessage("Warehouse.PickUp.Successfully", resolver.UserCode);
            resolver.Transformers = null;
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMERDETAIL;
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
    }
}




﻿
#region Extend Class

namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class PickupMgrE : com.Sconit.Service.Business.Impl.PickupMgr, IBusinessMgrE
    {
        
    }
}

#endregion
