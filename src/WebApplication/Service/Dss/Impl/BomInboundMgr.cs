using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity.Dss;

namespace com.Sconit.Service.Dss.Impl
{
    public class BomInboundMgr : AbstractInboundMgr
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.DssInbound");

        
        public IItemMgrE itemMgrE { get; set; }
        public IUomMgrE uomMgrE { get; set; }
        public IBomMgrE bomMgrE { get; set; }

        private string[] fields = new string[] 
            {
                "Description",
                "Uom"
            };

        public BomInboundMgr(IDssImportHistoryMgrE dssImportHistoryMgrE)
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
            Bom bom = new Bom();
            bom.Code = dssImportHistory[1]; //Bom代码
            if (isUpdate)
            {
                bom.Description = dssImportHistory[2]; //说明
                bom.Uom = this.uomMgrE.CheckAndLoadUom(dssImportHistory[3]);//单位
            }

            return bom;
        }

        protected override void CreateOrUpdateObject(object obj)
        {
            Bom bom = (Bom)obj;

            Bom newBom = this.bomMgrE.LoadBom(bom.Code);
            if (newBom == null)
            {
                bom.IsActive = true;
                this.bomMgrE.CreateBom(bom);
            }
            else
            {
                CloneHelper.CopyProperty(bom, newBom, this.fields);
                this.bomMgrE.UpdateBom(newBom);
            }
        }

        protected override void DeleteObject(object obj)
        {
            Bom bom = (Bom)obj;

            Bom newBom = this.bomMgrE.LoadBom(bom.Code);
            if (newBom != null)
            {
                newBom.IsActive = false;
                this.bomMgrE.UpdateBom(newBom);
            }
        }
    }
}




#region Extend Class

namespace com.Sconit.Service.Ext.Dss.Impl
{
    public partial class BomInboundMgrE : com.Sconit.Service.Dss.Impl.BomInboundMgr, IInboundMgrE
    {
        public BomInboundMgrE(IDssImportHistoryMgrE dssImportHistoryMgrE)
            : base(dssImportHistoryMgrE)
        {
            
        }
    }
}

#endregion
