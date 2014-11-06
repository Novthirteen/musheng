using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Distribution;
using Castle.Services.Transaction;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Distribution;

namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class RepItemMgr : ReportBaseMgr
    {
        public override string reportTemplateFolder { get; set; }

        public IItemMgrE itemMgrE { get; set; }


        public RepItemMgr()
        {
        }

        /**
         * 填充报表
         * 
         * Param list [0]bill
         * Param list [0]IList<Item>           
         */
        public override bool FillValues(String templateFileName, IList<object> list)
        {
            try
            {
                if (list == null || list.Count < 1) return false;


                IList<Item> itemDetails = (IList<Item>)(list[0]);


                if (itemDetails == null || itemDetails.Count == 0)
                {
                    return false;
                }
                this.init(templateFileName);
                int rowIndex = 1;

                foreach (Item item in itemDetails)
                {

                    //代码	描述1	描述2	是否有效	品牌	类型	基本单位	产品类	单包装	外包装	类型一	类型二	类型三	类型四	损耗率	引脚数	损耗单价	历史价格	销售成本	定点供应商

                    int col = 0;
                    //代码
                    this.SetRowCell(rowIndex, col++, item.Code);

                    //描述1
                    this.SetRowCell(rowIndex, col++, item.Desc1);

                    //描述2
                    if (item.Desc2 != null)
                        this.SetRowCell(rowIndex, col++, item.Desc2);
                    else
                    {
                        col++;
                    }

                    //是否有效
                    this.SetRowCell(rowIndex, col++, item.IsActive.ToString());

                    //品牌	
                    if (item.ItemBrand != null)
                    {
                        this.SetRowCell(rowIndex, col++, item.ItemBrand.Code);
                    }
                    else
                    {
                        col++;
                    }

                    //类型
                    this.SetRowCell(rowIndex, col++, item.Type);

                    //基本单位
                    if (item.Uom != null)
                        this.SetRowCell(rowIndex, col++, item.Uom.Code);
                    else
                    {
                        col++;
                    }
                    //产品类
                    if (item.ItemCategory != null)
                        this.SetRowCell(rowIndex, col++, item.ItemCategory.Code);
                    else
                    {
                        col++;
                    }
                    //单包装	
                    this.SetRowCell(rowIndex, col++, double.Parse(item.UnitCount.ToString("0.########")));

                    //外包装	
                    if (item.HuLotSize.HasValue)
                    {
                        this.SetRowCell(rowIndex, col++, double.Parse(item.HuLotSize.Value.ToString()));
                    }
                    else
                    {
                        col++;
                    }
                    //类型一	
                    if (item.Category1 != null)
                    {
                        this.SetRowCell(rowIndex, col++, item.Category1.Code);
                    }
                    else
                    {
                        col++;
                    }

                    //类型二	
                    if (item.Category2 != null)
                        this.SetRowCell(rowIndex, col++, item.Category2.Code);
                    else
                    {
                        col++;
                    }
                    //类型三		
                    if (item.Category3 != null)
                        this.SetRowCell(rowIndex, col++, item.Category3.Code);
                    else
                    {
                        col++;
                    }
                    //类型四	
                    if (item.Category4 != null)
                        this.SetRowCell(rowIndex, col++, item.Category4.Code);
                    else
                    {
                        col++;
                    }

                    //损耗率	
                    if (item.ScrapPercentage.HasValue)
                    {
                        this.SetRowCell(rowIndex, col++, double.Parse(item.ScrapPercentage.Value.ToString("0.########")));
                    }
                    else
                    {
                        col++;
                    }
                    //引脚数	
                    if (item.PinNumber.HasValue)
                    {
                        this.SetRowCell(rowIndex, col++, double.Parse(item.PinNumber.Value.ToString("0.########")));
                    }
                    else
                    {
                        col++;
                    }

                    //	损耗单价		
                    if (item.ScrapPrice.HasValue)
                        this.SetRowCell(rowIndex, col++, double.Parse(item.ScrapPrice.Value.ToString("0.########")));
                    else
                    {
                        col++;
                    }

                    //历史价格		
                    if (item.HistoryPrice.HasValue)
                        this.SetRowCell(rowIndex, col++, double.Parse(item.HistoryPrice.Value.ToString("0.########")));
                    else
                    {
                        col++;
                    }

                    //销售成本		
                    if (item.SalesCost.HasValue)
                        this.SetRowCell(rowIndex, col++, double.Parse(item.SalesCost.Value.ToString("0.########")));
                    else
                    {
                        col++;
                    }

                    //定点路线
                    this.SetRowCell(rowIndex, col++, item.DefaultSupplier);

                    rowIndex++;
                }

                //this.sheet.DisplayGridlines = false;
                this.sheet.IsPrintGridlines = false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        /*
         * 填充报表头
         * 
         * Param repack 报验单头对象
         */
        private void FillHead(Item item)
        {

        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {

        }

        public override IList<object> GetDataList(string code)
        {
            IList<object> list = new List<object>();

            return list;
        }
    }
}




#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepItemMgrE : com.Sconit.Service.Report.Impl.RepItemMgr, IReportBaseMgrE
    {

    }
}

#endregion
