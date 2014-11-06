<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="ManageSconit_Check_Main" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td>
                <asp:Button ID="btnUom" runat="server" Text="${Uom.Conversion.Check}" OnClick="btnUom_Click" />
            </td>
            <td>
                <asp:Button ID="btnFlowDetail" runat="server" Text="FlowDetail Check" OnClick="btnFlowDetail_Click" Visible="false"/>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
</fieldset>
<fieldset id="fldList" runat="server" visible="false">
<legend>${Uom.Conversion.Check}</legend>
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="false" OnRowDataBound="GV_List_RowDataBound"
        CellPadding="0">
        <Columns>
            <asp:BoundField HeaderText="Type" DataField="Type" />
            <asp:BoundField HeaderText="Code" DataField="Code" />
            <asp:BoundField HeaderText="Description" DataField="Description" />
            <asp:BoundField HeaderText="SubType" DataField="SubType" />
            <asp:TemplateField HeaderText="ItemCode">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Item.Code")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ItemDescription">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Item.Description")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="BaseUom">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Item.Uom.Code")%>
                    [<%# DataBinder.Eval(Container.DataItem, "Item.Uom.Name")%>]
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="AlterUom">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "AlterUom.Code")%>
                    [<%# DataBinder.Eval(Container.DataItem, "AlterUom.Name")%>]
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>
