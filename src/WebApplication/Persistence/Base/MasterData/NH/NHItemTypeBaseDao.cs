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
    public class NHItemTypeBaseDao : NHDaoBase, IItemTypeBaseDao
    {
        public NHItemTypeBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateItemType(ItemType entity)
        {
            Create(entity);
        }

        public virtual IList<ItemType> GetAllItemType()
        {
            return FindAll<ItemType>();
        }

        public virtual ItemType LoadItemType(String code)
        {
            return FindById<ItemType>(code);
        }

        public virtual void UpdateItemType(ItemType entity)
        {
            Update(entity);
        }

        public virtual void DeleteItemType(String code)
        {
            string hql = @"from ItemType entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteItemType(ItemType entity)
        {
            Delete(entity);
        }

        public virtual void DeleteItemType(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from ItemType entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteItemType(IList<ItemType> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (ItemType entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteItemType(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
