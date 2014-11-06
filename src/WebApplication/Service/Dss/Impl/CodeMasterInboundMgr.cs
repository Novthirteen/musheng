using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.Dss;

namespace com.Sconit.Service.Dss.Impl
{
    public class CodeMasterInboundMgr : AbstractInboundMgr
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.DssInbound");

        
        public IUomMgrE uomMgrE { get; set; }

        private string[] uomFields = new string[] 
            {
                "Name",
                "Description"
            };

        public CodeMasterInboundMgr(IDssImportHistoryMgrE dssImportHistoryMgrE)
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
            string key = dssImportHistory[1]; //代码

            //计量单位
            if (key.Trim().ToUpper() == "PT_UM")
            {
                Uom uom = new Uom();
                uom.Code = dssImportHistory[2];//计量单位
                if (isUpdate)
                {
                    uom.Name = dssImportHistory[3];//名称
                    uom.Description = dssImportHistory[3];//描述
                }
                return uom;
            }
            else
            {
                throw new BusinessErrorException("Common.Business.Error.EntityNotExist", key);
            }
        }

        protected override void CreateOrUpdateObject(object obj)
        {
            Uom uom = (Uom)obj;

            Uom newUom = this.uomMgrE.LoadUom(uom.Code);
            if (newUom == null)
            {
                this.uomMgrE.CreateUom(uom);
            }
            else
            {
                CloneHelper.CopyProperty(uom, newUom, this.uomFields);
                this.uomMgrE.UpdateUom(newUom);
            }
        }

        protected override void DeleteObject(object obj)
        {
            Uom uom = (Uom)obj;

            Uom newUom = this.uomMgrE.LoadUom(uom.Code);
            if (newUom != null)
            {
                this.uomMgrE.DeleteUom(newUom.Code);
            }
        }
    }
}




#region Extend Class
namespace com.Sconit.Service.Ext.Dss.Impl
{
    public partial class CodeMasterInboundMgrE : com.Sconit.Service.Dss.Impl.CodeMasterInboundMgr, IInboundMgrE
    {
        public CodeMasterInboundMgrE(IDssImportHistoryMgrE dssImportHistoryMgrE)
            : base(dssImportHistoryMgrE)
        {
           
        }
    }
}

#endregion
