using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.MasterData
{
    public interface IMenuBaseDao
    {
        #region Method Created By CodeSmith

        void CreateMenu(Menu entity);

        Menu LoadMenu(String id);
  
        IList<Menu> GetAllMenu();
  
        IList<Menu> GetAllMenu(bool includeInactive);
  
        void UpdateMenu(Menu entity);
        
        void DeleteMenu(String id);
    
        void DeleteMenu(Menu entity);
    
        void DeleteMenu(IList<String> pkList);
    
        void DeleteMenu(IList<Menu> entityList);    
        #endregion Method Created By CodeSmith
    }
}
