using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.MasterData.NH
{
    public class NHCustomerGoodsPriceListBaseDao : NHDaoBase, ICustomerGoodsPriceListBaseDao
    {
        public NHCustomerGoodsPriceListBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCustomerGoodsPriceList(CustomerGoodsPriceList entity)
        {
            Create(entity);
        }

        public virtual IList<CustomerGoodsPriceList> GetAllCustomerGoodsPriceList()
        {
            return GetAllCustomerGoodsPriceList(false);
        }

        public virtual IList<CustomerGoodsPriceList> GetAllCustomerGoodsPriceList(bool includeInactive)
        {
            string hql = @"from CustomerGoodsPriceList entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<CustomerGoodsPriceList> result = FindAllWithCustomQuery<CustomerGoodsPriceList>(hql);
            return result;
        }

        public virtual CustomerGoodsPriceList LoadCustomerGoodsPriceList(String code)
        {
            return FindById<CustomerGoodsPriceList>(code);
        }

        public virtual void UpdateCustomerGoodsPriceList(CustomerGoodsPriceList entity)
        {
            Update(entity);
        }

        public virtual void DeleteCustomerGoodsPriceList(String code)
        {
            string hql = @"from CustomerGoodsPriceList entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteCustomerGoodsPriceList(CustomerGoodsPriceList entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCustomerGoodsPriceList(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from CustomerGoodsPriceList entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCustomerGoodsPriceList(IList<CustomerGoodsPriceList> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (CustomerGoodsPriceList entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteCustomerGoodsPriceList(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
