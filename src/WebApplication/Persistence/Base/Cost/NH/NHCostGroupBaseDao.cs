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
    public class NHCostGroupBaseDao : NHDaoBase, ICostGroupBaseDao
    {
        public NHCostGroupBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCostGroup(CostGroup entity)
        {
            Create(entity);
        }

        public virtual IList<CostGroup> GetAllCostGroup()
        {
            return FindAll<CostGroup>();
        }

        public virtual IList<CostGroup> GetAllCostGroup(bool includeInactive)
        {
            string hql = @"from CostGroup entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<CostGroup> result = FindAllWithCustomQuery<CostGroup>(hql);
            return result;
        }

        public virtual CostGroup LoadCostGroup(String code)
        {
            return FindById<CostGroup>(code);
        }

        public virtual void UpdateCostGroup(CostGroup entity)
        {
            Update(entity);
        }

        public virtual void DeleteCostGroup(String code)
        {
            string hql = @"from CostGroup entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteCostGroup(CostGroup entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCostGroup(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from CostGroup entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCostGroup(IList<CostGroup> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (CostGroup entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteCostGroup(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
