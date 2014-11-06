using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections;
using com.Sconit.Service.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.View;
using com.Sconit.Entity.View;

public partial class _Default : com.Sconit.Web.PageBase
{
    protected string url = "Main.aspx";
    protected int leftdownHeight = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Permission = BusinessConstants.PERMISSION_NOTNEED_CHECK_PERMISSION;
        if (this.Session["Current_User"] == null)
        {
            this.Response.Redirect("~/Logoff.aspx");
        }
        else
        {
            this.Title = TheEntityPreferenceMgr.LoadEntityPreference("CompanyName").Value;

            if (!Page.IsPostBack)
            {
                string ThemePage = string.Empty;

                HttpCookie cookieThemePage = new HttpCookie("ThemePage");
                if (this.CurrentUser.UserThemePage == null || this.CurrentUser.UserThemePage.Trim() == string.Empty)
                {
                    cookieThemePage.Value = TheCodeMasterMgr.GetDefaultCodeMaster(BusinessConstants.CODE_MASTER_USER_PREFERENCE_VALUE_THEMEPAGE).Value;
                    Response.Cookies.Add(cookieThemePage);

                    this.CurrentUser.UserThemePage = cookieThemePage.Value;

                    UserPreference usrpf = new UserPreference();
                    usrpf.User = this.CurrentUser;
                    usrpf.Code = BusinessConstants.CODE_MASTER_USER_PREFERENCE_VALUE_THEMEPAGE;
                    usrpf.Value = cookieThemePage.Value;
                    TheUserPreferenceMgr.CreateUserPreference(usrpf);
                }
                else
                {
                    UserPreference userPreferenceThemePage = TheUserPreferenceMgr.LoadUserPreference(this.CurrentUser.Code, "ThemePage");
                    if (userPreferenceThemePage != null && userPreferenceThemePage.Value == BusinessConstants.CODE_MASTER_USER_PREFERENCE_VALUE_THEMEPAGE_RANDOM)
                    {
                        ThemePage = TheCodeMasterMgr.GetRandomTheme(BusinessConstants.CODE_MASTER_USER_PREFERENCE_VALUE_THEMEPAGE);
                    }
                    else
                    {
                        ThemePage = userPreferenceThemePage.Value;
                    }
                    cookieThemePage.Value = ThemePage;
                    Response.Cookies.Add(cookieThemePage);

                }

                #region 随机框架主题
                HttpCookie cookieThemeFrame = new HttpCookie("ThemeFrame");
                if (this.CurrentUser.UserThemeFrame == null || this.CurrentUser.UserThemeFrame.Trim() == string.Empty)
                {
                    cookieThemeFrame.Value = string.Empty;
                    Response.Cookies.Add(cookieThemeFrame);

                    this.CurrentUser.UserThemeFrame = TheCodeMasterMgr.GetDefaultCodeMaster(BusinessConstants.CODE_MASTER_USER_PREFERENCE_VALUE_THEMEFRAME).Value;

                    UserPreference usrpf = new UserPreference();
                    usrpf.User = this.CurrentUser;
                    usrpf.Code = BusinessConstants.CODE_MASTER_USER_PREFERENCE_VALUE_THEMEFRAME;
                    usrpf.Value = this.CurrentUser.UserThemeFrame;
                    TheUserPreferenceMgr.CreateUserPreference(usrpf);
                }
                else
                {
                    string themeFrame = TheUserPreferenceMgr.LoadUserPreference(this.CurrentUser.Code, "ThemeFrame").Value;
                    switch (themeFrame)
                    {
                        case "Picture":
                            cookieThemeFrame.Value = string.Empty;
                            Response.Cookies.Add(cookieThemeFrame);
                            break;
                        case "Random":
                            cookieThemeFrame.Value = TheCodeMasterMgr.GetRandomTheme("ThemeFrame");
                            Response.Cookies.Add(cookieThemeFrame);
                            break;
                        default:
                            cookieThemeFrame.Value = themeFrame;
                            Response.Cookies.Add(cookieThemeFrame);
                            break;
                    }
                }
                #endregion
            }

            //确定MainFrame的页面为退出前的页面
            if (Request.Params.Get("rightFrameUrl") == null)
            {
                Favorites favorites = TheFavoritesMgr.LoadLastFavorites(this.CurrentUser.Code, "History");

                if (favorites != null)
                {
                    MenuView menuView = TheMenuViewMgr.GetMenuView(favorites.PageName);
                    if (menuView != null)
                    {
                        url = menuView.PageUrl.Substring(2);
                    }
                    else
                    {
                        url = "Main.aspx?mid=Security.UserPreference";
                    }
                }
                else
                {
                    url = "Main.aspx?mid=Security.UserPreference";
                }
            }
            else
            {
                url = Request.Params.Get("rightFrameUrl");
            }

        }
    }

}