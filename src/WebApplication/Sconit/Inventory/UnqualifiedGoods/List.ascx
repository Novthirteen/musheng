<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Inventory_UnqualifiedGoods_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>

<script type="text/javascript" language="javascript">
    function CheckAll() {
        var GV_ListId = document.getElementById("<%=GV_List.ClientID %>");
        var allselect = GV_ListId.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked;
        for (i = 1; i < GV_ListId.rows.length; i++) {
            GV_ListId.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = allselect;
        }
    }

</script>

<fieldset>
    <legend>${MasterData.Inventory.InspectOrder.Detail}</legend>
    <div>
        <div class="GridView">
            <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div onclick="CheckAll()">
                                <asp:CheckBox ID="CheckAll" runat="server" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBoxGroup" name="CheckBoxGroup" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Inventory.InspectOrder.InspectNo}">
                        <ItemTemplate>
                            <asp:Label ID="lblInspectNo" runat="server" Text='<%# Bind("InspectOrder.InspectNo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataItemCode%>">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfId" runat="server" Value='<%# Bind("Id") %>' />
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
                    <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataHuId%>" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblHuId" runat="server" Text='<%# Bind("LocationLotDetail.Hu.HuId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataInspectQty%>">
                        <ItemTemplate>
                            <asp:Label ID="lblInspectQty" runat="server" Text='<%# Bind("InspectQty","{0:0.########}") %>' />
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
                    <asp:TemplateField HeaderText="${MasterData.Common.CreateUser}">
                        <ItemTemplate>
                            <asp:Label ID="lblCreaterUser" runat="server" Text='<%# Bind("InspectOrder.CreateUser.Name") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Common.CreateDate}">
                        <ItemTemplate>
                            <asp:Label ID="lblCreaterDate" runat="server" Text='<%# Bind("InspectOrder.CreateDate") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Label ID="lblMessage" runat="server" Visible="false" />
        </div>
    </div>
</fieldset>
<div class="tablefooter">
    <cc1:Button ID="btnPrint" runat="server" Text="${Common.Button.Print}" CssClass="button2"
        OnClick="btnPrint_Click" FunctionId="PrintOrder" />
</div>
