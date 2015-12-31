<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SubmitView.ascx.cs" Inherits="Quote_Quotes_SubmitView" %>

<script type="text/javascript">
    function test() {
        var a = $(<%=hfNum.ClientID%>).val();
        var strsNum = "";
        var strsPrice = "";
        var strsCountPrice = "";
        var strsAmount = "";
        for (var i = 1; i <= a; i++) {
            strsNum += $("#txtNum" + i).val() + ",";
            strsPrice += $("#txtPrice" + i).val() + ",";
            strsCountPrice += $("#txtCountPrice" + i).val() + ",";

            var amount = $(".lbAmount" + i);
            if (typeof (amount.attr("class")) == "undefined") {
            }
            else {
                strsAmount += amount.text() + ",";
            }
        }
        
        $(<%=hfNumList.ClientID%>).val(strsNum.substring(0, strsNum.length - 1));
        $(<%=hfPriceList.ClientID%>).val(strsPrice.substring(0, strsPrice.length - 1));
        $(<%=hfCountPriceList.ClientID%>).val(strsCountPrice.substring(0, strsCountPrice.length - 1));
        $(<%=hfAmount.ClientID%>).val(strsAmount.substring(0, strsAmount.length - 1));
    }

    function NumberChange(obj) {
        var id = obj.id;
        var num = id.replace(/[^0-9]/ig, '');
        var number = obj.value;
        var price = $("#txtPrice" + num).val();
        $("#txtCountPrice" + num).val(number * price);

        changeAmount(num);
    }
    function PriceChange(obj) {
        var id = obj.id;
        var num = id.replace(/[^0-9]/ig, '');
        var number = $("#txtNum" + num).val();
        var price = obj.value;
        $("#txtCountPrice" + num).val(number * price);

        changeAmount(num);
    }

    function changeAmount(num) {
        var rows = $(<%=hfNum.ClientID%>).val();
        var arr = new Array();
        for (var i = 1; i <= rows; i++) {
            var amount = $(".lbAmount" + i);
            if (typeof (amount.attr("class")) == "undefined") {
            }
            else {
                arr.push(amount.attr("class"));
            }
        }
        for (var ii = 0; ii < arr.length; ii++) {
            var cnum = $("." + arr[ii]).attr("class").replace(/[^0-9]/ig, '');
            if (num <= cnum) {
                var numstart;
                var newamount = 0;
                if (ii == 0) {
                    numstart = 1;
                }
                else {
                    numstart = parseInt($("." + arr[ii - 1]).attr("class").replace(/[^0-9]/ig, '')) + 1;
                }
                for (var j = numstart; j <= cnum; j++) {
                    var cp = $("#txtCountPrice" + j).val();
                    newamount = parseFloat(newamount) + parseFloat(cp);
                }
                $("." + arr[ii]).text(newamount.toFixed(4))
            }

        }
    }
