using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Utility;

namespace com.Sconit.Service.Business.Impl
{
    public class FlushBackMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IProductLineInProcessLocationDetailMgrE productLineInProcessLocationDetailMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }

        

        protected override void SetBaseInfo(Resolver resolver)
        {
            setBaseMgrE.FillResolverByFlow(resolver);
            if (resolver.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                throw new BusinessErrorException("Flow.ShipReturn.Error.FlowTypeIsNotDistribution", resolver.OrderType);
            }
        }

        protected override void GetDetail(Resolver resolver)
        {
            setDetailMgrE.SetMateria(resolver);
            var q = resolver.Transformers.Where(t => t.Qty > 0);
            resolver.Transformers = q.ToList();
        }

        protected override void SetDetail(Resolver resolver)
        {
            if (resolver.CodePrefix == string.Empty)
            {
                throw new BusinessErrorException("Common.Business.Error.ScanProductLineFirst");
            }
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            User user = userMgrE.LoadUser(resolver.UserCode, false, true);
            IDictionary<string, decimal> itemDictionary = new Dictionary<string, decimal>();
            foreach (Transformer transformer in resolver.Transformers)
            {
                itemDictionary.Add(transformer.ItemCode, transformer.CurrentQty);
            }
            productLineInProcessLocationDetailMgrE.RawMaterialBackflush(resolver.Code, itemDictionary, user);
            resolver.Result = languageMgrE.TranslateMessage("MasterData.BackFlush.Successfully", resolver.UserCode, resolver.Code);
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


#region Extend Class

namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class FlushBackMgrE : com.Sconit.Service.Business.Impl.FlushBackMgr, IBusinessMgrE
    {
       

    }
}

#endregion
