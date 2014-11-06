using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IMenuCommonBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateMenuCommon(MenuCommon entity);

        MenuCommon LoadMenuCommon(Int32 id);

        IList<MenuCommon> GetAllMenuCommon();
    
        IList<MenuCommon> GetAllMenuCommon(bool includeInactive);
      
        void UpdateMenuCommon(MenuCommon entity);

        void DeleteMenuCommon(Int32 id);
    
        void DeleteMenuCommon(MenuCommon entity);
    
        void DeleteMenuCommon(IList<Int32> pkList);
    
        void DeleteMenuCommon(IList<MenuCommon> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
