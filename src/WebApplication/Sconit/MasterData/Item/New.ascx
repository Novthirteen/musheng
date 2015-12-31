<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_Item_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/FileUpload.ascx" TagName="FileUpload" TagPrefix="uc3" %>
<div id="divFV">
    <asp:FormView ID="FV_Item" runat="server" DataSourceID="ODS_Item" DefaultMode="Insert"
        Width="100%" DataKeyNames="Code">
        <InsertItemTemplate>
            <fieldset>
                <legend>${MasterData.Item.NewItem}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Item.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rtvCode" runat="server" ErrorMessage="${MasterData.Item.Code.Empty}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvInsert" runat="server" ControlToValidate="tbCode" ErrorMessage="${MasterData.Item.CodeExist1}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkItemExists" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlItemImage" runat="server" Text="${MasterData.Item.Image}:" />
                        </td>
                        <td class="td02">
                            <asp:FileUpload ID="fileUpload" ContentEditable="false" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlDesc1" runat="server" Text="${MasterData.Item.Description}1:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc1" runat="server" Text='<%# Bind("Desc1") %>' CssClass="inputRequired"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDesc1" runat="server" ErrorMessage="${MasterData.Item.Desc1.Empty}"
                                Display="Dynamic" ControlToValidate="tbDesc1" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlDesc2" runat="server" Text="${MasterData.Item.Description}2:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc2" runat="server" Text='<%# Bind("Desc2") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlUom" runat="server" Text="${MasterData.Item.Uom}:" />
                        </td>
                        <td class="td02">
                            <%--<asp:DropDownList ID="ddlUom" runat="server" DataTextField="Description" DataValueField="Code"
                                Text='<%# Bind("Uom") %>' DataSourceID="ODS_ddlUom">
                            </asp:DropDownList>--%>
                            <uc3:textbox ID="tbUom" runat="server" Visible="true" DescField="Description" ValueField="Code"
                                ServicePath="UomMgr.service" ServiceMethod="GetAllUom" MustMatch="true" CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvUom" runat="server" ErrorMessage="${MasterData.Item.Uom.Empty}"
                                Display="Dynamic" ControlToValidate="tbUom" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlType" runat="server" Text="${MasterData.Item.Type}:" />
                        </td>
                        <td class="td02">
                            <asp:DropDownList ID="ddlType" runat="server" DataTextField="Description" DataValueField="Value"
                                Text='<%# Bind("Type") %>' DataSourceID="ODS_ddlType">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cvType" runat="server" ControlToValidate="ddlType" Display="Dynamic"
                                ValidationGroup="vgSave" OnServerValidate="checkInput" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlCount" runat="server" Text="${MasterData.Item.Uc}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCount" runat="server" Text='<%# Bind("UnitCount") %>' CssClass="inputRequired"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUC" runat="server" ErrorMessage="${MasterData.Item.UC.Empty}"
                                Display="Dynamic" ControlToValidate="tbCount" ValidationGroup="vgSave" />
                            <asp:RegularExpressionValidator ID="revCount" ControlToValidate="tbCount" runat="server"
                                ValidationGroup="vgSave" ErrorMessage="${MasterData.Item.UC.Format}" ValidationExpression="^[0-9]+(.[0-9]{1,8})?$"
                                Display="Dynamic" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblHuLotSize" runat="server" Text="${MasterData.Item.HuLotSize}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbHuLotSize" runat="server" Text='<%# Bind("HuLotSize") %>' CssClass="inputRequired"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvHuLotSize" runat="server" ErrorMessage="${MasterData.Item.HuLotSize.Empty}"
                                Display="Dynamic" ControlToValidate="tbHuLotSize" ValidationGroup="vgSave" />
                            <asp:RegularExpressionValidator ID="revHuLotSize" ControlToValidate="tbHuLotSize"
                                runat="server" ValidationGroup="vgSave" ErrorMessage="${MasterData.Item.UC.Format}"
                                ValidationExpression="^[0-9]+(.[0-9]{1,8})?$" Display="Dynamic" />
                        </td>
                    </tr>
                     <tr>
                        <td class="td01">
                            <asp:Literal ID="lblMsl" runat="server" Text="${MasterData.Item.Msl}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbMsl" runat="server" Text='<%# Bind("Msl") %>' />
                        </td>
                        <td class="td01">
                        <asp:Literal ID="lblBin" runat="server" Text="${MasterData.Item.Bin}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbBin" runat="server" Text='<%# Bind("Bin") %>' />
                        </td>
                    </tr>
                   <%-- <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlLocation" runat="server" Text="${MasterData.Item.Location}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbLocation" runat="server" Visible="true" DescField="Name" ValueField="Code"
                                ServicePath="LocationMgr.service" ServiceMethod="GetLocation" MustMatch="true"
                                Width="250" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlBom" runat="server" Text="${MasterData.Item.Bom}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbBom" runat="server" Visible="true" DescField="Description" ValueField="Code"
                                ServicePath="BomMgr.service" ServiceMethod="GetAllBom" MustMatch="true" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlRouting" runat="server" Text="${MasterData.Item.Routing}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbRouting" runat="server" Visible="true" DescField="Description"
                                ValueField="Code" ServicePath="RoutingMgr.service" ServiceMethod="GetAllRouting"
                                MustMatch="true" />
                        </td>
                    </tr>--%>
                    <tr>
                         <td class="td01">
                            <asp:Literal ID="lblDefaultSupplier" runat="server" Text="${MasterData.Item.DefaultSupplier}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbDefaultSupplier" runat="server" Visible="true" DescField="Name" ValueField="Code" Width="250"
                                ServicePath="SupplierMgr.service" ServiceMethod="GetSupplier" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblItemCategory" Text="${MasterData.Item.ItemCategory}:" runat="server" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbItemCategory" runat="server" Visible="true" Width="250" DescField="Description"
                                ValueField="Code" ServicePath="ItemCategoryMgr.service" ServiceMethod="GetCacheAllItemCategory"
                                CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvItemCategory" runat="server" ErrorMessage="${MasterData.Item.Category.Empty}"
                                Display="Dynamic" ControlToValidate="tbItemCategory" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCategory1" runat="server" Text="${MasterData.Item.Category1}:" />
                        </td>
                        <td class="td02">
                            <asp:DropDownList ID="ddlCategory1" runat="server" DataTextField="FullName" DataValueField="Code"
                                Width="200" DataSourceID="ODS_ddlCategory1">
                            </asp:DropDownList>
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCategory2" runat="server" Text="${MasterData.Item.Category2}:" />
                        </td>
                        <td class="td02">
                            <asp:DropDownList ID="ddlCategory2" runat="server" DataTextField="FullName" DataValueField="Code"
                                Width="200" DataSourceID="ODS_ddlCategory2">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCategory3" runat="server" Text="${MasterData.Item.Category3}:" />
                        </td>
                        <td class="td02">
                            <asp:DropDownList ID="ddlCategory3" runat="server" DataTextField="FullName" DataValueField="Code"
                                Width="200" DataSourceID="ODS_ddlCategory3">
                            </asp:DropDownList>
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCategory4" runat="server" Text="${MasterData.Item.Category4}:" />
                        </td>
                        <td class="td02">
                            <asp:DropDownList ID="ddlCategory4" runat="server" DataTextField="FullName" DataValueField="Code"
                                Width="200" DataSourceID="ODS_ddlCategory4">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblIsSortAndColor" runat="server" Text="${MasterData.Item.IsSortAndColor}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbIsSortAndColor" runat="server" Checked='<%#Bind("IsSortAndColor") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblItemBrand" runat="server" Text="${MasterData.Item.ItemBrand}:" />
                        </td>
                        <td class="td02">
                            <asp:DropDownList ID="ddlItemBrand" runat="server" DataTextField="FullDescription" DataValueField="Code"
                                Width="200" DataSourceID="ODS_ddlItemBrand">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblSortLevel1From" runat="server" Text="${MasterData.Item.SortLevel1From}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbSortLevel1From" runat="server" Text='<%# Bind("SortLevel1From") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblSortLevel1To" runat="server" Text="${MasterData.Item.SortLevel1To}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbSortLevel1To" runat="server" Text='<%# Bind("SortLevel1To") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblColorLevel1From" runat="server" Text="${MasterData.Item.ColorLevel1From}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbColorLevel1From" runat="server" Text='<%# Bind("ColorLevel1From") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblColorLevel1To" runat="server" Text="${MasterData.Item.ColorLevel1To}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbColorLevel1To" runat="server" Text='<%# Bind("ColorLevel1To") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblSortLevel2From" runat="server" Text="${MasterData.Item.SortLevel2From}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbSortLevel2From" runat="server" Text='<%# Bind("SortLevel2From") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblSortLevel2To" runat="server" Text="${MasterData.Item.SortLevel2To}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbSortLevel2To" runat="server" Text='<%# Bind("SortLevel2To") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblColorLevel2From" runat="server" Text="${MasterData.Item.ColorLevel2From}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbColorLevel2From" runat="server" Text='<%# Bind("ColorLevel2From") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblColorLevel2To" runat="server" Text="${MasterData.Item.ColorLevel2To}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbColorLevel2To" runat="server" Text='<%# Bind("ColorLevel2To") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblScrapPercentage" runat="server" Text="${MasterData.Item.ScrapPercentage}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbScrapPercentage" runat="server" Text='<%# Bind("ScrapPercentage") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblPinNumber" runat="server" Text="${MasterData.Item.PinNumber}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbPinNumber" runat="server" Text='<%# Bind("PinNumber") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblGoodsReceiptLotSize" runat="server" Text="${MasterData.Item.GoodsReceiptLotSize}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbGoodsReceiptLotSize" runat="server" Text='<%# Bind("GoodsReceiptLotSize") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblHistoryPrice" runat="server" Text="${MasterData.Item.HistoryPrice}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbHistoryPrice" runat="server" Text='<%# Bind("HistoryPrice") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblScrapPrice" runat="server" Text="${MasterData.Item.ScrapPrice}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbScrapPrice" runat="server" Text='<%# Bind("ScrapPrice") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblScrapBillAddress" runat="server" Text="${MasterData.Item.ScrapBillAddress}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbScrapBillAddress" runat="server" Visible="true" Width="250" DescField="Address"
                                ValueField="Code" ServicePath="BillAddressMgr.service" ServiceMethod="GetAllBillAddress" />
                        </td>
                    </tr>
                    <td class="td01">
                        <asp:Literal ID="lblMRPLeadTime" runat="server" Text="${MasterData.Item.MRPLeadTime}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbMRPLeadTime" runat="server" Text='<%# Bind("MRPLeadTime") %>'></asp:TextBox>
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblMemo" runat="server" Text="${MasterData.Item.Memo}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbMemo" runat="server" Text='<%# Bind("Memo") %>'></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblSalesCost" runat="server" Text="${MasterData.Item.SalesCost}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbSalesCost" runat="server" Text='<%# Bind("SalesCost") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${MasterData.Item.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="tbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblNeedInspect" runat="server" Text="${MasterData.Item.NeedInspect}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbNeedInspect" runat="server" Checked='<%#Bind("NeedInspect") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblIsRunMrp" runat="server" Text="${MasterData.Item.IsRunMrp}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbIsRunMrp" runat="server" Checked='<%# Bind("IsRunMrp") %>' />
                        </td>
                    </tr>
                   <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlItemPack" runat="server" Text="${Menu.MasterData.ItemPack}:"></asp:Literal>
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="txtItemPack" runat="server" Visible="true" Width="250" MustMatch="false"
                                DescField="Descr" ValueField="Spec" ServicePath="OrderProductionPlanMgr.service" ServiceMethod="GetItemPack" />
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
<asp:ObjectDataSource ID="ODS_Item" runat="server" TypeName="com.Sconit.Web.ItemMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Item" InsertMethod="CreateItem"
    OnInserted="ODS_Item_Inserted" OnInserting="ODS_Item_Inserting">
    <InsertParameters>
        <asp:Parameter Name="GoodsReceiptLotSize" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="SalesCost" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="ScrapPercentage" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="ScrapPrice" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="HistoryPrice" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="MRPLeadTime" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="PinNumber" Type="Decimal" ConvertEmptyStringToNull="true" />
    </InsertParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_ddlType" runat="server" TypeName="com.Sconit.Web.CodeMasterMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.CodeMaster" SelectMethod="GetCachedCodeMaster">
    <SelectParameters>
        <asp:QueryStringParameter Name="code" DefaultValue="ItemType" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_ddlUom" runat="server" TypeName="com.Sconit.Web.UomMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Uom" SelectMethod="GetAllUom">
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_ddlCategory1" runat="server" TypeName="com.Sconit.Web.ItemTypeMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemType" SelectMethod="GetItemTypeIncludeEmpty">
    <SelectParameters>
        <asp:QueryStringParameter Name="level" DefaultValue="1" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_ddlCategory2" runat="server" TypeName="com.Sconit.Web.ItemTypeMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemType" SelectMethod="GetItemTypeIncludeEmpty">
    <SelectParameters>
        <asp:QueryStringParameter Name="level" DefaultValue="2" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_ddlCategory3" runat="server" TypeName="com.Sconit.Web.ItemTypeMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemType" SelectMethod="GetItemTypeIncludeEmpty">
    <SelectParameters>
        <asp:QueryStringParameter Name="level" DefaultValue="3" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_ddlCategory4" runat="server" TypeName="com.Sconit.Web.ItemTypeMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemType" SelectMethod="GetItemTypeIncludeEmpty">
    <SelectParameters>
        <asp:QueryStringParameter Name="level" DefaultValue="4" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_ddlItemBrand" runat="server" TypeName="com.Sconit.Web.ItemBrandMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemBrand" SelectMethod="GetItemBrandIncludeEmpty">
</asp:ObjectDataSource>
