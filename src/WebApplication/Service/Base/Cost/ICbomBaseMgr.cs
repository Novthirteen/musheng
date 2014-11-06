using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICbomBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateCbom(Cbom entity);

        Cbom LoadCbom(Int32 id);

        IList<Cbom> GetAllCbom();
    
        void UpdateCbom(Cbom entity);

        void DeleteCbom(Int32 id);
    
        void DeleteCbom(Cbom entity);
    
        void DeleteCbom(IList<Int32> pkList);
    
        void DeleteCbom(IList<Cbom> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
