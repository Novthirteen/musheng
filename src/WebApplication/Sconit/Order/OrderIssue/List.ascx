﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Distribution_OrderIssue_List" %>
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
            </Columns>
        </asp:GridView>
    </div>
</fieldset>
<div class="tablefooter">
    <cc1:Button ID="btnEditShipQty" runat="server" OnClick="btnEditShipQty_Click" Text="${MasterData.Distribution.Button.EditShipQty}"
        CssClass="button2" FunctionId="ShipOrder" />
    <cc1:Button ID="btnCreatePickList" runat="server" Visible="false" OnClick="btnCreatePickList_Click"
        Text="${Warehouse.PickList.CreatePickList}" CssClass="button2" FunctionId="ShipOrder" />
</div>