</script>
<fieldset runat="server">
    <table class="mtable">
        <tr>
            <td>
                <asp:Literal ID="ltlCustomerName" runat="server" Text="${Quote.ProductInfo.CustomerName}:">
                </asp:Literal><asp:Literal ID="ltCustomerName" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlInputUserName" runat="server" Text="${quote.Project.InputUserName}:"></asp:Literal>
                <%--<asp:Literal ID="ltInputUserName" runat="server" ></asp:Literal>--%>
                <asp:TextBox ID="txtInputUserName" runat="server" Width="100"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="ltlInputDate" runat="server" Text="${quote.Project.InputDate}:"></asp:Literal>
                <%--<asp:Literal ID="ltInputDate" runat="server"></asp:Literal>--%>
                <asp:TextBox ID="txtInputDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="ltlPVision" runat="server" Text="${Quote.Project.Vision}:"></asp:Literal>
                <asp:TextBox ID="txtPVision" runat="server" Width="80"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlProductName" runat="server" Text="${Quote.ProductInfo.ProductName}:"></asp:Literal>
                <asp:Literal ID="ltProductName" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlProductNo" runat="server" Text="${Quote.ProductInfo.ProductNo}:"></asp:Literal>
                <asp:Literal ID="ltProductNo" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlVersionNo" runat="server" Text="${Quote.ProductInfo.VersionNo}:"></asp:Literal>
                <asp:Literal ID="ltVersionNo" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlDeliveryAdd" runat="server" Text="${Quote.ProductInfo.DeliveryAdd}:"></asp:Literal>
                <asp:Literal ID="ltDeliveryAdd" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlCooperationMode" runat="server" Text="${Quote.Project.CooperationMode}:"></asp:Literal>
                <asp:DropDownList ID="dplCooperationMode" runat="server" AutoPostBack="true"></asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="ltlSType" runat="server" Text="${Quote.Project.SType}:"></asp:Literal>
                <asp:DropDownList ID="dplSType" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="ltlBillPeriod" runat="server" Text="${Quote.Project.BillPeriod}:"></asp:Literal>
                <asp:Literal ID="ltBillPeriod" runat="server"></asp:Literal>天
            </td>
            <td>
                <asp:Literal ID="ltlMonthlyDemand" runat="server" Text="${Quote.Project.MonthlyDemand}"></asp:Literal>
                <asp:TextBox ID="txtMonthlyDemand" runat="server" Width="80"></asp:TextBox>套
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlQFor" runat="server" Text="${Quote.Project.QFor}:"></asp:Literal>
                <asp:DropDownList ID="dplQFor" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="ltlTSType" runat="server" Text="${Quote.Project.TSType}:"></asp:Literal>
                <asp:DropDownList ID="dplTSType" runat="server" AutoPostBack="true" OnTextChanged="dplTSType_Changed"></asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="ltlPlanAllocationNum" runat="server" Text="${Quote.Project.PlanAllocationNum}:"></asp:Literal>
                <asp:Literal ID="ltPlanAllocationNum" runat="server"></asp:Literal>套
            </td>
            <td>
                <asp:Literal ID="ltlPT" runat="server" Text="${Quote.ProductInfo.PT}:"></asp:Literal>
                <asp:Literal ID="ltPT" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlPCBNum" runat="server" Text="${Quote.ProductInfo.PCBNum}:"></asp:Literal>
                <asp:Literal ID="ltPCBNum" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlDoubleSideMount" runat="server" Text="${Quote.ProductInfo.DoubleSideMount}:"></asp:Literal>
                <asp:Literal ID="ltDoubleSideMount" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlChipBurning" runat="server" Text="${Quote.Project.ChipBurning}:"></asp:Literal>
                <asp:Literal ID="ltChipBurning" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlLightNum" runat="server" Text="${Quote.ProductInfo.LightNum}:"></asp:Literal>
                <asp:Literal ID="ltLightNum" runat="server"></asp:Literal>颗
            </td>
            <td>
                <asp:Literal ID="ltlBoardMode" runat="server" Text="${Quote.ProductInfo.BoardMode}:"></asp:Literal>
                <asp:Literal ID="ltBoardMode" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlConnPoint" runat="server" Text="${Quote.ProductInfo.ConnPoint}:"></asp:Literal>
                <asp:Literal ID="ltConnPoint" runat="server"></asp:Literal>个
            </td>
            <td>
                <asp:Literal ID="ltlDeviceShaping" runat="server" Text="${Quote.Project.DeviceShaping}:"></asp:Literal>
                <asp:Literal ID="ltDeviceShaping" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlDeviceCoding" runat="server" Text="${Quote.ProductInfo.DeviceCoding}:"></asp:Literal>
                <asp:Literal ID="ltDeviceCoding" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlCodingType" runat="server" Text="${Quote.ProductInfo.CodingType}:"></asp:Literal>
                <asp:Literal ID="ltCodingType" runat="server"></asp:Literal>种
            </td>
            <td>
                <asp:Literal ID="ltlSurfaceCoating" runat="server" Text="${Quote.ProductInfo.SurfaceCoating}:"></asp:Literal>
                <asp:Literal ID="ltSurfaceCoating" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlCoatingAcreage" runat="server" Text="${Quote.ProductInfo.CoatingAcreage}:"></asp:Literal>
                <asp:Literal ID="ltCoatingAcreage" runat="server"></asp:Literal>cm²
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlPackMode" runat="server" Text="${Quote.ProductInfo.PackMode}:"></asp:Literal>
                <asp:Literal ID="ltPackMode" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlEachBox" runat="server" Text="${Quote.Project.EachBox}:"></asp:Literal>
                <asp:Literal ID="ltEachBox" runat="server"></asp:Literal>块
            </td>
            <td>
                <asp:Literal ID="ltlLogisticsMode" runat="server" Text="${Quote.Project.LogisticsMode}:"></asp:Literal>
                <asp:DropDownList ID="dplLogisticsMode" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:Literal ID="ltlIsBack" runat="server" Text="${Quote.Project.IsBack}:"></asp:Literal>
                <asp:DropDownList ID="dplIsBack" runat="server">
                    <asp:ListItem Value="是">是</asp:ListItem>
                    <asp:ListItem Value="否">否</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlSalesUP" runat="server" Text="${Quote.Project.SalesUP}:"></asp:Literal>
                <asp:Literal ID="ltSalesUP" runat="server"></asp:Literal>
                <asp:Literal ID="ltlRMB" runat="server" Text="${Quote.Project.RMB}"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlSalesUPI" runat="server" Text="${Quote.Project.SalesUPI}:"></asp:Literal>
                <asp:Literal ID="ltSalesUPI" runat="server"></asp:Literal>
                <asp:Literal ID="ltlRMBI" runat="server" Text="${Quote.Project.RMB}"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlLumpSumFee" runat="server" Text="${Quote.Project.LumpSumFee}:"></asp:Literal>
                <asp:Literal ID="ltLumpSumFee" runat="server"></asp:Literal>
                <asp:Literal ID="ltlRMB1" runat="server" Text="${Quote.Project.RMB}"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlLumpSumFeeI" runat="server" Text="${Quote.Project.LumpSumFeeI}:"></asp:Literal>
                <asp:Literal ID="ltLumpSumFeeI" runat="server"></asp:Literal>
                <asp:Literal ID="ltlRMB2" runat="server" Text="${Quote.Project.RMB}"></asp:Literal>
            </td>
        </tr>
    </table>
</fieldset>
<%=OutHtml() %>
<table class="mtable">
    <tr>
        <td class="td01"></td>
        <td class="td02"></td>
        <td class="td01"></td>
        <td class="td02">
            <asp:Button ID="btnTest" runat="server" OnClientClick="test()" OnClick="btnTest_Click" Text="${Common.Button.Save}" />
            <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" OnClick="btnExport_Click" />
            <asp:Button ID="btnPrint" runat="server" Text="${Common.Button.Print}" Visible="false" />
            <asp:Button ID="btnCopy" runat="server" Text="Copy" OnClick="btnCopy_Click" />
            <asp:Button ID="btnExportToPDF" runat="server" Text="导出PDF" OnClick="btnExportToPDF_Click" />
        </td>
    </tr>
</table>
<asp:HiddenField ID="hfNum" runat="server" />
<asp:HiddenField ID="hfNumList" runat="server" />
<asp:HiddenField ID="hfPriceList" runat="server" />
<asp:HiddenField ID="hfCountPriceList" runat="server" />
<asp:HiddenField ID="hfAmount" runat="server" />

