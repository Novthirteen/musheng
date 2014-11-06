<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Modules_Cost_RawIOB_Main" %>
<%--@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" --%>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="Edit.ascx" TagName="Edit" TagPrefix="uc1" %>
<%@ Register Src="New.ascx" TagName="New" TagPrefix="uc1" %>
<fieldset id="fldSearch" runat="server">
    <table class="mtable">
        <tr>
            <td class="td01">
                ${Cost.RawIOB.Item}:
            </td>
            <td class="td02">
                <asp:TextBox ID="tbItem" runat="server" />
            </td>
            <td class="td01">
                ${Cost.RawIOB.FinanceCalendar}:
            </td>
            <td class="td02">
                <asp:TextBox ID="tbFinanceCalendar" runat="server" onClick="WdatePicker({dateFmt:'yyyy-M'})"
                    CssClass="inputRequired" />
                <asp:RequiredFieldValidator ID="rfvfc" runat="server" ErrorMessage="请选择会计年月" Display="Dynamic"
                    ControlToValidate="tbFinanceCalendar" ValidationGroup="vgSave" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:CheckBox ID="cbAdjust" runat="server" Text="只显示已调整" />
            </td>
            <td />
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    CssClass="button2" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" OnClick="btnSearch_Click"
                    CssClass="button2" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset id="fldList" runat="server" visible="false">
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll" SelectCountMethod="FindCount"
            OnRowDataBound="GV_List_RowDataBound" DefaultSortExpression="LastModifyTime"
            DefaultSortDirection="Descending">
            <Columns>
                <asp:BoundField DataField="Item" HeaderText="${Cost.RawIOB.Item}" />
                <asp:BoundField DataField="Uom" HeaderText="${Cost.RawIOB.Uom}" />
                <asp:BoundField DataField="StartQty" HeaderText="${Cost.RawIOB.StartQty}" />
                <asp:BoundField DataField="StartAmount" HeaderText="${Cost.RawIOB.StartAmount}" />
                <asp:BoundField DataField="StartCost" HeaderText="${Cost.RawIOB.StartCost}" />
                <asp:BoundField DataField="InQty" HeaderText="${Cost.RawIOB.InQty}" />
                <asp:BoundField DataField="InAmount" HeaderText="${Cost.RawIOB.InAmount}" />
                <asp:BoundField DataField="InCost" HeaderText="${Cost.RawIOB.InCost}" />
                <asp:BoundField DataField="DiffAmount" HeaderText="${Cost.RawIOB.DiffAmount}" />
                <asp:BoundField DataField="DiffCost" HeaderText="${Cost.RawIOB.DiffCost}" />
                <asp:BoundField DataField="EndQty" HeaderText="${Cost.RawIOB.EndQty}" />
                <asp:BoundField DataField="EndAmount" HeaderText="${Cost.RawIOB.EndAmount}" />
                <asp:BoundField DataField="EndCost" HeaderText="${Cost.RawIOB.EndCost}" />
                <asp:BoundField DataField="FinanceCalendar" HeaderText="${Cost.RawIOB.FinanceCalendar}" />
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
<uc1:Edit ID="ucEdit" runat="server" Visible="False" />
<uc1:New ID="ucNew" runat="server" Visible="False" />
