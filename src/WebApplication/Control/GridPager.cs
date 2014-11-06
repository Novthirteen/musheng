﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Web.Compilation;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using com.Sconit.Entity.Exception;

/// <summary>
/// Summary description for PageController
/// </summary>
namespace com.Sconit.Control
{
    #region GridPager Server Control

    [DefaultProperty("PageSize")]
    [DefaultEvent("PageChanged")]
    [ParseChildren(false)]
    [PersistChildren(false)]
    [Description("专用于ASP.Net Web应用程序的分页控件")]
    [Designer(typeof(PanelContainerDesigner))]
    [ToolboxData("<{0}:GridPager runat=server></{0}:GridPager>")]
    public class GridPager : Panel, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        private string cssClassName;
        private string urlPageIndexName = "page";
        private bool urlPaging = false;
        private string inputPageIndex;
        private string currentUrl = null;
        private NameValueCollection urlParams = null;

        #region Properties

        #region Navigation Buttons

        /// <summary>
        /// 获取或设置一个值，该值批示当鼠标指针悬停在导航按钮上时是否显示工具提示。
        /// </summary>
        [Browsable(true),
            Category("导航按钮"),
            DefaultValue(true),
            Description("指定当鼠标停留在导航按钮上时，是否显示工具提示")]
        public bool ShowNavigationToolTip
        {
            get
            {
                object obj = ViewState["ShowNavigationToolTip"];
                return (obj == null) ? true : (bool)obj;
            }
            set
            {
                ViewState["ShowNavigationToolTip"] = value;
            }
        }

