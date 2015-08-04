using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Persistence.MasterData.NH;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ItemMapBaseMgr : SessionBase, IItemMapBaseMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }
        public NHItemMapBaseDao entityDao { get; set; }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemMap(String id)
        {
            entityDao.DeleteItemMap(id);
        }

        public virtual ItemMap LoadItemMap(String item)
        {
            return FindById<ItemMap>(item);
        }

        public virtual void UpdateItemMap(ItemMap itemMap)
        {
            criteriaMgr.Update(itemMap);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateItemMap(ItemMap itemMap)
        {
            Create(itemMap);
        }
    }
}
