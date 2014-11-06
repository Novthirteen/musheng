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
    public class NHRawIOBBaseDao : NHDaoBase, IRawIOBBaseDao
    {
        public NHRawIOBBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateRawIOB(RawIOB entity)
        {
            Create(entity);
        }

        public virtual IList<RawIOB> GetAllRawIOB()
        {
            return FindAll<RawIOB>();
        }

        public virtual RawIOB LoadRawIOB(Int32 id)
        {
            return FindById<RawIOB>(id);
        }

        public virtual void UpdateRawIOB(RawIOB entity)
        {
            Update(entity);
        }

        public virtual void DeleteRawIOB(Int32 id)
        {
            string hql = @"from RawIOB entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteRawIOB(RawIOB entity)
        {
            Delete(entity);
        }

        public virtual void DeleteRawIOB(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from RawIOB entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteRawIOB(IList<RawIOB> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (RawIOB entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteRawIOB(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
