using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class ShiftPlanSchedule : ShiftPlanScheduleBase
    {
        #region Non O/R Mapping Properties

        //TODO: Add Non O/R Mapping Properties here. 
        public Bom Bom { get; set; }
        public string Remark { get; set; }

        #endregion
    }
}