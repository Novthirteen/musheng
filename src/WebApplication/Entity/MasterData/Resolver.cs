using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using com.Sconit.Entity.Exception;

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class Resolver : EntityBase
    {
        public string ModuleType { get; set; }
        public string UserCode { get; set; }
        public string CodePrefix { get; set; }
        public string BarcodeHead { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string PickBy { get; set; }
        /// <summary>
        /// 传递输入值(HuId,ItemCode)
        /// </summary>
        public string Input { get; set; }
        public string BinCode { get; set; }
        //是否强制打箱
        public bool IsOddCreateHu { get; set; }
        public bool IsScanHu { get; set; }
        public bool NeedPrintAsn { get; set; }
        public bool NeedPrintReceipt { get; set; }
        public bool NeedInspection { get; set; }
        public bool AutoPrintHu { get; set; }
        public bool AllowExceed { get; set; }
        public bool AllowCreateDetail { get; set; }
        public bool IsPickFromBin { get; set; }
        public string IOType { get; set; }
        public string Command { get; set; }
        public string Result { get; set; }
        public string OrderType { get; set; }
        public string PrintUrl { get; set; }
        public string ExternalOrderNo { get; set; }
        //用于移库和退库
        public string LocationCode { get; set; }
        public string LocationFormCode { get; set; }
        public string LocationToCode { get; set; }

        public List<Transformer> Transformers { get; set; }
        public List<ReceiptNote> ReceiptNotes { get; set; }
        public List<string[]> WorkingHours { get; set; }
        [XmlIgnore]
        public bool IsCSClient { get; set; }
        [XmlIgnore]//用于不整包收发(FulfillUnitCount)就不匹配单包装
        public bool FulfillUnitCount { get; set; }
        [XmlIgnore]//是否可以订单发货
        public bool IsShipByOrder { get; set; }
        public string FlowCode { get; set; }
        public string FlowFacility { get; set; }
        public string OrderNo { get; set; }

        /// <summary>
        /// 辅助字段
        /// </summary>
        public List<string> s { get; set; }

        public void AddTransformer(Transformer transformer)
        {
            if (transformer != null)
            {
                if (this.Transformers == null)
                {
                    this.Transformers = new List<Transformer>();
                }
                this.Transformers.Add(transformer);
            }
        }

        /// <summary>
        /// 用于创建订单类型的 增加明细.如:退货,移库,上下架,投料,盘点等
        /// 检查重复扫描
        /// 自动生成序号
        /// 匹配
        /// 自动新增明细未加控制
        /// </summary>
        /// <param name="transformerDetail"></param>
        public void AddTransformerDetail(TransformerDetail transformerDetail)
        {
            if (transformerDetail != null)
            {
                if (this.Transformers == null)
                {
                    this.Transformers = new List<Transformer>();
                }
                //检查重复扫描
                var oldtd =
                    from transformer in this.Transformers
                    from oldtransformerDetail in transformer.TransformerDetails
                    where oldtransformerDetail.HuId.ToLower() == transformerDetail.HuId.ToLower()
                    select oldtransformerDetail;
                if (oldtd.Count() == 1 && oldtd.Single().CurrentQty != 0M)
                {
                    throw new BusinessErrorException("Warehouse.Error.HuReScan", transformerDetail.HuId);
                }
                //自动生成序号
                var seq = from t in this.Transformers
                          from td in t.TransformerDetails
                          select td.Sequence;
                transformerDetail.Sequence = seq.Count() > 0 ? seq.Max() + 1 : 0;
                //匹配
                var query = this.Transformers.Where
                    (t => (t.ItemCode == transformerDetail.ItemCode
                           && t.UnitCount == transformerDetail.UnitCount
                           && t.UomCode == transformerDetail.UomCode
                           && t.StorageBinCode == transformerDetail.StorageBinCode
                           && t.LocationCode == transformerDetail.LocationCode
                           && t.LocationFromCode == transformerDetail.LocationFromCode
                           && t.LocationToCode == transformerDetail.LocationToCode));
                if (query.Count() == 1)
                {
                    Transformer transformer = query.Single();
                    if (oldtd.Count() == 1 && oldtd.Single().CurrentQty == 0M)
                    {
                        oldtd.Single().CurrentQty = transformerDetail.CurrentQty;
                        oldtd.Single().Sequence = transformerDetail.Sequence;
                    }
                    else
                    {
                        transformer.AddTransformerDetail(transformerDetail);
                    }
                    transformer.CurrentQty += transformerDetail.CurrentQty;
                    transformer.Qty += transformerDetail.Qty;
                    transformer.Cartons++;
                }
                else if (query.Count() == 0)
                {
                    Transformer transformer = new Transformer();
                    transformer.ItemCode = transformerDetail.ItemCode;
                    transformer.ItemDescription = transformerDetail.ItemDescription;
                    transformer.UomCode = transformerDetail.UomCode;
                    transformer.UnitCount = transformerDetail.UnitCount;
                    transformer.CurrentQty = transformerDetail.CurrentQty;
                    transformer.Qty = transformerDetail.Qty;
                    transformer.LocationCode = transformerDetail.LocationCode;
                    transformer.LocationFromCode = transformerDetail.LocationFromCode;
                    transformer.LocationToCode = transformerDetail.LocationToCode;
                    transformer.LotNo = transformerDetail.LotNo;
                    transformer.StorageBinCode = transformerDetail.StorageBinCode;
                    transformer.Cartons = 1;
                    transformer.Sequence = this.Transformers.Count > 0 ? this.Transformers.Max(t => t.Sequence) + 1 : 0;

                    transformer.AddTransformerDetail(transformerDetail);
                    this.Transformers.Add(transformer);
                }
                else
                {
                    throw new TechnicalException("com.Sconit.Entity.MasterData.Resolver:Line 147");
                }
            }
            this.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMERDETAIL;
        }

        private List<TransformerDetail> MatchHu(List<Transformer> transformers, TransformerDetail transformerDetail)
        {
            List<TransformerDetail> transformerDetailList = new List<TransformerDetail>();
            if (transformers != null && transformers.Count > 0)
            {
                foreach (Transformer transformer in transformers)
                {
                    if (transformer.TransformerDetails != null && transformer.TransformerDetails.Count > 0)
                    {
                        foreach (TransformerDetail td in transformer.TransformerDetails)
                        {
                            if (td.HuId != null
                                && td.HuId.Equals(transformerDetail.HuId, StringComparison.OrdinalIgnoreCase))
                            {
                                transformerDetailList.Add(td);
                            }
                        }
                    }
                }
            }
            return transformerDetailList;
        }

    }
}
