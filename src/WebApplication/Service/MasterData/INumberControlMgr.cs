using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Distribution;
namespace com.Sconit.Service.MasterData
{
    public interface INumberControlMgr : INumberControlBaseMgr
    {
        #region Customized Methods

        string GenerateNumber(string code);

        string GenerateNumber(string code, int numberSuffixLength);

        string GenerateHuId(int itemId, string sortAndColor);

        string GenerateOrderNo(string orderType);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface INumberControlMgrE : com.Sconit.Service.MasterData.INumberControlMgr
    {
       
    }
}

#endregion
