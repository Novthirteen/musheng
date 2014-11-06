using System;
using System.Collections.Generic;
using com.Sconit.Entity.Customize;
using com.Sconit.Entity.Production;
using System.IO;
using com.Sconit.Entity.MasterData; 

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize
{
    public interface IProdutLineFeedSeqenceMgr : IProdutLineFeedSeqenceBaseMgr
    {
        #region Customized Methods

        IList<ProdutLineFeedSeqence> GetProdutLineFeedSeqence(string productLineFacility, string fgCode);

        //IList<ProdutLineFeedSeqence> GetProdutLineFeedSeqence(string productLineFacility, string fgCode, string seq);

        IList<ProdutLineFeedSeqence> GetProdutLineFeedSeqence(string productLineFacility, string fgCode, int seq);

        IList<ProdutLineFeedSeqence> GetProdutLineFeedSeqence(string productLineFacility, string fgCode, string code);

        int ReadFromXls(Stream inputStream, User user);

        IList<ProdutLineFeedSeqence> GetActualProdutLineFeedSeqence(string woNo);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Customize
{
    public partial interface IProdutLineFeedSeqenceMgrE : com.Sconit.Service.Customize.IProdutLineFeedSeqenceMgr
    {
    }
}

#endregion Extend Interface