<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Item.ascx.cs" Inherits="Inventory_PrintHu_Item" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#<%= tbHuId.ClientID %>').focus();
    });
</script>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblHuId" runat="server" Text="${Common.Business.HuId}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbHuId" CssClass="inputRequired" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvHuId" runat="server" ErrorMessage="${Inventory.PrintHu.Copies.Required}"
                    Display="Dynamic" ControlToValidate="tbHuId" ValidationGroup="vgHuPrint" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblHuCopies" runat="server" Text="${Inventory.PrintHu.Item.Copies}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbHuCopies" CssClass="inputRequired" Text="1" runat="server" onmouseup="if(!readOnly)select();"></asp:TextBox>
                <asp:RangeValidator ID="rvHuCopies" ControlToValidate="tbHuCopies" runat="server"
                    Display="Dynamic" ErrorMessage="${Common.Validator.Valid.Number}" Type="Integer"
                    MinimumValue="0" MaximumValue="100" ValidationGroup="vgHuPrint" />
                <asp:RequiredFieldValidator ID="rfvHuCopies" runat="server" ErrorMessage="${Inventory.PrintHu.Copies.Required}"
                    Display="Dynamic" ControlToValidate="tbHuCopies" ValidationGroup="vgHuPrint" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:Button ID="btnPrintHu" runat="server" Text="${Common.Button.Print}" OnClick="btnHuPrint_Click"
                    CssClass="button2" ValidationGroup="vgHuPrint" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblItemCode" runat="server" Text="${Inventory.PrintHu.Item.Code}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItemCode" runat="server" Visible="true" DescField="Description"
                    ImageUrlField="ImageUrl" Width="280" ValueField="Code" ServicePath="ItemMgr.service"
                    ServiceMethod="GetCacheAllItem" CssClass="inputRequired" AutoPostBack="true"
                    OnTextChanged="tbItem_TextChanged" onclick="if(!readOnly)select();" />
                <asp:RequiredFieldValidator ID="rfvItem" runat="server" ErrorMessage="${Inventory.PrintHu.Item.Code.Required}"
                    Display="Dynamic" ControlToValidate="tbItemCode" ValidationGroup="vgPrint" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:RadioButtonList runat="server" ID="rblPackageType" RepeatDirection="Horizontal">
                    <asp:ListItem runat="server" Text="${Inventory.PrintHu.Inner.Package}" Value="0"
                        Selected="True" />
                    <asp:ListItem runat="server" Text="${Inventory.PrintHu.Outer.Package}" Value="1" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblItemHuId" runat="server" Text="${Common.Business.HuId}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbItemHuId" runat="server" AutoPostBack="true" OnTextChanged="tbItemHuId_TextChanged" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblItemUom" runat="server" Text="${Inventory.PrintHu.Item.Uom}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbItemUom" runat="server" onfocus="this.blur();"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblItemDescription" runat="server" Text="${Inventory.PrintHu.Item.Description}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbItemDescription" runat="server" onfocus="this.blur();"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="lblItemBrand" runat="server" Text="${Inventory.PrintHu.Item.Brand}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbItemBrand" runat="server" onfocus="this.blur();"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblItemUnitCount" runat="server" Text="${Inventory.PrintHu.Item.UnitCount}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbItemUnitCount" runat="server"></asp:TextBox>
                <asp:RangeValidator ID="rvItemUnitCount" ControlToValidate="tbItemUnitCount" runat="server"
                    Display="Dynamic" ErrorMessage="${Common.Validator.Valid.Number}" Type="Double"
                    MaximumValue="999999999" MinimumValue="0.00000001" ValidationGroup="vgPrint" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblHuLotSize" runat="server" Text="${Inventory.PrintHu.Item.HuLotSize}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbHuLotSize" runat="server"></asp:TextBox>
                <asp:RangeValidator ID="rvHuLotSize" ControlToValidate="tbHuLotSize" runat="server"
                    Display="Dynamic" ErrorMessage="${Common.Validator.Valid.Number}" Type="Integer"
                    MinimumValue="0" MaximumValue="999999999" ValidationGroup="vgPrint" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblManufactureDate" runat="server" Text="${Common.Business.InventoryDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbManufactureDate" runat="server" CssClass="inputRequired" onmouseup="if(!readOnly)select();"
                    onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvManufactureDate" runat="server" ErrorMessage="${Inventory.PrintHu.ManufactureDate.Required}"
                    Display="Dynamic" ControlToValidate="tbManufactureDate" ValidationGroup="vgPrint" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblSupplierLotNo" runat="server" Text="${Inventory.PrintHu.Item.SupplierLotNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbSupplierLotNo" runat="server" CssClass="inputRequired" onmouseup="if(!readOnly)select();"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSupplierLotNo" runat="server" ErrorMessage="${Inventory.PrintHu.Item.SupplierLotNo.Required}"
                    Display="Dynamic" ControlToValidate="tbSupplierLotNo" ValidationGroup="vgPrint" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:RadioButtonList runat="server" ID="rblQtyType" RepeatDirection="Horizontal"
                    CssClass="floatright">
                    <asp:ListItem runat="server" Text="${Common.Business.Qty}" Value="0" Selected="True" />
                    <asp:ListItem runat="server" Text="${Inventory.PrintHu.Item.Papers}" Value="1" />
                </asp:RadioButtonList>
            </td>
            <td class="td02">
                <asp:TextBox ID="tbPapers" runat="server" onmouseup="if(!readOnly)select();" CssClass="inputRequired"></asp:TextBox>
                <asp:RangeValidator ID="rvPapers" ControlToValidate="tbPapers" runat="server" Display="Dynamic"
                    ErrorMessage="${Common.Validator.Valid.Number}" Type="Double" MinimumValue="0"
                    MaximumValue="99999999" ValidationGroup="vgPrint" />
                <asp:RequiredFieldValidator ID="rfvPapers" runat="server" ErrorMessage="${Inventory.PrintHu.Papers.Required}"
                    Display="Dynamic" ControlToValidate="tbPapers" ValidationGroup="vgPrint" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblCopies" runat="server" Text="${Inventory.PrintHu.Item.Copies}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbCopies" Text="1" runat="server" onmouseup="if(!readOnly)select();"
                    CssClass="inputRequired"></asp:TextBox>
                <asp:RangeValidator ID="rvCopies" ControlToValidate="tbCopies" runat="server" Display="Dynamic"
                    ErrorMessage="${Common.Validator.Valid.Number}" Type="Integer" MinimumValue="0"
                    MaximumValue="100" ValidationGroup="vgPrint" />
                <asp:RequiredFieldValidator ID="rfvCopies" runat="server" ErrorMessage="${Inventory.PrintHu.Copies.Required}"
                    Display="Dynamic" ControlToValidate="tbCopies" ValidationGroup="vgPrint" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblSortLevel1" runat="server" Text="${Inventory.PrintHu.FlowDetail.SortLevel1}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbSortLevel1" ReadOnly="true" runat="server" onmouseup="this.value=this.value.toUpperCase()"  onkeyup="this.value=this.value.toUpperCase()"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSortLevel1" runat="server" ErrorMessage="${Inventory.PrintHu.SortLevel1.Required}"
                    Display="Dynamic" ControlToValidate="tbSortLevel1" ValidationGroup="vgPrint" />
                <asp:Literal ID="lblSortLevel1Msg" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblColorLevel1" runat="server" Text="${Inventory.PrintHu.FlowDetail.ColorLevel1}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbColorLevel1" ReadOnly="true" runat="server" onmouseup="this.value=this.value.toUpperCase()"  onkeyup="this.value=this.value.toUpperCase()"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvColorLevel1" runat="server" ErrorMessage="${Inventory.PrintHu.ColorLevel1.Required}"
                    Display="Dynamic" ControlToValidate="tbColorLevel1" ValidationGroup="vgPrint" />
                <asp:Literal ID="lblColorLevel1Msg" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblSortLevel2" runat="server" Text="${Inventory.PrintHu.FlowDetail.SortLevel2}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbSortLevel2" ReadOnly="true" runat="server" onmouseup="this.value=this.value.toUpperCase()"  onkeyup="this.value=this.value.toUpperCase()"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSortLevel2" runat="server" ErrorMessage="${Inventory.PrintHu.SortLevel2.Required}"
                    Display="Dynamic" ControlToValidate="tbSortLevel2" ValidationGroup="vgPrint" />
                <asp:Literal ID="lblSortLevel2Msg" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblColorLevel2" runat="server" Text="${Inventory.PrintHu.FlowDetail.ColorLevel2}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbColorLevel2" ReadOnly="true" runat="server" onmouseup="this.value=this.value.toUpperCase()"  onkeyup="this.value=this.value.toUpperCase()"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvColorLevel2" runat="server" ErrorMessage="${Inventory.PrintHu.ColorLevel2.Required}"
                    Display="Dynamic" ControlToValidate="tbColorLevel2" ValidationGroup="vgPrint" />
                <asp:Literal ID="lblColorLevel2Msg" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="ttd01">
            </td>
            <td class="ttd02">
                <asp:CheckBox ID="cbIsContinuousPrinting" runat="server" Text="${Inventory.PrintHu.ContinuousPrinting}" />
            </td>
            <td class="ttd01">
            </td>
            <td class="ttd02">
                <asp:Button ID="btnPrint" runat="server" Text="${Common.Button.Print}" OnClick="btnPrint_Click"
                    CssClass="button2" ValidationGroup="vgPrint" />
            </td>
        </tr>
    </table>
</fieldset>
