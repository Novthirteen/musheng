<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Quote_Item_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal runat="server" ID="ltlProjectId" Text="${Quote.Tooling.ProjectNo}:"></asp:Literal>
            </td>
            <td class="td02">
                <uc3:textbox ID="txtProjectId" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Descr" ValueField="ID" ServicePath="OrderProductionPlanMgr.service" ServiceMethod="GetGPID" ServiceParameter="bool:true" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlItemCode" runat="server" Text="${Menu.MasterData.Item}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtItemCode" runat="server"></asp:TextBox>
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