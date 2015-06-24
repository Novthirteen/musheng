<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="MasterData_ItemPoint_Search" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>

<fieldset>
<table class="mtable">
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlItem" runat="server" Text="${MasterData.ItemPoint.Item}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbItem" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01">
            </td>
            <td class="ttd02">
            </td>
        </tr>
        <tr>
        <td colspan="3">
        </td>
        <td class="ttd02">
            <asp:Button ID="SearchBtn" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                CssClass="button2" />
            <cc1:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                    CssClass="button2" FunctionId="EditOrder" />
        </td>
        </tr>
    </table>
</fieldset>
<%----------------------------------------------------------------------------------%>
<fieldset id="fldGV" runat="server" >
    <asp:GridView ID="GV_ItemPoint" runat="server" AutoGenerateColumns="False" DataSourceID="ODS_GV_ItemPoint"
        DataKeyNames="Item" AllowPaging="True" PageSize="10" visible="false">
        <Columns>
            <asp:BoundField DataField="Item" HeaderText="${MasterData.ItemPoint.Item}" />
            <asp:BoundField DataField="Flow" HeaderText="${MasterData.ItemPoint.Flow}" />
            <asp:BoundField DataField="Fact" HeaderText="${MasterData.ItemPoint.Fact}" />
            <asp:BoundField DataField="Model" HeaderText="${MasterData.ItemPoint.Model}" />
            <asp:BoundField DataField="Point" HeaderText="${MasterData.ItemPoint.Point}" />
            <asp:BoundField DataField="TransferTime" HeaderText="${MasterData.ItemPoint.TransferTime}" />
            <asp:BoundField DataField="EquipmentTime" HeaderText="${MasterData.ItemPoint.EquipmentTime}" />
            <asp:BoundField DataField="PCBNumber" HeaderText="${MasterData.ItemPoint.PCBNumber}" />
            <%--<asp:CommandField ShowDeleteButton="true" ItemStyle-Width="20%"
                HeaderText="${Common.GridView.Action}" DeleteText="&lt;span onclick=&quot;JavaScript:return confirm('${Common.Delete.Confirm}?')&quot;&gt;${Common.Button.Delete}&lt;/span&gt;">
            </asp:CommandField>--%>
            <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Item") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click">
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Item") %>'
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Label runat="server" ID="lblResult" Text="${Common.GridView.NoRecordFound}"
        Visible="false" />
</fieldset>
<asp:ObjectDataSource ID="ODS_GV_ItemPoint" runat="server" TypeName="com.Sconit.Web.ItemPointMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemPoint" SelectMethod="LoadItemPoint" 
    UpdateMethod="UpdateItemPoint" OnUpdating="ODS_GV_ItemPoint_OnUpdating" 
    DeleteMethod="DeleteItemPoint" OnDeleting="ODS_GV_ItemPoint_OnDeleting" OnDeleted="ODS_GV_ItemPoint_OnDeleted">
    <SelectParameters>
        <asp:ControlParameter ControlID="tbItem" Name="Item" PropertyName="Text" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>