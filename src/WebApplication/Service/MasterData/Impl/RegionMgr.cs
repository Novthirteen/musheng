using com.Sconit.Service.Ext.MasterData;


using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using NHibernate.Expression;
using System;
using System.Data.SqlClient;
using System.Data;
using com.Sconit.Persistence;
using com.Sconit.Entity.Exception;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class RegionMgr : RegionBaseMgr, IRegionMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IWorkCenterMgrE workCenterMgrE { get; set; }
        public IAddressMgrE addressMgrE { get; set; }
        public IPermissionMgrE permissionMgrE { get; set; }
        public IPermissionCategoryMgrE permissionCategoryMgrE { get; set; }
        public IUserPermissionMgrE userPermissionMgrE { get; set; }
        public IPartyDao partyDao { get; set; }
        public ISqlHelperDao sqlHelperDao { get; set; }
        public IUserMgrE userMgrE;
        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public override void DeleteRegion(string code)
        {
            IList<UserPermission> userPermissionList = userPermissionMgrE.GetUserPermission(code);
            userPermissionMgrE.DeleteUserPermission(userPermissionList);
            permissionMgrE.DeletePermission(code);
            if (partyDao.LoadParty(code) == null)
            {
                workCenterMgrE.DeleteWorkCenterByParent(code);
                addressMgrE.DeleteAddressByParent(code);
                base.DeleteRegion(code);
            }
            else
            {
                DeleteRegionOnly(code);
            }

        }

        [Transaction(TransactionMode.Unspecified)]
        public override void DeleteRegion(Region region)
        {
            DeleteRegion(region.Code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public int DeleteRegionOnly(string code)
        {
            string sql = "delete from Region where code=@code ";

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@code", SqlDbType.NVarChar, 50);
            param[0].Value = code;
            return sqlHelperDao.Delete(sql, param);
        }

        [Transaction(TransactionMode.Unspecified)]
        public override void CreateRegion(Region entity)
        {
            CreateRegion(entity, userMgrE.GetMonitorUser());
        }

        [Transaction(TransactionMode.Unspecified)]
        public void CreateRegion(Region entity, User currentUser)
        {

            if (partyDao.LoadParty(entity.Code) == null)
            {
                base.CreateRegion(entity);
            }
            else
            {
                CreateRegionOnly(entity);
            }
            Permission permission = new Permission();
            permission.Category = permissionCategoryMgrE.LoadPermissionCategory(BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_REGION);
            permission.Code = entity.Code;
            permission.Description = entity.Name;
            permissionMgrE.CreatePermission(permission);
            UserPermission userPermission = new UserPermission();
            userPermission.User = currentUser;
            userPermission.Permission = permission;
            userPermissionMgrE.CreateUserPermission(userPermission);

        }

        [Transaction(TransactionMode.Unspecified)]
        public int CreateRegionOnly(Region entity)
        {
            string sql = "insert into Region(code) values(@code) ";

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@code", SqlDbType.NVarChar, 50);
            param[0].Value = entity.Code;
            return sqlHelperDao.Create(sql, param);
        }


        [Transaction(TransactionMode.Unspecified)]
        public IList<Region> GetRegion(string userCode)
        {
            return GetRegion(userCode, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Region> GetRegion(string userCode, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Region>();
            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }

            DetachedCriteria[] pCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);

            criteria.Add(
                Expression.Or(
                    Subqueries.PropertyIn("Code", pCrieteria[0]),
                    Subqueries.PropertyIn("Code", pCrieteria[1])));

            return criteriaMgrE.FindAll<Region>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Region CheckAndLoadRegion(string regionCode)
        {
            Region region = this.LoadRegion(regionCode);
            if (region == null)
            {
                throw new BusinessErrorException("Region.Error.RegionCodeNotExist", regionCode);
            }

            return region;
        }

        [Transaction(TransactionMode.Unspecified)]
        public string GetDefaultInspectLocation(string regionCode)
        {
            return GetDefaultInspectLocation(regionCode, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public string GetDefaultInspectLocation(string regionCode, string defaultInspectLocation)
        {
            Region region = this.CheckAndLoadRegion(regionCode);
            if (defaultInspectLocation != null && defaultInspectLocation.Trim() != string.Empty)
            {
                return defaultInspectLocation;
            }

            string inspectLocation = region.InspectLocation;
            if (inspectLocation == null && inspectLocation.Trim() == string.Empty)
            {
                throw new BusinessErrorException("MasterData.Region.Error.InspectLocationIsEmpty", regionCode);
            }
            else
            {
                return inspectLocation;
            }

        }

        [Transaction(TransactionMode.Unspecified)]
        public string GetDefaultRejectLocation(string regionCode)
        {
            return GetDefaultRejectLocation(regionCode, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public string GetDefaultRejectLocation(string regionCode, string defaultRejectLocation)
        {
            Region region = this.CheckAndLoadRegion(regionCode);
            if (defaultRejectLocation != null && defaultRejectLocation.Trim() != string.Empty)
            {
                return defaultRejectLocation;
            }

            string rejectLocation = region.RejectLocation;
            if (rejectLocation == null && rejectLocation.Trim() == string.Empty)
            {
                throw new BusinessErrorException("MasterData.Region.Error.RejectLocationIsEmpty", regionCode);
            }
            else
            {
                return rejectLocation;
            }
        }
        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class RegionMgrE : com.Sconit.Service.MasterData.Impl.RegionMgr, IRegionMgrE
    {

    }
}
#endregion
