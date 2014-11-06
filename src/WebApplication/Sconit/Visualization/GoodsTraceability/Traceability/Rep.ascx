<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Rep.ascx.cs" Inherits="Visualization_GoodsTraceability_Traceability_Rep" %>
<div id="floatdiv">
    <div id='floatdivtitle'>
        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="btnClose" />
    </div>
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="true" CellPadding="0"
        AllowSorting="false">
        <Columns>
            <asp:TemplateField HeaderText="Seq">
                <ItemTemplate>
                    <%#Container.DataItemIndex + 1%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
<div id='divHide' />
