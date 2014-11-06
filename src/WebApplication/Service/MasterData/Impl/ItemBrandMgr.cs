using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ItemBrandMgr : ItemBrandBaseMgr, IItemBrandMgr
    {
        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ItemBrand> GetItemBrandIncludeEmpty()
        {
            IList<ItemBrand> itemBrandList = entityDao.GetAllItemBrand();
            IList<ItemBrand> newItemBrandList = new List<ItemBrand>();
            newItemBrandList.Add(new ItemBrand());
            foreach (ItemBrand itemBrand in itemBrandList)
            {
                newItemBrandList.Add(itemBrand);
            }

            return newItemBrandList;
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ItemBrandMgrE : com.Sconit.Service.MasterData.Impl.ItemBrandMgr, IItemBrandMgrE
    {
    }
}

#endregion Extend Class