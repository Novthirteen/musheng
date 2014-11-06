<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="ManageSconit_LeanEngine_Multi_Main" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td colspan="3" />
            <td class="ttd02">
                <asp:Button ID="btnRunMrp" runat="server" Text="${Print.Setup.Status.Run}" OnClick="btnRunMrp_Click" />
            </td>
        </tr>
    </table>
</fieldset>
