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
    public class NHLedColorLevelBaseDao : NHDaoBase, ILedColorLevelBaseDao
    {
        public NHLedColorLevelBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateLedColorLevel(LedColorLevel entity)
        {
            Create(entity);
        }

        public virtual IList<LedColorLevel> GetAllLedColorLevel()
        {
            return GetAllLedColorLevel(false);
        }

        public virtual IList<LedColorLevel> GetAllLedColorLevel(bool includeInactive)
        {
            string hql = @"from LedColorLevel entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<LedColorLevel> result = FindAllWithCustomQuery<LedColorLevel>(hql);
            return result;
        }

        public virtual LedColorLevel LoadLedColorLevel(Int32 id)
        {
            return FindById<LedColorLevel>(id);
        }

        public virtual void UpdateLedColorLevel(LedColorLevel entity)
        {
            Update(entity);
        }

        public virtual void DeleteLedColorLevel(Int32 id)
        {
            string hql = @"from LedColorLevel entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteLedColorLevel(LedColorLevel entity)
        {
            Delete(entity);
        }

        public virtual void DeleteLedColorLevel(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from LedColorLevel entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteLedColorLevel(IList<LedColorLevel> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (LedColorLevel entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteLedColorLevel(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
