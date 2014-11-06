<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="MasterData_Item_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Code"
            ShowSeqNo="true" AllowSorting="true" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${MasterData.Item.Image}" SortExpression="ImageUrl" ItemStyle-Width="150px">
                    <ItemTemplate>
                       <asp:Image ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ImageUrl")%>' runat="server" ID="imgImageUrl"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Code" HeaderText="${Common.Business.Code}" SortExpression="Code" />
                <asp:TemplateField HeaderText="${Common.Business.Description}" SortExpression="Desc1">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Description")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Description}1" Visible="false" SortExpression="Desc1">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Desc1")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Description}2" Visible="false" SortExpression="Desc2">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Desc2")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="UnitCount" DataFormatString="{0:0.########}" HeaderText="${MasterData.Item.Uc}"
                    SortExpression="UnitCount" />
                <asp:BoundField DataField="HuLotSize" DataFormatString="{0:0.########}" Visible="false" HeaderText="${MasterData.Item.HuLotSize}"
                    SortExpression="HuLotSize" />
                <asp:TemplateField HeaderText="${MasterData.Item.Category1}" Visible="false" SortExpression="Category1.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Category1.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Category2}" Visible="false" SortExpression="Category2.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Category2.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Category3}" Visible="false" SortExpression="Category3.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Category3.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Category4}" Visible="false" SortExpression="Category4.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Category4.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.ItemBrand}" Visible="false" SortExpression="ItemBrand.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ItemBrand.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.ScrapPercentage}" Visible="false" SortExpression="ScrapPercentage">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ScrapPercentage", "{0:#.#####}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.PinNumber}" Visible="false" SortExpression="PinNumber">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "PinNumber", "{0:#}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.ScrapPrice}" Visible="false" SortExpression="ScrapPrice">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ScrapPrice", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.HistoryPrice}" Visible="false" SortExpression="HistoryPrice">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "HistoryPrice", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.SalesCost}"  Visible="false" SortExpression="SalesCost">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "SalesCost", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.DefaultSupplier}" Visible="false" SortExpression="DefaultSupplier">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DefaultSupplier")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Type}" SortExpression="Type">
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="lblType" runat="server" Code="ItemType" Value='<%# DataBinder.Eval(Container.DataItem, "Type")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="${MasterData.Item.Uom}" SortExpression="Uom.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Uom.Code") %> [<%# DataBinder.Eval(Container.DataItem, "Uom.Description")%>]
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="${MasterData.Item.Location}" SortExpression="Location">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Location.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Bom}" SortExpression="Bom">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Bom.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Routing}" SortExpression="Routing">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Routing.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="${MasterData.Item.Msl}" SortExpression="Msl">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Msl")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Bin}" SortExpression="Bin">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Bin")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Memo" HeaderText="${MasterData.Item.Memo}" SortExpression="Memo" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="${MasterData.Item.IsActive}"
                    SortExpression="IsActive" />
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Code") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click" />
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Code") %>'
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
