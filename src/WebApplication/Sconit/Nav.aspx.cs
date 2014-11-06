using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using System.Globalization;
using System.Threading;
using com.Sconit.Entity;

public partial class Nav : com.Sconit.Web.PageBase
{
    private bool result = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Permission = BusinessConstants.PERMISSION_NOTNEED_CHECK_PERMISSION;

        this.TreeView1.TreeNodeDataBound += new TreeNodeEventHandler(TreeView1_TreeNodeDataBound);
        this.id_hideUser.Value = this.CurrentUser.Code;

        if (!Page.IsPostBack)
        {
            result = true;
        }
    }

    protected void TreeView1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    {
        TreeNode treeNode = e.Node;
        SiteMapNode siteMapNode = (SiteMapNode)treeNode.DataItem;

        if (siteMapNode.Url == string.Empty)
        {
            e.Node.SelectAction = TreeNodeSelectAction.Expand;
        }

        #region 生成Icon
        //string ImageIco = "~/Images/Nav/" + siteMapNode.Description + ".png";
        string ImageIco = siteMapNode.Description;
        if (File.Exists(Server.MapPath(ImageIco)))
        {
            treeNode.ImageUrl = ImageIco;
        }
        else
        {
            treeNode.ImageUrl = "~/Images/Nav/Default.png";
        }
        #endregion

        #region 判断权限
        if (this.CurrentUser.Code.ToLower() == "su" || HasPermission(siteMapNode))
        {
            string text = TheLanguageMgr.TranslateContent(treeNode.Text, this.CurrentUser.UserLanguage);
            //string toolTip = TheLanguageMgr.TranslateContent(siteMapNode.ResourceKey, this.language);
            treeNode.ToolTip = siteMapNode.ResourceKey;
            treeNode.Text = "&nbsp;" + text;
        }
        else
        {
            try
            {
                treeNode.Parent.ChildNodes.Remove(treeNode);
            }
            catch (Exception)
            {
                this.TreeView1.Nodes.Remove(treeNode);
            }
        }
        #endregion

        if (result)
        {
            try
            {
                this.TreeView1.Nodes[0].Expand();
            }
            catch (Exception)
            {
                //throw;
            }
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (this.Session["Current_User"] == null)
        {
            this.Response.Redirect("~/Logoff.aspx");
        }
    }

    private bool HasPermission(SiteMapNode siteMapNode)
    {
        //string url = siteMapNode.Url.Trim();
        //url = url.Contains("Main.aspx") ? ("~/" + url.Remove(0, siteMapNode.Url.IndexOf("Main.aspx"))) : string.Empty;

        if (this.CurrentUser.HasPermission(siteMapNode.Key))
        {
            return true;
        }
        else
        {
            if (siteMapNode.ChildNodes != null && siteMapNode.ChildNodes.Count > 0)
            {
                foreach (SiteMapNode childSiteMapNode in siteMapNode.ChildNodes)
                {
                    if (HasPermission(childSiteMapNode))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}