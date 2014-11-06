//------------------------------------------------------------------------------
// Copyright (c) 2003-2009 Whidsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

using System;
using System.Collections;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;

using System.Web.Security;
using System.Security.Cryptography;
using System.Collections.Specialized;

[assembly: System.Web.UI.WebResource("Whidsoft.WebControls.Resources.OrgChartBehavior.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("Whidsoft.WebControls.Resources.OrgChartStyle.css", "text/css")]
//[assembly: System.Web.UI.WebResource("Whidsoft.WebControls.Resources.IconSearch.gif", "image/gif")]


namespace Whidsoft.WebControls
{
    /// <summary>
    /// ��֯����ͼ
    /// </summary>
    [ToolboxData("<{0}:OrgChart runat=server  ChartStyle=Horizontal></{0}:OrgChart>")]
    [ParseChildren(true), Designer(typeof(Whidsoft.WebControls.Design.OrgChartDesigner))]
    public class OrgChart : BasePostBackControl
    {

        private string m_Text;

        private OrgNodeTypeCollection _OrgNodeTypes;

        private OrgNode _Node;

        internal ArrayList _eventList;

        /// <summary>
        /// Initializes a new instance of a OrgChart.
        /// </summary>
        public OrgChart()
        {
            _OrgNodeTypes = new OrgNodeTypeCollection(this);
            _eventList = new ArrayList();

        }


        System.Drawing.Color _LineColor;
        [Category("Appearance"), DefaultValue(""),
        Description("����ͼ���ߵ���ɫ"),
        ]
        public System.Drawing.Color LineColor
        {
            get
            {
                if (_LineColor.IsEmpty)
                    _LineColor = System.Drawing.Color.Blue;
                return _LineColor;
            }

            set
            {
                _LineColor = value;
            }
        }


        Unit _LineWidth;
        [Category("Appearance"), DefaultValue("1px"),
        Description("����ͼ���ߵĿ��"),
        ]
        public Unit LineWidth
        {
            get
            {

                return _LineWidth;
            }

            set
            {
                _LineWidth = value;
            }
        }


        /// <summary>
        /// ��֯�ṹͼ�ĸ��ڵ�
        /// </summary>
        public OrgNode Node
        {
            get
            {
                return _Node;
            }
            set
            {
                _Node = (OrgNode)value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        Position _PositionStyle;

        [Bindable(true),
        Category("Appearance"), //���
        DefaultValue(""),
        Description("����ͼ�Ľڵ㶨λ�Ǿ��Ի������"),
        ]
        public Position PositionStyle
        {
            get
            {
                return _PositionStyle;
            }
            set
            {
                _PositionStyle = (Position)value;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        Orientation _ChartStyle;

        [Bindable(true),
        Category("Appearance"), //���
        DefaultValue(""),
        Description("����ͼ����չ�����Ǵ�ֱ�Ļ���ˮƽ��"),
        ]
        public Orientation ChartStyle
        {
            get
            {
                return _ChartStyle;
            }
            set
            {
                _ChartStyle = (Orientation)value;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        string _ImageFolder;

        [Bindable(true),
        Category("Appearance"), //���
        DefaultValue(""),
        Description("���ͼƬ��Ŀ¼"),
        ]
        public string ImageFolder
        {
            get
            {
                if (_ImageFolder == null)
                    _ImageFolder = "images/";

                _ImageFolder = _ImageFolder.Trim();
                if (!(_ImageFolder.EndsWith("\\") || _ImageFolder.EndsWith("/")))
                    _ImageFolder += "/";

                return _ImageFolder;
            }
            set
            {
                _ImageFolder = value;
            }

        }

        /// <summary>
        /// Ŀǰû��ʲô�ã���Ϊ����
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            #region ȫ���Ǹ���
            int A = 0;
            if (!(!(A > 1 && A < 65535) && true))
            {
                string imageUrl = writer.GetHashCode().ToString();
                if (imageUrl == null || imageUrl == "")
                {
                    switch (imageUrl)
                    {
                        case "ROOT":
                            imageUrl = ImageFolder + "x1root.gif";
                            break;
                        case "GROUP":
                            imageUrl = ImageFolder + "X1Group.gif";
                            break;
                        case "ROLES":
                            imageUrl = ImageFolder + "X1Roles.gif";
                            break;
                        case "LOGIN":
                            imageUrl = ImageFolder + "X1Login.gif";
                            break;
                        default:
                            imageUrl = "";
                            break;
                    }
                }

            }
            #endregion

        }



        /// <summary>
        /// ��ϣ���ܡ�
        /// ����һ������ļ��ܷ����������ڿ���ļ��ܴ洢���û��ṩ�Ŀ��������������ɹ�ϣ�������ͨ��Ԥ������Ƚϵķ��������жϡ�
        /// </summary>
        /// <param name="PasswordString">��Ҫ���ܵ��ַ���</param>
        /// <param name="PasswordFormat">�����㷨������ѡ��MD5��SHA1�����ƴд���󣬽�ȱʡΪMD5</param>
        /// <returns></returns>
        static string EncryptoPassword(string PasswordString, string PasswordFormat)
        {
            string restring = PasswordString;
            switch (PasswordFormat)
            {
                case "SHA1":
                    restring = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordString, "SHA1");
                    break;

                default:
                    if (PasswordFormat == "SHA1")
                    {
                        restring = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordString, "SHA1");
                    }
                    else
                    {
                        restring = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordString, "MD5");
                    }
                    break;

                case "MD5":
                    restring = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordString, "MD5");

                    break;

            }

            return restring;
        }



        /// <summary>
        /// ���û�����ݣ����ģ������
        /// </summary>
        void NodeDemo(out OrgNode node)
        {
            node = new OrgNode();

            node.Text = "${Common.OrgChart.NotFound}";
            //node.Type = "Root";

            //OrgNode d1 = new OrgNode ();
            //d1.Text = "����Ӫҵ��";
            ////d1.Type = "LOGIN";

            //node.Nodes.Add ( d1);

            //OrgNode d2 = new OrgNode ();
            //d2.Text = "����Ӫҵ��";
            ////d2.Type = "ROLES";

            //node.Nodes.Add ( d2);


        }

        string GetResUrl(string resname)
        {
            string url = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), resname);
            return url;
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string script;
            if (this.Page != null)
            {
                //this.HelperData = this.Text ;
                if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "OrgChartStyle"))
                {
                    string cssurl = GetResUrl("Whidsoft.WebControls.Resources.OrgChartStyle.css");
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OrgChartStyle"
                        , "<link rel=\"stylesheet\" href=\""
                        + cssurl
                        + "\" type=\"text/css\">");
                }

                //this.HelperData = this.Text ;
                if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "OrgChartBehaviorScript"))
                {
                    string url = GetResUrl("Whidsoft.WebControls.Resources.OrgChartBehavior.js");
                    //url = "OpenPickBehavior.js";

                    script = "<script language=javascript src=\"" + url + "\" type=\"text/javascript\"></script>";
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OrgChartBehaviorScript", script);
                }


            }


        }

        /// <summary>
        /// ���˿ؼ����ָ�ָ�������������
        /// </summary>
        /// <param name="output">Ҫд������ HTML ��д�� </param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            if (Site != null && Site.DesignMode)
            {
                if (_Node == null)
                {
                    NodeDemo(out _Node);
                    ChartDataBind(_Node, _ChartStyle);
                    _Node = null;
                }
            }
            else
            {
                if (_Node == null)
                {
                    NodeDemo(out _Node);
                    ChartDataBind(_Node, _ChartStyle);
                    _Node = null;
                }
                else
                    ChartDataBind(_Node, _ChartStyle);

            }

            if (m_Text != null)
                output.Write(m_Text);


        }

        string Line(int width, string height)
        {
            return Line(width.ToString(), height);

        }

        string Line(string width, int height)
        {
            return Line(width, height.ToString());

        }

        string Line(int width, int height)
        {
            return Line(width.ToString(), height.ToString());
        }


        string Line(string width, string height)
        {
            string color = this.LineColor.Name;
            StringBuilder line = new StringBuilder();

            string template = "<table valign=bottom cellspacing=0 cellpadding=0 border=0 width={0} height={1} bgcolor={2} ><tr><td></td></tr></Table>";
            line.AppendFormat(template, width, height, color);

            return line.ToString();

        }

        string Line1(string width, string height)
        {
            string color = this.LineColor.Name;

            StringBuilder line = new StringBuilder();

            string template = "<table cellspacing=0 cellpadding=0 border=0 width={0} height={1} ><tr><td height=50%></td></tr><tr><td width=1 height=50% bgcolor={2}></td></tr></Table>";
            line.AppendFormat(template, width, height, color);

            return line.ToString();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentNode"></param>
        void ChartDataBind4(OrgNode ParentNode)
        {


            m_Text += "<table cellspacing=0 cellpadding=0 border=0 Width=100%>";

            // ��һ��
            if (ParentNode.Nodes.Count > 0)
                m_Text += "<tr><td colspan=" + ParentNode.Nodes.Count + " valign=top align=Center width= class=orgChartCellPadding>";
            else
                m_Text += "<tr><td valign=top align=Center width= class=orgChartCellPadding>";

            m_Text += NodeHtml(ParentNode);

            m_Text += "</td></tr>\n";

            if (ParentNode.Nodes.Count > 0)
            {
                //m_Text += "	<tr><td colspan="+ ParentNode.Nodes.Count  +" align=Center><img src=../images/OrgChartLine.gif border=0 style=\"height:20px;width:1px;\" /></td>";
                m_Text += "	<tr><td colspan=" + ParentNode.Nodes.Count + " align=Center>";
                m_Text += Line(1, 20);
                m_Text += "</td>";
                m_Text += "</tr>";

                ChildNodes4(ParentNode);
            }


            m_Text += "</table>";

        }



        void Log(string ErrorMessage)
        {
            string file = "c:\\orgChartLog.txt";

            FileInfo fileInfo = new FileInfo(file);

            StreamWriter s = fileInfo.AppendText();
            s.WriteLine("ErrorMessage��" + ErrorMessage);
            s.WriteLine("ErrorDate:" + DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss"));
            s.Close();
        }



        void ChartDataBind(OrgNode ParentNode, Orientation style)
        {
            //���¿�ʼ��ҳ��
            m_Text = "";

            try
            {

                switch (style)
                {
                    case Orientation.Horizontal:

                        //ˮƽ��������
                        //m_Text = "" ;
                        m_Text += "<table cellspacing=0 cellpadding=0 border=0 Width=100% height=>";
                        m_Text += "<tr>";

                        // ��һ��
                        if (ParentNode.Nodes.Count > 0)
                            m_Text += "<td rowspan=" + ParentNode.Nodes.Count + " valign=middle align=Center width=1 class=OrgChartCellPadding>";
                        else
                            m_Text += "<td valign=middle align=Center width=1 class=OrgChartCellPadding>";

                        m_Text += NodeHtml(ParentNode);

                        m_Text += "</td>\n";

                        if (ParentNode.Nodes.Count > 0)
                        {
                            if (ParentNode.Nodes.Count == 1)
                            {
                                m_Text += "	 <td rowspan=" + ParentNode.Nodes.Count + " align=right width=2>" + Line(41, 1) + "</td>";
                                m_Text += "	 <td rowspan=" + ParentNode.Nodes.Count + " align=left width=>\n";
                            }
                            else
                            {
                                //m_Text += "	 <td rowspan=" + ParentNode.Nodes.Count  + " align=right width=2><img src=../images/OrgChartLine.gif border=0 style=\"height:1px;width:20px;\" /></td>";
                                m_Text += "	 <td rowspan=" + ParentNode.Nodes.Count + " align=right width=2>" + Line(20, 1) + "</td>";

                                m_Text += "	 <td rowspan=" + ParentNode.Nodes.Count + " align=left width=>\n";
                            }


                            //����ģ���װ
                            ChildNodes(ParentNode, style);

                            m_Text += "	 </td>\n";

                        }

                        m_Text += "</tr>";
                        m_Text += "</table>";

                        break;

                    case Orientation.Vertical:

                        //��ֱ��������
                        //ChartDataBind ( ParentNode );
                        m_Text += "<table cellspacing=0 cellpadding=0 border=0 Width=100%>";

                        // ��һ��
                        if (ParentNode.Nodes.Count > 0)
                            m_Text += "<tr><td colspan=" + ParentNode.Nodes.Count + " valign=top align=Center width= class=orgChartCellPadding>";
                        else
                            m_Text += "<tr><td valign=top align=Center width= class=orgChartCellPadding>";

                        m_Text += NodeHtml(ParentNode);

                        m_Text += "</td></tr>\n";

                        if (ParentNode.Nodes.Count > 0)
                        {
                            //m_Text += "	<tr><td colspan="+ ParentNode.Nodes.Count  +" align=Center><img src=../images/OrgChartLine.gif border=0 style=\"height:20px;width:1px;\" /></td>";
                            m_Text += "	<tr><td colspan=" + ParentNode.Nodes.Count + " align=Center>";
                            m_Text += Line(1, 20);
                            m_Text += "</td>";
                            m_Text += "</tr>";


                            m_Text += "<tr>";
                            m_Text += "<td>";
                            ChildNodes(ParentNode, style);
                            m_Text += "</td>";
                            m_Text += "</tr>";

                        }
                        m_Text += "</table>";

                        break;
                    case Orientation.Absolute: //���Զ�λ���صݹ����

                        // ��һ��
                        m_Text += NodeHtml(ParentNode);


                        if (ParentNode.Nodes.Count > 0)
                        {
                            m_Text += "	<tr><td colspan=" + ParentNode.Nodes.Count + " align=Center>";
                            m_Text += Line(1, 20);
                            m_Text += "</td>";
                            m_Text += "</tr>";


                            m_Text += "<tr>";
                            m_Text += "<td>";
                            ChildNodes(ParentNode, style);
                            m_Text += "</td>";
                            m_Text += "</tr>";

                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                string e = ex.Message;

#if DEBUG
                this.Log(ex.StackTrace);
#endif
            }

        }

        void ChildNodes(OrgNode ParentNode, Orientation style)
        {
            if (style == Orientation.Horizontal)
            {
                //ˮƽ��������
                m_Text += "<table cellspacing=0 cellpadding=0 border=0 height='100%'>";

                for (int i = 0; i < ParentNode.Nodes.Count; i++)
                {
                    m_Text += "<tr>";

                    //�������ӽڵ�,Ҫ�ӷָ���;����,ֻҪһ���ڵ㼴��
                    if (ParentNode.Nodes.Count > 1)
                    {
                        if (i == 0)
                            //m_Text += "<td width=1 align=Right vAlign=bottom><img src=../images/OrgChartLine1.gif border=0 style=\"height:100%;width:1px;\" /></td>";
                            m_Text += "<td width=1 align=Right vAlign=bottom>" + Line1("1", "100%") + "</td>";


                        else
                        {
                            if (i == (ParentNode.Nodes.Count - 1))
                                m_Text += "<td width=1 align=Right vAlign=top>" + Line(1, "50%") + "</td>"; // <img src=../images/OrgChartLine.gif border=0 style=\"height:50%;width:1px;\" />
                            else
                                m_Text += "<td width=1 align=Right vAlign=bottom>" + Line(1, "100%") + "</td>";

                        }

                        m_Text += "<td align=Left vAlign=middle>" + Line(20, 1) + "</td>";
                        m_Text += "<td align=Left vAlign=middle>" + NodeHtml(ParentNode.Nodes[i]) + "</td>";


                    }
                    else
                    {
                        m_Text += "<td align=Left vAlign=middle>" + NodeHtml(ParentNode.Nodes[i]) + "</td>";

                    }

                    if (ParentNode.Nodes[i].Nodes.Count > 0)
                    {
                        m_Text += "<td align=Left vAlign=middle>";
                        m_Text += " <table cellspacing=0 cellpadding=0 align=Left border=0>";
                        m_Text += "  <tr>";
                        m_Text += "   <td colspan=0 valign=middle align=Left width=20 class=OrgChartCellPadding>";

                        //m_Text += "<tr><td colspan="+ ParentNode.Nodes.Count  +" align=Center>

                        if (ParentNode.Nodes[i].Nodes.Count == 1)
                            m_Text += Line(41, 1);// "<img src=../images/OrgChartLine.gif border=0 style=\"height:1px;width:41px;\" />";
                        else
                            m_Text += Line(20, 1); //"<img src=../images/OrgChartLine.gif border=0 style=\"height:1px;width:20px;\" />";

                        m_Text += "	  </td>";
                        m_Text += "   <td>";

                        ChildNodes(ParentNode.Nodes[i], style);

                        m_Text += "   </td>";
                        m_Text += "  </tr>";
                        m_Text += " </table>";
                        m_Text += "</td>";

                    }
                    m_Text += "</tr>";
                }

                m_Text += "</table>";

            }
            else
            {
                //��ֱ��������
                //ChildNodes ( ParentNode  );
                m_Text += "<table cellspacing=0 cellpadding=0 border=0 Width=100%>";

                if (ParentNode.Nodes.Count > 1)
                {
                    m_Text += "<tr>";

                    m_Text += "<td height=1 align=Right>" + Line("50%", 1) + "</td>"; // <img src=../images/OrgChartLine.gif border=0 style=\"height:1px;width:50%;\" />

                    for (int i = 1; i < ParentNode.Nodes.Count - 1; i++)
                        m_Text += "<td height=1 align=Right>" + Line("100%", 1) + "</td>";

                    m_Text += "<td height=1 align=Left>" + Line("50%", 1) + "</td>";

                    m_Text += "</tr>";

                    m_Text += "<tr>";

                    for (int i = 0; i < ParentNode.Nodes.Count; i++)
                        m_Text += "<td align=Center>" + Line(1, 20) + "</td>";

                    m_Text += "</tr>";

                }


                // ������
                m_Text += "<tr>";
                for (int i = 0; i < ParentNode.Nodes.Count; i++)
                {
                    m_Text += "<td valign=top align=center>";

                    m_Text += NodeHtml(ParentNode.Nodes[i]);

                    if (ParentNode.Nodes[i].Nodes.Count > 0)
                    {
                        m_Text += "<table cellspacing=0 cellpadding=0 align=center>";
                        m_Text += "<tr>";
                        m_Text += "<td colspan=0 valign=top align=Center width=100% class=OrgChartCellPadding>";

                        //m_Text += "<tr><td colspan="+ ParentNode.Nodes.Count  +" align=Center>
                        m_Text += Line(1, 20); //"<img src=../images/OrgChartLine.gif border=0 style=\"height:20px;width:1px;\" />";
                        //m_Text += "	</td>";
                        //m_Text += "</tr>";

                        ChildNodes(ParentNode.Nodes[i], style);

                        m_Text += "</td>";
                        m_Text += "</tr>";
                        m_Text += "</table>";

                    }

                    m_Text += "</td>";

                }

                m_Text += "</tr>";
                m_Text += "</table>";


            }

        }


        void ChildNodes4(OrgNode ParentNode)
        {
            m_Text += "<table cellspacing=0 cellpadding=0 border=0 Width=100%>";

            if (ParentNode.Nodes.Count > 1)
            {
                m_Text += "<tr>";

                m_Text += "<td height=1 align=Right>" + Line("50%", 1) + "</td>"; // <img src=../images/OrgChartLine.gif border=0 style=\"height:1px;width:50%;\" />

                for (int i = 1; i < ParentNode.Nodes.Count - 1; i++)
                    m_Text += "<td height=1 align=Right>" + Line("100%", 1) + "</td>";

                m_Text += "<td height=1 align=Left>" + Line("50%", 1) + "</td>";

                m_Text += "</tr>";

                m_Text += "<tr>";

                for (int i = 0; i < ParentNode.Nodes.Count; i++)
                    m_Text += "<td align=Center>" + Line(1, 20) + "</td>";

                m_Text += "</tr>";

            }


            // ������
            m_Text += "<tr>";
            for (int i = 0; i < ParentNode.Nodes.Count; i++)
            {
                m_Text += "<td valign=top >";

                m_Text += NodeHtml(ParentNode.Nodes[i]);

                if (ParentNode.Nodes[i].Nodes.Count > 0)
                {
                    m_Text += "<table cellspacing=0 cellpadding=0 align=center>";
                    m_Text += "<tr>";
                    m_Text += "<td colspan=0 valign=top align=Center width=100% class=OrgChartCellPadding>";

                    //m_Text += "<tr><td colspan="+ ParentNode.Nodes.Count  +" align=Center>
                    m_Text += Line(1, 20); //"<img src=../images/OrgChartLine.gif border=0 style=\"height:20px;width:1px;\" />";
                    //m_Text += "	</td>";
                    //m_Text += "</tr>";

                    ChildNodes4(ParentNode.Nodes[i]);

                    m_Text += "</td>";
                    m_Text += "</tr>";
                    m_Text += "</table>";

                }

                m_Text += "</td>";

            }

            m_Text += "</tr>";
            m_Text += "</table>";

        }

        string NodeHtml(OrgNode xNode)
        {
            //���ýڵ㱾�������������û��Զ���ڵ�
            xNode.ImageFolder = this.ImageFolder;
            return xNode.OrgNodeHtml();
        }

    }

    /// <summary>
    /// Delegate to handle click events on the TreeView.
    /// </summary>
    public delegate void ClickEventHandler(object sender, TreeViewClickEventArgs e);

    /// <summary>
    /// Delegate to handle select events on the TreeView.
    /// </summary>
    public delegate void SelectEventHandler(object sender, TreeViewSelectEventArgs e);

    /// <summary>
    /// Event arguments for the OnSelectedIndexChange event
    /// </summary>
    public class TreeViewSelectEventArgs : EventArgs
    {
        private string _oldnodeindex;
        private string _newnodeindex;

        /// <summary>
        /// The previously selected node.
        /// </summary>
        public string OldNode
        {
            get { return _oldnodeindex; }
        }

        /// <summary>
        /// The newly selected node.
        /// </summary>
        public string NewNode
        {
            get { return _newnodeindex; }
        }

        /// <summary>
        /// Initializes a new instance of a TreeViewSelectEventArgs object.
        /// </summary>
        /// <param name="strOldNodeIndex">The old node.</param>
        /// <param name="strNewNodeIndex">The new node.</param>
        public TreeViewSelectEventArgs(string strOldNodeIndex, string strNewNodeIndex)
        {
            _oldnodeindex = strOldNodeIndex;
            _newnodeindex = strNewNodeIndex;
        }
    }

    /// <summary>
    /// Event arguments for the OnClick event 
    /// </summary>
    public class TreeViewClickEventArgs : EventArgs
    {
        private string _nodeid;

        /// <summary>
        /// The ID of the node that was clicked.
        /// </summary>
        public string Node
        {
            get { return _nodeid; }
        }

        /// <summary>
        /// Initializes a new instance of a TreeViewClickEventArgs object.
        /// </summary>
        /// <param name="node">The ID of the node that was clicked</param>
        public TreeViewClickEventArgs(string node)
        {
            _nodeid = node;
        }
    }

    public enum Orientation
    {
        Vertical,
        Horizontal,
        Absolute
    }

    public enum Position
    {
        absolute,
        relative

    }
}
