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
    public class EntityPreferenceMgr : EntityPreferenceBaseMgr, IEntityPreferenceMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods

        public IList<EntityPreference> GetAllEntityPreferenceOrderBySeq()
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(EntityPreference)).AddOrder(Order.Asc("Seq"));
            return criteriaMgrE.FindAll<EntityPreference>(criteria);
        }

        #endregion Customized Methods
    }
}


#region Extend Class
namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class EntityPreferenceMgrE : com.Sconit.Service.MasterData.Impl.EntityPreferenceMgr, IEntityPreferenceMgrE
    {
        
    }
}
#endregion
