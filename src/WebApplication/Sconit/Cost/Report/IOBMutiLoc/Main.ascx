<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Cost_Report_IOBMutiLoc_Main" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<style type="text/css">
        .li_singula {
            /*border: solid 2px red;*/
            list-style-type:none; 
            text-align:left;
            border-bottom-width: 0px;
            background-color:#D4D4D4;
        }

        .li_double {
            /*border: solid 2px red;*/
            list-style-type:none; 
            text-align:left;
            border-bottom-width: 0px;
        }

    .li_mouseOver {
        list-style-type:none; 
            text-align:left;
            border-bottom-width: 0px;
            background-color:#808AF4;
    }



    </style>

<script type="text/javascript">

    var onMouseover = 0;
    var onFocus = 0;
    function doMouseOver()
    {
        onMouseover = 1;
     }



    function onblurClick(e) {
        onFocus = 0;
        if (onMouseover == 0) {
            $("#ctl01_demo").hide();
        } 
    }
    function onMouse(e) {
        onMouseover = 1;
        $(e).attr("class", "li_mouseOver");
    }
    function outMouse_singula(e){
        onMouseover = 1;
        $(e).attr("class", "li_singula");
    }

    function outMouse_double(e){
        $(e).attr("class", "li_double");
    }

    $(function () {
        $("#ctl01_demo").hide();
        $("body").click(function () {
            var mouseOverLi = $(".li_mouseOver");
                if (mouseOverLi.length == 0) {
                    $("#ctl01_demo").hide();
                }
        });
    });

    function textChange(e)
    {
        onFocus = 1;
        $("li").html("");
        var s = document.getElementById('ctl01_ulList');
        var checkName = $(e).attr("name");
        var existsVal = $("#ctl01_tbLocation").val();
        var valueArr = new Array();
        if (existsVal != "" && existsVal!=undefined)
        {
            valueArr = existsVal.split(',');
        }
        Sys.Net.WebServiceProxy.invoke('Webservice/LocationMgr.asmx', 'GetLocationByLikeCode', false,
              { "likeCode": $("#ctl01_valueText").val() },
          function OnSucceeded(data, eventArgs) {
              if (data == undefined || data == "" || data == null) {
              } else {
                  $("li").html("");
                  var jsonvalue = eval(data);
                  for (var i = 0; i < jsonvalue.length; i++) {
                      var v = jsonvalue[i];
                      var isexists = false;
                      for (var j = 0; j < valueArr.length; j++) {
                          var cValue = valueArr[j];
                          if (cValue == v.Code) {
                              isexists = true;
                              break;
                          }
                      }
                      if (i > 50) {
                          break;
                      }
                      var li = "<li class='li_singula'><div  onmouseover='onMouse(this)' onmouseout='outMouse_singula(this)' ><input type='checkbox' name='" + checkName + "'  onclick='changeClick(this)' value=" + v.Code + " >" + v.Code + "[" + v.Name + "]</div></li>";
                      if (isexists) {
                          li = "<li class='li_singula' ><div onmouseover='onMouse(this)' onmouseout='outMouse_double(this)'><input type='checkbox'  name='" + checkName + "' onclick='changeClick(this)' checked='checked' value=" + v.Code + " >" + v.Code + "[" + v.Name + "]</div></li>";
                      }
                      if (i % 2 == 1) {
                          li = "<li class='li_double' ><div onmouseover='onMouse(this)' onmouseout='outMouse_double(this)'><input type='checkbox'  name='" + checkName + "' onclick='changeClick(this)' value=" + v.Code + " >" + v.Code + "[" + v.Name + "]</div></li>";
                          if (isexists) {
                              li = "<li class='li_double' ><div onmouseover='onMouse(this)' onmouseout='outMouse_double(this)'><input type='checkbox'  name='" + checkName + "' onclick='changeClick(this)' checked='checked' value=" + v.Code + " >" + v.Code + "[" + v.Name + "]</div></li>";
                          }
                      }
                      $(s).append(li);
                  }
              }
              $("#ctl01_demo").show();
          },
          function OnFailed(error) {
              alert(error.get_message());
          });

        
    }

    function changeClick(e) {
        var checkboxName = $(e).attr("name");
        var $checkRecords = $("input[name='" + checkboxName + "']:checked");

        var existsVal = $("#ctl01_tbLocation").val();
        var valueArr = new Array();
        if (existsVal != "" && existsVal != undefined) {
            valueArr = existsVal.split(',');
        }
        var checkedValues = existsVal;

        for (var i = 0; i < $checkRecords.length; i++) {
            if ($checkRecords[i].checked) {
                var v = $checkRecords[i].value;
                var isexists = false;
                for (var j = 0; j < valueArr.length; j++) {
                    var cValue = valueArr[j];
                    if (cValue == v) {
                        isexists = true;
                        break;
                    }
                }
                if (isexists)
                {
                    continue;
                }
                if (checkedValues == "") {
                    checkedValues = v;
                } else {
                    checkedValues += "," + v;
                }
            }
        }
        $("#ctl01_tbLocation").val(checkedValues);
    }

