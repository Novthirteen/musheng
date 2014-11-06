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
    public class NHIndustryBaseDao : NHDaoBase, IIndustryBaseDao
    {
        public NHIndustryBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateIndustry(Industry entity)
        {
            Create(entity);
        }

        public virtual IList<Industry> GetAllIndustry()
        {
            return GetAllIndustry(false);
        }

        public virtual IList<Industry> GetAllIndustry(bool includeInactive)
        {
            string hql = @"from Industry entity";
            if (!includeInactive)
            {
                hql += " where entity.IsActive = 1";
            }
            IList<Industry> result = FindAllWithCustomQuery<Industry>(hql);
            return result;
        }

        public virtual Industry LoadIndustry(String code)
        {
            return FindById<Industry>(code);
        }

        public virtual void UpdateIndustry(Industry entity)
        {
            Update(entity);
        }

        public virtual void DeleteIndustry(String code)
        {
            string hql = @"from Industry entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteIndustry(Industry entity)
        {
            Delete(entity);
        }

        public virtual void DeleteIndustry(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from Industry entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteIndustry(IList<Industry> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (Industry entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteIndustry(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
