﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Expression;
using com.Sconit.Entity.View;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;

namespace com.Sconit.Utility
{
    /// <summary>
    /// 封装常用查询条件
    /// </summary>
    public class CriteriaHelper
    {
        #region Criteria: Like
        #region Item
        public static void SetItemCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetItemCriteria(criteria, propertyName, criteriaParam.Item);
        }
        public static void SetItemCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam, MatchMode matchMode)
        {
            SetItemCriteria(criteria, propertyName, criteriaParam.Item, matchMode);
        }
        public static void SetItemCriteria(DetachedCriteria criteria, string propertyName, string item)
        {
            SetItemCriteria(criteria, propertyName, item, MatchMode.Exact);//default
        }
        public static void SetItemCriteria(DetachedCriteria criteria, string propertyName, string item, MatchMode matchMode)
        {
            if (item != null && item.Trim() != string.Empty)
            {
                if (matchMode == MatchMode.Exact)
                {
                    criteria.Add(Expression.Eq(propertyName, item));
                }
                else
                {
                    criteria.Add(Expression.Like(propertyName, item, matchMode));
                }
            }
        }
        #endregion

        #region ItemDesc
        public static void SetItemDescCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetItemDescCriteria(criteria, propertyName, criteriaParam.ItemDesc);
        }
        public static void SetItemDescCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam, MatchMode matchMode)
        {
            SetItemDescCriteria(criteria, propertyName, criteriaParam.ItemDesc, matchMode);
        }
        public static void SetItemDescCriteria(DetachedCriteria criteria, string propertyName, string itemDesc)
        {
            SetItemDescCriteria(criteria, propertyName, itemDesc, MatchMode.Anywhere);//default
        }
        public static void SetItemDescCriteria(DetachedCriteria criteria, string propertyName, string itemDesc, MatchMode matchMode)
        {
            if (itemDesc != null && itemDesc.Trim() != string.Empty)
            {
                criteria.Add(Expression.Like(propertyName, itemDesc, matchMode));
            }
        }
        #endregion

        #region LotNo
        public static void SetLotNoCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetItemCriteria(criteria, propertyName, criteriaParam.LotNo);
        }
        public static void SetLotNoCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam, MatchMode matchMode)
        {
            SetItemCriteria(criteria, propertyName, criteriaParam.LotNo, matchMode);
        }
        public static void SetLotNoCriteria(DetachedCriteria criteria, string propertyName, string lotNo)
        {
            SetItemCriteria(criteria, propertyName, lotNo, MatchMode.Anywhere);//default
        }
        public static void SetLotNoCriteria(DetachedCriteria criteria, string propertyName, string lotNo, MatchMode matchMode)
        {
            if (lotNo != null && lotNo.Trim() != string.Empty)
            {
                criteria.Add(Expression.Like(propertyName, lotNo, matchMode));
            }
        }
        #endregion


        #region TransactionType
        public static void SetTransactionTypeCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetTransactionTypeCriteria(criteria, propertyName, criteriaParam.TransactionType);
        }
        public static void SetTransactionTypeCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam, MatchMode matchMode)
        {
            SetTransactionTypeCriteria(criteria, propertyName, criteriaParam.TransactionType, matchMode);
        }
        public static void SetTransactionTypeCriteria(DetachedCriteria criteria, string propertyName, string transactionType)
        {
            SetTransactionTypeCriteria(criteria, propertyName, transactionType, MatchMode.Start);//default
        }
        public static void SetTransactionTypeCriteria(DetachedCriteria criteria, string propertyName, string transactionType, MatchMode matchMode)
        {
            if (transactionType != null && transactionType.Trim() != string.Empty)
            {
                criteria.Add(Expression.Like(propertyName, transactionType, matchMode));
            }
        }
        #endregion

        #region OrderNo
        public static void SetOrderNoCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam, MatchMode matchMode)
        {
            SetOrderNoCriteria(criteria, propertyName, criteriaParam.OrderNo, matchMode);
        }
        public static void SetOrderNoCriteria(DetachedCriteria criteria, string propertyName, string orderNo, MatchMode matchMode)
        {
            if (orderNo != null && orderNo.Trim() != string.Empty)
            {
                criteria.Add(Expression.Like(propertyName, orderNo, matchMode));
            }
        }
        #endregion

        #endregion

        #region Criteria: In
        public static void SetInCriteria<T>(DetachedCriteria criteria, string propertyName, List<T> list)
        {
            if (list != null && list.Count > 0)
            {
                if (list.Count == 1)
                {
                    criteria.Add(Expression.Eq(propertyName, list[0]));
                }
                else
                {
                    criteria.Add(Expression.InG<T>(propertyName, list));
                }
            }
        }

        public static void SetPartyCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetPartyCriteria(criteria, propertyName, criteriaParam.Party);
        }
        public static void SetPartyCriteria(DetachedCriteria criteria, string propertyName, string[] party)
        {
            if (party != null && party.Length > 0)
            {
                if (party.Length == 1)
                {
                    criteria.Add(Expression.Eq(propertyName, party[0]));
                }
                else
                {
                    criteria.Add(Expression.In(propertyName, party));
                }
            }
        }

        public static void SetFlowCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetFlowCriteria(criteria, propertyName, criteriaParam.Flow);
        }
        public static void SetFlowCriteria(DetachedCriteria criteria, string propertyName, string[] flow)
        {
            if (flow != null && flow.Length > 0)
            {
                if (flow.Length == 1)
                {
                    criteria.Add(Expression.Eq(propertyName, flow[0]));
                }
                else
                {
                    criteria.Add(Expression.In(propertyName, flow));
                }
            }
        }

        public static void SetLocationCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetLocationCriteria(criteria, propertyName, criteriaParam.Location);
        }
        public static void SetLocationCriteria(DetachedCriteria criteria, string propertyName, string[] location)
        {
            if (location != null && location.Length > 0)
            {
                if (location.Length == 1)
                {
                    criteria.Add(Expression.Eq(propertyName, location[0]));
                }
                else
                {
                    criteria.Add(Expression.In(propertyName, location));
                }
            }
        }

        public static void SetBinCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetBinCriteria(criteria, propertyName, criteriaParam.BinCode);
        }
        public static void SetBinCriteria(DetachedCriteria criteria, string propertyName, string BinCode)
        {
            if (!string.IsNullOrEmpty(BinCode))
            {
                criteria.Add(Expression.Eq(propertyName, BinCode));
            }
        }
        #endregion

        public static void SetStartDateCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetStartDateCriteria(criteria, propertyName, criteriaParam.StartDate);
        }
        public static void SetStartDateCriteria(DetachedCriteria criteria, string propertyName, DateTime? startDate)
        {
            if (startDate.HasValue)
            {
                criteria.Add(Expression.Ge(propertyName, startDate.Value));
            }
        }

        public static void SetEndDateCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetEndDateCriteria(criteria, propertyName, criteriaParam.EndDate);
        }
        public static void SetEndDateCriteria(DetachedCriteria criteria, string propertyName, DateTime? endDate)
        {
            if (endDate.HasValue)
            {
                criteria.Add(Expression.Lt(propertyName, endDate.Value.AddDays(1)));
            }
        }

        public static void SetShiftCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetShiftCriteria(criteria, propertyName, criteriaParam.Shift);
        }
        public static void SetShiftCriteria(DetachedCriteria criteria, string propertyName, string shift)
        {
            if (shift != null && shift.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq(propertyName, shift));
            }
        }

        public static void SetStorageBinCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetStorageBinCriteria(criteria, propertyName, criteriaParam.BinCode);
        }
        public static void SetStorageBinCriteria(DetachedCriteria criteria, string propertyName, string binCode)
        {
            if (binCode != null && binCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq(propertyName, binCode));
            }
        }

        public static void SetItemCategoryCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetItemCategoryCriteria(criteria, propertyName, criteriaParam.ItemCategory);
        }
        public static void SetItemCategoryCriteria(DetachedCriteria criteria, string propertyName, string itemCategory)
        {
            if (itemCategory != null && itemCategory.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq(propertyName, itemCategory));
            }
        }

        public static void SetCostGroupCriteria(DetachedCriteria criteria, string propertyName, CriteriaParam criteriaParam)
        {
            SetCostGroupCriteria(criteria, propertyName, criteriaParam.CostGroup);
        }
        public static void SetCostGroupCriteria(DetachedCriteria criteria, string propertyName, string costGroup)
        {
            if (costGroup != null && costGroup.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq(propertyName, costGroup));
            }
        }

        public static object[] CollectMasterParam(IDictionary<string, string> dic, List<string> statusList, List<string> typeList, bool newItem)
        {
            string orderNo = (dic == null || !dic.ContainsKey("OrderNo")) ? null : dic["OrderNo"];
            string flow = (dic == null || !dic.ContainsKey("Flow")) ? null : dic["Flow"];
            string partyFrom = (dic == null || !dic.ContainsKey("PartyFrom")) ? null : dic["PartyFrom"];
            string partyTo = (dic == null || !dic.ContainsKey("PartyTo")) ? null : dic["PartyTo"];
            string moduleType = (dic == null || !dic.ContainsKey("ModuleType")) ? null : dic["ModuleType"];
            string locationFrom = (dic == null || !dic.ContainsKey("LocationFrom")) ? null : dic["LocationFrom"];
            string locationTo = (dic == null || !dic.ContainsKey("LocationTo")) ? null : dic["LocationTo"];
            string moduleSubType = (dic == null || !dic.ContainsKey("ModuleSubType")) ? null : dic["ModuleSubType"];
            string priority = (dic == null || !dic.ContainsKey("Priority")) ? null : dic["Priority"];
            string createUser = (dic == null || !dic.ContainsKey("CreateUser")) ? null : dic["CreateUser"];
            //modify by ljz start
            //string startDate = (dic == null || !dic.ContainsKey("StartDate")) ? null : dic["StartDate"];
            //string endDate = (dic == null || !dic.ContainsKey("EndDate")) ? null : dic["EndDate"];
            string ArriveStartDate = (dic == null || !dic.ContainsKey("ArriveStartDate")) ? null : dic["ArriveStartDate"];
            string ArriveEndDate = (dic == null || !dic.ContainsKey("ArriveEndDate")) ? null : dic["ArriveEndDate"];
            //modify by ljz end
            string currentUser = (dic == null || !dic.ContainsKey("CurrentUser")) ? null : dic["CurrentUser"];

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderHead));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(OrderHead))
                .SetProjection(Projections.Count("OrderNo"));
            IDictionary<string, string> alias = new Dictionary<string, string>();
            selectCriteria.CreateAlias("PartyFrom", "pf");
            selectCriteria.CreateAlias("PartyTo", "pt");
            selectCountCriteria.CreateAlias("PartyFrom", "pf");
            selectCountCriteria.CreateAlias("PartyTo", "pt");
            alias.Add("PartyFrom", "pf");
            alias.Add("PartyTo", "pt");

            #region 设置订单Type查询条件
            selectCriteria.Add(Expression.In("Type", typeList));
            selectCountCriteria.Add(Expression.In("Type", typeList));
            #endregion

            #region 设置订单SubType查询条件
            if (moduleSubType != null && moduleSubType != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("SubType", moduleSubType));
                selectCountCriteria.Add(Expression.Eq("SubType", moduleSubType));
            }
            #endregion

            selectCriteria.Add(Expression.Eq("IsNewItem", newItem));
            selectCountCriteria.Add(Expression.Eq("IsNewItem", newItem));

            if (orderNo != null && orderNo != string.Empty)
            {
                selectCriteria.Add(Expression.Like("OrderNo", orderNo, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("OrderNo", orderNo, MatchMode.Anywhere));
            }

            if (priority != null && priority != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("Priority", priority));
                selectCountCriteria.Add(Expression.Eq("Priority", priority));
            }

            #region partyFrom
            if (partyFrom != null && partyFrom != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("pf.Code", partyFrom));
                selectCountCriteria.Add(Expression.Eq("pf.Code", partyFrom));
            }
            else if (moduleType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
            {
                #region partyFrom
                SecurityHelper.SetPartyFromSearchCriteria(
                    selectCriteria, selectCountCriteria, partyFrom, moduleType, currentUser);
                #endregion
            }
            #endregion

            #region partyTo
            if (partyTo != null && partyTo != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("pt.Code", partyTo));
                selectCountCriteria.Add(Expression.Eq("pt.Code", partyTo));
            }
            else if (moduleType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                #region partyTo
                SecurityHelper.SetPartyToSearchCriteria(
                    selectCriteria, selectCountCriteria, partyTo, moduleType, currentUser);
                #endregion
            }
            #endregion

            if (locationFrom != null && locationFrom != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("LocationFrom.Code", locationFrom));
                selectCountCriteria.Add(Expression.Eq("LocationFrom.Code", locationFrom));
            }

            if (locationTo != null && locationTo != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("LocationTo.Code", locationTo));
                selectCountCriteria.Add(Expression.Eq("LocationTo.Code", locationTo));
            }

            if (flow != null && flow != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("Flow", flow));
                selectCountCriteria.Add(Expression.Eq("Flow", flow));
            }

            if (createUser != null && createUser != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("CreateUser.Code", createUser));
                selectCountCriteria.Add(Expression.Eq("CreateUser.Code", createUser));
            }

            //modify by ljz start
            #region date
            //if (startDate != null && startDate != string.Empty)
            //{
            //    selectCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(startDate)));
            //    selectCountCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(startDate)));
            //}
            //if (endDate != null && endDate != string.Empty)
            //{
            //    selectCriteria.Add(Expression.Lt("CreateDate", DateTime.Parse(endDate).AddDays(1)));
            //    selectCountCriteria.Add(Expression.Lt("CreateDate", DateTime.Parse(endDate).AddDays(1)));
            //}
            #endregion
            #region arrivedate
            if (ArriveStartDate != null && ArriveStartDate != string.Empty)
            {
                selectCriteria.Add(Expression.Ge("WindowTime", DateTime.Parse(ArriveStartDate)));
                selectCountCriteria.Add(Expression.Ge("WindowTime", DateTime.Parse(ArriveStartDate)));
            }
            if (ArriveEndDate != null && ArriveEndDate != string.Empty)
            {
                selectCriteria.Add(Expression.Lt("WindowTime", DateTime.Parse(ArriveEndDate).AddDays(1)));
                selectCountCriteria.Add(Expression.Lt("WindowTime", DateTime.Parse(ArriveEndDate).AddDays(1)));
            }
            #endregion
            //modify by ljz end

            #region status
            if (statusList != null && statusList.Count > 0)
            {
                selectCriteria.Add(Expression.In("Status", statusList));
                selectCountCriteria.Add(Expression.In("Status", statusList));
            }
            #endregion
            return new object[] { selectCriteria, selectCountCriteria, alias, true };
        }

        public static object[] CollectDetailParam(IDictionary<string, string> dic, List<string> statusList, List<string> typeList, bool newItem)
        {
            string orderNo = (dic == null || !dic.ContainsKey("OrderNo")) ? null : dic["OrderNo"];
            string flow = (dic == null || !dic.ContainsKey("Flow")) ? null : dic["Flow"];
            string partyFrom = (dic == null || !dic.ContainsKey("PartyFrom")) ? null : dic["PartyFrom"];
            string partyTo = (dic == null || !dic.ContainsKey("PartyTo")) ? null : dic["PartyTo"];
            string moduleType = (dic == null || !dic.ContainsKey("ModuleType")) ? null : dic["ModuleType"];
            string locationFrom = (dic == null || !dic.ContainsKey("LocationFrom")) ? null : dic["LocationFrom"];
            string locationTo = (dic == null || !dic.ContainsKey("LocationTo")) ? null : dic["LocationTo"];
            string moduleSubType = (dic == null || !dic.ContainsKey("ModuleSubType")) ? null : dic["ModuleSubType"];
            string priority = (dic == null || !dic.ContainsKey("Priority")) ? null : dic["Priority"];
            string createUser = (dic == null || !dic.ContainsKey("CreateUser")) ? null : dic["CreateUser"];
            //modify by ljz start
            //string startDate = (dic == null || !dic.ContainsKey("StartDate")) ? null : dic["StartDate"];
            //string endDate = (dic == null || !dic.ContainsKey("EndDate")) ? null : dic["EndDate"];
            string ArriveStartDate = (dic == null || !dic.ContainsKey("ArriveStartDate")) ? null : dic["ArriveStartDate"];
            string ArriveEndDate = (dic == null || !dic.ContainsKey("ArriveEndDate")) ? null : dic["ArriveEndDate"];
            //modify by ljz end 
            string currentUser = (dic == null || !dic.ContainsKey("CurrentUser")) ? null : dic["CurrentUser"];
            string item = (dic == null || !dic.ContainsKey("Item")) ? null : dic["Item"];

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderDetail));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(OrderDetail))
                .SetProjection(Projections.Count("Id"));
            IDictionary<string, string> alias = new Dictionary<string, string>();
            selectCriteria.CreateAlias("OrderHead", "od");
            selectCriteria.CreateAlias("od.PartyFrom", "pf");
            selectCriteria.CreateAlias("od.PartyTo", "pt");
            selectCriteria.CreateAlias("Item", "i");
            selectCountCriteria.CreateAlias("OrderHead", "od");
            selectCountCriteria.CreateAlias("od.PartyFrom", "pf");
            selectCountCriteria.CreateAlias("od.PartyTo", "pt");
            selectCountCriteria.CreateAlias("Item", "i");
            alias.Add("PartyFrom", "pf");
            alias.Add("PartyTo", "pt");
            alias.Add("OrderHead", "od");

            #region 设置订单Type查询条件
            selectCriteria.Add(Expression.In("od.Type", typeList));
            selectCountCriteria.Add(Expression.In("od.Type", typeList));
            #endregion

            #region 设置订单SubType查询条件
            if (moduleSubType != null && moduleSubType != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("od.SubType", moduleSubType));
                selectCountCriteria.Add(Expression.Eq("od.SubType", moduleSubType));
            }
            #endregion

            selectCriteria.Add(Expression.Eq("od.IsNewItem", newItem));
            selectCountCriteria.Add(Expression.Eq("od.IsNewItem", newItem));

            if (orderNo != null && orderNo != string.Empty)
            {
                selectCriteria.Add(Expression.Like("od.OrderNo", orderNo, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("od.OrderNo", orderNo, MatchMode.Anywhere));
            }

            if (priority != null && priority != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("od.Priority", priority));
                selectCountCriteria.Add(Expression.Eq("od.Priority", priority));
            }

            #region partyFrom
            if (partyFrom != null && partyFrom != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("pf.Code", partyFrom));
                selectCountCriteria.Add(Expression.Eq("pf.Code", partyFrom));
            }
            else if (moduleType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
            {
                #region partyFrom
                SecurityHelper.SetPartyFromSearchCriteria(
                    selectCriteria, selectCountCriteria, partyFrom, moduleType, currentUser);
                #endregion
            }
            #endregion

            #region partyTo
            if (partyTo != null && partyTo != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("pt.Code", partyTo));
                selectCountCriteria.Add(Expression.Eq("pt.Code", partyTo));
            }
            else if (moduleType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                #region partyTo
                SecurityHelper.SetPartyToSearchCriteria(
                    selectCriteria, selectCountCriteria, partyTo, moduleType, currentUser);
                #endregion
            }
            #endregion

            if (locationFrom != null && locationFrom != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("od.LocationFrom.Code", locationFrom));
                selectCountCriteria.Add(Expression.Eq("od.LocationFrom.Code", locationFrom));
            }

            if (locationTo != null && locationTo != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("od.LocationTo.Code", locationTo));
                selectCountCriteria.Add(Expression.Eq("od.LocationTo.Code", locationTo));
            }

            if (flow != null && flow != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("od.Flow", flow));
                selectCountCriteria.Add(Expression.Eq("od.Flow", flow));
            }

            if (item != null && item != string.Empty)
            {
                selectCriteria.Add(
                    Expression.Like("i.Code", item, MatchMode.Anywhere) ||
                    Expression.Like("i.Desc1", item, MatchMode.Anywhere) ||
                    Expression.Like("i.Desc2", item, MatchMode.Anywhere)
                    );
                selectCountCriteria.Add(
                    Expression.Like("i.Code", item, MatchMode.Anywhere) ||
                    Expression.Like("i.Desc1", item, MatchMode.Anywhere) ||
                    Expression.Like("i.Desc2", item, MatchMode.Anywhere)
                    );
            }

            if (createUser != null && createUser != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("od.CreateUser.Code", createUser));
                selectCountCriteria.Add(Expression.Eq("od.CreateUser.Code", createUser));
            }

            //modify by ljz start
            #region date
            //if (startDate != null && startDate != string.Empty)
            //{
            //    selectCriteria.Add(Expression.Ge("od.CreateDate", DateTime.Parse(startDate)));
            //    selectCountCriteria.Add(Expression.Ge("od.CreateDate", DateTime.Parse(startDate)));
            //}
            //if (endDate != null && endDate != string.Empty)
            //{
            //    selectCriteria.Add(Expression.Lt("od.CreateDate", DateTime.Parse(endDate).AddDays(1)));
            //    selectCountCriteria.Add(Expression.Lt("od.CreateDate", DateTime.Parse(endDate).AddDays(1)));
            //}
            #endregion
            #region arrivedate
            if (ArriveStartDate != null && ArriveStartDate != string.Empty)
            {
                selectCriteria.Add(Expression.Ge("od.WindowTime", DateTime.Parse(ArriveStartDate)));
                selectCountCriteria.Add(Expression.Ge("od.WindowTime", DateTime.Parse(ArriveStartDate)));
            }
            if (ArriveEndDate != null && ArriveEndDate != string.Empty)
            {
                selectCriteria.Add(Expression.Lt("od.WindowTime", DateTime.Parse(ArriveEndDate).AddDays(1)));
                selectCountCriteria.Add(Expression.Lt("od.WindowTime", DateTime.Parse(ArriveEndDate).AddDays(1)));
            }
            #endregion
            //modify by ljz end

            #region status
            if (statusList != null && statusList.Count > 0)
            {
                selectCriteria.Add(Expression.In("od.Status", statusList));
                selectCountCriteria.Add(Expression.In("od.Status", statusList));
            }
            #endregion
            return new object[] { selectCriteria, selectCountCriteria, alias, false };
        }
    }
}
