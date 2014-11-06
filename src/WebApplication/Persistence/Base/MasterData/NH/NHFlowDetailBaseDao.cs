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
    public class NHFlowDetailBaseDao : NHDaoBase, IFlowDetailBaseDao
    {
        public NHFlowDetailBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateFlowDetail(FlowDetail entity)
        {
            Create(entity);
        }

        public virtual IList<FlowDetail> GetAllFlowDetail()
        {
            return FindAll<FlowDetail>();
        }

        public virtual FlowDetail LoadFlowDetail(Int32 id)
        {
            return FindById<FlowDetail>(id);
        }

        public virtual void UpdateFlowDetail(FlowDetail entity)
        {
            Update(entity);
        }

        public virtual void DeleteFlowDetail(Int32 id)
        {
            string hql = @"from FlowDetail entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteFlowDetail(FlowDetail entity)
        {
            Delete(entity);
        }

        public virtual void DeleteFlowDetail(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from FlowDetail entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteFlowDetail(IList<FlowDetail> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (FlowDetail entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteFlowDetail(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
