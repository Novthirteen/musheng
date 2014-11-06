<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Cost_FinanceCalendar_New" %>



<div id="divFV">
    <asp:FormView ID="FV_FinanceCalendar" runat="server" DataSourceID="ODS_FinanceCalendar"
        DefaultMode="Insert" Width="100%" DataKeyNames="Id">
        <InsertItemTemplate>
            <fieldset>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblYear" runat="server" Text="${Cost.FinanceCalendar.Year}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbYear" runat="server" Text='<%# Bind("FinanceYear") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvYear" runat="server" ErrorMessage="${Cost.FinanceCalendar.Year.Empty}"
                                Display="Dynamic" ControlToValidate="tbYear" ValidationGroup="vgSave" />
                             <asp:RangeValidator ID="rvYear" ControlToValidate="tbYear" runat="server"
                                Display="Dynamic" ErrorMessage="${Common.Validator.Valid.Number}" MaximumValue="3000"
                                MinimumValue="2011" Type="Integer" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblMonth" runat="server" Text="${Cost.FinanceCalendar.Month}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbMonth" runat="server" Text='<%# Bind("FinanceMonth") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ErrorMessage="${Cost.FinanceCalendar.Month.Empty}"
                                Display="Dynamic" ControlToValidate="tbMonth" ValidationGroup="vgSave" />
                             <asp:RangeValidator ID="rvMonth" ControlToValidate="tbMonth" runat="server"
                                Display="Dynamic" ErrorMessage="${Common.Validator.Valid.Number}" MaximumValue="12"
                                MinimumValue="1" Type="Integer" ValidationGroup="vgSave" />
                             <asp:CustomValidator ID="cvFCCheck" runat="server" ControlToValidate="tbMonth" ErrorMessage="${Cost.FinanceCalendar.Exists}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkFinanceCalendarExists" />
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
                    <tr>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                            <div class="buttons">
                                <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="${Common.Button.Save}"
                                    CssClass="apply" ValidationGroup="vgSave" />
                                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                                    CssClass="back" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </InsertItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_FinanceCalendar" runat="server" TypeName="com.Sconit.Web.FinanceCalendarMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.FinanceCalendar" InsertMethod="CreateFinanceCalendar" 
    OnInserting="ODS_FinanceCalendar_Inserting" OnInserted="ODS_FinanceCalendar_Inserted">
</asp:ObjectDataSource>
