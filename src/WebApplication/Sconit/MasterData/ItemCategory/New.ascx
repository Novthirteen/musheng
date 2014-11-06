<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_ItemCategory_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_ItemCategory" runat="server" DataSourceID="ODS_ItemCategory" DefaultMode="Insert"
        Width="100%" DataKeyNames="Code" OnDataBinding="FV_ItemCategory_OnDataBinding">
        <InsertItemTemplate>
            <fieldset>
                <legend>${MasterData.ItemCategory.NewItemCategory}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.ItemCategory.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rtvCode" runat="server" ErrorMessage="${MasterData.ItemCategory.Code.Empty}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvInsert" runat="server" ControlToValidate="tbCode" ErrorMessage="${Common.Code.Exist}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkItemCategory" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlDesc" runat="server" Text="${Common.Business.Description}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc" runat="server" Text='<%# Bind("Description") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="${MasterData.ItemCategory.Description.Empty}"
                                Display="Dynamic" ControlToValidate="tbDesc" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlParent" runat="server" Text="${MasterData.ItemCategory.ParentCategory}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbParent" runat="server" Visible="true" DescField="Description" 
                                MustMatch="true" ValueField="Code" ServicePath="ItemCategoryMgo'rr.service" ServiceMethod="GetCacheAllItemCategory"
                                Width="250" />
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
<asp:ObjectDataSource ID="ODS_ItemCategory" runat="server" TypeName="com.Sconit.Web.ItemCategoryMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemCategory" InsertMethod="CreateItemCategory"
    OnInserted="ODS_ItemCategory_Inserted" OnInserting="ODS_ItemCategory_Inserting"></asp:ObjectDataSource>