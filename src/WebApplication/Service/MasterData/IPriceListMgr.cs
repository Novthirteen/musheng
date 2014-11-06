using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

using com.Sconit.Entity.MasterData;
namespace com.Sconit.Service.MasterData
{
    public interface IPriceListMgr : IPriceListBaseMgr
    {
        #region Customized Methods
        PriceList LoadPriceList(string code, bool includeDetail);

        #endregion Customized Methods
    }
}





#region Extend Interface




namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IPriceListMgrE : com.Sconit.Service.MasterData.IPriceListMgr
    {
        
    }
}

#endregion
