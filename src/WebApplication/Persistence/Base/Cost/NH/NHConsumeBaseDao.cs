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
    public class NHConsumeBaseDao : NHDaoBase, IConsumeBaseDao
    {
        public NHConsumeBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateConsume(Consume entity)
        {
            Create(entity);
        }

        public virtual IList<Consume> GetAllConsume()
        {
            return FindAll<Consume>();
        }

        public virtual Consume LoadConsume(Int32 id)
        {
            return FindById<Consume>(id);
        }

        public virtual void UpdateConsume(Consume entity)
        {
            Update(entity);
        }

        public virtual void DeleteConsume(Int32 id)
        {
            string hql = @"from Consume entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteConsume(Consume entity)
        {
            Delete(entity);
        }

        public virtual void DeleteConsume(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from Consume entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteConsume(IList<Consume> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (Consume entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteConsume(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
