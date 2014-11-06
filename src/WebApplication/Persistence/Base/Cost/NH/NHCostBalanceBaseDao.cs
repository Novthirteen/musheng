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
    public class NHCostBalanceBaseDao : NHDaoBase, ICostBalanceBaseDao
    {
        public NHCostBalanceBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCostBalance(CostBalance entity)
        {
            Create(entity);
        }

        public virtual IList<CostBalance> GetAllCostBalance()
        {
            return FindAll<CostBalance>();
        }

        public virtual CostBalance LoadCostBalance(Int32 id)
        {
            return FindById<CostBalance>(id);
        }

        public virtual void UpdateCostBalance(CostBalance entity)
        {
            Update(entity);
        }

        public virtual void DeleteCostBalance(Int32 id)
        {
            string hql = @"from CostBalance entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteCostBalance(CostBalance entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCostBalance(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from CostBalance entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCostBalance(IList<CostBalance> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (CostBalance entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteCostBalance(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
