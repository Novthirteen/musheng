using com.Sconit.Service.Ext.MasterData;


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class UserPermissionMgr : UserPermissionBaseMgr, IUserPermissionMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IPermissionMgrE permissionMgrE { get; set; }
        

        #region Customized Methods

        public IList<Permission> GetPermissionsNotInUser(string code, string categoryCode)
        {
            IList<Permission> allPermissions = permissionMgrE.GetALlPermissionsByCategory(categoryCode);
            IList<Permission> userPermissions = GetPermissionsByUserCode(code, categoryCode);
            List<Permission> otherPermissions = new List<Permission>();
            if (allPermissions != null && allPermissions.Count > 0)
            {
                foreach (Permission r in allPermissions)
                {
                    if (!userPermissions.Contains(r))
                    {
                        otherPermissions.Add(r);
                    }
                }
            }
            var q = otherPermissions.OrderBy(a => a.Description);
            return q.ToList();
        }

        public IList<Permission> GetPermissionsNotInUser(string code)
        {
            return GetPermissionsNotInUser(code, null);
        }

        public IList<Permission> GetPermissionsByUserCode(string code, string categoryCode)
        {

            DetachedCriteria criteria = DetachedCriteria.For(typeof(UserPermission)).Add(Expression.Eq("User.Code", code))
              .SetProjection(Projections.Property("Permission"));
            if (categoryCode != null)
            {
                criteria.CreateAlias("Permission", "p").CreateAlias("p.Category", "c")
                    .Add(Expression.Eq("c.Code", categoryCode));
            }
            IList<Permission> permissions = criteriaMgrE.FindAll<Permission>(criteria);
            var q = permissions.OrderBy(p => p.Description);
            return q.ToList();
        }

        public IList<Permission> GetPermissionsByUserCode(string code)
        {
            return GetPermissionsByUserCode(code, null);
        }

        public IList<UserPermission> GetUserPermission(string permissionCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(UserPermission));

            criteria.CreateAlias("Permission", "p")
                .Add(Expression.Eq("p.Code", permissionCode));

            return criteriaMgrE.FindAll<UserPermission>(criteria);
        }

        public UserPermission LoadUserPermission(string userCode, int permissionId)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(UserPermission)).Add(Expression.Eq("User.Code", userCode)).Add(Expression.Eq("Permission.Id", permissionId));
            IList<UserPermission> urList = criteriaMgrE.FindAll<UserPermission>(criteria);
            if (urList.Count == 0) return null;
            return urList[0];
        }

        public UserPermission LoadUserPermission(string userCode, string permissionCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(UserPermission))
                .CreateAlias("User", "u")
                .CreateAlias("Permission", "p")
                .Add(Expression.Eq("u.Code", userCode))
                .Add(Expression.Eq("p.Code", permissionCode));
            IList<UserPermission> urList = criteriaMgrE.FindAll<UserPermission>(criteria);
            if (urList.Count == 0) return null;
            return urList[0];
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateUserPermissions(User user, IList<Permission> rList)
        {
            foreach (Permission permission in rList)
            {
                UserPermission userPermission = new UserPermission();
                userPermission.User = user;
                userPermission.Permission = permission;
                entityDao.CreateUserPermission(userPermission);
            }
        }

        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class UserPermissionMgrE : com.Sconit.Service.MasterData.Impl.UserPermissionMgr, IUserPermissionMgrE
    {
        
    }
}
#endregion
