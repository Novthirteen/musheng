<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Distribution_PickList_Main" %>
<%@ Register Src="Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="PickListInfo.ascx" TagName="PickListInfo" TagPrefix="uc2" %>
<%@ Register Src="TabNavigator.ascx" TagName="TabNavigator" TagPrefix="uc2" %>
<%@ Register Src="Batch/Main.ascx" TagName="Batch" TagPrefix="uc2" %>

<uc2:TabNavigator ID="ucTabNavigator" runat="server" Visible="true" />
<div class="ajax__tab_body">
    <table class="mtable">
        <tr>
            <td>
                <uc2:Search ID="ucSearch" runat="server" Visible="true" />
                <uc2:List ID="ucList" runat="server" Visible="false" />
                <uc2:PickListInfo ID="ucPickList" runat="server" Visible="false" />
                <uc2:Batch ID="ucBatch" runat="server" Visible="false" />
            </td>
        </tr>
    </table>
</div>
</div> 