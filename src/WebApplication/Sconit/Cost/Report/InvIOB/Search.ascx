﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Cost_Report_InvIOB_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ac1" %>
<%@ Register Assembly="ASTreeView" Namespace="Geekees.Common.Controls" TagPrefix="ct" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblLocation" runat="server" Text="${Common.Business.Location}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbLocation" runat="server" Visible="true" DescField="Name" Width="280"
                    ValueField="Code" ServicePath="LocationMgr.service" ServiceMethod="GetLocationByUserCode" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblItem" runat="server" Text="${Common.Business.ItemCode}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" DescField="Description" ImageUrlField="ImageUrl"
                    Width="280" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlFinanceYear1" runat="server" Text="${Cost.FinanceCalendar.YearMonth}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbFinanceYear1" runat="server" onClick="WdatePicker({dateFmt:'yyyy-M'})"
                    OnTextChanged="tbFinanceYear_TextChange" AutoPostBack="true" CssClass="inputRequired" />
                <asp:RequiredFieldValidator ID="rfvfc" runat="server" ErrorMessage="请选择会计年月" Display="Dynamic"
                    ControlToValidate="tbFinanceYear1" ValidationGroup="vgSave" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblStartDate" runat="server" Text="${Common.Business.StartDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblEndDate" runat="server" Text="${Common.Business.EndDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <%-- <asp:Literal ID="lblDesc" runat="server" Text="${Common.Business.ItemDescription}:" />--%>
            </td>
            <td class="td02">
                <%-- <asp:TextBox ID="tbDesc" runat="server" />--%>
            </td>
            <td class="td01" />
            <td class="t02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" CssClass="button2"
                    OnClick="btnSearch_Click" ValidationGroup="vgSave" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                    OnClick="btnExport_Click" ValidationGroup="vgSave" />
            </td>
        </tr>
    </table>
</fieldset>
