using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;

namespace com.Sconit.Service.Report
{
    public interface IReportMgr
    {
        IReportBaseMgrE GetIReportBaseMgr(String template, IList<object> list);
        string WriteToFile(String template, string orderNo);
        string WriteToFile(String template, IList<object> list);
        string WriteToFile(String template, IList<object> list, String fileName);
        string WriteToFile(HSSFWorkbook workbook);
        string WriteToFile(String fileName, HSSFWorkbook workbook);


        void WriteToClient(String template, IList<object> list, String fileName);
        void WriteToClient(String fileName, HSSFWorkbook workbook);
        void WriteToClient(String template, string entityId, String fileName);
    }
}


namespace com.Sconit.Service.Ext.Report
{
    public partial interface IReportMgrE : com.Sconit.Service.Report.IReportMgr
    {
       
    }
}

