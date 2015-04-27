<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UpBillPeriod.ascx.cs" Inherits="Cost_FinanceCalendar_UpBillPeriod" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblPurchaseAccountDate" runat="server" Text="${Common.Business.PurchaseAccountDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbPurchaseAccountDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"  CssClass="inputRequired" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblFinanceAccountDate" runat="server" Text="${Common.Business.FinanceAccountDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbFinanceAccountDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"  CssClass="inputRequired" />
            </td>
        </tr>
    </table>
    <div class="tablefooter">
        <asp:Button ID="btnUpdate" runat="server" Text="修改" OnClick="btnUpdate_Click" OnClientClick="return confirm('确定要修改吗？')" />
        <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
    </div>
</fieldset>


