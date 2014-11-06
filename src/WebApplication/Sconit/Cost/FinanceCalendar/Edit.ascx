<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Cost_FinanceCalendar_Edit" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_FinanceCalendar" runat="server" DataSourceID="ODS_FinanceCalendar"
        DefaultMode="Edit" Width="100%" DataKeyNames="Id" OnDataBound="FV_FinanceCalendar_DataBound">
        <EditItemTemplate>
            <fieldset>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblYear" runat="server" Text="${Cost.FinanceCalendar.Year}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="tbYear" runat="server" Text='<%# Bind("FinanceYear") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblMonth" runat="server" Text="${Cost.FinanceCalendar.Month}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="lbMonth" runat="server" Text='<%# Bind("FinanceMonth") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblStartDate" runat="server" Text="${Common.Business.StartDate}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                                Text='<%# Bind("StartDate") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="${Cost.FinanceCalendar.StartDate.Empty}"
                                Display="Dynamic" ControlToValidate="tbStartDate" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblEndDate" runat="server" Text="${Common.Business.EndDate}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                                Text='<%# Bind("EndDate") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="${Cost.FinanceCalendar.EndDate.Empty}"
                                Display="Dynamic" ControlToValidate="tbEndDate" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblIsClose" runat="server" Text="${Cost.FinanceCalendar.IsClosed}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbIsClose" runat="server" Checked='<%#Bind("IsClosed") %>' />
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                    </tr>
                </table>
                <div class="tablefooter">
                    <asp:Button ID="btnSave" runat="server" CommandName="Update" Text="${Common.Button.Save}"
                        ValidationGroup="vgSave" />
                    <asp:Button ID="btnCbom" runat="server" Text="展Bom" OnClick="btnCBom_Click" />
                    <asp:Button ID="btnRm" runat="server" Text="计算所有成本" OnClick="btnRm_Click" OnClientClick="return confirm('计算所有成本将需要重新调整原材料成本，是否继续？')" />
                   
                    <asp:Button ID="btnClose" runat="server" Text="关闭" OnClick="btnClose_Click" OnClientClick="return confirm('确定要关闭吗？')" />
                    <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
                </div>
            </fieldset>
        </EditItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_FinanceCalendar" runat="server" TypeName="com.Sconit.Web.FinanceCalendarMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.FinanceCalendar" UpdateMethod="UpdateFinanceCalendar"
    OnUpdating="ODS_FinanceCalendar_Updating" OnUpdated="ODS_FinanceCalendar_Updated"
    SelectMethod="LoadFinanceCalendar" DeleteMethod="DeleteFinanceCalendar" OnDeleted="ODS_FinanceCalendar_Deleted">
    <SelectParameters>
        <asp:Parameter Name="id" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
