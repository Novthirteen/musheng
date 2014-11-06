<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Reports_InvIOBCust_Main" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblParty" runat="server" Text="${Common.Business.Flow}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServiceMethod="GetFlowListForMushengRequireCustOnly" ServicePath="FlowMgr.service" Width="250" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlLocation" runat="server" Text="${Menu.MasterData.Location}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbLocaion" runat="server" Visible="true" DescField="Name" ValueField="Code"
                    ServiceMethod="GetLocationListForMushengRequireForCust" ServicePath="LocationMgr.service" Width="250" /> 
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblItem" runat="server" Text="${Common.Business.ItemCode}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" DescField="Description" Width="280"
                    ValueField="Code" ServicePath="FlowMgr.service" ServiceMethod="GetFlowItems"
                    ServiceParameter="string:#tbFlow,string:" />
            </td>
            <td class="td01">
              <%--  <asp:Literal ID="lblDesc" runat="server" Text="${Common.Business.ItemDescription}:" />--%>
            </td>
            <td class="td02">
               <%-- <asp:TextBox ID="tbDesc" runat="server" />--%>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblStartDate" runat="server" Text="${Common.Business.StartDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblEndDate" runat="server" Text="${Common.Business.EndDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01" />
            <td class="t02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" OnClick="btnExport_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset id="fldGv" runat="server">
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" OnRowDataBound="GV_List_RowDataBound">
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
                        <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("Location.Code")%>' ToolTip='<%# Eval("Location.Name")%>'/>
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
        </asp:GridView>
    </div>
</fieldset>