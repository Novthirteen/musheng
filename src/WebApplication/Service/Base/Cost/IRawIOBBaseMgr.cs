using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IRawIOBBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateRawIOB(RawIOB entity);

        RawIOB LoadRawIOB(Int32 id);

        IList<RawIOB> GetAllRawIOB();
    
        void UpdateRawIOB(RawIOB entity);

        void DeleteRawIOB(Int32 id);
    
        void DeleteRawIOB(RawIOB entity);
    
        void DeleteRawIOB(IList<Int32> pkList);
    
        void DeleteRawIOB(IList<RawIOB> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
