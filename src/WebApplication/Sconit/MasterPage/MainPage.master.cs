using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using com.Sconit.Web;

public partial class MasterPage_MainPage : com.Sconit.Web.MasterPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.id_User.Value = this.CurrentUser.Code;

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "method",
            @"<script language='javascript' type='text/javascript'>window.onerror = function() { return true; };
              try{$('#floatdiv').css({'top':($(document).scrollTop())+40});}catch(err){}</script>", false);
    }

    protected override void OnPreRender(EventArgs e)
    {
        SiteMapPath smp = (SiteMapPath)SiteMapPath1;
        if (smp.Provider.CurrentNode == null)
        {
            //this.divsmp.Visible = false;
            Response.Redirect("~/NoPermission.aspx");
        }
        else
        {
            string title = smp.Provider.CurrentNode.Title;
            string key = smp.Provider.CurrentNode.Key;
            this.Head1.Title = TheLanguageMgr.TranslateContent(title, this.CurrentUser.UserLanguage);
            this.id_Key.Value = key;

            #region 权限认证
            if (this.CurrentUser.Code.ToLower() != "su")
            {
                if (!this.CurrentUser.HasPermission(title))
                {
                    Response.Redirect("~/NoPermission.aspx");
                }
            }
            #endregion

            string pageImage = smp.Provider.CurrentNode.Description;
            //记录访问历史
            if (!IsPostBack)
            {
                if (this.Request.UrlReferrer != this.Request.Url && this.Request.Url.Query != null)  //
                {
                    UserFavoritesMgrWS userFavoritesMgrWS = new UserFavoritesMgrWS();
                    userFavoritesMgrWS.AddUserFavorite(this.CurrentUser.Code, "History", key, this.CurrentUser.HasPermission(title));
                }
            }
        }
        base.OnPreRender(e);
    }

    protected void SiteMapPath1_ItemCreated(object sender, SiteMapNodeItemEventArgs e)
    {
        if (e.Item.SiteMapNode != null)
        {
            e.Item.SetRenderMethodDelegate(new RenderMethod(NewRenderMethod));
        }
    }

    private void NewRenderMethod(HtmlTextWriter writer, Control ctl)
    {
        for (int i = 0; i < ctl.Controls.Count; i++)
        {
            if (ctl.Controls[i] is HyperLink)
            {
                ((HyperLink)(ctl.Controls[i])).ToolTip = ((HyperLink)(ctl.Controls[i])).Text;
                ((HyperLink)(ctl.Controls[i])).Text = TheLanguageMgr.TranslateContent(((HyperLink)(ctl.Controls[i])).Text, this.CurrentUser.UserLanguage);
            }
        }
        for (int i = 0; i < ctl.Controls.Count; i++)
        {
            ctl.Controls[i].RenderControl(writer);
        }
    }
}
