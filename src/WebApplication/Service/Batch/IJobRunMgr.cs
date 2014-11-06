using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;

namespace com.Sconit.Service.Batch
{
    public interface IJobRunMgr
    {
        void RunBatchJobs(IWindsorContainer container);
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Batch
{
    public partial interface IJobRunMgrE : com.Sconit.Service.Batch.IJobRunMgr
    {

    }
}

#endregion

