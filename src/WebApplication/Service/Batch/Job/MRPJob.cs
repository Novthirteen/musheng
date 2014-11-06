using com.Sconit.Service.Ext.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Utility;
using com.Sconit.Entity.Exception;
using System.IO;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Service.MasterData;
using com.Sconit.Entity.Production;
using com.Sconit.Service.Ext.MRP;

namespace com.Sconit.Service.Batch.Job
{
    [Transactional]
    public class MRPJob : IJob
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.BatchJob");

        public IMrpMgrE mrpMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }

        public void Execute(JobRunContext context)
        {
            mrpMgrE.RunMrp(DateTime.Now.Date, this.userMgrE.GetMonitorUser());
        }
    }
}

#region Extend Class
namespace com.Sconit.Service.Ext.Batch.Job
{
    [Transactional]
    public partial class MRPJob : com.Sconit.Service.Batch.Job.MRPJob
    {
        public MRPJob()
        {
        }
    }
}

#endregion