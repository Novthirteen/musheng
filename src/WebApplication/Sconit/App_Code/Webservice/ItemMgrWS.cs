using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Web;
using com.Sconit.Entity;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using com.Sconit.Entity.Quote;
/// <summary>
/// Summary description for ItemManagerWS
/// </summary>
[WebService(Namespace = "http://com.Sconit.Webservice/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ItemMgrWS : BaseWS
{
    public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
    public IPriceListMgrE priceListMgrE { get; set; }

    public ItemMgrWS()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    [WebMethod]
    public Item GetParaByCode(string itemcode)
    {
        Item item = this.TheItemMgr.LoadItem(itemcode);
        item.Desc2 = null;
        IList<EntityPreference> entityPreferences = TheEntityPreferenceMgr.GetAllEntityPreference();
        string priceCode = string.Empty;
        if (entityPreferences != null && entityPreferences.Count > 0)
        {
            foreach (EntityPreference ep in entityPreferences)
            {
                if (ep.Code == "QuotePrice")
                {
                    priceCode = ep.Value;
                    break;
                }
            }
        }
        PriceList priceList = ThePriceListMgr.LoadPriceList(priceCode, true);
        foreach (PriceListDetail pd in priceList.PriceListDetails)
        {
            if(pd.Item.Code == itemcode)
            {
                if ((pd.StartDate == null ? DateTime.MinValue : pd.StartDate) <= DateTime.Now && (pd.EndDate == null ? DateTime.MaxValue : pd.EndDate) >= DateTime.Now)
                {
                    item.Desc2 = pd.UnitPrice.ToString();
                }
            }
        }

        return item;
    }

    [WebMethod]
    public Item GenerateItemProxy(string itemCode)
    {
        Item item = this.TheItemMgr.LoadItem(itemCode);
        Item simpleItem = new Item();
        if (item != null)
        {
            simpleItem.Code = item.Code;
            simpleItem.Desc1 = item.Desc1;
            simpleItem.Desc2 = item.Desc2;
            simpleItem.Uom = item.Uom;
        }
        return simpleItem;
    }

    [WebMethod]
    public string GetItemByLikeCode(string likeCode)
    {
        string hql = "select i from Item as i where 1=1 ";
        IList<object> parameters = new List<object>();
        if (!string.IsNullOrEmpty(likeCode))
        {
            string[] codeArr = likeCode.Split(',');
            string paraHql = string.Empty;
            foreach (var code in codeArr)
            {
                if (string.IsNullOrEmpty(paraHql))
                {
                    paraHql += "  i.Code like ? ";
                }
                else
                {
                    paraHql += " or i.Code like ? ";
                
                }
                parameters.Add(code + "%");
            }
            hql +=" and (" +paraHql +")";
        }
        IList<Item> itemList = TheGenericMgr.FindAllWithCustomQuery<Item>(hql , parameters.ToArray());
        if(itemList!=null && itemList.Count>0)
        {
            List<Item> serializerItems = new List<Item>();
            for (int i = 0; i < 50; i++)
            {
                if (i == itemList.Count)
                {
                    break;
                }
                var cItem = itemList[i];
                serializerItems.Add(new Item { Code = cItem.Code, Desc1 = cItem.Desc1,Desc2=cItem.Desc2 });
            }
            return JsonSerializer<Item>(serializerItems);
        }
        return string.Empty;
    }

    [WebMethod]
    public string GetLocationByLikeCode(string likeCode)
    {
        string hql = "select i from Location as i where 1=1 ";
        IList<object> parameters = new List<object>();
        if (!string.IsNullOrEmpty(likeCode))
        {
            string[] codeArr = likeCode.Split(',');
            string paraHql = string.Empty;
            foreach (var code in codeArr)
            {
                if (string.IsNullOrEmpty(paraHql))
                {
                    paraHql += "  i.Code like ? ";
                }
                else
                {
                    paraHql += " or i.Code like ? ";

                }
                parameters.Add(code + "%");
            }
            hql += " and (" + paraHql + ")";
        }
        IList<Location> lcoationList = TheGenericMgr.FindAllWithCustomQuery<Location>(hql, parameters.ToArray());
        if (lcoationList != null && lcoationList.Count > 0)
        {
            List<Location> serializerItems = new List<Location>();
            for (int i = 0; i < 50; i++)
            {
                if (i == lcoationList.Count)
                {
                    break;
                }
                var clocation = lcoationList[i];
                serializerItems.Add(new Location { Code = clocation.Code, Name = clocation.Name });
            }
            return JsonSerializer<Location>(serializerItems);
        }
        return string.Empty;
    }


}

