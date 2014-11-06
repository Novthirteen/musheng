﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="PickList_Batch_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="List.ascx" TagName="List" TagPrefix="uc2" %>



<fieldset>
    <table class="mtable">
        <tr>
            <td class="ttd01">
                <asp:Literal ID="lblPickListNo" runat="server" Text="${MasterData.Distribution.PickList}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbPickListNo" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlStatus" runat="server" Text="${Common.CodeMaster.Status}:" />
            </td>
            <td class="td02">
                <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="${Common.CodeMaster.Status.In-Process}" Value="In-Process"  />
                    <asp:ListItem Text="${Common.CodeMaster.Status.Submit}" Value="Submit" Selected="True" />
                </asp:RadioButtonList>
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
            <td colspan="3" />
            <td class="ttd02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    CssClass="button2" />
                <asp:Button ID="btnClose" runat="server" Text="${Common.Button.Close}" OnClick="btnClose_Click"
                    CssClass="button2" />
                <asp:Button ID="btnStart" runat="server" Text="${MasterData.PickList.Start}" OnClick="btnStart_Click"
                    CssClass="button2" />
                <asp:Button ID="btnCancel" runat="server" Text="${Common.Button.Cancel}" OnClick="btnCancel_Click"
                    CssClass="button2" />
            </td>
        </tr>
    </table>
</fieldset>
<uc2:List ID="ucList" runat="server" Visible="false" />
