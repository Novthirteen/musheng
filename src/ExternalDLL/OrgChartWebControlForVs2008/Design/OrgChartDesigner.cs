using System;
using System.Web.UI.Design;
using System.Web.UI;
using System.IO ;
using System.Web.UI.Design.WebControls;


namespace Whidsoft.WebControls.Design
{

	public class OrgChartDesigner : ControlDesigner 
	{
    
		/// <summary>
		/// 初始化 PagerDesigner 的新实例。
		/// </summary>
		public OrgChartDesigner()
		{
			//this.ReadOnly=true;
		}
		private OrgChart wb;


		protected override string GetEmptyDesignTimeHtml() 
		{
			return GetDesignTimeHtml();
		}

		public override string GetDesignTimeHtml() 
		{

			StringWriter sw = new StringWriter();
			HtmlTextWriter writer = new HtmlTextWriter(sw);

			//HtmlTextWriterStyle.Width
			//writer.AddStyleAttribute ( HtmlTextWriterStyle.Width ,);
			
			wb=(OrgChart)Component;
			//wb.RecordCount=225;
			wb.RenderControl(writer);
			//wb.RenderEndTag (writer);

			/*
			writer.Write("<table bgcolor=silver width=100% cellpadding=2 cellspacing=0 border=1 style='border-collapse:collapse;'>");
			writer.Write("<tr><td valign='top'>");
			writer.Write("<div>组织结构图</div>");
			writer.Write("</td><tr>");
			writer.Write("<tr><td valign='top'>");
			writer.Write("Version 1.0.0.0,2004.10," + this.AllowResize.ToString () );
			writer.Write("</td><tr>");
			writer.Write("</td></tr></table>");
			*/

			return sw.ToString();

		}

		/// <summary>
		/// 获取在呈现控件时遇到错误后在设计时为指定的异常显示的 HTML。
		/// </summary>
		/// <param name="e">要为其显示错误信息的异常。</param>
		/// <returns>设计时为指定的异常显示的 HTML。</returns>
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			string errorstr="创建控件时出错："+ e.Message;
			return CreatePlaceHolderDesignTimeHtml(errorstr);
		}

	}

}