</script>
<fieldset>
    <table class="mtable">
         <tr>
            <td class="td01" >
                <asp:Literal ID="lblLocation" runat="server" Text="${Common.Business.Location}:" />
            </td>
            <td class="td02" colspan="3" >
                <asp:TextBox ID="tbLocation" runat="server" CssClass="inputRequired" Width="600" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblLocation1" runat="server" Text="${Common.Business.Location}:" />
            </td>
            <td class="td02">
                <input type="text" ID="valueText" name="checkboxName" runat="server" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"  onclick="textChange(this)"   onkeyup="textChange(this)" onfocus="textChange(this)" onblur="onblurClick(this)" />
                <div id="demo" runat="server" style="width:180px;height:150px;background-color:white;border:1px solid black; display:none; z-index:0"  onmouseover="doMouseOver(this)" >
                    <ul id="ulList" runat="server" style="height:100%;background-color:white; overflow:auto;margin:0;padding:0;border:0;" size=12>
                    </ul>
                </div>
            </td>
            <td class="td01">
                <asp:Literal ID="lblItem" runat="server" Text="${Common.Business.ItemCode}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" DescField="Description" ImageUrlField="ImageUrl"
                    Width="280" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblStartDate" runat="server" Text="${Common.Business.StartDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                    CssClass="inputRequired" />
                <asp:RequiredFieldValidator ID="rfvfc" runat="server" ErrorMessage="开始时间不能为空" Display="Dynamic"
                    ControlToValidate="tbStartDate" ValidationGroup="vgSave" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblEndDate" runat="server" Text="${Common.Business.EndDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlFinanceYear" runat="server" Text="${Cost.FinanceCalendar.YearMonth}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbFinanceYear" runat="server" onClick="WdatePicker({dateFmt:'yyyy-M'})"
                    OnTextChanged="tbFinanceYear_TextChange" AutoPostBack="true" CssClass="inputRequired" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="请选择会计年月"
                    Display="Dynamic" ControlToValidate="tbFinanceYear" ValidationGroup="vgSave" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:CheckBox ID="cbOnlyTotal" runat="server" Text="${Cost.FinanceCalendar.OnlyTotal}" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <%-- <asp:Literal ID="lblDesc" runat="server" Text="${Common.Business.ItemDescription}:" />--%>
            </td>
            <td class="td02">
                <%-- <asp:TextBox ID="tbDesc" runat="server" />--%>
            </td>
            <td class="td01" />
            <td class="t02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    ValidationGroup="vgSave" OnClientClick="if($('#ctl01_tbLocation_suggest').val() ==''&& $('#ctl01_tbItem_suggest').val() =='') { return confirm('没有选择库位/物料,查询需要很长的时间,是否继续?')}" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" OnClick="btnSearch_Click"
                    ValidationGroup="vgSave" OnClientClick="if($('#ctl01_tbLocation_suggest').val() ==''&& $('#ctl01_tbItem_suggest').val() =='') { return confirm('没有选择库位/物料,查询需要很长的时间,是否继续?')}" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset runat="server" id="fld_Gv_List" visible="false">
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="true" OnRowDataBound="GV_List_RowDataBound"
        CellPadding="0" AllowSorting="false">
        <Columns>
            <asp:TemplateField HeaderText="Seq">
                <ItemTemplate>
                    <%#Container.DataItemIndex + 1%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>
