<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectSearch.ascx.cs" Inherits="Quote_Quotes_ProjectSearch" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                 <asp:Literal ID="ltlProjectId" runat="server" Text="${Quote.Tooling.ProjectNo}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="txtProjectId" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Descr" ValueField="ID" ServicePath="OrderProductionPlanMgr.service" ServiceMethod="GetGPID" ServiceParameter="bool:true" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlVision" runat="server" Text="${Quote.ProductInfo.VersionNo}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtVision" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlCustomerName" runat="server" Text="${Quote.ProductInfo.CustomerName}:"></asp:Literal>
            </td>
            <td class="td02">
                <uc3:textbox ID="txtCustomer" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Name" ValueField="Code" ServicePath="OrderProductionPlanMgr.service" ServiceMethod="GetCustomer" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlProductNo" runat="server" Text="${Quote.ProductInfo.ProductNo}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtProductNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click" />
                <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click" />
            </td>
        </tr>
    </table>
</fieldset>