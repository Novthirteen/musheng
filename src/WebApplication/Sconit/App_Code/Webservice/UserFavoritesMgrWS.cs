using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.Sconit.Service.Ext.MasterData;
using System.Web.Services;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using System.IO;
using com.Sconit.Entity;
using System.Collections;
using com.Sconit.Entity.View;


/// <summary>
/// Summary description for UserFavoriteMgrWS
/// </summary>
[WebService(Namespace = "http://com.Sconit.Webservice/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class UserFavoritesMgrWS : BaseWS
{
    public UserFavoritesMgrWS()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    [WebMethod]
    public void AddUserFavorite(string userCode, string type, string pageName, bool hasPermission)
    {
        try
        {
            if (TheFavoritesMgr.CheckFavoritesUniqueExist(userCode, type, pageName))
            {
                TheFavoritesMgr.DeleteFavorites(userCode, type, pageName);
            }
            if (hasPermission)
            {
                Favorites fav = new Favorites();
                fav.User = TheUserMgr.LoadUser(userCode);
                fav.Type = type;
                fav.PageName = pageName;
                fav.PageUrl = DateTime.Now.ToString();
                TheFavoritesMgr.CreateFavorites(fav);
            }
        }
        catch (Exception) { }
    }

    [WebMethod]
    public string ListUserFavorites(string userCode, string type)
    {
        string listf = string.Empty;
        IList<Favorites> ifavorites = TheFavoritesMgr.GetFavorites(userCode, type);
        int count = int.Parse(TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_HISTORYNO).Value);
        count = count < ifavorites.Count ? count : ifavorites.Count;
        User user = TheUserMgr.LoadUser(userCode, true, false);
        string language = user.UserLanguage;
        IList<MenuView> menuViewList = TheMenuViewMgr.GetAllMenuView();
        for (int i = 0; i < count; i++)
        {
            string imageUrl = string.Empty;
            string pageUrl = string.Empty;
            string title = string.Empty;
            Favorites fav = ifavorites[i];
            string pageName = fav.PageName;
            IList<MenuView> menuViews = (from m in menuViewList where m.Code == pageName select m).ToList();
            if (menuViews.Count == 1)
            {
                imageUrl = menuViews.Single().ImageUrl;
                pageUrl = menuViews.Single().PageUrl;
                title = menuViews.Single().Code;
            }
            if (!File.Exists(Server.MapPath(imageUrl)))
            {
                imageUrl = "~/Images/Nav/Default.png";
            }
            imageUrl = "<img src = '" + imageUrl.Substring(2) + "' />";
            listf += "<li class='div-favorite'><span onclick ='DeleteFavorite(" + fav.Id + ")'>XX</span>" + imageUrl;
            listf += "<a href = '" + pageUrl.Substring(2) + "' target = 'right'>" + TheLanguageMgr.TranslateContent(title, language) + "</a></li>";
        }
        return listf;
    }

    [WebMethod]
    public void DeleteUserFavorite(int id)
    {
        TheFavoritesMgr.DeleteFavorites(id);
    }
}
