<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="MasterData_Bom_BomDetail_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>

<script language="javascript" type="text/javascript">
    function GenerateBom(obj) {
        var objId = $(obj).attr("id");
        var parentId = objId.substring(0, objId.length - "tbParCode_suggest".length);
        if ($(obj).val() != "") {
            Sys.Net.WebServiceProxy.invoke('Webservice/ItemMgrWS.asmx', 'GenerateItemProxy', false,
                { "itemCode": $(obj).val() },
            function OnSucceeded(result, eventArgs) {
                $('#' + parentId + 'tbParUom').attr('value', result.Uom.Code);
            },
            function OnFailed(error) {
                alert(error.get_message());
            }
           );
        }
    }
    function GenerateComp(obj) {
        var objId = $(obj).attr("id");
        var parentId = objId.substring(0, objId.length - "tbCompCode_suggest".length);
        if ($(obj).val() != "") {
            Sys.Net.WebServiceProxy.invoke('Webservice/ItemMgrWS.asmx', 'GenerateItemProxy', false,
                { "itemCode": $(obj).val() },
            function OnSucceeded(result, eventArgs) {
                $('#' + parentId + 'tbCompUom').attr('value', result.Uom.Code);
            },
            function OnFailed(error) {
                alert(error.get_message());
            }
           );
        }
    }
</script>

