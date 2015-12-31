<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="MasterData_ItemPack_Search" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlSpec" runat="server" Text="${MasterData.ItemPack.Spec}"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtSpec" runat="server"></asp:TextBox>
            </td>
            <td class="td01"></td>
            <td class="td02"></td>
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