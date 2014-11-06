using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IMenuIndustryBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateMenuIndustry(MenuIndustry entity);

        MenuIndustry LoadMenuIndustry(Int32 id);

        IList<MenuIndustry> GetAllMenuIndustry();
    
        IList<MenuIndustry> GetAllMenuIndustry(bool includeInactive);
      
        void UpdateMenuIndustry(MenuIndustry entity);

        void DeleteMenuIndustry(Int32 id);
    
        void DeleteMenuIndustry(MenuIndustry entity);
    
        void DeleteMenuIndustry(IList<Int32> pkList);
    
        void DeleteMenuIndustry(IList<MenuIndustry> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
