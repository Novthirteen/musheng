using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class EmployeeMgr : EmployeeBaseMgr, IEmployeeMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public Employee LoadEmployee(string code, bool IncludeInactive)
        {
            Employee employee = LoadEmployee(code);
            if (!IncludeInactive)
            {
                if (employee.IsActive)
                {
                    return employee;
                }
                else
                {
                    return null;
                }
            }
            return employee;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Employee> GetAllEmployee(DateTime lastModifyDate, int firstRow, int maxRows)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Employee));
            criteria.Add(Expression.Gt("LastModifyDate", lastModifyDate));
            criteria.AddOrder(Order.Asc("LastModifyDate"));
            IList<Employee> employeeList = criteriaMgrE.FindAll<Employee>(criteria, firstRow, maxRows);
            if (employeeList.Count > 0)
            {
                return employeeList;
            }
            return null;
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class EmployeeMgrE : com.Sconit.Service.MasterData.Impl.EmployeeMgr, IEmployeeMgrE
    {
        
    }
}
#endregion
