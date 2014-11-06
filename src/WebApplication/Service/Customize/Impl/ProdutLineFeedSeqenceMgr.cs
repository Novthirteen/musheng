using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Customize;
using NHibernate.Expression;
using com.Sconit.Entity.Customize;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Entity.Production;
using com.Sconit.Entity.Exception;
using System.IO;
using com.Sconit.Entity.MasterData;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using System.Linq;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize.Impl
{
    [Transactional]
    public class ProdutLineFeedSeqenceMgr : ProdutLineFeedSeqenceBaseMgr, IProdutLineFeedSeqenceMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }
        public IFlowMgrE flowMgr { get; set; }
        public IItemMgrE itemMgr { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<ProdutLineFeedSeqence> GetProdutLineFeedSeqence(string productLineFacility, string fgCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ProdutLineFeedSeqence>();

            criteria.Add(Expression.Eq("ProductLineFacility", productLineFacility));
            criteria.Add(Expression.Eq("FinishGood.Code", fgCode));
            criteria.Add(Expression.Eq("IsActive", true));

            criteria.AddOrder(Order.Asc("Sequence"));

            return this.criteriaMgr.FindAll<ProdutLineFeedSeqence>(criteria);
        }

        //[Transaction(TransactionMode.Unspecified)]
        //public IList<ProdutLineFeedSeqence> GetProdutLineFeedSeqence(string productLineFacility, string fgCode, string seq)
        //{
        //    return this.GetProdutLineFeedSeqence(productLineFacility, fgCode, Int32.Parse(seq));
        //}

        [Transaction(TransactionMode.Unspecified)]
        public IList<ProdutLineFeedSeqence> GetProdutLineFeedSeqence(string productLineFacility, string fgCode, int seq)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ProdutLineFeedSeqence>();

            criteria.Add(Expression.Eq("ProductLineFacility", productLineFacility));
            criteria.Add(Expression.Eq("FinishGood.Code", fgCode));
            criteria.Add(Expression.Eq("Sequence", seq));

            criteria.AddOrder(Order.Asc("Sequence"));

            return this.criteriaMgr.FindAll<ProdutLineFeedSeqence>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ProdutLineFeedSeqence> GetProdutLineFeedSeqence(string productLineFacility, string fgCode, string code)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ProdutLineFeedSeqence>();

            criteria.Add(Expression.Eq("ProductLineFacility", productLineFacility));
            criteria.Add(Expression.Eq("FinishGood.Code", fgCode));
            criteria.Add(Expression.Eq("Code", code));

            criteria.AddOrder(Order.Asc("Sequence"));

            return this.criteriaMgr.FindAll<ProdutLineFeedSeqence>(criteria);
        }

        public IList<ProdutLineFeedSeqence> GetActualProdutLineFeedSeqence(string woNo)
        {
            IList<ProdutLineFeedSeqence> returnProdutLineFeedSeqenceList = new List<ProdutLineFeedSeqence>();

            DetachedCriteria criteria = DetachedCriteria.For<OrderLocationTransaction>();

            criteria.CreateAlias("OrderDetail", "od");
            criteria.CreateAlias("od.OrderHead", "oh");

            criteria.Add(Expression.Eq("oh.OrderNo", woNo));
            criteria.Add(Expression.Eq("IOType", BusinessConstants.IO_TYPE_OUT));

            IList<OrderLocationTransaction> orderLocaitonTransactionList = this.criteriaMgr.FindAll<OrderLocationTransaction>(criteria);

            if (orderLocaitonTransactionList != null && orderLocaitonTransactionList.Count > 0)
            {
                OrderDetail orderDetail = orderLocaitonTransactionList[0].OrderDetail;
                OrderHead orderHead = orderDetail.OrderHead;

                if (orderHead.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
                {
                    throw new BusinessErrorException("Order.Production.Error.CantCreateFeedSequenceFowWOStatusError", woNo, BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS);
                }

                criteria = DetachedCriteria.For<ProdutLineFeedSeqence>();

                criteria.Add(Expression.Eq("ProductLineFacility", orderHead.ProductLineFacility));
                criteria.Add(Expression.Eq("FinishGood", orderDetail.Item));
                criteria.Add(Expression.Eq("IsActive", true));

                criteria.AddOrder(Order.Asc("Sequence"));

                IList<ProdutLineFeedSeqence> produtLineFeedSeqenceList = this.criteriaMgr.FindAll<ProdutLineFeedSeqence>(criteria);

                if (produtLineFeedSeqenceList != null && produtLineFeedSeqenceList.Count > 0)
                {
                    foreach (ProdutLineFeedSeqence produtLineFeedSeqence in produtLineFeedSeqenceList)
                    {
                        var find = (from trans in orderLocaitonTransactionList
                                    where trans.Item.Code == produtLineFeedSeqence.RawMaterial.Code
                                    select trans).SingleOrDefault();

                        if (find == null)
                        {
                            #region 查找替代物料
                            criteria = DetachedCriteria.For<ItemDiscontinue>();

                            criteria.Add(Expression.Eq("Item", produtLineFeedSeqence.RawMaterial));
                            criteria.Add(Expression.Le("StartDate", orderHead.StartTime));
                            criteria.Add(Expression.Or(Expression.IsNull("EndDate"), Expression.Ge("EndDate", orderHead.StartTime)));

                            IList<ItemDiscontinue> disConItems = this.criteriaMgr.FindAll<ItemDiscontinue>(criteria);
                            if (disConItems != null && disConItems.Count > 0)
                            {
                                var findDisCon = (from trans in orderLocaitonTransactionList
                                                  join disCon in disConItems on trans.Item.Code equals disCon.DiscontinueItem.Code
                                                  orderby disCon.Priority ascending
                                                  select disCon).FirstOrDefault();

                                if (findDisCon != null)
                                {
                                    ProdutLineFeedSeqence dicConProdutLineFeedSeqence = new ProdutLineFeedSeqence();
                                    CloneHelper.CopyProperty(produtLineFeedSeqence, dicConProdutLineFeedSeqence);
                                    dicConProdutLineFeedSeqence.RawMaterial = findDisCon.DiscontinueItem;
                                    returnProdutLineFeedSeqenceList.Add(dicConProdutLineFeedSeqence);
                                }
                                else
                                {
                                    throw new BusinessErrorException("Order.Production.Error.NotFindRMInWo", woNo, produtLineFeedSeqence.RawMaterial.Code);
                                }
                            }
                            else
                            {
                                throw new BusinessErrorException("Order.Production.Error.NotFindRMInWo", woNo, produtLineFeedSeqence.RawMaterial.Code);
                            }
                            #endregion
                        }
                        else
                        {
                            returnProdutLineFeedSeqenceList.Add(produtLineFeedSeqence);
                        }
                    }
                }
                else
                {
                    throw new BusinessErrorException("Order.Production.Error.FeedSequenceNotFind", woNo);
                }
            }
            else
            {
                throw new BusinessErrorException("Order.Production.Error.WONotFind", woNo);
            }

            return returnProdutLineFeedSeqenceList;
        }

        public int ReadFromXls(Stream inputStream, User user)
        {
            if (inputStream.Length == 0)
                throw new BusinessErrorException("Import.Stream.Empty");

            IList<ProdutLineFeedSeqence> produtLineFeedSeqenceList = new List<ProdutLineFeedSeqence>();
            HSSFWorkbook workbook = new HSSFWorkbook(inputStream);

            Sheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            ImportHelper.JumpRows(rows, 1);

            #region 列定义

            int colFacility = 0;//生产线设备
            int colCode = 1;//位号
            int colSeq = 2;//seq
            int colFinishGood = 3;//成品代码
            int colRawMaterial = 4;//原材料代码

            #endregion

            DateTime now = DateTime.Now;
            int count = 0;
            while (rows.MoveNext())
            {

                Row row = (HSSFRow)rows.Current;
                if (!this.CheckValidDataRow(row, 0, 4))
                {
                    break;//边界
                }

                string prodLineFactCode = string.Empty;
                string finishGoodCode = string.Empty;
                int seq = 0;
                string rawMaterialCode = string.Empty;
                string code = string.Empty;


                #region 读取生产线设备
                try
                {
                    prodLineFactCode = GetCellStringValue(row.GetCell(colFacility));
                    if (prodLineFactCode == string.Empty)
                        throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Empty.Error.Facility", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Read.Error.Facility", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 读取位号
                try
                {
                    code = GetCellStringValue(row.GetCell(colCode));
                    if (code == string.Empty)
                        throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Empty.Error.Code", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Read.Error.Code", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 读取成品代码
                try
                {
                    finishGoodCode = GetCellStringValue(row.GetCell(colFinishGood));
                    if (finishGoodCode == string.Empty)
                        throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Empty.Error.FinishGood", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Read.Error.FinishGood", (row.RowNum + 1).ToString());
                }
                #endregion


                #region 读取序号
                try
                {
                    seq = (int)(row.GetCell(colSeq).NumericCellValue);
                }
                catch
                {
                    //continue;
                    throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Read.Error.Seq", (row.RowNum + 1).ToString());
                }
                #endregion


                #region 读取原材料代码
                try
                {
                    rawMaterialCode = GetCellStringValue(row.GetCell(colRawMaterial));
                    if (rawMaterialCode == string.Empty)
                        throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Empty.Error.RawMaterial", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Read.Error.RawMaterial", (row.RowNum + 1).ToString());
                }
                #endregion

                ProdutLineFeedSeqence produtLineFeedSeqence = new ProdutLineFeedSeqence();

                #region 验证设备号是否存在
                try
                {

                    DetachedCriteria criteria = DetachedCriteria.For<ProductLineFacility>();

                    criteria.Add(Expression.Eq("Code", prodLineFactCode));
                    criteria.SetProjection(Projections.ProjectionList().Add(Projections.RowCount()));

                    IList<int> rowCount = this.criteriaMgr.FindAll<int>(criteria);
                    if (rowCount == null || rowCount[0] == null || ((int)rowCount[0]) == 0)
                    {
                        throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Database.Error.ProductLineFact", (row.RowNum + 1).ToString());
                    }
                    produtLineFeedSeqence.ProductLineFacility = prodLineFactCode;
                }
                catch
                {
                    throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Read.Error.ProductLineFact", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 验证成品是否存在
                try
                {

                    Item finishGood = itemMgr.LoadItem(finishGoodCode);
                    if (finishGood == null)
                    {
                        throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Database.Error.FinishGood", (row.RowNum + 1).ToString());
                    }
                    produtLineFeedSeqence.FinishGood = finishGood;
                }
                catch
                {
                    throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Database.Error.FinishGood", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 验证原材料是否存在
                try
                {

                    Item rawMaterial = itemMgr.LoadItem(rawMaterialCode);
                    if (rawMaterial == null)
                    {
                        throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Database.Error.RawMaterial", (row.RowNum + 1).ToString());
                    }
                    produtLineFeedSeqence.RawMaterial = rawMaterial;
                }
                catch
                {
                    throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Import.Database.Error.RawMaterial", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 验证序号是否存在
                try
                {
                    IList<ProdutLineFeedSeqence> plfsList = this.GetProdutLineFeedSeqence(prodLineFactCode, finishGoodCode, seq);
                    if (plfsList == null || plfsList.Count > 0)
                    {
                        throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Sequence.Exists", (row.RowNum + 1).ToString());
                    }
                    produtLineFeedSeqence.Sequence = seq;
                }
                catch
                {
                    throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Sequence.Exists", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 验证位号是否存在
                try
                {
                    IList<ProdutLineFeedSeqence> plfsList = this.GetProdutLineFeedSeqence(prodLineFactCode, finishGoodCode, code);
                    if (plfsList == null || plfsList.Count > 0)
                    {
                        throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Code.Exists", (row.RowNum + 1).ToString());
                    }
                    produtLineFeedSeqence.Code = code;
                }
                catch
                {
                    throw new BusinessErrorException("Production.ProdutLineFeedSeqence.Code.Exists", (row.RowNum + 1).ToString());
                }
                #endregion


                produtLineFeedSeqence.IsActive = true;
                produtLineFeedSeqence.CreateUser = user.Code;
                produtLineFeedSeqence.CreateDate = now;
                produtLineFeedSeqence.LastModifyUser = user.Code;
                produtLineFeedSeqence.LastModifyDate = now;

                //produtLineFeedSeqenceList.Add(produtLineFeedSeqence);
                count++;
                this.CreateProdutLineFeedSeqence(produtLineFeedSeqence);
            }

            if (count == 0)
                throw new BusinessErrorException("Import.Result.Error.ImportNothing");

            return count;
            //return produtLineFeedSeqenceList;

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

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Customize.Impl
{
    [Transactional]
    public partial class ProdutLineFeedSeqenceMgrE : com.Sconit.Service.Customize.Impl.ProdutLineFeedSeqenceMgr, IProdutLineFeedSeqenceMgrE
    {
    }
}

#endregion Extend Class