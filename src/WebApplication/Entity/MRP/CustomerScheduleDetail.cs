using System;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MRP
{
    [Serializable]
    public class CustomerScheduleDetail : CustomerScheduleDetailBase
    {
        #region Non O/R Mapping Properties

        public int Sequence { get; set; }

        #endregion
    }

    [Serializable]
    public class ScheduleBody
    {
        #region  Properties

        public Int32 Seq { get; set; }
        public string Item { get; set; }
        public string Uom { get; set; }
        public Decimal UnitCount { get; set; }
        public string Location { get; set; }
        public string ItemDescription { get; set; }
        public string ItemReference { get; set; }

        public decimal Qty0 { get; set; }
        public decimal Qty1 { get; set; }
        public decimal Qty2 { get; set; }
        public decimal Qty3 { get; set; }
        public decimal Qty4 { get; set; }
        public decimal Qty5 { get; set; }
        public decimal Qty6 { get; set; }
        public decimal Qty7 { get; set; }
        public decimal Qty8 { get; set; }
        public decimal Qty9 { get; set; }
        public decimal Qty10 { get; set; }
        public decimal Qty11 { get; set; }
        public decimal Qty12 { get; set; }
        public decimal Qty13 { get; set; }
        public decimal Qty14 { get; set; }
        public decimal Qty15 { get; set; }
        public decimal Qty16 { get; set; }
        public decimal Qty17 { get; set; }
        public decimal Qty18 { get; set; }
        public decimal Qty19 { get; set; }
        public decimal Qty20 { get; set; }
        public decimal Qty21 { get; set; }
        public decimal Qty22 { get; set; }
        public decimal Qty23 { get; set; }
        public decimal Qty24 { get; set; }
        public decimal Qty25 { get; set; }
        public decimal Qty26 { get; set; }
        public decimal Qty27 { get; set; }
        public decimal Qty28 { get; set; }
        public decimal Qty29 { get; set; }
        public decimal Qty30 { get; set; }
        public decimal Qty31 { get; set; }
        public decimal Qty32 { get; set; }
        public decimal Qty33 { get; set; }
        public decimal Qty34 { get; set; }
        public decimal Qty35 { get; set; }
        public decimal Qty36 { get; set; }
        public decimal Qty37 { get; set; }
        public decimal Qty38 { get; set; }
        public decimal Qty39 { get; set; }
        public decimal Qty40 { get; set; }

        public decimal ReqQty0 { get; set; }
        public decimal ReqQty1 { get; set; }
        public decimal ReqQty2 { get; set; }
        public decimal ReqQty3 { get; set; }
        public decimal ReqQty4 { get; set; }
        public decimal ReqQty5 { get; set; }
        public decimal ReqQty6 { get; set; }
        public decimal ReqQty7 { get; set; }
        public decimal ReqQty8 { get; set; }
        public decimal ReqQty9 { get; set; }
        public decimal ReqQty10 { get; set; }
        public decimal ReqQty11 { get; set; }
        public decimal ReqQty12 { get; set; }
        public decimal ReqQty13 { get; set; }
        public decimal ReqQty14 { get; set; }
        public decimal ReqQty15 { get; set; }
        public decimal ReqQty16 { get; set; }
        public decimal ReqQty17 { get; set; }
        public decimal ReqQty18 { get; set; }
        public decimal ReqQty19 { get; set; }
        public decimal ReqQty20 { get; set; }
        public decimal ReqQty21 { get; set; }
        public decimal ReqQty22 { get; set; }
        public decimal ReqQty23 { get; set; }
        public decimal ReqQty24 { get; set; }
        public decimal ReqQty25 { get; set; }
        public decimal ReqQty26 { get; set; }
        public decimal ReqQty27 { get; set; }
        public decimal ReqQty28 { get; set; }
        public decimal ReqQty29 { get; set; }
        public decimal ReqQty30 { get; set; }
        public decimal ReqQty31 { get; set; }
        public decimal ReqQty32 { get; set; }
        public decimal ReqQty33 { get; set; }
        public decimal ReqQty34 { get; set; }
        public decimal ReqQty35 { get; set; }
        public decimal ReqQty36 { get; set; }
        public decimal ReqQty37 { get; set; }
        public decimal ReqQty38 { get; set; }
        public decimal ReqQty39 { get; set; }
        public decimal ReqQty40 { get; set; }

        public decimal ActQty0 { get; set; }
        public decimal ActQty1 { get; set; }
        public decimal ActQty2 { get; set; }
        public decimal ActQty3 { get; set; }
        public decimal ActQty4 { get; set; }
        public decimal ActQty5 { get; set; }
        public decimal ActQty6 { get; set; }
        public decimal ActQty7 { get; set; }
        public decimal ActQty8 { get; set; }
        public decimal ActQty9 { get; set; }
        public decimal ActQty10 { get; set; }
        public decimal ActQty11 { get; set; }
        public decimal ActQty12 { get; set; }
        public decimal ActQty13 { get; set; }
        public decimal ActQty14 { get; set; }
        public decimal ActQty15 { get; set; }
        public decimal ActQty16 { get; set; }
        public decimal ActQty17 { get; set; }
        public decimal ActQty18 { get; set; }
        public decimal ActQty19 { get; set; }
        public decimal ActQty20 { get; set; }
        public decimal ActQty21 { get; set; }
        public decimal ActQty22 { get; set; }
        public decimal ActQty23 { get; set; }
        public decimal ActQty24 { get; set; }
        public decimal ActQty25 { get; set; }
        public decimal ActQty26 { get; set; }
        public decimal ActQty27 { get; set; }
        public decimal ActQty28 { get; set; }
        public decimal ActQty29 { get; set; }
        public decimal ActQty30 { get; set; }
        public decimal ActQty31 { get; set; }
        public decimal ActQty32 { get; set; }
        public decimal ActQty33 { get; set; }
        public decimal ActQty34 { get; set; }
        public decimal ActQty35 { get; set; }
        public decimal ActQty36 { get; set; }
        public decimal ActQty37 { get; set; }
        public decimal ActQty38 { get; set; }
        public decimal ActQty39 { get; set; }
        public decimal ActQty40 { get; set; }

        public decimal DisconActQty0 { get; set; }
        public decimal DisconActQty1 { get; set; }
        public decimal DisconActQty2 { get; set; }
        public decimal DisconActQty3 { get; set; }
        public decimal DisconActQty4 { get; set; }
        public decimal DisconActQty5 { get; set; }
        public decimal DisconActQty6 { get; set; }
        public decimal DisconActQty7 { get; set; }
        public decimal DisconActQty8 { get; set; }
        public decimal DisconActQty9 { get; set; }
        public decimal DisconActQty10 { get; set; }
        public decimal DisconActQty11 { get; set; }
        public decimal DisconActQty12 { get; set; }
        public decimal DisconActQty13 { get; set; }
        public decimal DisconActQty14 { get; set; }
        public decimal DisconActQty15 { get; set; }
        public decimal DisconActQty16 { get; set; }
        public decimal DisconActQty17 { get; set; }
        public decimal DisconActQty18 { get; set; }
        public decimal DisconActQty19 { get; set; }
        public decimal DisconActQty20 { get; set; }
        public decimal DisconActQty21 { get; set; }
        public decimal DisconActQty22 { get; set; }
        public decimal DisconActQty23 { get; set; }
        public decimal DisconActQty24 { get; set; }
        public decimal DisconActQty25 { get; set; }
        public decimal DisconActQty26 { get; set; }
        public decimal DisconActQty27 { get; set; }
        public decimal DisconActQty28 { get; set; }
        public decimal DisconActQty29 { get; set; }
        public decimal DisconActQty30 { get; set; }
        public decimal DisconActQty31 { get; set; }
        public decimal DisconActQty32 { get; set; }
        public decimal DisconActQty33 { get; set; }
        public decimal DisconActQty34 { get; set; }
        public decimal DisconActQty35 { get; set; }
        public decimal DisconActQty36 { get; set; }
        public decimal DisconActQty37 { get; set; }
        public decimal DisconActQty38 { get; set; }
        public decimal DisconActQty39 { get; set; }
        public decimal DisconActQty40 { get; set; }

        public string DisplayQty0
        {
            get
            {
                return FormatQty(Qty0) + "(" + FormatReqQty(ReqQty0) +" | " + FormatActQty(ActQty0) + " | " + FormatDisconActQty(DisconActQty0) + ")";
            }
        }
        public string DisplayQty1
        {
            get
            {
                return FormatQty(Qty1) + "(" + FormatReqQty(ReqQty1) + " | " + FormatActQty(ActQty1) + " | " + FormatDisconActQty(DisconActQty1) + ")";
            }
        }
        public string DisplayQty2
        {
            get
            {
                return FormatQty(Qty2) + "(" + FormatReqQty(ReqQty2) + " | " + FormatActQty(ActQty2) + " | " + FormatDisconActQty(DisconActQty2) + ")";
            }
        }
        public string DisplayQty3
        {
            get
            {
                return FormatQty(Qty3) + "(" + FormatReqQty(ReqQty3) + " | " + FormatActQty(ActQty3) + " | " + FormatDisconActQty(DisconActQty3) + ")";
            }
        }
        public string DisplayQty4
        {
            get
            {
                return FormatQty(Qty4) + "(" + FormatReqQty(ReqQty4) + " | " + FormatActQty(ActQty4) + " | " + FormatDisconActQty(DisconActQty4) + ")";
            }
        }
        public string DisplayQty5
        {
            get
            {
                return FormatQty(Qty5) + "(" + FormatReqQty(ReqQty5) + " | " + FormatActQty(ActQty5) + " | " + FormatDisconActQty(DisconActQty5) + ")";
            }
        }
        public string DisplayQty6
        {
            get
            {
                return FormatQty(Qty6) + "(" + FormatReqQty(ReqQty6) + " | " + FormatActQty(ActQty6) + " | " + FormatDisconActQty(DisconActQty6) + ")";
            }
        }
        public string DisplayQty7
        {
            get
            {
                return FormatQty(Qty7) + "(" + FormatReqQty(ReqQty7) + " | " + FormatActQty(ActQty7) + " | " + FormatDisconActQty(DisconActQty7) + ")";
            }
        }
        public string DisplayQty8
        {
            get
            {
                return FormatQty(Qty8) + "(" + FormatReqQty(ReqQty8) + " | " + FormatActQty(ActQty8) + " | " + FormatDisconActQty(DisconActQty8) + ")";
            }
        }
        public string DisplayQty9
        {
            get
            {
                return FormatQty(Qty9) + "(" + FormatReqQty(ReqQty9) + " | " + FormatActQty(ActQty9) + " | " + FormatDisconActQty(DisconActQty9) + ")";
            }
        }
        public string DisplayQty10
        {
            get
            {
                return FormatQty(Qty10) + "(" + FormatReqQty(ReqQty10) + " | " + FormatActQty(ActQty10) + " | " + FormatDisconActQty(DisconActQty10) + ")";
            }
        }
        public string DisplayQty11
        {
            get
            {
                return FormatQty(Qty11) + "(" + FormatReqQty(ReqQty11) + " | " + FormatActQty(ActQty11) + " | " + FormatDisconActQty(DisconActQty11) + ")";
            }
        }
        public string DisplayQty12
        {
            get
            {
                return FormatQty(Qty12) + "(" + FormatReqQty(ReqQty12) + " | " + FormatActQty(ActQty12) + " | " + FormatDisconActQty(DisconActQty12) + ")";
            }
        }
        public string DisplayQty13
        {
            get
            {
                return FormatQty(Qty13) + "(" + FormatReqQty(ReqQty13) + " | " + FormatActQty(ActQty13) + " | " + FormatDisconActQty(DisconActQty13) + ")";
            }
        }
        public string DisplayQty14
        {
            get
            {
                return FormatQty(Qty14) + "(" + FormatReqQty(ReqQty14) + " | " + FormatActQty(ActQty14) + " | " + FormatDisconActQty(DisconActQty14) + ")";
            }
        }
        public string DisplayQty15
        {
            get
            {
                return FormatQty(Qty15) + "(" + FormatReqQty(ReqQty15) + " | " + FormatActQty(ActQty15) + " | " + FormatDisconActQty(DisconActQty15) + ")";
            }
        }
        public string DisplayQty16
        {
            get
            {
                return FormatQty(Qty16) + "(" + FormatReqQty(ReqQty16) + " | " + FormatActQty(ActQty16) + " | " + FormatDisconActQty(DisconActQty16) + ")";
            }
        }
        public string DisplayQty17
        {
            get
            {
                return FormatQty(Qty17) + "(" + FormatReqQty(ReqQty17) + " | " + FormatActQty(ActQty17) + " | " + FormatDisconActQty(DisconActQty17) + ")";
            }
        }
        public string DisplayQty18
        {
            get
            {
                return FormatQty(Qty18) + "(" + FormatReqQty(ReqQty18) + " | " + FormatActQty(ActQty18) + " | " + FormatDisconActQty(DisconActQty18) + ")";
            }
        }
        public string DisplayQty19
        {
            get
            {
                return FormatQty(Qty19) + "(" + FormatReqQty(ReqQty19) + " | " + FormatActQty(ActQty19) + " | " + FormatDisconActQty(DisconActQty19) + ")";
            }
        }
        public string DisplayQty20
        {
            get
            {
                return FormatQty(Qty20) + "(" + FormatReqQty(ReqQty20) + " | " + FormatActQty(ActQty20) + " | " + FormatDisconActQty(DisconActQty20) + ")";
            }
        }
        public string DisplayQty21
        {
            get
            {
                return FormatQty(Qty21) + "(" + FormatReqQty(ReqQty21) + " | " + FormatActQty(ActQty21) + " | " + FormatDisconActQty(DisconActQty21) + ")";
            }
        }
        public string DisplayQty22
        {
            get
            {
                return FormatQty(Qty22) + "(" + FormatReqQty(ReqQty22) + " | " + FormatActQty(ActQty22) + " | " + FormatDisconActQty(DisconActQty22) + ")";
            }
        }
        public string DisplayQty23
        {
            get
            {
                return FormatQty(Qty23) + "(" + FormatReqQty(ReqQty23) + " | " + FormatActQty(ActQty23) + " | " + FormatDisconActQty(DisconActQty23) + ")";
            }
        }
        public string DisplayQty24
        {
            get
            {
                return FormatQty(Qty24) + "(" + FormatReqQty(ReqQty24) + " | " + FormatActQty(ActQty24) + " | " + FormatDisconActQty(DisconActQty24) + ")";
            }
        }
        public string DisplayQty25
        {
            get
            {
                return FormatQty(Qty25) + "(" + FormatReqQty(ReqQty25) + " | " + FormatActQty(ActQty25) + " | " + FormatDisconActQty(DisconActQty25) + ")";
            }
        }
        public string DisplayQty26
        {
            get
            {
                return FormatQty(Qty26) + "(" + FormatReqQty(ReqQty26) + " | " + FormatActQty(ActQty26) + " | " + FormatDisconActQty(DisconActQty26) + ")";
            }
        }
        public string DisplayQty27
        {
            get
            {
                return FormatQty(Qty27) + "(" + FormatReqQty(ReqQty27) + " | " + FormatActQty(ActQty27) + " | " + FormatDisconActQty(DisconActQty27) + ")";
            }
        }
        public string DisplayQty28
        {
            get
            {
                return FormatQty(Qty28) + "(" + FormatReqQty(ReqQty28) + " | " + FormatActQty(ActQty28) + " | " + FormatDisconActQty(DisconActQty28) + ")";
            }
        }
        public string DisplayQty29
        {
            get
            {
                return FormatQty(Qty29) + "(" + FormatReqQty(ReqQty29) + " | " + FormatActQty(ActQty29) + " | " + FormatDisconActQty(DisconActQty29) + ")";
            }
        }
        public string DisplayQty30
        {
            get
            {
                return FormatQty(Qty30) + "(" + FormatReqQty(ReqQty30) + " | " + FormatActQty(ActQty30) + " | " + FormatDisconActQty(DisconActQty30) + ")";
            }
        }
        public string DisplayQty31
        {
            get
            {
                return FormatQty(Qty31) + "(" + FormatReqQty(ReqQty31) + " | " + FormatActQty(ActQty31) + " | " + FormatDisconActQty(DisconActQty31) + ")";
            }
        }
        public string DisplayQty32
        {
            get
            {
                return FormatQty(Qty32) + "(" + FormatReqQty(ReqQty32) + " | " + FormatActQty(ActQty32) + " | " + FormatDisconActQty(DisconActQty32) + ")";
            }
        }
        public string DisplayQty33
        {
            get
            {
                return FormatQty(Qty33) + "(" + FormatReqQty(ReqQty33) + " | " + FormatActQty(ActQty33) + " | " + FormatDisconActQty(DisconActQty33) + ")";
            }
        }
        public string DisplayQty34
        {
            get
            {
                return FormatQty(Qty34) + "(" + FormatReqQty(ReqQty34) + " | " + FormatActQty(ActQty34) + " | " + FormatDisconActQty(DisconActQty34) + ")";
            }
        }
        public string DisplayQty35
        {
            get
            {
                return FormatQty(Qty35) + "(" + FormatReqQty(ReqQty35) + " | " + FormatActQty(ActQty35) + " | " + FormatDisconActQty(DisconActQty35) + ")";
            }
        }
        public string DisplayQty36
        {
            get
            {
                return FormatQty(Qty36) + "(" + FormatReqQty(ReqQty36) + " | " + FormatActQty(ActQty36) + " | " + FormatDisconActQty(DisconActQty36) + ")";
            }
        }
        public string DisplayQty37
        {
            get
            {
                return FormatQty(Qty37) + "(" + FormatReqQty(ReqQty37) + " | " + FormatActQty(ActQty37) + " | " + FormatDisconActQty(DisconActQty37) + ")";
            }
        }
        public string DisplayQty38
        {
            get
            {
                return FormatQty(Qty38) + "(" + FormatReqQty(ReqQty38) + " | " + FormatActQty(ActQty38) + " | " + FormatDisconActQty(DisconActQty38) + ")";
            }
        }
        public string DisplayQty39
        {
            get
            {
                return FormatQty(Qty39) + "(" + FormatReqQty(ReqQty39) + " | " + FormatActQty(ActQty39) + " | " + FormatDisconActQty(DisconActQty39) + ")";
            }
        }
        public string DisplayQty40
        {
            get
            {
                return FormatQty(Qty40) + "(" + FormatReqQty(ReqQty40) + " | " + FormatActQty(ActQty40) + " | " + FormatDisconActQty(DisconActQty40) + ")";
            }
        }
        #endregion
        public decimal TotalQty
        {
            get
            {
                return this.Qty0 + this.Qty1 + this.Qty2 + this.Qty3 + this.Qty4 + this.Qty5 + this.Qty6 + this.Qty7 + this.Qty8 + this.Qty9
                    + this.Qty10 + this.Qty11 + this.Qty12 + this.Qty13 + this.Qty14 + this.Qty15 + this.Qty16 + this.Qty17 + this.Qty18 + this.Qty19
                    + this.Qty20 + this.Qty21 + this.Qty22 + this.Qty23 + this.Qty24 + this.Qty25 + this.Qty26 + this.Qty27 + this.Qty28 + this.Qty29
                    + this.Qty30 + this.Qty31 + this.Qty32 + this.Qty33 + this.Qty34 + this.Qty35 + this.Qty36 + this.Qty37 + this.Qty38 + this.Qty39
                    + this.Qty40;
            }
        }

        private string FormatQty(decimal qty)
        {
            if (qty != 0)
            {
                return "<font color='red'>" + qty.ToString("#,##0") + "</font>";
            }
            else
            {
                return qty.ToString("#,##0");
            }
        }

        private string FormatReqQty(decimal qty)
        {
            if (qty != 0)
            {
                return "<font color='orange'>" + qty.ToString("#,##0") + "</font>";
            }
            else
            {
                return qty.ToString("#,##0");
            }
        }

        private string FormatActQty(decimal qty)
        {
            if (qty != 0)
            {
                return "<font color='green'>" + qty.ToString("#,##0") + "</font>";
            }
            else
            {
                return qty.ToString("#,##0");
            }
        }

        private string FormatDisconActQty(decimal qty)
        {
            if (qty != 0)
            {
                return "<font color='blue'>" + qty.ToString("#,##0") + "</font>";
            }
            else
            {
                return qty.ToString("#,##0");
            }
        }
    }

    public class ScheduleHead
    {
        public string Flow { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public DateTime? LastDateFrom { get; set; }
        public DateTime? LastDateTo { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime StartDate { get; set; }
        private string _dateHead;
        public string DateHead
        {
            get
            {
                if (Type == BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_DAY)
                {
                    _dateHead = Type + "*" + DateFrom.ToString("ddd") + "*" + DateFrom.ToString("MMdd");
                }
                else if (Type == BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_MONTH
                    || Type == BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_WEEK)
                {
                    _dateHead = Type + "*" + DateFrom.ToString("MMdd") + "-" + DateTo.ToString("MMdd");
                }
                else
                {
                    _dateHead = DateTo.ToString("yyyy-MM-dd");
                }
                return _dateHead;
            }
        }
    }

    public class ScheduleView
    {
        public IList<ScheduleHead> ScheduleHeads { get; set; }
        public IList<ScheduleBody> ScheduleBodys { get; set; }
    }
}