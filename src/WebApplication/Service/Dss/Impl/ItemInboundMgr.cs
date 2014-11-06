using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity;
using com.Sconit.Entity.Dss;

namespace com.Sconit.Service.Dss.Impl
{
    public class ItemInboundMgr : AbstractInboundMgr
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.DssInbound");

        
        public IItemMgrE itemMgrE { get; set; }
        public IUomMgrE uomMgrE { get; set; }
        public IBomMgrE bomMgrE { get; set; }
        public IRoutingMgrE routingMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }

        private string[] Item2ItemFields = new string[] 
            { 
                "Uom",
                "Desc1",
                "Desc2",
                "Type",
                "Bom",
                "Routing"
            };

		public ItemInboundMgr(
            IDssImportHistoryMgrE dssImportHistoryMgrE)
            : base(dssImportHistoryMgrE)
        {
            
        }


        protected override void FillDssImportHistory(string[] lineData, DssImportHistory dssImportHistory)
        {
            if (lineData != null && lineData.Length > 0 && dssImportHistory != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (lineData[i] == "?")
                        lineData[i] = null;
                    else
                        dssImportHistory[i] = lineData[i];
                }

                dssImportHistory[5] = lineData[55];//P/M类型

                dssImportHistory[6] = lineData[74];//BOM
                dssImportHistory[7] = lineData[75];//Routing

                if (dssImportHistory[2] != null)
                    dssImportHistory[2] = dssImportHistory[2].ToUpper();//单位
            }
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
            Item item = new Item();
            item.Code = dssImportHistory[1]; //零件号
            if (isUpdate)
            {
                item.Uom = this.uomMgrE.CheckAndLoadUom(dssImportHistory[2].ToUpper());//单位
                item.Desc1 = dssImportHistory[3];//描述1
                item.Desc2 = dssImportHistory[4];//描述2
                item.Type = this.GetItemType(dssImportHistory[5], BusinessConstants.DSS_SYSTEM_CODE_QAD);//P/M类型
                if (dssImportHistory[6] != null && dssImportHistory[6].Trim() != string.Empty)
                {
                    item.Bom = this.bomMgrE.CheckAndLoadBom(dssImportHistory[6]); //BOM
                }
                if (dssImportHistory[7] != null && dssImportHistory[7].Trim() != string.Empty)
                {
                    item.Routing = this.routingMgrE.CheckAndLoadRouting(dssImportHistory[7]);//Routing
                }
            }

            #region 默认值
            item.UnitCount = 1;
            item.LastModifyUser = this.userMgrE.GetMonitorUser().Code;
            item.LastModifyDate = DateTime.Now;
            #endregion

            return item;
        }

        protected override void CreateOrUpdateObject(object obj)
        {
            Item item = (Item)obj;

            Item newItem = this.itemMgrE.LoadItem(item.Code);
            if (newItem == null)
            {
                item.IsActive = true;
                this.itemMgrE.CreateItem(item);
            }
            else
            {
                CloneHelper.CopyProperty(item, newItem, this.Item2ItemFields);
                this.itemMgrE.UpdateItem(newItem);
            }
        }

        protected override void DeleteObject(object obj)
        {
            Item item = (Item)obj;

            Item newItem = this.itemMgrE.LoadItem(item.Code);
            if (newItem != null)
            {
                newItem.IsActive = false;
                newItem.LastModifyUser = this.userMgrE.GetMonitorUser().Code;
                newItem.LastModifyDate = DateTime.Now;
                this.itemMgrE.UpdateItem(newItem);
            }
        }

        #region Private Method
        private string GetItemType(string type, string externalSystemCode)
        {
            string result = BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_M;
            if (StringHelper.Eq(externalSystemCode, BusinessConstants.DSS_SYSTEM_CODE_QAD))
            {
                if (StringHelper.Eq(type, BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_P))
                {
                    result = BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_P;
                }
            }

            return result;
        }
        #endregion
    }
}





#region Extend Interface
namespace com.Sconit.Service.Ext.Dss.Impl
{
    public partial class ItemInboundMgrE : com.Sconit.Service.Dss.Impl.ItemInboundMgr, IInboundMgrE
    {
        public ItemInboundMgrE(
            IDssImportHistoryMgrE dssImportHistoryMgrE)
            : base(dssImportHistoryMgrE)
        {
            
        }
    }
}

#endregion
