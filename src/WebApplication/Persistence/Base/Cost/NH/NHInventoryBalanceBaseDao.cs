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
    public class NHInventoryBalanceBaseDao : NHDaoBase, IInventoryBalanceBaseDao
    {
        public NHInventoryBalanceBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateInventoryBalance(InventoryBalance entity)
        {
            Create(entity);
        }

        public virtual IList<InventoryBalance> GetAllInventoryBalance()
        {
            return FindAll<InventoryBalance>();
        }

        public virtual InventoryBalance LoadInventoryBalance(Int32 id)
        {
            return FindById<InventoryBalance>(id);
        }

        public virtual void UpdateInventoryBalance(InventoryBalance entity)
        {
            Update(entity);
        }

        public virtual void DeleteInventoryBalance(Int32 id)
        {
            string hql = @"from InventoryBalance entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteInventoryBalance(InventoryBalance entity)
        {
            Delete(entity);
        }

        public virtual void DeleteInventoryBalance(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from InventoryBalance entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteInventoryBalance(IList<InventoryBalance> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (InventoryBalance entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteInventoryBalance(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
