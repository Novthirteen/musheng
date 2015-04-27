<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Cost_FinanceCalendar_Search" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblYear" runat="server" Text="${Cost.FinanceCalendar.Year}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbYear" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblMonth" runat="server" Text="${Cost.FinanceCalendar.Month}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbMonth" runat="server"></asp:TextBox>
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
                    <asp:Button ID="btnUpBillPeriod" runat="server" Text="${Common.Button.UpBillPeriod}" OnClick="btnUpBillPeriod_Click"
                        CssClass="add" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
