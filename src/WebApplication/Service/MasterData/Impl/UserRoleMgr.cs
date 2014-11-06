using com.Sconit.Service.Ext.MasterData;


using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using System.Text;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class UserRoleMgr : UserRoleBaseMgr, IUserRoleMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        private static IDictionary<string, IList> userRole = new Dictionary<string, IList>();
        public IRoleDao roleDao { get; set; }
        public IUserDao userDao { get; set; }
        

        #region Customized Methods

        public IList<Role> GetRolesNotInUser(string userCode)
        {
            IList<Role> allRoles = roleDao.GetAllRole();
            IList<Role> userRoles = GetRolesByUserCode(userCode);
            List<Role> otherRoles = new List<Role>();
            if (allRoles != null && allRoles.Count > 0)
            {
                foreach (Role r in allRoles)
                {
                    if (!userRoles.Contains(r))
                    {
                        otherRoles.Add(r);
                    }
                }
            }
            return otherRoles;
        }

        public IList<Role> GetRolesByUserCode(string userCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(UserRole)).Add(Expression.Eq("User.Code", userCode));
            List<Role> rList = new List<Role>();
            IList<UserRole> urList = criteriaMgrE.FindAll<UserRole>(criteria);
            foreach (UserRole ur in urList)
            {
                rList.Add(ur.Role);
            }
            return rList;
        }

        public IList<User> GetUsersNotInRole(string roleCode)
        {
            IList<User> allUsers = userDao.GetAllUser();
            IList<User> roleUsers = GetUsersByRoleCode(roleCode);
            List<User> otherUsers = new List<User>();
            if (allUsers != null && allUsers.Count > 0)
            {
                foreach (User u in allUsers)
                {
                    if (!roleUsers.Contains(u))
                    {
                        otherUsers.Add(u);
                    }
                }
            }
            return otherUsers;
        }

        public IList<User> GetUsersByRoleCode(string roleCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(UserRole)).Add(Expression.Eq("Role.Code", roleCode));
            List<User> uList = new List<User>();
            IList<UserRole> urList = criteriaMgrE.FindAll<UserRole>(criteria);
            foreach (UserRole ur in urList)
            {
                uList.Add(ur.User);
            }
            return uList;
        }

        

        public UserRole LoadUserRole(string userCode, string roleCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(UserRole)).Add(Expression.Eq("User.Code", userCode)).Add(Expression.Eq("Role.Code", roleCode));
            IList<UserRole> urList = criteriaMgrE.FindAll<UserRole>(criteria);
            if (urList.Count == 0) return null;
            return urList[0];
        }

        public void CreateUserRoles(User user, IList<Role> rList)
        {
            foreach (Role role in rList)
            {
                UserRole userRole = new UserRole();
                userRole.User = user;
                userRole.Role = role;
                entityDao.CreateUserRole(userRole);
            }
        }

        public void CreateUserRoles(IList<User> uList, Role role)
        {
            foreach (User user in uList)
            {
                UserRole userRole = new UserRole();
                userRole.User = user;
                userRole.Role = role;
                entityDao.CreateUserRole(userRole);
            }
        }
        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class UserRoleMgrE : com.Sconit.Service.MasterData.Impl.UserRoleMgr, IUserRoleMgrE
    {
        
    }
}
#endregion
