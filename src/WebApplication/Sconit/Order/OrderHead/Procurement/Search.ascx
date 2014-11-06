﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Order_OrderHead_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Assembly="ASTreeView" Namespace="Geekees.Common.Controls" TagPrefix="ct" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblOrderNo" runat="server" Text="${Warehouse.LocTrans.OrderNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderNo" runat="server" Visible="true" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblPriority" runat="server" Text="${MasterData.Order.OrderHead.Priority}:" />
            </td>
            <td class="td02">
                <cc1:CodeMstrDropDownList ID="ddlPriority" Code="OrderPriority" runat="server" IncludeBlankOption="true"
                    DefaultSelectedValue="" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblFlow" runat="server" Text="${MasterData.Order.OrderHead.Flow}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServicePath="FlowMgr.service" MustMatch="true" Width="250" ServiceMethod="GetFlowList" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblCreateUser" runat="server" Text="${MasterData.Order.OrderHead.CreateUser}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbCreateUser" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblPartyFrom" runat="server" Text="${MasterData.Order.OrderHead.PartyFrom}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyFrom" runat="server" Visible="true" Width="250" DescField="Name"
                    ValueField="Code" ServicePath="PartyMgr.service" ServiceMethod="GetOrderFromParty" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblPartyTo" runat="server" Text="${MasterData.Order.OrderHead.PartyTo.Region}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyTo" runat="server" Visible="true" Width="250" DescField="Name"
                    ValueField="Code" MustMatch="true" ServicePath="PartyMgr.service" ServiceMethod="GetOrderToParty" />
            </td>
        </tr>
        <tr runat="server" visible="false" id="trLoc">
            <td class="td01">
                <asp:Literal ID="lblLocFrom" runat="server" Text="${MasterData.Order.OrderHead.LocFrom}:"
                    Visible="false" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbLocFrom" runat="server" Width="250" DescField="Name" ValueField="Code"
                    ServicePath="LocationMgr.service" ServiceMethod="GetLocation" ServiceParameter="string:#tbPartyFrom"
                    Visible="false" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblLocTo" runat="server" Text="${MasterData.Order.OrderHead.LocTo}:"
                    Visible="false" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbLocTo" runat="server" Width="250" DescField="Name" ValueField="Code"
                    ServicePath="LocationMgr.service" ServiceMethod="GetLocation" ServiceParameter="string:#tbPartyTo"
                    Visible="false" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblStatus" runat="server" Text="${MasterData.Order.OrderHead.Status}:" />
            </td>
            <td>
                <ct:ASDropDownTreeView ID="astvMyTree" runat="server" BasePath="~/Js/astreeview/"
                    DataTableRootNodeValue="0" EnableRoot="false" EnableNodeSelection="false" EnableCheckbox="true"
                    EnableDragDrop="false" EnableTreeLines="false" EnableNodeIcon="false" EnableCustomizedNodeIcon="false"
                    EnableDebugMode="false" EnableRequiredValidator="false" InitialDropdownText=""
                    Width="170" EnableCloseOnOutsideClick="true" EnableHalfCheckedAsChecked="true"
                    DropdownIconDown="~/Js/astreeview/images/windropdown.gif" EnableContextMenuAdd="false"
                    MaxDropdownHeight="200" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlSubType" runat="server" Text="${MasterData.Order.OrderHead.SubType}:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="ddlSubType" runat="server" DataTextField="Description" DataValueField="Value" />
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
        <tr>
            <td class="td01">
                <asp:Literal ID="lblItem" runat="server" Text="${Common.Business.Item}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Description" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <span id="spanNamedQuery1" class="hidden">${MasterData.NamedQuery.QueryName}:</span>
                <asp:Literal ID="ltlListFormat" runat="server" Text="${Common.ListFormat}:" />
            </td>
            <td>
                <div id="divNamedQuery" runat="server" visible="false">
                    <span id="spanNamedQuery2"><a style="text-decoration: underline; cursor: pointer;"
                        onclick="ShowNamedQuery()">
                        <asp:Literal Text="${Common.Button.NamedQuery}" runat="server" ID="ltlNamedQueryLink" />
                    </a></span><span id="spanNamedQuery3" class="hidden">
                        <asp:TextBox ID="tbNamedQuery" runat="server" Width="100px" />
                        <asp:Button ID="btnNamedQuery" runat="server" Text="${Common.Button.Save}" OnClick="btnNamedQuery_Click"
                            CssClass="button2" />
                        <input type="button" class="button2" id="btnNamedQueryCancel" onclick='HideNamedQuery()'
                            value="${Common.Button.Cancel}" /></span>
                </div>
                <asp:RadioButtonList ID="rblListFormat" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="${Common.ListFormat.Group}" Value="Group" Selected="True" />
                    <asp:ListItem Text="${Common.ListFormat.Detail}" Value="Detail" />
                </asp:RadioButtonList>
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    CssClass="button2" />
                <cc1:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                    CssClass="button2" FunctionId="EditOrder" />
                <cc1:Button ID="btnNew2" runat="server" Text="${Common.Button.New}2" OnClick="btnNew_Click"
                    CssClass="button2" FunctionId="EditOrder"  />
                <cc1:Button ID="btnImport" runat="server" Text="${Common.Button.Import}" OnClick="btnImport_Click"
                    CssClass="button2" FunctionId="EditOrder" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                    OnClick="btnSearch_Click" />
            </td>
        </tr>
    </table>
</fieldset>
