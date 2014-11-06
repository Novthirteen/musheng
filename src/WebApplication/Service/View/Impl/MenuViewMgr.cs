using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.View;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Entity.View;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View.Impl
{
    [Transactional]
    public class MenuViewMgr : MenuViewBaseMgr, IMenuViewMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<MenuView> GetMenuViewByIsActive(bool isActive)
        {
            DetachedCriteria criteria = DetachedCriteria.For<MenuView>();
            criteria.Add(Expression.Eq("IsActive", isActive));
            criteria.Add(Expression.Eq("MenuRelationIsActive", isActive));
            criteria.AddOrder(Order.Asc("Level"));
            criteria.AddOrder(Order.Asc("Seq"));
            criteria.AddOrder(Order.Asc("Code"));
            return this.criteriaMgrE.FindAll<MenuView>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public MenuView GetMenuView(string key)
        {
            DetachedCriteria criteria = DetachedCriteria.For<MenuView>();
            criteria.Add(Expression.Eq("Code", key));
            IList<MenuView> menuViewList = this.criteriaMgrE.FindAll<MenuView>(criteria, 0, 1);
            if (menuViewList != null && menuViewList.Count == 1)
            {
                return menuViewList[0];
            }
            return null;
        }

        #endregion Customized Methods
    }
}



#region Extend Class


namespace com.Sconit.Service.Ext.View.Impl
{
    [Transactional]
    public partial class MenuViewMgrE : com.Sconit.Service.View.Impl.MenuViewMgr, IMenuViewMgrE
    {

    }
}
#endregion
