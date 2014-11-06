﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Import.ascx.cs" Inherits="Order_OrderHead_OrderImport_Import" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ac1" %>
<%@ Register Src="~/MRP/PlanSchedule/DateSelect.ascx" TagName="DateSelect" TagPrefix="uc2" %>



<fieldset>
    <table class="mtable">
        <uc2:DateSelect ID="ucDateSelect" runat="server" />
        <tr>
            <td class="ttd01">
                <asp:Literal ID="lblRegion" runat="server" Text="${Common.Business.Region}:" />
            </td>
            <td class="ttd02">
                <uc3:textbox ID="tbRegion" runat="server" DescField="Name" ValueField="Code" Width="300"
                    ServicePath="RegionMgr.service" ServiceMethod="GetRegion" MustMatch="true" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
        </tr>
        <tr> 
            <td class="td01">
                <asp:Literal ID="ltlSelect" runat="server" Text="${Common.FileUpload.PleaseSelect}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:FileUpload ID="fileUpload" ContentEditable="false" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlTemplate" runat="server" Text="${Common.Business.Template}:" />
            </td>
            <td class="td02">
                <asp:HyperLink ID="hlTemplate" runat="server" Text="${Common.Business.ClickToDownload}"></asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td colspan="3" />
            <td class="ttd02">
                <div class="buttons">
                    <asp:Button ID="btnImport" runat="server" Text="${Common.Button.Import}" OnClick="btnImport_Click"
                        CssClass="apply" />
                    <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                        CssClass="back" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
