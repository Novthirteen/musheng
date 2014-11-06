<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="ManageSconit_LeanEngine_Single_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblFlowCode" runat="server" Text="${MasterData.Item.FlowDetail.Flow}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbFlowCode" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblFlowDesc" runat="server" Text="${MasterData.Item.FlowDetail.FlowDescription}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbFlowDesc" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3" />
            <td class="ttd02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" CssClass="query"
                    OnClick="btnSearch_Click" />
            </td>
        </tr>
    </table>
</fieldset>
