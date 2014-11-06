using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;

namespace com.Sconit.Service.MasterData
{
    public interface IMiscOrderMgr : IMiscOrderBaseMgr
    {
        #region Customized Methods
        MiscOrder SaveMiscOrder(MiscOrder miscOrder, User user);
        void RemoveMiscOrder(MiscOrder miscOrder);
        MiscOrder ReLoadMiscOrder(string orderNo);
        #endregion Customized Methods
    }
}





#region Extend Interface


namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IMiscOrderMgrE : com.Sconit.Service.MasterData.IMiscOrderMgr
    {
        
    }
}

#endregion
