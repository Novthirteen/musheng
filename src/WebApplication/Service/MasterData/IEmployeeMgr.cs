using com.Sconit.Service.Ext.MasterData;
using System;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IEmployeeMgr : IEmployeeBaseMgr
    {
        #region Customized Methods

        Employee LoadEmployee(string code, bool IncludeInactive);

        IList<Employee> GetAllEmployee(DateTime lastModifyDate, int firstRow, int maxRows);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IEmployeeMgrE : com.Sconit.Service.MasterData.IEmployeeMgr
    {
        
    }
}

#endregion
