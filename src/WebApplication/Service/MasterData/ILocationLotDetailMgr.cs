using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ILocationLotDetailMgr : ILocationLotDetailBaseMgr
    {
        #region Customized Methods


        IList<LocationLotDetail> GetHuLocationLotDetail(string huId);

        IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string huId);

        IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string huId, bool includeZero);

        IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string huId, string itemCode, string lotNo, bool includeZero);

        IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero);

        IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode);

        IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode, string[] orderBy);

        IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode, string[] orderBy, bool inBin);

        IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode, string[] orderBy, bool inBin, bool createSBAlias);

        IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode, string[] orderBy, bool inBin, bool createSBAlias, DateTime? createDate, int? rowCount);

        //IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode, string[] orderBy, bool inBin, bool createSBAlias, DateTime? createDate, int? rowCount, string sort1From, string sort1To, string color1From, string color1To, string sort2From, string sort2To, string color2From, string color2To);

        IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment);

        IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment, bool includeZero);

        IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment, bool includeZero, string minusOrPlusInv);

        IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment, bool includeZero, string minusOrPlusInv, bool? isInBin);
        
        IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment, bool includeZero, string minusOrPlusInv, bool isIncludeHu, string bin);
       
        //IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment, bool includeZero, string minusOrPlusInv, bool? isInBin, string recNo);

        IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment, bool includeZero, string minusOrPlusInv, bool? isInBin, bool isIncludeHu);

        IList<LocationLotDetail> GetLocationLotDetail(string locationCode, List<string> itemCodes, bool isConsignment, bool includeZero, string minusOrPlusInv, bool? isInBin, bool isIncludeHu);

        IList<LocationLotDetail> GetLocationLotDetail(int plannedBillId);

        IList<LocationLotDetail> GetLocationLotDetail(PlannedBill plannedBill);

        IList<LocationLotDetail> GetLocationLotDetail(IList<int> IdList);

        LocationLotDetail CheckLoadHuLocationLotDetail(string huId, Location location);

        LocationLotDetail CheckLoadHuLocationLotDetail(string huId, User user, string locationCode);

        LocationLotDetail CheckLoadHuLocationLotDetail(string huId, string userCode, string locationCode);

        LocationLotDetail CheckLoadHuLocationLotDetail(string huId, string userCode);

        LocationLotDetail CheckLoadHuLocationLotDetail(string huId);

        LocationLotDetail LoadHuLocationLotDetail(string huId);

        bool CheckGoodsReceiveFIFO(string locationCode, string itemCode, DateTime manufactureDate, DateTime? baseManufatureDate);

        bool CheckGoodsIssueFIFO(string locationCode, string itemCode, DateTime baseManufatureDate, IList<string> huIdList);

        bool CheckHuLocationExist(string huId, string location);

        bool CheckHuLocationExist(string huId);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ILocationLotDetailMgrE : com.Sconit.Service.MasterData.ILocationLotDetailMgr
    {
        
    }
}

#endregion
