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
    public class NHCostAllocateTransactionBaseDao : NHDaoBase, ICostAllocateTransactionBaseDao
    {
        public NHCostAllocateTransactionBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCostAllocateTransaction(CostAllocateTransaction entity)
        {
            Create(entity);
        }

        public virtual IList<CostAllocateTransaction> GetAllCostAllocateTransaction()
        {
            return FindAll<CostAllocateTransaction>();
        }

        public virtual CostAllocateTransaction LoadCostAllocateTransaction(Int32 id)
        {
            return FindById<CostAllocateTransaction>(id);
        }

        public virtual void UpdateCostAllocateTransaction(CostAllocateTransaction entity)
        {
            Update(entity);
        }

        public virtual void DeleteCostAllocateTransaction(Int32 id)
        {
            string hql = @"from CostAllocateTransaction entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteCostAllocateTransaction(CostAllocateTransaction entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCostAllocateTransaction(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from CostAllocateTransaction entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCostAllocateTransaction(IList<CostAllocateTransaction> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (CostAllocateTransaction entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteCostAllocateTransaction(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
