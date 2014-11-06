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
    public class NHCostAllocateMethodBaseDao : NHDaoBase, ICostAllocateMethodBaseDao
    {
        public NHCostAllocateMethodBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCostAllocateMethod(CostAllocateMethod entity)
        {
            Create(entity);
        }

        public virtual IList<CostAllocateMethod> GetAllCostAllocateMethod()
        {
            return FindAll<CostAllocateMethod>();
        }

        public virtual CostAllocateMethod LoadCostAllocateMethod(Int32 id)
        {
            return FindById<CostAllocateMethod>(id);
        }

        public virtual void UpdateCostAllocateMethod(CostAllocateMethod entity)
        {
            Update(entity);
        }

        public virtual void DeleteCostAllocateMethod(Int32 id)
        {
            string hql = @"from CostAllocateMethod entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteCostAllocateMethod(CostAllocateMethod entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCostAllocateMethod(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from CostAllocateMethod entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCostAllocateMethod(IList<CostAllocateMethod> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (CostAllocateMethod entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteCostAllocateMethod(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
