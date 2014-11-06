using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using NHibernate;
using com.Sconit.Utility;
using System.Collections.Generic;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for ListModuleBase
/// </summary>
namespace com.Sconit.Web
{
    public abstract class ListModuleBase : ModuleBase
    {

        public bool IsExport 
        {
            get { return ViewState["IsExport"] == null ? false : (bool)ViewState["IsExport"]; }
            set { ViewState["IsExport"] = value; }
        }


        public ListModuleBase()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected ICriteriaMgrE CriteriaMgr
        {
            get
            {
                return GetService<ICriteriaMgrE>("CriteriaMgr.service");
            }
        }

        public void SetSearchCriteria(DetachedCriteria SelectCriteria, DetachedCriteria SelectCountCriteria)
        {
            new SessionHelper(this.Page).AddUserSelectCriteria(this.TemplateControl.AppRelativeVirtualPath, SelectCriteria, SelectCountCriteria);
        }

        public void SetSearchCriteria(DetachedCriteria SelectCriteria, DetachedCriteria SelectCountCriteria, IDictionary<string, string> alias)
        {
            new SessionHelper(this.Page).AddUserSelectCriteria(this.TemplateControl.AppRelativeVirtualPath, SelectCriteria, SelectCountCriteria, alias);
        }


        public virtual void UpdateView()
        { 
        
        }

       
    }
}
