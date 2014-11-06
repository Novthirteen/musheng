using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Configuration;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using System.Web.Security;
using com.Sconit.Utility;
using com.Sconit.Entity;

public partial class Index : com.Sconit.Web.PageBase
{
    private IList<EntityPreference> entityPerferences;
    private static log4net.ILog log = log4net.LogManager.GetLogger("Log.Login");

    protected void Login_Click(object sender, EventArgs e)
    {
        string ipFilter = (from en in entityPerferences where en.Code == "isEnableIPFilter" select en.Value).FirstOrDefault();
        bool isEnableIPFilter = ipFilter.ToUpper() == "TRUE" ? true : false;

        string userCode = this.txtUsername.Value.Trim().ToLower();
        string ipAdd = Request.UserHostAddress.ToString();
        string logInfo = " - IP:" + ipAdd + " User:" + userCode;
        if (isEnableIPFilter && !IPHelper.IsInnerIP(ipAdd))
        {
            log.Error("IPFilter:Not in permit ip range" + logInfo);
            return;
        }

        string password = this.txtPassword.Value.Trim();
        password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");

        if (userCode.Equals(string.Empty))
        {
            errorLabel.Text = "Please enter your Account!";
            return;
        }

        User user = TheUserMgr.LoadUser(userCode, false, false);

        if (user == null)
        {
            errorLabel.Text = "Identification failure. Try again!";
            log.Error("User code not exsit" + logInfo);
        }
        else
        {
            if (password == user.Password && user.IsActive)
            {
                this.Session["Current_User"] = TheUserMgr.LoadUser(userCode, true, true);
                log.Info("Login successfully." + logInfo);
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                errorLabel.Text = "Identification failure. Try again!";
                log.Error("Identification failure." + logInfo);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Permission = BusinessConstants.PERMISSION_NOTNEED_CHECK_PERMISSION;
        //Response.Redirect("~/Login.aspx");

        entityPerferences = TheEntityPreferenceMgr.GetAllEntityPreference();
        this.Literal1.Text = Resources.Language.IndexWelcome;
        this.Title = (from en in entityPerferences where en.Code == "CompanyName" select en.Value).FirstOrDefault();
        this.Documentation.NavigateUrl = (from en in entityPerferences where en.Code == "DocumentationURL" select en.Value).FirstOrDefault();
        this.Wiki.NavigateUrl = (from en in entityPerferences where en.Code == "WikiURL" select en.Value).FirstOrDefault();
        this.Forum.NavigateUrl = (from en in entityPerferences where en.Code == "ForumURL" select en.Value).FirstOrDefault();
        string companyCode = (from en in entityPerferences where en.Code == "CompanyCode" select en.Value).FirstOrDefault();
        companyCode = (companyCode == null || companyCode.Trim() == string.Empty) ? string.Empty : (companyCode + "/");
        string imagePath = "Images/OEM/" + companyCode;

        if (this.Title.ToLower().Contains("test"))
        {
            this.imgTest.Visible = true;
        }
        this.imgLogo.ImageUrl = imagePath + "Logo.png";
        string formBg = imagePath + "bg_form.png";
        this.login_form.Attributes.Add("style", "background: url(" + formBg + ") no-repeat;");

        if (!Page.IsPostBack)
        {
            string LoginImagePath = imagePath + "Banner.jpg";
            HttpCookie randomPicDate = new HttpCookie("RandomPicDate");
            randomPicDate.Value = LoginImagePath;
            Response.Cookies.Add(randomPicDate);
            //OEM
            string OEM = imagePath;
            HttpCookie httpCookie = new HttpCookie("OEM");
            httpCookie.Value = OEM;
            Response.Cookies.Add(httpCookie);
            //LoginPage
            HttpCookie loginPageCookie = new HttpCookie("LoginPage");
            loginPageCookie.Value = "~/Index.aspx";
            Response.Cookies.Add(loginPageCookie);
        }
    }
}