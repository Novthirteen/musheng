<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Reports_InvParty_Main" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                  <asp:Literal ID="lblParty" runat="server" Text="${Common.Business.Flow}:" />
            </td>
            <td class="td02">              
              <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServiceMethod="GetFlowListForMushengRequirePurOnly" ServicePath="FlowMgr.service" Width="250" />            </td>
            <td class="td01">
             <asp:Literal ID="ltlLocation" runat="server" Text="${Menu.MasterData.Location}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbLocaion" runat="server" Visible="true" DescField="Name" ValueField="Code"
                    ServiceMethod="GetAllLocation" ServicePath="LocationMgr.service" Width="250" /> 
            </td>
        </tr>
        <tr id="trLocation" runat="server">
            <td class="td01">
                <asp:Literal ID="lblItem" runat="server" Text="${Common.Business.ItemCode}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" DescField="Description" Width="280"
                    ValueField="Code" ServicePath="FlowMgr.service" ServiceMethod="GetFlowItems"
                    ServiceParameter="string:#tbFlow,string:" />
            </td>
            <td class="td01">
               <%-- <asp:Literal ID="lblDesc" runat="server" Text="${Common.Business.ItemDescription}:" />--%>
            </td>
            <td class="td02">
              <%--  <asp:TextBox ID="tbDesc" runat="server" />--%>
            </td>
        </tr>
        <tr>
            <td />
            <td />
            <td />
            <td class="t02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" CssClass="button2"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                    OnClick="btnExport_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset id="fldGv" runat="server">
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" OnRowDataBound="GV_List_RowDataBound"
        AllowSorting="false">
        <Columns>
            <asp:TemplateField HeaderText="${Common.Business.ItemCode}" SortExpression="Item.Code">
                <ItemTemplate>
                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item.Code")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.ItemDescription}" SortExpression="Item.Description">
                <ItemTemplate>
                    <asp:Label ID="lblItemDescription" runat="server" Text='<%# Eval("Item.Description")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Location}" SortExpression="Location.Code">
                <ItemTemplate>
                    <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location.Code")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.Location.Name}" SortExpression="Location.Name">
                <ItemTemplate>
                    <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("Location.Name")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Uom}">
                <ItemTemplate>
                    <asp:Label ID="lblUOM" runat="server" Text='<%# Eval("Item.Uom.Code")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Qty" HeaderText="${Reports.Inv}" DataFormatString="{0:0.###}" />
        </Columns>
    </asp:GridView>
</fieldset>
