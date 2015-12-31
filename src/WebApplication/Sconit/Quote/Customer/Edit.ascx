<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Quote_Customer_Edit" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlCustomerCode" runat="server" Text="${MasterData.Customer.Code}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtCustomerCode" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlCustomerName" runat="server" Text="${Quote.Tooling.Customer}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlSType" runat="server" Text="${Quote.Fee.SType}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlSType" runat="server"></asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlBillPeriod" runat="server" Text="${Quote.Fee.BillPeriod}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtBillPeriod" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlP_ManageFee" runat="server" Text="${Quote.Fee.P_ManageFee}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtP_ManageFee" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlP_FinanceFee" runat="server" Text="${Quote.Fee.P_FinanceFee}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtP_FinanceFee" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlP_Profit" runat="server" Text="${Quote.Fee.P_Profit}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtP_Profit" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlP_LossRate" runat="server" Text="${Quote.Fee.P_LossRate}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtP_LossRate" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlM_ManageFee" runat="server" Text="${Quote.Fee.M_ManageFee}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtM_ManageFee" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlM_FinanceFee" runat="server" Text="${Quote.Fee.M_FinanceFee}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtM_FinanceFee" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlSatrtDate" runat="server" Text="${Quote.Fee.StartDate}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlEndDate" runat="server" Text="${Quote.Fee.EndDate}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlDeliveryAdd" runat="server" Text="${Quote.Fee.DeliveryAdd}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtDeliveryAdd" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlDeliveryCity" runat="server" Text="${Quote.Fee.DeliveryCity}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtDeliveryCity" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlStatus" runat="server" Text="${Quote.Fee.Status}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:CheckBox ID="ckbStatus" runat="server" />
            </td>
            <td class="ttd01"></td>
            <td class="ttd02"></td>
        </tr>
        <tr>
            <td class="ttd01"></td>
            <td class="ttd02"></td>
            <td class="ttd01">
                <asp:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click" />
            </td>
            <td class="ttd02">
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
            </td>
        </tr>
    </table>
</fieldset>