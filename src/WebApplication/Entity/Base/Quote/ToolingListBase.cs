using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.Quote
{
    [Serializable]
    public class ToolingListBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

        /// <summary>
        /// Price
        /// </summary>
        public decimal? Price
        {
            get;
            set;
        }   
    }
}
