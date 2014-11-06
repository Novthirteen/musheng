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
    public class NHFgCostBaseDao : NHDaoBase, IFgCostBaseDao
    {
        public NHFgCostBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateFgCost(FgCost entity)
        {
            Create(entity);
        }

        public virtual void BatchCreate(IList<FgCost> entities)
        {
            BatchCreate(entities);
        }

        public virtual IList<FgCost> GetAllFgCost()
        {
            return FindAll<FgCost>();
        }

        public virtual FgCost LoadFgCost(Int32 id)
        {
            return FindById<FgCost>(id);
        }

        public virtual void UpdateFgCost(FgCost entity)
        {
            Update(entity);
        }

        public virtual void DeleteFgCost(Int32 id)
        {
            string hql = @"from FgCost entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteFgCost(FgCost entity)
        {
            Delete(entity);
        }

        public virtual void DeleteFgCost(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from FgCost entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteFgCost(IList<FgCost> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (FgCost entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteFgCost(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
