using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class QuoteCustomerInfoBase : EntityBase
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
        /// Code
        /// </summary>
        public string Code
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
        /// SType
        /// </summary>
        public string SType
        {
            get;
            set;
        }

        /// <summary>
        /// BillPeriod
        /// </summary>
        public int? BillPeriod
        {
            get;
            set;
        }

        /// <summary>
        /// P_LossRate
        /// </summary>
        public string P_LossRate
        {
            get;
            set;
        }

        /// <summary>
        /// P_ManageFee
        /// </summary>
        public string P_ManageFee
        {
            get;
            set;
        }

        /// <summary>
        /// P_FinanceFee
        /// </summary>
        public string P_FinanceFee
        {
            get;
            set;
        }

        /// <summary>
        /// P_Profit
        /// </summary>
        public string P_Profit
        {
            get;
            set;
        }

        /// <summary>
        /// M_ManageFee
        /// </summary>
        public string M_ManageFee
        {
            get;
            set;
        }

        /// <summary>
        /// M_FinanceFee
        /// </summary>
        public string M_FinanceFee
        {
            get;
            set;
        }

        /// <summary>
        /// DeliveryAdd
        /// </summary>
        public string DeliveryAdd
        {
            get;
            set;
        }

        /// <summary>
        /// DeliveryCity
        /// </summary>
        public string DeliveryCity
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
        /// StartDate
        /// </summary>
        public DateTime? StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// EndDate
        /// </summary>
        public DateTime? EndDate
        {
            get;
            set;
        }
    }
}
