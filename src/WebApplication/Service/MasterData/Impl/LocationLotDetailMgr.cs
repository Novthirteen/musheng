using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Criteria;
using NHibernate.Expression;
using System;
using System.Linq;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class LocationLotDetailMgr : LocationLotDetailBaseMgr, ILocationLotDetailMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetHuLocationLotDetail(string huId)
        {
            return GetHuLocationLotDetail(null, null, null, huId, null, null, false, null, null, new string[] { "Id;Desc" });
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string huId)
        {
            return GetHuLocationLotDetail(locationCode, null, null, huId, null, null, false, null, null, new string[] { "Id;Desc" });
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string huId, bool includeZero)
        {
            return GetHuLocationLotDetail(locationCode, null, null, huId, null, null, includeZero, null, null, new string[] { "Id;Desc" });
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string huId, string itemCode, string lotNo, bool includeZero)
        {
            return GetHuLocationLotDetail(locationCode, null, null, huId, itemCode, lotNo, includeZero, null, null, new string[] { "Id;Desc" });
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero)
        {
            return GetHuLocationLotDetail(locationCode, areaCode, binCode, huId, itemCode, lotNo, includeZero, null, null, new string[] { "Id;Desc" });
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode)
        {
            return GetHuLocationLotDetail(locationCode, areaCode, binCode, huId, itemCode, lotNo, includeZero, unitCount, uomCode, new string[] { "Id;Desc" });
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode, string[] orderBy)
        {
            return GetHuLocationLotDetail(locationCode, areaCode, binCode, huId, itemCode, lotNo, includeZero, unitCount, uomCode, orderBy, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode, string[] orderBy, bool inBin)
        {
            return GetHuLocationLotDetail(locationCode, areaCode, binCode, huId, itemCode, lotNo, includeZero, unitCount, uomCode, orderBy, inBin, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode, string[] orderBy, bool inBin, bool createSBAlias)
        {
            return GetHuLocationLotDetail(locationCode, areaCode, binCode, huId, itemCode, lotNo, includeZero, unitCount, uomCode, orderBy, inBin, createSBAlias, null, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode, string[] orderBy, bool inBin, bool createSBAlias, DateTime? createDate, int? rowCount)
        {
            return GetHuLocationLotDetail(locationCode, areaCode, binCode, huId, itemCode, lotNo, includeZero, unitCount, uomCode, orderBy, inBin, createSBAlias, createDate, rowCount, null, null, null, null, null, null, null, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetHuLocationLotDetail(string locationCode, string areaCode, string binCode, string huId, string itemCode, string lotNo, bool includeZero, decimal? unitCount, string uomCode, string[] orderBy, bool inBin, bool createSBAlias, DateTime? createDate, int? rowCount, string sort1From, string sort1To, string color1From, string color1To, string sort2From, string sort2To, string color2From, string color2To)
        {
            DetachedCriteria criteria = DetachedCriteria.For<LocationLotDetail>();

            if (huId != null || unitCount.HasValue || uomCode != null 
                || (sort1From != null && sort1From != string.Empty && sort1From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                || (color1From != null && color1From != string.Empty && color1From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                || (sort2From != null && sort2From != string.Empty && sort2From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                || (color2From != null && color2From != string.Empty && color2From != BusinessConstants.SORT_COLOR_IGNORE_LABEL))
            {
                criteria.CreateAlias("Hu", "hu");
            }

            if (createSBAlias)
            {
                criteria.CreateAlias("StorageBin", "sb");
            }

            if (locationCode != null && locationCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Location.Code", locationCode.Trim()));
            }

            if (areaCode != null && areaCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("StorageArea.Code", areaCode.Trim()));
            }

            if (binCode != null && binCode.Trim() != string.Empty)
            {
                if (!createSBAlias)
                {
                    criteria.CreateAlias("StorageBin", "sb");
                }
                criteria.Add(Expression.Eq("sb.Code", binCode.Trim()));
            }
            else if (inBin)
            {
                criteria.Add(Expression.IsNotNull("StorageBin"));
            }

            if (huId != null && huId.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Hu.HuId", huId.Trim()));
            }
            else
            {
                criteria.Add(Expression.IsNotNull("Hu"));
            }

            if (itemCode != null && itemCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Item.Code", itemCode.Trim()));
            }

            if (lotNo != null && lotNo.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("LotNo", lotNo.Trim()));
            }

            if (!includeZero)
            {
                //非零库存
                criteria.Add(Expression.Not(Expression.Eq("Qty", new decimal(0))));
            }

            if (unitCount.HasValue)
            {
                //criteria.Add(Expression.Eq("hu.UnitCount", unitCount.Value));
                criteria.Add(Expression.Eq("Qty", unitCount.Value));
            }

            if (uomCode != null)
            {
                criteria.CreateAlias("hu.Uom", "huUom");
                criteria.Add(Expression.Eq("huUom.Code", uomCode));
            }

            if (createDate.HasValue)
            {
                criteria.Add(Expression.Ge("CreateDate", createDate.Value));
                criteria.Add(Expression.Lt("CreateDate", createDate.Value.AddDays(1)));
            }

            if (sort1From != null && sort1From != string.Empty && sort1From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
            {
                criteria.Add(Expression.Between("hu.SortLevel1", sort1From, sort1To));
            }

            if (color1From != null && color1From != string.Empty && color1From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
            {
                criteria.Add(Expression.Between("hu.ColorLevel1", color1From, color1To));
            }

            if (sort2From != null && sort2From != string.Empty && sort2From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
            {
                criteria.Add(Expression.Between("hu.SortLevel2", sort1From, sort2To));
            }

            if (color2From != null && color2From != string.Empty && color2From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
            {
                criteria.Add(Expression.Between("hu.ColorLevel2", color2From, color2To));
            }

            if (orderBy != null && orderBy.Length > 0)
            {
                foreach (string ob in orderBy)
                {
                    string[] s = ob.Split(';');
                    if (s.Length > 1)
                    {
                        if (s[1].ToLower() == "desc")
                        {
                            criteria.AddOrder(Order.Desc(s[0]));
                        }
                        else
                        {
                            criteria.AddOrder(Order.Asc(s[0]));
                        }
                    }
                    else
                    {
                        criteria.AddOrder(Order.Asc(ob));
                    }
                }
            }
            if (rowCount.HasValue)
            {
                return this.criteriaMgrE.FindAll<LocationLotDetail>(criteria, 0, rowCount.Value);
            }
            else
            {
                return this.criteriaMgrE.FindAll<LocationLotDetail>(criteria);
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment)
        {
            return GetLocationLotDetail(locationCode, itemCode, isConsignment, false, null, false, true);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment, bool includeZero)
        {
            return GetLocationLotDetail(locationCode, itemCode, isConsignment, includeZero, null, false, true);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment, bool includeZero, string minusOrPlusInv)
        {
            return GetLocationLotDetail(locationCode, itemCode, isConsignment, includeZero, minusOrPlusInv, false, true);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment, bool includeZero, string minusOrPlusInv, bool? isInBin)
        {
            return GetLocationLotDetail(locationCode, itemCode, isConsignment, includeZero, minusOrPlusInv, isInBin, true);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment, bool includeZero, string minusOrPlusInv, bool? isInBin, bool isIncludeHu)
        {
            List<string> itemCodes = new List<string>() { itemCode };
            return GetLocationLotDetail(locationCode, itemCodes, isConsignment, includeZero, minusOrPlusInv, isInBin, isIncludeHu);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetLocationLotDetail(string locationCode, List<string> itemCodes, bool isConsignment, bool includeZero, string minusOrPlusInv, bool? isInBin, bool isIncludeHu)
        {
            DetachedCriteria criteria = DetachedCriteria.For<LocationLotDetail>();

            if (!isIncludeHu)
            {
                criteria.Add(Expression.IsNull("Hu"));
            }

            if (isInBin.HasValue)
            {
                if (!isInBin.Value)
                {
                    criteria.Add(Expression.IsNull("StorageBin"));
                }
                else
                {
                    criteria.Add(Expression.IsNotNull("StorageBin"));
                }
            }

            if (minusOrPlusInv != null && minusOrPlusInv.Trim() != string.Empty)
            {
                if (minusOrPlusInv == BusinessConstants.MINUS_INVENTORY)
                {
                    //负数的库存
                    criteria.Add(Expression.Lt("Qty", new decimal(0)));
                }
                else if (minusOrPlusInv == BusinessConstants.PLUS_INVENTORY)
                {
                    //正数的库存
                    criteria.Add(Expression.Gt("Qty", new decimal(0)));
                }
                else
                {
                    throw new TechnicalException("invalied minusOrPlusInv parameter:" + minusOrPlusInv);
                }
            }
            else if (!includeZero)
            {
                //非零库存
                criteria.Add(Expression.Not(Expression.Eq("Qty", new decimal(0))));
            }

            criteria.Add(Expression.Eq("IsConsignment", isConsignment));

            //if (recNo != null && recNo.Trim() != string.Empty)
            //{
            //    criteria.Add(Expression.Eq("PlannedBill.ReceiptNo", recNo.Trim()));
            //}

            if (locationCode != null && locationCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Location.Code", locationCode.Trim()));
            }

            if (itemCodes != null && itemCodes.Count() > 0)
            {
                criteria.Add(Expression.In("Item.Code", itemCodes));
            }

            criteria.AddOrder(Order.Asc("Id"));

            return this.criteriaMgrE.FindAll<LocationLotDetail>(criteria);
        }


        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetLocationLotDetail(string locationCode, string itemCode, bool isConsignment, bool includeZero, string minusOrPlusInv, bool isIncludeHu, string bin)
        {
            DetachedCriteria criteria = DetachedCriteria.For<LocationLotDetail>();

            if (!isIncludeHu)
            {
                criteria.Add(Expression.IsNull("Hu"));
            }

            if (bin != null && bin != string.Empty)
            {
                criteria.Add(Expression.Eq("StorageBin.Code", bin));
            }

            if (minusOrPlusInv != null && minusOrPlusInv.Trim() != string.Empty)
            {
                if (minusOrPlusInv == BusinessConstants.MINUS_INVENTORY)
                {
                    //负数的库存
                    criteria.Add(Expression.Lt("Qty", new decimal(0)));
                }
                else if (minusOrPlusInv == BusinessConstants.PLUS_INVENTORY)
                {
                    //正数的库存
                    criteria.Add(Expression.Gt("Qty", new decimal(0)));
                }
                else
                {
                    throw new TechnicalException("invalied minusOrPlusInv parameter:" + minusOrPlusInv);
                }
            }
            else if (!includeZero)
            {
                //非零库存
                criteria.Add(Expression.Not(Expression.Eq("Qty", new decimal(0))));
            }

            criteria.Add(Expression.Eq("IsConsignment", isConsignment));

            if (locationCode != null && locationCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Location.Code", locationCode.Trim()));
            }

            if (itemCode != null && itemCode != string.Empty)
            {
                criteria.Add(Expression.Eq("Item.Code", itemCode));
            }

            criteria.AddOrder(Order.Asc("Id"));

            return this.criteriaMgrE.FindAll<LocationLotDetail>(criteria);
        }


        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetLocationLotDetail(PlannedBill plannedBill)
        {
            return GetLocationLotDetail(plannedBill.Id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetLocationLotDetail(int plannedBillId)
        {
            DetachedCriteria criteria = DetachedCriteria.For<LocationLotDetail>();

            criteria.Add(Expression.Eq("PlannedBill", plannedBillId));
            criteria.Add(Expression.Eq("IsConsignment", true));
            criteria.Add(Expression.Not(Expression.Eq("Qty", new decimal(0))));

            IList<LocationLotDetail> locationLotDetailList = this.criteriaMgrE.FindAll<LocationLotDetail>(criteria);

            if (locationLotDetailList != null && locationLotDetailList.Count > 0)
            {
                return locationLotDetailList;
            }

            return null;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<LocationLotDetail> GetLocationLotDetail(IList<int> IdList)
        {
            DetachedCriteria criteria = DetachedCriteria.For<LocationLotDetail>();

            IList<LocationLotDetail> locationLotDetailList = new List<LocationLotDetail>();
            if (IdList != null && IdList.Count > 0)
            {
                if (IdList.Count == 1)
                {
                    criteria.Add(Expression.Eq("Id", IdList[0]));
                }
                else
                {
                    criteria.Add(Expression.InG<int>("Id", IdList));
                }
                locationLotDetailList = this.criteriaMgrE.FindAll<LocationLotDetail>(criteria);
            }

            return locationLotDetailList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public LocationLotDetail CheckLoadHuLocationLotDetail(string huId, Location location)
        {
            return CheckLoadHuLocationLotDetail(huId, string.Empty, location.Code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public LocationLotDetail LoadHuLocationLotDetail(string huId)
        {
            IList<LocationLotDetail> locationLotDetailList = GetHuLocationLotDetail(huId);
            if (locationLotDetailList.Count > 1)
            {
                throw new BusinessErrorException("Common.Business.Error.FindMultiHu", string.Empty, huId);
            }
            else if (locationLotDetailList.Count == 1)
            {
                return locationLotDetailList[0];
            }
            return null;
        }

        [Transaction(TransactionMode.Unspecified)]
        public LocationLotDetail CheckLoadHuLocationLotDetail(string huId, string userCode, string locationCode)
        {
            User user = null;
            if (userCode != string.Empty)
            {
                user = userMgrE.CheckAndLoadUser(userCode);
            }
            return CheckLoadHuLocationLotDetail(huId, user, locationCode);
        }

        [Transaction(TransactionMode.Unspecified)]
        public LocationLotDetail CheckLoadHuLocationLotDetail(string huId)
        {
            return CheckLoadHuLocationLotDetail(huId, string.Empty, string.Empty);
        }

        [Transaction(TransactionMode.Unspecified)]
        public LocationLotDetail CheckLoadHuLocationLotDetail(string huId, string userCode)
        {
            return CheckLoadHuLocationLotDetail(huId, userCode, string.Empty);
        }

        [Transaction(TransactionMode.Unspecified)]
        public LocationLotDetail CheckLoadHuLocationLotDetail(string huId, User user, string locationCode)
        {
            IList<LocationLotDetail> locationLotDetailList = this.GetHuLocationLotDetail(locationCode, huId);
            if (locationLotDetailList == null || locationLotDetailList.Count == 0)
            {
                throw new BusinessErrorException("Common.Business.Error.HuNoInventory", locationCode, huId);
            }
            else if (locationLotDetailList.Count > 1)
            {
                throw new BusinessErrorException("Common.Business.Error.FindMultiHu", locationCode, huId);
            }
            else
            {
                if (user != null)
                {
                    string regionCode = locationLotDetailList[0].Location.Region.Code;
                    if (!user.HasPermission(regionCode))
                    {
                        throw new BusinessErrorException("Common.Business.Error.NoPermission");
                    }
                }
                return locationLotDetailList[0];
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public bool CheckGoodsReceiveFIFO(string locationCode, string itemCode, DateTime manufactureDate, DateTime? baseManufatureDate)
        {
            DetachedCriteria criteria = DetachedCriteria.For<LocationLotDetail>();
            criteria.SetProjection(Projections.Count("Id"));

            criteria.CreateAlias("Hu", "hu");
            criteria.CreateAlias("Location", "loc");
            criteria.CreateAlias("Item", "item");
            //criteria.CreateAlias("hu.ManufactureParty", "party");

            criteria.Add(Expression.IsNotNull("Hu"));
            criteria.Add(Expression.Gt("Qty", new Decimal(0)));
            criteria.Add(Expression.Eq("item.Code", itemCode));
            criteria.Add(Expression.Eq("loc.Code", locationCode));
            criteria.Add(Expression.Gt("hu.ManufactureDate", manufactureDate));
            //criteria.Add(Expression.Eq("party.Code", manufactureParty));

            if (baseManufatureDate.HasValue)
            {
                criteria.Add(Expression.Lt("hu.ManufactureDate", baseManufatureDate.Value));
            }

            IList<int> list = this.criteriaMgrE.FindAll<int>(criteria);
            if (list[0] > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public bool CheckGoodsIssueFIFO(string locationCode, string itemCode, DateTime baseManufatureDate, IList<string> huIdList)
        {
            DetachedCriteria criteria = DetachedCriteria.For<LocationLotDetail>();
            criteria.SetProjection(Projections.Count("Id"));

            criteria.CreateAlias("Hu", "hu");
            criteria.CreateAlias("Location", "loc");
            criteria.CreateAlias("Item", "item");

            criteria.Add(Expression.IsNotNull("Hu"));
            criteria.Add(Expression.Gt("Qty", new Decimal(0)));
            criteria.Add(Expression.Eq("item.Code", itemCode));
            criteria.Add(Expression.Eq("loc.Code", locationCode));
            criteria.Add(Expression.Lt("hu.ManufactureDate", baseManufatureDate));
            criteria.Add(Expression.Not(Expression.In("hu.HuId", huIdList.ToArray<string>())));

            IList<int> list = this.criteriaMgrE.FindAll<int>(criteria);
            if (list[0] > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public bool CheckHuLocationExist(string huId, string location)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationLotDetail));

            criteria.Add(Expression.Eq("Hu.HuId", huId));
            criteria.Add(Expression.Gt("Qty", 0M));
            if (location != null && location.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Location.Code", location));
            }

            criteria.SetProjection(Projections.Count("Id"));

            IList result = criteriaMgrE.FindAll(criteria);
            if ((int)result[0] > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public bool CheckHuLocationExist(string huId)
        {
            return CheckHuLocationExist(huId, null);
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class LocationLotDetailMgrE : com.Sconit.Service.MasterData.Impl.LocationLotDetailMgr, ILocationLotDetailMgrE
    {

    }
}
#endregion