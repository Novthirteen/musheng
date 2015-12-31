using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.Quote;
using NCalc;
using System.Web.UI.HtmlControls;

public partial class Quote_CusTemplate_Search : SearchModuleBase
{
    public EventHandler NewEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.tbPartyFrom.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS + ",string:" + this.CurrentUser.Code;
        this.tbPartyFrom.DataBind();
        this.txtCostCategory.ServiceParameter = "string:" + this.CurrentUser.Code;
        this.txtCostCategory.DataBind();

        //#region Load
        if (txtCostCategory.Text.Trim() != string.Empty && tbPartyFrom.Text.Trim() != string.Empty)
        {
            //cblCostList.DataSource = TheToolingMgr.GetCostListByCCId(txtCostCategory.Text.Trim());
            //cblCostList.DataTextField = "Name";
            //cblCostList.DataValueField = "Id";
            //cblCostList.DataBind();

            IList<CusTemplate> cusTemplateList = TheToolingMgr.GetCostListByCusCodeAndCCId(tbPartyFrom.Text.Trim(), txtCostCategory.Text.Trim());
        //    //var nn = cusTemplateList1.OrderBy(n => n.SortId);
        //    //IList<CusTemplate> cusTemplateList = nn.ToList<CusTemplate>();
        //    if (cusTemplateList.Count > 0)
        //    {
        //        foreach (CusTemplate cusTemplate in cusTemplateList)
        //        {
        //            for (int i = 0; i < cblCostList.Items.Count; i++)
        //            {
        //                if (cblCostList.Items[i].Value == cusTemplate.CostList.Id.ToString())
        //                {
        //                    cblCostList.Items[i].Selected = true;
        //                }
        //            }
        //        }
        //    }
        #region
            IList<CostList> cl = TheToolingMgr.GetCostListByCCId(txtCostCategory.Text.Trim());
            for (int j = 0; j < cl.Count; j++)
        {
            #region Sort
            HtmlGenericControl div = new HtmlGenericControl();
            div.TagName = "div";
            div.Attributes.CssStyle.Add("margin-top", "2px");

            TextBox txt = new TextBox();
            txt.ID = "txt" + j.ToString();
            //if (cblCostList.Items[j].Selected && cblCostList.Items[j].Value == cusTemplateList[j].CostList.Id.ToString())
            //{
            //    txt.Text = cusTemplateList[j].SortId.ToString();
            //}
            //else
            //{
            //    txt.Enabled = false;
            //}
            txt.Width = 35;

            CompareValidator rev = new CompareValidator();
            rev.ControlToValidate = txt.ID;
            rev.Type = ValidationDataType.Integer;
            rev.Operator = ValidationCompareOperator.GreaterThan;
            rev.ValueToCompare = "0";
            rev.ErrorMessage = "大于0的数字";

            div.Controls.Add(txt);
            div.Controls.Add(rev);
            txtList.Controls.Add(div);
            #endregion
        }
        #endregion
        }
        //#endregion
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    { }

    protected override void DoSearch()
    { }

