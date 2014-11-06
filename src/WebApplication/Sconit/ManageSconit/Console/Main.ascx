<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="ManageSconit_Console_Main" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div class="AjaxClass  ajax__tab_default">
       <div class="ajax__tab_header">
        <span class='ajax__tab_active' id='tab_Predefined' runat="server"><span class='ajax__tab_outer'><span class='ajax__tab_inner'><span class='ajax__tab_tab'><asp:LinkButton ID="lbPredefined" Text="${ManageSconit.Console.Predefined}" runat="server" OnClick="lbPredefined_Click" /></span></span></span></span><span id='tab_Custom' runat="server" visible="false"><span 
        class='ajax__tab_outer'><span class='ajax__tab_inner'><span class='ajax__tab_tab'><asp:LinkButton ID="lbCustom" Text="${ManageSconit.Console.Custom}" runat="server" OnClick="lbCustom_Click" /></span></span></span></span>
    </div>
<div class="ajax__tab_body">
<fieldset>
    <div id="tblpredefined" runat="server">
        <table>
            <tr>
                <td class="ttd01">
                    ${ManageSconit.Console.Param}1:
                </td>
                <td class="ttd02">
                    <asp:TextBox ID="tbParam1" runat="server" Width="100" />
                </td>
                <td class="ttd01">
                    ${ManageSconit.Console.Param}2:
                </td>
                <td class="ttd02">
                    <asp:TextBox ID="tbParam2" runat="server" Width="100" />
                </td>
                <td class="ttd01">
                    ${ManageSconit.Console.Param}3:
                </td>
                <td class="ttd02">
                    <asp:TextBox ID="tbParam3" runat="server" Width="100" />
                </td>
                <td class="ttd01">
                    ${ManageSconit.Console.Param}4:
                </td>
                <td class="ttd02">
                    <asp:TextBox ID="tbParam4" runat="server" Width="100" />
                </td>
                <td class="ttd01">
                    ${ManageSconit.Console.Param}5:
                </td>
                <td class="ttd02">
                    <asp:TextBox ID="tbParam5" runat="server" Width="100" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="GV_List_Sql" runat="server" AutoGenerateColumns="false" OnRowDataBound="GV_List_Sql_RowDataBound"
            CellPadding="0">
            <Columns>
                <asp:BoundField DataField="Seq" HeaderText="${Common.GridView.Seq}" />
                <asp:TemplateField HeaderText="Sql">
                    <ItemTemplate>
                    <asp:TextBox ID="tbSql" Rows="2" Columns="55" MaxLength="450" TextMode="MultiLine" Font-Size="8"
                    Text='<%# DataBinder.Eval(Container.DataItem, "Sql")%>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Description}">
                    <ItemTemplate>
                       <asp:TextBox ID="tbDescription" Rows="2" Columns="50" MaxLength="450" TextMode="MultiLine" Font-Size="8"
                       Text='<%# DataBinder.Eval(Container.DataItem, "Description")%>' runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField DataField="Sql" HeaderText="Sql" />
                <asp:BoundField DataField="Description" HeaderText="${Common.Business.Description}" />--%>
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnRun" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Sql") %>'
                            Text="${Common.Button.Run}" OnClick="lbtnRun_Click" />
                        <asp:LinkButton ID="ltnExport" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Sql") %>'
                            Text="${Common.Button.Export}" OnClick="lbtnExport_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <table id="tblcustom" runat="server">
        <tr>
            <td class="td01">
                <asp:TextBox ID="tbMemo" runat="server" Text='<%# Bind("Memo") %>' Height="100" Width="800" Font-Size="10"
                    TextMode="MultiLine"></asp:TextBox>
            </td>
            <td class="td02" style="vertical-align: bottom">
                <asp:Button ID="btnRun" runat="server" Text="${Common.Button.Run}" OnClick="btnRun_Click" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" OnClick="btnRun_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset runat="server" id="fld_Gv_List">
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="true" OnRowDataBound="GV_List_RowDataBound"
        CellPadding="0" AllowSorting="false">
        <Columns>
            <asp:TemplateField HeaderText="Seq">
                <ItemTemplate>
                    <%#Container.DataItemIndex + 1%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>
</div> </div>