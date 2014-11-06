<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Cost_CostGroup_Search" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblCode" runat="server" Text="${Cost.CostGroup.Code}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbCode" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlDescription" runat="server" Text="${Cost.CostGroup.Description}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbDescription" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
            <td class="td02">
                <div class="buttons">
                    <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                        CssClass="query" />
                    <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                        CssClass="add" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
