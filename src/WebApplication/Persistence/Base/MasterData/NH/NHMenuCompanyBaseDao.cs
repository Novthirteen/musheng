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
    public class NHMenuCompanyBaseDao : NHDaoBase, IMenuCompanyBaseDao
    {
        public NHMenuCompanyBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateMenuCompany(MenuCompany entity)
        {
            Create(entity);
        }

        public virtual IList<MenuCompany> GetAllMenuCompany()
        {
            return GetAllMenuCompany(false);
        }

        public virtual IList<MenuCompany> GetAllMenuCompany(bool includeInactive)
        {
            string hql = @"from MenuCompany entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<MenuCompany> result = FindAllWithCustomQuery<MenuCompany>(hql);
            return result;
        }

        public virtual MenuCompany LoadMenuCompany(Int32 id)
        {
            return FindById<MenuCompany>(id);
        }

        public virtual void UpdateMenuCompany(MenuCompany entity)
        {
            Update(entity);
        }

        public virtual void DeleteMenuCompany(Int32 id)
        {
            string hql = @"from MenuCompany entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteMenuCompany(MenuCompany entity)
        {
            Delete(entity);
        }

        public virtual void DeleteMenuCompany(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from MenuCompany entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteMenuCompany(IList<MenuCompany> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (MenuCompany entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteMenuCompany(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
