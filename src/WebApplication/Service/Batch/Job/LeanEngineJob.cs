using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Procurement;
using com.Sconit.Service.Ext.Procurement;

namespace com.Sconit.Service.Batch.Job
{
    public class LeanEngineJob : IJob
    {
        public ILeanEngineMgrE leanEngineMgrE { get; set; }

        

        public void Execute(JobRunContext context)
        {
            //leanEngineMgrE.GenerateOrder();
            leanEngineMgrE.OrderGenerate();
        }
    }
}



#region Extend Class

namespace com.Sconit.Service.Ext.Batch.Job
{
    
    public partial class LeanEngineJob : com.Sconit.Service.Batch.Job.LeanEngineJob
    {
        public LeanEngineJob()
        {
        }
    }
}

#endregion
