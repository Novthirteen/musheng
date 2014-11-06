using Castle.Services.Transaction;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using System.Collections.Generic;
using com.Sconit.Service.Ext.Business;

namespace com.Sconit.Service.Business.Impl
{
    public class ReloadMaterialMgr : AbstractBusinessMgr
    {
        public IUserMgrE userMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IProductLineInProcessLocationDetailMgrE productLineInProcessLocationDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
        }

        protected override void GetDetail(Resolver resolver)
        {

        }

        protected override void SetDetail(Resolver resolver)
        {
            if (resolver.Transformers == null || resolver.Transformers.Count != 2)
            {
                resolver.Transformers = new List<Transformer>();
                resolver.Transformers.Add(new Transformer());
                resolver.Transformers.Add(new Transformer());
            }

            Hu hu = huMgrE.CheckAndLoadHu(resolver.Input.Trim());
            TransformerDetail td = Utility.TransformerHelper.ConvertHuToTransformerDetail(hu);

            if (resolver.IOType == BusinessConstants.IO_TYPE_IN)
            {
                resolver.Transformers[0] = new Transformer();
                resolver.Transformers[0].AddTransformerDetail(td);
            }
            else if (resolver.IOType == BusinessConstants.IO_TYPE_OUT)
            {
                //if (resolver.Transformers == null || resolver.Transformers.Count != 2)
                //{
                //    throw new BusinessErrorException("ReloadMaterial.BarCode.Count.Two");
                //}
                resolver.Transformers[1] = new Transformer();
                resolver.Transformers[1].AddTransformerDetail(td);
            }
            else
            {
                throw new BusinessErrorException("ReturnMaterial.BarCode.Type.Error");
            }
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            if (resolver.Transformers == null || resolver.Transformers.Count != 2
                || resolver.Transformers[0].TransformerDetails == null || resolver.Transformers[0].TransformerDetails.Count == 0
                || resolver.Transformers[1].TransformerDetails == null || resolver.Transformers[1].TransformerDetails.Count == 0)
            {
                throw new BusinessErrorException("ReloadMaterial.BarCode.Count.Two");
            }

            resolver.Result = languageMgrE.TranslateMessage("ReloadMaterial.Successfully", resolver.UserCode);
            productLineInProcessLocationDetailMgrE.ExchangeProdutLineFeed(resolver.Transformers[0].TransformerDetails[0].HuId,
                resolver.Transformers[1].TransformerDetails[0].HuId, this.userMgrE.LoadUser(resolver.UserCode), resolver.AllowExceed);

        }

        protected override void ExecuteCancel(Resolver resolver)
        {
            executeMgrE.CancelRepackOperation(resolver);
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
    public partial class ReloadMaterialMgrE : com.Sconit.Service.Business.Impl.ReloadMaterialMgr, IBusinessMgrE
    {


    }
}

#endregion
