//------------------------------------------------------------------------------
// Copyright (c) 2004 Whidsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Whidsoft.WebControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Data;
    using System.Drawing.Design;
    using System.Web;
    using System.Web.UI;
    using System.Xml;
    using System.Text;

    /// <summary>
    /// Indicates how a OrgNode should handle expanding and the plus sign.
    /// </summary>
    public enum ExpandableValue
    {
        /// <summary>
        /// Always shows a plus sign and attempts to expand.
        /// </summary>
        Always,

        /// <summary>
        /// Shows a plus sign and allows expanding only when there are children.
        /// </summary>
        Auto,

        /// <summary>
        /// Allows expanding to be attempted once, such as in a databinding case, when
        /// the existence of children is unknown.
        /// </summary>
        CheckOnce
    };



    /// <summary>
    /// OrgNode class: represents a tree node.
    /// Renders the necessary tags to display a treenode and handle its events.
    /// </summary>
    [
    ParseChildren(false),
    ToolboxItem(false),
    ]
    public class OrgNode : IOrgNode
    {
        internal OrgNodeCollection _Nodes;

        string _ID;
        /// <summary>
        /// Returns a reference to the ID.
        /// </summary>
        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;

            }
        }

        IOrgNode _Parent;
        /// <summary>
        /// Returns a reference to the parent object.
        /// </summary>
        public IOrgNode Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;

            }
        }

        string _ImageUrl;
        public string ImageUrl
        {
            get
            {
                return ((_ImageUrl == null) ? String.Empty : _ImageUrl);
            }
            set
            {
                _ImageUrl = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        int _X = 1;

        [Bindable(true),
        Category("Appearance"), //外观
        DefaultValue(""),
        Description("定位状态的横向坐标"),
        ]
        public int X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        int _Y = 1;

        [Bindable(true),
        Category("Appearance"), //外观
        DefaultValue(""),
        Description("定位状态的横向坐标"),
        ]
        public int Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }

        }

        int _Width , _Height ;

        [Bindable(true),
        Category("Appearance"), //外观
        DefaultValue(""),
        Description("定位状态的宽"),
        ]
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }

        }

        [Bindable(true),
        Category("Appearance"), //外观
        DefaultValue(""),
        Description("定位状态的高"),
        ]
        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        string _ImageFolder;

        [Bindable(true),
        Category("Appearance"), //外观
        DefaultValue(""),
        Description("存放图片的目录"),
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


        string _Type;

        /// <summary>
        /// Name of the OrgNodeType to apply to this node
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ]
        public string Type
        {
            get
            {
                //object str = ViewState["Type"];
                return ((_Type == null) ? String.Empty : _Type);
            }
            set
            {
                _Type = value;
            }
        }

        string _Text;
        /// <summary>
        /// Text to display
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ]
        public String Text
        {
            get
            {
                object str = _Text;
                return ((str == null) ? String.Empty : (String)str);
            }
            set
            {
                _Text = value;
            }
        }

        string _NavigateUrl;
        /// <summary>
        /// Url to navigate to when node is selected
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ]
        public String NavigateUrl
        {
            get
            {
                object str = _NavigateUrl;
                return ((str == null) ? String.Empty : (String)str);
            }
            set
            {
                _NavigateUrl = value;
            }
        }

        string _ToolTip;

        /// <summary>
        /// Custom data
        /// </summary>
        [
        Category("Data"),
        DefaultValue("结点描述"),
        PersistenceMode(PersistenceMode.Attribute),
        ]
        public string Description
        {
            get
            {

                object str = _ToolTip;
                if (str == null)
                    return String.Empty;
                else
                    return (string)str;
            }
            set
            {
                _ToolTip = value;
            }
        }


        /// <summary>
        /// Gets the collection of nodes in the control.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(null),
        MergableProperty(false),
        Browsable(false),
        PersistenceMode(PersistenceMode.InnerDefaultProperty),
        ]
        public virtual OrgNodeCollection Nodes
        {
            get { return _Nodes; }
        }

        /// <summary>
        /// Initializes a new instance of a OrgNode.
        /// </summary>
        public OrgNode()
            : base()
        {
            _Nodes = new OrgNodeCollection(this);
        }

        /// <summary>
        /// Event handler for the OnSelectedIndexChange event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        /// <returns>true to bubble, false to cancel</returns>
        protected virtual bool OnSelectedIndexChange(EventArgs e)
        {
            return true;
        }


        /// <summary>
        /// Returns a x.y.z format node index string representing the node's position in the hierarchy.
        /// </summary>
        /// <returns>The x.y.z formatted index.</returns>
        public string GetNodeIndex()
        {

            string strIndex = "";

            /*
            Object node = this;
            while (node is OrgNode)
            {
                if (((OrgNode)node).SibIndex == -1)
                    return String.Empty;
                if (strIndex.Length == 0)
                    strIndex = ((OrgNode)node).SibIndex.ToString();
                else
                    strIndex = ((OrgNode)node).SibIndex.ToString() + "." + strIndex;
                node = ((OrgNode)node).Parent;
            } 
            */
            return strIndex;
        }


        public virtual string OrgNodeHtml()
        {
            return OrgNodeHtml(this);
        }

        public virtual string OrgNodeHtml(OrgNode node)
        {
            return OrgNodeHtml("", node);
        }

        public virtual string OrgNodeAbsoluteHtml(OrgNode node)
        {
            string sbTemplate = "<TABLE Width=100 align=center border=0 style=\"LEFT: {5}px; TOP: {6}px; WIDTH: {7}px; HEIGHT: {8}px; POSITION: absolute; \" ><TR><TD align=center style=\"font-size: 12px;font-family: Verdana, Arial;padding : 5px 5px 5px 5px;border:thin solid  orange;background-color: lightgrey\" title='{1}'>{3}<a href='{4}'>{0}</a></TD></TR></TABLE>";

            return OrgNodeHtml(sbTemplate, node);
        }


        public virtual string OrgNodeHtml(string templateHtml, OrgNode xNode)
        {
            string imageUrl;
            string sbTemplate;

            if (templateHtml == null || templateHtml == "")
            {
                sbTemplate = "";
                //sbTemplate = "<TABLE Width=100 align=center border=0><TR><TD align=center class=\"orgchartTable\" title='{1}'>{3}<a href='{4}'>{0}</a></TD></TR></TABLE>";
                //sbTemplate = "<TABLE Width=100 align=center border=0><TR><TD align=center style=\"font-size: 12px;font-family: Verdana, Arial;padding : 5px 5px 5px 5px;border:thin solid  orange;background-color: lightgrey\" title='{1}'>{3}<a href='{4}'>{0}</a></TD></TR></TABLE>";
            }
            else
                sbTemplate = templateHtml;

            StringBuilder sbHtml = new StringBuilder();

            string[] nodeData = new string[10];
            nodeData[0] = (xNode.Text == null) ? "" : xNode.Text;
            nodeData[1] = (xNode.Description == null) ? "" : xNode.Description;
            nodeData[2] = (xNode.Type == null) ? "" : xNode.Type;
            nodeData[3] = (xNode.ImageUrl == "") ? "" : "<img border=0 src='" + xNode.ImageUrl + "'>";
            nodeData[4] = (xNode.NavigateUrl == null) ? "" : xNode.NavigateUrl;

            nodeData[5] = xNode.X.ToString();
            nodeData[6] = xNode.Y.ToString();
            nodeData[7] = xNode.Width.ToString();
            nodeData[8] = xNode.Height.ToString();

            imageUrl = xNode.ImageUrl;
            if (imageUrl == null || imageUrl == "")
            {
                switch (xNode.Type.ToUpper())
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
            nodeData[3] = (imageUrl == "") ? "" : "<img border=0 src='" + imageUrl + "'>";

            sbHtml.AppendFormat(sbTemplate, nodeData);

            return sbHtml.ToString();
        }


    }
}
