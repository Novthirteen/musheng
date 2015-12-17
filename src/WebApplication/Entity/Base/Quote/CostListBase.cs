using com.Sconit.Entity.Quote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.Quote
{
    [Serializable]
    public class CostListBase
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
        /// <summary>
        /// CCId
        /// </summary>
        public CostCategory CCId
        {
            get;
            set;
        }
        /// <summary>
        /// Number
        /// </summary>
        public string Number
        {
            get;
            set;
        }
        /// <summary>
        /// Unit
        /// </summary>
        public string Unit
        {
            get;
            set;
        }
        /// <summary>
        /// Price
        /// </summary>
        public string Price
        {
            get;
            set;
        }   
    }
}
