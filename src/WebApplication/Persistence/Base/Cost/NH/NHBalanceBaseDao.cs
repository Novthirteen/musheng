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
    public class NHBalanceBaseDao : NHDaoBase, IBalanceBaseDao
    {
        public NHBalanceBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateBalance(Balance entity)
        {
            Create(entity);
        }

        public virtual IList<Balance> GetAllBalance()
        {
            return FindAll<Balance>();
        }

        public virtual Balance LoadBalance(Int32 id)
        {
            return FindById<Balance>(id);
        }

        public virtual void UpdateBalance(Balance entity)
        {
            Update(entity);
        }

        public virtual void DeleteBalance(Int32 id)
        {
            string hql = @"from Balance entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteBalance(Balance entity)
        {
            Delete(entity);
        }

        public virtual void DeleteBalance(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from Balance entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteBalance(IList<Balance> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (Balance entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteBalance(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
