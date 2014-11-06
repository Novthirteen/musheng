using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Utility;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class MiscOrderMgr : MiscOrderBaseMgr, IMiscOrderMgr
    {
        public IMiscOrderDao miscOrderDao { get; set; }
        public IMiscOrderDetailMgrE miscOrderDetailMgrE { get; set; }
        public INumberControlMgrE numberControlMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        

        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public MiscOrder SaveMiscOrder(MiscOrder miscOrder, User user)
        {
            MiscOrder dbMiscOrder = null;
            if (miscOrder != null && miscOrder.OrderNo != null)
            {
                dbMiscOrder = miscOrderDao.LoadMiscOrder(miscOrder.OrderNo);
            }
                
            if (dbMiscOrder != null)
            {
                IList<MiscOrderDetail> dbMiscOrderDets = dbMiscOrder.MiscOrderDetails;
                foreach (MiscOrderDetail miscOrderDet in dbMiscOrderDets)
                {
                    miscOrderDetailMgrE.DeleteMiscOrderDetail(miscOrderDet);
                }

                MiscOrder pageMiscOrder = miscOrder;
                IList<MiscOrderDetail> pageMiscOrderDets = pageMiscOrder.MiscOrderDetails;
                foreach (MiscOrderDetail miscOrderDet in pageMiscOrderDets)
                {
                    miscOrderDet.MiscOrder = pageMiscOrder;
                    miscOrderDetailMgrE.CreateMiscOrderDetail(miscOrderDet);
                }
                dbMiscOrder.Remark = pageMiscOrder.Remark;
                dbMiscOrder.Reason = pageMiscOrder.Reason;
                dbMiscOrder.EffectiveDate = pageMiscOrder.EffectiveDate;
                dbMiscOrder.Location = pageMiscOrder.Location;
                dbMiscOrder.CreateDate = DateTime.Now;
                miscOrderDao.UpdateMiscOrder(dbMiscOrder);
            }
            else
            {
                dbMiscOrder = miscOrder;
                dbMiscOrder.CreateUser = user;
                dbMiscOrder.CreateDate = DateTime.Now;
                dbMiscOrder.OrderNo = numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_MISCO);
                miscOrderDao.CreateMiscOrder(dbMiscOrder);

                IList<MiscOrderDetail> pageMiscOrderDets = dbMiscOrder.MiscOrderDetails;
                foreach (MiscOrderDetail miscOrderDet in pageMiscOrderDets)
                {
                    miscOrderDet.MiscOrder = dbMiscOrder;
                    miscOrderDetailMgrE.CreateMiscOrderDetail(miscOrderDet);
                    
                    //出入库
                    if (dbMiscOrder.Type == BusinessConstants.CODE_MASTER_MISC_ORDER_TYPE_VALUE_GI)
                    {
                        //出库
                        this.locationMgrE.InventoryOut(miscOrderDet, user);
                    }
                    else if (dbMiscOrder.Type == BusinessConstants.CODE_MASTER_MISC_ORDER_TYPE_VALUE_GR)
                    {
                        //入库
                        this.locationMgrE.InventoryIn(miscOrderDet, user);
                    }
                    else if (dbMiscOrder.Type == BusinessConstants.CODE_MASTER_MISC_ORDER_TYPE_VALUE_ADJ)
                    {
                        //用入库操作就可以做调整
                        this.locationMgrE.InventoryIn(miscOrderDet, user);
                        //if (miscOrderDet.Qty > 0)
                        //{
                        //    //入库
                        //    this.locationMgrE.InventoryIn(miscOrderDet, user);
                        //}
                        //else if (miscOrderDet.Qty < 0)
                        //{
                        //    //出库
                        //    miscOrderDet.Qty = miscOrderDet.Qty * -1;  //调整为正数
                        //    this.locationMgrE.InventoryOut(miscOrderDet, user);
                        //}
                    }
                    else
                    {
                        throw new TechnicalException("MiscOrder Type:" + dbMiscOrder.Type + " is not valided");
                    }
                }
                
            }
            return ReLoadMiscOrder(dbMiscOrder.OrderNo);

        }
        [Transaction(TransactionMode.Requires)]
        public void RemoveMiscOrder(MiscOrder miscOrder)
        {
            MiscOrder dbMiscOrder = miscOrderDao.LoadMiscOrder(miscOrder.OrderNo);
            if (dbMiscOrder != null)
            {
                IList<MiscOrderDetail> dbMiscOrderDets = dbMiscOrder.MiscOrderDetails;
                foreach (MiscOrderDetail miscOrderDet in dbMiscOrderDets)
                {
                    miscOrderDetailMgrE.DeleteMiscOrderDetail(miscOrderDet);
                }
                miscOrderDao.DeleteMiscOrder(dbMiscOrder);
            }
        }
        [Transaction(TransactionMode.Unspecified)]
        public MiscOrder ReLoadMiscOrder(string orderNo)
        {
            MiscOrder reload = miscOrderDao.LoadMiscOrder(orderNo);
            reload.MiscOrderDetails = this.miscOrderDetailMgrE.GetMiscOrderDetail(orderNo);
            return reload;
        }


        #endregion Customized Methods
    }
}


#region Extend Class



namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class MiscOrderMgrE : com.Sconit.Service.MasterData.Impl.MiscOrderMgr, IMiscOrderMgrE
    {
        
    }
}
#endregion