        /// <summary>
        /// 获取或设置导航按钮工具提示文本的格式。
        /// </summary>
        [Browsable(true),
            Category("导航按钮"),
            DefaultValue("转到第{0}页"),
            Description("页导航按钮工具提示文本的格式")]
        public string NavigationToolTipTextFormatString
        {
            get
            {
                object obj = ViewState["NavigationToolTipTextFormatString"];
                return (obj == null) ? "转到第{0}页" : (string)obj;
            }
            set
            {
                string tip = value;
                if (tip.Trim().Length < 1 && tip.IndexOf("{0}") < 0)
                    tip = "{0}";
                ViewState["NavigationToolTipTextFormatString"] = tip;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否将页索引按钮用中文数字代替。
        /// </summary>
        /// <remarks>
        /// 将该值设为true并且未使用图片按钮时，页索引按钮中的数值1、2、3等将会被中文字符一、二、三等代替。
        /// </remarks>
        [Browsable(true),
            Category("导航按钮"),
            DefaultValue(false),
            Description("是否将页索引数值按钮用中文数字一、二、三等代替")]
        public bool ChinesePageIndex
        {
            get
            {
                object obj = ViewState["ChinesePageIndex"];
                return (obj == null) ? false : (bool)obj;
            }
            set
            {
                ViewState["ChinesePageIndex"] = value;
            }
        }

        /// <summary>
        /// 获取或设置页索引数值导航按钮上文字的显示格式。
        /// </summary>
        /// <value>
        /// 字符串，指定页索引数值按钮上文字的显示格式，默认值为<see cref="String.Empty"/>，即未设置该属性。</value>
        /// <remarks>
        /// 使用NumericButtonTextFormatString属性指定页索引数值按钮的显示格式，如未设置该值时索引按钮文本将会是：1 2 3 ...，设置该值将改变索引按钮文本的显示格式，
        /// 如将该值设为“[{0}]”则索引文本会显示为：[1] [2] [3] ...，将该值设为“-{0}-”则会使索引文本变为：-1- -2- -3- ...。
        /// </remarks>
        [Browsable(true),
            DefaultValue(""),
            Category("导航按钮"),
            Description("页索引数值按钮上文字的显示格式")]
        public string NumericButtonTextFormatString
        {
            get
            {
                object obj = ViewState["NumericButtonTextFormatString"];
                return (obj == null) ? String.Empty : (string)obj;
            }
            set
            {
                ViewState["NumericButtonTextFormatString"] = value;
            }
        }

        /// <summary>
        /// 获取或设置分页导航按钮的类型，即使用文字还是图片。
        /// </summary>
        [Browsable(true),
            DefaultValue(PagingButtonType.Text),
            Category("导航按钮"),
            Description("分页导航按钮的类型，是使用文字还是图片")]
        public PagingButtonType PagingButtonType
        {
            get
            {
                object obj = ViewState["PagingButtonType"];
                return (obj == null) ? PagingButtonType.Text : (PagingButtonType)obj;
            }
            set
            {
                ViewState["PagingButtonType"] = value;
            }
        }

        /// <summary>
        /// 获取或设置页导航数值按钮的类型，该值仅当PagingButtonType设为Image时才有效。
        /// </summary>
        /// <remarks>
        /// 当您将PagingButtonType设为Image当又不想让页索引数值按钮使用图片时，可以将该值设为Text，这会使页索引数据按钮使用文本而不是图片按钮。
        /// </remarks>
        [Browsable(true),
            DefaultValue(PagingButtonType.Text),
            Category("导航按钮"),
            Description("页导航数值按钮的类型")]
        public PagingButtonType NumericButtonType
        {
            get
            {
                object obj = ViewState["NumericButtonType"];
                return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
            }
            set
            {
                ViewState["NumericButtonType"] = value;
            }
        }

        /// <summary>
        /// 获取或设置第一页、上一页、下一页和最后一页按钮的类型，该值仅当PagingButtonType设为Image时才有效。
        /// </summary>
        /// <remarks>
        /// 当您将PagingButtonType设为Image但又不想让第一页、下一页、下一页和最后一页按钮使用图片，则可以将该值设为Text，这会使前面的四个按钮使用文本而不是图片按钮。
        /// </remarks>
        [Browsable(true),
            Category("导航按钮"),
            DefaultValue(PagingButtonType.Text),
            Description("第一页、上一页、下一页和最后一页按钮的类型")]
        public PagingButtonType NavigationButtonType
        {
            get
            {
                object obj = ViewState["NavigationButtonType"];
                return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
            }
            set
            {
                ViewState["NavigationButtonType"] = value;
            }
        }

        /// <summary>
        /// 获取或设置“更多页”（...）按钮的类型，该值仅当PagingButtonType设为Image时才有效。
        /// </summary>
        /// <remarks>
        /// 当您将PagingButtonType设为Image但又不想让更多页（...）按钮使用图片时，可以将此值设为Text，这会使更多页按钮使用文本而不是图片按钮。
        /// </remarks>
        [Browsable(true),
            Category("导航按钮"),
            DefaultValue(PagingButtonType.Text),
            Description("“更多页”（...）按钮的类型")]
        public PagingButtonType MoreButtonType
        {
            get
            {
                object obj = ViewState["MoreButtonType"];
                return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
            }
            set
            {
                ViewState["MoreButtonType"] = value;
            }
        }

        /// <summary>
        /// 获取或设置分页导航按钮之间的间距。
        /// </summary>
        [Browsable(true),
            Category("导航按钮"),
            DefaultValue(typeof(Unit), "5px"),
            Description("分页导航按钮之间的间距")]
        public Unit PagingButtonSpacing
        {
            get
            {
                object obj = ViewState["PagingButtonSpacing"];
                return (obj == null) ? Unit.Pixel(5) : (Unit.Parse(obj.ToString()));
            }
            set
            {
                ViewState["PagingButtonSpacing"] = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否在页导航元素中显示第一页和最后一页按钮。
        /// </summary>
        [Browsable(true),
            Description("是否在页导航元素中显示第一页和最后一页按钮"),
            Category("导航按钮"),
            DefaultValue(true)]
        public bool ShowFirstLast
        {
            get
            {
                object obj = ViewState["ShowFirstLast"];
                return (obj == null) ? true : (bool)obj;
            }
            set { ViewState["ShowFirstLast"] = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否在页导航元素中显示上一页和下一页按钮。
        /// </summary>
        [Browsable(true),
            Description("是否在页导航元素中显示上一页和下一页按钮"),
            Category("导航按钮"),
            DefaultValue(true)]
        public bool ShowPrevNext
        {
            get
            {
                object obj = ViewState["ShowPrevNext"];
                return (obj == null) ? true : (bool)obj;
            }
            set { ViewState["ShowPrevNext"] = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否在页导航元素中显示页索引数值按钮。
        /// </summary>
        [Browsable(true),
            Description("是否在页导航元素中显示数值按钮"),
            Category("导航按钮"),
            DefaultValue(true)]
        public bool ShowPageIndex
        {
            get
            {
                object obj = ViewState["ShowPageIndex"];
                return (obj == null) ? true : (bool)obj;
            }
            set { ViewState["ShowPageIndex"] = value; }
        }

        /// <summary>
        /// 获取或设置为第一页按钮显示的文本。
        /// </summary>
        [Browsable(true),
            Description("第一页按钮上显示的文本"),
            Category("导航按钮"),
            DefaultValue("首页")]
        public string FirstPageText
        {
            get
            {
                object obj = ViewState["FirstPageText"];
                return (obj == null) ? "${Common.GridView.FirstPage}" : (string)obj;
            }
            set { ViewState["FirstPageText"] = value; }
        }

        /// <summary>
        /// 获取或设置为上一页按钮显示的文本。
        /// </summary>
        [Browsable(true),
            Description("上一页按钮上显示的文本"),
            Category("导航按钮"),
            DefaultValue("上一页")]
        public string PrevPageText
        {
            get
            {
                object obj = ViewState["PrevPageText"];
                return (obj == null) ? "${Common.GridView.PrePage}" : (string)obj;
            }
            set { ViewState["PrevPageText"] = value; }
        }

        /// <summary>
        /// 获取或设置为下一页按钮显示的文本。
        /// </summary>
        [Browsable(true),
            Description("下一页按钮上显示的文本"),
            Category("导航按钮"),
            DefaultValue("下一页")]
        public string NextPageText
        {
            get
            {
                object obj = ViewState["NextPageText"];
                return (obj == null) ? "${Common.GridView.NextPage}" : (string)obj;
            }
            set { ViewState["NextPageText"] = value; }
        }

        /// <summary>
        /// 获取或设置为最后一页按钮显示的文本。
        /// </summary>
        [Browsable(true),
            Description("最后一页按钮上显示的文本"),
            Category("导航按钮"),
            DefaultValue("尾页")]
        public string LastPageText
        {
            get
            {
                object obj = ViewState["LastPageText"];
                return (obj == null) ? "${Common.GridView.LastPage}" : (string)obj;
            }
            set { ViewState["LastPageText"] = value; }
        }

        /// <summary>
        /// 获取或设置在 <see cref="AspNetPager"/> 控件的页导航元素中同时显示的数值按钮的数目。
        /// </summary>
        [Browsable(true),
            Description("要显示的页索引数值按钮的数目"),
            Category("导航按钮"),
            DefaultValue(10)]
        public int NumericButtonCount
        {
            get
            {
                object obj = ViewState["NumericButtonCount"];
                return (obj == null) ? 10 : (int)obj;
            }
            set { ViewState["NumericButtonCount"] = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指定是否显示已禁用的按钮。
        /// </summary>
        /// <remarks>
        /// 该值用来指定是否显示已禁用的分页导航按钮，当当前页为第一页时，第一页和上一页按钮将被禁用，当当前页为最后一页时，下一页和最后一页按钮将被禁用，被禁用的按钮没有链接，在按钮上点击也不会有任何作用。
        /// </remarks>
        [Browsable(true),
            Category("导航按钮"),
            Description("是否显示已禁用的按钮"),
            DefaultValue(true)]
        public bool ShowDisabledButtons
        {
            get
            {
                object obj = ViewState["ShowDisabledButtons"];
                return (obj == null) ? true : (bool)obj;
            }
            set
            {
                ViewState["ShowDisabledButtons"] = value;
            }
        }

        #endregion

        #region Image Buttons

        /// <summary>
        /// 获取或设置当使用图片按钮时，图片文件的路径。
        /// </summary>
        [Browsable(true),
            Category("图片按钮"),
            Description("当使用图片按钮时，指定图片文件的路径"),
            DefaultValue(null)]
        public string ImagePath
        {
            get
            {
                string imgPath = (string)ViewState["ImagePath"];
                if (imgPath != null)
                    imgPath = this.ResolveUrl(imgPath);
                return imgPath;
            }
            set
            {
                string imgPath = value.Trim().Replace("\\", "/");
                ViewState["ImagePath"] = (imgPath.EndsWith("/")) ? imgPath : imgPath + "/";
            }
        }

        /// <summary>
        /// 获取或设置当使用图片按钮时，图片的类型，如gif或jpg，该值即图片文件的后缀名。
        /// </summary>
        [Browsable(true),
            Category("图片按钮"),
            DefaultValue(".gif"),
            Description("当使用图片按钮时，图片的类型，如gif或jpg，该值即图片文件的后缀名")]
        public string ButtonImageExtension
        {
            get
            {
                object obj = ViewState["ButtonImageExtension"];
                return (obj == null) ? ".gif" : (string)obj;
            }
            set
            {
                string ext = value.Trim();
                ViewState["ButtonImageExtension"] = (ext.StartsWith(".")) ? ext : ("." + ext);
            }
        }

        /// <summary>
        /// 获取或设置自定义图片文件名的后缀字符串，以区分不同类型的按钮图片。
        /// </summary>
        /// <remarks><note>注意：</note>该值不是文件后缀名，而是为区分不同的图片文件而在图片名中加入的字符串，如：
        /// 当前有两套按钮图片，其中一套中的“1”的图片名可为“1f.gif”，另一套中的“1”的图片名可起为“1n.gif”，其中的f和n即为ButtonImageNameExtension。</remarks>
        [Browsable(true),
            DefaultValue(null),
            Category("图片按钮"),
            Description("自定义图片文件名的后缀字符串（非文件后缀名），如图片“1f.gif”的ButtonImageNameExtension即为“f”")]
        public string ButtonImageNameExtension
        {
            get
            {
                return (string)ViewState["ButtonImageNameExtension"];
            }
            set
            {
                ViewState["ButtonImageNameExtension"] = value;
            }
        }

        /// <summary>
        /// 获取或设置当前页索引按钮的图片名后缀。
        /// </summary>
        /// <remarks>
        /// 当 <see cref="PagingButtonType"/> 设为 Image 时，该属性允许您设置当前页索引数值按钮使用的图片名后缀字符，因此可以使当前页索引按钮与其它页索引按钮使用不同的图片，若未设置该值，则默认值为<see cref="ButtonImageNameExtension"/>，即当前页索引按钮与其它页索引按钮使用相同的图片。
        /// </remarks>
        [Browsable(true),
            DefaultValue(null),
            Category("图片按钮"),
            Description("当前页索引按钮的图片名后缀字符串")]
        public string CpiButtonImageNameExtension
        {
            get
            {
                object obj = ViewState["CpiButtonImageNameExtension"];
                return (obj == null) ? ButtonImageNameExtension : (string)obj;
            }
            set
            {
                ViewState["CpiButtonImageNameExtension"] = value;
            }
        }

        /// <summary>
        /// 获取或设置已禁用的页导航按钮图片名后缀字符串。
        /// </summary>
        /// <remarks>
        /// 当 <see cref="PagingButtonType"/> 设为 Image 时， 该值允许您设置已禁用（即没有链接，因而点击后无反应）的页导航按钮（包括第一页、上一页、下一页、最后一页四个按钮）的图片文件名后缀字符串，因此可以使已禁用的页导航按钮不同于正常的页导航按钮。若未设置该值，则默认值为<see cref="ButtonImageNameExtension"/>，即已禁用的页导航按钮与正常的页导航按钮使用相同的图片。
        /// </remarks>
        [Browsable(true),
            DefaultValue(null),
            Category("图片按钮"),
            Description("已禁用的页导航按钮的图片名后缀字符串")]
        public string DisabledButtonImageNameExtension
        {
            get
            {
                object obj = ViewState["DisabledButtonImageNameExtension"];
                return (obj == null) ? ButtonImageNameExtension : (string)obj;
            }
            set
            {
                ViewState["DisabledButtonImageNameExtension"] = value;
            }
        }
        /// <summary>
        /// 指定当使用图片按钮时，图片的对齐方式。
        /// </summary>

        [Browsable(true),
            Description("指定当使用图片按钮时，图片的对齐方式"),
            DefaultValue(ImageAlign.Baseline),
            Category("图片按钮")]
        public ImageAlign ButtonImageAlign
        {
            get
            {
                object obj = ViewState["ButtonImageAlign"];
                return (obj == null) ? ImageAlign.Baseline : (ImageAlign)obj;
            }
            set { ViewState["ButtonImageAlign"] = value; }
        }

        [Category("GridView"),
        Browsable(true),
        Description("GridView控件ID，此属性必须设置。")]
        public string GridViewID
        {
            get
            {
                if (ViewState["GridViewID"] != null)
                {
                    return (string)ViewState["GridViewID"];
                }
                else
                {
                    throw new TechnicalException("GridViewID not specified");
                }
            }
            set
            {
                ViewState["GridViewID"] = value;
            }
        }

        #endregion

        #region Paging

        /// <summary>
        /// 获取或设置是否启用url来传递分页信息。
        /// </summary>
        /// <remarks>
        /// 启用Url分页方式是将用户欲访问的页索引通过Url来传递，由于该分页方式不使用页面向自身回发来传递数据，
        /// 所以每次分页时所有的数据都恢复为初始值或需要重新获取。使用Url分页方式不支持动态改变分页控件的属性值，
        /// 因暂时无法将新的属性值通过Url来传递给下一页。
        /// </remarks>
        [Browsable(true),
            Category("分页"),
            DefaultValue(false),
            Description("是否使用url传递分页信息的方式来分页")]
        public bool UrlPaging
        {
            get
            {
                return urlPaging;
            }
            set
            {
                urlPaging = value;
            }
        }

        /// <summary>
        /// 获取或设置当启用Url分页方式时，在url中表示要传递的页索引的参数的名称。
        /// </summary>
        /// <remarks>
        /// 该属性允许您自定义通过Url传递页索引时表示要传递的页索引的参数的名称，以避免与现有的参数名重复。
        /// <p>该属性的默认值是“page”，即通过Url分页时，显示在浏览器地址栏中的Url类似于：</p>http://www.webdiyer.com/aspnetpager/samples/datagrid_url.aspx?page=2 
        /// <p>如将该值改为“pageindex”，则上面的Url将变为：</p><p>http://www.webdiyer.com/aspnetpager/samples/datagrid_url.aspx?pageindex=2 </p>
        /// </remarks>
        [Browsable(true),
            DefaultValue("page"),
            Category("分页"),
            Description("当启用Url分页方式时，显示在url中表示要传递的页索引的参数的名称")]
        public string UrlPageIndexName
        {
            get { return urlPageIndexName; }
            set { urlPageIndexName = value; }
        }

        /// <summary>
        /// 获取或设置当前显示页的索引。
        /// </summary>
        ///<remarks>使用此属性来确定在 AspNetPager 控件中当前显示的页，当前显示的页的数字索引将以红色字体加粗显示。此属性还用于以编程的方式控制所显示的页。
        ///<p>　<b>注意：</b>不同于DataGrid控件的CurrentPageIndex，AspNetPager的CurrentPageIndex属性是从1开始的。</p></remarks>
        [ReadOnly(true),
            Browsable(false),
            Description("当前显示页的索引"),
            Category("分页"),
            DefaultValue(1),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentPageIndex
        {
            get
            {
                object cpage = ViewState["CurrentPageIndex"];
                int pindex = (cpage == null) ? 1 : (int)cpage;
                if (pindex > PageCount && PageCount > 0)
                    return PageCount;
                else if (pindex < 1)
                    return 1;
                return pindex;
            }
            set
            {
                int cpage = value;
                if (cpage < 1)
                    cpage = 1;
                //				else if(cpage>this.PageCount)
                //					cpage=this.PageCount;
                ViewState["CurrentPageIndex"] = cpage;
            }
        }

        /// <summary>
        /// 获取或设置需要分页的所有记录的总数。
        /// </summary>
        /// <remarks>
        /// 当页面第一次加载时，应以编程方式将从存储过程或Sql语句中返回的数据表中所有要分页的记录的总数赋予该属性，AspNetPager会将其保存的ViewState中并在页面回发时从ViewState中获取该值，因此避免了每次分页都要访问数据库而影响分页性能。AspNetPager根据要分页的所有数据的总项数和 <see cref="PageSize"/> 属性来计算显示所有数据需要的总页数，即 <see cref="PageCount"/>的值。
        /// </remarks>
        [Browsable(false),
            Description("要分页的所有记录的总数，该值须在程序运行时设置，默认值为225是为设计时支持而设置的参照值。"),
            Category("Data"),
            DefaultValue(225)]
        public int RecordCount
        {
            get
            {
                object obj = ViewState["Recordcount"];
                return (obj == null) ? 0 : (int)obj;
            }
            set { ViewState["Recordcount"] = value; }
        }

        /// <summary>
        /// 获取当前页之后未显示的页的总数。
        /// </summary>
        [Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PagesRemain
        {
            get
            {
                return PageCount - CurrentPageIndex;
            }
        }

        /// <summary>
        /// 获取或设置每页显示的项数。
        /// </summary>
        /// <remarks>
        /// 该值获取或设置数据呈现控件每次要显示数据表中的的数据的项数，AspNetPager根据该值和 <see cref="RecordCount"/> 来计算显示所有数据需要的总页数，即 <see cref="PageCount"/>的值。</remarks>
        /// <example>以下示例将 <see cref="AspNetPager"/> 设置为允许每页显示8条数据：
        /// <code>
        /// <![CDATA[
        ///  ...
        ///  <Webdiyer:AspNetPager id="pager" runat="server" PageSize=8 OnPageChanged="ChangePage"/>
        ///  ...
        /// ]]></code></example>
        [Browsable(true),
            Description("每页显示的记录数"),
            Category("分页"),
            DefaultValue(20)]
        public int PageSize
        {
            get
            {
                //todo 从企业参数中获取默认分页记录数量
                return ViewState["PageSize"] != null ? Convert.ToInt32(ViewState["PageSize"]) : 20;
            }
            set
            {
                ViewState["PageSize"] = value;
            }
            /*
            get
            {

                return SysFramework.Configuration.PageSize;
            }*/
        }

        /// <summary>
        /// 获取在当前页之后还未显示的剩余记录的项数。
        /// </summary>
        [Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RecordsRemain
        {
            get
            {
                if (CurrentPageIndex < PageCount)
                    return RecordCount - (CurrentPageIndex * PageSize);
                return 0;
            }
        }


        /// <summary>
        /// 获取所有要分页的记录需要的总页数。
        /// </summary>
        [Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageCount
        {
            get { return (int)Math.Ceiling((double)RecordCount / (double)PageSize); }
        }


        #endregion

        #region TextBox and Submit Button

        /// <summary>
        /// 获取或设置页索引文本框的显示方式。
        /// </summary>
        /// <remarks>
        /// 页索引文件框允许用户手式输入要访问的页的索引，当页数非常多时，显示页索引文本框非常方便用户跳转到指定的页，默认情况下，该文本框只有在总页数大于或等于 <see cref="ShowBoxThreshold"/> 的值时才显示，否则不显示，要想该文本框任何时候都显示，请将其值设为Always，若希望任何时候都不显示，则应设为Never。
        ///</remarks>
        [Browsable(true),
            Description("指定页索引文本框的显示方式"),
            Category("文本框及提交按钮"),
            DefaultValue(ShowDropList.Auto)]
        public ShowDropList ShowDropList
        {
            get
            {
                object obj = ViewState["ShowDropList"];
                return (obj == null) ? ShowDropList.Always : (ShowDropList)obj;
            }
            set { ViewState["ShowDropList"] = value; }
        }

        /// <summary>
        /// 获取或设置应用于页索引输入文本框的CSS类名。
        /// </summary>
        [Browsable(true),
            Category("文本框及提交按钮"),
            DefaultValue(null),
            Description("应用于页索引输入文本框的CSS类名")]
        public string InputBoxClass
        {
            get
            {
                return (string)ViewState["InputBoxClass"];
            }
            set
            {
                if (value.Trim().Length > 0)
                    ViewState["InputBoxClass"] = value;
            }
        }

        /// <summary>
        /// 获取或设置页索引输入文本框的CSS样式文本。
        /// </summary>

        [Browsable(true),
            Category("文本框及提交按钮"),
            DefaultValue(null),
            Description("应用于页索引输入文本框的CSS样式文本")]
        public string InputBoxStyle
        {
            get
            {
                return (string)ViewState["InputBoxStyle"];
            }
            set
            {
                if (value.Trim().Length > 0)
                    ViewState["InputBoxStyle"] = value;
            }
        }

        /// <summary>
        /// 获取或设置页索引页索引输入文本框前的文本字符串值。
        /// </summary>
        [Browsable(true),
            Category("文本框及提交按钮"),
            DefaultValue(null),
            Description("页索引输入文本框前的文本内容字符串")]
        public string TextBeforeInputBox
        {
            get
            {
                return (string)ViewState["TextBeforeInputBox"];
            }
            set
            {
                ViewState["TextBeforeInputBox"] = value;
            }
        }

        /// <summary>
        /// 获取或设置页索引文本输入框后的文本内容字符串值。
        /// </summary>
        [Browsable(true),
            DefaultValue(null),
            Category("文本框及提交按钮"),
            Description("页索引输入文本框后的文本内容字符串")]
        public string TextAfterInputBox
        {
            get
            {
                return (string)ViewState["TextAfterInputBox"];
            }
            set
            {
                ViewState["TextAfterInputBox"] = value;
            }
        }


        /// <summary>
        /// 获取或设置提交按钮上的文本。
        /// </summary>
        [Browsable(true),
            Category("文本框及提交按钮"),
            DefaultValue("GO"),
            Description("提交按钮上的文本")]
        public string SubmitButtonText
        {
            get
            {
                object obj = ViewState["SubmitButtonText"];
                return (obj == null) ? "GO" : (string)obj;
            }
            set
            {
                if (value.Trim().Length > 0)
                    ViewState["SubmitButtonText"] = value;
            }
        }

        /// <summary>
        /// 获取或设置应用于提交按钮的CSS样式。
        /// </summary>
        [Browsable(true),
            Category("文本框及提交按钮"),
            DefaultValue(null),
            Description("应用于提交按钮的CSS样式")]
        public string SubmitButtonStyle
        {
            get
            {
                return (string)ViewState["SubmitButtonStyle"];
            }
            set
            {
                ViewState["SubmitButtonStyle"] = value;
            }
        }
        /// <summary>
        /// 获取或设置自动显示页索引输入文本框的最低起始页数。
        /// </summary>
        /// <remarks>
        /// 当 <see cref="ShowDropList"/> 设为Auto（默认）并且要分页的数据的总页数达到该值时会自动显示页索引输入文本框，默认值为30。该选项当 <see cref="ShowDropList"/> 设为Never或Always时没有任何作用。
        /// </remarks>
        [Browsable(true),
            Description("指定当ShowDropList设为ShowDropList.Auto时，当总页数达到多少时才显示页索引输入文本框"),
            Category("文本框及提交按钮"),
            DefaultValue(30)]
        public int ShowBoxThreshold
        {
            get
            {
                object obj = ViewState["ShowBoxThreshold"];
                return (obj == null) ? 30 : (int)obj;
            }
            set { ViewState["ShowBoxThreshold"] = value; }
        }


        #endregion

        #region CustomInfoSection

        /// <summary>
        /// 获取或设置显示用户自定义信息区的方式。
        /// </summary>
        /// <remarks>
        /// 该属性值设为Left或Right时会在分页导航元素左边或右边划出一个专门的区域来显示有关用户自定义信息，设为Never时不显示。
        /// </remarks>
        [Browsable(true),
            Description("显示当前页和总页数信息，默认值为不显示，值为ShowCustomInfoSection.Left时将显示在页索引前，为ShowCustomInfoSection.Right时将显示在页索引后"),
            DefaultValue(ShowCustomInfoSection.Never),
            Category("自定义信息区")]
        public ShowCustomInfoSection ShowCustomInfoSection
        {
            get
            {
                object obj = ViewState["ShowCustomInfoSection"];
                return (obj == null) ? ShowCustomInfoSection.Never : (ShowCustomInfoSection)obj;
            }
            set { ViewState["ShowCustomInfoSection"] = value; }
        }

        /// <summary>
        /// 获取或设置用户自定义信息区文本的对齐方式。
        /// </summary>
        [Browsable(true),
            Category("自定义信息区"),
            DefaultValue(HorizontalAlign.Left),
            Description("用户自定义信息区文本的对齐方式")]
        public HorizontalAlign CustomInfoTextAlign
        {
            get
            {
                object obj = ViewState["CustomInfoTextAlign"];
                return (obj == null) ? HorizontalAlign.Left : (HorizontalAlign)obj;
            }
            set
            {
                ViewState["CustomInfoTextAlign"] = value;
            }
        }

        /// <summary>
        /// 获取或设置用户自定义信息区的宽度。
        /// </summary>
        [Browsable(true),
            Category("自定义信息区"),
            DefaultValue(typeof(Unit), "40%"),
            Description("用户自定义信息区的宽度")]
        public Unit CustomInfoSectionWidth
        {
            get
            {
                object obj = ViewState["CustomInfoSectionWidth"];
                return (obj == null) ? Unit.Percentage(40) : (Unit)obj;
            }
            set
            {
                ViewState["CustomInfoSectionWidth"] = value;
            }
        }

        /// <summary>
        /// 获取或设置应用于用户自定义信息区的级联样式表类名。
        /// </summary>
        [Browsable(true),
            Category("自定义信息区"),
            DefaultValue(null),
            Description("应用于用户自定义信息区的级联样式表类名")]
        public string CustomInfoClass
        {
            get
            {
                object obj = ViewState["CustomInfoClass"];
                return (obj == null) ? CssClass : (string)obj;
            }
            set
            {
                ViewState["CustomInfoClass"] = value;
            }
        }

        /// <summary>
        /// 获取或设置应用于用户自定义信息区的CSS样式文本。
        /// </summary>
        /// <value>字符串值，要应用于用户自定义信息区的CSS样式文本。</value>
        [Browsable(true),
            Category("自定义信息区"),
            DefaultValue(null),
            Description("应用于用户自定义信息区的CSS样式文本")]
        public string CustomInfoStyle
        {
            get
            {
                object obj = ViewState["CustomInfoStyle"];
                return (obj == null) ? GetStyleString() : (string)obj;
            }
            set
            {
                ViewState["CustomInfoStyle"] = value;
            }
        }

        /// <summary>
        /// 获取或设置在显示在用户自定义信息区的用户自定义文本。
        /// </summary>
        [Browsable(true),
            Category("自定义信息区"),
            DefaultValue(null),
            Description("要显示在用户自定义信息区的用户自定义信息文本")]
        public string CustomInfoText
        {
            get
            {
                return (string)ViewState["CustomInfoText"];
            }
            set
            {
                ViewState["CustomInfoText"] = value;
            }
        }

        #endregion

        #region Others

        /// <summary>
        /// 获取或设置一个值，该值指定是否总是显示AspNetPager分页按件，即使要分页的数据只有一页。
        /// </summary>
        /// <remarks>
        /// 默认情况下，当要分页的数据小于两页时，AspNetPager不会在页面上显示任何内容，将此属性值设为true时，即使总页数只有一页，AspNetPager也将显示分页导航元素。
        /// </remarks>
        [Browsable(true),
            Category("Behavior"),
            DefaultValue(false),
            Description("总是显示分页控件，即使要分页的数据只要一页")]
        public bool AlwaysShow
        {
            get
            {
                object obj = ViewState["AlwaysShow"];
                return (obj == null) ? false : (bool)obj;
            }
            set
            {
                ViewState["AlwaysShow"] = value;
            }
        }


        /// <summary>
        /// 获取或设置由 AspNetPager 服务器控件在客户端呈现的级联样式表 (CSS) 类。
        /// </summary>
        [Browsable(true),
            Description("应用于控件的CSS类名"),
            Category("Appearance"),
            DefaultValue("Pager")]
        public override string CssClass
        {
            get { return base.CssClass; }
            set
            {
                base.CssClass = value;
                cssClassName = value;
            }
        }


        /// <summary>
        /// 获取或设置一个值，该值指示 AspNetPager 服务器控件是否向发出请求的客户端保持自己的视图状态，该属性经重写后不允许设为false。
        /// </summary>
        /// <remarks><see cref="AspNetPager"/> 服务器控件将一些重要的分页信息保存在ViewState中，当使用Url分页方式时，虽然视图状态在分页过程中没有任何作用，但若当前页需要回发，则必须启用视图状态以便分页控件能在页面回发后获取回发前的分页状态；当通过页面回发（PostBack）的方式来分页时，要使AspNetPager正常工作，必须启用视图状态。
        /// <p><note>该属性并不能禁止用户用<![CDATA[<%@Page EnableViewState=false%> ]]>页指令来禁用整个页面的视图状态，当使用此指令并且设置AspNetPager通过页面回发来分页时，AspNetPager因为无法获取保存的信息而不能正常工作。</note></p></remarks>
        [Browsable(false),
            Description("是否启用控件的视图状态，该属性的值必须为true，不允许用户设置。"),
            DefaultValue(true),
            Category("Behavior")]
        public override bool EnableViewState
        {
            get
            {
                return base.EnableViewState;
            }
            set
            {
                base.EnableViewState = true;
            }
        }

        /// <summary>
        /// 获取或设置当用户输入的页索引超出范围（大于最大页索引或小于最小页索引）时在客户端显示的错误信息。
        /// </summary>
        [Browsable(true),
            Description("当用户输入的页索引超出范围（大于最大页索引或小于最小页索引）时在客户端显示的错误信息。"),
            DefaultValue("页数超出范围！"),
            Category("Data")]
        public string PageIndexOutOfRangeErrorString
        {
            get
            {
                object obj = ViewState["PageIndexOutOfRangeErrorString"];
                return (obj == null) ? "页数超出范围！" : (string)obj;
            }
            set
            {
                ViewState["PageIndexOutOfRangeErrorString"] = value;
            }
        }

        /// <summary>
        /// 获取或设置当用户输入无效的页索引（负值或非数字）时在客户端显示的错误信息。
        /// </summary>
        [Browsable(true),
            Description("当用户输入无效的页索引（负值或非数字）时在客户端显示的错误信息。"),
            DefaultValue("页索引无效！"),
            Category("Data")]
        public string InvalidPageIndexErrorString
        {
            get
            {
                object obj = ViewState["InvalidPageIndexErrorString"];
                return (obj == null) ? "页索引无效！" : (string)obj;
            }
            set
            {
                ViewState["InvalidPageIndexErrorString"] = value;
            }
        }
        /// <summary>
        /// 获取或设置当用户输入无效的页索引（负值或非数字）时在客户端显示的错误信息。
        /// </summary>
        [Browsable(true),
        Description("该控件是否导致激发验证。"),
        DefaultValue("False"),
        Category("Behavior")]
        public bool CausesValidation
        {
            get
            {
                object obj = ViewState["CausesValidation"];
                return (obj == null) ? false : (bool)obj;
            }
            set
            {
                ViewState["CausesValidation"] = value;
            }
        }




        #endregion

        #region NamedQuery
        /// <summary>
        /// 控制是否显示Set as Named Query链接。
        /// </summary>
        [Browsable(true),
        Description("是否显示Set as Named Query链接。"),
        DefaultValue("true"),
        Category("Behavior")]
        public bool NamedQuery
        {
            get
            {
                object obj = ViewState["NamedQuery"];
                return (obj == null) ? false : (bool)obj;
            }
            set
            {
                ViewState["NamedQuery"] = value;
            }
        }

        [Browsable(false),
        Description("Named Query Status"),
        Category("Data")]
        public string NamedQueryStatus
        {
            get
            {
                object obj = ViewState["NamedQueryStatus"];
                return (obj == null) ? string.Empty : (string)obj;
            }
            set
            {
                ViewState["NamedQueryStatus"] = value;
            }
        }

        [Browsable(false),
        Description("Named Query Name"),
        Category("Data")]
        public string NamedQueryName
        {
            get
            {
                object obj = ViewState["NamedQueryName"];
                return (obj == null) ? string.Empty : (string)obj;
            }
            set
            {
                ViewState["NamedQueryName"] = value;
            }
        }

        [Browsable(true),
        Description("Named Query TypeName"),
        Category("Behavior")]
        public string NamedQueryTypeName
        {
            get
            {
                object obj = ViewState["NamedQueryTypeName"];
                return (obj == null) ? "com.Sconit.Web.NamedQueryMgrProxy" : (string)obj;
            }
            set
            {
                ViewState["NamedQueryTypeName"] = value;
            }
        }

        [Browsable(true),
        Description("Named Query Load Method"),
        Category("Behavior")]
        public string NamedQueryLoadMethod
        {
            get
            {
                object obj = ViewState["NamedQueryLoadMethod"];
                return (obj == null) ? "LoadNamedQuery" : (string)obj;
            }
            set
            {
                ViewState["NamedQueryLoadMethod"] = value;
            }
        }

        [Browsable(true),
        Description("Named Query Insert Method"),
        Category("Behavior")]
        public string NamedQueryInsertMethod
        {
            get
            {
                object obj = ViewState["NamedQueryInsertMethod"];
                return (obj == null) ? "CreateNamedQuery" : (string)obj;
            }
            set
            {
                ViewState["NamedQueryInsertMethod"] = value;
            }
        }

        [Browsable(true),
        Description("Named Query Update Method"),
        Category("Behavior")]
        public string NamedQueryUpdateMethod
        {
            get
            {
                object obj = ViewState["NamedQueryUpdateMethod"];
                return (obj == null) ? "UpdateNamedQuery" : (string)obj;
            }
            set
            {
                ViewState["NamedQueryUpdateMethod"] = value;
            }
        }
        #endregion

        #endregion

        #region Control Rendering Logic

        /// <summary>
        /// 重写 <see cref="System.Web.UI.Control.OnLoad"/> 方法。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="EventArgs"/> 对象。</param>
        protected override void OnLoad(EventArgs e)
        {
            if (urlPaging)
            {
                currentUrl = Page.Request.Path;
                urlParams = Page.Request.QueryString;
                string pageIndex = Page.Request.QueryString[urlPageIndexName];
                int index = 1;
                try
                {
                    index = int.Parse(pageIndex);
                }
                catch { }
                OnPageChanged(new PageChangedEventArgs(index));
            }
            else
            {
                inputPageIndex = Page.Request.Form[this.UniqueID + "_input"];
            }
            this.Page.ClientScript.GetPostBackEventReference(this, this.ID);
            base.OnLoad(e);
        }
        public void InitPageIndex(EventArgs e)
        {
            this.OnLoad(e);
        }
        /// <summary>
        /// 重写<see cref="System.Web.UI.Control.OnPreRender"/>方法。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="EventArgs"/> 对象。</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (PageCount > 1)
            {
                string checkscript = "<script language=\"Javascript\">function doCheck(el){var r=new RegExp(\"^\\\\s*(\\\\d+)\\\\s*$\");if(r.test(el.value)){if(RegExp.$1<1||RegExp.$1>" + PageCount.ToString() + "){alert(\"" + PageIndexOutOfRangeErrorString + "\");document.all[\'" + this.UniqueID + "_input\'].select();return false;}return true;}alert(\"" + InvalidPageIndexErrorString + "\");document.all[\'" + this.UniqueID + "_input\'].select();return false;}</script>";
                if ((ShowDropList == ShowDropList.Always) || (ShowDropList == ShowDropList.Auto && PageCount >= ShowBoxThreshold))
                {
                    if (!Page.ClientScript.IsClientScriptBlockRegistered("checkinput"))
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "checkinput", checkscript);
                    string script = "<script language=\"javascript\" > <!-- \nfunction BuildUrlString(key,value){ var _key=key.toLowerCase(); var prms=location.search; if(prms.length==0) return location.pathname+\"?\"+_key+\"=\"+value; var params=prms.substring(1).split(\"&\"); var newparam=\"\"; var found=false; for(i=0;i<params.length;i++){ if(params[i].split(\"=\")[0].toLowerCase()==_key){ params[i]=_key+\"=\"+value; found=true; break; } } if(found) return location.pathname+\"?\"+params.join(\"&\"); else return location+\"&\"+_key+\"=\"+value; }\n//--> </script>";
                    if (!Page.ClientScript.IsClientScriptBlockRegistered("BuildUrlScript"))
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "BuildUrlScript", script);
                }
            }
            base.OnPreRender(e);
        }

        /// <summary>
        /// 重写<see cref="System.Web.UI.WebControls.WebControl.AddAttributesToRender"/> 方法，将需要呈现的 HTML 属性和样式添加到指定的 <see cref="System.Web.UI.HtmlTextWriter"/> 中
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (this.Page != null)
                this.Page.VerifyRenderingInServerForm(this);
            base.AddAttributesToRender(writer);
        }

        ///<summary>
        ///重写 <see cref="System.Web.UI.WebControls.WebControl.RenderBeginTag"/> 方法，将 <see cref="AspNetPager"/> 控件的 HTML 开始标记输出到指定的 <see cref="System.Web.UI.HtmlTextWriter"/> 编写器中。
        ///</summary>
        ///<param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>，表示要在客户端呈现 HTML 内容的输出流。</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            bool showPager = (PageCount > 1 || (PageCount <= 1 && AlwaysShow));
            base.RenderBeginTag(writer);

            if ((ShowCustomInfoSection == ShowCustomInfoSection.Left || ShowCustomInfoSection == ShowCustomInfoSection.Right) && showPager)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, GetStyleString());
                if (Height != Unit.Empty)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString());
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                WriteCellAttributes(writer, true);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
            }
        }

        ///<summary>
        ///重写 <see cref="System.Web.UI.WebControls.WebControl.RenderEndTag"/> 方法，将 <see cref="AspNetPager"/> 控件的 HTML 结束标记输出到指定的 <see cref="System.Web.UI.HtmlTextWriter"/> 编写器中。
        ///</summary>
        ///<param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>，表示要在客户端呈现 HTML 内容的输出流。</param>

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if ((ShowCustomInfoSection == ShowCustomInfoSection.Left || ShowCustomInfoSection == ShowCustomInfoSection.Right) && (PageCount > 1 || (PageCount <= 1 && AlwaysShow)))
            {
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            base.RenderEndTag(writer);
        }

        private DropDownList GetDDLPager()
        {
            DropDownList ddl = new DropDownList();
            ddl.ID = "ddlPager";
            int _pageCount = this.PageCount > 50 ? 50 : this.PageCount;
            for (int i = 1; i <= _pageCount; i++)
            {
                ddl.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddl.Items.Add(new ListItem("...", this.PageCount.ToString()));
            ddl.SelectedIndex = this.CurrentPageIndex - 1;
            ddl.CssClass = "PagerDDl";
            return ddl;
        }

        /// <summary>
        /// 重写 <see cref="System.Web.UI.WebControls.WebControl.RenderContents"/> 方法，将控件的内容呈现到指定 <see cref="System.Web.UI.HtmlTextWriter"/> 的编写器中。
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>，表示要在客户端呈现 HTML 内容的输出流。</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "float:left");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            GridView gv = FindGV();
            if (gv.SelectCriteriaExpire)
            {
                writer.Write("${Common.GridView.SelectCriteriaExpire}");
                writer.RenderEndTag();
                return;
            }

            if (RecordCount == 0)
            {
                writer.Write("${Common.GridView.NoRecordFound}");
                writer.RenderEndTag();
                return;
            }
            if (PageCount <= 1 && !AlwaysShow)
            {
                writer.RenderEndTag();
                return;
            }

            if (ShowCustomInfoSection == ShowCustomInfoSection.Left)
            {
                writer.Write(CustomInfoText);
                writer.RenderEndTag();
                WriteCellAttributes(writer, false);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
            }

            int midpage = ((CurrentPageIndex - 1) / NumericButtonCount);
            int pageoffset = midpage * NumericButtonCount;
            int endpage = ((pageoffset + NumericButtonCount) > PageCount) ? PageCount : (pageoffset + NumericButtonCount);
            writer.Write("${Common.GridView.RecordCountBar," + RecordCount + "," + ((CurrentPageIndex - 1) * PageSize + 1) + "," + (CurrentPageIndex == PageCount ? RecordCount : CurrentPageIndex * PageSize) + "}");
            this.CreateNavigationButton(writer, "first");
            writer.Write("&nbsp;|&nbsp;");
            this.CreateNavigationButton(writer, "prev");
            writer.Write("&nbsp;|&nbsp;");
            this.CreateNavigationButton(writer, "next");
            writer.Write("&nbsp;|&nbsp;");
            this.CreateNavigationButton(writer, "last");
            writer.Write("&nbsp;${Common.GridView.To}");
            if ((ShowDropList == ShowDropList.Always) || (ShowDropList == ShowDropList.Auto && PageCount >= ShowBoxThreshold))
            {
                writer.Write("&nbsp;");
                if (TextBeforeInputBox != null)
                    writer.Write(TextBeforeInputBox);
                this.GetDDLPager().RenderControl(writer);
                writer.Write("&nbsp;");
                string clickScript = "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "__doPostBack(\'" + this.UniqueID + "\',document.all[\'ddlPager\'].selectedIndex+1);}";
                if (UrlPaging)
                {
                    clickScript = "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + " window.location.href='" + GetHrefString() + "' + (document.all[\'ddlPager\'].selectedIndex*1+1);}";
                }
                if (TextAfterInputBox != null)
                    writer.Write(TextAfterInputBox);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, (urlPaging == true) ? "Button" : "Submit");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
                writer.AddAttribute(HtmlTextWriterAttribute.Value, SubmitButtonText);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "PagerGo");
                if (SubmitButtonStyle != null && SubmitButtonStyle.Trim().Length > 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, SubmitButtonStyle);
                if (PageCount <= 1 && AlwaysShow)
                    writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, clickScript);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.Write("&nbsp;");
            }

            if (ShowCustomInfoSection == ShowCustomInfoSection.Right)
            {
                writer.RenderEndTag();
                WriteCellAttributes(writer, false);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(CustomInfoText);
            }
            writer.RenderEndTag();

            RendNamedQueryContent(writer);
        }

        protected void RendNamedQueryContent(HtmlTextWriter writer)
        {
            if (NamedQuery)
            {
                if (NamedQueryStatus == string.Empty || NamedQueryStatus == "Cancel")
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "float:right");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "DivNameQuery");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "$('#divNamedQuery').show();}");
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("Save as Named Query");
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.Write("&nbsp;");

                    //writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 100%; text-align: center");
                    //writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "divNamedQuery");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "floatdiv");
                    if (NamedQueryStatus == "Cancel")
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:block");
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("名称:");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "tbNamedQueryName");
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    if (NamedQueryStatus == "Cancel")
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Value, this.NamedQueryName);
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                    //writer.RenderBeginTag(HtmlTextWriterTag.Br);
                    //writer.RenderEndTag();
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, "确定");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "__doPostBack(\'" + this.UniqueID + "\','NQSave' + '$' + $('#tbNamedQueryName').val());}");
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, "取消");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "$('#divNamedQuery').hide();}");
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    //writer.RenderEndTag();
                }
                else if (NamedQueryStatus == "Existed")
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "float:right");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "$('#divNamedQuery').show();}");
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("Save as Named Query");
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.Write("&nbsp;");

                    //writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 100%; text-align: center");
                    //writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "divNamedQuery");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "floatdiv");
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:block");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("名称:" + this.NamedQueryName + "已经存在，是否要覆盖？");
                    //writer.RenderBeginTag(HtmlTextWriterTag.Br);
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "hfNamedQueryName");
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, this.NamedQueryName);
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    //writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, "确定");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "__doPostBack(\'" + this.UniqueID + "\','NQConfirm'+'$'+ $('#hfNamedQueryName').val());}");
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, "取消");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "__doPostBack(\'" + this.UniqueID + "\','NQCancel'+'$'+ $('#hfNamedQueryName').val());}");
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    //writer.RenderEndTag();

                }
                //else if (NamedQueryStatus == "Cancel")
                //{
                //    writer.AddAttribute(HtmlTextWriterAttribute.Style, "float:right");
                //    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                //    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                //    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "$('#divNamedQuery').show();}");
                //    writer.RenderBeginTag(HtmlTextWriterTag.A);
                //    writer.Write("Save as Named Query");
                //    writer.RenderEndTag();
                //    writer.RenderEndTag();
                //    writer.Write("&nbsp;");

                //    writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 100%; text-align: center");
                //    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                //    writer.AddAttribute(HtmlTextWriterAttribute.Id, "divNamedQuery");
                //    writer.AddAttribute(HtmlTextWriterAttribute.Class, "floatdiv");
                //    writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:block");
                //    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                //    writer.Write("名称:");
                //    writer.AddAttribute(HtmlTextWriterAttribute.Id, "tbNamedQueryName");
                //    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                //    writer.AddAttribute(HtmlTextWriterAttribute.Value, this.NamedQueryName);
                //    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                //    writer.RenderEndTag();
                //    writer.RenderBeginTag(HtmlTextWriterTag.Br);
                //    writer.RenderEndTag();
                //    writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                //    writer.AddAttribute(HtmlTextWriterAttribute.Value, "确定");
                //    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "__doPostBack(\'" + this.UniqueID + "\','NQSave'+'$'+document.all[\'tbNamedQueryName\'].value);}");
                //    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                //    writer.RenderEndTag();
                //    writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                //    writer.AddAttribute(HtmlTextWriterAttribute.Value, "取消");
                //    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "$('#divNamedQuery').hide();}");
                //    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                //    writer.RenderEndTag();
                //    writer.RenderEndTag();
                //    writer.RenderEndTag();
                //}
                else if (NamedQueryStatus == "SaveOK")
                {
                    //writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 100%; text-align: center");
                    //writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "divNamedQuery");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "floatdiv");
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:block");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("保存成功");
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, "确定");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "$('#divNamedQuery').hide();}");
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    //writer.RenderEndTag();
                }
            }
        }

        #endregion


        #region Private Helper Functions

        /// <summary>
        /// 将基控件的Style转换为CSS字符串。
        /// </summary>
        /// <returns></returns>
        private string GetStyleString()
        {
            if (Style.Count > 0)
            {
                string stl = null;
                string[] skeys = new string[Style.Count];
                Style.Keys.CopyTo(skeys, 0);
                for (int i = 0; i < skeys.Length; i++)
                {
                    stl += String.Concat(skeys[i], ":", Style[skeys[i]], ";");
                }
                return stl;
            }
            return null;
        }

        /// <summary>
        /// 为用户自定义信息区和页导航按钮区和td添加属性。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="leftCell">是否为第一个td</param>
        private void WriteCellAttributes(HtmlTextWriter writer, bool leftCell)
        {
            string customUnit = CustomInfoSectionWidth.ToString();
            if (ShowCustomInfoSection == ShowCustomInfoSection.Left && leftCell || ShowCustomInfoSection == ShowCustomInfoSection.Right && !leftCell)
            {
                if (CustomInfoClass != null && CustomInfoClass.Trim().Length > 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, CustomInfoClass);
                if (CustomInfoStyle != null && CustomInfoStyle.Trim().Length > 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, CustomInfoStyle);
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, customUnit);
                writer.AddAttribute(HtmlTextWriterAttribute.Align, CustomInfoTextAlign.ToString().ToLower());
            }
            else
            {
                if (CustomInfoSectionWidth.Type == UnitType.Percentage)
                {
                    customUnit = (Unit.Percentage(100 - CustomInfoSectionWidth.Value)).ToString();
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, customUnit);
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
                writer.AddAttribute(HtmlTextWriterAttribute.Align, HorizontalAlign.ToString().ToLower());
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "true");
        }

        /// <summary>
        /// 获取分页导航按钮的超链接字符串。
        /// </summary>
        /// <param name="pageIndex">该分页按钮相对应的页索引。</param>
        /// <returns>分页导航按钮的超链接字符串。</returns>
        private string GetHrefString(int pageIndex)
        {
            if (urlPaging)
            {
                NameValueCollection col = new NameValueCollection();
                col.Add(urlPageIndexName, pageIndex.ToString());
                return BuildUrlString(col);
            }
            return "javascript:{" + (CausesValidation ? "if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())" : "") + "__doPostBack('" + this.UniqueID + "','" + pageIndex.ToString() + "');}";
        }
        /// <summary>
        /// 获取分页导航按钮的超链接字符串。
        /// </summary>
        /// <returns>分页导航按钮的超链接字符串。</returns>
        private string GetHrefString()
        {
            if (urlPaging)
            {
                NameValueCollection col = new NameValueCollection();
                col.Add(urlPageIndexName, "");
                return BuildUrlString(col);
            }
            return Page.ClientScript.GetPostBackClientHyperlink(this, "");
        }

        /// <summary>
        /// 当使用Url分页方式时，在当前Url上加入分页参数，若该参数存在，则改变其值。
        /// </summary>
        /// <param name="col">要加入到新Url中的参数名和值的集合。</param>
        /// <returns>分页导航按钮的超链接字符串，包括分页参数。</returns>
        private string BuildUrlString(NameValueCollection col)
        {
            int i;
            string tempstr = "";
            if (urlParams == null || urlParams.Count <= 0)
            {
                for (i = 0; i < col.Count; i++)
                {
                    tempstr += String.Concat("&", col.Keys[i], "=", col[i]);
                }
                return String.Concat(currentUrl, "?", tempstr.Substring(1));
            }
            NameValueCollection newCol = new NameValueCollection(urlParams);
            string[] newColKeys = newCol.AllKeys;
            for (i = 0; i < newColKeys.Length; i++)
            {
                newColKeys[i] = newColKeys[i].ToLower();
            }
            for (i = 0; i < col.Count; i++)
            {
                if (Array.IndexOf(newColKeys, col.Keys[i].ToLower()) < 0)
                    newCol.Add(col.Keys[i], col[i]);
                else
                    newCol[col.Keys[i]] = col[i];
            }
            StringBuilder sb = new StringBuilder();
            for (i = 0; i < newCol.Count; i++)
            {
                sb.Append("&");
                sb.Append(newCol.Keys[i]);
                sb.Append("=");
                sb.Append(newCol[i]);
            }
            return String.Concat(currentUrl, "?", sb.ToString().Substring(1));
        }

        /// <summary>
        /// 创建第一页、上一页、下一页及最后一页分页按钮。
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>，表示要在客户端呈现 HTML 内容的输出流。</param>
        /// <param name="btnname">分页按钮名。</param>
        private void CreateNavigationButton(HtmlTextWriter writer, string btnname)
        {
            if (!ShowFirstLast && (btnname == "first" || btnname == "last"))
                return;
            if (!ShowPrevNext && (btnname == "prev" || btnname == "next"))
                return;
            string linktext = "";
            bool disabled;
            int pageIndex;
            bool imgButton = (PagingButtonType == PagingButtonType.Image && NavigationButtonType == PagingButtonType.Image);
            if (btnname == "prev" || btnname == "first")
            {
                disabled = (CurrentPageIndex <= 1);
                if (!ShowDisabledButtons && disabled)
                    return;
                pageIndex = (btnname == "first") ? 1 : (CurrentPageIndex - 1);
                if (imgButton)
                {
                    if (!disabled)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                        AddToolTip(writer, pageIndex);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, ButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, DisabledButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                    }
                }
                else
                {
                    linktext = (btnname == "prev") ? PrevPageText : FirstPageText;
                    if (disabled)
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                    else
                    {
                        WriteCssClass(writer);
                        AddToolTip(writer, pageIndex);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(linktext);
                    writer.RenderEndTag();
                }
            }
            else
            {
                disabled = (CurrentPageIndex >= PageCount);
                if (!ShowDisabledButtons && disabled)
                    return;
                pageIndex = (btnname == "last") ? PageCount : (CurrentPageIndex + 1);
                if (imgButton)
                {
                    if (!disabled)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                        AddToolTip(writer, pageIndex);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, ButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, DisabledButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                    }
                }
                else
                {
                    linktext = (btnname == "next") ? NextPageText : LastPageText;
                    if (disabled)
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                    else
                    {
                        WriteCssClass(writer);
                        AddToolTip(writer, pageIndex);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(linktext);
                    writer.RenderEndTag();
                }
            }
            WriteButtonSpace(writer);
        }

        /// <summary>
        /// 写入CSS类名。
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>，表示要在客户端呈现 HTML 内容的输出流。</param>
        private void WriteCssClass(HtmlTextWriter writer)
        {
            if (cssClassName != null && cssClassName.Trim().Length > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClassName);
        }

        /// <summary>
        /// 加入导航按钮提示文本。
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>，表示要在客户端呈现 HTML 内容的输出流。</param>
        /// <param name="pageIndex">导航按钮对应的页索引。</param>
        private void AddToolTip(HtmlTextWriter writer, int pageIndex)
        {
            if (ShowNavigationToolTip)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, String.Format(NavigationToolTipTextFormatString, pageIndex));
            }
        }

        /// <summary>
        /// 创建分页数值导航按钮。
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>，表示要在客户端呈现 HTML 内容的输出流。</param>
        /// <param name="index">要创建按钮的页索引的值。</param>
        private void CreateNumericButton(HtmlTextWriter writer, int index)
        {
            bool isCurrent = (index == CurrentPageIndex);
            if (PagingButtonType == PagingButtonType.Image && NumericButtonType == PagingButtonType.Image)
            {
                if (!isCurrent)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(index));
                    AddToolTip(writer, index);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    CreateNumericImages(writer, index, isCurrent);
                    writer.RenderEndTag();
                }
                else
                    CreateNumericImages(writer, index, isCurrent);
            }
            else
            {
                if (isCurrent)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "Bold");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                    writer.RenderBeginTag(HtmlTextWriterTag.Font);
                    if (NumericButtonTextFormatString.Length > 0)
                        writer.Write(String.Format(NumericButtonTextFormatString, (ChinesePageIndex == true) ? GetChinesePageIndex(index) : (index.ToString())));
                    else
                        writer.Write((ChinesePageIndex == true) ? GetChinesePageIndex(index) : index.ToString());
                    writer.RenderEndTag();
                }
                else
                {
                    WriteCssClass(writer);
                    AddToolTip(writer, index);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(index));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    if (NumericButtonTextFormatString.Length > 0)
                        writer.Write(String.Format(NumericButtonTextFormatString, (ChinesePageIndex == true) ? GetChinesePageIndex(index) : (index.ToString())));
                    else
                        writer.Write((ChinesePageIndex == true) ? GetChinesePageIndex(index) : index.ToString());
                    writer.RenderEndTag();
                }
            }
            WriteButtonSpace(writer);
        }

        /// <summary>
        /// 在分页导航元素间加入空格。
        /// </summary>
        /// <param name="writer"></param>
        private void WriteButtonSpace(HtmlTextWriter writer)
        {
            if (PagingButtonSpacing.Value > 0)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, PagingButtonSpacing.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// 获取中文页索引字符。
        /// </summary>
        /// <param name="index">中文字符对应的页索引数值</param>
        /// <returns>对应于页索引数值的中文字符</returns>
        private string GetChinesePageIndex(int index)
        {
            Hashtable cnChars = new Hashtable();
            cnChars.Add("0", "Ｏ");
            cnChars.Add("1", "一");
            cnChars.Add("2", "二");
            cnChars.Add("3", "三");
            cnChars.Add("4", "四");
            cnChars.Add("5", "五");
            cnChars.Add("6", "六");
            cnChars.Add("7", "七");
            cnChars.Add("8", "八");
            cnChars.Add("9", "九");
            string indexStr = index.ToString();
            string retStr = "";
            for (int i = 0; i < indexStr.Length; i++)
            {
                retStr = String.Concat(retStr, cnChars[indexStr[i].ToString()]);
            }
            return retStr;
        }

        /// <summary>
        /// 创建页索引图片按钮。
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>，表示要在客户端呈现 HTML 内容的输出流。</param>
        /// <param name="index">页索引数值。</param>
        /// <param name="isCurrent">是否是当前页索引。</param>
        private void CreateNumericImages(HtmlTextWriter writer, int index, bool isCurrent)
        {
            string indexStr = index.ToString();
            for (int i = 0; i < indexStr.Length; i++)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, indexStr[i], (isCurrent == true) ? CpiButtonImageNameExtension : ButtonImageNameExtension, ButtonImageExtension));
                writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// 创建“更多页”（...）按钮。
        /// </summary>
        /// <param name="writer"><see cref="System.Web.UI.HtmlTextWriter"/>，表示要在客户端呈现 HTML 内容的输出流。</param>
        /// <param name="pageIndex">链接到按钮的页的索引。</param>
        private void CreateMoreButton(HtmlTextWriter writer, int pageIndex)
        {
            WriteCssClass(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
            AddToolTip(writer, pageIndex);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            if (PagingButtonType == PagingButtonType.Image && MoreButtonType == PagingButtonType.Image)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, "more", ButtonImageNameExtension, ButtonImageExtension));
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
            else
                writer.Write("...");
            writer.RenderEndTag();
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, PagingButtonSpacing.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.RenderEndTag();
        }

        #endregion

        #region IPostBackEventHandler Implementation

        /// <summary>
        /// 实现<see cref="IPostBackEventHandler"/> 接口，使 <see cref="AspNetPager"/> 控件能够处理将窗体发送到服务器时引发的事件。
        /// </summary>
        /// <param name="args"></param>
        public void RaisePostBackEvent(string args)
        {
            int pageIndex = CurrentPageIndex;
            try
            {
                if (args == null || args == "")
                    args = inputPageIndex;
                pageIndex = int.Parse(args);
            }
            catch
            {
                //    if (args.StartsWith("NQSave"))
                //    {
                //        this.NamedQueryStatus = "Save";
                //        this.NamedQueryName = args.Split('$')[1];

                //        if (IsNamedQueryNameExist())
                //        {
                //            this.NamedQueryStatus = "Existed";
                //        }
                //        else
                //        {
                //            InsertNamedQuery();
                //            this.NamedQueryStatus = "SaveOK";
                //        }
                //    }
                //    else if (args.StartsWith("NQConfirm"))
                //    {
                //        this.NamedQueryStatus = "Confirm";
                //        this.NamedQueryName = args.Split('$')[1];

                //        if (IsNamedQueryNameExist())
                //        {
                //            UpdateNamedQuery();
                //            this.NamedQueryStatus = "SaveOK";
                //        }
                //        else
                //        {
                //            InsertNamedQuery();
                //            this.NamedQueryStatus = "SaveOK";
                //        }

                //    }
                //    else if (args.StartsWith("NQCancel"))
                //    {
                //        this.NamedQueryStatus = "Cancel";
                //        this.NamedQueryName = args.Split('$')[1];
                //    }
            }
            this.CurrentPageIndex = pageIndex;
            OnPageChanged(new PageChangedEventArgs(pageIndex));
        }

        //private NamedQuery LoadNamedQuery()
        //{
        //    MethodInfo namedQueryLoadMethod = BuildManager.GetType(NamedQueryTypeName, true).GetMethod(NamedQueryLoadMethod);
        //    ConstructorInfo constructorInfo = BuildManager.GetType(NamedQueryTypeName, true).GetConstructor(System.Type.EmptyTypes);

        //    if (namedQueryLoadMethod != null)
        //    {
        //        NamedQuery namedQuery = null;
        //        User user = new SessionHelper(this.Page).CurrentUser;
        //        if (namedQueryLoadMethod.IsStatic)
        //        {
        //            namedQuery = (NamedQuery)namedQueryLoadMethod.Invoke(null, new object[] { user.Id, this.NamedQueryName });
        //        }
        //        else
        //        {
        //            if (constructorInfo != null)
        //            {
        //                if (constructorInfo.IsStatic)
        //                {
        //                    namedQuery = (NamedQuery)namedQueryLoadMethod.Invoke(null, new object[] { user.Id, this.NamedQueryName });
        //                }
        //                else
        //                {
        //                    object obj = constructorInfo.Invoke(null);
        //                    namedQuery = (NamedQuery)namedQueryLoadMethod.Invoke(obj, new object[] { user.Id, this.NamedQueryName });
        //                }
        //            }
        //            else
        //            {
        //                throw new TechnicalException(NamedQueryTypeName + " specified has not zero parameter constructor");
        //            }
        //        }

        //        return namedQuery;
        //    }
        //    else
        //    {
        //        throw new TechnicalException("no specified method:" + namedQueryLoadMethod);
        //    }
        //}

        //private bool IsNamedQueryNameExist()
        //{
        //    if (LoadNamedQuery() != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private void InsertNamedQuery()
        //{
        //    MethodInfo namedQueryInsertMethod = BuildManager.GetType(NamedQueryTypeName, true).GetMethod(NamedQueryInsertMethod);
        //    ConstructorInfo constructorInfo = BuildManager.GetType(NamedQueryTypeName, true).GetConstructor(System.Type.EmptyTypes);

        //    if (namedQueryInsertMethod != null)
        //    {
        //        com.Sconit.Entity.MasterData.NamedQuery namedQuery = GenerateNameQuery();
        //        if (namedQueryInsertMethod.IsStatic)
        //        {
        //            namedQueryInsertMethod.Invoke(null, new object[] { namedQuery });
        //        }
        //        else
        //        {
        //            if (constructorInfo != null)
        //            {
        //                if (constructorInfo.IsStatic)
        //                {
        //                    namedQueryInsertMethod.Invoke(null, new object[] { namedQuery });
        //                }
        //                else
        //                {
        //                    object obj = constructorInfo.Invoke(null);
        //                    namedQueryInsertMethod.Invoke(obj, new object[] { namedQuery });
        //                }
        //            }
        //            else
        //            {
        //                throw new TechnicalException(NamedQueryTypeName + " specified has not zero parameter constructor");
        //            }
        //        }

        //    }
        //    else
        //    {
        //        throw new TechnicalException("no specified method:" + namedQueryInsertMethod);
        //    }
        //}

        //private void UpdateNamedQuery()
        //{
        //    MethodInfo namedQueryUpdateMethod = BuildManager.GetType(NamedQueryTypeName, true).GetMethod(NamedQueryUpdateMethod);
        //    ConstructorInfo constructorInfo = BuildManager.GetType(NamedQueryTypeName, true).GetConstructor(System.Type.EmptyTypes);

        //    if (namedQueryUpdateMethod != null)
        //    {
        //        NamedQuery namedQuery = this.LoadNamedQuery();
        //        namedQuery.ListPage = this.NamingContainer.TemplateControl.AppRelativeVirtualPath;
        //        namedQuery.SelectCountCriteria = SerializeObject(FindGV().SelectCountCriteria);
        //        namedQuery.SelectCriteria = SerializeObject(FindGV().SelectCriteria);

        //        if (namedQueryUpdateMethod.IsStatic)
        //        {
        //            namedQueryUpdateMethod.Invoke(null, new object[] { namedQuery });
        //        }
        //        else
        //        {
        //            if (constructorInfo != null)
        //            {
        //                if (constructorInfo.IsStatic)
        //                {
        //                    namedQueryUpdateMethod.Invoke(null, new object[] { namedQuery });
        //                }
        //                else
        //                {
        //                    object obj = constructorInfo.Invoke(null);
        //                    namedQueryUpdateMethod.Invoke(obj, new object[] { namedQuery });
        //                }
        //            }
        //            else
        //            {
        //                throw new TechnicalException(NamedQueryTypeName + " specified has not zero parameter constructor");
        //            }
        //        }

        //    }
        //    else
        //    {
        //        throw new TechnicalException("no specified method:" + namedQueryUpdateMethod);
        //    }
        //}

        //private byte[] SerializeObject(object source)
        //{
        //    BinaryFormatter formatter = new BinaryFormatter();
        //    MemoryStream stream = new MemoryStream();
        //    using (stream)
        //    {
        //        formatter.Serialize(stream, source);
        //        return stream.ToArray();
        //    }
        //}

        //private com.Sconit.Entity.MasterData.NamedQuery GenerateNameQuery()
        //{
        //    com.Sconit.Entity.MasterData.NamedQuery namedQuery = new com.Sconit.Entity.MasterData.NamedQuery();
        //    User user = new SessionHelper(this.Page).CurrentUser;
        //    namedQuery.User = user;
        //    namedQuery.Name = this.NamedQueryName;
        //    namedQuery.ListPage = this.NamingContainer.TemplateControl.AppRelativeVirtualPath;
        //    namedQuery.SelectCountCriteria = SerializeObject(FindGV().SelectCountCriteria);
        //    namedQuery.SelectCriteria = SerializeObject(FindGV().SelectCriteria);

        //    return namedQuery;
        //}
        #endregion

        #region IPostBackDataHandler Implementation

        /// <summary>
        /// 实现 <see cref="IPostBackDataHandler"/> 接口，为 <see cref="AspNetPager"/> 服务器控件处理回发数据。
        /// </summary>
        /// <param name="pkey">控件的主要标识符。</param>
        /// <param name="pcol">所有传入名称值的集合。</param>
        /// <returns></returns>
        public virtual bool LoadPostData(string pkey, NameValueCollection pcol)
        {
            string str = pcol[this.UniqueID + "_input"];
            if (str != null && str.Trim().Length > 0)
            {
                try
                {
                    int pindex = int.Parse(str);
                    if (pindex > 0 && pindex <= PageCount)
                    {
                        inputPageIndex = str;
                        Page.RegisterRequiresRaiseEvent(this);
                    }
                }
                catch
                { }
            }
            return false;
        }

        /// <summary>
        /// 实现 <see cref="IPostBackDataHandler"/> 接口，用信号要求服务器控件对象通知 ASP.NET 应用程序该控件的状态已更改。
        /// </summary>
        public virtual void RaisePostDataChangedEvent() { }

        #endregion

        #region PageChanged Event
        public event PageChangedEventHandler PageChanged;

        #endregion

        #region OnPageChanged Method

        /// <summary>
        /// 引发 <see cref="PageChanged"/> 事件。这使您可以为事件提供自定义处理程序。
        /// </summary>
        /// <param name="e">一个 <see cref="PageChangedEventArgs"/>，它包含事件数据。</param>
        protected virtual void OnPageChanged(PageChangedEventArgs e)
        {

            if (this.PageChanged != null)
                PageChanged(this, e);

            GridView gv = FindGV();
            gv.Execute();
        }

        private GridView FindGV()
        {
            GridView gv = (GridView)this.Parent.FindControl(this.GridViewID);
            if (gv != null)
            {
                return gv;
            }
            else
            {
                throw new TechnicalException("GridViewID:" + GridViewID + " specified is not valided");
            }
        }

        #endregion
    }


    #endregion

    #region PageChangedEventHandler Delegate
    /// <summary>
    /// 表示处理 <see cref="AspNetPager.PageChanged"/> 事件的方法。
    /// </summary>
    public delegate void PageChangedEventHandler(object src, PageChangedEventArgs e);

    #endregion

    #region PageChangedEventArgs Class
    /// <summary>
    /// 为 <see cref="AspNetPager"/> 控件的 <see cref="AspNetPager.PageChanged"/> 事件提供数据。无法继承此类。
    /// </summary>
    /// <remarks>
    /// 当 <see cref="AspNetPager"/> 控件的页导航元素之一被单击或用户输入页索引提交时引发 <see cref="AspNetPager.PageChanged"/> 事件。
    /// <p>有关 PageChangedEventArgs 实例的初始属性值列表，请参阅 PageChangedEventArgs 构造函数。</p>
    /// </remarks>
    public sealed class PageChangedEventArgs : EventArgs
    {
        private readonly int _newpageindex;

        /// <summary>
        /// 使用新页面索引初始化 PageChangedEventArgs 类的新实例。
        /// </summary>
        /// <param name="newPageIndex">用户从 <see cref="AspNetPager"/> 控件的页选择元素选定的或在页索引文本框中手工输入的页的索引。</param>
        public PageChangedEventArgs(int newPageIndex)
        {
            this._newpageindex = newPageIndex;
        }

        public int NewPageIndex
        {
            get { return _newpageindex; }
        }
    }
    #endregion

    #region ShowDropList,ShowCustomInfoSection and PagingButtonType Enumerations
    /// <summary>
    /// 指定页索引输入文本框的显示方式，以便用户可以手工输入页索引。
    /// </summary>
    public enum ShowDropList : byte
    {
        /// <summary>
        /// 从不显示页索引输入文本框。
        /// </summary>
        Never,
        /// <summary>
        /// 自动，选择此项后可以用 <see cref="AspNetPager.ShowBoxThreshold"/> 可控制当总页数达到多少时自动显示页索引输入文本框。
        /// </summary>
        Auto,
        /// <summary>
        /// 总是显示页索引输入文本框。
        /// </summary>
        Always
    }


    /// <summary>
    /// 指定当前页索引和总页数信息的显示方式。
    /// </summary>
    public enum ShowCustomInfoSection : byte
    {
        /// <summary>
        /// 不显示。
        /// </summary>
        Never,
        /// <summary>
        /// 显示在页导航元素之前。
        /// </summary>
        Left,
        /// <summary>
        /// 显示在页导航元素之后。
        /// </summary>
        Right
    }

    /// <summary>
    /// 指定页导航按钮的类型。
    /// </summary>
    public enum PagingButtonType : byte
    {
        /// <summary>
        /// 使用文字按钮。
        /// </summary>
        Text,
        /// <summary>
        /// 使用图片按钮。
        /// </summary>
        Image
    }


    #endregion

    #region AspNetPager Control Designer
    /// <summary>
    /// <see cref="AspNetPager"/> 服务器控件设计器。
    /// </summary>
    public class GridPagerDesigner : PanelContainerDesigner
    {
        /// <summary>
        /// 初始化 PagerDesigner 的新实例。
        /// </summary>
        public GridPagerDesigner()
        {
            this.ReadOnly = true;
        }
        private GridPager wb;

        /// <summary>
        /// 获取用于在设计时表示关联控件的 HTML。
        /// </summary>
        /// <returns>用于在设计时表示控件的 HTML。</returns>
        public override string GetDesignTimeHtml()
        {

            wb = (GridPager)Component;
            wb.RecordCount = 225;
            StringWriter sw = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(sw);
            wb.RenderControl(writer);
            return sw.ToString();
        }

        /// <summary>
        /// 获取在呈现控件时遇到错误后在设计时为指定的异常显示的 HTML。
        /// </summary>
        /// <param name="e">要为其显示错误信息的异常。</param>
        /// <returns>设计时为指定的异常显示的 HTML。</returns>
        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            string errorstr = "创建控件时出错：" + e.Message;
            return CreatePlaceHolderDesignTimeHtml(errorstr);
        }
    }
    #endregion
}
