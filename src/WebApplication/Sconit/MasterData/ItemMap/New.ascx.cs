using com.Sconit.Entity.MasterData;
using com.Sconit.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterData_ItemMap_New : NewModuleBase
{

    public event EventHandler BackEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            ItemMap itemMap = new ItemMap();
            itemMap.Item = tdItem.Text;
            itemMap.MapItem = tdMapItem.Text;
            itemMap.StartDate = DateTime.Parse(tbStartDate.Text);
            itemMap.EndDate = DateTime.Parse(tbEndDate.Text);
            itemMap.CreateDate = DateTime.Now;
            itemMap.CreateUser = CurrentUser.Code;
            itemMap.LastModifyDate = DateTime.Now;
            itemMap.LastModifyUser = CurrentUser.Code;
            TheItemMapMgr.CreateItemMap(itemMap);
            ShowSuccessMessage("MasterData.ItemMap.CreateItemMap.Successfully", itemMap.Item);
        }
        catch
        {
            ShowErrorMessage("MasterData.ItemMap.CreateItemMap.Fail");
        }
    }

    protected void checkItemExists(object source, ServerValidateEventArgs args)
    {
        string code = tdItem.Text;

        if (TheItemMgr.LoadItem(code) == null || TheItemMgr.LoadItem(code).Equals(""))
        {
            ShowErrorMessage("MasterData.ItemMap.CodeExist11", code);
            args.IsValid = false;
        }
    }

    protected void checkItemExists1(object source, ServerValidateEventArgs args)
    {
        string code = tdMapItem.Text;

        if (TheItemMgr.LoadItem(code) == null || TheItemMgr.LoadItem(code).Equals(""))
        {
            ShowErrorMessage("MasterData.ItemMap.CodeExist22", code);
            args.IsValid = false;
        }
    }
}