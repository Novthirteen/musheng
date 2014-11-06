using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using System.Reflection;

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class LanguageMgr : LanguageBaseMgr, ILanguageMgr
    {
        public string languageFileFolder { get; set; }
        public IDictionary<string, IDictionary<string, string>> languageDic;
        public ICodeMasterMgrE codeMasterMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }

        protected Regex regex = new Regex("\\${[\\w \\. , \\- * _ ( )]+?}", RegexOptions.Singleline);
        protected char[] prefix = new char[] { '$', '{' };
        protected char[] surfix = new char[] { '}' };


        #region ILanguageMgrE Members
        public string ProcessLanguage(string content, string language)
        {
            if (languageDic == null || languageDic.Count == 0)
            {
                this.GetLanguage();
            }
            if (languageDic.ContainsKey(language))
            {
                IDictionary<string, string> targetLanguageDic = languageDic[language];
                MatchCollection mc = regex.Matches(content);

                var r = from Match m in mc
                        where mc != null
                        where mc.Count > 0
                        group m by m.Value into result
                        select new
                        {
                            Value = result.Key
                        };

                foreach (var p in r)
                {
                    string[] splitKey = p.Value.TrimStart(prefix).TrimEnd(surfix).Split(',');
                    string actualKey = splitKey[0];
                    if (targetLanguageDic.ContainsKey(actualKey))
                    {
                        //处理Message中的参数
                        string value = targetLanguageDic[actualKey];
                        if (splitKey.Length > 1)
                        {
                            string[] para = new string[splitKey.Length - 1];
                            for (int j = 1; j < splitKey.Length; j++)
                            {
                                para[j - 1] = splitKey[j].Trim();
                            }
                            try
                            {
                                value = string.Format(value, para);
                            }
                            catch (Exception ex)
                            {
                                //throw;
                            }
                        }

                        content = content.Replace(p.Value, value);
                    }
                }
            }
            else
            {
                throw new TechnicalException("没有指定类型的语言：" + language);
            }

            return content;
        }

        public void ReLoadLanguage()
        {
            this.GetLanguage();
        }
        public string TranslateMessage(string content, string userCode)
        {
            return this.TranslateMessage(content, userCode, null);
        }
        public string TranslateMessage(string content, User user)
        {
            return this.TranslateMessage(content, user, null);
        }
        public string TranslateMessage(string content, string userCode, params string[] parameters)
        {
            User user = userMgrE.LoadUser(userCode, true, false);
            return this.TranslateMessage(content, user, parameters);
        }
        public string TranslateMessage(string content, User user, params string[] parameters)
        {
            //try
            //{
            //    content = ProcessMessage(content, parameters);

            //    if (user != null && user.UserLanguage != null && user.UserLanguage != string.Empty)
            //    {
            //        content = this.ProcessLanguage(content, user.UserLanguage);
            //    }
            //    else
            //    {
            //        EntityPreference defaultLanguage = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_LANGUAGE);
            //        content = this.ProcessLanguage(content, defaultLanguage.Value);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new TechnicalException("翻译时出现异常:" + ex.Message);
            //}
            //return content;

            string language = null;
            if (user != null && user.UserLanguage != null && user.UserLanguage != string.Empty)
            {
                language = user.UserLanguage;
            }
            else
            {
                EntityPreference defaultLanguage = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_LANGUAGE);
                language = defaultLanguage.Value;
            }
            return TranslateContent(content, language, parameters);
        }

        public string TranslateContent(string content, string language, params string[] parameters)
        {
            try
            {
                content = ProcessMessage(content, parameters);
                if (language == null || language.Trim() == string.Empty)
                {
                    EntityPreference defaultLanguage = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_LANGUAGE);
                    language = defaultLanguage.Value;
                }
                content = this.ProcessLanguage(content, language);
            }
            catch (Exception ex)
            {
                throw new TechnicalException("TranslateContent Exception:" + ex.Message);
            }
            return content;
        }
        #endregion

        private void GetLanguage()
        {
            languageDic = new Dictionary<string, IDictionary<string, string>>();

            IList<Language> languages = this.GetAllLanguage();

            PropertyInfo[] myPropertyInfo = typeof(Language).GetProperties();
            IList<CodeMaster> codeMasters = codeMasterMgrE.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_LANGUAGE);

            //foreach (CodeMaster codeMaster in codeMasters)
            //{
            //    string languageKey = codeMaster.Value;
            //    IDictionary<string, string> targetLanguageDic = new Dictionary<string, string>();
            //    foreach (Language language in languages)
            //    {
            //        foreach (PropertyInfo pi in myPropertyInfo)
            //        {
            //            if (pi.Name != null && languageKey.ToLower().Contains(pi.Name.ToLower()))
            //            {
            //                string lang = string.Empty;

            //                if (pi.GetValue(language, null) != null)
            //                {
            //                    lang = pi.GetValue(language, null).ToString();
            //                }

            //                targetLanguageDic.Add((myPropertyInfo[0].GetValue(language, null)).ToString(), lang);
            //            }
            //        }
            //    }
            //    languageDic.Add(languageKey, targetLanguageDic);
            //}
            foreach (CodeMaster codeMaster in codeMasters)
            {
                string languageKey = codeMaster.Value;
                string resourceFile = languageFileFolder + "/Language_" + languageKey + ".properties";
                IDictionary<string, string> targetLanguageDic = new Dictionary<string, string>();

                PropertyFileReader propertyFileReader = new PropertyFileReader(resourceFile);
                while (!propertyFileReader.EndOfStream)
                {
                    string[] property = propertyFileReader.GetPropertyLine();
                    if (property != null)
                    {
                        try
                        {
                            targetLanguageDic.Add(property[0].Trim(), property[1].Trim());
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                try
                {
                    string resourceExtFile = languageFileFolder + "/Language-ext_" + languageKey + ".properties";
                    if (File.Exists(resourceExtFile))
                    {
                        PropertyFileReader propertyExtFileReader = new PropertyFileReader(resourceExtFile);
                        while (!propertyExtFileReader.EndOfStream)
                        {
                            string[] property = propertyExtFileReader.GetPropertyLine();
                            if (property != null)
                            {
                                try
                                {
                                    if (targetLanguageDic.ContainsKey(property[0].Trim()))
                                    {
                                        targetLanguageDic.Remove(property[0].Trim());
                                    }
                                    targetLanguageDic.Add(property[0].Trim(), property[1].Trim());
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                }

                languageDic.Add(languageKey, targetLanguageDic);
            }
        }

        protected string ProcessMessage(string message, string[] paramters)
        {
            string messageParams = string.Empty;
            if (paramters != null && paramters.Length > 0)
            {
                //处理Message参数
                foreach (string para in paramters)
                {
                    messageParams += "," + para;
                }
            }
            message = "${" + message + messageParams + "}";

            return message;
        }
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class LanguageMgrE : com.Sconit.Service.MasterData.Impl.LanguageMgr, ILanguageMgrE
    {
    }
}

#endregion Extend Class