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
    public class NHItemBrandBaseDao : NHDaoBase, IItemBrandBaseDao
    {
        public NHItemBrandBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateItemBrand(ItemBrand entity)
        {
            Create(entity);
        }

        public virtual IList<ItemBrand> GetAllItemBrand()
        {
            return FindAll<ItemBrand>();
        }

        public virtual ItemBrand LoadItemBrand(String code)
        {
            return FindById<ItemBrand>(code);
        }

        public virtual void UpdateItemBrand(ItemBrand entity)
        {
            Update(entity);
        }

        public virtual void DeleteItemBrand(String code)
        {
            string hql = @"from ItemBrand entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteItemBrand(ItemBrand entity)
        {
            Delete(entity);
        }

        public virtual void DeleteItemBrand(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from ItemBrand entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteItemBrand(IList<ItemBrand> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (ItemBrand entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteItemBrand(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
