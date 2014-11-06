using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Utility;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class RepackMgr : RepackBaseMgr, IRepackMgr
    {
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IStorageBinMgrE storageBinMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public INumberControlMgrE numberControlMgrE { get; set; }
        public IRepackDetailMgrE repackDetailMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public Repack LoadRepack(String repackNo, bool includeDetail)
        {
            Repack repack = this.LoadRepack(repackNo);
            if (includeDetail && repack.RepackDetails != null && repack.RepackDetails.Count > 0)
            {

            }
            return repack;
        }

        [Transaction(TransactionMode.Requires)]
        public Repack CreateRepack(IList<RepackDetail> repackDetailList, User user)
        {
            return CreateRepack(repackDetailList, BusinessConstants.CODE_MASTER_REPACK_TYPE_VALUE_REPACK, user);
        }

        [Transaction(TransactionMode.Requires)]
        public Repack CreateDevanning(IList<RepackDetail> repackDetailList, User user)
        {
            return CreateRepack(repackDetailList, BusinessConstants.CODE_MASTER_REPACK_TYPE_VALUE_DEVANNING, user);
        }
        #endregion Customized Methods

        #region Private Methods
        private Repack CreateRepack(IList<RepackDetail> repackDetailList, string type, User user)
        {
            IList<RepackDetail> inRepackDetailList = new List<RepackDetail>();
            IList<RepackDetail> outRepackDetailList = new List<RepackDetail>();

            #region 判断RepackDetailList是否为空
            if (repackDetailList != null && repackDetailList.Count > 0)
            {
                foreach (RepackDetail repackDetail in repackDetailList)
                {
                    if (repackDetail.Qty != 0)
                    {
                        if (repackDetail.IOType == BusinessConstants.IO_TYPE_IN)
                        {
                            inRepackDetailList.Add(repackDetail);
                        }
                        else if (repackDetail.IOType == BusinessConstants.IO_TYPE_OUT)
                        {
                            outRepackDetailList.Add(repackDetail);
                        }
                        else
                        {
                            throw new TechnicalException("Invalid IO Type:" + repackDetail.IOType);
                        }
                    }
                }

                if (inRepackDetailList.Count == 0 || outRepackDetailList.Count == 0)
                {
                    throw new BusinessErrorException("MasterData.Inventory.Repack.Error.RepackDetailEmpty");
                }
            }
            else
            {
                throw new BusinessErrorException("MasterData.Inventory.Repack.Error.RepackDetailEmpty");
            }
            #endregion

            #region 检查In和Out明细数量是否匹配
            IDictionary<string, decimal> inItemQtyDic = new Dictionary<string, decimal>();
            Location location = null;

            #region 收集In数量
            foreach (RepackDetail inRepackDetail in inRepackDetailList)
            {
                LocationLotDetail inLocationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inRepackDetail.LocationLotDetail.Id);

                if (location == null)
                {
                    location = inLocationLotDetail.Location;

                    if (!user.HasPermission(inLocationLotDetail.Location.Region.Code))
                    {
                        throw new BusinessErrorException("MasterData.Inventory.Repack.Error.NoPermission", location.Code);
                    }
                }
                else if (location.Code != inLocationLotDetail.Location.Code)
                {
                    throw new BusinessErrorException("MasterData.Inventory.Repack.Error.InRepackDetailLocationNotEqual");
                }

                if (inItemQtyDic.ContainsKey(inLocationLotDetail.Item.Code))
                {
                    inItemQtyDic[inLocationLotDetail.Item.Code] += inRepackDetail.Qty;
                }
                else
                {
                    inItemQtyDic.Add(inLocationLotDetail.Item.Code, inRepackDetail.Qty);
                }
            }
            #endregion

            #region 收集Out数量
            IDictionary<string, decimal> outItemQtyDic = new Dictionary<string, decimal>();

            foreach (RepackDetail outRepackDetail in outRepackDetailList)
            {
                if (type == BusinessConstants.CODE_MASTER_REPACK_TYPE_VALUE_REPACK)
                {
                    if (outRepackDetail.Hu == null)
                    {
                        throw new BusinessErrorException("MasterData.Inventory.Repack.Error.HuIdIsEmpty");
                    }
                    else
                    {
                        if (outItemQtyDic.ContainsKey(outRepackDetail.Hu.Item.Code))
                        {
                            outItemQtyDic[outRepackDetail.Hu.Item.Code] += outRepackDetail.Qty;
                        }
                        else
                        {
                            outItemQtyDic.Add(outRepackDetail.Hu.Item.Code, outRepackDetail.Qty);
                        }
                    }
                }
                else if (type == BusinessConstants.CODE_MASTER_REPACK_TYPE_VALUE_DEVANNING)
                {
                    string itemCode = outRepackDetail.Hu != null ? outRepackDetail.Hu.Item.Code : outRepackDetail.itemCode;

                    if (itemCode == null)
                    {
                        throw new TechnicalException("ItemCode not specified.");
                    }

                    if (outItemQtyDic.ContainsKey(itemCode))
                    {
                        outItemQtyDic[itemCode] += outRepackDetail.Qty;
                    }
                    else
                    {
                        outItemQtyDic.Add(itemCode, outRepackDetail.Qty);
                    }
                }
                else
                {
                    throw new TechnicalException("Repack type: " + type + " is not valided.");
                }
            }
            #endregion

            #region 比较
            if (inItemQtyDic.Count != outItemQtyDic.Count)
            {
                throw new BusinessErrorException("MasterData.Inventory.Repack.Error.InOutQtyNotMatch");
            }

            foreach (string itemCode in inItemQtyDic.Keys)
            {
                if (outItemQtyDic.ContainsKey(itemCode))
                {
                    decimal inQty = inItemQtyDic[itemCode];
                    decimal outQty = outItemQtyDic[itemCode];

                    //是否自动创建剩余数量的记录
                    bool autoCreate =  bool.Parse(entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AUTO_CREATE_WHEN_DEAVING).Value);
                    
                    #region 拆箱根据剩余数量得到剩余数量的条码
                    if (autoCreate &&type == BusinessConstants.CODE_MASTER_REPACK_TYPE_VALUE_DEVANNING && inQty > outQty)
                    {
                        RepackDetail remainRepackDetail = new RepackDetail();
                        CloneHelper.CopyProperty(inRepackDetailList[0], remainRepackDetail);
                        //CloneHelper.DeepClone(inRepackDetailList[0]);
                        remainRepackDetail.Qty = inQty - outQty;
                        remainRepackDetail.IOType = BusinessConstants.IO_TYPE_OUT;
                        outRepackDetailList.Add(remainRepackDetail);
                    }
                    #endregion

                    else if (inQty != outQty)
                    {
                        throw new BusinessErrorException("MasterData.Inventory.Repack.Error.InOutQtyNotMatch");
                    }
                }
                else
                {
                    throw new BusinessErrorException("MasterData.Inventory.Repack.Error.InOutItemNotMatch", itemCode);
                }
            }
            #endregion
            #endregion

            #region 创建翻箱单头
            Repack repack = new Repack();
            repack.RepackNo = this.numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_REPACK);
            repack.CreateDate = DateTime.Now;
            repack.CreateUser = user;
            repack.Type = type;

            this.CreateRepack(repack);
            #endregion

            #region 创建翻箱单明细
            Int32? plannedBillId = null;  //拆箱传递PlannedBill
            foreach (RepackDetail inRepackDetail in inRepackDetailList)
            {
                //出库
                inRepackDetail.Repack = repack;
                this.locationMgrE.InventoryRepackIn(inRepackDetail, user);

                this.repackDetailMgrE.CreateRepackDetail(inRepackDetail);

                if (repack.Type == BusinessConstants.CODE_MASTER_REPACK_TYPE_VALUE_DEVANNING)
                {
                    plannedBillId = inRepackDetail.LocationLotDetail.IsConsignment ? inRepackDetail.LocationLotDetail.PlannedBill : null;
                }
            }

            foreach (RepackDetail outRepackDetail in outRepackDetailList)
            {
                //入库
                outRepackDetail.Repack = repack;
                InventoryTransaction inventoryTransaction = this.locationMgrE.InventoryRepackOut(outRepackDetail, location, plannedBillId, user);
                outRepackDetail.LocationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransaction.LocationLotDetailId);

                this.repackDetailMgrE.CreateRepackDetail(outRepackDetail);
            }
            #endregion

            return repack;
        }
        #endregion
    }
}


#region Extend Class



namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class RepackMgrE : com.Sconit.Service.MasterData.Impl.RepackMgr, IRepackMgrE
    {
        
    }
}
#endregion
