using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class PlanView : EntityBase
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemReference { get; set; }
        public string UomCode { get; set; }
        public decimal Uc { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        //Qty 7Day
        public decimal D1 { get; set; }
        public decimal D2 { get; set; }
        public decimal D3 { get; set; }
        public decimal D4 { get; set; }
        public decimal D5 { get; set; }
        public decimal D6 { get; set; }
        public decimal D7 { get; set; }
        //Qty 5Week
        public decimal D8 { get; set; }
        public decimal D9 { get; set; }
        public decimal D10 { get; set; }
        public decimal D11 { get; set; }
        public decimal D12 { get; set; }
        //Qty 3Month
        public decimal D13 { get; set; }
        public decimal D14 { get; set; }
        public decimal D15 { get; set; }
    }
}