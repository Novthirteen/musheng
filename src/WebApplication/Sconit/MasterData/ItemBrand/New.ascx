<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_ItemBrand_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_ItemBrand" runat="server" DataSourceID="ODS_ItemBrand" DefaultMode="Insert"
        Width="100%" DataKeyNames="Code" OnDataBinding="FV_ItemBrand_OnDataBinding">
        <InsertItemTemplate>
            <fieldset>
                <legend>${MasterData.ItemBrand.NewItemBrand}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlCode" runat="server" Text="${MasterData.ItemBrand.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tdCode" runat="server" Text='<%# Bind("Code") %>'  CssClass="inputRequired"/>
                            <asp:RequiredFieldValidator ID="requiredFieldValidator1" runat="server" ErrorMessage="${MasterData.ItemBrand.Code}${MasterData.ItemBrand.Empty}"
                                Display="Dynamic" ControlToValidate="tdCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvInsert" runat="server" ControlToValidate="tdCode" ErrorMessage="${MasterData.ItemBrand.CodeExist1}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkItemExists" />
                        </td>
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">                            
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlDescription" runat="server" Text="${MasterData.ItemBrand.Description}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tdDescription" runat="server" Text='<%# Bind("Description") %>'   CssClass="inputRequired"/>
                            <asp:RequiredFieldValidator ID="requiredFieldValidatorItem" runat="server" ErrorMessage="${MasterData.ItemBrand.Description}${MasterData.ItemBrand.Empty}"
                                Display="Dynamic" ControlToValidate="tdDescription" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlAbbreviation" runat="server" Text="${MasterData.ItemBrand.Abbreviation}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tdAbbreviation" runat="server" Text='<%# Bind("Abbreviation") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlManufactureParty" runat="server" Text="${MasterData.ItemBrand.ManufactureParty}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbManufactureParty" runat="server" Text='<%# Bind("ManufactureParty") %>'  />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlOrigin" runat="server" Text="${MasterData.ItemBrand.Origin}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbOrigin" runat="server" Text='<%# Bind("Origin") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlManufactureAddress" runat="server" Text="${MasterData.ItemBrand.ManufactureAddress}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbManufactureAddress" Width="250" runat="server" Text='<%# Bind("ManufactureAddress") %>'  />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlIsActive" runat="server" Text="${MasterData.ItemBrand.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="tbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
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
<asp:ObjectDataSource ID="ODS_ItemBrand" runat="server" TypeName="com.Sconit.Web.ItemBrandMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemBrand" InsertMethod="CreateItemBrand"
    OnInserted="ODS_ItemBrand_Inserted" OnInserting="ODS_ItemBrand_Inserting"></asp:ObjectDataSource>