    public void btnSearch_Click(object sender, EventArgs e)
    {
        #region Load
        if (txtCostCategory.Text.Trim() != string.Empty && tbPartyFrom.Text.Trim() != string.Empty)
        {
            cblCostList.DataSource = TheToolingMgr.GetCostListByCCId(txtCostCategory.Text.Trim());
            cblCostList.DataTextField = "Name";
            cblCostList.DataValueField = "Id";
            cblCostList.DataBind();

            IList<CusTemplate> cusTemplateList = TheToolingMgr.GetCostListByCusCodeAndCCId(tbPartyFrom.Text.Trim(), txtCostCategory.Text.Trim());
            //var nn = cusTemplateList1.OrderBy(n => n.SortId);
            //IList<CusTemplate> cusTemplateList = nn.ToList<CusTemplate>();
            if (cusTemplateList.Count > 0)
            {
                foreach (CusTemplate cusTemplate in cusTemplateList)
                {
                    for (int i = 0; i < cblCostList.Items.Count; i++)
                    {
                        if (cblCostList.Items[i].Value == cusTemplate.CostList.Id.ToString())
                        {
                            cblCostList.Items[i].Selected = true;
                        }
                    }
                }
            }
            //#region
            //for (int j = 0; j < cblCostList.Items.Count; j++)
            //{
            //    #region Sort
            //    HtmlGenericControl div = new HtmlGenericControl();
            //    div.TagName = "div";
            //    div.Attributes.CssStyle.Add("margin-top", "2px");

            //    TextBox txt = new TextBox();
            //    txt.ID = "txt" + j.ToString();
            //    txt.Text = cusTemplateList[j].SortId.ToString();
            //    txt.Width = 35;

            //    CompareValidator rev = new CompareValidator();
            //    rev.ControlToValidate = txt.ID;
            //    rev.Type = ValidationDataType.Integer;
            //    rev.Operator = ValidationCompareOperator.GreaterThan;
            //    rev.ValueToCompare = "0";
            //    rev.ErrorMessage = "大于0的数字";

            //    div.Controls.Add(txt);
            //    div.Controls.Add(rev);
            //    txtList.Controls.Add(div);
            //    #endregion
            //}
            //#endregion
            #region Load Sort
            for(int j = 0; j < cblCostList.Items.Count; j++)
            {
                TextBox txt = txtList.FindControl("txt" + j.ToString()) as TextBox;
                if(cblCostList.Items[j].Selected)
                {
                    foreach(CusTemplate cusTemplate in cusTemplateList)
                    {
                        if (cblCostList.Items[j].Value == cusTemplate.CostList.Id.ToString())
                        {
                            txt.Text = cusTemplate.SortId.ToString();
                        }
                    }
                }
                //else
                //{
                //    txt.Enabled = false;
                //}
            }
            #endregion
        }
        #endregion
        
    }

    public void btnNew_Click(object sender, EventArgs e)
    {
        Expression ee = new Expression("if((1=1 and 1=1),1,0)");
        Response.Write(ee.Evaluate());
        //NewEvent(txtCostCategory.Text,e);
    }

    public void btnSave_Click(object sender, EventArgs e)
    {
        if(cblCostList.Items.Count > 0)
        {
            for (int i = 0; i < cblCostList.Items.Count; i++)
            {
                TextBox txt = txtList.FindControl("txt" + i.ToString()) as TextBox;
                IList<CusTemplate> ctList = TheToolingMgr.GetCostListByIdAndCusCodeAndCCId(cblCostList.Items[i].Value, tbPartyFrom.Text.Trim(), txtCostCategory.Text.Trim());
                if (ctList.Count > 0)
                {
                    if (!cblCostList.Items[i].Selected)
                    {
                        TheToolingMgr.DeleteCusTemplate(ctList[0]);
                    }
                    else
                    {
                        if (txt.Text == string.Empty || txt.Text == null)
                        {
                            txt.Text = "0";
                        }
                        else
                        {
                            ctList[0].SortId = Int32.Parse(txt.Text);
                        }
                        TheToolingMgr.UpdateCusTemplate(ctList[0]);
                    }
                }
                else
                {
                    if (cblCostList.Items[i].Selected)
                    {
                        CusTemplate ct = new CusTemplate();
                        ct.CustomerCode = tbPartyFrom.Text.Trim();
                        //ct.CustomerName = tbPartyFrom.DescField;
                        CostCategory cc = new CostCategory();
                        cc.Id = Int32.Parse(txtCostCategory.Text.Trim());
                        ct.CostCategory = cc;
                        CostList cl = new CostList();
                        cl.Id = Int32.Parse(cblCostList.Items[i].Value);
                        ct.CostList = cl;
                        if (txt.Text == string.Empty || txt.Text == null)
                        {
                            txt.Text = "0";
                        }
                        else
                        {
                            ct.SortId = Int32.Parse(txt.Text);
                        }
                        TheToolingMgr.CreateCusTemplate(ct);
                    }
                }
            }
        }
    }
}