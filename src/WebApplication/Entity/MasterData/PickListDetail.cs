using System;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class PickListDetail : PickListDetailBase
    {
        #region Non O/R Mapping Properties

        public string SortLevel1
        {
            get
            {
                string _SortLevel1 = null;

                _SortLevel1 = ((this.SortLevel1From != null ? this.SortLevel1From : string.Empty) + "-"
                    + ((this.SortLevel1To != null) ? this.SortLevel1To : string.Empty));

                return _SortLevel1;
            }
        }

        public string SortLevel2
        {
            get
            {
                string _SortLevel2 = null;

                _SortLevel2 = ((this.SortLevel2From != null ? this.SortLevel2From : string.Empty) + "-"
                    + ((this.SortLevel2To != null) ? this.SortLevel2To : string.Empty));

                return _SortLevel2;
            }
        }


        public string ColorLevel1
        {
            get
            {
                string _ColorLevel1 = null;

                _ColorLevel1 = ((this.ColorLevel1From != null ? this.ColorLevel1From : string.Empty) + "-"
                    + ((this.ColorLevel1To != null) ? this.ColorLevel1To : string.Empty));

                return _ColorLevel1;
            }
        }

        public string ColorLevel2
        {
            get
            {
                string _ColorLevel2 = null;

                _ColorLevel2 = ((this.ColorLevel2From != null ? this.ColorLevel2From : string.Empty) + "-"
                        + ((this.ColorLevel2To != null) ? this.ColorLevel2To : string.Empty));

                return _ColorLevel2;
            }
        }

        public void AddPickListResult(PickListResult pickListResult)
        {
            if (this.PickListResults == null)
            {
                this.PickListResults = new List<PickListResult>();
            }

            this.PickListResults.Add(pickListResult);
        }

        public void RemovePickListResult(PickListResult pickListResult)
        {
            if (this.PickListResults != null)
            {
                this.PickListResults.Remove(pickListResult);
            }
        }

        #endregion
    }
}