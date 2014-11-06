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
    public class NHCostInventoryBalanceBaseDao : NHDaoBase, ICostInventoryBalanceBaseDao
    {
        public NHCostInventoryBalanceBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCostInventoryBalance(CostInventoryBalance entity)
        {
            Create(entity);
        }

        public virtual IList<CostInventoryBalance> GetAllCostInventoryBalance()
        {
            return FindAll<CostInventoryBalance>();
        }

        public virtual CostInventoryBalance LoadCostInventoryBalance(Int32 id)
        {
            return FindById<CostInventoryBalance>(id);
        }

        public virtual void UpdateCostInventoryBalance(CostInventoryBalance entity)
        {
            Update(entity);
        }

        public virtual void DeleteCostInventoryBalance(Int32 id)
        {
            string hql = @"from CostInventoryBalance entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteCostInventoryBalance(CostInventoryBalance entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCostInventoryBalance(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from CostInventoryBalance entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCostInventoryBalance(IList<CostInventoryBalance> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (CostInventoryBalance entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteCostInventoryBalance(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
