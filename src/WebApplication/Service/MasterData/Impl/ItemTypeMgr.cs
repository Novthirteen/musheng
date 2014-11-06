using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ItemTypeMgr : ItemTypeBaseMgr, IItemTypeMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }

        #region Customized Methods


        [Transaction(TransactionMode.Unspecified)]
        public IList<ItemType> GetItemType(int level, bool includeEmpty)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ItemType));
            criteria.Add(Expression.Eq("Level", level));
            criteria.AddOrder(Order.Asc("Code"));

            IList<ItemType> itemTypeList = criteriaMgrE.FindAll<ItemType>(criteria);

            if (includeEmpty)
            {
                IList<ItemType> newItemTypeList = new List<ItemType>();
                newItemTypeList.Add(new ItemType { });
                foreach (ItemType itemType in itemTypeList)
                {
                    newItemTypeList.Add(itemType);
                }
                return newItemTypeList;
            }

            return itemTypeList;
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ItemTypeMgrE : com.Sconit.Service.MasterData.Impl.ItemTypeMgr, IItemTypeMgrE
    {
    }
}

#endregion Extend Class