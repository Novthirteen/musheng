<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="MasterData_TaxRate_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc2" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_TaxRate" runat="server" DataSourceID="ODS_TaxRate" DefaultMode="Edit"
        Width="100%" DataKeyNames="Code" OnDataBound="FV_TaxRate_DataBound">
        <EditItemTemplate>
            <fieldset>
                <legend>${MasterData.TaxRate.UpdateTaxRate}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${Common.Business.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' ReadOnly="true" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlName" runat="server" Text="${Common.Business.Description}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Description") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="${MasterData.TaxRate.Description.Empty}"
                                Display="Dynamic" ControlToValidate="tbName" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlTaxRate" runat="server" Text="${Common.Business.TaxRate}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbTaxRate" runat="server" Text='<%# Bind("TaxRate","{0:0.########}") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvTaxRate" runat="server" ErrorMessage="${MasterData.TaxRate.Rate.Empty}"
                                Display="Dynamic" ControlToValidate="tbName" ValidationGroup="vgSave" />
                            <asp:RangeValidator ID="rvTaxRate" runat="server" ErrorMessage="${MasterData.TaxRate.IsNotNumeric}"
                                Display="Dynamic" MaximumValue="999999" MinimumValue="0" ControlToValidate="tbTaxRate" ValidationGroup="vgSave" />
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
                                <asp:Button ID="btnSave" runat="server" CommandName="Update" Text="${Common.Button.Save}"
                                    CssClass="apply" ValidationGroup="vgSave" />
                                <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="${Common.Button.Delete}"
                                    CssClass="delete" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />
                                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                                    CssClass="back" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </EditItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_TaxRate" runat="server" TypeName="com.Sconit.Web.TaxRateMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.TaxRate" UpdateMethod="UpdateTaxRate"
    OnUpdated="ODS_TaxRate_Updated" SelectMethod="LoadTaxRate" OnUpdating="ODS_TaxRate_Updating"
    DeleteMethod="DeleteTaxRate" OnDeleted="ODS_TaxRate_Deleted">
    <SelectParameters>
        <asp:Parameter Name="code" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
