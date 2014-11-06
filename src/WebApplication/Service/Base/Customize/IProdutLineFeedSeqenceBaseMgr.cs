using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Customize;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize
{
    public interface IProdutLineFeedSeqenceBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateProdutLineFeedSeqence(ProdutLineFeedSeqence entity);

        ProdutLineFeedSeqence LoadProdutLineFeedSeqence(Int32 id);

        IList<ProdutLineFeedSeqence> GetAllProdutLineFeedSeqence();
    
        IList<ProdutLineFeedSeqence> GetAllProdutLineFeedSeqence(bool includeInactive);
      
        void UpdateProdutLineFeedSeqence(ProdutLineFeedSeqence entity);

        void DeleteProdutLineFeedSeqence(Int32 id);
    
        void DeleteProdutLineFeedSeqence(ProdutLineFeedSeqence entity);
    
        void DeleteProdutLineFeedSeqence(IList<Int32> pkList);
    
        void DeleteProdutLineFeedSeqence(IList<ProdutLineFeedSeqence> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
