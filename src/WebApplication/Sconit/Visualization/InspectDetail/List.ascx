<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Visualization_InspectDetail_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <legend>${MasterData.Inventory.InspectOrder.Detail}</legend>
    <div>
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" DefaultSortExpression="Id" DefaultSortDirection="Descending">
            <Columns>
                <asp:TemplateField HeaderText="${MasterData.Inventory.InspectOrder.InspectNo}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("InspectOrder.InspectNo") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataItemCode%>">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("LocationLotDetail.Item.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataItemDesc%>">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("LocationLotDetail.Item.Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataUom%>">
                    <ItemTemplate>
                        <asp:Label ID="lblUom" runat="server" Text='<%# Bind("LocationLotDetail.Item.Uom.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataUnitCount%>">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCount" runat="server" Text='<%# Bind("LocationLotDetail.Item.UnitCount","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataLocationFrom%>">
                    <ItemTemplate>
                        <asp:Label ID="lblLocationFrom" runat="server" Text='<%# Bind("LocationFrom.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataLocationTo%>">
                    <ItemTemplate>
                        <asp:Label ID="lblLocationTo" runat="server" Text='<%# Bind("LocationTo.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataHuId%>">
                    <ItemTemplate>
                        <asp:Label ID="lblHuId" runat="server" Text='<%# Bind("LocationLotDetail.Hu.HuId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataInspectQty%>">
                    <ItemTemplate>
                        <asp:Label ID="lblInspectQty" runat="server" Text='<%# Bind("InspectQty","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataQualifiedQty%>">
                    <ItemTemplate>
                        <asp:Label ID="lblQualifiedQty" runat="server" Text='<%# Bind("QualifiedQty","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataRejectedQty%>">
                    <ItemTemplate>
                        <asp:Label ID="lblRejectedQty" runat="server" Text='<%# Bind("RejectedQty","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataDisposition%>">
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="lblDisposition" runat="server" Code="Disposition" Value='<%# Bind("Disposition") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Distribution.PickList.CreateDate}">
                    <ItemTemplate>
                        <asp:Label ID="lblCreateDate" runat="server" Text='<%# Bind("InspectOrder.CreateDate") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
