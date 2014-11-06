using System;
using System.Web.UI.HtmlControls;
using com.Sconit.Entity;

public partial class Lefttop : com.Sconit.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Permission = BusinessConstants.PERMISSION_NOTNEED_CHECK_PERMISSION;
        this.SUser.Text = this.CurrentUser.Code;
    }
}
