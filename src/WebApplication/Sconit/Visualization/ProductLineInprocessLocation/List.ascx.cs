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
using com.Sconit.Web;
using com.Sconit.Service.MasterData;
using com.Sconit.Entity.Exception;
using NHibernate.Expression;
using com.Sconit.Service.Criteria;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using System.Text;

public partial class Visualization_ProductLineInprocessLocation_List : ListModuleBase
{
    

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override void UpdateView()
    {
        if (!IsExport)
        {
            this.GV_List.Execute();
        }
        else
        {
            this.Export();
        }
    }

    public void Export()
    {
        string dateTime = DateTime.Now.ToString("ddhhmmss");
        this.ExportXLS(this.GV_List, "ProductLineInprocessLocation" + dateTime + ".xls");
    }
}
