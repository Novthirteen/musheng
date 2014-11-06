using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IBomTreeBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateBomTree(BomTree entity);

        BomTree LoadBomTree();

        IList<BomTree> GetAllBomTree();
    
        void UpdateBomTree(BomTree entity);

        void DeleteBomTree();
    
        void DeleteBomTree(BomTree entity);
    
        void DeleteBomTree(IList<BomTree> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
