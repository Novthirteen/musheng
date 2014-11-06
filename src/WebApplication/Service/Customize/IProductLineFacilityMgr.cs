using System;
using System.Collections.Generic;
using com.Sconit.Entity.Customize;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize
{
    public interface IProductLineFacilityMgr : IProductLineFacilityBaseMgr
    {
        #region Customized Methods

        bool HasProductLineFacility(string productionLineCode);

        IList<ProductLineFacility> GetProductLineFacility(string productionLineCode);

        ProductLineFacility CheckAndLoadProductLineFacility(string productLineFacilityCode);

        ProductLineFacility GetPLFacility(string productLineFacilityCode);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Customize
{
    public partial interface IProductLineFacilityMgrE : com.Sconit.Service.Customize.IProductLineFacilityMgr
    {
    }
}

#endregion Extend Interface