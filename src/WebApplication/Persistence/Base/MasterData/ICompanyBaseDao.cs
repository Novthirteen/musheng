using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.MasterData
{
    public interface ICompanyBaseDao
    {
        #region Method Created By CodeSmith

        void CreateCompany(Company entity);

        Company LoadCompany(String code);
  
        IList<Company> GetAllCompany();
  
        IList<Company> GetAllCompany(bool includeInactive);
  
        void UpdateCompany(Company entity);
        
        void DeleteCompany(String code);
    
        void DeleteCompany(Company entity);
    
        void DeleteCompany(IList<String> pkList);
    
        void DeleteCompany(IList<Company> entityList);    
        #endregion Method Created By CodeSmith
    }
}
