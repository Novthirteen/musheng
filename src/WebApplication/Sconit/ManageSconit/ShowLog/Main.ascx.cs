using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Web;
using com.Sconit.Entity;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using com.Sconit.Utility;

public partial class ManageSconit_ShowLog_Main : MainModuleBase
{
    private string logDir
    {
        get
        {
            string companyCode = TheEntityPreferenceMgr.LoadEntityPreference("CompanyCode").Value;
            string dir = companyCode.ToLower().Contains("test") ? "test" : string.Empty;
            if (this.rblType.SelectedIndex == 0)
            {
                return @"D:\logs\Web" + dir + @"\";
            }
            else
            {
                return @"D:\logs\windowsservice\";
            }
        }
    }

    private List<Logview> logviews
    {
        get
        {
            FileStream fs = new FileStream(logDir + ddlLogFile.SelectedValue, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            string log = sr.ReadToEnd();
            string[] lines = Regex.Split(log, "\r\n", RegexOptions.IgnoreCase);

            List<Logview> logviews_ = new List<Logview>();
            foreach (string line in lines)
            {
                if (!line.Contains("|") && logviews_.Count > 0)
                {
                    logviews_.Last().Exception += line.Trim();
                }
                else if (line.Split('|').Count() > 4)
                {
                    Logview logview_ = new Logview();
                    logview_.Date = DateTime.Parse(line.Split('|')[0]);
                    logview_.Thread = line.Split('|')[1].Trim();
                    logview_.Level = line.Split('|')[2].Trim();
                    logview_.Logger = line.Split('|')[3].Trim();
                    logview_.Message = line.Split('|')[4].Trim();
                    logviews_.Add(logview_);
                }
            }
            fs.Dispose();
            return logviews_;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.rblType_Change(sender, e);
            this.ddlLogFile_Change(sender, e);
            this.tbDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
    }

    protected void rblType_Change(object sender, EventArgs e)
    {
        DirectoryInfo di = new System.IO.DirectoryInfo(logDir);
        FileInfo[] fimore = di.GetFiles();
        this.ddlLogFile.DataSource = fimore.Select(f => f.Name);
        this.ddlLogFile.DataBind();

    }
    protected void ddlLogFile_Change(object sender, EventArgs e)
    {
        List<string> loggers = logviews.GroupBy(l => l.Logger).Select(l => l.Key).ToList();
        loggers.Add("ALL");
        loggers.Sort();
        this.ddlLogger.DataSource = loggers;
        this.ddlLogger.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var newLogView = logviews.Where(l => (ddlLevel.SelectedValue == "ALL" || StringHelper.Eq(l.Level.Trim(), ddlLevel.SelectedValue.Trim()))
            && (ddlLogger.SelectedValue == "ALL" || StringHelper.Eq(l.Logger.Trim(), ddlLogger.SelectedValue.Trim()))
            && (this.tbDateTime.Text == string.Empty || l.Date > DateTime.Parse(this.tbDateTime.Text)));

        this.GV_List.DataSource = newLogView.OrderByDescending(l => l.Date).Take(1000);
        this.GV_List.DataBind();
        this.fldList.Visible = newLogView.Count() > 0;
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    class Logview
    {
        public DateTime Date { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
