using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.Quote
{
    [Serializable]
    public class CusTemplateBase
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
        /// CustomerCode
        /// </summary>
        public string CustomerCode
        {
            get;
            set;
        }
        /// <summary>
        /// CustomerName
        /// </summary>
        public string CustomerName
        {
            get;
            set;
        }
        /// <summary>
        /// CostCategory
        /// </summary>
        public CostCategory CostCategory
        {
            get;
            set;
        }
        /// <summary>
        /// CostList
        /// </summary>
        public CostList CostList
        {
            get;
            set;
        }

        public int SortId
        {
            get;
            set;
        }
    }
}
