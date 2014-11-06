using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class WorkCenter : WorkCenterBase
    {
        public String DefaultCostCenter
        {
            get
            {
                if (this.CostCenter != null && this.CostCenter.Trim() != string.Empty)
                {
                    return this.CostCenter;
                }
                else
                {
                    return this.Region.CostCenter;
                }
            }
        }
    }
}
