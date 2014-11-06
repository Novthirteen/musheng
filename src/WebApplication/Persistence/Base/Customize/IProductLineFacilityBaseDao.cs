using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Customize;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Customize
{
    public interface IProductLineFacilityBaseDao
    {
        #region Method Created By CodeSmith

        void CreateProductLineFacility(ProductLineFacility entity);

        ProductLineFacility LoadProductLineFacility(Int32 id);
  
        IList<ProductLineFacility> GetAllProductLineFacility();
  
        IList<ProductLineFacility> GetAllProductLineFacility(bool includeInactive);
  
        void UpdateProductLineFacility(ProductLineFacility entity);

        void DeleteProductLineFacility(Int32 id);
    
        void DeleteProductLineFacility(ProductLineFacility entity);

        void DeleteProductLineFacility(IList<Int32> pkList);
    
        void DeleteProductLineFacility(IList<ProductLineFacility> entityList);    
        #endregion Method Created By CodeSmith
    }
}
