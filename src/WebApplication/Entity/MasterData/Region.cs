using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class Region : Party
    {
        public string CostCenter { get; set; }
        public string CostGroup { get; set; }
        public string InspectLocation { get; set; }
        public string RejectLocation { get; set; }

    }
}
