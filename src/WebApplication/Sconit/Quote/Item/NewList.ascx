<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewList.ascx.cs" Inherits="Quote_Item_NewList" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>

<script type="text/javascript">
    //function GenerateFlowDetail(obj) {
    //    var objId = $(obj).attr("id");
    //    var parentId = objId.substring(0, objId.length - "tbItemCode_suggest".length);
    //    if ($(obj).val() != "") {
    //        Sys.Net.WebServiceProxy.invoke('Webservice/ItemMgrWS.asmx', 'GetParaByCode', false,
    //            {
    //                "itemcode": $(obj).val()
    //            },
    //        function OnSucceeded(result, eventArgs) {
    //            if (result.ItemPack != null) {
    //                $('#' + parentId + 'txtPinNum').attr('value', result.ItemPack.PinNum);
    //                $('#' + parentId + 'txtPinConversion').attr('value', result.ItemPack.PinConversion);
    //            }
    //            else {
    //                $('#' + parentId + 'txtPinNum').attr('value', '');
    //                $('#' + parentId + 'txtPinConversion').attr('value', '');
    //            }
    //            if (result.Desc2 != null) {
    //                $('#' + parentId + 'txtPurchasePrice').attr('value', result.Desc2);
    //            }
    //            else {
    //                $('#' + parentId + 'txtPurchasePrice').attr('value', '');
    //            }
    //        },
    //        function OnFailed(error) {
    //            alert(error.get_message());
    //        }
    //       );
    //    }
    //}

    function txtSingleNumChange(obj) {
        var num = $(obj).val();

        var objId = $(obj).attr("id");
        var parentId = objId.substring(0, objId.length - "txtSingleNum".length);

        var price = $('#' + parentId + 'txtPurchasePrice').val();
        $('#' + parentId + 'txtPrice').attr('value', num * price);

        //加工点数
        var pinC = $('#' + parentId + 'txtPinConversion').val();
        $('#' + parentId + 'txtPoint').attr('value', num * pinC);
    }

    function txtPurchasePriceChange(obj) {
        var price = $(obj).val();

        var objId = $(obj).attr("id");
        var parentId = objId.substring(0, objId.length - "txtPurchasePrice".length);

        var num = $('#' + parentId + 'txtSingleNum').val();
        $('#' + parentId + 'txtPrice').attr('value', num * price);
    }

    function txtPinConversionChange(obj) {
        var pinPC = $(obj).val();

        var objId = $(obj).attr("id");
        var parentId = objId.substring(0, objId.length - "txtPinConversion".length);

        var num = $('#' + parentId + 'txtSingleNum').val();
        $('#' + parentId + 'txtPoint').attr('value', num * pinPC);
    }

    function test(obj) {
        alert(123);
    }
</script>

<fieldset>
    <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                OnRowDataBound="GV_List_RowDataBound" OnRowEditing="GV_List_RowEditing" OnRowCancelingEdit="GV_List_RowCancelingEdit" OnRowUpdating="GV_List_RowUpdating">
        <Columns>
            <asp:TemplateField HeaderText="${Common.GridView.Seq}">
                <ItemTemplate>
                    <asp:Literal ID="ltlNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemMap.Item}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>--%>
                    <%--<uc3:textbox ID="tbItemCode" runat="server" Visible="false" Width="250" DescField="Description"
                                ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"
                                CssClass="inputRequired" InputWidth="150" MustMatch="true" TabIndex="1" />--%>
                    <asp:TextBox ID="txtItemCode" runat="server" Text='<%# Bind("ItemCode") %>' Enabled="false"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Item.Side}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblSide" runat="server" Text='<%# Bind("Side") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtSide" runat="server" Width="50" Text='<%# Bind("Side") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.Supplier}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("Supplier") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtSupplier" runat="server" Text='<%# Bind("Supplier") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.Category}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblCategory" runat="server" Text='<%# Bind("Category") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtCategory" runat="server" Width="50" Text='<%# Bind("Category") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.Brand}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblBrand" runat="server" Text='<%# Bind("Brand") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtBrand" runat="server" Width="50" Text='<%# Bind("Brand") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.Model}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblModel" runat="server" Text='<%# Bind("Model") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtModel" runat="server" Width="50" Text='<%# Bind("Model") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.SingleNum}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblSingleNum" runat="server" Text='<%# Bind("SingleNum") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtSingleNum" runat="server" Width="50" Text='<%# Bind("SingleNum") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.PurchasePrice}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblPurchasePrice" runat="server" Text='<%# Bind("PurchasePrice") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtPurchasePrice" runat="server" Width="50" Text='<%# Bind("PurchasePrice") %>' Enabled="false"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.Price}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtPrice" runat="server" Width="50" Text='<%# Bind("Price") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.PinNum}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblPinNum" runat="server" Text='<%# Bind("PinNum") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtPinNum" runat="server" Width="50" Text='<%# Bind("PinNum") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.PinConversion}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblPinConversion" runat="server" Text='<%# Bind("PinConversion") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtPinConversion" runat="server" Width="50" Text='<%# Bind("PinConversion") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.Point}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblPoint" runat="server" Text='<%# Bind("Point") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtPoint" runat="server" Width="50" Text='<%# Bind("Point") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Item.BitNum}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblBitNum" runat="server" Text='<%# Bind("BitNum") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtBitNum" runat="server" Width="50" Text='<%# Bind("BitNum") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <cc1:LinkButton ID="lbtnAdd" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ProjectId") %>'
                        Text="${Common.Button.New}" OnClick="lbtnAdd_Click" Visible="false" FunctionId="EditOrderDetail"
                        ValidationGroup="vgAdd" TabIndex="1">
                    </cc1:LinkButton>
                    <cc1:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                        Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')"
                        FunctionId="DeleteOrderDetail">
                    </cc1:LinkButton>
                    <asp:LinkButton ID="lbtnEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'></asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton ID="lbtnUpdate" runat="server" Text="更新" CommandName="Update" />
                    <asp:LinkButton ID="lbtnCancel" runat="server" Text="取消" CommandName="Cancel" />
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>