/// <summary>
///BaseMasterPage 的摘要说明
/// </summary>

using System;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity.Exception;
namespace com.Sconit.Web
{
    public class MasterPageBase : System.Web.UI.MasterPage, IMessage
    {
        public MasterPageBase()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public void ShowSuccessMessage(string message)
        {
            IMessage ucMessage = (IMessage)Page.FindControl("ucMessage");
            if (ucMessage != null)
            {
                ucMessage.ShowSuccessMessage(message);
            }
        }

        public void ShowSuccessMessage(string message, params string[] parameters)
        {
            IMessage ucMessage = (IMessage)Page.FindControl("ucMessage");
            if (ucMessage != null)
            {
                ucMessage.ShowSuccessMessage(message, parameters);
            }
        }

        public void ShowWarningMessage(string message)
        {
            IMessage ucMessage = (IMessage)Page.FindControl("ucMessage");
            if (ucMessage != null)
            {
                ucMessage.ShowWarningMessage(message);
            }
        }

        public void ShowWarningMessage(string message, params string[] parameters)
        {
            IMessage ucMessage = (IMessage)Page.FindControl("ucMessage");
            if (ucMessage != null)
            {
                ucMessage.ShowWarningMessage(message, parameters);
            }
        }

        public void ShowErrorMessage(string message)
        {
            IMessage ucMessage = (IMessage)Page.FindControl("ucMessage");
            if (ucMessage != null)
            {
                ucMessage.ShowErrorMessage(message);
            }
        }

        public void ShowErrorMessage(string message, params string[] parameters)
        {
            IMessage ucMessage = (IMessage)Page.FindControl("ucMessage");
            if (ucMessage != null)
            {
                ucMessage.ShowErrorMessage(message, parameters);
            }
        }

        public void ShowErrorMessage(BusinessErrorException ex)
        {
            IMessage ucMessage = (IMessage)Page.FindControl("ucMessage");
            if (ucMessage != null)
            {
                if (ex.MessageParams != null && ex.MessageParams.Length > 0)
                {
                    ucMessage.ShowErrorMessage(ex.Message, ex.MessageParams);
                }
                else
                {
                    ucMessage.ShowErrorMessage(ex.Message);
                }
            }
        }

        public void CleanMessage()
        {
            IMessage ucMessage = (IMessage)Page.FindControl("ucMessage");
            if (ucMessage != null)
            {
                ucMessage.CleanMessage();
            }
        }


        protected User CurrentUser
        {
            get
            {
                User user = (new SessionHelper(this.Page)).CurrentUser;
                if (user == null || user.UserLanguage == null || user.UserLanguage == string.Empty)
                {
                    user.UserLanguage = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_LANGUAGE).Value;
                }
                return user;
            }
        }

        public T GetService<T>(string serviceName) { return ServiceLocator.GetService<T>(serviceName); }
        public IEntityPreferenceMgrE TheEntityPreferenceMgr { get { return GetService<IEntityPreferenceMgrE>("EntityPreferenceMgr.service"); } }
        protected ILanguageMgrE TheLanguageMgr { get { return GetService<ILanguageMgrE>("LanguageMgr.service"); } }
    }
}