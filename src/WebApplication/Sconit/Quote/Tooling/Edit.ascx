<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Quote_Tooling_Edit" %>

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
                <asp:Literal ID="ltlProductName" runat="server" Text="${Quote.Tooling.ProductName}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtProductName" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlTLName" runat="server" Text="${Quote.Tooling.TLName}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtTLName" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlTLSpec" runat="server" Text="${Quote.Tooling.TLSpec}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtTLSpec" runat="server" Enabled="false"></asp:TextBox>
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
                <asp:TextBox ID="txtMSNo" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlArriveDate" runat="server" Text="${Quote.Tooling.ArriveDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox runat="server" ID="txtArriveDate" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" Enabled="false"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlApplyDate" runat="server" Text="${Quote.Tooling.ApplyDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox runat="server" ID="txtApplyDate" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlApplyUser" runat="server" Text="${Quote.Tooling.ApplyUser}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtApplyUser" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlSupplier" runat="server" Text="${Quote.Tooling.Supplier}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtSupplier" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPCBNo" runat="server" Text="${Quote.Tooling.PCBNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPCBNo" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlSupplierInNo" runat="server" Text="${Quote.Tooling.SupplierInNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtSupplierInNo" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPrice" runat="server" Text="${Quote.Tooling.Price}:" />
            </td>
            <td class="td02">
                <asp:TextBox runat="server" ID="txtPrice" Enabled="false" ></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlNumber" runat="server" Text="${Quote.Tooling.Number}:" />
            </td>
            <td class="td02">
                <asp:TextBox runat="server" ID="txtNumber" Enabled="false" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlTLSalesType" runat="server" Text="${Quote.Tooling.TLSalesType}:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="dplTLSalesType" runat="server" Enabled="false">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem>企业内部消化</asp:ListItem>
                    <asp:ListItem>客户一次性买断</asp:ListItem>
                    <asp:ListItem>其它</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlCustomerBStatus" runat="server" Text="${Quote.Tooling.CustomerBStatus}:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="dplCustomerBStatus" runat="server" Enabled="false">
                    <asp:ListItem>未结算</asp:ListItem>
                    <asp:ListItem>已结算</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlCustomerBillDate" runat="server" Text="${Quote.Tooling.CustomerBillDate}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtCustomerBillDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlCustomerBillNo" runat="server" Text="${Quote.Tooling.CustomerBillNo}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtCustomerBillNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlTLNo" runat="server" Text="${Quote.Tooling.TLNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox runat="server" ID="txtTLNo" Enabled="false" ></asp:TextBox>
            </td>
            <td class="td01">

            </td>
            <td class="td02">

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
                <asp:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
            </td>
        </tr>
    </table>
</fieldset>
