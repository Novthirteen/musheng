//------------------------------------------------------------------------------
// Copyright (c) 2004 Whidsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Whidsoft.WebControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// OrgNodeType class
    /// </summary>
    [
    ParseChildren(true),
    ToolboxItem(false),
    ]
    public class OrgNodeType //: TreeBase
    {
		/// <summary>
		/// 从原TreeBase中拿来
		/// </summary>
		internal Object _Parent;

		/// <summary>
		/// Returns a reference to the parent object.
		/// </summary>
		public object Parent
		{
			get
			{
				return _Parent;
			}
		}


        /// <summary>
        /// Returns the string representation of the OrgNodeType.
        /// </summary>
        /// <returns>The string representation of the OrgNodeType.</returns>
        public override string ToString()
        {
            if (Type != String.Empty)
            {
                return Type;
            }

            return base.ToString();
        }


		string _Type;
        /// <summary>
        /// The name of this OrgNodeType.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ]
        public String Type
        {
            get 
            {
				/*
                object str = ViewState["Type"];
                return ((str == null) ? String.Empty : (String)str);
				*/
				return ( _Type == null ) ? String.Empty : (String)_Type ;
            }
            set
            {
                //ViewState["Type"] = value;
				_Type = value ;
            }
        }

		string _NavigateUrl ;

        /// <summary>
        /// The URL that TreeNodes of this type should navigate to when clicked.
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
                //object str = ViewState["NavigateUrl"];
                //return ((str == null) ? String.Empty : (String)str);
				return ( _NavigateUrl  == null ) ? String.Empty : (String)_NavigateUrl ;

            }
            set
            {
                //ViewState["NavigateUrl"] = value;
				_NavigateUrl  = value ;

            }
        }

    }
}
