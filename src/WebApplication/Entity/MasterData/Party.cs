using System;
using com.Sconit.Entity.Exception;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class Party : PartyBase
    {
        #region Non O/R Mapping Properties
        public virtual string Type
        {
            get
            {
                if (this is Region)
                {
                    return BusinessConstants.PARTY_TYPE_REGION;
                }
                else if (this is Supplier)
                {
                    return BusinessConstants.PARTY_TYPE_SUPPLIER;
                }
                else if (this is Customer)
                {
                    return BusinessConstants.PARTY_TYPE_CUSTOMER;
                }
                else
                {
                    throw new TechnicalException("Unknow party type " + this.GetType());
                }
            }
        }
        #endregion
    }
}