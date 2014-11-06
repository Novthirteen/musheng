using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ShiftDetailMgr : ShiftDetailBaseMgr, IShiftDetailMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
       

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<ShiftDetail> GetShiftDetail(string shiftCode, DateTime date)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ShiftDetail));
            criteria.Add(Expression.Eq("Shift.Code", shiftCode));
            criteria.Add(Expression.Or(Expression.Le("StartDate", date), Expression.IsNull("StartDate")));
            criteria.Add(Expression.Or(Expression.Ge("EndDate", date), Expression.IsNull("EndDate")));
            criteria.AddOrder(Order.Desc("StartDate"));

            return criteriaMgrE.FindAll<ShiftDetail>(criteria);
        }

        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ShiftDetailMgrE : com.Sconit.Service.MasterData.Impl.ShiftDetailMgr, IShiftDetailMgrE
    {
       
    }
}
#endregion
