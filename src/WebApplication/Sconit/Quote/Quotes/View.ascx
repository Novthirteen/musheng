<%@ Control Language="C#" AutoEventWireup="true" CodeFile="View.ascx.cs" Inherits="Quote_Quotes_View" %>

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
                <asp:Literal ID="txtInputUserName" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlInputDate" runat="server" Text="${quote.Project.InputDate}:"></asp:Literal>
                <%--<asp:Literal ID="ltInputDate" runat="server"></asp:Literal>--%>
                <asp:Literal ID="txtInputDate" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlPVision" runat="server" Text="${Quote.Project.Vision}:"></asp:Literal>
                <asp:Literal ID="txtPVision" runat="server"></asp:Literal>
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
                <asp:Literal ID="dplCooperationMode" runat="server" ></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlSType" runat="server" Text="${Quote.Project.SType}:"></asp:Literal>
                <asp:Literal ID="dplSType" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlBillPeriod" runat="server" Text="${Quote.Project.BillPeriod}:"></asp:Literal>
                <asp:Literal ID="ltBillPeriod" runat="server"></asp:Literal>天
            </td>
            <td>
                <asp:Literal ID="ltlMonthlyDemand" runat="server" Text="${Quote.Project.MonthlyDemand}"></asp:Literal>
                <asp:Literal ID="txtMonthlyDemand" runat="server"></asp:Literal>套
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlQFor" runat="server" Text="${Quote.Project.QFor}:"></asp:Literal>
                <asp:Literal ID="dplQFor" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlTSType" runat="server" Text="${Quote.Project.TSType}:"></asp:Literal>
                <asp:Literal ID="dplTSType" runat="server"></asp:Literal>
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
                <asp:Literal ID="dplLogisticsMode" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlIsBack" runat="server" Text="${Quote.Project.IsBack}:"></asp:Literal>
                <asp:Literal ID="dplIsBack" runat="server">
                </asp:Literal>
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
            <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" OnClick="btnExport_Click" />
            <asp:Button ID="btnPrint" runat="server" Text="${Common.Button.Print}" Visible="false" />
        </td>
    </tr>
</table>