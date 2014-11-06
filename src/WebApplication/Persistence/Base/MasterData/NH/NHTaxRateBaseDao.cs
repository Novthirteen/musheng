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
    public class NHTaxRateBaseDao : NHDaoBase, ITaxRateBaseDao
    {
        public NHTaxRateBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateTaxRate(TaxRate entity)
        {
            Create(entity);
        }

        public virtual IList<TaxRate> GetAllTaxRate()
        {
            return FindAll<TaxRate>();
        }

        public virtual TaxRate LoadTaxRate(String code)
        {
            return FindById<TaxRate>(code);
        }

        public virtual void UpdateTaxRate(TaxRate entity)
        {
            Update(entity);
        }

        public virtual void DeleteTaxRate(String code)
        {
            string hql = @"from TaxRate entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteTaxRate(TaxRate entity)
        {
            Delete(entity);
        }

        public virtual void DeleteTaxRate(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from TaxRate entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteTaxRate(IList<TaxRate> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (TaxRate entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteTaxRate(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
