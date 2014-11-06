using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.View;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using NHibernate.Expression;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using com.Sconit.Entity.MRP;
using com.Sconit.Service.Ext.MRP;

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ImportMgr : IImportMgr
    {
        #region 变量
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IShiftMgrE shiftMgrE { get; set; }
        public IFlowDetailMgrE flowDetailMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
        public IUomMgrE uomMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IStorageBinMgrE storageBinMgrE { get; set; }
        public IItemKitMgrE itemKitMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public IBomMgrE bomMgrE { get; set; }
        //public IBomDetailMgrE bomDetailMgrE { get; set; }
        public ICustomerMgrE customerMgrE { get; set; }
        public IItemReferenceMgrE itemReferenceMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public ICustomerScheduleMgrE customerScheduleMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderloctransMgrE { get; set; }
        #endregion

        #region IImportMgr接口实现
        [Transaction(TransactionMode.Unspecified)]
        public IList<ShiftPlanSchedule> ReadPSModelFromXls(Stream inputStream, User user, string regionCode, string flowCode, DateTime date, string shiftCode)
        {
            IList<ShiftPlanSchedule> spsList = new List<ShiftPlanSchedule>();
            IList<Shift> shifts = shiftMgrE.GetAllShift();

            //IList<FlowDetail> flowDetails = flowDetailMgrE.GetFlowDetail(flowCode, true);

            var shift = shifts.Where(s => StringHelper.Eq(s.Code, shiftCode));

            if (inputStream.Length == 0)
                throw new BusinessErrorException("Import.Stream.Empty");

            //if (shift == null)
            //    throw new BusinessErrorException("Import.PSModel.ShiftNotExist");

            HSSFWorkbook workbook = new HSSFWorkbook(inputStream);

            Sheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            ImportHelper.JumpRows(rows, 4);

            //if (colIndex < 0)
            //    throw new BusinessErrorException("Import.PSModel.Shift.Not.Exist", shift.ShiftName);

            Row shiftRow = (HSSFRow)rows.Current;
            ImportHelper.JumpRows(rows, 1);

            #region 列定义
            int colSeq = 0;//seq
            int colFlow = 1;//生产线
            int colItem = 2;//物料代码
            int colBom = 4;//Bom
            #endregion

            while (rows.MoveNext())
            {
                Row row = (HSSFRow)rows.Current;
                if (!this.CheckValidDataRow(row, 0, 4))
                {
                    break;//边界
                }

                string fCode = string.Empty;
                string itemCode = string.Empty;
                string bomstr = string.Empty;
                string remark = string.Empty;

                Bom bom = null;
                int seq = 0;
                decimal planQty = 0;
                Cell cell = null;

                #region 读取生产线
                fCode = row.GetCell(colFlow).StringCellValue;
                if (fCode.Trim() == string.Empty)
                    throw new BusinessErrorException("Import.PSModel.Empty.Error.Flow", (row.RowNum + 1).ToString());

                if (flowCode != null && flowCode.Trim() != string.Empty)
                {
                    if (fCode.Trim().ToUpper() != flowCode.Trim().ToUpper())
                        continue;//生产线过滤
                }
                #endregion

                #region 读取bom
                try
                {
                    bomstr = GetCellStringValue(row.GetCell(colBom));
                    if (bomstr != null && bomstr != "0")
                    {
                        bom = bomMgrE.CheckAndLoadBom(bomstr);
                    }
                }
                catch (Exception ex)
                {
                    throw new BusinessErrorException("Bom.Error.BomCodeNotExist", bomstr);
                }
                #endregion

                #region 读取序号
                try
                {
                    //string seqStr = row.GetCell(colSeq).StringCellValue;
                    //seq = row.GetCell(colSeq).StringCellValue.Trim() != string.Empty ? int.Parse(row.GetCell(colSeq).StringCellValue) : 0;
                    seq = (int)(row.GetCell(colSeq).NumericCellValue);
                }
                catch
                {
                    continue;
                    //throw new BusinessErrorException("Import.PSModel.Read.Error.Seq", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 读取成品代码
                try
                {
                    itemCode = GetCellStringValue(row.GetCell(colItem));
                    if (itemCode == string.Empty)
                        throw new BusinessErrorException("Import.PSModel.Empty.Error.ItemCode", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.ItemCode", (row.RowNum + 1).ToString());
                }
                #endregion

                FlowDetail flowDetail = flowDetailMgrE.LoadFlowDetail(fCode, itemCode, seq);

                //var flowDetails_ = flowDetails.Where(f => StringHelper.Eq(f.Item.Code, itemCode));
                //if (flowDetails_.Count() > 1)
                //{
                //    var flowDetails_seq = flowDetails_.Where(f => f.Sequence == seq);
                //    if (flowDetails_seq.Count() > 0)
                //    {
                //        flowDetails_ = flowDetails_seq;
                //    }
                //}
                //FlowDetail flowDetail = flowDetails_.FirstOrDefault();

                if (flowDetail == null)
                    throw new BusinessErrorException("Import.PSModel.FlowDetail.Not.Exist", (row.RowNum + 1).ToString());

                //区域权限过滤
                if (regionCode != null && regionCode.Trim() != string.Empty)
                {
                    if (regionCode.Trim().ToUpper() != flowDetail.Flow.PartyTo.Code.ToUpper())
                        continue;
                }
                if (!user.HasPermission(flowDetail.Flow.PartyTo.Code))
                    continue;

                if (shift.Count() == 0)
                {
                    int startColIndex = 5; //从第5列开始

                    int dayOfWeek = (int)date.DayOfWeek;
                    if (dayOfWeek == 0)
                        dayOfWeek = 7;

                    startColIndex = startColIndex + (dayOfWeek - 1) * 6;
                    int endColIndex = startColIndex + 6;
                    for (int i = startColIndex; i < endColIndex; i = i + 2)
                    {
                        string shiftName = this.GetCellStringValue(shiftRow.GetCell(i));

                        var shifts_ = shifts.Where(s => StringHelper.Eq(s.ShiftName, shiftName));

                        if (shifts_.Count() == 1)
                        {
                            planQty = Convert.ToDecimal(row.GetCell(i).NumericCellValue);

                            //if (planQty <= 0)
                            //{
                            //    continue;
                            //}
                            remark = GetCellStringValue(row.GetCell(i + 1));

                            ShiftPlanSchedule sps = new ShiftPlanSchedule();
                            sps.FlowDetail = flowDetail;
                            sps.ReqDate = date;
                            sps.Shift = shifts_.SingleOrDefault();
                            sps.PlanQty = planQty;
                            sps.LastModifyUser = user;
                            sps.LastModifyDate = DateTime.Now;
                            sps.Bom = bom;
                            sps.Remark = remark;
                            spsList.Add(sps);
                        }
                        else if (shifts_.Count() > 1)
                        {
                            throw new BusinessErrorException("找到重复的班次");
                        }
                    }

                }
                else
                {
                    #region 读取计划量
                    try
                    {
                        int colIndex = this.GetPlanColumnIndexToRead(shiftRow, shift.SingleOrDefault().ShiftName, date);
                        cell = row.GetCell(colIndex);
                        if (cell == null || cell.CellType == NPOI.SS.UserModel.CellType.BLANK)
                            continue;

                        planQty = Convert.ToDecimal(row.GetCell(colIndex).NumericCellValue);
                        //if (planQty <= 0)
                        //{
                        //    continue;
                        //}
                        remark = GetCellStringValue(row.GetCell(colIndex + 1));
                    }
                    catch
                    {
                        throw new BusinessErrorException("Import.PSModel.Read.Error.PlanQty", (row.RowNum + 1).ToString());
                    }
                    #endregion

                    ShiftPlanSchedule sps = new ShiftPlanSchedule();
                    sps.FlowDetail = flowDetail;
                    sps.ReqDate = date;
                    sps.Shift = shift.SingleOrDefault();
                    sps.PlanQty = planQty;
                    sps.LastModifyUser = user;
                    sps.LastModifyDate = DateTime.Now;
                    sps.Bom = bom;
                    sps.Remark = remark;
                    spsList.Add(sps);
                }
            }

            if (spsList.Count == 0)
                throw new BusinessErrorException("Import.Result.Error.ImportNothing");

            spsList = spsList.Where(s => s.PlanQty > 0).ToList();
            return spsList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<FlowPlan> ReadShipScheduleYFKFromXls(Stream inputStream, User user, string planType, string flowCode, string timePeriodType, DateTime date)
        {
            IList<FlowPlan> flowPlanList = new List<FlowPlan>();
            if (inputStream.Length == 0)
                throw new BusinessErrorException("Import.Stream.Empty");

            HSSFWorkbook workbook = new HSSFWorkbook(inputStream);

            Sheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            ImportHelper.JumpRows(rows, 8);
            int colIndex = this.GetColumnIndexToRead_ShipScheduleYFK((HSSFRow)rows.Current, date);

            if (colIndex < 0)
                throw new BusinessErrorException("Import.MRP.DateNotExist", date.ToShortDateString());

            #region 列定义
            int colFlow = 1;//Flow
            int colUC = 6;//单包装
            #endregion

            while (rows.MoveNext())
            {
                Row row = (HSSFRow)rows.Current;
                if (!this.CheckValidDataRow(row, 1, 6))
                {
                    break;//边界
                }

                //string regCode=row.GetCell(
                string itemCode = string.Empty;
                decimal UC = 1;
                decimal planQty = 0;
                string flowCodeCell = string.Empty;

                #region 读取Flow
                try
                {
                    if (row.GetCell(colFlow) == null)
                    {
                        continue;
                    }
                    flowCodeCell = row.GetCell(colFlow).StringCellValue;
                    if (flowCodeCell.Trim() == string.Empty)
                    {
                        continue;
                    }
                    else if ((flowCode == null || flowCode.Trim() == string.Empty) || flowCodeCell == flowCode)
                    {

                    }
                    else
                    {
                        continue;
                    }
                }
                catch
                {
                    this.ThrowCommonError(row, colIndex);
                }
                #endregion

                #region 读取成品代码
                try
                {
                    itemCode = GetCellStringValue(row.GetCell(4));
                    if (itemCode == string.Empty)
                        throw new BusinessErrorException("Import.PSModel.Empty.Error.ItemCode", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.ItemCode", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 读取单包装
                try
                {
                    UC = Convert.ToDecimal(row.GetCell(colUC).NumericCellValue);
                }
                catch
                {
                    this.ThrowCommonError(row.RowNum, colUC, row.GetCell(colUC));
                }
                #endregion

                #region 读取计划量
                try
                {
                    if (row.GetCell(colIndex) == null)
                    {
                        planQty = 0;
                    }
                    else
                    {
                        planQty = Convert.ToDecimal(row.GetCell(colIndex).NumericCellValue);
                    }
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.PlanQty", (row.RowNum + 1).ToString());
                }
                #endregion

                FlowDetail flowDetail = this.LoadFlowDetailByFlow(flowCodeCell, itemCode, UC);
                if (flowDetail == null)
                    throw new BusinessErrorException("Import.MRP.Distribution.FlowDetail.Not.Exist", (row.RowNum + 1).ToString());


                //if (partyCode != null && partyCode.Trim() != string.Empty)
                //{
                //    if (!StringHelper.Eq(partyCode, flowDetail.Flow.PartyTo.Code))
                //    {
                //        continue;//客户过滤
                //    }
                //}
                //区域权限过滤
                if (!user.HasPermission(flowDetail.Flow.PartyFrom.Code) && !user.HasPermission(flowDetail.Flow.PartyTo.Code))
                {
                    continue;
                }

                FlowPlan flowPlan = new FlowPlan();
                flowPlan.FlowDetail = flowDetail;
                flowPlan.TimePeriodType = timePeriodType;
                flowPlan.ReqDate = date;
                flowPlan.PlanQty = planQty;
                flowPlanList.Add(flowPlan);
            }

            if (flowPlanList.Count == 0)
                throw new BusinessErrorException("Import.Result.Error.ImportNothing");

            return flowPlanList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<FlowPlan> ReadShipScheduleCSFromXls(Stream inputStream, User user, string planType, string flowCode, string timePeriodType, DateTime date)
        {
            IList<FlowPlan> flowPlanList = new List<FlowPlan>();
            if (inputStream.Length == 0)
                throw new BusinessErrorException("Import.Stream.Empty");

            HSSFWorkbook workbook = new HSSFWorkbook(inputStream);

            Sheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            ImportHelper.JumpRows(rows, 8);
            int colIndex = this.GetColumnIndexToRead_ShipScheduleYFK((HSSFRow)rows.Current, date);
            int colrefOrderNoIndex = colIndex + 1;

            if (colIndex < 0)
                throw new BusinessErrorException("Import.MRP.DateNotExist", date.ToShortDateString());

            #region 列定义
            int colFlow = 1;//Flow
            int colUC = 6;//单包装
            #endregion
            rows.MoveNext();
            while (rows.MoveNext())
            {
                Row row = (HSSFRow)rows.Current;
                if (!this.CheckValidDataRow(row, 1, 6))
                {
                    break;//边界
                }

                //string regCode=row.GetCell(
                string itemCode = string.Empty;
                decimal UC = 1;
                decimal planQty = 0;
                string flowCodeCell = string.Empty;
                string refOrderNo = null;

                #region 读取Flow
                try
                {
                    if (row.GetCell(colFlow) == null)
                    {
                        continue;
                    }
                    flowCodeCell = row.GetCell(colFlow).StringCellValue;
                    if (flowCodeCell.Trim() == string.Empty)
                    {
                        continue;
                    }
                    else if ((flowCode == null || flowCode.Trim() == string.Empty) || flowCodeCell == flowCode)
                    {

                    }
                    else
                    {
                        continue;
                    }
                }
                catch
                {
                    this.ThrowCommonError(row, colIndex);
                }
                #endregion

                #region 读取成品代码
                try
                {
                    itemCode = GetCellStringValue(row.GetCell(4));
                    if (itemCode == string.Empty)
                        throw new BusinessErrorException("Import.PSModel.Empty.Error.ItemCode", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.ItemCode", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 读取单包装
                try
                {
                    UC = Convert.ToDecimal(row.GetCell(colUC).NumericCellValue);
                }
                catch
                {
                    this.ThrowCommonError(row.RowNum, colUC, row.GetCell(colUC));
                }
                #endregion

                #region 读取计划量
                try
                {
                    if (row.GetCell(colIndex) == null)
                    {
                        planQty = 0;
                    }
                    else
                    {
                        planQty = Convert.ToDecimal(row.GetCell(colIndex).NumericCellValue);
                    }
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.PlanQty", (row.RowNum + 1).ToString());
                }

                if (planQty == 0)
                {
                    continue;
                }
                #endregion

                #region 读取参考订单号
                try
                {
                    refOrderNo = GetCellStringValue(row.GetCell(colrefOrderNoIndex));
                }
                catch
                {
                    //Nothing to do
                }
                #endregion

                FlowDetail flowDetail = this.LoadFlowDetailByFlow(flowCodeCell, itemCode, UC);
                if (flowDetail == null)
                    throw new BusinessErrorException("Import.MRP.Distribution.FlowDetail.Not.Exist", (row.RowNum + 1).ToString());

                if (!user.HasPermission(flowDetail.Flow.PartyFrom.Code) && !user.HasPermission(flowDetail.Flow.PartyTo.Code))
                {
                    continue;
                }

                FlowPlan flowPlan = new FlowPlan();
                flowPlan.FlowDetail = flowDetail;
                flowPlan.TimePeriodType = timePeriodType;
                flowPlan.ReqDate = date;
                flowPlan.PlanQty = planQty;
                flowPlan.Memo = refOrderNo;
                flowPlanList.Add(flowPlan);
            }

            if (flowPlanList.Count == 0)
            {
                throw new BusinessErrorException("Import.Result.Error.ImportNothing");
            }
            return flowPlanList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<FlowPlan> ReadScheduleFromXls(Stream inputStream, User user, string moduleType, string flowCode, string timePeriodType, DateTime date)
        {
            IList<FlowPlan> flowPlanList = new List<FlowPlan>();
            if (inputStream.Length == 0)
                throw new BusinessErrorException("Import.Stream.Empty");

            HSSFWorkbook workbook = new HSSFWorkbook(inputStream);

            Sheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            ImportHelper.JumpRows(rows, 9);

            #region 列定义
            int colFlow = 1;//Flow
            int colItemCode = 2;//物料代码
            int colQty = 3;//计划数量
            int colDate = 4;//发运时间
            int colUC = 5;//单包装
            int colMemo = 6;//备注
            #endregion

            while (rows.MoveNext())
            {
                FlowPlan flowPlan = new FlowPlan();
                flowPlan.TimePeriodType = timePeriodType;

                Row row = (HSSFRow)rows.Current;
                if (!this.CheckValidDataRow(row, 1, 6))
                {
                    break;//边界
                }

                #region 读取Flow
                try
                {
                    if (row.GetCell(colFlow) == null || row.GetCell(colFlow).StringCellValue.Trim() == string.Empty)
                    {
                        continue;
                    }
                    string rowFlowCode = row.GetCell(colFlow).StringCellValue.Trim();
                    if ((flowCode == null || flowCode.Trim() == string.Empty) || rowFlowCode == flowCode)
                    {
                        flowPlan.FlowCode = rowFlowCode;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch
                {
                    this.ThrowCommonError(row, colFlow);
                }
                #endregion

                #region 读取发运时间
                try
                {
                    if (row.GetCell(colDate) == null)
                    {
                        continue;
                    }
                    DateTime cellValue = row.GetCell(colDate).DateCellValue;
                    if (DateTime.Compare(cellValue.Date, date) == 0)
                    {
                        flowPlan.ReqDate = cellValue;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch
                {
                    this.ThrowCommonError(row.RowNum, colDate, row.GetCell(colDate));
                }
                #endregion

                #region 读取物料代码
                try
                {
                    if (row.GetCell(colItemCode) == null)
                    {
                        throw new BusinessErrorException("Import.PSModel.Empty.Error.ItemCode", (row.RowNum + 1).ToString());
                    }
                    string itemCode = GetCellStringValue(row.GetCell(colItemCode));
                    if (itemCode == string.Empty)
                    {
                        throw new BusinessErrorException("Import.PSModel.Empty.Error.ItemCode", (row.RowNum + 1).ToString());
                    }
                    else
                    {
                        flowPlan.ItemCode = itemCode;
                    }
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.ItemCode", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 读取计划量
                try
                {
                    if (row.GetCell(colQty) == null)
                    {
                        flowPlan.PlanQty = 0;
                    }
                    else
                    {
                        flowPlan.PlanQty = Convert.ToDecimal(row.GetCell(colQty).NumericCellValue);
                    }
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.PlanQty", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 读取单包装
                try
                {
                    if (row.GetCell(colUC) != null)
                    {
                        flowPlan.UC = Convert.ToDecimal(row.GetCell(colUC).NumericCellValue);
                    }
                }
                catch
                {
                    this.ThrowCommonError(row.RowNum, colUC, row.GetCell(colUC));
                }
                #endregion

                #region 读取备注
                try
                {
                    if (row.GetCell(colMemo) != null)
                    {
                        flowPlan.Memo = GetCellStringValue(row.GetCell(colMemo));
                    }
                }
                catch
                {
                    //Nothing to do
                }
                #endregion

                flowPlan.Seq = row.RowNum + 1;

                flowPlanList.Add(flowPlan);
            }

            List<string> flowCodes = flowPlanList.Select(f => f.FlowCode).Distinct().ToList();

            List<FlowDetail> flowDetails = new List<FlowDetail>();

            foreach (string f in flowCodes)
            {
                IList<FlowDetail> flowDetailList = flowDetailMgrE.GetFlowDetail(f, true);
                if (flowDetailList != null && flowDetailList.Count > 0 &&
                    user.HasPermission(flowDetailList[0].Flow.PartyTo.Code) &&
                    user.HasPermission(flowDetailList[0].Flow.PartyFrom.Code))
                {
                    if (flowDetailList[0].Flow.Type != moduleType)
                    {
                        throw new BusinessErrorException("Import.PSModel.FlowType.Error", moduleType);
                    }
                    flowDetails.AddRange(flowDetailList);
                }
            }

            IList<FlowPlan> newflowPlanList = new List<FlowPlan>();
            int seq = 1;
            IList<FlowPlan> notMatchFlowPlans = new List<FlowPlan>();
            foreach (FlowPlan flowPlan in flowPlanList)
            {
                bool mark = false;
                foreach (FlowPlan newfp in newflowPlanList)
                {
                    if (((flowPlan.UC == null || flowPlan.UC == 0) && (flowPlan.FlowCode == newfp.FlowCode && flowPlan.ItemCode == newfp.ItemCode)) ||
                        (flowPlan.FlowCode == newfp.FlowCode && flowPlan.ItemCode == newfp.ItemCode && flowPlan.UC == newfp.UC))
                    {
                        newfp.PlanQty += flowPlan.PlanQty;
                        if (flowPlan.Memo != null && flowPlan.Memo != string.Empty
                            && newfp.Memo != null && newfp.Memo != string.Empty)
                        {
                            newfp.Memo += "+" + flowPlan.Memo;
                        }
                        mark = true;
                        break;
                    }
                }
                if (!mark)
                {
                    foreach (FlowDetail fd in flowDetails)
                    {
                        if ((flowPlan.UC == null || flowPlan.UC == 0) && (flowPlan.FlowCode == fd.Flow.Code && flowPlan.ItemCode == fd.Item.Code) ||
                            (flowPlan.FlowCode == fd.Flow.Code && flowPlan.ItemCode == fd.Item.Code && flowPlan.UC == fd.UnitCount))
                        {
                            flowPlan.FlowDetail = fd;
                            flowPlan.FlowDetail.Sequence = seq;
                            newflowPlanList.Add(flowPlan);
                            seq++;
                            mark = true;
                            break;
                        }
                    }
                }
                if (!mark)
                {
                    notMatchFlowPlans.Add(flowPlan);
                }
            }
            if (notMatchFlowPlans.Count > 0)
            {
                string notMatchItems = string.Empty;
                foreach (FlowPlan notMatchFlowPlan in notMatchFlowPlans)
                {
                    if (notMatchItems == string.Empty)
                    {
                        notMatchItems += "Row" + notMatchFlowPlan.Seq.ToString() + "Item" + notMatchFlowPlan.ItemCode;
                    }
                    else
                    {
                        notMatchItems += "_Row" + notMatchFlowPlan.Seq.ToString() + "Item" + notMatchFlowPlan.ItemCode;
                    }
                }
                throw new BusinessErrorException("Import.PSModel.FlowDetail.NotExist", notMatchItems);
            }

            if (newflowPlanList.Count == 0)
            {
                throw new BusinessErrorException("Import.Result.Error.ImportNothing");
            }

            return newflowPlanList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public CustomerSchedule ReadCustomerScheduleFromXls(Stream inputStream, User user, DateTime? startDate, DateTime? endDate,
            string flowCode, string refScheduleNo, bool isItemRef)
        {
            if (inputStream.Length == 0)
            {
                throw new BusinessErrorException("Import.Stream.Empty");
            }

            HSSFWorkbook workbook = new HSSFWorkbook(inputStream);

            Sheet sheet = workbook.GetSheetAt(0);

            IEnumerator rows = sheet.GetRowEnumerator();

            #region 读取路线,参考日程号等
            if (flowCode == null || flowCode.Trim() == string.Empty)
            {
                throw new BusinessErrorException("MRP.Schedule.Import.CustomerSchedule.Result.SelectFlow");
            }
            Row flowRow = sheet.GetRow(1);
            string xlsFlowCode = GetCellStringValue(flowRow.GetCell(2));
            if (xlsFlowCode == null)
            {
                throw new BusinessErrorException("MRP.Schedule.Import.CustomerSchedule.Result.FillFLowInTemplate");
            }
            else if (!StringHelper.Eq(xlsFlowCode, flowCode))
            {
                throw new BusinessErrorException("MRP.Schedule.Import.CustomerSchedule.Result.SelectedFlowNotMatchTheFlowInTemplate");
            }
            Flow flow = flowMgrE.CheckAndLoadFlow(flowCode, true, true);
            //todo 权限判断

            decimal leadTime = flow.LeadTime.HasValue ? flow.LeadTime.Value : 0M;

            Row refOrderNoRow = sheet.GetRow(2);
            string referenceScheduleNo = GetCellStringValue(refOrderNoRow.GetCell(2));
            if (referenceScheduleNo != null && !StringHelper.Eq(referenceScheduleNo, refScheduleNo))
            {
                throw new BusinessErrorException("MRP.Schedule.Import.CustomerSchedule.Result.RefCustomerScheduleNotMatchThatInTemplate");
            }

            Row typeRow = sheet.GetRow(5);
            Row dateRow = sheet.GetRow(6);

            IList<CustomerSchedule> customerSchedules = customerScheduleMgrE.GetCustomerSchedules(flowCode, refScheduleNo, null, null, null);
            if (customerSchedules.Count > 0)
            {
                throw new BusinessErrorException("MRP.Schedule.Import.CustomerSchedule.Result.CannotImportSameRefCustomerSchedule");
            }

            #endregion

            #region CustomerSchedule
            CustomerSchedule customerSchedule = new CustomerSchedule();
            customerSchedule.ReferenceScheduleNo = refScheduleNo;
            customerSchedule.Flow = flowCode;
            customerSchedule.CustomerScheduleDetails = new List<CustomerScheduleDetail>();
            #endregion

            ImportHelper.JumpRows(rows, 7);

            #region 列定义
            int colItemCode = 0;//物料代码或参考物料号
            int colItemDescription = 1;//物料描述
            int colUom = 2;//单位
            int colUc = 3;//单包装
            #endregion

            while (rows.MoveNext())
            {
                Item item = null;
                Uom uom = null;
                decimal? uc = null;
                string itemReference = null;
                string location = null;

                Row row = (HSSFRow)rows.Current;
                if (!this.CheckValidDataRow(row, 0, 3))
                {
                    break;//边界
                }
                string rowIndex = (row.RowNum + 1).ToString();

                #region 读取物料代码
                try
                {
                    string itemCode = GetCellStringValue(row.GetCell(colItemCode));
                    if (itemCode == null)
                    {
                        throw new BusinessErrorException("Import.ShipSchedule.ItemCode.Empty", rowIndex);
                    }
                    if (isItemRef)
                    {
                        item = itemReferenceMgrE.GetItemReferenceByRefItem(itemCode, flow.PartyTo.Code, flow.PartyFrom.Code);
                        itemReference = itemCode;
                    }
                    else
                    {
                        item = itemMgrE.LoadItem(itemCode);
                        itemReference = itemReferenceMgrE.GetItemReferenceByItem(itemCode, flow.PartyTo.Code, flow.PartyFrom.Code);
                    }
                    if (item == null)
                    {
                        throw new BusinessErrorException("Import.ShipSchedule.Item.NotExist", itemCode, rowIndex);
                    }
                }
                catch
                {
                    throw new BusinessErrorException("Import.ShipSchedule.ItemCode.Error", rowIndex);
                }
                #endregion

                #region 读取单位
                try
                {
                    string uomCode = GetCellStringValue(row.GetCell(colUom));
                    if (uomCode != null)
                    {
                        uom = uomMgrE.CheckAndLoadUom(uomCode);
                    }
                }
                catch
                {
                    this.ThrowCommonError(row, colUom);
                }
                #endregion

                #region 读取单包装
                try
                {
                    string uc_ = GetCellStringValue(row.GetCell(colUc));
                    if (uc_ != null)
                    {
                        uc = Convert.ToDecimal(uc_);
                    }
                }
                catch
                {
                    this.ThrowCommonError(row, colUc);
                }
                #endregion

                #region 使用flowDet过滤
                bool isMatch = false;
                if (flow.FlowDetails != null)
                {
                    var q = flow.FlowDetails.Where(f => StringHelper.Eq(f.Item.Code, item.Code)
                        && uom == null ? true : StringHelper.Eq(f.Uom.Code, uom.Code)
                        && !uc.HasValue ? true : uc.Value == f.UnitCount);
                    if (q.Count() > 0)
                    {
                        uom = q.FirstOrDefault().Uom;
                        uc = q.FirstOrDefault().UnitCount;
                        location = q.FirstOrDefault().DefaultLocationFrom.Code;
                        isMatch = true;
                    }
                }
                if (flow.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
                {
                    Flow refFlow = flowMgrE.CheckAndLoadFlow(flow.ReferenceFlow, true, true);
                    var q = refFlow.FlowDetails.Where(f => StringHelper.Eq(f.Item.Code, item.Code)
                       && uom == null ? true : StringHelper.Eq(f.Uom.Code, uom.Code)
                       && !uc.HasValue ? true : uc.Value == f.UnitCount);
                    if (q.Count() > 0)
                    {
                        uom = q.FirstOrDefault().Uom;
                        uc = q.FirstOrDefault().UnitCount;
                        location = flow.LocationTo.Code;
                        isMatch = true;
                    }
                }
                if (!isMatch)
                {
                    if (flow.AllowCreateDetail)
                    {
                        uc = uc == null ? item.UnitCount : uc;
                        uom = uom == null ? item.Uom : uom;
                        location = flow.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING ? flow.LocationTo.Code : flow.LocationFrom.Code;
                    }
                    else
                    {
                        throw new BusinessErrorException("Import.MRP.Distribution.FlowDetail.Not.Exist", rowIndex);
                    }
                }
                #endregion

                #region 读取数量
                try
                {
                    for (int i = 4; ; i++)
                    {
                        string periodType = GetCellStringValue(typeRow.GetCell(i));

                        if (periodType != BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_DAY &&
                            periodType != BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_MONTH &&
                            periodType != BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_WEEK)
                        {
                            break;
                        }
                        Cell dateCell = dateRow.GetCell(i);
                        DateTime? dateCellValue = null;
                        if (dateCell != null && dateCell.CellType == CellType.NUMERIC)
                        {
                            dateCellValue = dateCell.DateCellValue;
                        }
                        else
                        {
                            break;
                        }
                        if (startDate.HasValue && dateCellValue.Value.Date < startDate.Value.Date)
                        {
                            continue;
                        }
                        if (endDate.HasValue && dateCellValue.Value.Date > endDate.Value.Date)
                        {
                            break;
                        }
                        string qtyValue = GetCellStringValue(row.GetCell(i));
                        decimal qty = 0M;
                        if (qtyValue != null)
                        {
                            qty = Convert.ToDecimal(qtyValue);
                        }
                        if (qty < 0M)
                        {
                            throw new BusinessErrorException("Import.ShipSchedule.Qty.MustGreatThan0", rowIndex);
                        }
                        else
                        {
                            CustomerScheduleDetail customerScheduleDetail = new CustomerScheduleDetail();
                            customerScheduleDetail.DateFrom = DateTimeHelper.GetStartTime(periodType, dateCellValue.Value);
                            customerScheduleDetail.DateTo = DateTimeHelper.GetEndTime(periodType, dateCellValue.Value);
                            customerScheduleDetail.Item = item.Code;
                            customerScheduleDetail.ItemDescription = item.Description;
                            customerScheduleDetail.ItemReference = itemReference;
                            customerScheduleDetail.Location = location;
                            customerScheduleDetail.Type = periodType;
                            customerScheduleDetail.UnitCount = uc.Value;
                            customerScheduleDetail.Uom = uom.Code;
                            customerScheduleDetail.StartTime = customerScheduleDetail.DateFrom.AddHours(-(double)leadTime);
                            customerScheduleDetail.Qty = qty;
                            customerSchedule.CustomerScheduleDetails.Add(customerScheduleDetail);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new BusinessErrorException("Import.ShipSchedule.Qty.Error", rowIndex);
                }
                #endregion
            }
            customerSchedule.CustomerScheduleDetails = customerSchedule.CustomerScheduleDetails.OrderBy(c => c.StartTime).ToList();

            return customerSchedule;
        }


        [Transaction(TransactionMode.Unspecified)]
        public IList<CycleCountDetail> ReadCycleCountFromXls(Stream inputStream, User user, CycleCount cycleCount)
        {
            if (inputStream.Length == 0)
                throw new BusinessErrorException("Import.Stream.Empty");

            //区域权限过滤
            if (!user.HasPermission(cycleCount.Location.Region.Code))
            {
                throw new BusinessErrorException("Common.Business.Error.NoPartyPermission", cycleCount.Location.Region.Code);
            }

            HSSFWorkbook workbook = new HSSFWorkbook(inputStream);

            Sheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            ImportHelper.JumpRows(rows, 11);

            #region 列定义
            int colItem = 1;//物料代码
            int colUom = 3;//单位
            int colQty = 4;//数量
            int colHu = 5;//条码
            int colBin = 6;//库格
            int colStartTime = 7;//开始时间
            int colEndTime = 8;//结束时间
            #endregion

            DateTime dateTimeNow = DateTime.Now;
            IList<CycleCountDetail> cycleCountDetailList = new List<CycleCountDetail>();
            while (rows.MoveNext())
            {
                Row row = (HSSFRow)rows.Current;
                if (!this.CheckValidDataRow(row, 1, 9))
                {
                    break;//边界
                }

                DateTime startTime = cycleCount.StartDate.Value;
                DateTime endTime = dateTimeNow;

                if (row.GetCell(colHu) == null || row.GetCell(colHu).ToString() == string.Empty)
                {
                    string itemCode = string.Empty;
                    decimal qty = 0;
                    string uomCode = string.Empty;

                    #region 读取数据
                    #region 读取物料代码
                    itemCode = GetCellStringValue(row.GetCell(colItem));
                    if (itemCode == null || itemCode.Trim() == string.Empty)
                        this.ThrowCommonError(row.RowNum, colItem, row.GetCell(colItem));

                    var i = (
                        from c in cycleCountDetailList
                        where c.HuId == null && c.Item.Code.Trim().ToUpper() == itemCode.Trim().ToUpper()
                        select c).Count();

                    if (i > 0)
                        throw new BusinessErrorException("Import.Business.Error.Duplicate", itemCode, (row.RowNum + 1).ToString(), (colItem + 1).ToString());
                    #endregion

                    #region 读取数量
                    try
                    {
                        qty = Convert.ToDecimal(row.GetCell(colQty).NumericCellValue);
                    }
                    catch
                    {
                        this.ThrowCommonError(row.RowNum, colQty, row.GetCell(colQty));
                    }
                    #endregion

                    #region 读取单位
                    uomCode = row.GetCell(colUom) != null ? row.GetCell(colUom).StringCellValue : string.Empty;
                    if (uomCode == null || uomCode.Trim() == string.Empty)
                        throw new BusinessErrorException("Import.Read.Error.Empty", (row.RowNum + 1).ToString(), colUom.ToString());
                    #endregion

                    #region 读取开始时间
                    if (row.GetCell(colStartTime) != null)
                    {
                        try
                        {
                            startTime = row.GetCell(colStartTime).DateCellValue;
                        }
                        catch
                        {
                            this.ThrowCommonError(row.RowNum, colStartTime, row.GetCell(colStartTime));
                        }

                        //if (startTime < cycleCount.EffectiveDate)
                        //{
                        //throw new BusinessErrorException("MasterData.Inventory.Stocktaking.Error.StartTimeLtStartDate", itemCode);
                        //}
                    }
                    #endregion

                    #region 读取结束时间
                    if (row.GetCell(colEndTime) != null)
                    {
                        try
                        {
                            endTime = row.GetCell(colEndTime).DateCellValue;
                        }
                        catch
                        {
                            this.ThrowCommonError(row.RowNum, colEndTime, row.GetCell(colEndTime));
                        }

                        if (endTime > dateTimeNow)
                        {
                            throw new BusinessErrorException("MasterData.Inventory.Stocktaking.Error.EndDateGtNow", itemCode);
                        }
                        else if (endTime < startTime)
                        {
                            throw new BusinessErrorException("MasterData.Inventory.Stocktaking.Error.EndDateGtStartTime", itemCode);
                        }
                    }
                    #endregion
                    #endregion

                    #region 填充数据
                    Item item = itemMgrE.CheckAndLoadItem(itemCode);
                    Uom uom = uomMgrE.CheckAndLoadUom(uomCode);
                    //单位换算
                    if (item.Uom.Code.Trim().ToUpper() != uom.Code.Trim().ToUpper())
                    {
                        qty = uomConversionMgrE.ConvertUomQty(item, uom, qty, item.Uom);
                    }

                    #region 套件处理
                    IDictionary<Item, decimal> newItemDic = new Dictionary<Item, decimal>();

                    decimal? convertRate = null;
                    IList<ItemKit> itemKitList = null;
                    if (item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_K)
                    {
                        itemKitList = itemKitMgrE.GetChildItemKit(item);
                        foreach (ItemKit itemKit in itemKitList)
                        {
                            if (!convertRate.HasValue)
                            {
                                if (itemKit.ParentItem.Uom.Code != uom.Code)
                                {
                                    convertRate = uomConversionMgrE.ConvertUomQty(item, uom, 1, itemKit.ParentItem.Uom);
                                }
                                else
                                {
                                    convertRate = 1;
                                }
                            }
                            newItemDic.Add(itemKit.ChildItem, convertRate.Value * qty * itemKit.Qty);

                        }
                    }
                    else
                    {
                        newItemDic.Add(item, qty);
                    }
                    #endregion

                    foreach (KeyValuePair<Item, decimal> entry in newItemDic)
                    {

                        var j = (
                            from c in cycleCountDetailList
                            where c.HuId == null && c.Item.Code.Trim().ToUpper() == entry.Key.Code.Trim().ToUpper()
                            select c).Count();

                        if (j > 0)
                        {
                            foreach (CycleCountDetail detail in cycleCountDetailList)
                            {
                                if (detail.Item.Code == entry.Key.Code)
                                {
                                    detail.Qty += entry.Value;
                                }
                            }
                        }
                        else
                        {
                            CycleCountDetail cycleCountDetail = new CycleCountDetail();
                            cycleCountDetail.CycleCount = cycleCount;
                            cycleCountDetail.Item = entry.Key;
                            cycleCountDetail.Qty = entry.Value;
                            cycleCountDetail.StartTime = startTime;
                            cycleCountDetail.EndTime = endTime;
                            cycleCountDetailList.Add(cycleCountDetail);
                        }
                    }
                    #endregion
                }
                else
                {
                    string huId = string.Empty;
                    string binCode = string.Empty;

                    #region 读取数据
                    #region 读取条码
                    huId = row.GetCell(colHu) != null ? row.GetCell(colHu).StringCellValue : string.Empty;
                    if (huId == null || huId.Trim() == string.Empty)
                        throw new BusinessErrorException("Import.Read.Error.Empty", (row.RowNum + 1).ToString(), colHu.ToString());

                    var i = (
                        from c in cycleCountDetailList
                        where c.HuId != null && c.HuId.Trim().ToUpper() == huId.Trim().ToUpper()
                        select c).Count();

                    if (i > 0)
                        throw new BusinessErrorException("Import.Business.Error.Duplicate", huId, (row.RowNum + 1).ToString(), colHu.ToString());
                    #endregion

                    #region 读取库格
                    binCode = row.GetCell(colBin) != null ? row.GetCell(colBin).StringCellValue : null;
                    #endregion
                    #endregion

                    #region 填充数据
                    Hu hu = huMgrE.CheckAndLoadHu(huId);
                    StorageBin bin = null;
                    if (binCode != null && binCode.Trim() != string.Empty)
                    {
                        bin = storageBinMgrE.CheckAndLoadStorageBin(binCode);
                    }

                    CycleCountDetail cycleCountDetail = new CycleCountDetail();
                    cycleCountDetail.CycleCount = cycleCount;
                    cycleCountDetail.Item = hu.Item;
                    cycleCountDetail.Qty = hu.Qty * hu.UnitQty;
                    cycleCountDetail.HuId = hu.HuId;
                    cycleCountDetail.LotNo = hu.LotNo;
                    cycleCountDetail.StorageBin = bin.Code;
                    cycleCountDetail.StartTime = startTime;
                    cycleCountDetail.EndTime = endTime;
                    cycleCountDetailList.Add(cycleCountDetail);
                    #endregion
                }
            }

            if (cycleCountDetailList.Count == 0)
                throw new BusinessErrorException("Import.Result.Error.ImportNothing");

            return cycleCountDetailList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> ReadOrderLocationTransactionFromXls(Stream inputStream, string orderNo)
        {
            if (inputStream.Length == 0)
            {
                throw new BusinessErrorException("Import.Stream.Empty");

            }

            HSSFWorkbook workbook = new HSSFWorkbook(inputStream);

            Sheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            ImportHelper.JumpRows(rows, 11);

            #region 列定义
            int colItem = 1;//物料代码
            int colUom = 3;//单位
            int colQty = 4;//数量
            #endregion

            IList<OrderLocationTransaction> existOrderLocTransList = orderloctransMgrE.GetOrderLocationTransaction(orderNo, BusinessConstants.IO_TYPE_OUT);
            //if (existOrderLocTransList == null || existOrderLocTransList.Count == 0)
            //{
            //    throw new TechnicalException();
            //}

            IList<OrderLocationTransaction> outOrderLocTransList = orderloctransMgrE.GetOrderLocationTransaction(orderNo, BusinessConstants.IO_TYPE_IN);
            OrderDetail od = outOrderLocTransList[0].OrderDetail;

            IList<OrderLocationTransaction> orderLocTransList = new List<OrderLocationTransaction>();
            while (rows.MoveNext())
            {
                Row row = (HSSFRow)rows.Current;
                if (!this.CheckValidDataRow(row, 1, 5))
                {
                    break;//边界
                }

                string itemCode = string.Empty;
                decimal qty = 0;
                string uomCode = string.Empty;

                #region 读取数据

                #region 读取物料代码
                itemCode = row.GetCell(colItem) != null ? row.GetCell(colItem).StringCellValue : string.Empty;
                if (itemCode == null || itemCode.Trim() == string.Empty)
                    this.ThrowCommonError(row.RowNum, colItem, row.GetCell(colItem));

                var i = (
                    from c in orderLocTransList
                    where c.Item.Code.Trim().ToUpper() == itemCode.Trim().ToUpper()
                    select c).Count();

                if (i == 0)
                {
                    i = (
                    from c in existOrderLocTransList
                    where c.Item.Code.Trim().ToUpper() == itemCode.Trim().ToUpper()
                    select c).Count();
                }

                if (i > 0)
                {
                    throw new BusinessErrorException("Import.Business.Error.Duplicate", itemCode, (row.RowNum + 1).ToString(), (colItem + 1).ToString());
                }
                #endregion

                #region 读取数量
                try
                {
                    qty = Convert.ToDecimal(row.GetCell(colQty).NumericCellValue);
                }
                catch
                {
                    this.ThrowCommonError(row.RowNum, colQty, row.GetCell(colQty));
                }
                #endregion

                #region 读取单位
                uomCode = row.GetCell(colUom) != null ? row.GetCell(colUom).StringCellValue : string.Empty;
                if (uomCode == null || uomCode.Trim() == string.Empty)
                {
                    throw new BusinessErrorException("Import.Read.Error.Empty", (row.RowNum + 1).ToString(), colUom.ToString());
                }
                #endregion

                #endregion

                #region 填充数据
                Item item = itemMgrE.CheckAndLoadItem(itemCode);
                Uom uom = uomMgrE.CheckAndLoadUom(uomCode);
                //单位换算
                if (item.Uom.Code.Trim().ToUpper() != uom.Code.Trim().ToUpper())
                {
                    qty = uomConversionMgrE.ConvertUomQty(item, uom, qty, item.Uom);
                }

                OrderLocationTransaction orderLocTrans = new OrderLocationTransaction();
                orderLocTrans.OrderDetail = od;
                orderLocTrans.Location = od.DefaultLocationFrom;
                orderLocTrans.Item = item;
                orderLocTrans.RawItem = item;
                orderLocTrans.Uom = uom;
                orderLocTrans.OrderedQty = qty;
                orderLocTrans.UnitQty = 1;
                orderLocTrans.Operation = 10;
                orderLocTrans.IsAssemble = true;
                orderLocTrans.IOType = BusinessConstants.IO_TYPE_OUT;
                orderLocTrans.IsBlank = false;
                orderLocTrans.TransactionType = BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_WO;

                orderLocTransList.Add(orderLocTrans);
                #endregion


            }
            return orderLocTransList;
        }



        #endregion

        #region Private Method
        private int GetPlanColumnIndexToRead(Row row, string shiftName, DateTime date)
        {
            int colIndex = -1;
            int startColIndex = 5; //从第5列开始

            int dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 0)
                dayOfWeek = 7;

            startColIndex = startColIndex + (dayOfWeek - 1) * 6;
            for (int i = startColIndex; i < row.LastCellNum; i = i + 2)
            {
                Cell cell = row.GetCell(i);
                string cellValue = cell.StringCellValue;
                if (cellValue == shiftName)
                {
                    colIndex = i;
                    break;
                }
            }

            return colIndex;
        }

        private int GetColumnIndexToRead_ShipScheduleYFK(Row row, DateTime date)
        {
            int colIndex = -1;
            int startColIndex = 7; //从第7列开始

            for (int i = startColIndex; i < row.LastCellNum; i++)
            {
                Cell cell = row.GetCell(i);
                if (cell != null && cell.CellType == CellType.NUMERIC)
                {
                    DateTime cellValue = DateTime.Now;
                    try
                    {
                        cellValue = cell.DateCellValue;
                    }
                    catch (Exception e)
                    {
                        throw new BusinessErrorException("Import.Error");
                    }
                    if (DateTime.Compare(cellValue, date) == 0)
                    {
                        colIndex = i;
                        break;
                    }
                }
            }
            return colIndex;
        }

        private int GetColumnIndexFromShipSchedule(Row row, DateTime date, int startColIndex)
        {
            int colIndex = -1;
            //int startColIndex = 7; //从第7列开始

            for (int i = startColIndex; i < row.LastCellNum; i++)
            {
                Cell cell = row.GetCell(i);
                if (cell != null && cell.CellType == CellType.NUMERIC)
                {
                    DateTime cellValue = DateTime.Now;
                    try
                    {
                        cellValue = cell.DateCellValue;
                    }
                    catch (Exception e)
                    {
                        throw new BusinessErrorException("Import.Error");
                    }
                    if (DateTime.Compare(cellValue, date) == 0)
                    {
                        colIndex = i;
                        break;
                    }
                }
            }
            return colIndex;
        }


        private bool CheckValidDataRow(Row row, int startColIndex, int endColIndex)
        {
            for (int i = startColIndex; i < endColIndex; i++)
            {
                Cell cell = row.GetCell(i);
                if (cell != null && cell.CellType != NPOI.SS.UserModel.CellType.BLANK)
                {
                    return true;
                }
            }

            return false;
        }

        private void ThrowCommonError(Row row, int colIndex)
        {
            this.ThrowCommonError(row.RowNum, colIndex, row.GetCell(colIndex));
        }
        private void ThrowCommonError(int rowIndex, int colIndex, Cell cell)
        {
            string errorValue = string.Empty;
            if (cell != null)
            {
                if (cell.CellType == NPOI.SS.UserModel.CellType.STRING)
                {
                    errorValue = cell.StringCellValue;
                }
                else if (cell.CellType == NPOI.SS.UserModel.CellType.NUMERIC)
                {
                    errorValue = cell.NumericCellValue.ToString("0.########");
                }
                else if (cell.CellType == NPOI.SS.UserModel.CellType.BOOLEAN)
                {
                    errorValue = cell.NumericCellValue.ToString();
                }
                else if (cell.CellType == NPOI.SS.UserModel.CellType.BLANK)
                {
                    errorValue = "Null";
                }
                else
                {
                    errorValue = "Unknow value";
                }
            }
            throw new BusinessErrorException("Import.Read.CommonError", (rowIndex + 1).ToString(), (colIndex + 1).ToString(), errorValue);
        }

        private string GetCellStringValue(Cell cell)
        {
            string strValue = null;
            if (cell != null)
            {
                if (cell.CellType == CellType.STRING)
                {
                    strValue = cell.StringCellValue;
                }
                else if (cell.CellType == CellType.NUMERIC)
                {
                    strValue = cell.NumericCellValue.ToString("0.########");
                }
                else if (cell.CellType == CellType.BOOLEAN)
                {
                    strValue = cell.NumericCellValue.ToString();
                }
                else if (cell.CellType == CellType.FORMULA)
                {
                    if (cell.CachedFormulaResultType == CellType.STRING)
                    {
                        strValue = cell.StringCellValue;
                    }
                    else if (cell.CachedFormulaResultType == CellType.NUMERIC)
                    {
                        strValue = cell.NumericCellValue.ToString("0.########");
                    }
                    else if (cell.CachedFormulaResultType == CellType.BOOLEAN)
                    {
                        strValue = cell.NumericCellValue.ToString();
                    }
                }
            }
            if (strValue != null)
            {
                strValue = strValue.Trim();
            }
            strValue = strValue == string.Empty ? null : strValue;
            return strValue;
        }

        [Transaction(TransactionMode.Unspecified)]
        private FlowDetail LoadFlowDetailByFlow(string flowCode, string itemCode, decimal UC)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(FlowView));
            criteria.CreateAlias("FlowDetail", "fd");
            criteria.Add(Expression.Eq("Flow.Code", flowCode));
            criteria.Add(Expression.Eq("fd.Item.Code", itemCode));
            IList<FlowView> flowViewList = criteriaMgrE.FindAll<FlowView>(criteria);

            FlowDetail flowDetail = null;
            if (flowViewList != null && flowViewList.Count > 0)
            {
                var q1 = flowViewList.Where(f => f.FlowDetail.UnitCount == UC).Select(f => f.FlowDetail);
                if (q1.Count() > 0)
                {
                    flowDetail = q1.First();
                }
                else
                {
                    flowDetail = flowViewList[0].FlowDetail;
                }
            }

            return flowDetail;
        }

        #endregion
    }
}

#region Extend Interface
namespace com.Sconit.Service.Ext.MasterData.Impl
{
    public partial class ImportMgrE : com.Sconit.Service.MasterData.Impl.ImportMgr, IImportMgrE
    {

    }
}

#endregion

#region Extend Interface
namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ImportMgrE : com.Sconit.Service.MasterData.Impl.ImportMgr, IImportMgrE
    {

    }
}
#endregion
