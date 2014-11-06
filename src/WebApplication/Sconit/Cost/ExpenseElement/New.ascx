<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Cost_ExpenseElement_New" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="divFV">
    <asp:FormView ID="FV_ExpenseElement" runat="server" DataSourceID="ODS_ExpenseElement" DefaultMode="Insert"
        Width="100%" DataKeyNames="Code">
        <InsertItemTemplate>
            <fieldset>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${Cost.ExpenseElement.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rtvCode" runat="server" ErrorMessage="${Cost.ExpenseElement.Code.Empty}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvInsert" runat="server" ControlToValidate="tbCode" ErrorMessage="${Cost.ExpenseElement.CodeExist}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkExpenseElementExists" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblDescription" runat="server" Text="${Cost.ExpenseElement.Description}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
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
<asp:ObjectDataSource ID="ODS_ExpenseElement" runat="server" TypeName="com.Sconit.Web.ExpenseElementMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Cost.ExpenseElement" InsertMethod="CreateExpenseElement"
    OnInserting="ODS_ExpenseElement_Inserting" OnInserted="ODS_ExpenseElement_Inserted">
</asp:ObjectDataSource>
