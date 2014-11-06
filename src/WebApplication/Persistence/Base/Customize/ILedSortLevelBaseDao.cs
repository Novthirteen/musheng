using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Customize;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Customize
{
    public interface ILedSortLevelBaseDao
    {
        #region Method Created By CodeSmith

        void CreateLedSortLevel(LedSortLevel entity);

        LedSortLevel LoadLedSortLevel(Int32 id);
  
        IList<LedSortLevel> GetAllLedSortLevel();
  
        IList<LedSortLevel> GetAllLedSortLevel(bool includeInactive);
  
        void UpdateLedSortLevel(LedSortLevel entity);
        
        void DeleteLedSortLevel(Int32 id);
    
        void DeleteLedSortLevel(LedSortLevel entity);
    
        void DeleteLedSortLevel(IList<Int32> pkList);
    
        void DeleteLedSortLevel(IList<LedSortLevel> entityList);    
        #endregion Method Created By CodeSmith
    }
}
