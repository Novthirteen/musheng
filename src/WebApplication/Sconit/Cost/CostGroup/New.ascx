<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Cost_CostGroup_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<div id="divFV">
    <asp:FormView ID="FV_CostGroup" runat="server" DataSourceID="ODS_CostGroup" DefaultMode="Insert"
        Width="100%" DataKeyNames="Code">
        <InsertItemTemplate>
            <fieldset>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${Cost.CostGroup.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rtvCode" runat="server" ErrorMessage="${Cost.CostGroup.Code.Empty}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvInsert" runat="server" ControlToValidate="tbCode" ErrorMessage="${Cost.CostGroup.CodeExist}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="CheckCostGroupExists" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblDescription" runat="server" Text="${Cost.CostGroup.Description}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="${Cost.CostGroup.Description.Empty}"
                                Display="Dynamic" ControlToValidate="tbDescription" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${Common.Business.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="tbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
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
<asp:ObjectDataSource ID="ODS_CostGroup" runat="server" TypeName="com.Sconit.Web.CostGroupMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Cost.CostGroup" InsertMethod="CreateCostGroup"
    OnInserted="ODS_CostGroup_Inserted" ></asp:ObjectDataSource>

