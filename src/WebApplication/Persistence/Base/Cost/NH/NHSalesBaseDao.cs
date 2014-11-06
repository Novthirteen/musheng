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
    public class NHSalesBaseDao : NHDaoBase, ISalesBaseDao
    {
        public NHSalesBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateSales(Sales entity)
        {
            Create(entity);
        }

        public virtual IList<Sales> GetAllSales()
        {
            return FindAll<Sales>();
        }

        public virtual Sales LoadSales(Int32 id)
        {
            return FindById<Sales>(id);
        }

        public virtual void UpdateSales(Sales entity)
        {
            Update(entity);
        }

        public virtual void DeleteSales(Int32 id)
        {
            string hql = @"from Sales entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteSales(Sales entity)
        {
            Delete(entity);
        }

        public virtual void DeleteSales(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from Sales entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteSales(IList<Sales> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (Sales entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteSales(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
