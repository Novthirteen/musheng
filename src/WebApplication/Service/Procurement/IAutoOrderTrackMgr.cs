
//TODO: Add other using statements here.

namespace com.Sconit.Service.Procurement
{
    public interface IAutoOrderTrackMgr : IAutoOrderTrackBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}



#region Extend Interface



namespace com.Sconit.Service.Ext.Procurement
{
    public partial interface IAutoOrderTrackMgrE : com.Sconit.Service.Procurement.IAutoOrderTrackMgr
    {
        
    }
}

#endregion
