﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData.Impl;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Distribution;
using NHibernate.Expression;

public partial class MasterData_Flow_View : ModuleBase
{
    public event EventHandler BackEvent;

    public string ModuleType
    {
        get
        {
            return (string)ViewState["ModuleType"];
        }
        set
        {
            ViewState["ModuleType"] = value;
        }
    }

    protected string FlowCode
    {
        get
        {
            return (string)ViewState["FlowCode"];
        }
        set
        {
            ViewState["FlowCode"] = value;
        }
    }

    public void InitPageParameter(string flowCode)
    {
        this.FlowCode = flowCode;
        this.ucFlow.InitPageParameter(flowCode);
        this.ucStrategy.InitPageParameter(flowCode);
        IDictionary<string, string> mpDic = new Dictionary<string, string>();
        mpDic.Add("FlowCode", this.FlowCode);
        Flow currentFlow = TheFlowMgr.LoadFlow(flowCode, true, true, true);
        if (currentFlow.FlowDetails.Count > 0)
        {
            this.fdDetail.Visible = true;
            this.ucDetailSearch.QuickSearch(mpDic);
        }
        if (currentFlow.Facilitys.Count > 0)
        {
            this.fdFacility.Visible = true;
            this.ucFacilitySearch.QuickSearch(mpDic);
        }
        if (currentFlow == null && currentFlow.ReturnRouting == null)
        {
            this.fdProductionRouting.Visible = false;
        }
        else
        {
            if (currentFlow.Routing != null)
            {
                this.fdRouting.Visible = true;
                this.ucRouting.InitPageParameter(flowCode, false);
            }
            if (currentFlow.ReturnRouting != null)
            {
                this.fdReturn.Visible = true;
                this.ucReturnRouting.InitPageParameter(flowCode, true);
            }
        }
        this.ucLocTrans.FlowCode = flowCode;
        this.ucLocTrans.InitPageParameter();

      

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        this.ucDetailSearch.SearchEvent += new System.EventHandler(this.Search_Render);
        this.ucFacilitySearch.SearchEvent += new System.EventHandler(this.FacilitySearch_Render);
        this.ucRouting.EditEvent += new System.EventHandler(this.Edit_Render);
        this.ucDetailList.ListViewEvent += new System.EventHandler(this.View_Render);
        this.ucFacilityList.ListViewEvent += new System.EventHandler(this.FacilityView_Render);
        this.ucViewDetail.BackEvent += new System.EventHandler(this.Back_Render);
        this.ucViewFacility.BackEvent += new System.EventHandler(this.FacilityBack_Render);
        this.ucReturnRouting.EditEvent += new System.EventHandler(this.Edit_Render);
        if (!IsPostBack)
        {
            this.ucFlow.ModuleType = this.ModuleType;
            this.ucStrategy.ModuleType = this.ModuleType;
            this.ucDetailSearch.ModuleType = this.ModuleType;
            this.ucDetailList.ModuleType = this.ModuleType;
            this.ucViewDetail.ModuleType = this.ModuleType;
            this.ucRouting.ModuleType = this.ModuleType;
        }
    }

    void Search_Render(object sender, EventArgs e)
    {
        this.ucDetailList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
        this.ucDetailList.Visible = true;
        this.ucDetailList.UpdateView();
    }

    void FacilitySearch_Render(object sender, EventArgs e)
    {
        this.ucFacilityList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
        this.ucFacilityList.Visible = true;
        this.ucFacilityList.UpdateView();
    }
    

    void Edit_Render(object sender, EventArgs e)
    {

        if (!(bool)((object[])sender)[2])
        {
            this.ucRoutingList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
            this.ucRouting.Visible = true;
            this.ucRoutingList.Visible = true;
            this.ucRoutingList.UpdateView();
        }
        else
        {
            this.ucReturnRoutingList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
            this.ucReturnRouting.Visible = true;
            this.ucReturnRoutingList.Visible = true;
            this.ucReturnRoutingList.UpdateView();
        }

    }

    //The event handler when user view itemflowdetail
    void View_Render(object sender, EventArgs e)
    {
        this.ucViewDetail.Visible = true;
        this.ucViewDetail.FlowCode = this.FlowCode;
        this.ucViewDetail.InitPageParameter((Int32)sender);
    }

    void FacilityView_Render(object sender, EventArgs e)
    {
        this.ucViewFacility.Visible = true;
        this.ucViewFacility.FlowCode = this.FlowCode;
        this.ucViewFacility.InitPageParameter((Int32)sender);
    }
    

    void Back_Render(object sender, EventArgs e)
    {
        this.ucViewDetail.Visible = false;
    }

    void FacilityBack_Render(object sender, EventArgs e)
    {
        this.ucViewFacility.Visible = false;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }
}
