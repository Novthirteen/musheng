using System;
using System.Collections.Generic;
using com.Sconit.Entity.Procurement;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Production;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class OrderDetail : OrderDetailBase
    {
        #region Non O/R Mapping Properties

        public void AddOrderLocationTransaction(OrderLocationTransaction orderLocationTransaction)
        {
            if (this.OrderLocationTransactions == null)
            {
                this.OrderLocationTransactions = new List<OrderLocationTransaction>();
            }

            this.OrderLocationTransactions.Add(orderLocationTransaction);
        }

        public void RemoveOrderLocationTransaction(OrderLocationTransaction orderLocationTransaction)
        {
            if (this.OrderLocationTransactions != null)
            {
                this.OrderLocationTransactions.Remove(orderLocationTransaction);
            }
        }

        public void RemoveOrderLocationTransaction(int position)
        {
            if (this.OrderLocationTransactions != null)
            {
                this.OrderLocationTransactions.RemoveAt(position);
            }
        }

        public Location DefaultLocationFrom
        {
            get
            {
                if (this.LocationFrom == null)
                {
                    if (this.OrderHead != null)
                    {
                        return this.OrderHead.LocationFrom;
                    }
                    return null;
                }
                else
                {
                    return this.LocationFrom;
                }
            }
        }

        public Location DefaultLocationTo
        {
            get
            {
                if (this.LocationTo == null)
                {
                    if (this.OrderHead != null)
                    {
                        return this.OrderHead.LocationTo;
                    }
                    return null;
                }
                else
                {
                    return this.LocationTo;
                }
            }
        }

        public String DefaultInspectLocationFrom
        {
            get
            {
                if (this.InspectLocationFrom == null)
                {
                    if (this.OrderHead != null)
                    {
                        return this.OrderHead.InspectLocationFrom;
                    }
                    return null;
                }
                else
                {
                    return this.InspectLocationFrom;
                }
            }
        }

        public String DefaultInspectLocationTo
        {
            get
            {
                if (this.InspectLocationTo == null)
                {
                    if (this.OrderHead != null)
                    {
                        return this.OrderHead.InspectLocationTo;
                    }
                    return null;
                }
                else
                {
                    return this.InspectLocationTo;
                }
            }
        }

        public String DefaultRejectLocationFrom
        {
            get
            {
                if (this.RejectLocationFrom == null)
                {
                    if (this.OrderHead != null)
                    {
                        return this.OrderHead.RejectLocationFrom;
                    }
                    return null;
                }
                else
                {
                    return this.RejectLocationFrom;
                }
            }
        }

        public String DefaultRejectLocationTo
        {
            get
            {
                if (this.RejectLocationTo == null)
                {
                    if (this.OrderHead != null)
                    {
                        return this.OrderHead.RejectLocationTo;
                    }
                    return null;
                }
                else
                {
                    return this.RejectLocationTo;
                }
            }
        }

        public BillAddress DefaultBillAddress
        {
            get
            {
                if (this.BillAddress == null)
                {
                    return this.OrderHead.BillAddress;
                }
                else
                {
                    return this.BillAddress;
                }
            }
        }

        public PriceList DefaultPriceList
        {
            get
            {
                if (this.PriceList == null)
                {
                    return this.OrderHead.PriceList;
                }
                else
                {
                    return this.PriceList;
                }
            }
        }

        public string DefaultBillSettleTerm
        {
            get
            {
                if (this.BillSettleTerm == null || this.BillSettleTerm == string.Empty)
                {
                    return this.OrderHead.BillSettleTerm;
                }
                else
                {
                    return this.BillSettleTerm;
                }
            }
        }

        private decimal _currentShipQty;
        public decimal CurrentShipQty
        {
            get
            {
                return this._currentShipQty;
            }
            set
            {
                this._currentShipQty = value;
            }
        }

        private decimal _currentReceiveQty;
        public decimal CurrentReceiveQty
        {
            get
            {
                return this._currentReceiveQty;
            }
            set
            {
                this._currentReceiveQty = value;
            }
        }

        private decimal _currentRejectQty;
        public decimal CurrentRejectQty
        {
            get
            {
                return this._currentRejectQty;
            }
            set
            {
                this._currentRejectQty = value;
            }
        }

        private decimal _currentScrapQty;
        public decimal CurrentScrapQty
        {
            get
            {
                return this._currentScrapQty;
            }
            set
            {
                this._currentScrapQty = value;
            }
        }

        private FlowDetail _flowDetail;
        public FlowDetail FlowDetail
        {
            get
            {
                return this._flowDetail;
            }
            set
            {
                this._flowDetail = value;
            }
        }

        private IList<AutoOrderTrack> _autoOrderTracks;
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public IList<AutoOrderTrack> AutoOrderTracks
        {
            get
            {
                return _autoOrderTracks;
            }
            set
            {
                _autoOrderTracks = value;
            }
        }

        public void AddAutoOrderTrack(AutoOrderTrack autoOrderTrack)
        {
            if (this.AutoOrderTracks == null)
            {
                this.AutoOrderTracks = new List<AutoOrderTrack>();
            }

            this.AutoOrderTracks.Add(autoOrderTrack);
        }

        public void AddRangeAutoOrderTrack(IList<AutoOrderTrack> autoOrderTrackList)
        {
            foreach (AutoOrderTrack autoOrderTrack in autoOrderTrackList)
            {
                this.AddAutoOrderTrack(autoOrderTrack);
            }
        }

        public decimal RemainQty
        {
            get
            {
                decimal shippedQty = this.ShippedQty == null ? 0 : (decimal)this.ShippedQty;
                return (this.OrderedQty - shippedQty) > 0 ? (this.OrderedQty - shippedQty) : 0;
            }
        }

        //¸¨Öú×Ö¶Î£¬ÅÐ¶Ï¸ÃDetailÊÇ·ñÐÂ¼ÓµÄ¿ÕÐÐ
        private Boolean _isBlankDetail = false;
        public Boolean IsBlankDetail
        {
            get
            {
                return _isBlankDetail;
            }
            set
            {
                _isBlankDetail = value;
            }
        }

        //ÕÛ¿ÛÂÊ
        public decimal DiscountRate
        {
            get
            {

                decimal discount = this.Discount == null ? 0 : (decimal)this.Discount;
                if (this.OrderedQty == 0 || !this.UnitPrice.HasValue || this.UnitPrice.Value == 0)
                {
                    return 0;
                }
                else
                {
                    return discount / (this.OrderedQty * this.UnitPrice.Value) * 100;
                }
            }

        }

        //¸¨Öú×Ö¶Î£¬¼ÆËã¶©µ¥Ã÷Ï¸ÕÛ¿ÛºóµÄ²É¹º¼Û¸ñ£¬²»°üº¬Í·ÕÛ¿Û
        public decimal OrderDetailAmountAfterDiscount
        {
            get
            {
                return this.UnitPrice.HasValue ? this.UnitPrice.Value * this.OrderedQty - (this.Discount.HasValue ? this.Discount.Value : 0) : 0;
            }
        }

        //¸¨Öú×Ö¶Î£¬HuId
        private string _huId;
        public string HuId
        {
            get
            {
                return _huId;
            }
            set
            {
                _huId = value;
            }
        }

        //¸¨Öú×Ö¶Î£¬HuÊýÁ¿
        private decimal _huQty;
        public decimal HuQty
        {
            get
            {
                return _huQty;
            }
            set
            {
                _huQty = value;
            }
        }

        //¸¨Öú×Ö¶Î£¬ÊÇ·ñ±»É¨Ãè
        private Boolean _isScanHu;
        public Boolean IsScanHu
        {
            get
            {
                return _isScanHu;
            }
            set
            {
                _isScanHu = value;
            }
        }

        public decimal RemainShippedQty
        {
            get
            {
                decimal shippedQty = this.ShippedQty.HasValue ? this.ShippedQty.Value : 0;
                decimal remainShippedQty = this.OrderedQty > 0 ?
                    (this.OrderedQty > shippedQty ? this.OrderedQty - shippedQty : 0) : (this.OrderedQty < shippedQty ? this.OrderedQty - shippedQty : 0);
                return remainShippedQty;
            }
        }
        public decimal InTransitQty
        {
            get
            {
                decimal shippedQty = this.ShippedQty.HasValue ? this.ShippedQty.Value : 0;
                decimal receivedQty = this.ReceivedQty.HasValue ? this.ReceivedQty.Value : 0;
                return shippedQty > receivedQty ? shippedQty - receivedQty : 0;
            }
        }
        public decimal RemainReceivedQty
        {
            get
            {
                decimal receivedQty = this.ReceivedQty.HasValue ? this.ReceivedQty.Value : 0;
                decimal remainReceivedQty = this.OrderedQty > 0 ?
                    (this.OrderedQty > receivedQty ? this.OrderedQty - receivedQty : 0) : (this.OrderedQty < receivedQty ? this.OrderedQty - receivedQty : 0);
                return remainReceivedQty;
            }
        }
        public decimal WrapSize
        {
            get
            {
                if (this.HuLotSize.HasValue && this.HuLotSize.Value != 0)
                {
                    return this.HuLotSize.Value;
                }
                else
                {
                    return this.UnitCount;
                }
            }
        }

        public string PutAwayBinCode { get; set; }
        public string HuLotNo { get; set; }

        public List<OrderTracer> OrderTracers { get; set; }        

        public string HuSupplierLotNo { get; set; }

        public string HuSortLevel1 { get; set; }

        public string HuColorLevel1 { get; set; }

        public string HuSortLevel2 { get; set; }

        public string HuColorLevel2 { get; set; }

        public decimal ExpectOrderQty { get; set; }

        public decimal ExpectDisconOrderQty { get; set; }

        public Routing DefaultRouting
        {
            get
            {
                if (this.Routing == null)
                {
                    if (this.OrderHead != null)
                    {
                        return this.OrderHead.Routing;
                    }
                    return null;
                }
                else
                {
                    return this.Routing;
                }
            }
        }

        public Routing DefaultReturnRouting
        {
            get
            {
                if (this.ReturnRouting == null)
                {
                    if (this.OrderHead != null)
                    {
                        return this.OrderHead.Routing;
                    }
                    return null;
                }
                else
                {
                    return this.ReturnRouting;
                }
            }
        }
        #endregion
    }
}