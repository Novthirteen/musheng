<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ScrapNew.ascx.cs" Inherits="Order_OrderHead_ScrapNew" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Src="~/Order/OrderDetail/List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="~/Order/OrderDetail/HuList.ascx" TagName="HuList" TagPrefix="uc2" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblFlow" runat="server" Text="${MasterData.Flow.Flow.Production}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServicePath="FlowMgr.service" OnTextChanged="tbFlow_TextChanged" AutoPostBack="true"
                    MustMatch="true" Width="250" CssClass="inputRequired" ServiceMethod="GetFlowList" />
                <asp:RequiredFieldValidator ID="rfvFlow" runat="server" ErrorMessage="${MasterData.Order.OrderHead.Flow.Required}"
                    Display="Dynamic" ControlToValidate="tbFlow" ValidationGroup="vgCreate" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlProductLineFacility" runat="server" Text="${MasterData.Order.OrderHead.ProductLineFacility}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="txProductLineFacility" runat="server" Visible="true" DescField="Code" ValueField="Code"
                    ServicePath="ProductLineFacilityMgr.service" AutoPostBack="true"
                    MustMatch="true" Width="250" ServiceMethod="GetProductLineFacility" ServiceParameter="string:#tbFlow" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlShift" runat="server" Text="${MasterData.WorkCalendar.Shift}:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="ddlShift" runat="server" DataTextField="ShiftName" DataValueField="Code" />
                <asp:RequiredFieldValidator ID="rfvShift" runat="server" ErrorMessage="${MasterData.Order.OrderHead.Shift.Required}"
                    Display="Dynamic" ControlToValidate="ddlShift" ValidationGroup="vgCreate" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblExtOrderNo" runat="server" Text="${MasterData.Order.OrderHead.ExtOrderNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbExtOrderNo" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="lblRefOrderNo" runat="server" Text="${MasterData.Order.OrderHead.Flow.RefOrderNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbRefOrderNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:CheckBox ID="cbPrintOrder" runat="server" Text="${MasterData.Order.OrderHead.PrintOrder}" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <cc1:Button ID="btnConfirm" runat="server" Text="${Common.Button.Create}" OnClick="btnConfirm_Click"
                    CssClass="button2" ValidationGroup="vgCreate" FunctionId="EditOrder" />
            </td>
        </tr>
    </table>
</fieldset>
<uc2:List ID="ucList" runat="server" Visible="false" />
<uc2:HuList ID="ucHuList" runat="server" Visible="false" />
