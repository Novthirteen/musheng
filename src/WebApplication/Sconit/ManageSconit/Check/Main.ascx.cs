using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using NHibernate.Expression;
using com.Sconit.Entity.View;
using com.Sconit.Entity;

public partial class ManageSconit_Check_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnUom_Click(object sender, EventArgs e)
    {
        IList<Item> items = TheItemMgr.GetAllItem();
        IList<BomDetail> bomDetails = TheBomDetailMgr.GetAllBomDetail();
        IList<UomConversion> uomConversions = TheUomConversionMgr.GetAllUomConversion();
        IList<PriceListDetail> priceListDetails = ThePriceListDetailMgr.GetAllPriceListDetail();
        IList<FlowDetail> flowDetails = TheFlowDetailMgr.GetAllFlowDetail();

        //BomDetail
        var q_bd = bomDetails.Join(items, d => d.Item.Code, i => i.Code, (d, i) => new { d, i }).Where(q1 => q1.d.Uom.Code != q1.i.Uom.Code &&
                 (uomConversions.Where(u => u.AlterUom.Code == q1.d.Uom.Code && u.BaseUom.Code == q1.i.Uom.Code && (u.Item == null || u.Item.Code == q1.i.Code)).Count() == 0
               && uomConversions.Where(u => u.BaseUom.Code == q1.d.Uom.Code && u.AlterUom.Code == q1.i.Uom.Code && (u.Item == null || u.Item.Code == q1.i.Code)).Count() == 0))
               .Select(q2 => new UomCheckView { Code = q2.d.Bom.Code, Description = q2.d.Bom.Description, Type = "BomDetail", Item = q2.i, AlterUom = q2.d.Uom })
               .ToList();

        //FlowDetail
        var q_fd = flowDetails.Join(items, d => d.Item.Code, i => i.Code, (d, i) => new { d, i }).Where(q1 => q1.d.Uom.Code != q1.i.Uom.Code &&
                 (uomConversions.Where(u => u.AlterUom.Code == q1.d.Uom.Code && u.BaseUom.Code == q1.i.Uom.Code && (u.Item == null || u.Item.Code == q1.i.Code)).Count() == 0
               && uomConversions.Where(u => u.BaseUom.Code == q1.d.Uom.Code && u.AlterUom.Code == q1.i.Uom.Code && (u.Item == null || u.Item.Code == q1.i.Code)).Count() == 0))
               .Select(q2 => new UomCheckView { Code = q2.d.Flow.Code, Description = q2.d.Flow.Description, SubType = q2.d.Flow.Type, Type = "FlowDetail", Item = q2.i, AlterUom = q2.d.Uom })
               .ToList();

        //PriceListDetail
        var q_pd = priceListDetails.Join(items, d => d.Item.Code, i => i.Code, (d, i) => new { d, i }).Where(q1 => q1.d.Uom.Code != q1.i.Uom.Code &&
                 (uomConversions.Where(u => u.AlterUom.Code == q1.d.Uom.Code && u.BaseUom.Code == q1.i.Uom.Code && (u.Item == null || u.Item.Code == q1.i.Code)).Count() == 0
               && uomConversions.Where(u => u.BaseUom.Code == q1.d.Uom.Code && u.AlterUom.Code == q1.i.Uom.Code && (u.Item == null || u.Item.Code == q1.i.Code)).Count() == 0))
               .Select(q2 => new UomCheckView { Code = q2.d.PriceList.Code, Description = q2.d.PriceList.Party.Name, SubType = q2.d.PriceList.Party.Type, Type = "PriceListDetail", Item = q2.i, AlterUom = q2.d.Uom })
               .ToList();


        //BomMstr 取值item  
        var q_bm1 = bomDetails.Select(b => b.Bom).Distinct().Join(items, d => d.Code, i => i.DefaultBomCode, (d, i) => new { d, i }).Where(q1 => q1.d.Uom.Code != q1.i.Uom.Code &&
                 (uomConversions.Where(u => u.AlterUom.Code == q1.d.Uom.Code && u.BaseUom.Code == q1.i.Uom.Code && (u.Item == null || u.Item.Code == q1.i.Code)).Count() == 0
               && uomConversions.Where(u => u.BaseUom.Code == q1.d.Uom.Code && u.AlterUom.Code == q1.i.Uom.Code && (u.Item == null || u.Item.Code == q1.i.Code)).Count() == 0))
               .Select(q2 => new UomCheckView { Code = q2.d.Code, Description = q2.d.Description, Type = "BomMaster", Item = q2.i, AlterUom = q2.d.Uom })
               .ToList();


        //BomMstr 取值 flowdet 
        var q_bm2 = bomDetails.Select(b => b.Bom).Distinct().Join(flowDetails.Where(f => f.Bom != null), d => d.Code, i => i.Bom.Code, (d, i) => new { d, i }).Where(q1 => q1.d.Uom.Code != q1.i.Uom.Code &&
                 (uomConversions.Where(u => u.AlterUom.Code == q1.d.Uom.Code && u.BaseUom.Code == q1.i.Uom.Code && (u.Item == null || u.Item.Code == q1.i.Item.Code)).Count() == 0
               && uomConversions.Where(u => u.BaseUom.Code == q1.d.Uom.Code && u.AlterUom.Code == q1.i.Uom.Code && (u.Item == null || u.Item.Code == q1.i.Item.Code)).Count() == 0))
               .Select(q2 => new UomCheckView { Code = q2.d.Code, Description = q2.d.Description, Type = "BomMaster", Item = q2.i.Item, AlterUom = q2.d.Uom })
               .ToList();

        q_bd.AddRange(q_fd);
        q_bd.AddRange(q_pd);
        q_bd.AddRange(q_bm1);
        q_bd.AddRange(q_bm2);

        this.GV_List.DataSource = q_bd.Take(1000);
        this.GV_List.DataBind();
        this.fldList.Visible = q_bd.Count > 0;
    }

    protected void btnFlowDetail_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    DetachedCriteria criteria = DetachedCriteria.For(typeof(FlowView));
        //    criteria.CreateAlias("Flow", "f");
        //    criteria.Add(Expression.Eq("f.Type", BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION));
        //    IList<FlowView> list = TheCriteriaMgr.FindAll<FlowView>(criteria);

        //    foreach (FlowView flowView in list)
        //    {
        //        TheSupplyChainMgr.GenerateSupplyChain(flowView.Flow.Code, flowView.FlowDetail.Item.Code);
        //    }
        //}
        //catch (BusinessErrorException ex)
        //{
        //    ShowErrorMessage(ex);
        //    return;
        //}
        ShowSuccessMessage("尚未实现");
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        { }
    }
    class UomCheckView
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string SubType { get; set; }
        public string Type { get; set; }
        public Item Item { get; set; }
        public Uom AlterUom { get; set; }
    }

}
