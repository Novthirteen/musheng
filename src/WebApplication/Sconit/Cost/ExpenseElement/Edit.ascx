<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Cost_ExpenseElement_Edit" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>

<div id="divFV" runat="server">
    <asp:FormView ID="FV_ExpenseElement" runat="server" DataSourceID="ODS_ExpenseElement" DefaultMode="Edit"
        Width="100%" DataKeyNames="Code" OnDataBound="FV_ExpenseElement_DataBound">
        <EditItemTemplate>
            <fieldset>
                <table class="mtable">
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="lblCode" runat="server" Text="${Cost.ExpenseElement.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbCode" runat="server" Text='<%# Bind("Code") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblDescription" runat="server" Text="${Cost.ExpenseElement.Description}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                        </td>
                    </tr>
                    
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
<asp:ObjectDataSource ID="ODS_ExpenseElement" runat="server" TypeName="com.Sconit.Web.ExpenseElementMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Cost.ExpenseElement" UpdateMethod="UpdateExpenseElement"  OnUpdating="ODS_ExpenseElement_Updating"
    OnUpdated="ODS_ExpenseElement_Updated" SelectMethod="LoadExpenseElement" DeleteMethod="DeleteExpenseElement"
    OnDeleted="ODS_ExpenseElement_Deleted">
    <SelectParameters>
        <asp:Parameter Name="code" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
