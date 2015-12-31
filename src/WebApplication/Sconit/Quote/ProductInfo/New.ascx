<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Quote_ProductInfo_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlProjectId" runat="server" Text="${Quote.Tooling.ProjectNo}:" />
            </td>
            <td class="ttd02">
                <uc3:textbox ID="txtProjectId" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Descr" ValueField="ID" ServicePath="OrderProductionPlanMgr.service" ServiceMethod="GetGPID" ServiceParameter="bool:true" />
            </td>
            <td class="ttd01"></td>
            <td class="ttd02"></td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlCustomerName" runat="server" Text="${Quote.ProductInfo.CustomerName}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <uc3:textbox ID="txtCustomer" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Name" ValueField="Code" ServicePath="OrderProductionPlanMgr.service" ServiceMethod="GetCustomer" />
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlProductName" runat="server" Text="${Quote.ProductInfo.ProductName}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtProductName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlProductNo" runat="server" Text="${Quote.ProductInfo.ProductNo}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtProductNo" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlVersionNo" runat="server" Text="${Quote.ProductInfo.VersionNo}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtVersionNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlPT" runat="server" Text="${Quote.ProductInfo.PT}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlPT" runat="server"></asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlPCBNum" runat="server" Text="${Quote.ProductInfo.PCBNum}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtPCBNum" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlAdvisePCBNum" runat="server" Text="${Quote.ProductInfo.AdvisePCBNum}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtAdvisePCBNum" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlDoubleSideMount" runat="server" Text="${Quote.ProductInfo.DoubleSideMount}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlDoubleSideMount" runat="server">
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlChipBurning" runat="server" Text="${Quote.ProductInfo.ChipBurning}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlChipBurning" runat="server">
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlBurningNum" runat="server" Text="${Quote.ProductInfo.BurningNum}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtBurningNum" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlLightNum" runat="server" Text="${Quote.ProductInfo.LightNum}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtLightNum" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlBoardMode" runat="server" Text="${Quote.ProductInfo.BoardMode}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlBoardMode" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlConnPoint" runat="server" Text="${Quote.ProductInfo.ConnPoint}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtConnPoint" runat="server"></asp:TextBox>
            </td>
            
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlDeviceShaping" runat="server" Text="${Quote.ProductInfo.DeviceShaping}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlDeviceShaping" runat="server">
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlShapingType" runat="server" Text="${Quote.ProductInfo.ShapingType}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtShapingType" runat="server"></asp:TextBox>
            </td>
            
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlShapingSecCount" runat="server" Text="${Quote.ProductInfo.ShapingSecCount}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtShapingSecCount" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01"></td>
            <td class="ttd02"></td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlDeviceCoding" runat="server" Text="${Quote.ProductInfo.DeviceCoding}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlDeviceCoding" runat="server">
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlCodingType" runat="server" Text="${Quote.ProductInfo.CodingType}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtCodingType" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlCodingSecCount" runat="server" Text="${Quote.ProductInfo.CodingSecCount}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtCodingSecCount" runat="server"></asp:TextBox>
            </td>
            
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlFCTTest" runat="server" Text="${Quote.ProductInfo.FCTTest}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlFCTTest" runat="server">
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlTestSec" runat="server" Text="${Quote.ProductInfo.TestSec}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtTestSec" runat="server"></asp:TextBox>
            </td>
            
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlProductAssembly" runat="server" Text="${Quote.ProductInfo.ProductAssembly}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlProductAssembly" runat="server">
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlAssemblySec" runat="server" Text="${Quote.ProductInfo.AssemblySec}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtAssemblySec" runat="server"></asp:TextBox>
            </td>
            
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlFinalAssemblyTest" runat="server" Text="${Quote.ProductInfo.FinalAssemblyTest}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlFinalAssemblyTest" runat="server">
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlFinalTestSec" runat="server" Text="${Quote.ProductInfo.FinalTestSec}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtFinalTestSec" runat="server"></asp:TextBox>
            </td>
            
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlSpecialReq" runat="server" Text="${Quote.ProductInfo.SpecialReq}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtSpecialReq" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01"></td>
            <td class="ttd02"></td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlSurfaceCoating" runat="server" Text="${Quote.ProductInfo.SurfaceCoating}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlSurfaceCoating" runat="server">
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlMaterialNo" runat="server" Text="${Quote.ProductInfo.MaterialNo}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtMaterialNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlCoatingAcreage" runat="server" Text="${Quote.ProductInfo.CoatingAcreage}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtCoatingAcreage" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlCoatingSec" runat="server" Text="${Quote.ProductInfo.CoatingSec}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtCoatingSec" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlProductFilling" runat="server" Text="${Quote.ProductInfo.ProductFilling}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlProductFilling" runat="server">
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlFillingPrice" runat="server" Text="${Quote.ProductInfo.FillingPrice}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtFillingPrice" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlPackMode" runat="server" Text="${Quote.ProductInfo.PackMode}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlPackMode" runat="server"></asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlOutBox" runat="server" Text="${Quote.ProductInfo.OutBox}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlOutBox" runat="server" OnSelectedIndexChanged="ddlOutBox_Click" AutoPostBack="true"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlOutBoxPrice" runat="server" Text="${Quote.ProductInfo.OutBoxPrice}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtOutBoxPrice" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlPlate" runat="server" Text="${Quote.ProductInfo.Plate}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlPlate" runat="server" OnSelectedIndexChanged="ddlPlate_Click" AutoPostBack="true"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlPlateNum" runat="server" Text="${Quote.ProductInfo.PlateNum}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtPlateNum" runat="server" OnTextChanged="txtPlateNum_OnChangeClick" AutoPostBack="true"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlPlatePrice" runat="server" Text="${Quote.ProductInfo.PlatePrice}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtPlatePrice" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlPartition" runat="server" Text="${Quote.ProductInfo.Partition}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlPartition" runat="server" OnSelectedIndexChanged="ddlPartition_Click" AutoPostBack="true"></asp:DropDownList>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlPartitionNum" runat="server" Text="${Quote.ProductInfo.PartitionNum}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtPartitionNum" runat="server" OnTextChanged="txtPartitionNum_ChangeClick" AutoPostBack="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlPartitionPrice" runat="server" Text="${Quote.ProductInfo.PartitionPrice}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtPartitionPrice" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlBubbleBag" runat="server" Text="${Quote.ProductInfo.BubbleBag}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlBubbleBag" runat="server" OnSelectedIndexChanged="ddlBubbleBag_Click" AutoPostBack="true"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlBubbleBagPrice" runat="server" Text="${Quote.ProductInfo.BubbleBagPrice}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtBubbleBagPrice" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlBlister" runat="server" Text="${Quote.ProductInfo.Blister}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:DropDownList ID="ddlBlister" runat="server" OnSelectedIndexChanged="ddlBlister_Click" AutoPostBack="true"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlBlisterNum" runat="server" Text="${Quote.ProductInfo.BlisterNum}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtBlisterNum" runat="server" OnTextChanged="txtBlisterNum_ChangeClick" AutoPostBack="true"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlBlisterPrice" runat="server" Text="${Quote.ProductInfo.BlisterPrice}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtBlisterPrice" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlFCLNum" runat="server" Text="${Quote.ProductInfo.FCLNum}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtFCLNum" runat="server" OnTextChanged="txtFCLNum_ChangeClick" AutoPostBack="true"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlDeliveryAdd" runat="server" Text="${Quote.ProductInfo.DeliveryAdd}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtDeliveryAdd" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlLogisticsFee" runat="server" Text="${Quote.ProductInfo.LogisticsFee}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtLogisticsFee" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlLogisticsCost" runat="server" Text="${Quote.ProductInfo.LogisticsCost}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtLogisticsCost" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlOutBoxResult" runat="server" Text="${Quote.ProductInfo.OutBox}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtOutBoxResult" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlPlateResult" runat="server" Text="${Quote.ProductInfo.Plate}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtPlateResult" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlPartitionResult" runat="server" Text="${Quote.ProductInfo.Partition}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtPartitionResult" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlBubbleBagResult" runat="server" Text="${Quote.ProductInfo.BubbleBag}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtBubbleBagResult" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlBlisterResult" runat="server" Text="${Quote.ProductInfo.Blister}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtBlisterResult" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlSource" runat="server" Text="${Quote.ProductInfo.Source}:"></asp:Literal>
            </td>
            <td class="ttd02">
                <asp:TextBox ID="txtSource" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlRemark1" runat="server" Text="${Quote.ProductInfo.Remark1}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtRemark1" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlRemark2" runat="server" Text="${Quote.ProductInfo.Remark2}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtRemark2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01"></td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
            </td>
        </tr>
    </table>
</fieldset>

