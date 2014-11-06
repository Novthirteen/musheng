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
    public class NHLanguageBaseDao : NHDaoBase, ILanguageBaseDao
    {
        public NHLanguageBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateLanguage(Language entity)
        {
            Create(entity);
        }

        public virtual IList<Language> GetAllLanguage()
        {
            return FindAll<Language>();
        }

        public virtual Language LoadLanguage(String code)
        {
            return FindById<Language>(code);
        }

        public virtual void UpdateLanguage(Language entity)
        {
            Update(entity);
        }

        public virtual void DeleteLanguage(String code)
        {
            string hql = @"from Language entity where entity.Code = ?";
            Delete(hql, new object[] { code }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteLanguage(Language entity)
        {
            Delete(entity);
        }

        public virtual void DeleteLanguage(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from Language entity where entity.Code in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteLanguage(IList<Language> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (Language entity in entityList)
            {
                pkList.Add(entity.Code);
            }

            DeleteLanguage(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
