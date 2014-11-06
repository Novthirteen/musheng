using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
namespace com.Sconit.Service.MasterData
{
    public interface IShipAddressMgr : IShipAddressBaseMgr
    {
        #region Customized Methods

        ShipAddress GetDefaultShipAddress(string partyCode);

        ShipAddress GetDefaultShipAddress(Party party);

        #endregion Customized Methods
    }
}





#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IShipAddressMgrE : com.Sconit.Service.MasterData.IShipAddressMgr
    {
        #region Customized Methods

        ShipAddress GetDefaultShipAddress(string partyCode);

        ShipAddress GetDefaultShipAddress(Party party);

        #endregion Customized Methods
    }
}

#endregion
