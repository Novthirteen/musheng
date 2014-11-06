using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using Castle.Services.Transaction;
using com.Sconit.Entity.Customize;
using com.Sconit.Service.Ext.Customize;

namespace com.Sconit.Service.Business.Impl
{
    public class OnlineMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IOrderMgrE orderMgrE { get; set; }
        public IProductLineFacilityMgrE productLineFacilityMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
            if (resolver.CodePrefix == BusinessConstants.BARCODE_HEAD_FLOW_FACILITY)
            {
                ProductLineFacility productLineFacility = productLineFacilityMgrE.CheckAndLoadProductLineFacility(resolver.Input);
                Flow flow = flowMgrE.LoadFlow(productLineFacility.ProductLine);
                setBaseMgrE.FillResolverByFlow(resolver, flow);
                resolver.Code = productLineFacility.Code;
                resolver.OrderNo = null;
                resolver.FlowFacility = productLineFacility.Code;
                resolver.Result = languageMgrE.TranslateMessage("Production.Online.Current.Status", resolver.UserCode, resolver.FlowCode, resolver.FlowFacility);
            }
            else if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_WORK_ORDER)
            {
                this.StartOrder(resolver);
            }
            else
            {
                throw new BusinessErrorException("ReturnMaterial.BarCode.Type.Error");
            }
        }

        protected override void GetDetail(Resolver resolver)
        {
        }

        protected override void SetDetail(Resolver resolver)
        {
            //this.StartOrder(resolver);
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
        }

        private void StartOrder(Resolver resolver)
        {
            if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_WORK_ORDER)
            {
                setBaseMgrE.FillResolverByOrder(resolver);

                #region 校验
                if (resolver.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
                {
                    throw new BusinessErrorException("Common.Business.Error.StatusError", resolver.OrderNo, resolver.Status);
                }

                if (resolver.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                {
                    throw new BusinessErrorException("Order.Error.OrderOfflineIsNotProduction", resolver.OrderNo, resolver.OrderType);
                }
                #endregion

                orderMgrE.StartOrder(resolver.OrderNo, resolver.UserCode, resolver.FlowFacility);


                //resolver.Result = DateTime.Now.ToString("HH:mm:ss");
                //resolver.Code = resolver.Input;
            }
            else
            {
                throw new BusinessErrorException("Common.Business.Error.BarCodeInvalid");
            }

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





#region Extend Class
namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class OnlineMgrE : com.Sconit.Service.Business.Impl.OnlineMgr, IBusinessMgrE
    {

    }
}

#endregion
