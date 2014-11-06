using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using Castle.Services.Transaction;

namespace com.Sconit.Service.Business.Impl
{
    public class InspectMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IInspectOrderMgrE inspectOrderMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
            if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_INSPECTION)
            {
                InspectOrder inspectOrder = inspectOrderMgrE.CheckAndLoadInspectOrder(resolver.Input);
                if (inspectOrder.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
                {
                    throw new BusinessErrorException("InspectOrder.Error.StatusIsNotValid", resolver.Input, inspectOrder.Status);
                }
                resolver.Code = inspectOrder.InspectNo;
                resolver.IsScanHu = inspectOrder.IsDetailHasHu;
                resolver.Status = inspectOrder.Status;
                if (resolver.IsScanHu)
                {
                    resolver.PickBy = BusinessConstants.CODE_MASTER_PICKBY_HU;
                }
                else
                {
                    resolver.PickBy = BusinessConstants.CODE_MASTER_PICKBY_ITEM;
                }
            }
            else
            {
                throw new BusinessErrorException("Common.Business.Error.BarCodeInvalid");
            }
        }

        protected override void GetDetail(Resolver resolver)
        {
            InspectOrder inspectOrder = inspectOrderMgrE.LoadInspectOrder(resolver.Code, true);
            resolver.Transformers = TransformerHelper.ConvertInspectDetailToTransformer(inspectOrder.InspectOrderDetails);
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
            resolver.Result = languageMgrE.TranslateMessage("MasterData.Inventory.CurrentInspectOrder", resolver.UserCode, resolver.Code);
        }

        protected override void SetDetail(Resolver resolver)
        {
            if (resolver.CodePrefix == string.Empty)
            {
                throw new BusinessErrorException("Common.Business.Error.ScanFlowFirst");
            }
            setDetailMgrE.MatchInspet(resolver);
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void ExecuteSubmit(Resolver resolver)
        {
            IList<InspectOrderDetail> repackDetailList = executeMgrE.ConvertResolverToInspectOrderDetails(resolver);
            inspectOrderMgrE.ProcessInspectOrder(repackDetailList, resolver.UserCode);
            resolver.Result = languageMgrE.TranslateMessage("MasterData.InspectOrder.Process.Successfully", resolver.UserCode, resolver.Code);
            resolver.Transformers = null;
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
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

#region Extend Interface

namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class InspectMgrE : com.Sconit.Service.Business.Impl.InspectMgr, IBusinessMgrE
    {

    }
}

#endregion


