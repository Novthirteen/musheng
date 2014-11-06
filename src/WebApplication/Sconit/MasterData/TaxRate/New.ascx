<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_TaxRate_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_TaxRate" runat="server" DataSourceID="ODS_TaxRate" DefaultMode="Insert"
        Width="100%" DataKeyNames="Code" OnDataBinding="FV_TaxRate_OnDataBinding">
        <InsertItemTemplate>
            <fieldset>
                <legend>${MasterData.TaxRate.NewTaxRate}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${Common.Business.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rtvCode" runat="server" ErrorMessage="${MasterData.TaxRate.Code.Empty}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvInsert" runat="server" ControlToValidate="tbCode" ErrorMessage="${Common.Code.Exist}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkTaxRate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlDesc" runat="server" Text="${Common.Business.Description}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc" runat="server" Text='<%# Bind("Description") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="${MasterData.TaxRate.Description.Empty}"
                                Display="Dynamic" ControlToValidate="tbDesc" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlTaxRate" runat="server" Text="${Common.Business.TaxRate}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbTaxRate" runat="server" Text='<%# Bind("TaxRate") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvTaxRate" runat="server" ErrorMessage="${MasterData.TaxRate.Rate.Empty}"
                                Display="Dynamic" ControlToValidate="tbTaxRate" ValidationGroup="vgSave" />
                            <asp:RangeValidator ID="rvTaxRate" runat="server" ErrorMessage="${MasterData.TaxRate.IsNotNumeric}"
                                Display="Dynamic" ControlToValidate="tbTaxRate" MinimumValue="0" MaximumValue="999999" ValidationGroup="vgSave" />
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
<asp:ObjectDataSource ID="ODS_TaxRate" runat="server" TypeName="com.Sconit.Web.TaxRateMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.TaxRate" InsertMethod="CreateTaxRate"
    OnInserted="ODS_TaxRate_Inserted" OnInserting="ODS_TaxRate_Inserting"></asp:ObjectDataSource>
