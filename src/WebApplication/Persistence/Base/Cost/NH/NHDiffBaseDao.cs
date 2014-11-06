using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.Cost;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.Cost.NH
{
    public class NHDiffBaseDao : NHDaoBase, IDiffBaseDao
    {
        public NHDiffBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateDiff(Diff entity)
        {
            Create(entity);
        }

        public virtual IList<Diff> GetAllDiff()
        {
            return FindAll<Diff>();
        }

        public virtual Diff LoadDiff(Int32 id)
        {
            return FindById<Diff>(id);
        }

        public virtual void UpdateDiff(Diff entity)
        {
            Update(entity);
        }

        public virtual void DeleteDiff(Int32 id)
        {
            string hql = @"from Diff entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteDiff(Diff entity)
        {
            Delete(entity);
        }

        public virtual void DeleteDiff(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from Diff entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteDiff(IList<Diff> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (Diff entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteDiff(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
