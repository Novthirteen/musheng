<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="MasterData_ItemCategory_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc2" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_ItemCategory" runat="server" DataSourceID="ODS_ItemCategory" DefaultMode="Edit"
        Width="100%" DataKeyNames="Code">
        <EditItemTemplate>
            <fieldset>
                <legend>${MasterData.ItemCategory.UpdateItemCategory}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.ItemCategory.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' ReadOnly="true" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlName" runat="server" Text="${Common.Business.Description}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Description") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="${MasterData.ItemCategory.Description.Empty}"
                                Display="Dynamic" ControlToValidate="tbName" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlParent" runat="server" Text="${MasterData.ItemCategory.ParentCategory}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbParent" runat="server" Visible="true" DescField="Description" 
                                MustMatch="true" ValueField="Code" ServicePath="ItemCategoryMgr.service" ServiceMethod="GetCacheAllItemCategory"
                                Width="250" />                       
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
<asp:ObjectDataSource ID="ODS_ItemCategory" runat="server" TypeName="com.Sconit.Web.ItemCategoryMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemCategory" UpdateMethod="UpdateItemCategory"
    OnUpdated="ODS_ItemCategory_Updated" SelectMethod="LoadItemCategory" OnUpdating="ODS_ItemCategory_Updating"
    DeleteMethod="DeleteItemCategory" OnDeleted="ODS_ItemCategory_Deleted">
    <SelectParameters>
        <asp:Parameter Name="code" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>