using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IExpenseElementBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateExpenseElement(ExpenseElement entity);

        ExpenseElement LoadExpenseElement(String code);

        IList<ExpenseElement> GetAllExpenseElement();
    
        void UpdateExpenseElement(ExpenseElement entity);

        void DeleteExpenseElement(String code);
    
        void DeleteExpenseElement(ExpenseElement entity);
    
        void DeleteExpenseElement(IList<String> pkList);
    
        void DeleteExpenseElement(IList<ExpenseElement> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
