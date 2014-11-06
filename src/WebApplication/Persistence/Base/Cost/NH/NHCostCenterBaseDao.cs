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
    public class NHCostCenterBaseDao : NHDaoBase, ICostCenterBaseDao
    {
        public NHCostCenterBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCostCenter(CostCenter entity)
        {
            Create(entity);
        }

        public virtual IList<CostCenter> GetAllCostCenter()
        {
            return FindAll<CostCenter>();
        }

        public virtual IList<CostCenter> GetAllCostCenter(bool includeInactive)
        {
            string hql = @"from CostCenter entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<CostCenter> result = FindAllWithCustomQuery<CostCenter>(hql);
            return result;
        }

        public virtual CostCenter LoadCostCenter(String code)
        {
            return FindById<CostCenter>(code);
        }

        public virtual void UpdateCostCenter(CostCenter entity)
        {
            Update(entity);
        }

        public virtual void DeleteCostCenter(String code)
        {
            string hql = @"from CostCenter entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteCostCenter(CostCenter entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCostCenter(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from CostCenter entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCostCenter(IList<CostCenter> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (CostCenter entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteCostCenter(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
