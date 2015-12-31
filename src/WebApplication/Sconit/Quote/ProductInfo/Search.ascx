<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Quote_ProductInfo_Search" %>

<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblPartyFrom" runat="server" Text="${MasterData.Flow.Party.From.Customer}:" />
            </td>
            <td class="td02">
                <%--<asp:DropDownList ID="ddlCustomerName" runat="server"></asp:DropDownList>--%>
                 <uc3:textbox ID="tbPartyFrom" runat="server" Visible="true" DescField="Name" ValueField="Code" Width="250"
                    ServicePath="PartyMgr.service" ServiceMethod="GetFromParty" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlProductName" runat="server" Text="${Quote.ProductInfo.ProductName}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtProductName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlProductNo" runat="server" Text="${Quote.ProductInfo.ProductNo}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtProductNo" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlProjectNo" runat="server" Text="${Quote.Tooling.ProjectNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtProjectNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01"></td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click" />
                <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click" />
            </td>
        </tr>
    </table>
</fieldset>