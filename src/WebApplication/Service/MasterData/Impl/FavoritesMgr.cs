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
    public class FavoritesMgr : FavoritesBaseMgr, IFavoritesMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }


        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<Favorites> GetFavorites(string userCode, string type)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Favorites>();
            criteria.Add(Expression.Eq("User.Code", userCode));
            criteria.Add(Expression.Eq("Type", type));
            criteria.AddOrder(Order.Desc("Id"));
            return criteriaMgrE.FindAll<Favorites>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public bool CheckFavoritesUniqueExist(string userCode, string type, string pageName)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Favorites>();
            criteria.Add(Expression.Eq("User.Code", userCode));
            criteria.Add(Expression.Eq("Type", type));
            criteria.Add(Expression.Eq("PageName", pageName));
            IList<Favorites> temp = criteriaMgrE.FindAll<Favorites>(criteria);

            if (temp.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public Favorites LoadLastFavorites(string userCode, string type)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Favorites>();
            criteria.Add(Expression.Eq("User.Code", userCode));
            criteria.Add(Expression.Eq("Type", type));
            criteria.AddOrder(Order.Desc("Id"));
            IList<Favorites> favoritesList = criteriaMgrE.FindAll<Favorites>(criteria, 0, 1);
            if (favoritesList != null && favoritesList.Count == 1)
            {
                return favoritesList[0];
            }
            return null;
        }


        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class FavoritesMgrE : com.Sconit.Service.MasterData.Impl.FavoritesMgr, IFavoritesMgrE
    {

    }
}
#endregion
