using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.Batch;
using Castle.Windsor;

namespace com.Sconit.Service.Batch
{
    [Serializable]
    public class JobRunContext
    {
        protected BatchTrigger trigger;
        protected BatchJobDetail jobDetail;
        protected JobDataMap jobDataMap;
        protected IWindsorContainer container;

        public JobRunContext(BatchTrigger trigger, BatchJobDetail jobDetail, JobDataMap jobDataMap, IWindsorContainer container)
        {
            this.trigger = trigger;
            this.jobDetail = jobDetail;
            this.jobDataMap = jobDataMap;
            this.container = container;
        }

        public BatchTrigger BatchTrigger
        {
            get
            {
                return trigger;
            }
        }

        public BatchJobDetail BatchJobDetail
        {
            get
            {
                return jobDetail;
            }
        }

        public JobDataMap JobDataMap
        {
            get
            {
                return jobDataMap;
            }
        }

        public IWindsorContainer Container
        {
            get
            {
                return container;
            }
        }
    }
}



#region Extend Class


namespace com.Sconit.Service.Ext.Batch
{
    [Serializable]
    public partial class JobRunContextE : com.Sconit.Service.Batch.JobRunContext
    {
        public JobRunContextE(BatchTrigger trigger, BatchJobDetail jobDetail, JobDataMap jobDataMap, IWindsorContainer container)
            : base(trigger, jobDetail, jobDataMap, container)
        {
            this.trigger = trigger;
            this.jobDetail = jobDetail;
            this.jobDataMap = jobDataMap;
            this.container = container;
        }
    }
}

#endregion
