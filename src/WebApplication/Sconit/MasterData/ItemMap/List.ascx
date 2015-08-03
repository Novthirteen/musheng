<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="MasterData_ItemMap_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="true" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount">
            <Columns>
                <asp:TemplateField HeaderText="${MasterData.ItemMap.Item}" SortExpression="Item">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Item.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ItemMap.MapItem}" SortExpression="DiscontinueItem">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DiscontinueItem.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="StartDate" HeaderText="${MasterData.ItemMap.StartDate}" SortExpression="StartDate" />
                <asp:BoundField DataField="EndDate" HeaderText="${MasterData.ItemMap.EndDate}" SortExpression="EndDate" />  
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click" />
                            
                        <cc1:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' FunctionId="DeleteItemDisContinue"
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />

                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>