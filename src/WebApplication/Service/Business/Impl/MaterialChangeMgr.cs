using Castle.Services.Transaction;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using System.Collections.Generic;

namespace com.Sconit.Service.Business.Impl
{
    public class MaterialChangeMgr : AbstractBusinessMgr
    {
        public IUserMgrE userMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IProductLineInProcessLocationDetailMgrE productLineInProcessLocationDetailMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
        }

        protected override void GetDetail(Resolver resolver)
        {

        }

        protected override void SetDetail(Resolver resolver)
        {
            Hu hu = huMgrE.CheckAndLoadHu(resolver.Input.Trim());
            TransformerDetail td = Utility.TransformerHelper.ConvertHuToTransformerDetail(hu);

            if (resolver.IOType == BusinessConstants.IO_TYPE_IN)
            {
                resolver.Transformers[0] = new Transformer();
                resolver.Transformers[0].AddTransformerDetail(td);
            }
            else if (resolver.IOType == BusinessConstants.IO_TYPE_OUT)
            {
                if (resolver.Transformers == null || resolver.Transformers.Count != 2)
                {
                    throw new BusinessErrorException("换料条码数不是2");
                }
                resolver.Transformers[1] = new Transformer();
                resolver.Transformers[1].AddTransformerDetail(td);
            }
            else
            {
                throw new BusinessErrorException("未知类型");
            }
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            if (resolver.Transformers == null || resolver.Transformers.Count != 2)
            {
                throw new BusinessErrorException("换料条码数不是2");
            }

            productLineInProcessLocationDetailMgrE.ExchangeProdutLineFeed(resolver.Transformers[0].TransformerDetails[0].HuId,
                resolver.Transformers[1].TransformerDetails[0].HuId, this.userMgrE.LoadUser(resolver.UserCode));

        }

        protected override void ExecuteCancel(Resolver resolver)
        {

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
    public partial class MaterialChangeMgrE : com.Sconit.Service.Business.Impl.MaterialChangeMgr, IBusinessMgrE
    {


    }
}

#endregion
