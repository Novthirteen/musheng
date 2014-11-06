using com.Sconit.Service.Ext.MasterData;


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
    public class PermissionCategoryMgr : PermissionCategoryBaseMgr, IPermissionCategoryMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }


        #region Customized Methods

        public IList<PermissionCategory> GetCategoryByType(string type)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(PermissionCategory));
            criteria.Add(Expression.Eq("Type", type));
            return criteriaMgrE.FindAll<PermissionCategory>(criteria);
        }


        #endregion Customized Methods
    }
}


#region Extend Class
namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class PermissionCategoryMgrE : com.Sconit.Service.MasterData.Impl.PermissionCategoryMgr, IPermissionCategoryMgrE
    {
        
    }
}
#endregion
