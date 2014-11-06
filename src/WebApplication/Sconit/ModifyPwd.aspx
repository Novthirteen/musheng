<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModifyPwd.aspx.cs" Inherits="ModifyPwd" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>${Security.User.UpdatePassword}</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="divPassword" runat="server">
            <fieldset>
                <legend>${Security.User.FirstLoginUpdatePassword}</legend>
                <table width="100%">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblPassword" runat="server" Text="${Security.User.Password}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbPassword" TextMode="Password" runat="server" EnableViewState="false"
                                Width="250" />
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="${Security.User.Password.Empty}"
                                Display="Dynamic" ControlToValidate="tbPassword" ValidationGroup="vgPassword" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblConfirmPassword" runat="server" Text="${Security.User.ConfirmPassword}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbConfirmPassword" TextMode="Password" runat="server"
                                Width="250" />
                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ErrorMessage="${Security.User.ConfirmPassword.Empty}"
                                Display="Dynamic" ControlToValidate="tbConfirmPassword" ValidationGroup="vgPassword" />
                            <asp:CompareValidator ID="cvPassword" runat="server" ErrorMessage="${Security.User.Password.Different}"                                 ControlToValidate="tbConfirmPassword" ControlToCompare="tbPassword"                                 Operator="Equal" Display="Dynamic" Type="String" ValidationGroup="vgPassword" /> 
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        <div class="buttons">
                            <asp:Button ID="btnUpdatePassword" runat="server" OnClick="btnUpdatePassword_Click"
                                Text="${Common.Button.Save}" CssClass="apply" ValidationGroup="vgPassword" />
                        </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </form>
</body>
</html>
