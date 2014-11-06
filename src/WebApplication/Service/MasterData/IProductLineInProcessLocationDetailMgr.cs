using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.Production;
using com.Sconit.Entity.Customize;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IProductLineInProcessLocationDetailMgr : IProductLineInProcessLocationDetailBaseMgr
    {
        #region Customized Methods

        IList<ProductLineInProcessLocationDetail> GetProductLineInProcessLocationDetail(string prodLineCode, string prodLineFacilityCode, string orderNo, string status, string[] items);

        decimal? GetPLIpQty(string huId);

        void RawMaterialIn(string prodLineCode, IList<MaterialIn> materialInList, User user);

        void RawMaterialIn(Flow prodLine, IList<MaterialIn> materialInList, User user);

        void RawMaterialIn4Order(string orderNo, IDictionary<string, string> seqHuIdDic, User user);

        void RawMaterialIn4Order(OrderHead orderHead, IDictionary<string, string> seqHuIdDic, User user);

        void RawMaterialBackflush(string prodLineCode, User user);

        void RawMaterialBackflush(Flow prodLine, User user);

        void RawMaterialBackflush(string prodLineCode, IDictionary<string, decimal> itemQtydic, User user);

        void RawMaterialBackflush(Flow prodLine, IDictionary<string, decimal> itemQtydic, User user);

        IList<ProductLineInProcessLocationDetail> GetProductLineInProcessLocationDetailGroupByItem(string prodLineCode, string status);

        void BackflushRawMaterial(string prodLineCode, Item item, ref decimal qty, OrderLocationTransaction orderLocationTransaction, string ipNo, User user);

        void RawMaterialReturnByProductLine(string prodLineCode, User user);

        void RawMaterialReturnByProductLineFacility(string prodLineFacilityCode, User user);

        void RawMaterialReturnByOrderNo(string orderNo, User user);

        void RawMaterialReturnByProductLine(string prodLineCode, IDictionary<string, decimal> huQty, User user);

        void RawMaterialReturnByProductLineFacility(string prodLineFacilityCode, IDictionary<string, decimal> huQty, User user);

        void RawMaterialReturnByOrderNo(string orderNo, IDictionary<string, decimal> huQty, User user);

        void RawMaterialReturnByHuIdQty(IDictionary<string, decimal> huQty, User user);

        void RawMaterialReturnByHuId(IList<string> returnHu, User user);

        ProdutLineFeedSeqence CheckAndGetNextProdutLineFeedSeqence(string orderNo, string position, string huId);

        void ExchangeProdutLineFeed(string exchangeHuIdFrom, string exchangeHuIdTo, User user);

        void ExchangeProdutLineFeed(string exchangeHuIdFrom, string exchangeHuIdTo, User user, bool checkSortAndColor);

        #endregion Customized Methods
    }
}

#region Extend Interface






namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IProductLineInProcessLocationDetailMgrE : com.Sconit.Service.MasterData.IProductLineInProcessLocationDetailMgr
    {
        
    }
}

#endregion
