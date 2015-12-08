using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.Quote
{
    [Serializable]
    public class QuoteItemBase
    {
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// ItemCode
        /// </summary>
        public string ItemCode
        {
            get;
            set;
        }
        /// <summary>
        /// Supplier
        /// </summary>
        public string Supplier
        {
            get;
            set;
        }
        /// <summary>
        /// Category
        /// </summary>
        public string Category
        {
            get;
            set;
        }
        /// <summary>
        /// Brand
        /// </summary>
        public string Brand
        {
            get;
            set;
        }
        /// <summary>
        /// Model
        /// </summary>
        public string Model
        {
            get;
            set;
        }
        /// <summary>
        /// SingleNum
        /// </summary>
        public string SingleNum
        {
            get;
            set;
        }
        /// <summary>
        /// PurchasePrice
        /// </summary>
        public decimal? PurchasePrice
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
        /// PinNum
        /// </summary>
        public string PinNum
        {
            get;
            set;
        }
        /// <summary>
        /// PinConversion
        /// </summary>
        public string PinConversion
        {
            get;
            set;
        }
        /// <summary>
        /// Point
        /// </summary>
        public string Point
        {
            get;
            set;
        }

        public string ProjectId
        {
            get;
            set;
        }
    }
}
