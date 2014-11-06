<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Modules_Cost_RawIOB_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<asp:FormView ID="FV_RawIOB" runat="server" DataSourceID="ODS_RawIOB" DefaultMode="Insert"
    DataKeyNames="Id">
    <InsertItemTemplate>
        <fieldset>
            <legend>${Common.New}</legend>
            <table class="mtable">
                <tr>
                    <td class="td01">
                        ${Cost.RawIOB.Item}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbItem" runat="server" Text='<%# Bind("Item") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvItem" runat="server" ControlToValidate="tbItem"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                    <td class="td01">
                        ${Cost.RawIOB.Uom}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbUom" runat="server" Text='<%# Bind("Uom") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvUom" runat="server" ControlToValidate="tbUom"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        ${Cost.RawIOB.StartQty}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbStartQty" runat="server" Text='<%# Bind("StartQty") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvStartQty" runat="server" ControlToValidate="tbStartQty"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                    <td class="td01">
                        ${Cost.RawIOB.StartAmount}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbStartAmount" runat="server" Text='<%# Bind("StartAmount") %>'
                            CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvStartAmount" runat="server" ControlToValidate="tbStartAmount"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        ${Cost.RawIOB.StartCost}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbStartCost" runat="server" Text='<%# Bind("StartCost") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvStartCost" runat="server" ControlToValidate="tbStartCost"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                    <td class="td01">
                        ${Cost.RawIOB.InQty}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbInQty" runat="server" Text='<%# Bind("InQty") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvInQty" runat="server" ControlToValidate="tbInQty"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        ${Cost.RawIOB.InAmount}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbInAmount" runat="server" Text='<%# Bind("InAmount") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvInAmount" runat="server" ControlToValidate="tbInAmount"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                    <td class="td01">
                        ${Cost.RawIOB.InCost}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbInCost" runat="server" Text='<%# Bind("InCost") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvInCost" runat="server" ControlToValidate="tbInCost"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        ${Cost.RawIOB.DiffAmount}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbDiffAmount" runat="server" Text='<%# Bind("DiffAmount") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvDiffAmount" runat="server" ControlToValidate="tbDiffAmount"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                    <td class="td01">
                        ${Cost.RawIOB.DiffCost}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbDiffCost" runat="server" Text='<%# Bind("DiffCost") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvDiffCost" runat="server" ControlToValidate="tbDiffCost"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        ${Cost.RawIOB.EndQty}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbEndQty" runat="server" Text='<%# Bind("EndQty") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvEndQty" runat="server" ControlToValidate="tbEndQty"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                    <td class="td01">
                        ${Cost.RawIOB.EndAmount}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbEndAmount" runat="server" Text='<%# Bind("EndAmount") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvEndAmount" runat="server" ControlToValidate="tbEndAmount"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        ${Cost.RawIOB.EndCost}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbEndCost" runat="server" Text='<%# Bind("EndCost") %>' CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvEndCost" runat="server" ControlToValidate="tbEndCost"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                    <td class="td01">
                        ${Cost.RawIOB.FinanceCalendar}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbFinanceCalendar" runat="server" Text='<%# Bind("FinanceCalendar") %>'
                            CssClass="inputRequired" />
                        <asp:RequiredFieldValidator ID="rfvFinanceCalendar" runat="server" ControlToValidate="tbFinanceCalendar"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <div class="tablefooter">
            <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="${Common.Button.Save}"
                CssClass="button2" ValidationGroup="vgSave" />
            <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                CssClass="button2" />
        </div>
    </InsertItemTemplate>
</asp:FormView>
<asp:ObjectDataSource ID="ODS_RawIOB" runat="server" TypeName="com.Sconit.Web.RawIOBMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Cost.RawIOB" InsertMethod="CreateRawIOB"
    OnInserted="ODS_RawIOB_Inserted" OnInserting="ODS_RawIOB_Inserting" SelectMethod="LoadRawIOB">
    <SelectParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </SelectParameters>
    <InsertParameters>
    </InsertParameters>
</asp:ObjectDataSource>
