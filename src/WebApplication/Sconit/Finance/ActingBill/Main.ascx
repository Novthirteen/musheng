<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Finance_ActingBill_Main" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblReceiptNo" runat="server" Text="${Reports.ActBill.ReceiptNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbReceiptNo" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblItem" runat="server" Text="${Reports.ActBill.ItemCode}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" DescField="Description" ImageUrlField="ImageUrl"
                    Width="280" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlStartDate" runat="server" Text="${Common.Business.StartDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlEndDate" runat="server" Text="${Common.Business.EndDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
        </tr>
        <tr>
            <td colspan="3" />
            <td class="ttd02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    Width="59px" CssClass="button2" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" OnClick="btnSearch_Click"
                    Width="59px" CssClass="button2" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" OnRowDataBound="GV_List_RowDataBound" DefaultSortExpression="Id"
            DefaultSortDirection="Descending">
            <Columns>
                <asp:TemplateField HeaderText=" ${MasterData.ActingBill.Supplier}" >
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
                <asp:TemplateField HeaderText=" ${Common.Business.ItemDescription}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Item.Description")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.ReferenceItemCode}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ReferenceItemCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.Uom}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Uom.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.EffectiveDate}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "EffectiveDate", "{0:yyyy-MM-dd}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.UnitPrice}">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfUnitPrice" runat="server" Value='<%# Bind("UnitPrice") %>' />
                        <%# DataBinder.Eval(Container.DataItem, "UnitPrice", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.Currency}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Currency.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.BillQty}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "BillQty", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.BilledQty}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "BilledQty", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.ActingBillQty}">
                    <ItemTemplate>
                        <asp:Literal ID="ltlQty" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.ActingAmount}">
                    <ItemTemplate>
                        <asp:Literal ID="ltlAmount" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Finance.Bill.IsProvisionalEstimate}">
                    <ItemTemplate>
                        <asp:Label ID="lblIsProvisionalEstimate" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
