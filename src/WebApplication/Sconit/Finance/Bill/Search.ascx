<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Finance_Bill_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="ASTreeView" Namespace="Geekees.Common.Controls" TagPrefix="ct" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Bill.BillNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbCode" runat="server" Visible="true" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlStatus" runat="server" Text="${MasterData.Bill.Status}:" />
            </td>
            <td class="td02">
                <ct:ASDropDownTreeView ID="astvMyTree" runat="server" BasePath="~/Js/astreeview/"
                    DataTableRootNodeValue="0" EnableRoot="false" EnableNodeSelection="false" EnableCheckbox="true"
                    EnableDragDrop="false" EnableTreeLines="false" EnableNodeIcon="false" EnableCustomizedNodeIcon="false"
                    EnableDebugMode="false" EnableRequiredValidator="false" InitialDropdownText=""
                    Width="170" EnableCloseOnOutsideClick="true" EnableHalfCheckedAsChecked="true"
                    DropdownIconDown="~/Js/astreeview/images/windropdown.gif" EnableContextMenuAdd="false"
                    MaxDropdownHeight="200" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPartyCode" runat="server" Text="${MasterData.Bill.Supplier}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyCode" runat="server" DescField="Name" ValueField="Code" ServicePath="SupplierMgr.service"
                    ServiceMethod="GetAllSupplier" Width="250" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlExternalBillNo" runat="server" Text="${MasterData.Bill.ExternalBillNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbExternalBillNo" runat="server" Visible="true" />
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
            <td class="td01">
                物料:
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Description" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
            <td />
            <td class="ttd02">
                <cc1:Button ID="btnSearch1" runat="server" Text="查询明细" OnClick="btnSearch_Click1"
                    Width="79px" CssClass="button2" FunctionId="EditBill" />
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    Width="59px" CssClass="button2" />
                <cc1:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                    Width="59px" CssClass="button2" FunctionId="EditBill" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset runat="server" id="fld_Gv_List" visible="false">
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="true" OnRowDataBound="GV_List_RowDataBound"
        CellPadding="0" AllowSorting="false">
        <Columns>
            <asp:TemplateField HeaderText="Seq">
                <ItemTemplate>
                    <%#Container.DataItemIndex + 1%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>
