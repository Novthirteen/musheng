using System;
using System.Web.UI.HtmlControls;
using com.Sconit.CasClient.Utils;
using com.Sconit.Service.MasterData;
using com.Sconit.Utility;
using System.Threading;
using System.Globalization;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using System.Drawing;
using System.Web;
using System.Collections.Generic;
using com.Sconit.Service.Ext.MasterData;
using System.Text;

public partial class Top : com.Sconit.Web.PageBase
{

    protected ILocationDetailMgrE locationDetailMgr { get { return GetService<ILocationDetailMgrE>("LocationDetailMgr.service"); } }
    protected override void InitializeCulture()
    {
        if (this.CurrentUser.UserLanguage == null || this.CurrentUser.UserLanguage.Trim() == string.Empty)
        {
            string userLanguage = TheCodeMasterMgr.GetDefaultCodeMaster(BusinessConstants.CODE_MASTER_LANGUAGE).Value;
            this.CurrentUser.UserLanguage = userLanguage;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(userLanguage);
        }
        else
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(this.CurrentUser.UserLanguage);
        }

        //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        AsyncLoadLocationDetail();
        this.Permission = BusinessConstants.PERMISSION_NOTNEED_CHECK_PERMISSION;

        string companyCode = TheEntityPreferenceMgr.LoadEntityPreference("CompanyCode").Value;
        string imagePath = "Images/OEM/" + companyCode;
        this.LeftImage.ImageUrl = imagePath + "/Logo_Lit.png";

        //HttpCookie cookie = Request.Cookies["OEM"];
        //if (cookie != null)
        //{
        //    string imagePath = (cookie.Value == null || cookie.Value.Trim() == string.Empty) ? "Images/" : cookie.Value.Trim();
        //    this.LeftImage.ImageUrl = imagePath + "Logo_Lit.png";
        //}
        this.Info.ForeColor = Color.Black;

        if (this.Session["Current_User"] == null)
        {
            this.Response.Redirect("~/Logoff.aspx");
        }
        this.Info.Text = TheEntityPreferenceMgr.LoadEntityPreference("CompanyName").Value;

        if (Session[AbstractCasModule.CONST_CAS_PRINCIPAL] != null)
        {
            this.logoffHL.NavigateUrl = "https://sso.hoternet.cn:8443/cas/logout?service=http://demo.sconit.com/Logoff.aspx";
            this.logoffHL.Target = "_parent";
        }
        else
        {
            this.logoffHL.NavigateUrl = "~/Logoff.aspx";
            this.logoffHL.Target = "_parent";
        }
        if (this.Info.Text.ToLower().Contains("test"))
        {
            this.imgTest.Visible = true;
        }

        //[{ desc: '愛彼思塑膠-原材料仓库', value: 'ABSS' },{ desc: '上海阿仨希-外购件二楼仓库', value: 'ABXG' }]
        //IList<Item> items = TheItemMgr.GetCacheAllItem();
        //StringBuilder data = new StringBuilder("[");
        //int count = items.Count;
        //for (int i = 0; i < count; i++)
        //{
        //    Item item = items[i];
        //    data.Append(TextBoxHelper.GenSingleData(item.Description, item.Code));
        //    if (i < (count - 1))
        //    {
        //        data.Append(",");
        //    }
        //}
        //data.Append("]");
        //this.data.Value = Server.HtmlEncode(data.ToString());
        this.data.Value = TheItemMgr.GetCacheAllItemString();
    }

    private delegate LocationDetail Async(string location, string item);
    private void AsyncLoadLocationDetail()
    {
        Async async = new Async(locationDetailMgr.GetCatchLocationDetail);
        async.BeginInvoke(string.Empty, string.Empty, null, null);
    }
}
