using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

/// <summary>
///GridViewTemplate 的摘要说明
/// </summary>
namespace com.Sconit.Web
{
    public class GridViewTemplate : ITemplate
    {
        private DataControlRowType templateType;
        private object control;
        private string columnName;

        public GridViewTemplate(DataControlRowType type, string colname)
        {
            templateType = type;
            columnName = colname;
        }

        public GridViewTemplate(DataControlRowType type, object control, string colname)
        {
            templateType = type;
            columnName = colname;
            this.control = control;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            // Create the content for the different row types.
            switch (templateType)
            {
                case DataControlRowType.Header:
                    // Create the controls to put in the header
                    // section and set their properties.
                    Literal lc = new Literal();
                    lc.Text = "<b>" + columnName + "</b>";

                    // Add the controls to the Controls collection
                    // of the container.
                    container.Controls.Add(lc);
                    break;
                case DataControlRowType.DataRow:
                    switch (control.ToString())
                    {
                        case "System.Web.UI.WebControls.Label":
                            Label label = new Label();
                            label.DataBinding += new EventHandler(this.Label_DataBinding);
                            container.Controls.Add(label);
                            break;
                        case "System.Web.UI.WebControls.LinkButton":
                            LinkButton linkButton = new LinkButton();
                            linkButton.DataBinding += new EventHandler(this.LinkButton_DataBinding);
                            linkButton.Click += new EventHandler(this.LinkButtonClick_DataBinding);
                            container.Controls.Add(linkButton);
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }
        }

        private void Label_DataBinding(Object sender, EventArgs e)
        {
            Label l = (Label)sender;
            GridViewRow row = (GridViewRow)l.NamingContainer;
            l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();

            l.ID = "lbl" + columnName;
        }

        private void LinkButton_DataBinding(Object sender, EventArgs e)
        {
            LinkButton l = (LinkButton)sender;
            GridViewRow row = (GridViewRow)l.NamingContainer;
            l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();
            l.ID = "lbn" + columnName;
        }

        public virtual void LinkButtonClick_DataBinding(Object sender, EventArgs e)
        {
            //
        }
    }
}
