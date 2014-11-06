using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.View;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.View.NH
{
    public class NHMenuViewBaseDao : NHDaoBase, IMenuViewBaseDao
    {
        public NHMenuViewBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateMenuView(MenuView entity)
        {
            Create(entity);
        }

        public virtual IList<MenuView> GetAllMenuView()
        {
            return GetAllMenuView(false);
        }

        public virtual IList<MenuView> GetAllMenuView(bool includeInactive)
        {
            string hql = @"from MenuView entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<MenuView> result = FindAllWithCustomQuery<MenuView>(hql);
            return result;
        }

        public virtual MenuView LoadMenuView(String Code)
        {
            return FindById<MenuView>(Code);
        }

        public virtual void UpdateMenuView(MenuView entity)
        {
            Update(entity);
        }

        public virtual void DeleteMenuView(String Code)
        {
            string hql = @"from MenuView entity where entity.Code = ?";
            Delete(hql, new object[] { Code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteMenuView(MenuView entity)
        {
            Delete(entity);
        }

        public virtual void DeleteMenuView(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from MenuView entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteMenuView(IList<MenuView> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (MenuView entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteMenuView(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
