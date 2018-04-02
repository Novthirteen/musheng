<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Production_PLFeedSeqInfo_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="true" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${Common.Business.ProductionLineFacility}" SortExpression="ProductLineFacility">
                    <ItemTemplate>
                        <asp:Label ID="lblProductionFacility" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProductLineFacility")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Sequence" HeaderText="${Production.ProdutLineFeedSeqence.Sequence}"
                    SortExpression="Sequence" />
                <asp:BoundField DataField="Code" HeaderText="${Production.ProdutLineFeedSeqence.Code}"
                    SortExpression="Code" />
                <asp:TemplateField HeaderText="${Production.ProdutLineFeedSeqence.FinishGoodCode}"
                    SortExpression="FinishGood.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "FinishGood.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="${Production.ProdutLineFeedSeqence.FinishGoodDescription}"
                    SortExpression="FinishGood.Desc1">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "FinishGood.Description")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="${Production.ProdutLineFeedSeqence.RawMaterialCode}"
                    SortExpression="RawMaterial.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "RawMaterial.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="${Production.ProdutLineFeedSeqence.RawMaterialDescription}"
                    SortExpression="RawMaterial.Desc1">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "RawMaterial.Description")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <%--<asp:TemplateField HeaderText="${Common.Business.IsActive}" SortExpression="IsActive">
                    <ItemTemplate>
                        <asp:Label ID="lblIsActive" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <%--<asp:TemplateField HeaderText="${Common.Business.LastModifyDate}" SortExpression="LastModifyDate">
                    <ItemTemplate>
                        <asp:Label ID="lblLastModifyDate" runat="server" Text='<%# Eval("LastModifyDate","{0:yyyy-MM-dd HH:mm}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click" />
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="1000" Visible="false">
        </cc1:GridPager>
    </div>
</fieldset>
