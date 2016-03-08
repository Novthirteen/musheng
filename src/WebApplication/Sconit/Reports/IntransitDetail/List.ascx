<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_IntransitDetail_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeys="ItemCode, Uom, UnitCount" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                 <asp:TemplateField HeaderText="${Reports.IntransitDetail.OrderNo}">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("OrderNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="${Reports.IntransitDetail.Asn}">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("IpNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.Supplier}">
                    <ItemTemplate>
                        <asp:Label ID="lblSupplier" runat="server" Text='<%# Eval("PartyFrom")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.Customer}">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("PartyTo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.ItemCode}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.ItemDescription}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDescription" runat="server" Text='<%# Eval("ItemName")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.ReferenceItemCode}">
                    <ItemTemplate>
                        <asp:Label ID="lblReferenceItemCode" runat="server" Text='<%# Eval("ReferenceItem")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.Uom}">
                    <ItemTemplate>
                        <asp:Label ID="lblUom" runat="server" Text='<%# Eval("Uom")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.UnitCount}">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCount" runat="server" Text='<%# Eval("UnitCount", "{0:0.###}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.DefaultActivity}">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCount" runat="server" Text='<%# Eval("DefaultActivity", "{0:0.###}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Literal ID="lblNoRecordFound" runat="server" Text="${Common.GridView.NoRecordFound}"
            Visible="false" />
    </div>
</fieldset>
