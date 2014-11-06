<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewSearch.ascx.cs" Inherits="Finance_Bill_NewSearch" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Src="NewList.ascx" TagName="NewList" TagPrefix="uc2" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPartyCode" runat="server" Text="${MasterData.ActingBill.Supplier}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyCode" runat="server" DescField="Name" ValueField="Code" ServicePath="SupplierMgr.service"
                    ServiceMethod="GetAllSupplier" Width="250" />
                <asp:Literal ID="ltlParty" runat="server" Visible="false" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlReceiver" runat="server" Text="${MasterData.ActingBill.ReceiptNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbReceiver" runat="server" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlBillAddress" runat="server" Text="${MasterData.Address.BillAddress}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbBillAddress" runat="server" DescField="Address" ValueField="Code"
                    ServicePath="BillAddressMgr.service" ServiceMethod="GetBillAddress" Width="300" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlFlow" runat="server" Text="${Common.Business.Message.Flow}" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    Width="250" ServicePath="FlowMgr.service" ServiceMethod="GetProcurementFlow" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlStartDate" runat="server" Text="${MasterData.ActingBill.EffectiveDateFrom}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlEndDate" runat="server" Text="${MasterData.ActingBill.EffectiveDateTo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlItem" runat="server" Text="${Common.Business.ItemCode}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItemCode" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Description" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlCurrency" runat="server" Text="${MasterData.ActingBill.Currency}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCurrency" runat="server" Visible="true" Width="250" DescField="Name"
                    ValueField="Code" ServicePath="CurrencyMgr.service" ServiceMethod="GetAllCurrency" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:CheckBox ID="IsRelease" runat="server" Text="${MasterData.ActingBill.IsRelease}" />
            </td>
            <td class="td02">
                <asp:CheckBox ID="cbOrderByItem" runat="server" Text="${Common.GridView.OrderBy.Item}" />
                <!---Start 春申客户化-->
                <asp:CheckBox ID="cbZS" runat="server" Visible="false" Text="注塑" />
                <asp:CheckBox ID="cbGS" runat="server" Visible="false" Text="钢丝" />
                <!---End 春申客户化-->
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    Width="59px" CssClass="button2" />
                <asp:Button ID="btnConfirm" runat="server" Text="${Common.Button.Create}" OnClick="btnConfirm_Click"
                    Width="59px" CssClass="button2" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                    Width="59px" CssClass="button2" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                    OnClick="btnSearch_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<uc2:NewList ID="ucNewList" runat="server" />
<asp:Button ID="btnAddDetail" runat="server" Text="${Common.Button.AddDetail}" OnClick="btnAddDetail_Click"
    CssClass="button2" Visible="false" />
<asp:Button ID="btnClose" runat="server" OnClick="btnBack_Click" Width="59px" CssClass="button2"
    Text="返回" Visible="false" />