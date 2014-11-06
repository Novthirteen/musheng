<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="MasterData_ItemType_Edit" %>

<div id="divFV" runat="server">
    <asp:FormView ID="FV_ItemType" runat="server" DataSourceID="ODS_ItemType" DefaultMode="Edit"
        Width="100%" DataKeyNames="Code" OnDataBound="FV_ItemType_DataBound">
        <EditItemTemplate>
            <fieldset>
                <legend>${MasterData.ItemType.UpdateItemType}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.ItemType.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbCode" runat="server" Text='<%# Bind("Code") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblName" runat="server" Text="${MasterData.ItemType.Name}:" />
                        </td>
                        <td class="td02">
                             <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' Width="250"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblShortName" runat="server" Text="${MasterData.ItemType.ShortName}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbShortName" runat="server" Text='<%# Bind("ShortName") %>' Width="250"></asp:TextBox>
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblLevel" runat="server" Text="${MasterData.ItemType.Level}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbLevel" runat="server" Text='<%#Bind("Level","{0:0}") %>'
                                CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvLevel" runat="server" ErrorMessage="${MasterData.ItemType.Level.Empty}"
                                Display="Dynamic" ControlToValidate="tbLevel" ValidationGroup="vgSave" />
                            <asp:RangeValidator ID="rvLevel" ControlToValidate="tbLevel" runat="server"
                                Display="Dynamic" ErrorMessage="${MasterData.ItemType.Level.Format}" MaximumValue="4"
                                MinimumValue="1" Type="Double" ValidationGroup="vgSave" />
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
                                <asp:Button ID="Button1" runat="server" CommandName="Update" Text="${Common.Button.Save}" CssClass="apply"
                                    ValidationGroup="vgSave" />
                                <asp:Button ID="Button2" runat="server" CommandName="Delete" Text="${Common.Button.Delete}" CssClass="delete"
                                    OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />
                                <asp:Button ID="Button3" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" CssClass="back" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </EditItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_ItemType" runat="server" TypeName="com.Sconit.Web.ItemTypeMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemType" UpdateMethod="UpdateItemType"
    OnUpdated="ODS_ItemType_Updated" OnUpdating="ODS_ItemType_Updating" DeleteMethod="DeleteItemType"
    OnDeleted="ODS_ItemType_Deleted" SelectMethod="LoadItemType">
    <SelectParameters>
        <asp:Parameter Name="code" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="code" Type="String" />
    </DeleteParameters>
</asp:ObjectDataSource>
