using System;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ILanguageMgr : ILanguageBaseMgr
    {
        #region Customized Methods

        string ProcessLanguage(string content, string language);

        void ReLoadLanguage();

        string TranslateMessage(string content, string userCode);

        string TranslateMessage(string content, User user);

        string TranslateMessage(string content, string userCode, params string[] parameters);

        string TranslateMessage(string content, User user, params string[] parameters);

        string TranslateContent(string content, string language, params string[] parameters);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ILanguageMgrE : com.Sconit.Service.MasterData.ILanguageMgr
    {
    }
}

#endregion Extend Interface
