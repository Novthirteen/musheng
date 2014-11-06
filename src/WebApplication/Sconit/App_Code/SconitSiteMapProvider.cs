using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.View;
using com.Sconit.Entity.View;

/// <summary>
///SconitSiteMapProvider 的摘要说明
/// </summary>
/// 
namespace com.Sconit.Web
{
    public class SconitSiteMapProvider : StaticSiteMapProvider
    {
        private SiteMapNode rootNode = null;

        private IMenuViewMgrE menuViewMgrE = null;

        private log4net.ILog log = log4net.LogManager.GetLogger("com.Sconit.Web.SconitSiteMapProvider");

        //private IDictionary<string, SiteMapNode> dicMenu = new Dictionary<string, SiteMapNode>();
        private IList<MenuView> menuList = new List<MenuView>();

        // Some basic state to help track the initialization state of the provider.
        private bool initialized = false;
        public virtual bool IsInitialized
        {
            get
            {
                return initialized;
            }
        }

        protected override void Clear()
        {
            lock (this)
            {
                rootNode = null;
                base.Clear();
            }

        }

        public override SiteMapNode BuildSiteMap()
        {
            lock (this)
            {
                this.ResourceKey = "";
                if (!IsInitialized)
                {
                    throw new Exception("BuildSiteMap called incorrectly.");
                }
                if (null == rootNode)
                {

                    try
                    {
                        // Start with a clean slate
                        this.Clear();

                        IList<MenuView> menuViewList = menuViewMgrE.GetMenuViewByIsActive(true);

                        foreach (MenuView menu in menuViewList)
                        {
                            try
                            {
                                SiteMapNode node = new SiteMapNode(this, menu.Code, menu.PageUrl, menu.Code, menu.ImageUrl);
                                node.ResourceKey = menu.Type + "." + menu.Code + "." + menu.Seq;
                                //node.ResourceKey = menu.Description;
                                if (menu.Code != menu.ParentCode && menu.Level != 0)
                                {
                                    SiteMapNode parentSiteMapNode = this.FindSiteMapNodeFromKey(menu.ParentCode);
                                    if (parentSiteMapNode != null)
                                    {
                                        AddNode(node, parentSiteMapNode);//this.dicMenu[menu.ParentNode.Code]
                                    }
                                    else
                                    {
                                        menuList.Add(menu);
                                    }
                                }
                                else if (menu.Code == menu.ParentCode && menu.Level == 0)
                                {
                                    rootNode = node;
                                    AddNode(rootNode, null);
                                }
                                else
                                {
                                    menuList.Add(menu);
                                }
                            }
                            catch (Exception e)
                            {
                                log.Error(e.Message, e);
                                if (this.FindSiteMapNodeFromKey(menu.Code) == null)
                                {
                                    menuList.Add(menu);
                                }
                            }
                            //this.dicMenu.Add(menu.Code, node);
                        }

                        if (menuList.Count > 0)
                        {
                            bool UnallocatedNodeBool = true;
                            SiteMapNode UnallocatedNode = null;
                            foreach (MenuView menu in menuList)
                            {
                                SiteMapNode siteMapNode = this.FindSiteMapNodeFromKey(menu.Code);
                                if (siteMapNode == null)
                                {
                                    if (UnallocatedNodeBool)
                                    {
                                        UnallocatedNode = new SiteMapNode(this, "Unallocated", "", "Menu.Unallocated", "Unallocated");
                                        AddNode(UnallocatedNode, rootNode);
                                        UnallocatedNodeBool = false;
                                    }
                                    try
                                    {
                                        SiteMapNode node = new SiteMapNode(this, menu.Code, menu.PageUrl, menu.Code, menu.ImageUrl);
                                        node.ResourceKey = menu.Type + "." + menu.Code + "." + menu.Seq;
                                        AddNode(node, UnallocatedNode);
                                    }
                                    catch (Exception e)
                                    {
                                        log.Error(e.Message, e);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        log.Error(e.Message, e);
                    }
                }

            }
            //返回构建后的根节点。
            return rootNode;
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return RootNode;
        }

        public override SiteMapNode RootNode
        {
            get
            {
                SiteMapNode temp = BuildSiteMap();
                return temp;
            }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes)
        {
            if (IsInitialized)
                return;

            this.EnableLocalization = false;
            //this.ResourceKey = "Language";

            base.Initialize(name, attributes);

            this.menuViewMgrE = ServiceLocator.GetService<IMenuViewMgrE>("MenuViewMgr.service");

            initialized = true;
        }
    }
}