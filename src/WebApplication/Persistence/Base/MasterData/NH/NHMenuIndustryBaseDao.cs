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
    public class NHMenuIndustryBaseDao : NHDaoBase, IMenuIndustryBaseDao
    {
        public NHMenuIndustryBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateMenuIndustry(MenuIndustry entity)
        {
            Create(entity);
        }

        public virtual IList<MenuIndustry> GetAllMenuIndustry()
        {
            return GetAllMenuIndustry(false);
        }

        public virtual IList<MenuIndustry> GetAllMenuIndustry(bool includeInactive)
        {
            string hql = @"from MenuIndustry entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<MenuIndustry> result = FindAllWithCustomQuery<MenuIndustry>(hql);
            return result;
        }

        public virtual MenuIndustry LoadMenuIndustry(Int32 id)
        {
            return FindById<MenuIndustry>(id);
        }

        public virtual void UpdateMenuIndustry(MenuIndustry entity)
        {
            Update(entity);
        }

        public virtual void DeleteMenuIndustry(Int32 id)
        {
            string hql = @"from MenuIndustry entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteMenuIndustry(MenuIndustry entity)
        {
            Delete(entity);
        }

        public virtual void DeleteMenuIndustry(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from MenuIndustry entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteMenuIndustry(IList<MenuIndustry> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (MenuIndustry entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteMenuIndustry(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
