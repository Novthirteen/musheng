<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="MasterData_Item_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_Item" runat="server" DataSourceID="ODS_Item" DefaultMode="Edit"
        Width="100%" DataKeyNames="Code" OnDataBound="FV_Item_DataBound">
        <EditItemTemplate>
            <fieldset>
                <legend>${MasterData.Item.UpdateItem}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td02" rowspan="18" style="width: 150px">
                            <asp:Image ID="imgUpload" ImageUrl='<%# Eval("ImageUrl") %>' runat="server" Width="150px" />
                        </td>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Item.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbCode" runat="server" Text='<%# Bind("Code") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlItemImage" runat="server" Text="${MasterData.Item.Image}:" />
                        </td>
                        <td class="td02">
                            <asp:FileUpload ID="fileUpload" ContentEditable="false" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlDesc1" runat="server" Text="${MasterData.Item.Description}1:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc1" runat="server" Text='<%# Bind("Desc1") %>' CssClass="inputRequired"
                                Width="200" />
                            <asp:RequiredFieldValidator ID="rfvDesc1" runat="server" ErrorMessage="${MasterData.Item.Desc1.Empty}"
                                Display="Dynamic" ControlToValidate="tbDesc1" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlDesc2" runat="server" Text="${MasterData.Item.Description}2:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc2" runat="server" Text='<%# Bind("Desc2") %>' Width="200" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlUom" runat="server" Text="${MasterData.Item.Uom}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbUom" runat="server" ReadOnly="true" />
                            <%--                            <uc3:textbox ID="tbUom" runat="server" Visible="true" DescField="Description" CssClass="inputRequired"
                                ValueField="Code" ServicePath="UomMgr.service" ServiceMethod="GetAllUom" MustMatch="true" />
                            <asp:RequiredFieldValidator ID="rfvtbUom" runat="server" ErrorMessage="${MasterData.Item.Uom.Empty}"
                                Display="Dynamic" ControlToValidate="tbUom" ValidationGroup="vgSave" />--%>
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlType" runat="server" Text="${MasterData.Item.Type}:" />
                        </td>
                        <td class="td02">
                            <cc1:CodeMstrDropDownList ID="ddlType" Code="ItemType" runat="server" />
                            <asp:CustomValidator ID="cvType" runat="server" ControlToValidate="ddlType" Display="Dynamic"
                                ValidationGroup="vgSave" OnServerValidate="checkInput" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlCount" runat="server" Text="${MasterData.Item.Uc}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCount" runat="server" Text='<%# Bind("UnitCount","{0:0.########}") %>'
                                CssClass="inputRequired" />
                            <asp:RegularExpressionValidator ID="revCount" ControlToValidate="tbCount" runat="server"
                                ValidationGroup="vgSave" ErrorMessage="${MasterData.Item.UC.Format}" ValidationExpression="^[0-9]+(.[0-9]{1,8})?$"
                                Display="Dynamic" />
                            <asp:RequiredFieldValidator ID="rfvUC" runat="server" ErrorMessage="${MasterData.Item.UC.Empty}"
                                Display="Dynamic" ControlToValidate="tbCount" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblHuLotSize" runat="server" Text="${MasterData.Item.HuLotSize}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbHuLotSize" runat="server" Text='<%# Bind("HuLotSize") %>' CssClass="inputRequired"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvHuLotSize" runat="server" ErrorMessage="${MasterData.Item.HuLotSize.Empty}"
                                Display="Dynamic" ControlToValidate="tbHuLotSize" ValidationGroup="vgSave" />
                            <asp:RegularExpressionValidator ID="revHuLotSize" ControlToValidate="tbHuLotSize" runat="server"
                                ValidationGroup="vgSave" ErrorMessage="${MasterData.Item.UC.Format}" ValidationExpression="^[0-9]+(.[0-9]{1,8})?$"
                                Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                           <%-- 湿敏等级:--%>
                            <asp:Literal ID="lblMsl" runat="server" Text="${MasterData.Item.Msl}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbMsl" runat="server" Text='<%# Bind("Msl") %>' />
                        </td>
                        <td class="td01">
                        <asp:Literal ID="lblBin" runat="server" Text="${MasterData.Item.Bin}:" />
                           <%-- 制造库格:--%>
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbBin" runat="server" Text='<%# Bind("Bin") %>' />
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlBom" runat="server" Text="${MasterData.Item.Bom}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbBom" runat="server" Visible="true" DescField="Description" ValueField="Code"
                                ServicePath="BomMgr.service" ServiceMethod="GetAllBom" MustMatch="true" Width="250" />
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
                            <asp:DropDownList ID="ddlCategory1" runat="server" DataTextField="FullName" DataValueField="Code" Width="200"
                                DataSourceID="ODS_ddlCategory1">
                            </asp:DropDownList>
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCategory2" runat="server" Text="${MasterData.Item.Category2}:" />
                        </td>
                        <td class="td02">
                            <asp:DropDownList ID="ddlCategory2" runat="server" DataTextField="FullName" DataValueField="Code" Width="200"
                                DataSourceID="ODS_ddlCategory2">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCategory3" runat="server" Text="${MasterData.Item.Category3}:" />
                        </td>
                        <td class="td02">
                            <asp:DropDownList ID="ddlCategory3" runat="server" DataTextField="FullName" DataValueField="Code" Width="200"
                                DataSourceID="ODS_ddlCategory3">
                            </asp:DropDownList>
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCategory4" runat="server" Text="${MasterData.Item.Category4}:" />
                        </td>
                        <td class="td02">
                            <asp:DropDownList ID="ddlCategory4" runat="server" DataTextField="FullName" DataValueField="Code" Width="200"
                                DataSourceID="ODS_ddlCategory4">
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
                            <asp:DropDownList ID="ddlItemBrand" runat="server" DataTextField="FullDescription" DataValueField="Code" Width="200"
                                DataSourceID="ODS_ddlItemBrand">
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
                            <asp:TextBox ID="tbScrapPercentage" runat="server" Text='<%# Bind("ScrapPercentage", "{0:0.#####}") %>'  />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblPinNumber" runat="server" Text="${MasterData.Item.PinNumber}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbPinNumber" runat="server" Text='<%# Bind("PinNumber", "{0:#}") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblGoodsReceiptLotSize" runat="server" Text="${MasterData.Item.GoodsReceiptLotSize}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbGoodsReceiptLotSize" runat="server" Text='<%# Bind("GoodsReceiptLotSize", "{0:#.##}") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblHistoryPrice" runat="server" Text="${MasterData.Item.HistoryPrice}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbHistoryPrice" runat="server" Text='<%# Bind("HistoryPrice", "{0:0.####}") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblScrapPrice" runat="server" Text="${MasterData.Item.ScrapPrice}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbScrapPrice" runat="server" Text='<%# Bind("ScrapPrice", "{0:0.####}") %>' />
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
                        <asp:TextBox ID="tbMRPLeadTime" runat="server" Text='<%# Bind("MRPLeadTime", "{0:#.##}") %>'></asp:TextBox>
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblMemo" runat="server" Text="${MasterData.Item.Memo}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbMemo" runat="server" Text='<%# Bind("Memo") %>'></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                       <%-- <td class="td01"></td>--%>
                        <td class="td01">
                            <asp:Literal ID="lblSalesCost" runat="server" Text="${MasterData.Item.SalesCost}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbSalesCost" runat="server" Text='<%# Bind("SalesCost", "{0:0.####}") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${MasterData.Item.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="tbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 150px; text-align: center">
                            <asp:Literal ID="ltlDeleteImage" runat="server" Text="${MasterData.Item.DeleteImage}:" />
                            <asp:CheckBox ID="cbDeleteImage" runat="server" />
                        </td>
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
                        <td class="td01" style="width: 150px"></td>
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
                        <td class="td01" style="width: 150px">
                            &nbsp;
                        </td>
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
<asp:ObjectDataSource ID="ODS_Item" runat="server" TypeName="com.Sconit.Web.ItemMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Item" UpdateMethod="UpdateItem"
    OnUpdated="ODS_Item_Updated" SelectMethod="LoadItem" OnUpdating="ODS_Item_Updating"
    DeleteMethod="DeleteItem" OnDeleted="ODS_Item_Deleted" OnDeleting="ODS_Item_Deleting">
    <SelectParameters>
        <asp:Parameter Name="code" Type="String" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="GoodsReceiptLotSize" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="SalesCost" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="ScrapPercentage" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="ScrapPrice" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="HistoryPrice" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="MRPLeadTime" Type="Decimal" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="PinNumber" Type="Decimal" ConvertEmptyStringToNull="true" />
    </UpdateParameters>
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
