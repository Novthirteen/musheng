using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.MasterData.NH
{
    public class NHRoleBaseDao : NHDaoBase, IRoleBaseDao
    {
        public NHRoleBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateRole(Role entity)
        {
            Create(entity);
        }

        public virtual IList<Role> GetAllRole()
        {
            return FindAll<Role>();
        }

        public virtual Role LoadRole(String code)
        {
            return FindById<Role>(code);
        }

        public virtual void UpdateRole(Role entity)
        {
            Update(entity);
        }

        public virtual void DeleteRole(String code)
        {
            string hql = @"from Role entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteRole(Role entity)
        {
            Delete(entity);
        }

        public virtual void DeleteRole(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from Role entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteRole(IList<Role> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (Role entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteRole(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
