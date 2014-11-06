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
    public class NHBomTreeBaseDao : NHDaoBase, IBomTreeBaseDao
    {
        public NHBomTreeBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateBomTree(BomTree entity)
        {
            Create(entity);
        }

        public virtual IList<BomTree> GetAllBomTree()
        {
            return FindAll<BomTree>();
        }

        public virtual BomTree LoadBomTree()
        {
            string hql = @"from BomTree entity where ";
            IList<BomTree> result = FindAllWithCustomQuery<BomTree>(hql, new object[] {  }, new IType[] {  });
            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return null;
            }
        }

        public virtual void UpdateBomTree(BomTree entity)
        {
            Update(entity);
        }

        public virtual void DeleteBomTree()
        {
            string hql = @"from BomTree entity where ";
            Delete(hql, new object[] {  }, new IType[] {  });
        }

        public virtual void DeleteBomTree(BomTree entity)
        {
            Delete(entity);
        }

        public virtual void DeleteBomTree(IList<BomTree> entityList)
        {
            foreach (BomTree entity in entityList)
            {
                DeleteBomTree(entity);
            }
        }


        #endregion Method Created By CodeSmith
    }
}
