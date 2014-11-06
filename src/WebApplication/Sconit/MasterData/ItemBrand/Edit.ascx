<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="MasterData_ItemBrand_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc2" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_ItemBrand" runat="server" DataSourceID="ODS_ItemBrand" DefaultMode="Edit"
        Width="100%" DataKeyNames="Code" OnDataBound="FV_ItemBrand_DataBound">
        <EditItemTemplate>
            <fieldset>
                <legend>${MasterData.ItemBrand.UpdateItemDisCon}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlCode" runat="server" Text="${MasterData.ItemBrand.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' ReadOnly="true"/>
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
                            <asp:Literal ID="ltlAbbreviation" runat="server" Text="${MasterData.ItemBrand.Abbreviation}" />
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
                            <asp:TextBox ID="tbManufactureAddress" runat="server" Text='<%# Bind("ManufactureAddress") %>'  />
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
<asp:ObjectDataSource ID="ODS_ItemBrand" runat="server" TypeName="com.Sconit.Web.ItemBrandMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemBrand" UpdateMethod="UpdateItemBrand"
    OnUpdated="ODS_ItemBrand_Updated" SelectMethod="LoadItemBrand" OnUpdating="ODS_ItemBrand_Updating"
    DeleteMethod="DeleteItemBrand" OnDeleted="ODS_ItemBrand_Deleted">
    <SelectParameters>
        <asp:Parameter Name="code" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>