using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.Quote
{
    [Serializable]
    public abstract class ToolingBase : EntityBase
    {
        public string TL_No
        {
            get;
            set;
        }
        /// <summary>
        /// TL_Name
        /// </summary>
        public string TL_Name
        {
            get;
            set;
        }
        /// <summary>
        /// TL_Spec
        /// </summary>
        public string TL_Spec
        {
            get;
            set;
        }
        /// <summary>
        /// Customer
        /// </summary>
        public string Customer
        {
            get;
            set;
        }
        /// <summary>
        /// ProductName
        /// </summary>
        public string ProductName
        {
            get;
            set;
        }
        /// <summary>
        /// PCBNo
        /// </summary>
        public string PCBNo
        {
            get;
            set;
        }
        /// <summary>
        /// MSNo
        /// </summary>
        public string MSNo
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
        /// <summary>
        /// Number
        /// </summary>
        public int? Number
        {
            get;
            set;
        }
        /// <summary>
        /// ApplyDate
        /// </summary>
        public DateTime? ApplyDate
        {
            get;
            set;
        }
        /// <summary>
        /// ApplyUser
        /// </summary>
        public string ApplyUser
        {
            get;
            set;
        }
        /// <summary>
        /// ArriveDate
        /// </summary>
        public DateTime? ArriveDate
        {
            get;
            set;
        }
        /// <summary>
        /// TL_SalesType
        /// </summary>
        public string TL_SalesType
        {
            get;
            set;
        }
        /// <summary>
        /// Suppliers
        /// </summary>
        public string Suppliers
        {
            get;
            set;
        }
        /// <summary>
        /// SuppliersInNo
        /// </summary>
        public string SuppliersInNo
        {
            get;
            set;
        }
        /// <summary>
        /// CustomerBStatus
        /// </summary>
        public bool CustomerBStatus
        {
            get;
            set;
        }
        /// <summary>
        /// ProjectNo
        /// </summary>
        public string ProjectNo
        {
            get;
            set;
        }

        public DateTime? CustomerBillDate
        {
            get;
            set;
        }

        public string CustomerBillNo
        {
            get;
            set;
        }
    }
}
