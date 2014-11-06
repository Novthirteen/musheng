<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Asn.ascx.cs" Inherits="Inventory_PrintHu_Asn" %>
<%@ Register Src="AsnList.ascx" TagName="List" TagPrefix="uc2" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblAsn" runat="server" Text="${Inventory.PrintHu.Ip}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbAsn" runat="server" CssClass="inputRequired" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="tbAsn_TextChanged"
                    CssClass="button2" />
            </td>
        </tr>
    </table>
</fieldset>
<uc2:List ID="ucList" runat="server" Visible="false" />
