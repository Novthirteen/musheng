<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="MasterData_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="OrderNo"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" OnRowDataBound="GV_List_RowDataBound" DefaultSortExpression="OrderNo"
            DefaultSortDirection="Descending">
            <Columns>
                <asp:BoundField DataField="OrderNo" HeaderText="${Common.Business.Id}" SortExpression="OrderNo" />
                <asp:BoundField DataField="EffectiveDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="${MasterData.MiscOrder.EffectDate}"
                    SortExpression="EffectiveDate" />
                <asp:TemplateField HeaderText="${Common.Business.Region}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Location.Region.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Location}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Location.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.MiscOrder.SubjectCode}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "SubjectList.SubjectCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.MiscOrder.CostCenterCode}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "SubjectList.CostCenterCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.MiscOrder.CreateUser}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CreateUser.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CreateDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="${Common.Business.CreateDate}"
                    SortExpression="CreateDate" />
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnView" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderNo") %>'
                            Text="${Common.Button.View}" OnClick="lbtnView_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
    <div class="GridView">
        <cc1:GridView ID="GV_List_Detail" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp_Detail" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" OnRowDataBound="GV_List_Detail_RowDataBound" DefaultSortExpression="Id"
            DefaultSortDirection="Descending">
            <Columns>
                <asp:TemplateField HeaderText="${Common.Business.Id}" SortExpression="MiscOrder.OrderNo">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "MiscOrder.OrderNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.MiscOrder.EffectDate}" SortExpression="MiscOrder.EffectiveDate">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "MiscOrder.EffectiveDate","{0:yyyy-MM-dd}") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Region}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "MiscOrder.Location.Region.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Location}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "MiscOrder.Location.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.MiscOrder.SubjectCode}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "MiscOrder.SubjectList.SubjectCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.MiscOrder.CostCenterCode}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "MiscOrder.SubjectList.CostCenterCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.MiscOrder.CreateUser}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "MiscOrder.CreateUser.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.ItemCode}" SortExpression="Item.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblItem" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Item.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Item.Description}" SortExpression="Item.Description">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Item.Description")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:BoundField DataField="HuId"  HeaderText="${Common.Business.HuId}" />
                 <asp:BoundField DataField="Qty" DataFormatString="{0:0.##}" HeaderText="${Common.Business.ItemQty}" />
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnView" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MiscOrder.OrderNo") %>'
                            Text="${Common.Button.View}" OnClick="lbtnView_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp_Detail" runat="server" GridViewID="GV_List_Detail" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
