<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="MasterData_Facility_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Flow.Facility.Code}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCode" runat="server" Visible="true" Width="250" MustMatch="true" DescField="Code"
                                ValueField="Code" ServicePath="ProductLineFacilityMgr.service" ServiceMethod="GetAllProductLineFacility"/>
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click" CssClass="button2" />
                <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click" CssClass="button2" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" CssClass="button2" />
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
</fieldset>
