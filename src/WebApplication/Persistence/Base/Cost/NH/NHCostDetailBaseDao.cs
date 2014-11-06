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
    public class NHCostDetailBaseDao : NHDaoBase, ICostDetailBaseDao
    {
        public NHCostDetailBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCostDetail(CostDetail entity)
        {
            Create(entity);
        }

        public virtual IList<CostDetail> GetAllCostDetail()
        {
            return FindAll<CostDetail>();
        }

        public virtual CostDetail LoadCostDetail(Int32 id)
        {
            return FindById<CostDetail>(id);
        }

        public virtual void UpdateCostDetail(CostDetail entity)
        {
            Update(entity);
        }

        public virtual void DeleteCostDetail(Int32 id)
        {
            string hql = @"from CostDetail entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteCostDetail(CostDetail entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCostDetail(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from CostDetail entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCostDetail(IList<CostDetail> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (CostDetail entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteCostDetail(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
