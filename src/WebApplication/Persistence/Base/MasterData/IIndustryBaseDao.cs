using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.MasterData
{
    public interface IIndustryBaseDao
    {
        #region Method Created By CodeSmith

        void CreateIndustry(Industry entity);

        Industry LoadIndustry(String code);
  
        IList<Industry> GetAllIndustry();
  
        IList<Industry> GetAllIndustry(bool includeInactive);
  
        void UpdateIndustry(Industry entity);
        
        void DeleteIndustry(String code);
    
        void DeleteIndustry(Industry entity);
    
        void DeleteIndustry(IList<String> pkList);
    
        void DeleteIndustry(IList<Industry> entityList);    
        #endregion Method Created By CodeSmith
    }
}
