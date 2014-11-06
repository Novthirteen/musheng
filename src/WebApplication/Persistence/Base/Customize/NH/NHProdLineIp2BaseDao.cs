using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.Customize;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.Customize.NH
{
    public class NHProdLineIp2BaseDao : NHDaoBase, IProdLineIp2BaseDao
    {
        public NHProdLineIp2BaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateProdLineIp2(ProdLineIp2 entity)
        {
            Create(entity);
        }

        public virtual IList<ProdLineIp2> GetAllProdLineIp2()
        {
            return FindAll<ProdLineIp2>();
        }

        public virtual ProdLineIp2 LoadProdLineIp2(Int32 id)
        {
            return FindById<ProdLineIp2>(id);
        }

        public virtual void UpdateProdLineIp2(ProdLineIp2 entity)
        {
            Update(entity);
        }

        public virtual void DeleteProdLineIp2(Int32 id)
        {
            string hql = @"from ProdLineIp2 entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteProdLineIp2(ProdLineIp2 entity)
        {
            Delete(entity);
        }

        public virtual void DeleteProdLineIp2(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from ProdLineIp2 entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteProdLineIp2(IList<ProdLineIp2> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (ProdLineIp2 entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteProdLineIp2(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
