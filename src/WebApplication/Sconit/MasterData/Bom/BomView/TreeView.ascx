﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TreeView.ascx.cs" Inherits="MasterData_Bom_BomView_TreeView" %>
<%@ Register Assembly="Whidsoft.WebControls.OrgChart" Namespace="Whidsoft.WebControls"
    TagPrefix="oc" %>
    <script type="text/javascript" language="javascript">
    if(!$.browser.msie)
    {
       // alert(GetFrameWidth()-70);
        $(document).ready(function(){
          $("#scrollx").attr("width",GetFrameWidth()-70);
        });
        
    }
    </script>
<fieldset runat="server" id="fld">
    <div class="scrollx" style="width: expression((documentElement.clientWidth-70)+'px');
        text-align: center" id="scrollx">
        <table>
            <tr>
                <td>
                    <oc:OrgChart ID="OrgChartBomTreeView" runat="server" ChartStyle="Vertical" Font-Size="X-Small"
                        LineColor="Silver" ImageFolder="Images"></oc:OrgChart>
                </td>
            </tr>
        </table>
    </div>
</fieldset>
