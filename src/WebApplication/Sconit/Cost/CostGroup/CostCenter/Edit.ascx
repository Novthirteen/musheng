<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Cost_CostCenter_Edit" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_CostCenter" runat="server" DataSourceID="ODS_CostCenter" DefaultMode="Edit"
        Width="100%" DataKeyNames="Code">
        <EditItemTemplate>
            <fieldset>
                <table class="mtable">
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="lblCode" runat="server" Text="${Cost.CostCenter.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbCode" runat="server" Text='<%# Bind("Code") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblDescription" runat="server" Text="${Cost.CostCenter.Description}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${Common.Business.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
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
<asp:ObjectDataSource ID="ODS_CostCenter" runat="server" TypeName="com.Sconit.Web.CostCenterMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Cost.CostCenter" UpdateMethod="UpdateCostCenter"  OnUpdating="ODS_CostCenter_Updating"
    OnUpdated="ODS_CostCenter_Updated" SelectMethod="LoadCostCenter" DeleteMethod="DeleteCostCenter"
    OnDeleted="ODS_CostCenter_Deleted">
    <SelectParameters>
        <asp:Parameter Name="code" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
