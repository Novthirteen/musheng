<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="ManageSconit_ShowLog_Main" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" CssClass="floatright"
                    AutoPostBack="true" OnSelectedIndexChanged="rblType_Change">
                    <asp:ListItem Text="Web" Value="Web" Selected="True" />
                    <asp:ListItem Text="Batch" Value="Batch" />
                </asp:RadioButtonList>
            </td>
            <td class="td02">
                <asp:DropDownList ID="ddlLogFile" runat="server" OnSelectedIndexChanged="ddlLogFile_Change"
                    AutoPostBack="true" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlDateTime" runat="server" Text="DateTime:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbDateTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm',isShowWeek:true})"
                    Width="130" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlLogger" runat="server" Text="Logger:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="ddlLogger" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblLevel" runat="server" Text="Level:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="ddlLevel" runat="server">
                    <asp:ListItem Selected="True" Text="ALL" Value="ALL" />
                    <asp:ListItem Text="DEBUG" Value="DEBUG" />
                    <asp:ListItem Text="INFO" Value="INFO" />
                    <asp:ListItem Text="WARN" Value="WARN" />
                    <asp:ListItem Text="ERROR" Value="ERROR" />
                    <asp:ListItem Text="FATAL" Value="FATAL" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="3" />
            <td class="ttd02">
                <asp:Button ID="btnSearch" runat="server" Text="Search Log" OnClick="btnSearch_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset id="fldList" runat="server" visible="false">
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="false" OnRowDataBound="GV_List_RowDataBound"
        CellPadding="0" Width="100%">
        <Columns>
            <asp:BoundField HeaderText="Date" DataField="Date" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
            <asp:BoundField HeaderText="Thread" DataField="Thread" />
            <asp:BoundField HeaderText="Level" DataField="Level" />
            <asp:BoundField HeaderText="Logger" DataField="Logger" />
            <asp:BoundField HeaderText="Message" DataField="Message" />
            <asp:BoundField HeaderText="Exception" DataField="Exception" />
        </Columns>
    </asp:GridView>
</fieldset>
