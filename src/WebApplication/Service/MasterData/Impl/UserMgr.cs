using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class UserMgr : UserBaseMgr, IUserMgr
    {
        public IUserPermissionMgrE userPermissionMgrE { get; set; }
        public IUserRoleMgrE userRoleMgrE { get; set; }
        public IRolePermissionMgrE rolePermissionMgrE { get; set; }


        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public override void DeleteUser(string code)
        {
            List<UserRole> urList = new List<UserRole>();
            IList<Role> roleList = userRoleMgrE.GetRolesByUserCode(code);
            foreach (Role role in roleList)
            {
                UserRole userRole = userRoleMgrE.LoadUserRole(code, role.Code);
                urList.Add(userRole);
            }
            if (urList.Count > 0) userRoleMgrE.DeleteUserRole(urList);
            base.DeleteUser(code);
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteUser(User user)
        {
            this.DeleteUser(user.Code);
        }


        [Transaction(TransactionMode.Unspecified)]
        public User CheckAndLoadUser(string userCode)
        {
            User user = this.LoadUser(userCode, true, true);
            if (user == null)
            {
                throw new BusinessErrorException("Security.Error.UserCodeNotExist", userCode);
            }

            return user;
        }

        [Transaction(TransactionMode.Unspecified)]
        public User LoadUser(string code, bool isLoadUserPreference, bool isLoadPermission)
        {
            User user = entityDao.LoadUser(code);
            if (isLoadUserPreference)
            {
                if (user != null && user.UserPreferences != null && user.UserPreferences.Count > 0)
                {
                    //just for lazy load user.UserPreference
                }
            }
            if (user != null && isLoadPermission)
            {
                user.Permissions = GetAllPermissions(code);
            }
            return user;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Permission> GetAllPermissions(string usrCode)
        {
            List<Permission> userPermissions = (List<Permission>)(userPermissionMgrE.GetPermissionsByUserCode(usrCode));
            List<Role> userRoles = (List<Role>)(userRoleMgrE.GetRolesByUserCode(usrCode));
            if (userRoles.Count > 0)
            {
                foreach (Role role in userRoles)
                {
                    List<Permission> rolePermissions = (List<Permission>)(rolePermissionMgrE.GetPermissionsByRoleCode(role.Code));
                    foreach (Permission p in rolePermissions)
                    {
                        if (!userPermissions.Contains(p))
                        {
                            userPermissions.Add(p);
                        }
                    }
                }
            }
            return userPermissions;
        }

        [Transaction(TransactionMode.Requires)]
        public bool HasPermission(string userCode, string permissionCode)
        {
            IList<Permission> listPermission = GetAllPermissions(userCode);
            foreach (Permission p in listPermission)
            {
                if (p.Code == permissionCode)
                {
                    return true;
                }
            }
            return false;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<User> GetAllUser(DateTime LastModifyDate)
        {
            IList<User> userList = GetAllUser(true);
            IList<User> resulteUserList = new List<User>();
            foreach (User user in userList)
            {
                if (user.LastModifyDate > LastModifyDate)
                {
                    resulteUserList.Add(user);
                }
            }
            return resulteUserList;
        }


        [Transaction(TransactionMode.Unspecified)]
        public User GetMonitorUser()
        {
            return GetMonitorUser(false, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public User GetMonitorUser(bool isLoadUserPreference, bool isLoadPermission)
        {
            User user = this.LoadUser(BusinessConstants.SYSTEM_USER_MONITOR, isLoadUserPreference, isLoadPermission);
            return user;
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class UserMgrE : com.Sconit.Service.MasterData.Impl.UserMgr, IUserMgrE
    {

    }
}
#endregion
