using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Customize;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Customize
{
    public interface IProdLineIp2BaseDao
    {
        #region Method Created By CodeSmith

        void CreateProdLineIp2(ProdLineIp2 entity);

        ProdLineIp2 LoadProdLineIp2(Int32 id);
  
        IList<ProdLineIp2> GetAllProdLineIp2();
  
        void UpdateProdLineIp2(ProdLineIp2 entity);
        
        void DeleteProdLineIp2(Int32 id);
    
        void DeleteProdLineIp2(ProdLineIp2 entity);
    
        void DeleteProdLineIp2(IList<Int32> pkList);
    
        void DeleteProdLineIp2(IList<ProdLineIp2> entityList);    
        #endregion Method Created By CodeSmith
    }
}
