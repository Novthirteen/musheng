<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Quote_Quotes_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="ASTreeView" Namespace="Geekees.Common.Controls" TagPrefix="ct" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblPartyFrom" runat="server" Text="${MasterData.Flow.Party.From.Customer}:" />
            </td>
            <td class="td02">
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
                <asp:Literal ID="ltlSatrtDate" runat="server" Text="${Common.Business.StartTime}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox runat="server" ID="txtStartDate" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlEndDate" runat="server" Text="${Common.Business.EndTime}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox runat="server" ID="txtEndDate" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlSataus" runat="server" Text="${Quote.Project.Status}:" Visible="false"></asp:Literal>
            </td>
            <td class="td02">
                <ct:ASDropDownTreeView ID="astvMyTree" runat="server" BasePath="~/Js/astreeview/"
                    DataTableRootNodeValue="0" EnableRoot="false" EnableNodeSelection="false" EnableCheckbox="true"
                    EnableDragDrop="false" EnableTreeLines="false" EnableNodeIcon="false" EnableCustomizedNodeIcon="false"
                    EnableDebugMode="false" EnableRequiredValidator="false" InitialDropdownText=""
                    Width="170" EnableCloseOnOutsideClick="true" EnableHalfCheckedAsChecked="true"
                    DropdownIconDown="~/Js/astreeview/images/windropdown.gif" EnableContextMenuAdd="false"
                    MaxDropdownHeight="300" Visible="false" />
            </td>
            <td class="td01"></td>
            <td class="td02"></td>
        </tr>
        <tr>
            <td class="td01"></td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click" />

            </td>
        </tr>
    </table>
</fieldset>