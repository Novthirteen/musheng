<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="MasterData_LedSortLevel_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
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
                 <uc3:textbox ID="tbItemCode" runat="server" Visible="true" Width="250" MustMatch="true" DescField="Description"
                                ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"/>
            </td>
        </tr>
        <tr>
            <td colspan="3" />
            <td class="ttd02">
                <div class="buttons">
                    <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                        CssClass="query" />
                    <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                        CssClass="back" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<asp:ObjectDataSource ID="ODS_ddlItemBrand" runat="server" TypeName="com.Sconit.Web.ItemBrandMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemBrand" SelectMethod="GetItemBrandIncludeEmpty">
</asp:ObjectDataSource>
