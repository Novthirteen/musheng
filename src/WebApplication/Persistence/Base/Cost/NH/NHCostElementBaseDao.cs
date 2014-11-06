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
    public class NHCostElementBaseDao : NHDaoBase, ICostElementBaseDao
    {
        public NHCostElementBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCostElement(CostElement entity)
        {
            Create(entity);
        }

        public virtual IList<CostElement> GetAllCostElement()
        {
            return FindAll<CostElement>();
        }

        public virtual CostElement LoadCostElement(String code)
        {
            return FindById<CostElement>(code);
        }

        public virtual void UpdateCostElement(CostElement entity)
        {
            Update(entity);
        }

        public virtual void DeleteCostElement(String code)
        {
            string hql = @"from CostElement entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteCostElement(CostElement entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCostElement(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from CostElement entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCostElement(IList<CostElement> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (CostElement entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteCostElement(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
