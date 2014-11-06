using System;
using System.Collections.Generic;
using com.Sconit.Entity.Exception;
using System.Collections;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class OrderHead : OrderHeadBase
    {
        #region Non O/R Mapping Properties

        /// <remarks/>
        public void AddOrderDetail(OrderDetail orderDetail)
        {
            if (this.OrderDetails == null)
            {
                this.OrderDetails = new List<OrderDetail>();
                this.OrderDetails.Add(orderDetail);
            }
            else
            {
                //����Sequence�����뵽����λ��
                int position = this.OrderDetails.Count;
                for (int i = 0; i < this.OrderDetails.Count; i++)
                {
                    OrderDetail od = this.OrderDetails[i];
                    if (od.Sequence > orderDetail.Sequence)
                    {
                        position = i;
                        break;
                    }

                    //���Sequence�Ƿ��ظ�
                    //if (od.Sequence == orderDetail.Sequence)
                    //{
                    //    throw new BusinessErrorException("OrderDetail.Error.SequenceOverlap", orderDetail.Sequence.ToString());
                    //}
                }

                this.OrderDetails.Insert(position, orderDetail);
            }
        }

        public OrderDetail GetOrderDetailById(int id)
        {
            if (this.OrderDetails != null)
            {
                int rowIndex = -1;
                for (int i = 0; i < this.OrderDetails.Count; i++)
                {
                    if (this.OrderDetails[i].Id == id)
                    {
                        rowIndex = i;
                        break;
                    }
                }

                if (rowIndex > -1)
                {
                    return this.OrderDetails[rowIndex];
                }
            }

            return null;
        }

        public OrderDetail GetOrderDetailByRowIndex(int rowIndex)
        {
            if (this.OrderDetails != null && rowIndex < this.OrderDetails.Count)
            {
                return this.OrderDetails[rowIndex];
            }

            return null;
        }

        public OrderDetail GetOrderDetailBySequence(int sequence)
        {
            if (this.OrderDetails != null)
            {
                int rowIndex = -1;
                for (int i = 0; i < this.OrderDetails.Count; i++)
                {
                    if (this.OrderDetails[i].Sequence == sequence)
                    {
                        rowIndex = i;
                        break;
                    }
                }

                if (rowIndex > -1)
                {
                    return this.OrderDetails[rowIndex];
                }
            }

            return null;
        }

        public OrderDetail GetOrderDetailByFlowDetailIdAndItemCode(int flowDetailId, string itemCode)
        {
            bool isKit = false;
            if (flowDetailId == 0)
            {
                isKit = true;
            }

            return GetOrderDetailByFlowDetailIdAndItemCode(flowDetailId, itemCode, isKit);
        }

        public OrderDetail GetOrderDetailByFlowDetailIdAndItemCode(int flowDetailId, string itemCode, bool isKit)
        {
            if (this.OrderDetails != null && this.OrderDetails.Count > 0)
            {
                foreach (OrderDetail orderDetail in this.OrderDetails)
                {
                    if (isKit)
                    {
                        if (orderDetail.Item.Code.Trim().ToUpper() == itemCode.Trim().ToUpper())
                        {
                            return orderDetail;
                        }
                    }
                    else
                    {
                        if (orderDetail.FlowDetail != null && orderDetail.FlowDetail.Id == flowDetailId
                           && orderDetail.FlowDetail.Item.Code.Trim().ToUpper() == itemCode.Trim().ToUpper())
                        {
                            return orderDetail;
                        }
                    }
                }
            }

            return null;
        }


        public void RemoveOrderDetailById(int id)
        {
            if (this.OrderDetails != null)
            {
                int rowIndex = -1;
                for (int i = 0; i < this.OrderDetails.Count; i++)
                {
                    if (this.OrderDetails[i].Id == id)
                    {
                        rowIndex = i;
                        break;
                    }
                }

                if (rowIndex > -1)
                {
                    this.OrderDetails.RemoveAt(rowIndex);
                }
            }
        }

        public void RemoveOrderDetailByRowIndex(int rowIndex)
        {
            if (this.OrderDetails != null)
            {
                this.OrderDetails.RemoveAt(rowIndex);
            }
        }

        public void RemoveOrderDetailBySequence(int sequence)
        {
            if (this.OrderDetails != null)
            {
                int rowIndex = -1;
                for (int i = 0; i < this.OrderDetails.Count; i++)
                {
                    if (this.OrderDetails[i].Sequence == sequence)
                    {
                        rowIndex = i;
                        break;
                    }
                }

                if (rowIndex > -1)
                {
                    this.OrderDetails.RemoveAt(rowIndex);
                }
            }
        }

        public void AddOrderOperation(OrderOperation orderOperation)
        {
            if (this.OrderOperations == null)
            {
                this.OrderOperations = new List<OrderOperation>();
                this.OrderOperations.Add(orderOperation);
            }
            else
            {
                //����Op�����뵽����λ��
                int position = this.OrderOperations.Count;
                for (int i = 0; i < this.OrderOperations.Count; i++)
                {
                    OrderOperation oo = this.OrderOperations[i];
                    if (oo.Operation > orderOperation.Operation)
                    {
                        position = i;
                        break;
                    }

                    //�ж��Ƿ��Ѿ�����ͬ����ŵ�orderOperation�����û�в����
                    if (oo.Operation == orderOperation.Operation)
                    {
                        return;
                    }
                }

                this.OrderOperations.Insert(position, orderOperation);
            }
        }

        public OrderOperation GetOrderOperationById(int id)
        {
            if (this.OrderOperations != null)
            {
                int rowIndex = -1;
                for (int i = 0; i < this.OrderOperations.Count; i++)
                {
                    if (this.OrderOperations[i].Id == id)
                    {
                        rowIndex = i;
                        break;
                    }
                }

                if (rowIndex > -1)
                {
                    return this.OrderOperations[rowIndex];
                }
            }

            return null;
        }

        public OrderOperation GetOrderOperationByRowIndex(int rowIndex)
        {
            if (this.OrderOperations != null && rowIndex < this.OrderOperations.Count)
            {
                return this.OrderOperations[rowIndex];
            }

            return null;
        }

        public OrderOperation GetOrderOperationByOperation(int operation)
        {
            if (this.OrderOperations != null)
            {
                int rowIndex = -1;
                for (int i = 0; i < this.OrderOperations.Count; i++)
                {
                    if (this.OrderOperations[i].Operation == operation)
                    {
                        rowIndex = i;
                        break;
                    }
                }

                if (rowIndex > -1)
                {
                    return this.OrderOperations[rowIndex];
                }
            }

            return null;
        }

        public void RemoveOrderOperationById(int id)
        {
            if (this.OrderOperations != null)
            {
                int rowIndex = -1;
                for (int i = 0; i < this.OrderOperations.Count; i++)
                {
                    if (this.OrderOperations[i].Id == id)
                    {
                        rowIndex = i;
                        break;
                    }
                }

                if (rowIndex > -1)
                {
                    this.OrderOperations.RemoveAt(rowIndex);
                }
            }
        }

        public void RemoveOrderOperationByRowIndex(int rowIndex)
        {
            if (this.OrderOperations != null)
            {
                this.OrderOperations.RemoveAt(rowIndex);
            }
        }

        public void RemoveOrderOperationByOperation(int operation)
        {
            if (this.OrderOperations != null)
            {
                int rowIndex = -1;
                for (int i = 0; i < this.OrderOperations.Count; i++)
                {
                    if (this.OrderOperations[i].Operation == operation)
                    {
                        rowIndex = i;
                        break;
                    }
                }

                if (rowIndex > -1)
                {
                    this.OrderOperations.RemoveAt(rowIndex);
                }
            }
        }

        public decimal OrderAmountAfterDiscount
        {
            get
            {
                return this.OrderDetailAmountAfterDiscount - (this.Discount.HasValue ? this.Discount.Value : 0);
            }
        }

        public decimal OrderDiscountRate
        {
            get
            {
                if (this.OrderAmountAfterDiscount != 0)
                {
                    return (this.Discount == null ? 0 : (decimal)this.Discount) / this.OrderDetailAmountAfterDiscount * 100;
                }
                else
                {
                    return 0;
                }
            }
        }

        public decimal OrderDetailAmountAfterDiscount
        {
            get
            {
                decimal orderDetailAmount = 0;
                if (this.OrderDetails != null)
                {
                    foreach (OrderDetail orderDetail in OrderDetails)
                    {
                        orderDetailAmount += orderDetail.OrderDetailAmountAfterDiscount;
                    }
                }
                return orderDetailAmount;
            }
        }

        //��portal�ӵ�
        public string WinDate
        {
            get
            {
                if (this.WindowTime != null)
                {
                    return this.WindowTime.ToString("yyyy-MM-dd");
                }
                else
                {
                    return null;
                }
            }
        }

        public string WinTime
        {
            get
            {
                if (this.WindowTime != null)
                {
                    return this.WindowTime.ToString("hh:mm:ss");
                }
                else
                {
                    return null;
                }
            }
        }

        public string DeliverySite
        {
            get
            {
                string shipToAddress = this.ShipTo.Address;
                if (this.DockDescription != string.Empty)
                {
                    shipToAddress = shipToAddress + "@" + this.DockDescription;
                }
                return shipToAddress;
            }
        }

        public bool IsConfirmed
        {
            get
            {
                bool isConfirmed = false;
                if (this.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
                {
                    isConfirmed = true;
                }
                return isConfirmed;
            }
        }

        public DateTime? NextWindowTime { get; set; }
        //private string _subType;
        //public string SubType
        //{
        //    get
        //    {
        //        return this._subType;
        //    }
        //    set
        //    {
        //        this._subType = value;
        //    }
        //}
        public bool IsMark { get; set; }

        #endregion
    }
}