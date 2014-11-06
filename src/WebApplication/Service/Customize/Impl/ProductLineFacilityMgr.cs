using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Customize;
using com.Sconit.Entity.Customize;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.Exception;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize.Impl
{
    [Transactional]
    public class ProductLineFacilityMgr : ProductLineFacilityBaseMgr, IProductLineFacilityMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }
        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public bool HasProductLineFacility(string productionLineCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ProductLineFacility>();

            criteria.SetProjection(Projections.ProjectionList().Add(Projections.RowCount()));

            criteria.Add(Expression.Eq("ProductLine", productionLineCode));
            criteria.Add(Expression.Eq("IsActive", true));

            return (this.criteriaMgr.FindAll<int>(criteria)[0] > 0);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ProductLineFacility> GetProductLineFacility(string productionLineCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ProductLineFacility>();

            criteria.Add(Expression.Eq("ProductLine", productionLineCode));
            criteria.Add(Expression.Eq("IsActive", true));

            return this.criteriaMgr.FindAll<ProductLineFacility>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public ProductLineFacility CheckAndLoadProductLineFacility(string productLineFacilityCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ProductLineFacility>();

            criteria.Add(Expression.Eq("Code", productLineFacilityCode));

            IList<ProductLineFacility> productLineFacilityList = this.criteriaMgr.FindAll<ProductLineFacility>(criteria);

            if (productLineFacilityList == null && productLineFacilityList.Count == 0)
            {
                throw new BusinessErrorException("MasterDate.ProductLineFacility.ProductLineFacilityCodeNotExist", productLineFacilityCode);
            }

            return productLineFacilityList[0];
        }


        [Transaction(TransactionMode.Unspecified)]
        public ProductLineFacility GetPLFacility(string productLineFacilityCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ProductLineFacility>();

            criteria.Add(Expression.Eq("Code", productLineFacilityCode));

            IList<ProductLineFacility> productLineFacilityList = this.criteriaMgr.FindAll<ProductLineFacility>(criteria);

            if (productLineFacilityList == null || productLineFacilityList.Count > 0)
            {
                return productLineFacilityList[0];
            }
            return null;
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Customize.Impl
{
    [Transactional]
    public partial class ProductLineFacilityMgrE : com.Sconit.Service.Customize.Impl.ProductLineFacilityMgr, IProductLineFacilityMgrE
    {
    }
}

#endregion Extend Class