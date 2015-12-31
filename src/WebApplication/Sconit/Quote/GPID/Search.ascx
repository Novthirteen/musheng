<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Quote_GPID_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPID" runat="server" Text="${Quote.Tooling.ProjectNo}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPID" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlCustomer" runat="server" Text="${Quote.GPID.Cusomer}:"></asp:Literal>
            </td>
            <td class="td02">
                <uc3:textbox ID="txtCustomer" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Name" ValueField="Code" ServicePath="OrderProductionPlanMgr.service" ServiceMethod="GetCustomer" />
            </td>
        </tr>
        <tr>
            <td class="td01"></td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click" />
                <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click" />
            </td>
        </tr>
    </table>
</fieldset>