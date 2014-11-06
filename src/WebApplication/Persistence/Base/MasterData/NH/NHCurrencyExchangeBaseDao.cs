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
    public class NHCurrencyExchangeBaseDao : NHDaoBase, ICurrencyExchangeBaseDao
    {
        public NHCurrencyExchangeBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCurrencyExchange(CurrencyExchange entity)
        {
            Create(entity);
        }

        public virtual IList<CurrencyExchange> GetAllCurrencyExchange()
        {
            return FindAll<CurrencyExchange>();
        }

        public virtual CurrencyExchange LoadCurrencyExchange(Int32 id)
        {
            return FindById<CurrencyExchange>(id);
        }

        public virtual void UpdateCurrencyExchange(CurrencyExchange entity)
        {
            Update(entity);
        }

        public virtual void DeleteCurrencyExchange(Int32 id)
        {
            string hql = @"from CurrencyExchange entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteCurrencyExchange(CurrencyExchange entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCurrencyExchange(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from CurrencyExchange entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCurrencyExchange(IList<CurrencyExchange> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (CurrencyExchange entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteCurrencyExchange(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
