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
    public class NHItemDiscontinueBaseDao : NHDaoBase, IItemDiscontinueBaseDao
    {
        public NHItemDiscontinueBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateItemDiscontinue(ItemDiscontinue entity)
        {
            Create(entity);
        }

        public virtual IList<ItemDiscontinue> GetAllItemDiscontinue()
        {
            return FindAll<ItemDiscontinue>();
        }

        public virtual ItemDiscontinue LoadItemDiscontinue(Int32 id)
        {
            return FindById<ItemDiscontinue>(id);
        }

        public virtual void UpdateItemDiscontinue(ItemDiscontinue entity)
        {
            Update(entity);
        }

        public virtual void DeleteItemDiscontinue(Int32 id)
        {
            string hql = @"from ItemDiscontinue entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteItemDiscontinue(ItemDiscontinue entity)
        {
            Delete(entity);
        }

        public virtual void DeleteItemDiscontinue(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from ItemDiscontinue entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteItemDiscontinue(IList<ItemDiscontinue> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (ItemDiscontinue entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteItemDiscontinue(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
