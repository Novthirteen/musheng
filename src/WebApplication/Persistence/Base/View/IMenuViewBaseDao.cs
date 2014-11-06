using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.View;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.View
{
    public interface IMenuViewBaseDao
    {
        #region Method Created By CodeSmith

        void CreateMenuView(MenuView entity);

        MenuView LoadMenuView(String Code);
  
        IList<MenuView> GetAllMenuView();
  
        IList<MenuView> GetAllMenuView(bool includeInactive);
  
        void UpdateMenuView(MenuView entity);

        void DeleteMenuView(String Code);
    
        void DeleteMenuView(MenuView entity);
    
        void DeleteMenuView(IList<String> pkList);
    
        void DeleteMenuView(IList<MenuView> entityList);    
        #endregion Method Created By CodeSmith
    }
}
