using com.Sconit.Service.Ext.MasterData;


using System.Collections;
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
    public class UserPreferenceMgr : UserPreferenceBaseMgr, IUserPreferenceMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods

        public bool CheckUserPreferenceUniqueExist(string userCode, string code)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(UserPreference));
            criteria.Add(Expression.Eq("User.Code", userCode));
            criteria.Add(Expression.Eq("Code", code));
            IList temp = criteriaMgrE.FindAll(criteria);

            if (temp.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateUserPreference(IList<UserPreference> userPreferenceList)
        {
            foreach (UserPreference userPreference in userPreferenceList)
            {
                UpdateUserPreference(userPreference);
            }        
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class UserPreferenceMgrE : com.Sconit.Service.MasterData.Impl.UserPreferenceMgr, IUserPreferenceMgrE
    {
        
    }
}
#endregion
