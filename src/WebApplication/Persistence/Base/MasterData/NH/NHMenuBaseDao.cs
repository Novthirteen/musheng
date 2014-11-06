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
    public class NHMenuBaseDao : NHDaoBase, IMenuBaseDao
    {
        public NHMenuBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateMenu(Menu entity)
        {
            Create(entity);
        }

        public virtual IList<Menu> GetAllMenu()
        {
            return GetAllMenu(false);
        }

        public virtual IList<Menu> GetAllMenu(bool includeInactive)
        {
            string hql = @"from Menu entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<Menu> result = FindAllWithCustomQuery<Menu>(hql);
            return result;
        }

        public virtual Menu LoadMenu(String id)
        {
            return FindById<Menu>(id);
        }

        public virtual void UpdateMenu(Menu entity)
        {
            Update(entity);
        }

        public virtual void DeleteMenu(String id)
        {
            string hql = @"from Menu entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteMenu(Menu entity)
        {
            Delete(entity);
        }

        public virtual void DeleteMenu(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from Menu entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteMenu(IList<Menu> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (Menu entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteMenu(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
