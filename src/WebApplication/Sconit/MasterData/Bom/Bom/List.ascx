<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="MasterData_Bom_Bom_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Code"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" OnRowDataBound="GV_List_RowDataBound"
            OnRowEditing="GV_List_RowEditing" OnRowCancelingEdit="GV_List_RowCancelingEdit" OnRowUpdating="GV_List_RowUpdating" >
            <Columns>
                <asp:BoundField ReadOnly="true" DataField="Code" HeaderText="${Common.Business.Code}" SortExpression="Code" />
                <asp:BoundField DataField="Description" HeaderText="${Common.Business.Description}"
                    SortExpression="Description" />
                <asp:TemplateField HeaderText="${Common.Business.Uom}" SortExpression="Uom.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Uom.Code")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtUom" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Uom.Code")%>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Region}" SortExpression="Region.Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Region.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField DataField="IsActive" HeaderText="${Common.Business.IsActive}" SortExpression="IsActive" />
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnEdit1" runat="server" Text="编辑" CommandName="Edit"></asp:LinkButton>
                        <%--<asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Code") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click">
                        </asp:LinkButton>--%>
                        <asp:LinkButton ID="lbnClone" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Code") %>'
                            Text="${Common.Button.Clone}" OnClick="lbtnClone_Click" OnClientClick="return confirm('${Common.Button.Clone.Confirm}')">
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Code") %>'
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')">
                        </asp:LinkButton>
                    </ItemTemplate>
                     <EditItemTemplate>  
                        <asp:LinkButton ID="lbtnUpdate" runat="server" Text="更新" CommandName="Update" />  
                        <asp:LinkButton ID="lbtnCancel" runat="server" Text="取消" CommandName="Cancel" />  
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
