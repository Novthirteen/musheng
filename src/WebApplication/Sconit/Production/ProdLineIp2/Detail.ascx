<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Detail.ascx.cs" Inherits="Production_ProdLineIp2_Detail" %>
<div id="floatdiv">
    <div id='floatdivtitle'>
        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="btnClose" />
    </div>
    产出:<asp:Literal ID="totalfg" runat="server" />
    <br />
    <asp:GridView ID="GV_List_FG" runat="server" AutoGenerateColumns="true" OnRowDataBound="GV_List_FG_RowDataBound"
        CellPadding="0" AllowSorting="false">
        <Columns>
            <asp:TemplateField HeaderText="Seq">
                <ItemTemplate>
                    <%#Container.DataItemIndex + 1%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    投入:<asp:Literal ID="totalrm" runat="server" />
    <br />
    <asp:GridView ID="GV_List_RM" runat="server" AutoGenerateColumns="true" OnRowDataBound="GV_List_RM_RowDataBound"
        CellPadding="0" AllowSorting="false">
        <Columns>
            <asp:TemplateField HeaderText="Seq">
                <ItemTemplate>
                    <%#Container.DataItemIndex + 1%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Button ID="btnBack1" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
</div>

<div id='divHide' />