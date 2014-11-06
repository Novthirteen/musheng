using com.Sconit.Service.Ext.MasterData;
using System;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IInspectOrderDetailMgr : IInspectOrderDetailBaseMgr
    {
        #region Customized Methods

        IList<InspectOrderDetail> GetInspectOrderDetail(string inspectOrderNo);

        IList<InspectOrderDetail> GetInspectOrderDetail(InspectOrder inspectOrder);

        IList<InspectOrderDetail> ConvertTransformerToInspectDetail(IList<Transformer> transformerList);

        IList<InspectOrderDetail> ConvertTransformerToInspectDetail(IList<Transformer> transformerList, bool includeZero);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IInspectOrderDetailMgrE : com.Sconit.Service.MasterData.IInspectOrderDetailMgr
    {

    }
}

#endregion
