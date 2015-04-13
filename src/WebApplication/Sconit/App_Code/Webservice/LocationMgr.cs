using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using com.Sconit.Utility;

/// <summary>
/// Summary description for LocationMgr
/// </summary>
//[WebService(Namespace = "http://com.Sconit.Webservice")]
//[SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.SoapAction)]
//[System.Web.Services.WebServiceBindingAttribute(Name = "LocationMgr", Namespace = "http://com.Sconit.Webservice")]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

[WebService(Namespace = "http://com.Sconit.Webservice/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class LocationMgr : BaseWS
{
    public LocationMgr()
    {
        
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public LocationDetail[] FindLocationDetail(string[] locationCode, string[] itemCode, DateTime effectiveDate, string userCode)
    {   
        try
        {
            IList<LocationDetail> locationDetailList = TheLocationDetailMgr.FindLocationDetail(IListHelper.ConvertToList(locationCode), IListHelper.ConvertToList(itemCode), effectiveDate, userCode);

            if (locationDetailList != null && locationDetailList.Count > 0)
            {
                return locationDetailList.ToArray();
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            throw new SoapException(ex.Message, SoapException.ServerFaultCode, Context.Request.Url.AbsoluteUri);
        }
    }


    [WebMethod]
    public LocationTransaction[] FindLocationTransaction(string[] transType,
           string[] loc, DateTime effdateStart, DateTime effDateEnd, DateTime createDateStart, DateTime createDateEnd,
           string partyFrom, string partyTo, string[] itemCode,
           string[] orderNo, string[] recNo, string createUser, string[] ipNo, string userCode)
    {
        try
        {
            IList<LocationTransaction> LocationTransactionList = TheLocationMgr.GetLocationTransaction(transType,
                    loc,  effdateStart,  effDateEnd,  createDateStart,  createDateEnd,
                    partyFrom,  partyTo,  itemCode, orderNo,  recNo,  createUser,  ipNo, userCode);

            if (LocationTransactionList != null && LocationTransactionList.Count > 0)
            {
                return LocationTransactionList.ToArray();
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            throw new SoapException(ex.Message, SoapException.ServerFaultCode, Context.Request.Url.AbsoluteUri);
        }
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
            hql += " and (" + paraHql + ")";
        }
        IList<Item> itemList = TheGenericMgr.FindAllWithCustomQuery<Item>(hql, parameters.ToArray());
        if (itemList != null && itemList.Count > 0)
        {
            List<Item> serializerItems = new List<Item>();
            for (int i = 0; i < 50; i++)
            {
                if (i == itemList.Count)
                {
                    break;
                }
                var cItem = itemList[i];
                serializerItems.Add(new Item { Code = cItem.Code, Desc1 = cItem.Desc1, Desc2 = cItem.Desc2 });
            }
            return JsonSerializer<Item>(serializerItems);
        }
        return string.Empty;
    }
}

