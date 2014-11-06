using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Distribution;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Distribution;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Distribution.Impl
{
    [Transactional]
    public class InProcessLocationTrackMgr : InProcessLocationTrackBaseMgr, IInProcessLocationTrackMgr
    {
        public IRoutingDetailMgrE routingDetailMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods

        //[Transaction(TransactionMode.Requires)]
        //public IList<InProcessLocationTrack> CreateIInProcessLocationTrack(InProcessLocation inProcessLocation, Routing routing)
        //{
        //    IList<RoutingDetail> routingDetailList = this.routingDetailMgrE.GetRoutingDetail(routing);

        //    if (routingDetailList != null && routingDetailList.Count > 0)
        //    {
        //        IList<InProcessLocationTrack> inProcessLocationTrackList = new List<InProcessLocationTrack>();
        //        foreach (RoutingDetail routingDetail in routingDetailList)
        //        {
        //            InProcessLocationTrack inProcessLocationTrack = new InProcessLocationTrack();
        //            inProcessLocationTrack.IpProcessLocation = inProcessLocation;
        //            inProcessLocationTrack.Operation = routingDetail.Operation;
        //            inProcessLocationTrack.Activity = routingDetail.Activity;
        //            inProcessLocationTrack.WorkCenter = routingDetail.WorkCenter;

        //            this.entityDao.CreateInProcessLocationTrack(inProcessLocationTrack);
        //            inProcessLocationTrackList.Add(inProcessLocationTrack);
        //        }

        //        return inProcessLocationTrackList;
        //    }

        //    return null;
        //}

        //[Transaction(TransactionMode.Requires)]
        //public IList<InProcessLocationTrack> GetInProcessLocationTrack(string ipNo,int op)
        //{
        //    DetachedCriteria criteria = DetachedCriteria.For<InProcessLocationTrack>();
        //    criteria.Add(Expression.Eq("IpProcessLocation.IpNo", ipNo));
        //    criteria.Add(Expression.Ge("Operation", op));
        //    return criteriaMgrE.FindAll<InProcessLocationTrack>(criteria);
        //}

        #endregion Customized Methods
    }
}




#region Extend Interface




namespace com.Sconit.Service.Ext.Distribution.Impl
{
    [Transactional]
    public partial class InProcessLocationTrackMgrE : com.Sconit.Service.Distribution.Impl.InProcessLocationTrackMgr, IInProcessLocationTrackMgrE
    {
      
    }
}
#endregion
