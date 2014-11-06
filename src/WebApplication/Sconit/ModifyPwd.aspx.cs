using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity.MasterData;
using com.Sconit.Web;

public partial class ModifyPwd : com.Sconit.Web.PageBase
{
    protected string UserCode
    {
        get
        {
            return (string)ViewState["UserCode"];
        }
        set
        {
            ViewState["UserCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["Current_User"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        else
        {
            this.UserCode = ((com.Sconit.Entity.MasterData.UserBase)(this.Session["Current_User"])).Code;
        }
    }

    protected void btnUpdatePassword_Click(object sender, EventArgs e)
    {
        TextBox tbPassword = (TextBox)(this.divPassword.FindControl("tbPassword"));
        User user = TheUserMgr.LoadUser(UserCode);
        user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(tbPassword.Text.Trim(), "MD5");
        user.LastModifyDate = DateTime.Now;
        user.LastModifyUser = this.CurrentUser;
        user.ModifyPwd = true;
        TheUserMgr.UpdateUser(user);

        this.Session["Current_User"] = TheUserMgr.LoadUser(UserCode, true, true);
        Response.Redirect("~/Default.aspx");
    }
}
