<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Cost_Report_Bill_Main" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPartyCode" runat="server" Text="客户代号:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyCode" runat="server" DescField="Name" ValueField="Code" ServicePath="CustomerMgr.service"
                    ServiceMethod="GetAllCustomer" Width="250" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlItem" runat="server" Text="${Common.Business.ItemCode}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbItemCode" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlFinanceYear1" runat="server" Text="${Cost.FinanceCalendar.YearMonth}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbFinanceYear1" runat="server" onClick="WdatePicker({dateFmt:'yyyy-M'})"
                    OnTextChanged="tbFinanceYear_TextChange" AutoPostBack="true" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlStartTime" runat="server" Text="${Common.Business.StartTime}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                    CssClass="inputRequired" />
                <asp:RequiredFieldValidator ID="rfvStartTime" runat="server" ErrorMessage="${MasterData.WorkCalendar.WarningMessage.TimeEmpty}"
                    Display="Dynamic" ControlToValidate="tbStartTime" ValidationGroup="vgSave" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlEndTime" runat="server" Text="${Common.Business.EndTime}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                    CssClass="inputRequired" />
                <asp:RequiredFieldValidator ID="rfvEndTime" runat="server" ErrorMessage="${MasterData.WorkCalendar.WarningMessage.TimeEmpty}"
                    Display="Dynamic" ControlToValidate="tbEndTime" ValidationGroup="vgSave" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:CheckBox ID="cbGroupByParty" runat="server" Text="按客户汇总" />
            </td>
            <td>
            </td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" OnClick="btnSearch_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset runat="server" id="fld_Gv_List">
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="true" OnRowDataBound="GV_List_RowDataBound"
        CellPadding="0" AllowSorting="false">
        <Columns>
            <asp:TemplateField HeaderText="Seq">
                <ItemTemplate>
                    <%#Container.DataItemIndex + 1%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>
