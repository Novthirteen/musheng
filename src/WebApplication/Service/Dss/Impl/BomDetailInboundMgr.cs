using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity;
using Castle.Services.Transaction;
using com.Sconit.Entity.Dss;

namespace com.Sconit.Service.Dss.Impl
{
    public class BomDetailInboundMgr : AbstractInboundMgr
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.DssInbound");

        public IItemMgrE itemMgrE { get; set; }
        public IUomMgrE uomMgrE { get; set; }
        public IBomMgrE bomMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IBomDetailMgrE bomDetailMgrE { get; set; }

        private string[] fields = new string[] 
            { 
                "RateQty",
                "StructureType",
                "EndDate",
                "ScrapPercentage",
                "Operation"
            };

        public BomDetailInboundMgr(IDssImportHistoryMgrE dssImportHistoryMgrE)
            : base(dssImportHistoryMgrE)
        {
            
        }
        protected override object DeserializeForDelete(DssImportHistory dssImportHistory)
        {
            return this.Deserialize(dssImportHistory, false);
        }

        protected override object DeserializeForCreate(DssImportHistory dssImportHistory)
        {
            return this.Deserialize(dssImportHistory, true);
        }

        private object Deserialize(DssImportHistory dssImportHistory, bool isUpdate)
        {
            BomDetail bomDetail = new BomDetail();
            bomDetail.Bom = this.LoadBom(dssImportHistory[1]);//Bom代码
            bomDetail.Item = this.itemMgrE.CheckAndLoadItem(dssImportHistory[2]);//零件号
            bomDetail.Reference = dssImportHistory[3];//参考
            bomDetail.StartDate = dssImportHistory[4] != null ?
                DssHelper.GetDate(dssImportHistory[4], BusinessConstants.DSS_SYSTEM_CODE_QAD) : DateTime.Now;

            if (isUpdate)
            {
                bomDetail.RateQty = decimal.Parse(dssImportHistory[5]);//用量
                bomDetail.StructureType = this.GetStructureType(dssImportHistory[6], BusinessConstants.DSS_SYSTEM_CODE_QAD);//类型
                //dssImportHistory[7];//生效日期
                if (dssImportHistory[8] != null) bomDetail.EndDate = DssHelper.GetDate(dssImportHistory[8], BusinessConstants.DSS_SYSTEM_CODE_QAD);//结束日期
                //dssImportHistory[9];//备注
                bomDetail.ScrapPercentage = decimal.Parse(dssImportHistory[10]) / 100;//废品百分比
                //dssImportHistory[11];//提前期冲销
                bomDetail.Operation = int.Parse(dssImportHistory[12]);
                //dssImportHistory[13];//序列号
                //dssImportHistory[14];//预测百分比
                //dssImportHistory[15];//分组
                //dssImportHistory[16];//处理
            }

            #region 默认值
            bomDetail.Uom = bomDetail.Item.Uom;
            bomDetail.Priority = 0;
            bomDetail.NeedPrint = true;
            bomDetail.IsShipScanHu = false;
            bomDetail.BackFlushMethod = BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_GOODS_RECEIVE;
            #endregion

            return bomDetail;
        }

        protected override void CreateOrUpdateObject(object obj)
        {
            BomDetail bomDetail = (BomDetail)obj;

            BomDetail newBomDetail = this.bomDetailMgrE.LoadBomDetail(bomDetail.Bom.Code, bomDetail.Item.Code, bomDetail.Reference, DateTime.Now);
            if (newBomDetail == null)
            {
                this.bomDetailMgrE.CreateBomDetail(bomDetail);
                log.Debug("Create BomDetail:" + bomDetail.Bom.Code + "," + bomDetail.Item.Code + "," + bomDetail.RateQty.ToString("0.########"));
            }
            else
            {
                CloneHelper.CopyProperty(newBomDetail, bomDetail, this.fields);
                this.bomDetailMgrE.UpdateBomDetail(newBomDetail);
                log.Debug("Update BomDetail:" + bomDetail.Bom.Code + "," + bomDetail.Item.Code + "," + bomDetail.RateQty.ToString("0.########"));
            }
        }

        protected override void DeleteObject(object obj)
        {
            BomDetail bomDetail = (BomDetail)obj;
            BomDetail newBomDetail = this.bomDetailMgrE.LoadBomDetail(bomDetail.Bom.Code, bomDetail.Item.Code, bomDetail.Reference, bomDetail.StartDate);
            if (newBomDetail != null)
            {
                newBomDetail.EndDate = DateTime.Today.AddDays(-1);
                this.bomDetailMgrE.UpdateBomDetail(newBomDetail);
                log.Debug("Update BomDetail to inactive");
                // this.bomDetailMgrE.DeleteBomDetail(newBomDetail.Id);
            }
        }

        #region Private Method
        private string GetStructureType(string type, string externalSystemCode)
        {
            string result = BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE_VALUE_N;
            if (externalSystemCode == BusinessConstants.DSS_SYSTEM_CODE_QAD)
            {
                if (type.Trim().ToUpper() == BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE_VALUE_X)
                {
                    result = BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE_VALUE_X;
                }
                else if (type.Trim().ToUpper() == BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE_VALUE_O)
                {
                    result = BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE_VALUE_O;
                }
            }

            return result;
        }

        [Transaction(TransactionMode.Requires)]
        private Bom LoadBom(string code)
        {
            Bom bom = this.bomMgrE.LoadBom(code);
            if (bom == null)
            {
                bom = new Bom();
                bom.Code = code;
                Item item = this.itemMgrE.LoadItem(code);
                if (item != null)
                {
                    bom.Description = item.Description;
                    bom.Uom = item.Uom;
                    bom.IsActive = true;
                }
                this.bomMgrE.CreateBom(bom);
            }
            return bom;
        }

        #endregion
    }
}




#region Extend Class

namespace com.Sconit.Service.Ext.Dss.Impl
{
    public partial class BomDetailInboundMgrE : com.Sconit.Service.Dss.Impl.BomDetailInboundMgr, IInboundMgrE
    {
        public BomDetailInboundMgrE(IDssImportHistoryMgrE dssImportHistoryMgrE)
            : base(dssImportHistoryMgrE)
        {
            
        }
    }
}

#endregion
