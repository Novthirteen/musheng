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
    public class NHItemCategoryBaseDao : NHDaoBase, IItemCategoryBaseDao
    {
        public NHItemCategoryBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateItemCategory(ItemCategory entity)
        {
            Create(entity);
        }

        public virtual IList<ItemCategory> GetAllItemCategory()
        {
            return FindAll<ItemCategory>();
        }

        public virtual ItemCategory LoadItemCategory(String code)
        {
            return FindById<ItemCategory>(code);
        }

        public virtual void UpdateItemCategory(ItemCategory entity)
        {
            Update(entity);
        }

        public virtual void DeleteItemCategory(String code)
        {
            string hql = @"from ItemCategory entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteItemCategory(ItemCategory entity)
        {
            Delete(entity);
        }

        public virtual void DeleteItemCategory(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from ItemCategory entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteItemCategory(IList<ItemCategory> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (ItemCategory entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteItemCategory(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
