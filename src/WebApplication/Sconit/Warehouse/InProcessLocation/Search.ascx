﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Order_GoodsReceipt_AsnReceipt_Search" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxControlToolkit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>



<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblIpNo" runat="server" Text="${InProcessLocation.IpNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbIpNo" runat="server" Visible="true" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblStatus" runat="server" Text="${InProcessLocation.Status}:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="ddlStatus" runat="server" DataTextField="Description" DataValueField="Value" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblPartyFrom" runat="server" Text="${MasterData.Order.OrderHead.PartyFrom.Supplier}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyFrom" runat="server" Visible="true" Width="250" DescField="Name"
                    ValueField="Code" ServicePath="PartyMgr.service" ServiceMethod="GetFromParty" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblPartyTo" runat="server" Text="${MasterData.Order.OrderHead.PartyTo.Customer}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyTo" runat="server" Visible="true" Width="250" DescField="Name"
                    ValueField="Code" MustMatch="true" ServicePath="PartyMgr.service" ServiceMethod="GetToParty" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlStartDate" runat="server" Text="${MasterData.PlannedBill.CreateDateFrom}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlEndDate" runat="server" Text="${MasterData.PlannedBill.CreateDateTo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
        </tr>
        <tr runat="server" id="trDetails">
            <td class="td01">
                <asp:Literal ID="ltlOrderNo" runat="server" Text="${InProcessLocation.InProcessLocationDetail.OrderNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderNo" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlItem" runat="server" Text="${InProcessLocation.InProcessLocationDetail.Item}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" DescField="Description" ImageUrlField="ImageUrl"
                    Width="280" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlListFormat" runat="server" Text="${Common.ListFormat}:" />
            </td>
            <td class="td02">
                <asp:RadioButtonList ID="rblListFormat" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="${Common.ListFormat.Group}" Value="Group" Selected="True" />
                    <asp:ListItem Text="${Common.ListFormat.Detail}" Value="Detail" />
                </asp:RadioButtonList>
            </td>
            <td></td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    CssClass="button2" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                    OnClick="btnSearch_Click" />
            </td>
        </tr>
    </table>
</fieldset>
