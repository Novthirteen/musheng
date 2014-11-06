using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.Customize;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.Customize.NH
{
    public class NHProductLineFacilityBaseDao : NHDaoBase, IProductLineFacilityBaseDao
    {
        public NHProductLineFacilityBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateProductLineFacility(ProductLineFacility entity)
        {
            Create(entity);
        }

        public virtual IList<ProductLineFacility> GetAllProductLineFacility()
        {
            return GetAllProductLineFacility(false);
        }

        public virtual IList<ProductLineFacility> GetAllProductLineFacility(bool includeInactive)
        {
            string hql = @"from ProductLineFacility entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<ProductLineFacility> result = FindAllWithCustomQuery<ProductLineFacility>(hql);
            return result;
        }

        public virtual ProductLineFacility LoadProductLineFacility(Int32 id)
        {
            return FindById<ProductLineFacility>(id);
        }

        public virtual void UpdateProductLineFacility(ProductLineFacility entity)
        {
            Update(entity);
        }

        public virtual void DeleteProductLineFacility(Int32 id)
        {
            string hql = @"from ProductLineFacility entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteProductLineFacility(ProductLineFacility entity)
        {
            Delete(entity);
        }

        public virtual void DeleteProductLineFacility(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from ProductLineFacility entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteProductLineFacility(IList<ProductLineFacility> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (ProductLineFacility entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteProductLineFacility(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
