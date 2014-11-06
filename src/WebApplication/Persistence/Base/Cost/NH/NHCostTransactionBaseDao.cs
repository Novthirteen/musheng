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
    public class NHCostTransactionBaseDao : NHDaoBase, ICostTransactionBaseDao
    {
        public NHCostTransactionBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCostTransaction(CostTransaction entity)
        {
            Create(entity);
        }

        public virtual IList<CostTransaction> GetAllCostTransaction()
        {
            return FindAll<CostTransaction>();
        }

        public virtual CostTransaction LoadCostTransaction(Int32 id)
        {
            return FindById<CostTransaction>(id);
        }

        public virtual void UpdateCostTransaction(CostTransaction entity)
        {
            Update(entity);
        }

        public virtual void DeleteCostTransaction(Int32 id)
        {
            string hql = @"from CostTransaction entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteCostTransaction(CostTransaction entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCostTransaction(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from CostTransaction entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCostTransaction(IList<CostTransaction> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (CostTransaction entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteCostTransaction(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
