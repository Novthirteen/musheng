<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_LedSortLevel_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_LedSortLevel" runat="server" DataSourceID="ODS_LedSortLevel"
        DefaultMode="Insert" DataKeyNames="Code">
        <InsertItemTemplate>
            <fieldset>
                <legend>${MasterData.LedSortLevel.AddLedSortLevel}</legend>
                <table class="mtable">
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lblBrand" runat="server" Text="${MasterData.LedSortLevel.Brand}:" />
                        </td>
                        <td class="ttd02">
                            <asp:DropDownList ID="ddlItemBrand" runat="server" DataTextField="Description" DataValueField="Code"
                                Width="200" DataSourceID="ODS_ddlItemBrand">
                            </asp:DropDownList>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblItem" runat="server" Text="${MasterData.LedSortLevel.Item}:" />
                        </td>
                        <td class="ttd02">
                            <uc3:textbox ID="tbItemCode" runat="server" Visible="true" Width="250" MustMatch="true"
                                DescField="Description" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblSequence" runat="server" Text="${MasterData.LedSortLevel.Sequence}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbSequence" runat="server" Text='<%# Bind("Sequence") %>' Width="250"></asp:TextBox>
                            <asp:RangeValidator ID="rvSeq" runat="server" ControlToValidate="tbSequence" ErrorMessage="${Common.Validator.Valid.Number}"
                                Display="Dynamic" Type="Integer" MinimumValue="0" MaximumValue="99999999" ValidationGroup="vgSaveGroup" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblValue" runat="server" Text="${MasterData.LedSortLevel.Value}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbValue" runat="server" Text='<%# Bind("Value") %>' Width="250"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                       
                        <td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${MasterData.LedSortLevel.IsActive}:" />
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
                                    CssClass="apply" ValidationGroup="vgSaveGroup" />
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
<asp:ObjectDataSource ID="ODS_LedSortLevel" runat="server" TypeName="com.Sconit.Web.LedSortLevelMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Customize.LedSortLevel" InsertMethod="CreateLedSortLevel"
    OnInserted="ODS_LedSortLevel_Inserted" OnInserting="ODS_LedSortLevel_Inserting">
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_ddlItemBrand" runat="server" TypeName="com.Sconit.Web.ItemBrandMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemBrand" SelectMethod="GetItemBrandIncludeEmpty">
</asp:ObjectDataSource>
