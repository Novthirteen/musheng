<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewList.ascx.cs" Inherits="Finance_Bill_NewList" %>

<script language="javascript" type="text/javascript" src="Js/calcamount.js"></script>

<script type="text/javascript" language="javascript">
    function GVCheckClick() {
        if ($(".GVHeader input:checkbox").attr("checked") == true) {
            $(".GVRow input:checkbox").attr("checked", true);
            $(".GVAlternatingRow input:checkbox").attr("checked", true);
        }
        else {
            $(".GVRow input:checkbox").attr("checked", false);
            $(".GVAlternatingRow input:checkbox").attr("checked", false);
        }
    }
</script>

<fieldset>
    <div>
        <asp:Literal ID="ltlCurrentBillQtyTotal" runat="server" Text="${MasterData.ActingBill.CurrentBillQtyTotal}:" />
        <asp:Literal ID="ltlCurrentBillQtyTotal1" runat="server" Text="0" />
        <asp:Literal ID="ltlAmountTotal" runat="server" Text="${MasterData.ActingBill.AmountTotal}:" />
        <asp:Literal ID="ltlAmountTotal1" runat="server" Text="0" />
    </div>
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AllowPaging="False" DataKeyNames="Id" AllowSorting="False"
            AutoGenerateColumns="False" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <div onclick="GVCheckClick()">
                            <asp:CheckBox ID="CheckAll" runat="server" OnCheckedChanged="CheckAll_CheckedChanged" AutoPostBack="true" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HiddenField ID="hfId" runat="server" Value='<%# Bind("Id") %>' />
                        <%--modify by ljz start--%>
                        <%--<asp:CheckBox ID="CheckBoxGroup" name="CheckBoxGroup" runat="server" />--%>
                        <asp:CheckBox ID="CheckBoxGroup" name="CheckBoxGroup" runat="server" OnCheckedChanged="CheckBoxGroup_CheckedChanged" AutoPostBack="true" />
                        <%--modify by ljz end--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.ActingBill.Supplier}" SortExpression="BillAddress.Party.Name">
                    <ItemTemplate>
                        <asp:Label ID="lblSupplier" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BillAddress.Party.Code")%>'
                            ToolTip='<%# DataBinder.Eval(Container.DataItem, "BillAddress.Party.Name")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ReceiptNo" HeaderText="${MasterData.ActingBill.ReceiptNo}"
                    SortExpression="ReceiptNo" />
                <asp:BoundField DataField="ExternalReceiptNo" HeaderText="${MasterData.ActingBill.ExternalReceiptNo}"
                    SortExpression="ExternalReceiptNo" />
                <asp:TemplateField HeaderText=" ${Common.Business.ItemCode}" SortExpression="Item.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Item.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.ItemDescription}" SortExpression="Item.Description">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Item.Description")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.ReferenceItemCode}" SortExpression="ReferenceItemCode">
                    <%--HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"--%>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ReferenceItemCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.Uom}" SortExpression="Uom.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Uom.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.EffectiveDate}" SortExpression="EffectiveDate">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "EffectiveDate", "{0:yyyy-MM-dd}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.UnitPrice}" SortExpression="UnitPrice">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfUnitPrice" runat="server" Value='<%# Bind("UnitPrice") %>' />
                        <%# DataBinder.Eval(Container.DataItem, "UnitPrice", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.Currency}" SortExpression="Currency.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Currency.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.BillQty}" SortExpression="BillQty">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "BillQty", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.BilledQty}" SortExpression="BilledQty">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "BilledQty", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.CurrentBillQty}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbQty" runat="server" onmouseup="if(!readOnly)select();" Width="50"
                            onchange="CalCulateRowAmount(this, 'tbQty', 'BaseOnDiscountRate', 'hfUnitPrice', 'tbQty', 'tbDiscount', 'tbDiscountRate', 'tbAmount',false);"></asp:TextBox>
                        <span style="display: none">
                            <asp:TextBox ID="tbDiscountRate" runat="server" Text="0" />
                            <asp:TextBox ID="tbDiscount" runat="server" Text="0" />
                        </span>
                        <asp:Literal ID="ltlQty" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- <asp:TemplateField HeaderText="${MasterData.ActingBill.DiscountRate}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbDiscountRate" runat="server" onmouseup="if(!readOnly)select();"
                            Width="50" onchange="CalCulateRowAmount(this, 'tbDiscountRate', 'BaseOnDiscountRate', 'hfUnitPrice', 'tbQty', 'tbDiscount', 'tbDiscountRate', 'tbAmount',false);" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.Discount}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbDiscount" runat="server" onmouseup="if(!readOnly)select();" Width="50"
                            onchange="CalCulateRowAmount(this, 'tbDiscount', 'BaseOnDiscount', 'hfUnitPrice', 'tbQty', 'tbDiscount', 'tbDiscountRate', 'tbAmount',false);" />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.Amount}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbAmount" runat="server" Width="80" />
                        <asp:Literal ID="ltlAmount" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.Address.Code}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "BillAddress.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.Address.Address}" >
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "BillAddress.Address")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.Flow.Code}" >
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "FlowCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Literal ID="lblNoRecordFound" runat="server" Text="${Common.GridView.NoRecordFound}"
            Visible="false" />
    </div>
</fieldset>
