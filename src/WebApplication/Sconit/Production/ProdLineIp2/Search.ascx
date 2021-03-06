﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Production_ProdLineIp2_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ac1" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblProdLine" runat="server" Text="${Common.Business.ProductionLine}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbProdLine" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblProdLineFact" runat="server" Text="${Production.ProdLineIp2.ProdLineFact}" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbProdLineFact" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblOrderNo" runat="server" Text="${Common.Business.OrderNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderNo" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblFg" runat="server" Text="${Common.Business.FG}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFG" runat="server" Visible="true" DescField="Description" ImageUrlField="ImageUrl"
                    Width="280" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblItem" runat="server" Text="${Reports.ActBill.ItemCode}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" DescField="Description" ImageUrlField="ImageUrl"
                    Width="280" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblHuLotNo" runat="server" Text="${Common.Business.LotNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbHuLotNo" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblCreateDateFrom" runat="server" Text="${MasterData.Common.CreateDateFrom}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbCreateDateFrom" runat="server" />
                <ac1:CalendarExtender ID="CalendarExtender2" TargetControlID="tbCreateDateFrom" Format="yyyy-MM-dd"
                    runat="server">
                </ac1:CalendarExtender>
            </td>
            <td class="td01">
                <asp:Literal ID="lblCreateDateTo" runat="server" Text="${MasterData.Common.CreateDateTo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbCreateDateTo" runat="server" />
                <ac1:CalendarExtender ID="CalendarExtender1" TargetControlID="tbCreateDateTo" Format="yyyy-MM-dd"
                    runat="server">
                </ac1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td class="td01">
                条码:
            </td>
            <td class="td02">
                <asp:TextBox ID="tbHu" runat="server" />
            </td>
            <td />
            <td class="t02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" CssClass="button2"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                    OnClick="btnExport_Click" />
            </td>
        </tr>
    </table>
</fieldset>
