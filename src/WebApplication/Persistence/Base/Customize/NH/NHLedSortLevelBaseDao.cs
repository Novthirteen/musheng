using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.Customize;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.Customize.NH
{
    public class NHLedSortLevelBaseDao : NHDaoBase, ILedSortLevelBaseDao
    {
        public NHLedSortLevelBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateLedSortLevel(LedSortLevel entity)
        {
            Create(entity);
        }

        public virtual IList<LedSortLevel> GetAllLedSortLevel()
        {
            return GetAllLedSortLevel(false);
        }

        public virtual IList<LedSortLevel> GetAllLedSortLevel(bool includeInactive)
        {
            string hql = @"from LedSortLevel entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<LedSortLevel> result = FindAllWithCustomQuery<LedSortLevel>(hql);
            return result;
        }

        public virtual LedSortLevel LoadLedSortLevel(Int32 id)
        {
            return FindById<LedSortLevel>(id);
        }

        public virtual void UpdateLedSortLevel(LedSortLevel entity)
        {
            Update(entity);
        }

        public virtual void DeleteLedSortLevel(Int32 id)
        {
            string hql = @"from LedSortLevel entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteLedSortLevel(LedSortLevel entity)
        {
            Delete(entity);
        }

        public virtual void DeleteLedSortLevel(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from LedSortLevel entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteLedSortLevel(IList<LedSortLevel> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (LedSortLevel entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteLedSortLevel(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
