using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IMenuCompanyBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateMenuCompany(MenuCompany entity);

        MenuCompany LoadMenuCompany(Int32 id);

        IList<MenuCompany> GetAllMenuCompany();
    
        IList<MenuCompany> GetAllMenuCompany(bool includeInactive);
      
        void UpdateMenuCompany(MenuCompany entity);

        void DeleteMenuCompany(Int32 id);
    
        void DeleteMenuCompany(MenuCompany entity);
    
        void DeleteMenuCompany(IList<Int32> pkList);
    
        void DeleteMenuCompany(IList<MenuCompany> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
