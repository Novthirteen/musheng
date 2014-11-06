using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IFavoritesMgr : IFavoritesBaseMgr
    {
        #region Customized Methods

        IList<Favorites> GetFavorites(string userCode, string type);

        bool CheckFavoritesUniqueExist(string userCode, string type, string pageName);

        Favorites LoadLastFavorites(string userCode, string type);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IFavoritesMgrE : com.Sconit.Service.MasterData.IFavoritesMgr
    {
        
    }
}

#endregion
