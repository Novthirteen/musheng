using com.Sconit.Service.Ext.MasterData;
using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.Exception;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Linq;
using com.Sconit.Utility;
using System.Data;
using System.Text;
using com.Sconit.Persistence;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ItemMgr : ItemBaseMgr, IItemMgr
    {
        private static IList<Item> cachedAllItem;
        private static DateTime cacheDateTime;
        private static string cachedAllItemString;
        private static long cachedAllItemCount;
        private static long cachedAllItemId;

        public IItemKitMgrE itemKitMgrE { get; set; }

        public ICriteriaMgrE criteriaMgrE { get; set; }
        public ICodeMasterMgrE codeMasterMgrE { get; set; }
        public IItemTypeMgrE itemTypeMgrE { get; set; }
        public IItemBrandMgrE itemBrandMgrE { get; set; }
        //public IUomMgrE uomMgrE { get; set; }
        public IItemCategoryMgrE itemCategoryMgrE { get; set; }
        public ISqlHelperDao sqlHelperDao { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<Item> GetCacheAllItem()
        {
            if (cachedAllItem == null)
            {
                cachedAllItem = GetAllItem();
                cacheDateTime = DateTime.Now;
            }
            else
            {
                //���Item��С�Ƿ����仯
                //DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
                //criteria.Add(Expression.Eq("IsActive", true));
                //criteria.SetProjection(Projections.ProjectionList().Add(Projections.Count("Code")));
                //IList<int> count = this.criteriaMgrE.FindAll<int>(criteria);

                if (cacheDateTime < DateTime.Now.AddMinutes(-10))
                {
                    cachedAllItem = GetAllItem();
                    cacheDateTime = DateTime.Now;
                }
            }

            return cachedAllItem;
        }

        public Item GetCatchItem(string itemCode)
        {
            return GetCacheAllItem().FirstOrDefault(p => string.Equals(itemCode.Trim(), p.Code.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public string GetCacheAllItemString()
        {
            if (cachedAllItemString == null)
            {
                DoGetAllCacheItemString();
            }
            else
            {
                //���Item��С�Ƿ����仯
                DataSet ds = sqlHelperDao.GetDatasetBySql("select COUNT(1) as c, SUM(Id) as s from Item where IsActive = 1", null);
                long count = long.Parse(ds.Tables[0].Rows[0][0].ToString());
                long sumId = long.Parse(ds.Tables[0].Rows[0][1].ToString());

                if (count != cachedAllItemCount || sumId != cachedAllItemId)
                {
                    DoGetAllCacheItemString();
                }
            }

            return cachedAllItemString;
        }

        private static object GetAllItemStringLock = new object();
        private void DoGetAllCacheItemString()
        {
            lock (GetAllItemStringLock)
            {
                //���Item��С�Ƿ����仯
                DataSet ds = sqlHelperDao.GetDatasetBySql("select COUNT(1) as c, SUM(Id) as s from Item where IsActive = 1", null);
                long count = long.Parse(ds.Tables[0].Rows[0][0].ToString());
                long sumId = long.Parse(ds.Tables[0].Rows[0][1].ToString());

                if (count != cachedAllItemCount || sumId != cachedAllItemId)
                {
                    IList<Item> thisAllItem = GetAllItem();
                    StringBuilder data = new StringBuilder("[");
                    for (int i = 0; i < thisAllItem.Count; i++)
                    {
                        Item item = thisAllItem[i];
                        string desc = item.Description1.Replace("\r", "").Replace("\n", "");
                        desc = desc.Replace("'", "");
                        data.Append(TextBoxHelper.GenSingleData(desc, item.Code.Replace("\r", "").Replace("\n", "")) + (i < (thisAllItem.Count - 1) ? "," : string.Empty));
                    }
                    data.Append("]");

                    cachedAllItemString = data.ToString();
                    //cachedAllItemCount = count;
                    //cachedAllItemId = sumId;
                }
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Item> GetPMItem()
        {
            IList<Item> listItem = GetCacheAllItem();
            IList<Item> listPMItem = new List<Item>();
            foreach (Item item in listItem)
            {
                if (item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_M || item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_P)
                {
                    listPMItem.Add(item);
                }
            }

            return listPMItem;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Item> GetItem(DateTime lastModifyDate, int firstRow, int maxRows)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
            criteria.Add(Expression.Gt("LastModifyDate", lastModifyDate));
            criteria.AddOrder(Order.Asc("LastModifyDate"));
            IList<Item> itemList = criteriaMgrE.FindAll<Item>(criteria, firstRow, maxRows);
            if (itemList.Count > 0)
            {
                return itemList;
            }
            return null;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Item> GetItem(IList<string> itemCodeList)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
            if (itemCodeList.Count == 1)
            {
                criteria.Add(Expression.Eq("Code", itemCodeList[0]));
            }
            else
            {
                criteria.Add(Expression.InG<string>("Code", itemCodeList));
            }
            return criteriaMgrE.FindAll<Item>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public int GetItemCount(DateTime lastModifyDate)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
            criteria.Add(Expression.Gt("LastModifyDate", lastModifyDate));
            IList<Item> itemList = criteriaMgrE.FindAll<Item>(criteria);
            return itemList.Count;
        }

        [Transaction(TransactionMode.Unspecified)]
        public int GetItemCount(string itemCategoryCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
            criteria.Add(Expression.Eq("ItemCategory.Code", itemCategoryCode));
            IList<Item> itemList = criteriaMgrE.FindAll<Item>(criteria);
            return itemList.Count;
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteItem(string code)
        {
            IList<ItemKit> itemKitList = itemKitMgrE.GetChildItemKit(code, true);
            itemKitMgrE.DeleteItemKit(itemKitList);

            base.DeleteItem(code);
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteItem(Item entity)
        {
            IList<ItemKit> itemKitList = itemKitMgrE.GetChildItemKit(entity, true);
            itemKitMgrE.DeleteItemKit(itemKitList);

            base.DeleteItem(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteItem(IList<Item> entityList)
        {
            IList<ItemKit> itemKitList = new List<ItemKit>();
            foreach (Item item in entityList)
            {
                itemKitList = itemKitMgrE.GetChildItemKit(item, true);
                itemKitMgrE.DeleteItemKit(itemKitList);
            }

            base.DeleteItem(entityList);
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteItem(IList<string> pkList)
        {
            IList<ItemKit> itemKitList = new List<ItemKit>();
            foreach (string item in pkList)
            {
                itemKitList = itemKitMgrE.GetChildItemKit(item, true);
                itemKitMgrE.DeleteItemKit(itemKitList);
            }

            base.DeleteItem(pkList);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Item CheckAndLoadItem(string itemCode)
        {
            Item item = this.LoadItem(itemCode);
            if (item == null)
            {
                throw new BusinessErrorException("Item.Error.ItemCodeNotExist", itemCode);
            }

            return item;
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateItem(Item item, User user)
        {
            item.LastModifyDate = DateTime.Now;
            item.LastModifyUser = user.Code;

            this.UpdateItem(item);
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateItem(Item item, User user)
        {
            item.LastModifyDate = DateTime.Now;
            item.LastModifyUser = user.Code;

            this.CreateItem(item);
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

            int colCode = 0;//����	
            int colDesc1 = 1;//����1	
            int colDesc2 = 2;//����2	
            int colIsActive = 3;//�Ƿ���Ч
            int colBrand = 4;//Ʒ��	
            int colType = 5;///����
            int colUom = 6;//������λ	
            int colCategory = 7;//��Ʒ��
            int colUnitCount = 8;//����װ		
            int colHuLotSize = 9;//���װ		
            int colCategory1 = 10;//����һ	
            int colCategory2 = 11;//���Ͷ�	
            int colCategory3 = 12;//������
            int colCategory4 = 13;//������
            int colScrapPercentage = 14;//�����		
            int colPinNumber = 15;//������		
            int colScrapPrice = 16;//��ĵ���		
            int colHistoryPrice = 17;//��ʷ�۸�		
            int colSalesCost = 18;//���۳ɱ�
            int colDefaultSupplier = 19;//����·��

            #endregion

            DateTime now = DateTime.Now;
            int count = 0;
            while (rows.MoveNext())
            {
                Row row = (HSSFRow)rows.Current;
                if (!ImportHelper.CheckValidDataRow(row, 0, 20))
                {
                    break;//�߽�
                }

                #region ��ʱ����
                string code = string.Empty;
                string desc1 = string.Empty;
                string desc2 = string.Empty;
                string type = string.Empty;
                bool isActive = true;
                decimal unitCount = decimal.Zero;
                string uomCode = string.Empty;
                int? huLotSize = null;
                string categoryCode = string.Empty;
                string category1 = string.Empty;
                string category2 = string.Empty;
                string category3 = string.Empty;
                string category4 = string.Empty;
                string itemBrand = string.Empty;
                decimal scrapPercentage = decimal.Zero;
                decimal pinNumber = decimal.Zero;
                decimal scrapPrice = decimal.Zero;
                decimal historyPrice = decimal.Zero;
                decimal salesCost = decimal.Zero;
                string defaultSupplier = string.Empty;
                #endregion

                Item item = null;
                #region ��ȡ�����
                try
                {
                    code = ImportHelper.GetCellStringValue(row.GetCell(colCode));
                    if (code == null || code == string.Empty)
                        throw new BusinessErrorException("MasterData.Item.Import.Empty.Error.Item", (row.RowNum + 1).ToString());
                    else
                    {

                        #region ��֤����Ƿ����
                        try
                        {
                            item = this.LoadItem(code);
                            if (item == null)
                            {
                                //throw new BusinessErrorException("MasterData.Item.Import.Database.Error.Item", (row.RowNum + 1).ToString());
                                item = new Item();
                            }
                        }
                        catch
                        {
                            throw new BusinessErrorException("MasterData.Item.Import.Database.Error.Item", (row.RowNum + 1).ToString());
                        }
                        #endregion
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.Item", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ����1
                try
                {
                    desc1 = ImportHelper.GetCellStringValue(row.GetCell(colDesc1));
                    if (desc1 == string.Empty)
                        throw new BusinessErrorException("MasterData.Item.Import.Empty.Error.Desc1", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.Desc1", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ����2
                try
                {
                    desc2 = ImportHelper.GetCellStringValue(row.GetCell(colDesc2));
                    if (desc2 == string.Empty)
                        throw new BusinessErrorException("MasterData.Item.Import.Empty.Error.Desc2", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.Desc2", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ�Ƿ���Ч
                try
                {
                    isActive = bool.Parse(row.GetCell(colIsActive).StringCellValue);
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.IsActive", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ��λ
                try
                {
                    uomCode = ImportHelper.GetCellStringValue(row.GetCell(colUom));
                    if (uomCode == string.Empty)
                        throw new BusinessErrorException("MasterData.Item.Import.Empty.Error.UOM", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.UOM", (row.RowNum + 1).ToString());
                }
                #endregion

                Uom uom = null;
                #region ��֤��λ�Ƿ����
                try
                {
                    //uom = uomMgrE.LoadUom(uomCode);

                    DetachedCriteria criteria = DetachedCriteria.For(typeof(Uom));
                    criteria.Add(Expression.Eq("Code", uomCode));
                    IList<Uom> uomList = criteriaMgrE.FindAll<Uom>(criteria);

                    if (uomList == null || uomList.Count == 0)
                    {
                        throw new BusinessErrorException("MasterData.Item.Import.Database.Error.UOM", (row.RowNum + 1).ToString());
                    }
                    else
                    {
                        uom = uomList[0];
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Database.Error.UOM", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ����
                try
                {
                    type = ImportHelper.GetCellStringValue(row.GetCell(colType));
                    if (type == string.Empty)
                        throw new BusinessErrorException("MasterData.Item.Import.Empty.Error.Type", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.Type", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��֤�����Ƿ����
                try
                {
                    CodeMaster codeMaster = codeMasterMgrE.LoadCodeMaster(BusinessConstants.CODE_MASTER_ITEM_TYPE, type);
                    if (codeMaster == null)
                    {
                        throw new BusinessErrorException("MasterData.Item.Import.Database.Error.Type", (row.RowNum + 1).ToString());
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Database.Error.Type", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ��Ʒ��
                try
                {
                    categoryCode = ImportHelper.GetCellStringValue(row.GetCell(colCategory));
                    if (categoryCode == string.Empty)
                        throw new BusinessErrorException("MasterData.Item.Import.Empty.Error.Category", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.Category", (row.RowNum + 1).ToString());
                }
                #endregion

                ItemCategory itemCategory = null;
                #region ��֤��Ʒ���Ƿ����
                try
                {
                    itemCategory = itemCategoryMgrE.LoadItemCategory(categoryCode);
                    if (item == null)
                    {
                        throw new BusinessErrorException("MasterData.Item.Import.Database.Error.Category", (row.RowNum + 1).ToString());
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Database.Error.Category", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ����װ
                try
                {
                    unitCount = (decimal)(row.GetCell(colUnitCount).NumericCellValue);
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.UnitCount", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ���װ
                try
                {
                    huLotSize = (int?)(row.GetCell(colHuLotSize).NumericCellValue);
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.HuLotSize", (row.RowNum + 1).ToString());
                }
                #endregion

                ItemType itemType1 = null;
                #region ����һ
                try
                {
                    category1 = ImportHelper.GetCellStringValue(row.GetCell(colCategory1));
                    if (category1 != null && category1 == string.Empty)
                    {
                        //��֤�Ƿ����
                        itemType1 = itemTypeMgrE.LoadItemType(category1);
                        if (itemType1 == null || itemType1.Level != 1)
                        {
                            throw new BusinessErrorException("MasterData.Item.Import.Database.Error.Category1", (row.RowNum + 1).ToString());
                        }
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.Category1", (row.RowNum + 1).ToString());
                }
                #endregion

                ItemType itemType2 = null;
                #region ���Ͷ�
                try
                {

                    category2 = ImportHelper.GetCellStringValue(row.GetCell(colCategory2));
                    if (category2 != null && category2 == string.Empty)
                    {
                        //��֤�Ƿ����

                        itemType2 = itemTypeMgrE.LoadItemType(category2);
                        if (itemType2 == null || itemType2.Level != 2)
                        {
                            throw new BusinessErrorException("MasterData.Item.Import.Database.Error.Category2", (row.RowNum + 1).ToString());
                        }
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.Category2", (row.RowNum + 1).ToString());
                }
                #endregion

                ItemType itemType3 = null;
                #region ������
                try
                {

                    category3 = ImportHelper.GetCellStringValue(row.GetCell(colCategory3));
                    if (category3 != null && category3 == string.Empty)
                    {
                        //��֤�Ƿ����

                        itemType3 = itemTypeMgrE.LoadItemType(category3);
                        if (itemType3 == null || itemType3.Level != 3)
                        {
                            throw new BusinessErrorException("MasterData.Item.Import.Database.Error.Category3", (row.RowNum + 1).ToString());
                        }
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.Category3", (row.RowNum + 1).ToString());
                }
                #endregion

                ItemType itemType4 = null;
                #region ������
                try
                {

                    category4 = ImportHelper.GetCellStringValue(row.GetCell(colCategory4));
                    if (category4 != null && category4 != string.Empty)
                    {
                        //��֤�Ƿ����

                        itemType4 = itemTypeMgrE.LoadItemType(category4);
                        if (itemType4 == null || itemType4.Level != 4)
                        {
                            throw new BusinessErrorException("MasterData.Item.Import.Database.Error.Category4", (row.RowNum + 1).ToString());
                        }
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.Category4", (row.RowNum + 1).ToString());
                }
                #endregion

                ItemBrand brand = null;
                #region Ʒ��
                try
                {
                    itemBrand = ImportHelper.GetCellStringValue(row.GetCell(colBrand));
                    if (itemBrand != null && itemBrand != string.Empty)
                    {
                        //��֤�Ƿ����
                        brand = itemBrandMgrE.LoadItemBrand(itemBrand);
                        if (brand == null)
                        {
                            throw new BusinessErrorException("MasterData.Item.Import.Database.Error.ItemBrand", (row.RowNum + 1).ToString());
                        }
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.ItemBrand", (row.RowNum + 1).ToString());
                }
                #endregion

                #region �����
                try
                {
                    string temp = ImportHelper.GetCellStringValue(row.GetCell(colScrapPercentage));
                    if (temp != null && temp.Length > 0)
                        scrapPercentage = decimal.Parse(temp);
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.ScrapPercentage", (row.RowNum + 1).ToString());
                }
                #endregion

                #region  ������
                try
                {
                    string temp = ImportHelper.GetCellStringValue(row.GetCell(colPinNumber));
                    if (temp != null && temp.Length > 0)
                        pinNumber = decimal.Parse(temp);
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.PinNumber", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ��ĵ���

                try
                {
                    string temp = ImportHelper.GetCellStringValue(row.GetCell(colScrapPrice));
                    if (temp != null && temp.Length > 0)
                        scrapPrice = decimal.Parse(temp);
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.ScrapPrice", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ��ʷ�۸�
                try
                {
                    string temp = ImportHelper.GetCellStringValue(row.GetCell(colHistoryPrice));
                    if (temp != null && temp.Length > 0)
                        historyPrice = decimal.Parse(temp);
                }
                catch
                {
                    //continue;
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.HistoryPrice", (row.RowNum + 1).ToString());
                }
                #endregion

                #region ��ȡ���۳ɱ�
                try
                {
                    string temp = ImportHelper.GetCellStringValue(row.GetCell(colSalesCost));
                    if (temp != null && temp.Length > 0)
                        salesCost = decimal.Parse(temp);
                }
                catch
                {
                    //continue;
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.SalesCost", (row.RowNum + 1).ToString());
                }
                #endregion

                Supplier supplier = null;
                #region ���㹩Ӧ��
                try
                {
                    defaultSupplier = ImportHelper.GetCellStringValue(row.GetCell(colDefaultSupplier));
                    if (defaultSupplier != null && defaultSupplier != string.Empty)
                    {
                        //��֤�Ƿ����

                        DetachedCriteria detachedCriteria = DetachedCriteria.For<Supplier>();
                        detachedCriteria.Add(Expression.Eq("Code", defaultSupplier));

                        IList<Supplier> supplierList = criteriaMgrE.FindAll<Supplier>(detachedCriteria);

                        if (supplierList != null && supplierList.Count > 0)
                        {
                            supplier = supplierList[0];
                        }
                        if (supplier == null)
                        {
                            throw new BusinessErrorException("MasterData.Item.Import.Database.Error.DefaultSupplier", (row.RowNum + 1).ToString());
                        }
                    }
                }
                catch
                {
                    throw new BusinessErrorException("MasterData.Item.Import.Read.Error.DefaultSupplier", (row.RowNum + 1).ToString());
                }
                #endregion

                item.Type = type;//����
                item.ItemCategory = itemCategory;//��Ʒ��
                item.IsActive = isActive;//�Ƿ���Ч
                item.Uom = uom;//������λ
                item.Desc1 = desc1;//����1
                item.Desc2 = desc2;//����2
                item.DefaultSupplier = defaultSupplier;//���㹩Ӧ��
                item.UnitCount = unitCount;//����װ
                item.HuLotSize = huLotSize;//���װ
                item.ItemBrand = brand;//Ʒ��
                item.PinNumber = pinNumber;//������
                item.Category1 = itemType1;//����һ
                item.Category2 = itemType2;//���Ͷ�
                item.Category3 = itemType3;//������
                item.Category4 = itemType4;//������
                item.ScrapPercentage = scrapPercentage;//�����
                item.HistoryPrice = historyPrice;//��ʷ�۸�
                item.SalesCost = salesCost;//���۳ɱ�
                item.ScrapPrice = scrapPrice;//��ĵ���

                item.IsRunMrp = false;
                item.IsSortAndColor = false;
                item.NeedInspect = false;

                //produtLineFeedSeqenceList.Add(produtLineFeedSeqence);
                count++;
                if (item.Code != null && item.Code.Length > 0)
                {
                    this.UpdateItem(item, user);
                }
                else
                {
                    item.Code = code;
                    this.CreateItem(item, user);
                }
            }

            if (count == 0)
                throw new BusinessErrorException("Import.Result.Error.ImportNothing");

            return count;
        }


        #endregion Customized Methods
    }
}


#region Extend Interface


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ItemMgrE : com.Sconit.Service.MasterData.Impl.ItemMgr, IItemMgrE
    {

    }
}
#endregion
