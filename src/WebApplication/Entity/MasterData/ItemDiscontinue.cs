using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class ItemDiscontinue : ItemDiscontinueBase
    {
        #region Non O/R Mapping Properties

        public string DiscontinueItemAndId()
        {
            return this.DiscontinueItem.Code + "[" + this.Id.ToString() + "]";
        }

        #endregion
    }
}