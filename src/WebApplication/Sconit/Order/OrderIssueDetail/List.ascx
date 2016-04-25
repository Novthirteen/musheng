<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Order_OrderIssueDetail_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">
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
    <legend>${MasterData.Order.OrderHead.AvailableOrder}</legend>
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" OnRowDataBound="GV_List_RowDataBound" >
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <div onclick="GVCheckClick()">
                            <asp:CheckBox ID="CheckAll" OnCheckedChanged="CheckAll_CheckedChanged" AutoPostBack="true" runat="server" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxGroup" OnCheckedChanged="CheckBoxGroup_CheckedChanged" AutoPostBack="true" name="CheckBoxGroup" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Literal ID="ltlId" runat="server" Text='<%# Eval("Id")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderHead.OrderNo.Distribution}"
                    SortExpression="OrderNo">
                    <ItemTemplate>
                        <%--<asp:Literal ID="ltlOrderNo" runat="server" Text='<%# Eval("OrderNo")%>' />--%>
                        <asp:Literal ID="ltlOrderNo" runat="server" Text='<%# Eval("OrderHead.OrderNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataSequence%>">
                    <ItemTemplate>
                        <asp:Literal ID="ltlSequence" runat="server" Text='<%# Eval("Sequence")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataItemCode%>"
                    SortExpression="Item.Code">
                    <ItemTemplate>
                        <%--<%# DataBinder.Eval(Container.DataItem, "PartyFrom.Name")%>--%>
                        <asp:Literal ID="ltlItem" runat="server" Text='<%# Eval("Item.Code")%>'></asp:Literal>
                        <%--<%# DataBinder.Eval(Container.DataItem, "Item.Code")%>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataItemDesc%>" SortExpression="Item.Description">
                    <ItemTemplate>
                       <%-- <%# DataBinder.Eval(Container.DataItem, "PartyTo.Name")%>--%>
                        <%# DataBinder.Eval(Container.DataItem, "Item.Description")%>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="${MasterData.Order.OrderHead.StartTime}" SortExpression="Item.Description">
                    <ItemTemplate>
                       <%-- <%# DataBinder.Eval(Container.DataItem, "PartyTo.Name")%>--%>
                        <%# DataBinder.Eval(Container.DataItem, "OrderHead.StartTime")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderHead.WindowTime}" SortExpression="Item.Description">
                    <ItemTemplate>
                       <%-- <%# DataBinder.Eval(Container.DataItem, "PartyTo.Name")%>--%>
                       <asp:Label ID="lblWinTime" Text='<%# Bind("OrderHead.WindowTime") %>' runat="server" />
                        <%--<%# DataBinder.Eval(Container.DataItem, "OrderHead.WindowTime")%>--%>
                    </ItemTemplate>
                </asp:TemplateField>  
                <%--<asp:BoundField DataField="StartTime" HeaderText="${MasterData.Order.OrderHead.StartTime}"
                    SortExpression="StartTime" />--%>
                <asp:BoundField DataField="ReferenceItemCode" HeaderText="<%$Resources:Language,MasterDataReferenceItemCode%>" />
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataUom%>" SortExpression="Uom.Code">
                    <ItemTemplate>
                        <%--<asp:Label ID="lblWinTime" Text='<%# Bind("WindowTime") %>' runat="server" />--%>
                        <%# DataBinder.Eval(Container.DataItem, "Uom.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataUnitCount%>">
                    <ItemTemplate>
                        <%--<cc1:CodeMstrLabel ID="lblPriority" runat="server" Code="OrderPriority" Value='<%# Bind("Priority") %>' />--%>
                        <asp:Label ID="lblUnitCount" runat="server" Text='<%# Bind("UnitCount","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderHead.CreateUser}" SortExpression="OrderHead.CreateUser.CodeName">
                    <ItemTemplate>
                        <%--<%# DataBinder.Eval(Container.DataItem, "CreateUser.Name")%>--%>
                        <%# DataBinder.Eval(Container.DataItem, "OrderHead.CreateUser.CodeName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.OrderedQty}">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderedQty" runat="server" Text='<%# string.Format("{0:0.########}", Eval("OrderedQty")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.ShippedQty}">
                    <ItemTemplate>
                        <asp:Label ID="lblShippedQty" runat="server" Text='<%# string.Format("{0:0.########}", Eval("ShippedQty")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
<%--                <asp:TemplateField HeaderText="待发数">
                    <ItemTemplate>
                        <asp:Label ID="lblReceivedQty" runat="server" Text='<%# string.Format("{0:0.########}", Convert.ToDecimal(Eval("OrderedQty"))-Convert.ToDecimal(Eval("ShippedQty") == null? 0:Eval("ShippedQty"))) %>' />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.CurrentShipQty}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbCurrentQty" runat="server" Text='<%# string.Format("{0:0.########}", Convert.ToDecimal(Eval("OrderedQty"))-Convert.ToDecimal(Eval("ShippedQty") == null? 0:Eval("ShippedQty"))) %>'
                            onmouseup="if(!readOnly)select();" Width="50"></asp:TextBox>
                        <asp:RangeValidator ID="rvCurrentQty" ControlToValidate="tbCurrentQty" runat="server"
                            Display="Dynamic" ErrorMessage="*" MaximumValue="999999999" MinimumValue="0"
                            Enabled="false" Type="Double" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</fieldset>
<div class="tablefooter">
    <asp:CheckBox ID="cbPrintAsn" runat="server" Text="${MasterData.Distribution.PrintAsn}" />
    <cc1:Button ID="btnShip" runat="server" OnClick="btnShip_Click" Text="${MasterData.Distribution.Button.Ship}"
        FunctionId="ShipOrder" OnClientClick="return confirm('${Common.Order.Confirm.Ship}')"  />
    <cc1:Button ID="btnPrint" runat="server" Text="${Common.Button.Print}" CssClass="button2"
        OnClick="btnPrint_Click" FunctionId="PrintOrder" />
    <%--<cc1:Button ID="btnCreatePickList" runat="server" OnClick="btnCreatePickList_Click"
        Text="${Warehouse.PickList.CreatePickList}" CssClass="button2" FunctionId="ShipOrder" />
    <cc1:Button ID="btnEditShipQty" runat="server" OnClick="btnEditShipQty_Click" Text="条码发货"
        CssClass="button2" FunctionId="ShipOrder" />--%>
</div>
<div class="tablefooter" style=" display:none;">
<%--<div class="tablefooter">--%>
<%--    <cc1:Button ID="btnEditShipQty" runat="server" OnClick="btnEditShipQty_Click" Text="${MasterData.Distribution.Button.EditShipQty}"
        CssClass="button2" FunctionId="ShipOrder" />--%>
<%--    <cc1:Button ID="btnCreatePickList" runat="server" Visible="false" OnClick="btnCreatePickList_Click"
        Text="${Warehouse.PickList.CreatePickList}" CssClass="button2" FunctionId="ShipOrder" />--%>
</div>
