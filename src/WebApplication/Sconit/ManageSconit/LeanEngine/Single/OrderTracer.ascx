<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderTracer.ascx.cs" Inherits="ManageSconit_LeanEngine_Single_OrderTracer" %>
<div id="floatdiv">
    <fieldset>
        <legend>${Reports.ViewDetail}</legend>
        <div class="GridView">
            <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" OnRowDataBound="GV_List_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="${Common.Business.Type}">
                        <ItemTemplate>
                            <asp:Label ID="lblTracerType" runat="server" Text='<%# Bind("TracerType") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Common.Business.Code}">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderNo" runat="server" Text='<%# Bind("Code") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Common.Business.Date}">
                        <ItemTemplate>
                            <asp:Label ID="lblReqTime" runat="server" Text='<%# Bind("ReqTime","{0:yyyy-MM-dd HH:mm}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Common.Business.Item}">
                        <ItemTemplate>
                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("Item") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="${Common.Business.ItemDescription}">
                        <ItemTemplate>
                            <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("OrderDetail.Item.Description") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="${Common.Business.Qty}">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderedQty" runat="server" Text='<%# Bind("Qty","{0:0.########}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Common.Business.Remark}">
                        <ItemTemplate>
                            <asp:Label ID="lblMemo" runat="server" Text='<%# Bind("Memo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="tablefooter">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" />
            </div>
        </div>
    </fieldset>
</div>
