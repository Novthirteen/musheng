using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Customize;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.Customize;
using com.Sconit.Entity.Exception;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize.Impl
{
    [Transactional]
    public class LedColorLevelMgr : LedColorLevelBaseMgr, ILedColorLevelMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public bool CheckLedFeedColorLevel(string itemCode, string brand, string startLevel, string endLevel, string targetLevel)
        {
            LedColorLevel startLedColorLevel = GetLedColorLevel(itemCode, brand, startLevel);
            if (startLedColorLevel == null)
            {
                throw new BusinessErrorException("MasterData.LedColor.Error.ColorLevelNotExist", brand, startLevel);
            }

            LedColorLevel endLedColorLevel = GetLedColorLevel(itemCode, brand, endLevel);
            if (endLedColorLevel == null)
            {
                throw new BusinessErrorException("MasterData.LedColor.Error.ColorLevelNotExist", brand, endLevel);
            }

            LedColorLevel currentLedColorLevel = GetLedColorLevel(itemCode, brand, targetLevel);
            if (currentLedColorLevel == null)
            {
                throw new BusinessErrorException("MasterData.LedColor.Error.ColorLevelNotExist", brand, targetLevel);
            }

            if (currentLedColorLevel.Sequence < startLedColorLevel.Sequence
                || currentLedColorLevel.Sequence > endLedColorLevel.Sequence)
            {
                throw new BusinessErrorException("MasterData.LedColor.Error.ColorLevelNotInRange", targetLevel, startLevel, endLevel);
            }

            return true;
        }

        [Transaction(TransactionMode.Unspecified)]
        public bool IsNearByLedColorLevel(string itemCode, string brand, string orgLevel, string targetLevel)
        {
            LedColorLevel orgLedColorLevel = GetLedColorLevel(itemCode, brand, orgLevel);
            if (orgLedColorLevel == null)
            {
                throw new BusinessErrorException("MasterData.LedColor.Error.ColorLevelNotExist", brand, orgLevel);
            }

            LedColorLevel currentLedColorLevel = GetLedColorLevel(itemCode, brand, targetLevel);
            if (currentLedColorLevel == null)
            {
                throw new BusinessErrorException("MasterData.LedColor.Error.ColorLevelNotExist", brand, targetLevel);
            }

            if (orgLedColorLevel.Sequence == currentLedColorLevel.Sequence)
            {
                return false;
            }

            DetachedCriteria criteria = DetachedCriteria.For<LedColorLevel>();

            criteria.Add(Expression.Eq("Brand.Code", brand));
            if (orgLedColorLevel.Sequence > currentLedColorLevel.Sequence)
            {
                criteria.Add(Expression.Between("Sequence", currentLedColorLevel.Sequence, orgLedColorLevel.Sequence));
            }
            else
            {
                criteria.Add(Expression.Between("Sequence", currentLedColorLevel.Sequence, orgLedColorLevel.Sequence));
            }

            IList<LedColorLevel> ledColorLevelList = this.criteriaMgr.FindAll<LedColorLevel>(criteria);

            if (ledColorLevelList != null && ledColorLevelList.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private LedColorLevel GetLedColorLevel(string itemCode, string brand, string colorLevel)
        {
            DetachedCriteria criteria = DetachedCriteria.For<LedColorLevel>();

            LedColorLevel ledColorLevel = null;

            if (itemCode != null && itemCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Item.Code", itemCode));
                criteria.Add(Expression.Eq("Brand.Code", brand));
                criteria.Add(Expression.Eq("Value", colorLevel));

                IList<LedColorLevel> ledColorLevelList = this.criteriaMgr.FindAll<LedColorLevel>(criteria);

                if (ledColorLevelList != null && ledColorLevelList.Count > 0)
                {
                    ledColorLevel = ledColorLevelList[0];
                }
            }

            if (ledColorLevel == null)
            {
                criteria = DetachedCriteria.For<LedColorLevel>();

                criteria.Add(Expression.IsNull("Item"));
                criteria.Add(Expression.Eq("Brand.Code", brand));
                criteria.Add(Expression.Eq("Value", colorLevel));

                IList<LedColorLevel> ledColorLevelList = this.criteriaMgr.FindAll<LedColorLevel>(criteria);

                if (ledColorLevelList != null && ledColorLevelList.Count > 0)
                {
                    ledColorLevel = ledColorLevelList[0];
                }
            }

            return ledColorLevel;
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Customize.Impl
{
    [Transactional]
    public partial class LedColorLevelMgrE : com.Sconit.Service.Customize.Impl.LedColorLevelMgr, ILedColorLevelMgrE
    {
    }
}

#endregion Extend Class