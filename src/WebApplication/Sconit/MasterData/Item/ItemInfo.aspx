<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Page.master" AutoEventWireup="true"
    CodeFile="ItemInfo.aspx.cs" Inherits="MasterData_Item_ItemInfo" %>

<asp:Content ID="ctBody" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:FormView ID="FV_Item" runat="server" DefaultMode="ReadOnly" Width="100%" OnDataBound="FV_Item_DataBound">
        <ItemTemplate>
            <fieldset>
                <legend>${MasterData.Item.BasicInfo}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01" style="width: 150px" rowspan="6">
                            <asp:Image ID="imgItem" ImageUrl='<%# Eval("ImageUrl") %>' runat="server" Width="150px" />
                        </td>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlCode" runat="server" Text="${MasterData.Item.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code") %>' />
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td01">
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlDesc1" runat="server" Text="${MasterData.Item.Description}1:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="lblDesc1" runat="server" Text='<%# Eval("Desc1") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlDesc2" runat="server" Text="${MasterData.Item.Description}2:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="lblDesc2" runat="server" Text='<%# Eval("Desc2") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlUom" runat="server" Text="${MasterData.Item.Uom}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="lblUom" runat="server" Text='<%# Eval("Uom.Code") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlType" runat="server" Text="${MasterData.Item.Type}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlCount" runat="server" Text="${MasterData.Item.Uc}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="lblCount" runat="server" Text='<%# Eval("UnitCount","{0:0.########}") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlLocation" runat="server" Text="${MasterData.Item.Location}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location.Code") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlBom" runat="server" Text="${MasterData.Item.Bom}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="lblBom" runat="server" Text='<%# Eval("Bom.Code") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlRouting" runat="server" Text="${MasterData.Item.Routing}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="lblRouting" runat="server" Text='<%# Eval("Routing.Code") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlMemo" runat="server" Text="${MasterData.Item.Memo}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="lblMemo" runat="server" Text='<%# Eval("Memo") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${MasterData.Item.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="tbIsActive" runat="server" Checked='<%# Eval("IsActive") %>' Enabled="false" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ItemTemplate>
    </asp:FormView>
    <fieldset id="fldItemKit" runat="server" visible="false">
        <legend>${MasterData.Item.ParentKit}</legend>
        <asp:GridView ID="GV_List_ItemKit" runat="server" AutoGenerateColumns="False" DataKeyNames="ParentItem,ChildItem">
            <Columns>
                <asp:TemplateField HeaderText="${MasterData.Item.Code}" SortExpression="ChildItem.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ChildItem.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Description}" SortExpression="ChildItem.Desc1">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ChildItem.Description")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Type}" SortExpression="ChildItem.Type">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ChildItem.Type")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Uom}" SortExpression="ChildItem.Uom.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ChildItem.Uom.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Uc}" SortExpression="ChildItem.UnitCount">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ChildItem.UnitCount","{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Qty" HeaderText="${MasterData.ItemKit.Qty}" SortExpression="Qty"
                    DataFormatString="{0:0.########}" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="${MasterData.Item.IsActive}"
                    SortExpression="IsActive" />
            </Columns>
        </asp:GridView>
    </fieldset>
    <fieldset id="fldItemRef" runat="server" visible="false">
        <legend>${MasterData.ItemRef}</legend>
        <asp:GridView ID="GV_List_ItemRef" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
            <Columns>
                <asp:BoundField DataField="Id" Visible="false" />
                <asp:TemplateField HeaderText="${MasterData.Item.Code}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Description}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDescription" runat="server" Text='<%# Eval("Item.Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ItemReference.Party.Code}">
                    <ItemTemplate>
                        <asp:Label ID="lblPartyCode" runat="server" Text='<%# Eval("Party.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ItemReference.Party.Name}">
                    <ItemTemplate>
                        <asp:Label ID="lblPartyName" runat="server" Text='<%# Eval("Party.Name") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ItemReference.ReferenceCode}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemReferenceCode" runat="server" Text='<%# Eval("ReferenceCode") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ItemReference.Description}" ItemStyle-Width="160px">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ItemReference.Remark}" ItemStyle-Width="160px">
                    <ItemTemplate>
                        <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.IsActive}">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbActive1" runat="server" Checked='<%# Eval("IsActive") %>' Enabled="false" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>
</asp:Content>
