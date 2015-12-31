using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class ItemPackBase
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
        /// Spec
        /// </summary>
        public string Spec
        {
            get;
            set;
        }
        /// <summary>
        /// Descr
        /// </summary>
        public string Descr
        {
            get;
            set;
        }
        /// <summary>
        /// PinNum
        /// </summary>
        public decimal? PinNum
        {
            get;
            set;
        }
        /// <summary>
        /// PinConversion
        /// </summary>
        public decimal? PinConversion
        {
            get;
            set;
        }        
    }
}
