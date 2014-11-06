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
    public class NHStandardCostBaseDao : NHDaoBase, IStandardCostBaseDao
    {
        public NHStandardCostBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateStandardCost(StandardCost entity)
        {
            Create(entity);
        }

        public virtual IList<StandardCost> GetAllStandardCost()
        {
            return FindAll<StandardCost>();
        }

        public virtual StandardCost LoadStandardCost(Int32 id)
        {
            return FindById<StandardCost>(id);
        }

        public virtual void UpdateStandardCost(StandardCost entity)
        {
            Update(entity);
        }

        public virtual void DeleteStandardCost(Int32 id)
        {
            string hql = @"from StandardCost entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteStandardCost(StandardCost entity)
        {
            Delete(entity);
        }

        public virtual void DeleteStandardCost(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from StandardCost entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteStandardCost(IList<StandardCost> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (StandardCost entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteStandardCost(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