<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${Common.Business.Op}" SortExpression="Operation">
                    <ItemTemplate>
                        <asp:Label Width="80" ID="lblOperation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Operation")%>' />
                        <asp:TextBox Width="80" ID="tbOperation" runat="server" Visible="false" Text='<%# Bind("Operation") %>'
                                TabIndex="-1" />
                       <%-- <%# DataBinder.Eval(Container.DataItem, "Operation")%>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bom.Reference}" SortExpression="Reference">
                    <ItemTemplate>
                        <asp:Label Width="80" ID="lblReference" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Reference")%>' />
                        <asp:TextBox Width="80" ID="tbReference" runat="server" Visible="false" Text='<%# Bind("Reference") %>'
                                TabIndex="-1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField DataField="Reference" HeaderText="${MasterData.Bom.Reference}" SortExpression="Reference" />--%>
                <asp:TemplateField HeaderText="${MasterData.Bom.ParCode}" SortExpression="Bom.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblParCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Bom.Code")%>'
                            ToolTip='<%# DataBinder.Eval(Container.DataItem, "Bom.Description")%>' />
                        <uc3:TextBox ID="tbParCode" runat="server" Visible="false" Width="250"  DescField="Description"
                                ValueField="Code" ServicePath="BomMgr.service" ServiceMethod="GetAllBom" Text='<%# DataBinder.Eval(Container.DataItem, "Bom.Code")%>'
                                CssClass="inputRequired" InputWidth="170" MustMatch="true" TabIndex="1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Uom}" SortExpression="Bom.Uom.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblParUom" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Bom.Uom.Code")%>' />
                        <asp:TextBox ID="tbParUom" Width="50" runat="server" Visible="false" Enabled="false" Text='<%# Bind("Bom.Uom.Code") %>'
                                TabIndex="-1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bom.CompCode}" SortExpression="Item.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblCompCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Item.Code")%>'
                            ToolTip='<%# DataBinder.Eval(Container.DataItem, "Item.Description")%>' />
                        <uc3:TextBox ID="tbCompCode" runat="server" Visible="false" Width="250"  DescField="Description" Text='<%# DataBinder.Eval(Container.DataItem, "Item.Code")%>'
                                ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"
                                CssClass="inputRequired" InputWidth="170" MustMatch="true" TabIndex="1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Uom}" SortExpression="Uom.Code">
                    <ItemTemplate>
                        <%--<%# DataBinder.Eval(Container.DataItem, "Uom.Code")%>--%>
                        <asp:Label ID="lblCompUom" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Uom.Code")%>' />
                        <asp:TextBox ID="tbCompUom" Width="50" runat="server" Visible="false" Enabled="false" Text='<%# Bind("Uom.Code") %>'
                                TabIndex="-1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bom.StructureType}" SortExpression="StructureType">
                    <ItemTemplate>
                        <%--<%# DataBinder.Eval(Container.DataItem, "Uom.Code")%>--%>
                        <asp:Label ID="lblStructureType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StructureType")%>' />
                        <uc3:TextBox ID="tbStructureType" runat="server" InputWidth="50" Text='<%# Bind("StructureType")%>' Width="50" DescField="Description" Visible="false"
                                ValueField="Value" ServicePath="CodeMasterMgr.service" ServiceMethod="GetCachedCodeMaster"
                                ServiceParameter="string:BomDetType" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.StartTime}" SortExpression="StartDate">
                    <ItemTemplate>
                        <asp:Label ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StartDate", "{0:yyyy-MM-dd }")%>'  />
                        <asp:TextBox ID="tbStartDate" runat="server" Width="100" Text='<%# DataBinder.Eval(Container.DataItem, "StartDate", "{0:yyyy-MM-dd }")%>' Visible="false" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.EndTime}" SortExpression="EndDate">
                    <ItemTemplate>
                        <asp:Label ID="lblEndDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EndDate", "{0:yyyy-MM-dd}")%>'  />
                        <asp:TextBox ID="tbEndDate" runat="server" Width="100"  Visible="false" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"   Text='<%# DataBinder.Eval(Container.DataItem, "EndDate", "{0:yyyy-MM-dd}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bom.RateQty}" SortExpression="RateQty">
                    <ItemTemplate>
                        <asp:Label ID="lblRateQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RateQty", "{0:0.########}")%>' />
                        <asp:TextBox ID="tbRateQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RateQty", "{0:0.########}")%>' Width="40"  Visible="false" 
                                TabIndex="-1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bom.ScrapPercentage}" SortExpression="ScrapPercentage">
                    <ItemTemplate>
                        <asp:Label ID="lblScrapPercentage" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ScrapPercentage", "{0:0.########}")%>' />
                        <asp:TextBox ID="tbScrapPercentage" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ScrapPercentage", "{0:0.########}")%>' Width="40" Visible="false" 
                                TabIndex="-1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField DataField="StructureType" HeaderText="${MasterData.Bom.StructureType}"
                    SortExpression="StructureType" />
                <asp:BoundField DataField="StartDate" HeaderText="${Common.Business.StartTime}" SortExpression="StartDate"
                    DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:BoundField DataField="EndDate" HeaderText="${Common.Business.EndTime}" SortExpression="EndDate"
                    DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:BoundField DataField="RateQty" HeaderText="${MasterData.Bom.RateQty}" SortExpression="RateQty"
                    DataFormatString="{0:0.########}" />
                <asp:BoundField DataField="ScrapPercentage" HeaderText="${MasterData.Bom.ScrapPercentage}"
                    SortExpression="ScrapPercentage" DataFormatString="{0:0.########}" />--%>
                <asp:TemplateField HeaderText="${MasterData.BomDetail.BackFlushMethod}" SortExpression="BackFlushMethod">
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="lblBackFlushMethod" runat="server" Code="BackFlushMethod"
                            Value='<%# Bind("BackFlushMethod") %>' />
                        <cc1:CodeMstrDropDownList ID="ddlBackFlushMethod" Code="BackFlushMethod" runat="server" Visible="false">
                            </cc1:CodeMstrDropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PositionNo" HeaderText="${MasterData.BomDetail.PositionNo}" SortExpression="PositionNo" />
                <%--<asp:TemplateField HeaderText="${MasterData.Flow.IsShipScanHu}" SortExpression="IsShipScanHu">
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="lblIsShipScanHu" runat="server" Code="TrueOrFalse" Value='<%# Bind("IsShipScanHu") %>' />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="${MasterData.Flow.IsShipScanHu}" SortExpression="IsShipScanHu">
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="lblIsShipScanHu" runat="server" Code="TrueOrFalse" Value='<%# Bind("IsShipScanHu") %>' />
                        <asp:CheckBox ID="cbIsShipScanHu" runat="server" Visible="false"  Checked='<%# Eval("IsShipScanHu") %>'  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bom.NeedPrint}" SortExpression="NeedPrint">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbNeedPrint" runat="server" Enabled="false"  Checked='<%# Eval("NeedPrint") %>'   />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:CheckBoxField DataField="NeedPrint" HeaderText="${MasterData.Bom.NeedPrint}"
                    SortExpression="NeedPrint" />--%>
                <asp:TemplateField HeaderText="${Common.Business.Location}" Visible="false" SortExpression="Location.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Location.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bom.Priority}" Visible="false" SortExpression="Priority">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Priority")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnSave" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                            Text="${Common.Button.Save}" Visible="false" OnClick="lbtnSave_Click">
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnCancel" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                            Text="${Common.Button.Cancel}" Visible="false" OnClick="lbtnCancel_Click">
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click">
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnView" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.View}" OnClick="lbtnView_Click">
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
