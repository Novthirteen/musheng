<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="MasterData_ItemPoint_Edit" %>

<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblItem" runat="server" Text="${MasterData.Order.OrderDetail.Item.Code}:" />
            </td>
            <td class="td02">
                <asp:Literal ID="tbItemCode" runat="server" Text="" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblModel" runat="server" Text="${MasterData.ItemPoint.Model}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbModel" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblFlow" runat="server" Text="${MasterData.Flow.Flow.Production}:" />
            </td>
            <td class="td02">
                <%--<uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServicePath="FlowMgr.service" AutoPostBack="true" MustMatch="true" Width="250" CssClass="inputRequired"
                    ServiceMethod="GetFlowList" />--%>
                <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServicePath="FlowMgr.service" AutoPostBack="true" MustMatch="true" Width="250" CssClass="inputRequired"
                    ServiceParameter="string:#tbItemCode" ServiceMethod="GetProductionFlowCode" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblPoint" runat="server" Text="${MasterData.ItemPoint.Point}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbPoint" runat="server" />
                <asp:RangeValidator  ID="rvPoint" runat="server" ErrorMessage="${MasterData.ItemPoint.InputError}"
                    Display="Dynamic" ControlToValidate="tbPoint" ValidationGroup="vgCreate" Type="Integer" MinimumValue="0" MaximumValue="99999" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlProductLineFacility" runat="server" Text="${MasterData.Order.OrderHead.ProductLineFacility}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="txProductLineFacility" runat="server" Visible="true" DescField="Code"
                    ValueField="Code" ServicePath="ProductLineFacilityMgr.service" AutoPostBack="true"
                    MustMatch="true" Width="250" ServiceMethod="GetProductLineFacility" ServiceParameter="string:#tbFlow"
                     OnTextChanged="txProductLineFacility_TextChange" CssClass="inputRequired" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblPCBNum" runat="server" Text="${MasterData.ItemPoint.PCBNumber}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPCBNum" runat="server" />
                <asp:RangeValidator  ID="rvPCBNum" runat="server" ErrorMessage="${MasterData.ItemPoint.InputError}"
                    Display="Dynamic" ControlToValidate="txtPCBNum" ValidationGroup="vgCreate" Type="Integer" MinimumValue="0" MaximumValue="99999" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblTransferTime" runat="server" Text="${MasterData.ItemPoint.TransferTime}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtTransferTime" runat="server" />
                <asp:RangeValidator ID="rvTransferTime" runat="server" ErrorMessage="${MasterData.ItemPoint.InputError}"
                 ControlToValidate="txtTransferTime" Type="Double" MinimumValue="0" MaximumValue="99999" ValidationGroup="vgCreate" ></asp:RangeValidator>
            </td>
            <td class="td01">
                <asp:Literal ID="lblEquipmentTime" runat="server" Text="${MasterData.ItemPoint.EquipmentTime}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtEquipmentTime" runat="server" />
                <asp:RangeValidator ID="rvEquipmentTime" runat="server" ErrorMessage="${MasterData.ItemPoint.InputError}"
                 ControlToValidate="txtEquipmentTime" Type="Double" MinimumValue="0" MaximumValue="99999" ValidationGroup="vgCreate" ></asp:RangeValidator>
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
                <cc1:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click"
                    CssClass="button2" ValidationGroup="vgCreate" FunctionId="EditOrder" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                    CssClass="button2" />
            </td>
        </tr>
    </table>
</fieldset>