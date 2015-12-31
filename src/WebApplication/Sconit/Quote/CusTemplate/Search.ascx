<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Quote_CusTemplate_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblPartyFrom" runat="server" Text="${MasterData.Flow.Party.From.Customer}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyFrom" runat="server" Visible="true" DescField="Name" ValueField="Code" Width="250"
                    ServicePath="PartyMgr.service" ServiceMethod="GetFromParty" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlCostCategory" runat="server" Text="${Quote.Template.CostCategory}:"></asp:Literal>
            </td>
            <td class="td02">
                <uc3:textbox ID="txtCostCategory" runat="server" Visible="true" DescField="Name" ValueField="Id" Width="250"
                    ServicePath="PartyMgr.service" ServiceMethod="GetCostCategory" />
            </td>
        </tr>
        <tr>
            <td class="td01"></td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click" />
                <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click" Visible="false" />
                <asp:Button ID="btnSave" runat="server"  Text="${Common.Button.Save}" OnClick="btnSave_Click" />
            </td>
        </tr>
    </table>
</fieldset>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Panel ID="txtList" runat="server"></asp:Panel></td>
            <td class="td02">
                <asp:CheckBoxList ID="cblCostList" runat="server"></asp:CheckBoxList>
            </td>
            <td class="td01"></td>
            <td class="td02">
            </td>
        </tr>
    </table>
</fieldset>
