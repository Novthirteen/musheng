using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IBalanceMgr : IBalanceBaseMgr
    {
        #region Customized Methods

        void UpdateRawIOB(RawIOB rawIOB);

        void GenBomTree(FinanceCalendar financeCalendar, string userCode);

        void GenCbom(FinanceCalendar financeCalendar, string userCode);

        void GenBalance(FinanceCalendar financeCalendar, string userCode, bool isGenRm, bool isGenCbom);

        Balance GetBalance(string fc, string item);

        IList<Balance> GetHisInv(FinanceCalendar financeCalendar);

        IList<Balance> GetHisInv(FinanceCalendar financeCalendar, string itemCategory);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface IBalanceMgrE : com.Sconit.Service.Cost.IBalanceMgr
    {
    }
}

#endregion Extend Interface