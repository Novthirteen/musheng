using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Customize;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Customize
{
    public interface ILedColorLevelBaseDao
    {
        #region Method Created By CodeSmith

        void CreateLedColorLevel(LedColorLevel entity);

        LedColorLevel LoadLedColorLevel(Int32 id);
  
        IList<LedColorLevel> GetAllLedColorLevel();
  
        IList<LedColorLevel> GetAllLedColorLevel(bool includeInactive);
  
        void UpdateLedColorLevel(LedColorLevel entity);
        
        void DeleteLedColorLevel(Int32 id);
    
        void DeleteLedColorLevel(LedColorLevel entity);
    
        void DeleteLedColorLevel(IList<Int32> pkList);
    
        void DeleteLedColorLevel(IList<LedColorLevel> entityList);    
        #endregion Method Created By CodeSmith
    }
}
