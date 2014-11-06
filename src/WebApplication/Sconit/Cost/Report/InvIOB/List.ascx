<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Cost_Report_InvIOB_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<fieldset>
    <div class="GridView">
        <sc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" seqtext="No."
            ShowSeqNo="true" AllowSorting="false" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${Common.Business.ItemCode}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.ItemDescription}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDescription" runat="server" Text='<%# Eval("Item.Description")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Location}">
                    <ItemTemplate>
                        <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("Location.Name")%>'
                            ToolTip='<%# Eval("Location.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Uom}">
                    <ItemTemplate>
                        <asp:Label ID="lblUOM" runat="server" Text='<%# Eval("Item.Uom.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="StartInvQty" HeaderText="${Reports.StartInv}" DataFormatString="{0:0.###}"
                    ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="RCTPO" HeaderText="${Reports.TransType.RCTPO}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="RCTTRNML" HeaderText="${Reports.TransType.RCTTRNML}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="RCTTRSUB" HeaderText="${Reports.TransType.RCTTRSUB}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="RCTTRREM" HeaderText="${Reports.TransType.RCTTRREM}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="RCTINP" HeaderText="${Reports.TransType.RCTINP}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="RCTWOHOM" HeaderText="${Reports.TransType.RCTWOHOM}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="RCTWOSUB" HeaderText="${Reports.TransType.RCTWOSUB}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="RCTUNP" HeaderText="${Reports.TransType.RCTUNP}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="TotalInQty" HeaderText="${Reports.TotalInQty}" DataFormatString="{0:0.###}"
                    ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="ISSSO" HeaderText="${Reports.TransType.ISSSO}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="ISSTRNML" HeaderText="${Reports.TransType.ISSTRNML}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="ISSTRSUB" HeaderText="${Reports.TransType.ISSTRSUB}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="ISSTRREM" HeaderText="${Reports.TransType.ISSTRREM}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="ISSINP" HeaderText="${Reports.TransType.ISSINP}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="ISSWO" HeaderText="${Reports.TransType.ISSWO}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="ISSUNP" HeaderText="${Reports.TransType.ISSUNP}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="TotalOutQty" HeaderText="${Reports.TotalOutQty}" DataFormatString="{0:0.###}"
                    ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="CYCCNT" HeaderText="${Reports.TransType.CYCCNT}" DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="NoStatsQty" HeaderText="${Reports.TransType.NoStatsQty}"
                    DataFormatString="{0:0.###}" />
                <asp:BoundField DataField="InvQty" HeaderText="${Reports.EndInv}" DataFormatString="{0:0.###}"
                    ItemStyle-Font-Bold="true" />
            </Columns>
        </sc1:GridView>
        <sc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </sc1:GridPager>
    </div>
</fieldset>
