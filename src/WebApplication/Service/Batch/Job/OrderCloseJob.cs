using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Service.Ext.MasterData;

namespace com.Sconit.Service.Batch.Job
{
    [Transactional]
    public class OrderCloseJob : IJob
    {
        public IOrderMgrE orderMgrE { get; set; }

        

        [Transaction(TransactionMode.Unspecified)]
        public void Execute(JobRunContext context)
        {
            orderMgrE.TryCloseOrder();
        }
    }
}



#region Extend Class


namespace com.Sconit.Service.Ext.Batch.Job
{
    [Transactional]
    public partial class OrderCloseJob : com.Sconit.Service.Batch.Job.OrderCloseJob
    {
        public OrderCloseJob()
        {
        }
    }
}

#endregion
