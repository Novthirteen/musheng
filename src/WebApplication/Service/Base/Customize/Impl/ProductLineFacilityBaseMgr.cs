using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.Customize;
using com.Sconit.Persistence.Customize;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize.Impl
{
    [Transactional]
    public class ProductLineFacilityBaseMgr : SessionBase, IProductLineFacilityBaseMgr
    {
        public IProductLineFacilityDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateProductLineFacility(ProductLineFacility entity)
        {
            entityDao.CreateProductLineFacility(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual ProductLineFacility LoadProductLineFacility(Int32 id)
        {
            return entityDao.LoadProductLineFacility(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ProductLineFacility> GetAllProductLineFacility()
        {
            return entityDao.GetAllProductLineFacility(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ProductLineFacility> GetAllProductLineFacility(bool includeInactive)
        {
            return entityDao.GetAllProductLineFacility(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateProductLineFacility(ProductLineFacility entity)
        {
            entityDao.UpdateProductLineFacility(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProductLineFacility(Int32 id)
        {
            entityDao.DeleteProductLineFacility(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProductLineFacility(ProductLineFacility entity)
        {
            entityDao.DeleteProductLineFacility(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProductLineFacility(IList<Int32> pkList)
        {
            entityDao.DeleteProductLineFacility(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProductLineFacility(IList<ProductLineFacility> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteProductLineFacility(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
