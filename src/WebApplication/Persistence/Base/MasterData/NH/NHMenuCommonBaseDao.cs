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
    public class NHMenuCommonBaseDao : NHDaoBase, IMenuCommonBaseDao
    {
        public NHMenuCommonBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateMenuCommon(MenuCommon entity)
        {
            Create(entity);
        }

        public virtual IList<MenuCommon> GetAllMenuCommon()
        {
            return GetAllMenuCommon(false);
        }

        public virtual IList<MenuCommon> GetAllMenuCommon(bool includeInactive)
        {
            string hql = @"from MenuCommon entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<MenuCommon> result = FindAllWithCustomQuery<MenuCommon>(hql);
            return result;
        }

        public virtual MenuCommon LoadMenuCommon(Int32 id)
        {
            return FindById<MenuCommon>(id);
        }

        public virtual void UpdateMenuCommon(MenuCommon entity)
        {
            Update(entity);
        }

        public virtual void DeleteMenuCommon(Int32 id)
        {
            string hql = @"from MenuCommon entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteMenuCommon(MenuCommon entity)
        {
            Delete(entity);
        }

        public virtual void DeleteMenuCommon(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from MenuCommon entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteMenuCommon(IList<MenuCommon> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (MenuCommon entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteMenuCommon(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
