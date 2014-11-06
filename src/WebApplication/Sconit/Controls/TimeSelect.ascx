<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimeSelect.ascx.cs" Inherits="Controls_TimeSelect" %>
<table>
    <tr>
        <td>
            <asp:DropDownList runat="server" ID="ddlHour">
                <asp:ListItem Selected="True" Text="0" Value="0" />
                <asp:ListItem Text="1" Value="01" />
                <asp:ListItem Text="2" Value="02" />
                <asp:ListItem Text="3" Value="03" />
                <asp:ListItem Text="4" Value="04" />
                <asp:ListItem Text="5" Value="05" />
                <asp:ListItem Text="6" Value="06" />
                <asp:ListItem Text="7" Value="07" />
                <asp:ListItem Text="8" Value="08" />
                <asp:ListItem Text="9" Value="09" />
                <asp:ListItem Text="10" Value="10" />
                <asp:ListItem Text="11" Value="11" />
                <asp:ListItem Text="12" Value="12" />
                <asp:ListItem Text="13" Value="13" />
                <asp:ListItem Text="14" Value="14" />
                <asp:ListItem Text="15" Value="15" />
                <asp:ListItem Text="16" Value="16" />
                <asp:ListItem Text="17" Value="17" />
                <asp:ListItem Text="18" Value="18" />
                <asp:ListItem Text="19" Value="19" />
                <asp:ListItem Text="20" Value="20" />
                <asp:ListItem Text="21" Value="21" />
                <asp:ListItem Text="22" Value="22" />
                <asp:ListItem Text="23" Value="23" />
            </asp:DropDownList>
        </td>
        <td>
            <asp:Label ID="lblHour" runat="server" Text="时" />
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlMinute">
                <asp:ListItem Selected="True" Text="0" Value="0" />
                <asp:ListItem Text="5" Value="05" />
                <asp:ListItem Text="10" Value="10" />
                <asp:ListItem Text="15" Value="15" />
                <asp:ListItem Text="20" Value="20" />
                <asp:ListItem Text="25" Value="25" />
                <asp:ListItem Text="30" Value="30" />
                <asp:ListItem Text="35" Value="35" />
                <asp:ListItem Text="40" Value="40" />
                <asp:ListItem Text="45" Value="45" />
                <asp:ListItem Text="50" Value="50" />
                <asp:ListItem Text="55" Value="55" />
            </asp:DropDownList>
        </td>
        <td>
            <asp:Label ID="lblMinute" runat="server" Text="分" />
        </td>
    </tr>
</table>
