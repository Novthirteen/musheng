using com.Sconit.Service.Ext.MasterData;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ItemCategoryMgr : ItemCategoryBaseMgr, IItemCategoryMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        private static IList<ItemCategory> cachedAllItemCategory;
        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList GetItemCategory(string code, string desc1, string desc2)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ItemCategory));
            if (code != string.Empty && code != null)
                criteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
            if (desc1 != string.Empty && desc1 != null)
                criteria.Add(Expression.Like("Desc1", desc1, MatchMode.Anywhere));
            if (desc2 != string.Empty && desc2 != null)
                criteria.Add(Expression.Like("Desc2", desc2, MatchMode.Anywhere));
            return criteriaMgrE.FindAll(criteria);
        }
        [Transaction(TransactionMode.Unspecified)]
        public IList GetItemCategory(string code, string desc1)
        {
            return this.GetItemCategory(code, desc1, string.Empty);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ItemCategory> GetCacheAllItemCategory()
        {
            if (cachedAllItemCategory == null)
            {
                cachedAllItemCategory = GetAllItemCategory();
            }
            else
            {
                //检查ItemCategory大小是否发生变化
                DetachedCriteria criteria = DetachedCriteria.For(typeof(ItemCategory));
                criteria.SetProjection(Projections.ProjectionList().Add(Projections.Count("Code")));
                IList<int> count = this.criteriaMgrE.FindAll<int>(criteria);

                if (count[0] != cachedAllItemCategory.Count)
                {
                    cachedAllItemCategory = GetAllItemCategory();
                }
            }

            return cachedAllItemCategory;
        }
        #endregion Customized Methods
    }
}

#region Extend Class
namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ItemCategoryMgrE : com.Sconit.Service.MasterData.Impl.ItemCategoryMgr, IItemCategoryMgrE
    {

    }
}
#endregion