<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Quote_Template_Edit" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlCostCategory" runat="server" Text="${Quote.Template.CostCategory}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtCostCategory" runat="server"></asp:TextBox>
            </td>
            <td class="td01"></td>
            <td class="td02"></td>
        </tr>
        <tr>
            <td class="td01"></td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <asp:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click" />
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Butto.SearchCostList}" OnClick="btnSearch_Click" />
                <asp:Button ID="btnNew" runat="server" Text="${Common.Butto.NewCostList}" OnClick="btnNew_Click" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
            </td>
        </tr>
    </table>
</fieldset>