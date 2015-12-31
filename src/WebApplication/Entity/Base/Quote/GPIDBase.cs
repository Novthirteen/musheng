using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.Quote
{
    [Serializable]
    public class GPIDBase
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// CustomerCode
        /// </summary>
        public string CustomerCode
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
        /// StartDate
        /// </summary>
        public DateTime? StartDate
        {
            get;
            set;
        }
        /// <summary>
        /// Status
        /// </summary>
        public bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// Product
        /// </summary>
        public string Product
        {
            get;
            set;
        }
        /// <summary>
        /// EndCustomer
        /// </summary>
        public string EndCustomer
        {
            get;
            set;
        }
        /// <summary>
        /// Addr
        /// </summary>
        public string Addr
        {
            get;
            set;
        }
        /// <summary>
        /// LifeCycle
        /// </summary>
        public string LifeCycle
        {
            get;
            set;
        }
        /// <summary>
        /// OTS
        /// </summary>
        public DateTime? OTS
        {
            get;
            set;
        }
        /// <summary>
        /// PPAP
        /// </summary>
        public DateTime? PPAP
        {
            get;
            set;
        }
        /// <summary>
        /// SOP
        /// </summary>
        public DateTime? SOP
        {
            get;
            set;
        }
        /// <summary>
        /// ProjectManager
        /// </summary>
        public string ProjectManager
        {
            get;
            set;
        }
        /// <summary>
        /// Buyer
        /// </summary>
        public string Buyer
        {
            get;
            set;
        }
        /// <summary>
        /// Technology
        /// </summary>
        public string Technology
        {
            get;
            set;
        }
        /// <summary>
        /// Quality
        /// </summary>
        public string Quality
        {
            get;
            set;
        }
        /// <summary>
        /// Desc1
        /// </summary>
        public string Desc1
        {
            get;
            set;
        }
        /// <summary>
        /// Desc2
        /// </summary>
        public string Desc2
        {
            get;
            set;
        }
    }
}
