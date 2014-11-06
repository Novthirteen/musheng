using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.MasterData.NH
{
    public class NHCompanyBaseDao : NHDaoBase, ICompanyBaseDao
    {
        public NHCompanyBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCompany(Company entity)
        {
            Create(entity);
        }

        public virtual IList<Company> GetAllCompany()
        {
            return GetAllCompany(false);
        }

        public virtual IList<Company> GetAllCompany(bool includeInactive)
        {
            string hql = @"from Company entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<Company> result = FindAllWithCustomQuery<Company>(hql);
            return result;
        }

        public virtual Company LoadCompany(String code)
        {
            return FindById<Company>(code);
        }

        public virtual void UpdateCompany(Company entity)
        {
            Update(entity);
        }

        public virtual void DeleteCompany(String code)
        {
            string hql = @"from Company entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteCompany(Company entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCompany(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from Company entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCompany(IList<Company> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (Company entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteCompany(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
