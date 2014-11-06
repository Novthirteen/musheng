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
    public class NHCbomBaseDao : NHDaoBase, ICbomBaseDao
    {
        public NHCbomBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateCbom(Cbom entity)
        {
            Create(entity);
        }

        public virtual IList<Cbom> GetAllCbom()
        {
            return FindAll<Cbom>();
        }

        public virtual Cbom LoadCbom(Int32 id)
        {
            return FindById<Cbom>(id);
        }

        public virtual void UpdateCbom(Cbom entity)
        {
            Update(entity);
        }

        public virtual void DeleteCbom(Int32 id)
        {
            string hql = @"from Cbom entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteCbom(Cbom entity)
        {
            Delete(entity);
        }

        public virtual void DeleteCbom(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from Cbom entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteCbom(IList<Cbom> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (Cbom entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteCbom(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
