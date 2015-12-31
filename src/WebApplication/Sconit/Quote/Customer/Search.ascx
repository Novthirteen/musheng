<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Quote_Customer_Search" %>

<%@ Register Assembly="ASTreeView" Namespace="Geekees.Common.Controls" TagPrefix="ct" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="ttd01">
                <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Customer.Code}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbCode" runat="server" Visible="true" />
            </td>
            <td class="ttd01">
                <asp:Literal ID="lblName" runat="server" Text="${MasterData.Customer.Name}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbName" runat="server" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlSataus" runat="server" Text="${Quote.Project.Status}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:CheckBox ID="cbTrue" runat="server" Checked="true" />有效<asp:CheckBox ID="cbFalse" runat="server" Checked="true" />无效
            </td>
            <td class="ttd01"></td>
            <td class="ttd02"></td>
        </tr>
        <tr>
            <td colspan="3" />
            <td class="ttd02">
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