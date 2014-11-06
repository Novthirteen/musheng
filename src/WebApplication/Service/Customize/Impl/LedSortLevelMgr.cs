using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Customize;
using com.Sconit.Entity.Customize;
using NHibernate.Expression;
using com.Sconit.Entity.Exception;
using com.Sconit.Service.Ext.Criteria;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize.Impl
{
    [Transactional]
    public class LedSortLevelMgr : LedSortLevelBaseMgr, ILedSortLevelMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public bool CheckLedFeedSortLevel(string itemCode, string brand, string startLevel, string endLevel, string targetLevel)
        {
            LedSortLevel startLedSortLevel = GetLedSortLevel(itemCode, brand, startLevel);
            if (startLedSortLevel == null)
            {
                throw new BusinessErrorException("MasterData.LedSort.Error.SortLevelNotExist", brand, startLevel);
            }

            LedSortLevel endLedSortLevel = GetLedSortLevel(itemCode, brand, endLevel);
            if (endLedSortLevel == null)
            {
                throw new BusinessErrorException("MasterData.LedSort.Error.SortLevelNotExist", brand, endLevel);
            }

            LedSortLevel currentLedSortLevel = GetLedSortLevel(itemCode, brand, targetLevel);
            if (currentLedSortLevel == null)
            {
                throw new BusinessErrorException("MasterData.LedSort.Error.SortLevelNotExist", brand, targetLevel);
            }

            if (currentLedSortLevel.Sequence < startLedSortLevel.Sequence
                || currentLedSortLevel.Sequence > endLedSortLevel.Sequence)
            {
                throw new BusinessErrorException("MasterData.LedSort.Error.SortLevelNotInRange", targetLevel, startLevel, endLevel);
            }

            return true;
        }

        [Transaction(TransactionMode.Unspecified)]
        public bool IsNearByLedSortLevel(string itemCode, string brand, string orgLevel, string targetLevel)
        {
            LedSortLevel orgLedSortLevel = GetLedSortLevel(itemCode, brand, orgLevel);
            if (orgLedSortLevel == null)
            {
                throw new BusinessErrorException("MasterData.LedSort.Error.SortLevelNotExist", brand, orgLevel);
            }

            LedSortLevel currentLedSortLevel = GetLedSortLevel(itemCode, brand, targetLevel);
            if (currentLedSortLevel == null)
            {
                throw new BusinessErrorException("MasterData.LedSort.Error.SortLevelNotExist", brand, targetLevel);
            }

            if (orgLedSortLevel.Sequence == currentLedSortLevel.Sequence)
            {
                return false;
            }

            DetachedCriteria criteria = DetachedCriteria.For<LedSortLevel>();

            criteria.Add(Expression.Eq("Brand.Code", brand));
            if (orgLedSortLevel.Sequence > currentLedSortLevel.Sequence)
            {
                criteria.Add(Expression.Between("Sequence", currentLedSortLevel.Sequence, orgLedSortLevel.Sequence));
            }
            else
            {
                criteria.Add(Expression.Between("Sequence", orgLedSortLevel.Sequence, currentLedSortLevel.Sequence));
            }

            criteria.SetProjection(Projections.ProjectionList().Add(Projections.RowCount()));

            if (this.criteriaMgr.FindAll<int>(criteria)[0] > 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private LedSortLevel GetLedSortLevel(string itemCode, string brand, string sortLevel)
        {
            DetachedCriteria criteria = DetachedCriteria.For<LedSortLevel>();

            LedSortLevel ledSortLevel = null;

            if (itemCode != null && itemCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Item.Code", itemCode));
                criteria.Add(Expression.Eq("Brand.Code", brand));
                criteria.Add(Expression.Eq("Value", sortLevel));

                IList<LedSortLevel> ledSortLevelList = this.criteriaMgr.FindAll<LedSortLevel>(criteria);

                if (ledSortLevelList != null && ledSortLevelList.Count > 0)
                {
                    ledSortLevel = ledSortLevelList[0];
                }
            }

            if (ledSortLevel == null)
            {
                criteria = DetachedCriteria.For<LedSortLevel>();

                criteria.Add(Expression.IsNull("Item"));
                criteria.Add(Expression.Eq("Brand.Code", brand));
                criteria.Add(Expression.Eq("Value", sortLevel));

                IList<LedSortLevel> ledSortLevelList = this.criteriaMgr.FindAll<LedSortLevel>(criteria);

                if (ledSortLevelList != null && ledSortLevelList.Count > 0)
                {
                    ledSortLevel = ledSortLevelList[0];
                }
            }

            return ledSortLevel;
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Customize.Impl
{
    [Transactional]
    public partial class LedSortLevelMgrE : com.Sconit.Service.Customize.Impl.LedSortLevelMgr, ILedSortLevelMgrE
    {
    }
}

#endregion Extend Class