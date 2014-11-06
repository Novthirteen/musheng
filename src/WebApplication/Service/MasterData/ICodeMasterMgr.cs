using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ICodeMasterMgr : ICodeMasterBaseMgr
    {
        #region Customized Methods

        IList<CodeMaster> GetCachedCodeMaster(string code);

        CodeMaster GetCachedCodeMaster(string code, string value);

        IList<CodeMaster> GetCodeMasterList(string code, object[] valueArray);

        CodeMaster GetDefaultCodeMaster(string code);

        string GetRandomTheme(string themeType);

        IList<CodeMaster> GetAllCodeMaster();
        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ICodeMasterMgrE : com.Sconit.Service.MasterData.ICodeMasterMgr
    {
        
    }
}

#endregion
