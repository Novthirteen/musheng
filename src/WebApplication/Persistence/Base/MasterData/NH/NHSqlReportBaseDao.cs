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
    public class NHSqlReportBaseDao : NHDaoBase, ISqlReportBaseDao
    {
        public NHSqlReportBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateSqlReport(SqlReport entity)
        {
            Create(entity);
        }

        public virtual IList<SqlReport> GetAllSqlReport()
        {
            return FindAll<SqlReport>();
        }

        public virtual SqlReport LoadSqlReport(Int32 id)
        {
            return FindById<SqlReport>(id);
        }

        public virtual void UpdateSqlReport(SqlReport entity)
        {
            Update(entity);
        }

        public virtual void DeleteSqlReport(Int32 id)
        {
            string hql = @"from SqlReport entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteSqlReport(SqlReport entity)
        {
            Delete(entity);
        }

        public virtual void DeleteSqlReport(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from SqlReport entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteSqlReport(IList<SqlReport> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (SqlReport entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteSqlReport(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
