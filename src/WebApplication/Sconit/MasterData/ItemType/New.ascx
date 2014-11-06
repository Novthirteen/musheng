<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_ItemType_New" %>

<div id="divFV" runat="server">
    <asp:FormView ID="FV_ItemType" runat="server" DataSourceID="ODS_ItemType" DefaultMode="Insert"
        DataKeyNames="Code">
        <InsertItemTemplate>
            <fieldset>
                <legend>${MasterData.ItemType.AddItemType}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.ItemType.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' CssClass="inputRequired"
                                Width="250"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ErrorMessage="${MasterData.ItemType.Code.Empty}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvCode" runat="server" ControlToValidate="tbCode" Display="Dynamic"
                                ValidationGroup="vgSave" OnServerValidate="CV_ServerValidate" />
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
<asp:ObjectDataSource ID="ODS_ItemType" runat="server" TypeName="com.Sconit.Web.ItemTypeMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemType" InsertMethod="CreateItemType"
    OnInserted="ODS_ItemType_Inserted" OnInserting="ODS_ItemType_Inserting">
</asp:ObjectDataSource>
