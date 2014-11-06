using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.Cost;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.Cost.NH
{
    public class NHExpenseElementBaseDao : NHDaoBase, IExpenseElementBaseDao
    {
        public NHExpenseElementBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateExpenseElement(ExpenseElement entity)
        {
            Create(entity);
        }

        public virtual IList<ExpenseElement> GetAllExpenseElement()
        {
            return FindAll<ExpenseElement>();
        }

        public virtual ExpenseElement LoadExpenseElement(String code)
        {
            return FindById<ExpenseElement>(code);
        }

        public virtual void UpdateExpenseElement(ExpenseElement entity)
        {
            Update(entity);
        }

        public virtual void DeleteExpenseElement(String code)
        {
            string hql = @"from ExpenseElement entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteExpenseElement(ExpenseElement entity)
        {
            Delete(entity);
        }

        public virtual void DeleteExpenseElement(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from ExpenseElement entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteExpenseElement(IList<ExpenseElement> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (ExpenseElement entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteExpenseElement(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
