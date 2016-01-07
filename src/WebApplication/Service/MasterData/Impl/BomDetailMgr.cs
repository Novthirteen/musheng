using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using NHibernate.Expression;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using com.Sconit.Service.Ext;
using System.Data.SqlClient;
using System.Linq;
using com.Sconit.Entity.Cost;
using com.Sconit.Service.Ext.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class BomDetailMgr : BomDetailBaseMgr, IBomDetailMgr
    {
        #region ����
        public ICriteriaMgrE criterialMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public IBomMgrE bomMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
        public IRoutingMgrE routingMgrE { get; set; }
        public IRoutingDetailMgrE routingDetailMgrE { get; set; }
        public IUomMgrE uomMgrE { get; set; }
        public ISqlHelperMgrE sqlHelperMgrE { get; set; }
        public IBomTreeMgrE bomTreeMgrE { get; set; }
        //public ILocationMgrE locationMgrE { get; set; }
        private string[] BomCompDetail = new string[] { 
            "Item", 
            "Operation", 
            "Reference", 
            "StructureType", 
            "StartDate", 
            "EndDate", 
            "Uom", 
            "RateQty", 
            "ScrapPercentage", 
            "NeedPrint", 
            "Priority", 
            "Location", 
            "IsShipScanHu", 
            "HuLotSize", 
            "OptionalItemGroup",
            "CalculatedQty",
            "BomLevel"
        };
        #endregion

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetNextLevelBomDetail(string bomCode, DateTime efftiveDate)
        {
            //NullableDateTime nullableEffDate = new NullableDateTime(efftiveDate);

            DetachedCriteria detachedCriteria = DetachedCriteria.For<BomDetail>();
            detachedCriteria.Add(Expression.Eq("Bom.Code", bomCode));
            detachedCriteria.Add(Expression.Le("StartDate", efftiveDate));
            detachedCriteria.Add(Expression.Or(Expression.Ge("EndDate", efftiveDate), Expression.IsNull("EndDate")));

            IList<BomDetail> bomDetailList = criterialMgrE.FindAll<BomDetail>(detachedCriteria);

            return this.GetNoOverloadBomDetail(bomDetailList);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetFlatBomDetail(string bomCode, DateTime efftiveDate)
        {
            IList<BomDetail> flatBomDetailList = new List<BomDetail>();
            IList<BomDetail> nextBomDetailList = this.GetNextLevelBomDetail(bomCode, efftiveDate);

            if (nextBomDetailList != null && nextBomDetailList.Count > 0)
            {
                foreach (BomDetail nextBomDetail in nextBomDetailList)
                {
                    nextBomDetail.CalculatedQty = nextBomDetail.RateQty * (1 + nextBomDetail.DefaultScrapPercentage);
                    nextBomDetail.CalculatedQtyWithoutScrapRate = nextBomDetail.RateQty;
                    ProcessCurrentBomDetail(flatBomDetailList, nextBomDetail, efftiveDate);
                }
            }
            return flatBomDetailList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public bool CheckUniqueExist(string parCode, string compCode, int Operation, string Reference, DateTime startTime)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(BomDetail));
            criteria.Add(Expression.Eq("Bom.Code", parCode));
            criteria.Add(Expression.Eq("Item.Code", compCode));
            criteria.Add(Expression.Eq("Operation", Operation));
            criteria.Add(Expression.Eq("Reference", Reference));
            criteria.Add(Expression.Ge("StartDate", startTime));

            IList bomDetails = criterialMgrE.FindAll(criteria);
            if (bomDetails.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetTreeBomDetail(string bomCode, DateTime effDate)
        {
            IList<BomDetail> treeBomDetailList = new List<BomDetail>();
            IList<BomDetail> nextBomDetailList = this.GetNextLevelBomDetail(bomCode, effDate);
            if (nextBomDetailList != null && nextBomDetailList.Count > 0)
            {
                foreach (BomDetail nextBomDetail in nextBomDetailList)
                {
                    nextBomDetail.CalculatedQty = nextBomDetail.RateQty * (1 + nextBomDetail.DefaultScrapPercentage);
                    nextBomDetail.CalculatedQtyWithoutScrapRate = nextBomDetail.RateQty;
                    nextBomDetail.AccumQty = nextBomDetail.CalculatedQty;
                    nextBomDetail.BomLevel = 1;
                }
                treeBomDetailList = this.GetAllBomDetailTree(nextBomDetailList, effDate);
            }

            return this.GetNoOverloadBomDetail(treeBomDetailList);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetTopLevelBomDetail(string itemCode, DateTime effDate)
        {
            IList<BomDetail> returnBomDetailList = new List<BomDetail>();
            //IList<BomDetail> bomDetailList = new List<BomDetail>();
            //IList<BomDetail> lastLevelBomDetailList = new List<BomDetail>();
            //lastLevelBomDetailList = this.GetLastLevelBomDetail(itemCode, effDate);
            //if (lastLevelBomDetailList != null && lastLevelBomDetailList.Count > 0)
            //{
            //    foreach (BomDetail bomDetail in lastLevelBomDetailList)
            //    {
            //        bomDetailList = this.GetTopLevelBomDetail(bomDetail.Bom.Code, effDate);
            //        if (lastLevelBomDetailList != null && lastLevelBomDetailList.Count > 0)
            //        {
            //            foreach (BomDetail bd in bomDetailList)
            //            {
            //                returnBomDetailList.Add(bd);
            //            }
            //        }
            //        else
            //        {
            //            returnBomDetailList.Add(bomDetail);
            //        }
            //    }
            //}

            return returnBomDetailList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetLastLevelBomDetail(string itemCode, DateTime effDate)
        {
            //NullableDateTime nullableEffDate = new NullableDateTime(effDate);

            DetachedCriteria criteria = DetachedCriteria.For<BomDetail>();
            criteria.Add(Expression.Eq("Item.Code", itemCode));
            criteria.Add(Expression.Le("StartDate", effDate));
            criteria.Add(Expression.Or(Expression.Ge("EndDate", effDate), Expression.IsNull("EndDate")));

            IList<BomDetail> bomDetailList = criterialMgrE.FindAll<BomDetail>(criteria);

            return this.GetNoOverloadBomDetail(bomDetailList);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetBomView_Nml(Item item, DateTime effDate)
        {
            IList<BomDetail> bomViewList = new List<BomDetail>();
            IList<BomDetail> bomDetailList = new List<BomDetail>();
            IList<BomDetail> lastLevelBomDetailList = this.GetLastLevelBomDetail(item.Code, effDate);
            if (lastLevelBomDetailList != null && lastLevelBomDetailList.Count > 0)
            {
                foreach (BomDetail bomDetail in lastLevelBomDetailList)
                {
                    bomDetailList = this.GetNextLevelBomDetail(bomDetail.Bom.Code, effDate);
                    foreach (BomDetail bd in bomDetailList)
                    {
                        bd.CalculatedQty = bd.RateQty * (1 + bd.DefaultScrapPercentage);
                        bd.CalculatedQtyWithoutScrapRate = bd.RateQty;
                        bomViewList.Add(bd);
                    }
                }
            }

            string nextLevelBomCode = (item.Bom != null ? item.Bom.Code : item.Code);
            bomDetailList = this.GetNextLevelBomDetail(nextLevelBomCode, effDate);
            if (bomDetailList != null && bomDetailList.Count > 0)
            {
                foreach (BomDetail bd in bomDetailList)
                {
                    bd.CalculatedQty = bd.RateQty * (1 + bd.DefaultScrapPercentage);
                    bd.CalculatedQtyWithoutScrapRate = bd.RateQty;
                    bomViewList.Add(bd);
                }
            }

            return this.GetNoOverloadBomDetail(bomViewList);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetBomView_Cost(string itemCode, DateTime effDate)
        {
            Item item = itemMgrE.LoadItem(itemCode);
            string bomCode = item.Bom != null ? item.Bom.Code : item.Code;
            IList<BomDetail> bomDetailList = this.GetFlatBomDetail(bomCode, effDate);
            IList<BomDetail> bomViewList = new List<BomDetail>();
            IList<BomDetail> costBomViewList = new List<BomDetail>();
            bomViewList = this.GetAllBomDetailTree(bomDetailList, effDate);
            bomViewList = this.GetNoOverloadBomDetail(bomViewList);
            if (bomViewList != null && bomViewList.Count > 0)
            {
                bomViewList = this.GetCostBomDetail(bomViewList);
                Bom bom = bomMgrE.LoadBom(bomCode);
                foreach (BomDetail bomDetail in bomViewList)
                {
                    BomDetail costBomDetail = new BomDetail();
                    CloneHelper.CopyProperty(bomDetail, costBomDetail, BomCompDetail);
                    costBomDetail.Bom = bom;

                    costBomViewList.Add(costBomDetail);
                }
            }

            return this.GetNoOverloadBomDetail(costBomViewList);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BomDetail GetDefaultBomDetailForAbstractItem(Item abstractItem, Routing routing, DateTime effDate)
        {
            return GetDefaultBomDetailForAbstractItem(abstractItem, routing, effDate, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BomDetail GetDefaultBomDetailForAbstractItem(string abstractItemCode, Routing routing, DateTime effDate)
        {
            return GetDefaultBomDetailForAbstractItem(abstractItemCode, routing, effDate, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BomDetail GetDefaultBomDetailForAbstractItem(Item abstractItem, Routing routing, DateTime effDate, Location defaultLocationFrom)
        {
            string bomCode = this.bomMgrE.FindBomCode(abstractItem);
            IList<BomDetail> bomDetailList = this.GetNextLevelBomDetail(bomCode, effDate);
            if (bomDetailList != null && bomDetailList.Count > 0)
            {
                bomDetailList = IListHelper.Sort<BomDetail>(bomDetailList, "Priority"); //����Priority��������

                foreach (BomDetail bomDetail in bomDetailList)
                {
                    #region ��Դ��λ�����߼�BomDetail-->RoutingDetail-->defaultLocationFrom
                    //defaultLocationFrom = FlowDetail-->Flow
                    Location bomLocation = bomDetail.Location;

                    if (bomLocation == null)
                    {
                        RoutingDetail routingDetail = routingDetailMgrE.LoadRoutingDetail(routing, bomDetail.Operation, bomDetail.Reference);
                        if (routingDetail != null)
                        {
                            if (bomLocation == null)
                            {
                                bomLocation = routingDetail.Location;
                            }
                        }
                    }

                    if (bomLocation == null)
                    {
                        bomLocation = defaultLocationFrom;
                    }
                    #endregion

                    //���û���ҵ���λ��ֱ��������һ��bomDetail
                    if (bomLocation != null)
                    {
                        if (!bomLocation.AllowNegativeInventory)
                        {
                            //���������
                            //todo �����
                            throw new NotImplementedException();
                        }
                        else
                        {
                            //������棬ֱ�ӷ���
                            return bomDetail;
                        }
                    }
                }
            }

            return null;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetBomDetailListForAbstractItem(Item abstractItem, Routing routing, DateTime effDate, Location defaultLocationFrom)
        {
            string bomCode = this.bomMgrE.FindBomCode(abstractItem);

            return GetBomDetailListForAbstractItem(routing, bomCode, effDate, defaultLocationFrom);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetBomDetailListForAbstractItem(Routing routing, string bomCode, DateTime effDate, Location defaultLocationFrom)
        {
            IList<BomDetail> bomDetailList = this.GetNextLevelBomDetail(bomCode, effDate);
            if (bomDetailList != null && bomDetailList.Count > 0)
            {
                bomDetailList = IListHelper.Sort<BomDetail>(bomDetailList, "Priority"); //����Priority��������

                foreach (BomDetail bomDetail in bomDetailList)
                {
                    #region ��Դ��λ�����߼�BomDetail-->RoutingDetail-->defaultLocationFrom
                    //defaultLocationFrom = FlowDetail-->Flow
                    Location bomLocation = bomDetail.Location;

                    if (bomLocation == null)
                    {
                        RoutingDetail routingDetail = routingDetailMgrE.LoadRoutingDetail(routing, bomDetail.Operation, bomDetail.Reference);
                        if (routingDetail != null)
                        {
                            if (bomLocation == null)
                            {
                                bomLocation = routingDetail.Location;
                            }
                        }
                    }

                    if (bomLocation == null)
                    {
                        bomLocation = defaultLocationFrom;
                    }
                    bomDetail.Location = bomLocation;
                    #endregion
                }
            }

            return bomDetailList;
        }


        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetBomDetailListForAbstractItem(string abstractItemCode, Routing routing, DateTime effDate, Location defaultLocationFrom)
        {
            Item abstractItem = this.itemMgrE.LoadItem(abstractItemCode);
            return GetBomDetailListForAbstractItem(abstractItem, routing, effDate, defaultLocationFrom);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BomDetail GetDefaultBomDetailForAbstractItem(string abstractItemCode, Routing routing, DateTime effDate, Location defaultLocationFrom)
        {
            Item abstractItem = this.itemMgrE.LoadItem(abstractItemCode);
            return GetDefaultBomDetailForAbstractItem(abstractItem, routing, effDate, defaultLocationFrom);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BomDetail GetDefaultBomDetailForAbstractItem(Item abstractItem, string routingCode, DateTime effDate)
        {
            return GetDefaultBomDetailForAbstractItem(abstractItem, routingCode, effDate, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BomDetail GetDefaultBomDetailForAbstractItem(string abstractItemCode, string routingCode, DateTime effDate)
        {
            return GetDefaultBomDetailForAbstractItem(abstractItemCode, routingCode, effDate, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BomDetail GetDefaultBomDetailForAbstractItem(Item abstractItem, string routingCode, DateTime effDate, Location defaultLocationFrom)
        {
            Routing routing = this.routingMgrE.LoadRouting(routingCode);
            return GetDefaultBomDetailForAbstractItem(abstractItem, routing, effDate, defaultLocationFrom);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BomDetail GetDefaultBomDetailForAbstractItem(string abstractItemCode, string routingCode, DateTime effDate, Location defaultLocationFrom)
        {
            Item abstractItem = this.itemMgrE.LoadItem(abstractItemCode);
            Routing routing = this.routingMgrE.LoadRouting(routingCode);
            return GetDefaultBomDetailForAbstractItem(abstractItem, routing, effDate, defaultLocationFrom);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BomDetail LoadBomDetail(string bomCode, string itemCode, string reference, DateTime date)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(BomDetail));
            criteria.Add(Expression.Eq("Bom.Code", bomCode));
            criteria.Add(Expression.Eq("Item.Code", itemCode));
            criteria.Add(Expression.Eq("Reference", reference));
            criteria.Add(Expression.Le("StartDate", date));
            criteria.Add(Expression.Or(Expression.IsNull("EndDate"), Expression.Ge("EndDate", date)));
            criteria.AddOrder(Order.Desc("StartDate"));

            IList<BomDetail> bomDetailList = criterialMgrE.FindAll<BomDetail>(criteria);
            if (bomDetailList != null && bomDetailList.Count > 0)
            {
                return bomDetailList[0];
            }
            else
            {
                return null;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public BomDetail LoadBomDetail(string parCode, string compCode, int Operation, string Reference, DateTime startTime)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(BomDetail));
            criteria.Add(Expression.Eq("Bom.Code", parCode));
            criteria.Add(Expression.Eq("Item.Code", compCode));
            criteria.Add(Expression.Eq("Operation", Operation));
            //criteria.Add(Expression.Eq("Reference", Reference));
            if (string.IsNullOrEmpty(Reference))
            {
                criteria.Add(Expression.Or(Expression.IsNull("Reference"), Expression.Eq("Reference", "")));
            }
            else
            {
                criteria.Add(Expression.Eq("Reference", Reference));
            }
            criteria.Add(Expression.Ge("StartDate", startTime));

            IList<BomDetail> bomDetails = criterialMgrE.FindAll<BomDetail>(criteria);
            if (bomDetails.Count > 0)
            {
                return bomDetails[0];
            }
            else
            {
                return null;
            }
        }

        [Transaction(TransactionMode.Requires)]
        public int ReadFromXls(Stream inputStream, User user)
        {
            if (inputStream.Length == 0)
                throw new BusinessErrorException("Import.Stream.Empty");

            HSSFWorkbook workbook = null;
            Sheet sheet = null;
            try
            {
                workbook = new HSSFWorkbook(inputStream);
                sheet = workbook.GetSheetAt(0);
            }
            catch (IOException e)
            {
                if (sheet != null)
                {
                    sheet.Dispose();
                    sheet = null;
                }
                if (workbook != null)
                {
                    workbook.Dispose();
                    workbook = null;
                }

                throw new BusinessErrorException("Import.Result.Error.SaveAs");
            }

            IList<Item> itemList = new List<Item>();
            IEnumerator rows = sheet.GetRowEnumerator();

            ImportHelper.JumpRows(rows, 1);

            #region �ж���

            int col0 = 0;//���
            int col1 = 1;//����	
            int col2 = 2;//�ο�
            int col3 = 3;//������	
            int col4 = 4;//������	
            int col5 = 5;//��λ	
            int col6 = 6;//����	
            int col7 = 7;//��ʼʱ��	
            int col8 = 8;//����ʱ��	
            int col9 = 9;//����	
            int col10 = 10;//��Ʒ��	
            int col11 = 11;//�س巽ʽ	
            int col12 = 12;//λ��
            int col13 = 13;//����ɨ������
            int col14 = 14;//��ӡ
            int col15 = 15;//��λ	
            int col16 = 16;//���ȼ�

            #endregion

            DateTime now = DateTime.Now;
            int count = 0;
            while (rows.MoveNext())
            {
                Row row = (HSSFRow)rows.Current;
                if (!ImportHelper.CheckValidDataRow(row, 0, 15))
                {
                    break;//�߽�
                }

                string reference = string.Empty;
                int operation = 0;
                string bomCode = string.Empty;
                string itemCode = string.Empty;
                string uomCode = string.Empty;
                string structureType = string.Empty;
                DateTime startDate = DateTime.Now;
                DateTime? endDate = DateTime.Now;

                decimal rateQty = decimal.Zero;

                decimal scrapPercentage = decimal.Zero;
                string backFlushMethod = string.Empty;
                bool isShipScanHu = false;
                bool needPrint = false;

                string locationCode = string.Empty;
                int priority = 0;
                string positionNo = string.Empty;

                #region ��ȡ������
                try
                {
                    bomCode = ImportHelper.GetCellStringValue(row.GetCell(col3));
                    if (bomCode == string.Empty)
                        throw new BusinessErrorException("MasterData.Bom.Import.Empty.Error.Bom", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.Bom", (row.RowNum + 1).ToString());
                }
                #endregion

                Bom bom = null;
                #region ��֤������Ƿ����
                try
                {
                    bom = bomMgrE.LoadBom(bomCode);
                    if (bom == null)
                    {
                        throw new BusinessErrorException("MasterData.Bom.Import.Database.Error.Bom", (row.RowNum + 1).ToString());
                    }

                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Database.Error.Bom", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ����
                try
                {
                    operation = (int)((row.GetCell(col1)).NumericCellValue);
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.Operation", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ�ο�
                try
                {
                    reference = ImportHelper.GetCellStringValue(row.GetCell(col2));

                    if (string.IsNullOrEmpty(reference))
                    {
                        reference = null;
                    }
                    //throw new BusinessErrorException("MasterData.Bom.Import.Empty.Error.Reference", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.Reference", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ������
                try
                {
                    itemCode = ImportHelper.GetCellStringValue(row.GetCell(col4));
                    if (itemCode == string.Empty)
                        throw new BusinessErrorException("MasterData.Bom.Import.Empty.Error.Item", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.Item", (row.RowNum + 1).ToString());
                }
                #endregion

                Item item = null;
                #region ��֤����Ƿ����
                try
                {
                    item = itemMgrE.LoadItem(itemCode);
                    if (item == null)
                    {
                        throw new BusinessErrorException("MasterData.Bom.Import.Database.Error.Item", (row.RowNum + 1).ToString());
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Database.Error.Item", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ��λ
                try
                {
                    uomCode = ImportHelper.GetCellStringValue(row.GetCell(col5));
                    if (uomCode == string.Empty)
                        throw new BusinessErrorException("MasterData.Bom.Import.Empty.Error.UOM", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.UOM", (row.RowNum + 1).ToString());
                }
                #endregion

                Uom uom = null;
                #region ��֤��λ�Ƿ����
                try
                {
                    uom = uomMgrE.LoadUom(uomCode);
                    if (item == null)
                    {
                        throw new BusinessErrorException("MasterData.Bom.Import.Database.Error.UOM", (row.RowNum + 1).ToString());
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Database.Error.UOM", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ����
                try
                {
                    structureType = ImportHelper.GetCellStringValue(row.GetCell(col6));
                    if (structureType == string.Empty)
                        throw new BusinessErrorException("MasterData.Bom.Import.Empty.Error.StructureType", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.StructureType", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ��ʼʱ��
                try
                {

                    string temp = ImportHelper.GetCellStringValue(row.GetCell(col7));
                    if (temp == null || temp == string.Empty)
                    {
                        throw new BusinessErrorException("MasterData.Bom.Import.Empty.Error.StartDate", (row.RowNum + 1).ToString());
                    }
                    else
                    {
                        startDate = DateTime.Parse(temp);
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.StartDate", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ����ʱ��
                try
                {
                    string temp = ImportHelper.GetCellStringValue(row.GetCell(col8));
                    if (temp == null || temp == string.Empty)
                    {
                        endDate = null;
                    }
                    else
                    {
                        endDate = DateTime.Parse(temp);
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.EndDate", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ����
                try
                {
                    rateQty = (decimal)(row.GetCell(col9).NumericCellValue);
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.RateQty", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ��Ʒ��
                try
                {
                    scrapPercentage = (decimal)(row.GetCell(col10).NumericCellValue);
                }
                catch
                {
                    //throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.ScrapPercentage", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ�س巽ʽ
                try
                {
                    backFlushMethod = ImportHelper.GetCellStringValue(row.GetCell(col11));
                    if (backFlushMethod == string.Empty)
                        throw new BusinessErrorException("MasterData.Bom.Import.Empty.Error.BackFlushMethod", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.BackFlushMethod", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡλ��
                try
                {
                    positionNo = row.GetCell(col12).StringCellValue;
                    if (string.IsNullOrEmpty(positionNo))
                    {
                        positionNo = null;
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.PositionNo", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ����ɨ������
                try
                {
                    isShipScanHu = bool.Parse(row.GetCell(col13).StringCellValue);
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.IsShipScanHu", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ��ӡ
                try
                {
                    needPrint = bool.Parse(row.GetCell(col14).StringCellValue);
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.NeedPrint", (row.RowNum + 1).ToString());
                }
                #endregion
                Location location = null;
                #region ��ȡ��λ
                try
                {
                    locationCode = ImportHelper.GetCellStringValue(row.GetCell(col15));
                    if (locationCode != null && locationCode != string.Empty)
                    {

                        DetachedCriteria detachedCriteria = DetachedCriteria.For<Location>();
                        detachedCriteria.Add(Expression.Eq("Code", locationCode));

                        IList<Location> locationList = criterialMgrE.FindAll<Location>(detachedCriteria);

                        if (locationList != null && locationList.Count > 0)
                        {
                            location = locationList[0];
                        }
                    }
                    else
                    {
                        locationCode = null;
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.Location", (row.RowNum + 1).ToString());
                }
                #endregion


                #region ��ȡ���ȼ�
                try
                {
                    priority = (int)(row.GetCell(col16).NumericCellValue);
                }
                catch
                {
                    //throw new BusinessErrorException("MasterData.Bom.Import.Read.Error.Priority", (row.RowNum + 1).ToString());
                }
                #endregion

                count++;
                BomDetail bomDetail = null;
                bomDetail = this.LoadBomDetail(bomCode, itemCode, operation, reference, startDate);
                bool isUpdate = true;
                if (bomDetail == null)
                {
                    bomDetail = new BomDetail();
                    bomDetail.Operation = operation;
                    bomDetail.Bom = bom;
                    bomDetail.Item = item;
                    bomDetail.StartDate = startDate;
                    isUpdate = false;
                }


                bomDetail.Uom = uom;
                bomDetail.StructureType = structureType;

                bomDetail.EndDate = endDate;
                bomDetail.RateQty = rateQty;
                bomDetail.ScrapPercentage = scrapPercentage;

                bomDetail.BackFlushMethod = backFlushMethod;
                bomDetail.NeedPrint = needPrint;
                bomDetail.PositionNo = positionNo;
                bomDetail.IsShipScanHu = isShipScanHu;
                bomDetail.Location = location;
                bomDetail.Priority = priority;

                if (isUpdate)
                {
                    this.UpdateBomDetail(bomDetail);
                }
                else
                {

                    this.CreateBomDetail(bomDetail);
                }
            }


            if (count == 0)
                throw new BusinessErrorException("Import.Result.Error.ImportNothing");

            return count;
        }

        #endregion Customized Methods

        #region Private Methods
        private void ProcessCurrentBomDetail(IList<BomDetail> flatBomDetailList, BomDetail currentBomDetail, DateTime efftiveDate)
        {
            if (currentBomDetail.StructureType == BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE_VALUE_N) //��ͨ�ṹ(N)
            {
                ProcessCurrentBomDetailByItemType(flatBomDetailList, currentBomDetail, efftiveDate);
            }
            else if (currentBomDetail.StructureType == BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE_VALUE_O) //ѡ���(O)
            {
                currentBomDetail.OptionalItemGroup = currentBomDetail.Bom.Code;   //Ĭ����BomCode��Ϊѡ��������
                ProcessCurrentBomDetailByItemType(flatBomDetailList, currentBomDetail, efftiveDate);
            }
            else if (currentBomDetail.StructureType == BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE_VALUE_X) //��ṹ(X)
            {
                //�������ṹ(X)�������Լ��ӵ����ر���������·ֽ�
                NestingGetNextLevelBomDetail(flatBomDetailList, currentBomDetail, efftiveDate);
            }
            else
            {
                throw new TechnicalException("no such kind fo bomdetail structure type " + currentBomDetail.StructureType);
            }
        }

        private void ProcessCurrentBomDetailByItemType(IList<BomDetail> flatBomDetailList, BomDetail currentBomDetail, DateTime efftiveDate)
        {
            if (currentBomDetail.Item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_X)
            {
                //����������(X)���������·ֽ�
                NestingGetNextLevelBomDetail(flatBomDetailList, currentBomDetail, efftiveDate);
            }
            else if (currentBomDetail.Item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_A)
            {
                //todo ������費��Ҫ�ֽ⣿�Ƿ���������ϻس�ʱ��ָ����
                flatBomDetailList.Add(currentBomDetail);
            }
            else if (currentBomDetail.Item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_K)
            {
                //K���͵�Item���ܳ�����Bom�ṹ��
                throw new BusinessErrorException("Bom.Error.ItemTypeKInBom", currentBomDetail.Bom.Code);

                //������������·ֽ�
                //NestingGetNextLevelBomDetail(flatBomDetailList, currentBomDetail, efftiveDate);
            }
            else
            {
                //ֱ�Ӽ��뵽flatBomDetailList
                flatBomDetailList.Add(currentBomDetail);
            }
        }

        private void NestingGetNextLevelBomDetail(IList<BomDetail> flatBomDetailList, BomDetail currentBomDetail, DateTime efftiveDate)
        {
            string nextLevelBomCode = this.bomMgrE.FindBomCode(currentBomDetail.Item);
            IList<BomDetail> nextBomDetailList = this.GetNextLevelBomDetail(nextLevelBomCode, efftiveDate);

            if (nextBomDetailList != null && nextBomDetailList.Count > 0)
            {
                foreach (BomDetail nextBomDetail in nextBomDetailList)
                {
                    if (nextBomDetail.Item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_K)
                    {
                        //K���͵�Item���ܳ�����Bom�ṹ��
                        throw new BusinessErrorException("Bom.Error.ItemTypeKInBom", nextBomDetail.Bom.Code);
                    }

                    //��ǰ�Ӽ���Uom���²�Bom��Uom��ƥ�䣬��Ҫ����λת��
                    if (currentBomDetail.Uom.Code != nextBomDetail.Bom.Uom.Code)
                    {
                        //��λ����
                        nextBomDetail.CalculatedQty = uomConversionMgrE.ConvertUomQty(currentBomDetail.Item, currentBomDetail.Uom, 1, nextBomDetail.Bom.Uom)
                            * currentBomDetail.CalculatedQty * nextBomDetail.RateQty * (1 + nextBomDetail.DefaultScrapPercentage);

                        nextBomDetail.CalculatedQtyWithoutScrapRate = uomConversionMgrE.ConvertUomQty(currentBomDetail.Item, currentBomDetail.Uom, 1, nextBomDetail.Bom.Uom)
                           * currentBomDetail.CalculatedQtyWithoutScrapRate * nextBomDetail.RateQty;
                    }
                    else
                    {
                        nextBomDetail.CalculatedQty = nextBomDetail.RateQty * currentBomDetail.CalculatedQty * (1 + nextBomDetail.DefaultScrapPercentage);
                        nextBomDetail.CalculatedQtyWithoutScrapRate = nextBomDetail.RateQty * currentBomDetail.CalculatedQtyWithoutScrapRate;
                    }

                    nextBomDetail.OptionalItemGroup = currentBomDetail.OptionalItemGroup;

                    ProcessCurrentBomDetail(flatBomDetailList, nextBomDetail, efftiveDate);
                }
            }
            else
            {
                throw new BusinessErrorException("Bom.Error.NotFoundForItem", currentBomDetail.Item.Code);
            }
        }

        public IList<BomDetail> GetNoOverloadBomDetail(IList<BomDetail> bomDetailList)
        {
            //����BomCode��ItemCode��Operation��Reference��ͬ��BomDetail��ֻȡStartDate���ġ�
            if (bomDetailList != null && bomDetailList.Count > 0)
            {
                IList<BomDetail> noOverloadBomDetailList = new List<BomDetail>();
                foreach (BomDetail bomDetail in bomDetailList)
                {
                    int overloadIndex = -1;
                    for (int i = 0; i < noOverloadBomDetailList.Count; i++)
                    {
                        //�ж�BomCode��ItemCode��Operation��Reference�Ƿ���ͬ
                        if (noOverloadBomDetailList[i].Bom.Code == bomDetail.Bom.Code
                            && noOverloadBomDetailList[i].Item.Code == bomDetail.Item.Code
                            && noOverloadBomDetailList[i].Operation == bomDetail.Operation
                            && noOverloadBomDetailList[i].Reference == bomDetail.Reference)
                        {
                            //������ͬ�ģ���¼λ�á�
                            overloadIndex = i;
                            break;
                        }
                    }

                    if (overloadIndex == -1)
                    {
                        //û����ͬ�ļ�¼��ֱ�Ӱ�BomDetail���뷵���б�
                        noOverloadBomDetailList.Add(bomDetail);
                    }
                    else
                    {
                        //����ͬ�ļ�¼���ж�bomDetail.StartDate�ͽ�����еĴ�
                        if (noOverloadBomDetailList[overloadIndex].StartDate < bomDetail.StartDate)
                        {
                            //bomDetail.StartDate���ڽ�����еģ��滻�����
                            noOverloadBomDetailList[overloadIndex] = bomDetail;
                        }
                    }
                }
                return noOverloadBomDetailList;
            }
            else
            {
                return null;
            }
        }

        public IList<BomDetail> GetAllBomDetailTree(IList<BomDetail> treeBomDetailList, DateTime effDate)
        {
            IList<BomDetail> bomDetailList = new List<BomDetail>();
            foreach (BomDetail bomDetail in treeBomDetailList)
            {
                bomDetail.CalculatedQty = bomDetail.RateQty * (1 + bomDetail.DefaultScrapPercentage);
                bomDetail.CalculatedQtyWithoutScrapRate = bomDetail.RateQty;
                bomDetailList.Add(bomDetail);

                IList<BomDetail> tempBomDetailList = new List<BomDetail>();
                string nextLevelBomCode = (bomDetail.Item.Bom != null ? bomDetail.Item.Bom.Code : bomDetail.Item.Code);
                tempBomDetailList = this.GetNextLevelBomDetail(nextLevelBomCode, effDate);
                if (tempBomDetailList != null && tempBomDetailList.Count > 0)
                {
                    foreach (var tempBomDetail in tempBomDetailList)
                    {
                        tempBomDetail.BomLevel = bomDetail.BomLevel + 1;
                        tempBomDetail.CalculatedQty = tempBomDetail.RateQty * (1 + tempBomDetail.DefaultScrapPercentage);
                        tempBomDetail.CalculatedQtyWithoutScrapRate = tempBomDetail.RateQty;
                        tempBomDetail.AccumQty = bomDetail.AccumQty * tempBomDetail.CalculatedQty;
                    }
                    IList<BomDetail> nextBomDetailList = new List<BomDetail>();
                    nextBomDetailList = this.GetAllBomDetailTree(tempBomDetailList, effDate);
                    foreach (BomDetail bd in nextBomDetailList)
                    {
                        bd.CalculatedQty = bd.RateQty * (1 + bd.DefaultScrapPercentage);
                        bd.CalculatedQtyWithoutScrapRate = bd.RateQty;
                        bomDetailList.Add(bd);
                    }
                }
            }

            return bomDetailList;
        }

        public IList<BomDetail> GetCostBomDetail(IList<BomDetail> bomDetailList)
        {
            IList<BomDetail> costBomDetailList = new List<BomDetail>();
            foreach (BomDetail bomDetail in bomDetailList)
            {
                bool isExist = false;
                int index = bomDetailList.IndexOf(bomDetail);
                foreach (BomDetail bd in bomDetailList)
                {
                    int bdIndex = bomDetailList.IndexOf(bd);
                    if (bdIndex <= index)
                    {
                        continue;
                    }

                    string nextLevelBomCode = (bomDetail.Item.Bom != null ? bomDetail.Item.Bom.Code : bomDetail.Item.Code);
                    if (nextLevelBomCode == bd.Bom.Code)
                    {
                        isExist = true;
                        bomDetailList[bdIndex].CalculatedQty = bomDetail.CalculatedQty * bd.CalculatedQty;
                        bomDetailList[bdIndex].CalculatedQtyWithoutScrapRate = bomDetail.CalculatedQtyWithoutScrapRate * bd.CalculatedQtyWithoutScrapRate;
                        break;
                    }
                }

                if (!isExist)
                {
                    costBomDetailList.Add(bomDetail);
                }
            }

            return this.GetNoOverloadCostBomDetail(costBomDetailList);
        }

        public IList<BomDetail> GetNoOverloadCostBomDetail(IList<BomDetail> bomDetailList)
        {
            IList<BomDetail> noOverloadBomDetailList = new List<BomDetail>();
            //����ItemCode,UOM��ͬ��BomDetail,���ۼ�
            if (bomDetailList != null && bomDetailList.Count > 0)
            {
                foreach (BomDetail bomDetail in bomDetailList)
                {
                    int overloadIndex = -1;
                    for (int i = 0; i < noOverloadBomDetailList.Count; i++)
                    {
                        //�ж�BomCode��ItemCode��Operation��Reference�Ƿ���ͬ
                        if (noOverloadBomDetailList[i].Item.Code == bomDetail.Item.Code
                            && noOverloadBomDetailList[i].Uom.Code == bomDetail.Uom.Code)
                        {
                            //������ͬ�ģ���¼λ�á�
                            overloadIndex = i;
                            break;
                        }
                    }

                    if (overloadIndex == -1)
                    {
                        //û����ͬ�ļ�¼��ֱ�Ӱ�BomDetail���뷵���б�
                        noOverloadBomDetailList.Add(bomDetail);
                    }
                    else
                    {
                        //����ͬ�ļ�¼�ۼ�
                        noOverloadBomDetailList[overloadIndex].CalculatedQty += bomDetail.CalculatedQty;
                        noOverloadBomDetailList[overloadIndex].CalculatedQtyWithoutScrapRate += bomDetail.CalculatedQtyWithoutScrapRate;
                    }
                }
            }

            return noOverloadBomDetailList;
        }
        #endregion
    }
}


#region Extend Class




namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class BomDetailMgrE : com.Sconit.Service.MasterData.Impl.BomDetailMgr, IBomDetailMgrE
    {

    }
}
#endregion
