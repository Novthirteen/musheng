using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class StorageAreaMgr : StorageAreaBaseMgr, IStorageAreaMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }

        

        #region Customized Methods

        public IList<StorageArea> GetStorageArea(Location location)
        {
            return GetStorageArea(location.Code,string.Empty);
        }

        public IList<StorageArea> GetStorageArea(string locationCode)
        {
            return GetStorageArea(locationCode, string.Empty);
        }

        public IList<StorageArea> GetStorageArea(string locationCode, string AreaCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<StorageArea>();
            criteria.Add(Expression.Eq("Location.Code", locationCode));
            if (AreaCode != null && AreaCode != string.Empty)
            {
                criteria.Add(Expression.Like("Code", AreaCode));
            }
            return this.criteriaMgrE.FindAll<StorageArea>(criteria);
        }

        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class StorageAreaMgrE : com.Sconit.Service.MasterData.Impl.StorageAreaMgr, IStorageAreaMgrE
    {
       
    }
}
#endregion
