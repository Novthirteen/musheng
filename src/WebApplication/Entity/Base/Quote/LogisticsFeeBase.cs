using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.Quote
{
    [Serializable]
    public class LogisticsFeeBase
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
        /// CityName
        /// </summary>
        public string CityName
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

        public decimal? Price
        {
            get;
            set;
        }   
    }
}
