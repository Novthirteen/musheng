using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface IDiffBaseDao
    {
        #region Method Created By CodeSmith

        void CreateDiff(Diff entity);

        Diff LoadDiff(Int32 id);
  
        IList<Diff> GetAllDiff();
  
        void UpdateDiff(Diff entity);
        
        void DeleteDiff(Int32 id);
    
        void DeleteDiff(Diff entity);
    
        void DeleteDiff(IList<Int32> pkList);
    
        void DeleteDiff(IList<Diff> entityList);    
        #endregion Method Created By CodeSmith
    }
}
