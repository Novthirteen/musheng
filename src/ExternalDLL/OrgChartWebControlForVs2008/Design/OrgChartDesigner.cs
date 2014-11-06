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
		/// ��ʼ�� PagerDesigner ����ʵ����
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
			writer.Write("<div>��֯�ṹͼ</div>");
			writer.Write("</td><tr>");
			writer.Write("<tr><td valign='top'>");
			writer.Write("Version 1.0.0.0,2004.10," + this.AllowResize.ToString () );
			writer.Write("</td><tr>");
			writer.Write("</td></tr></table>");
			*/

			return sw.ToString();

		}

		/// <summary>
		/// ��ȡ�ڳ��ֿؼ�ʱ��������������ʱΪָ�����쳣��ʾ�� HTML��
		/// </summary>
		/// <param name="e">ҪΪ����ʾ������Ϣ���쳣��</param>
		/// <returns>���ʱΪָ�����쳣��ʾ�� HTML��</returns>
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			string errorstr="�����ؼ�ʱ����"+ e.Message;
			return CreatePlaceHolderDesignTimeHtml(errorstr);
		}

	}

}
