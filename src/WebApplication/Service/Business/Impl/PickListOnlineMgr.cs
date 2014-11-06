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

namespace com.Sconit.Service.Business.Impl
{
    public class PickListOnlineMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IPickListMgrE pickListMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
            this.StartPickList(resolver);
        }

        protected override void GetDetail(Resolver resolver)
        {
        }

        protected override void SetDetail(Resolver resolver)
        {
            //this.StartPickList(resolver);
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
        }

        private void StartPickList(Resolver resolver)
        {
            if (resolver.BarcodeHead == BusinessConstants.CODE_PREFIX_PICKLIST)
            {
                //setBaseMgrE.FillResolverByPickList(resolver);
                pickListMgrE.StartPickList(resolver.Input, userMgrE.CheckAndLoadUser(resolver.UserCode));
                //resolver.Result = DateTime.Now.ToString("HH:mm:ss");
                resolver.OrderNo = resolver.Input;
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

﻿
#region Extend Class
namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class PickListOnlineMgrE : com.Sconit.Service.Business.Impl.PickListOnlineMgr, IBusinessMgrE
    {
        
    }
}

#endregion
