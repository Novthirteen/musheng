<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="MasterData_ItemType_Search" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="ttd01">
                <asp:Literal ID="lblCode" runat="server" Text="${MasterData.ItemType.Code}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbCode" runat="server" Visible="true" />
            </td>
            <td class="ttd01">
                <asp:Literal ID="lblName" runat="server" Text="${MasterData.ItemType.Name}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbName" runat="server" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="lblLevel" runat="server" Text="${MasterData.ItemType.Level}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbLevel" runat="server" Visible="true" />
                <asp:RangeValidator ID="rvLevel" ControlToValidate="tbLevel" runat="server"
                    Display="Dynamic" ErrorMessage="${MasterData.ItemType.Level.Format}" MaximumValue="4"
                    MinimumValue="1" Type="Double" ValidationGroup="vgSave" />
            </td>
            <td class="ttd01">
            </td>
            <td class="ttd02">
            </td>
        </tr>
        <tr>
            <td colspan="3" />
            <td class="ttd02">
                <div class="buttons">
                    <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                        CssClass="query" />
                    <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                        CssClass="back" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
