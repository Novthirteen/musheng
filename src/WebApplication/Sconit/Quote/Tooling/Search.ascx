<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Quote_Tooling_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlProjectNo" runat="server" Text="${Quote.Tooling.ProjectNo}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="txtProjectNo" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Descr" ValueField="ID" ServicePath="OrderProductionPlanMgr.service" ServiceMethod="GetGPID" ServiceParameter="bool:true" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlTLNo" runat="server" Text="${Quote.Tooling.TLNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtTLNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlTLName" runat="server" Text="${Quote.Tooling.TLName}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtTLName" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlTLSpec" runat="server" Text="${Quote.Tooling.TLSpec}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtTLSpec" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlCustomerName" runat="server" Text="${Quote.Tooling.Customer}:" />
            </td>
            <td class="td02">
                <%--<asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>--%>
                <uc3:textbox ID="dplCustomer" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Name" ValueField="Code" ServicePath="OrderProductionPlanMgr.service" ServiceMethod="GetCustomer" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlMSNo" runat="server" Text="${Quote.Tooling.MSNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtMSNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlApplyStartDate" runat="server" Text="${Quote.Tooling.ApplyStartDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox runat="server" ID="txtApplyStartDate" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlApplyEndDate" runat="server" Text="${Quote.Tooling.ApplyEndDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox runat="server" ID="txtApplyEndDate" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlApplyUser" runat="server" Text="${Quote.Tooling.ApplyUser}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtApplyUser" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlSupplier" runat="server" Text="${Quote.Tooling.Supplier}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtSupplier" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlArriveStartDate" runat="server" Text="${Quote.Tooling.ArriveStartDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox runat="server" ID="txtArriveStartDate" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlArriveEndDate" runat="server" Text="${Quote.Tooling.ArriveEndDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox runat="server" ID="txtArriveEndDate" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlTLSalesType" runat="server" Text="${Quote.Tooling.TLSalesType}:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="dplTLSalesType" runat="server">
                    <asp:ListItem>企业内部消化</asp:ListItem>
                    <asp:ListItem>客户一次性买断</asp:ListItem>
                    <asp:ListItem>其它</asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlCustomerBStatus" runat="server" Text="${Quote.Tooling.CustomerBStatus}:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="dplCustomerBStatus" runat="server">
                    <asp:ListItem>未结算</asp:ListItem>
                    <asp:ListItem>已结算</asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td01">

            </td>
            <td class="td02">

            </td>
            <td class="td01">

            </td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click" />
                <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click" />
            </td>
        </tr>
    </table>
</fieldset>
