using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ILanguageBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateLanguage(Language entity);

        Language LoadLanguage(String code);

        IList<Language> GetAllLanguage();
    
        void UpdateLanguage(Language entity);

        void DeleteLanguage(String code);
    
        void DeleteLanguage(Language entity);
    
        void DeleteLanguage(IList<String> pkList);
    
        void DeleteLanguage(IList<Language> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
