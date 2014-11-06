using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class WorkCenterMgr : WorkCenterBaseMgr, IWorkCenterMgr
    {
        
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods

        public IList<WorkCenter> GetWorkCenter(string regionCode)
        {
            return GetWorkCenter(regionCode, false);
        }

        public IList<WorkCenter> GetWorkCenter(Region region)
        {
            return GetWorkCenter(region.Code, false);
        }

        public IList<WorkCenter> GetWorkCenter(string regionCode, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(WorkCenter));
            criteria.CreateAlias("Region", "region");
            criteria.Add(Expression.Eq("region.Code", regionCode));
            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }
            return criteriaMgrE.FindAll<WorkCenter>(criteria);
        }

        public IList<WorkCenter> GetWorkCenter(Region region, bool includeInactive)
        {
            return GetWorkCenter(region.Code, includeInactive);
        }

        public void DeleteWorkCenterByParent(String parentCode)
        {
            entityDao.DeleteWorkCenterByParent(parentCode);
        }

        #endregion Customized Methods
    }
}


#region Extend Class
namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class WorkCenterMgrE : com.Sconit.Service.MasterData.Impl.WorkCenterMgr, IWorkCenterMgrE
    {
        
        
    }
}
#endregion
