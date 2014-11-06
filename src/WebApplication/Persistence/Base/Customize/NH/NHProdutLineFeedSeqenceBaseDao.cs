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
    public class NHProdutLineFeedSeqenceBaseDao : NHDaoBase, IProdutLineFeedSeqenceBaseDao
    {
        public NHProdutLineFeedSeqenceBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateProdutLineFeedSeqence(ProdutLineFeedSeqence entity)
        {
            Create(entity);
        }

        public virtual IList<ProdutLineFeedSeqence> GetAllProdutLineFeedSeqence()
        {
            return GetAllProdutLineFeedSeqence(false);
        }

        public virtual IList<ProdutLineFeedSeqence> GetAllProdutLineFeedSeqence(bool includeInactive)
        {
            string hql = @"from ProdutLineFeedSeqence entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<ProdutLineFeedSeqence> result = FindAllWithCustomQuery<ProdutLineFeedSeqence>(hql);
            return result;
        }

        public virtual ProdutLineFeedSeqence LoadProdutLineFeedSeqence(Int32 id)
        {
            return FindById<ProdutLineFeedSeqence>(id);
        }

        public virtual void UpdateProdutLineFeedSeqence(ProdutLineFeedSeqence entity)
        {
            Update(entity);
        }

        public virtual void DeleteProdutLineFeedSeqence(Int32 id)
        {
            string hql = @"from ProdutLineFeedSeqence entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteProdutLineFeedSeqence(ProdutLineFeedSeqence entity)
        {
            Delete(entity);
        }

        public virtual void DeleteProdutLineFeedSeqence(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from ProdutLineFeedSeqence entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteProdutLineFeedSeqence(IList<ProdutLineFeedSeqence> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (ProdutLineFeedSeqence entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteProdutLineFeedSeqence(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
