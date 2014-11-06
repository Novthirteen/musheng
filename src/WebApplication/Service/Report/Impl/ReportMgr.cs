using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity;
using Castle.Services.Transaction;
using NPOI.HSSF.UserModel;
using com.Sconit.Entity.Exception;
using System.Collections;
using com.Sconit.Utility;



namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class ReportMgr : IReportMgr
    {
        public IDictionary<string, string> dicReportService { get; set; }

        public IReportBaseMgrE GetIReportBaseMgr(String template, IList<object> list)
        {
            IReportBaseMgrE reportMgrE = this.GetImplService(template);

            if (reportMgrE != null)
            {
                reportMgrE.FillValues(template, list);
            }

            return reportMgrE;

        }

        public IReportBaseMgrE GetImplService(String template)
        {
            if (template == null || dicReportService == null || !dicReportService.ContainsKey(template)
                || dicReportService[template] == null || dicReportService[template].Length == 0)
            {
                throw new BusinessErrorException("Common.Business.Error.EntityNotExist", template);
            }

            return ServiceLocator.GetService<IReportBaseMgrE>(dicReportService[template]);
        }


        public string WriteToFile(String template, string entityId)
        {
            IReportBaseMgrE iReportMgrE = this.GetImplService(template);
            iReportMgrE.FillValues(template, entityId);
            return this.WriteToFile(iReportMgrE.GetWorkbook());
        }

        public string WriteToFile(String template, IList<object> list)
        {
            IReportBaseMgrE iReportMgrE = GetIReportBaseMgr(template, list);
            return this.WriteToFile(iReportMgrE.GetWorkbook());
        }

        public string WriteToFile(String template, IList<object> list, String fileName)
        {
            IReportBaseMgrE iReportMgrE = GetIReportBaseMgr(template, list);
            return this.WriteToFile(fileName, iReportMgrE.GetWorkbook());
        }

        public void WriteToClient(String template, string entityId, String fileName)
        {
            IReportBaseMgrE iReportMgrE = this.GetImplService(template);
            iReportMgrE.FillValues(template, entityId);
            this.WriteToClient(fileName, iReportMgrE.GetWorkbook());
        }

        public void WriteToClient(String template, IList<object> list, String fileName)
        {
            IReportBaseMgrE iReportMgrE = GetIReportBaseMgr(template, list);
            this.WriteToClient(fileName, iReportMgrE.GetWorkbook());
        }

        public void WriteToClient(String fileName, HSSFWorkbook workbook)
        {
            XlsHelper.WriteToClient(fileName, workbook);
        }

        public string WriteToFile(HSSFWorkbook workbook)
        {
            return XlsHelper.WriteToFile(workbook);
        }
        public string WriteToFile(String fileName, HSSFWorkbook workbook)
        {
            return XlsHelper.WriteToFile(fileName, workbook);
        }


    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    public partial class ReportMgrE : com.Sconit.Service.Report.Impl.ReportMgr, IReportMgrE
    {

    }
}

#endregion
