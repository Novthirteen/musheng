// �Զ���ű� 

function AddToFavorite() {
    var pageImage = $("#SiteMapPath1 span:last a").attr("title");
    var hasPermission = false;
    if ($("#divFavorite") != null) {
        hasPermission = true;
    }
    Sys.Net.WebServiceProxy.invoke('Webservice/UserFavoritesMgrWS.asmx', 'AddUserFavorite', false,
        {
            "userCode": document.getElementById("id_User").value,
            "type": "Favorite",
            "pageName": document.getElementById("id_Key").value,
            //"pageUrl": document.URL.replace(/.*?\?/, '?'),
            //"pageImage": pageImage,
            "hasPermission": hasPermission
        },
        function OnSucceeded(result, eventArgs) {
            var RsltElem =
            document.getElementById("FavoriteResultId");
            RsltElem.innerHTML = "Success";
            //alert("Success");
            if (window.top.leftFrame) {
                window.top.leftFrame.LoadFavorite();
            }
        },
        function OnFailed(error) {
            alert(error.get_message());
        }
    );
    $("#FavoriteResultId").fadeIn("slow");
    $("#FavoriteResultId").fadeOut(1500);
}

function OnMainPageLoad() {
    try {
        if (top.location == self.location) {
            top.location = "Default.aspx";
        }
        //$('.GV').fixedtableheader();
        // $('.blank').watermark('Leave blank for All',{className: 'watermark'});
        //$(".colorbox").colorbox({ width: "80%", height: "80%", iframe: true });
        window.parent.document.title = window.parent.document.title.replace(/ -.*/, '');
        window.parent.document.title += " - " + document.title;
        if (window.top.leftFrame != null) {
            window.top.leftFrame.LoadHistory();
        }
        messagesFadeIn();

        //Fix IE Top Padding
        if ($("fieldset legend") != null && $.browser.msie) {
            $("fieldset legend").after("<div style='height:5px;' />");
        }

        //�������Ӹ���ҳ��Ĺرհ�ť.
        if ($("#floatdiv") != null) {
            if ($("#floatdiv").html() != null) {
                //alert($("#floatdiv").html().length);        
                if ($("#floatdiv").html().length > 10) {
                    $("#floatdiv").show();
                    $("#floatdiv").prepend("<div style='margin-left:-10px;margin-right:-10px;background:lavender;height:20px;' id='floatdivtitle'></div>");
                    $("#floatdiv").attr("style", "border-style: solid;border-width: 1px;background-color: #f9f9f9;");

                    if ($("#floatdiv fieldset") != null) {
                        if ($.browser.msie) {
                            $("#floatdiv fieldset:first")
                            .prepend("<div style='margin-top:-37px;margin-right:-18px;' id='divclose'></div>");
                        }
                        else {
                            $("#floatdiv fieldset:first")
                            .prepend("<div style='margin-top:-60px;margin-right:-15px;' id='divclose'></div>");
                        }
                    }
                    else {
                        $("#floatdiv").prepend("<div style='margin-top:-5px;margin-right:-5px;' id='divclose'></div>");
                    }

                    $("#divclose").addClass("floatdivclose");
                    // $("#floatdiv").prepend("<div style='background:#CCCCCC;width:100%' id='floatdivtitle'>test</div>");
                    //����һ:�ѷ��ذ�ť�Ƶ����Ͻ�,��̨����,��ˢ��ҳ��
//                    var btreturn = $("#floatdiv input[type=\"submit\"]:last");
//                    btreturn.attr("value", "");
//                    btreturn.attr("style", "background:url(Images/Icon/close.png) no-repeat;border:none; width:14px;height:14px;cursor:pointer; ");
//                    $("#divclose").append(btreturn);

                    //�������͸����
                    $("body").prepend("<div id='divHide' style='display: block; z-index: 15; top: 0pt; left: 0pt; position: fixed; height: 100%; width: 100%; opacity: 0.5;filter:alpha(opacity=50); background-color: rgb(0, 0, 0);'/>");

                    //
                    //������:ǰ̨����div
                    //$("#divclose").click( function () { $("#floatdiv").hide(); });    

                    //�϶���          
                    $("#floatdiv").easydrag();
                    $("#floatdiv").setHandler("floatdivtitle");
                }
                else {
                    $("#floatdiv").hide();
                }

            }
        }
        //end�������Ӹ���ҳ��Ĺرհ�ť.
        //����ɨ��HuId ~Order_OrderDetail_List  ~Hu_List
        $(".btnShipHuIdCss").before($("#huIdEditDiv"));
        //ֻ����ɫ
    }
    catch (err) { }
}

function GetFrameHeight() {
    return jQuery(window.document).height();
}

function GetFrameWidth() {
    return jQuery(window.document).width();
}

//start message animate
function setbgColor(elId, r, g, b) {
    //document.getElementById(elId).style.backgroundColor = "rgb("+r+","+g+","+b+")";
    $(elId).css({ "background-color": "rgb(" + r + "," + g + "," + b + ")" });
}

function fade(elId, sr, sg, sb, er, eg, eb, step, current, speed) {
    // printfire("----- START fade()");
    if (current <= step) {
        setbgColor(elId, Math.floor(sr * ((step - current) / step) + er * (current / step)), Math.floor(sg * ((step - current) / step) + eg * (current / step)), Math.floor(sb * ((step - current) / step) + eb * (current / step)));
        current++;
        setTimeout("fade('" + elId + "'," + sr + "," + sg + "," + sb + "," + er + "," + eg + "," + eb + "," + step + "," + current + "," + speed + ")", parseInt(speed));
    }
    // printfire("----- END fade()");
}

function messagesFadeIn() {
    //alert($("#ucMessage_divmessages span").attr("title"));
    if ($(".messages span").attr("title") == "Error")
        fade('.messages', 187, 0, 0, 255, 238, 221, 30, 1, 20);
    if ($(".messages span").attr("title") == "Success" || $(".messages span").attr("title") == "")
        fade('.messages', 51, 204, 0, 221, 255, 170, 30, 1, 20);
    if ($(".messages span").attr("title") == "Warning")
        fade('.messages', 255, 153, 0, 255, 255, 102, 30, 1, 20);
}
//end message animate

//Validator

function Check() {
    fade('.messages_Check', 255, 153, 0, 255, 255, 102, 30, 1, 20);
}

function More() {
    if ($("#divMore").is(":hidden ")) {
        $("#divMore").fadeIn("slow");
        $("#more").attr("innerHTML", "Hide...");
    }
    else {
        $("#divMore").hide("slow");
        $("#more").attr("innerHTML", "More...");
    }
}

function ShowNamedQuery() {
    $("#spanNamedQuery2").hide();
    $("#spanNamedQuery1").show();
    $("#spanNamedQuery3").show();
    if ($("#spanNamedQuery3 input[type='text']").attr("value") == null || $("#spanNamedQuery3 input[type='text']").attr("value") == "") {
        $("#spanNamedQuery3 input[type='text']").attr("value", document.title);
    }
}
function HideNamedQuery() {
    $("#spanNamedQuery2").show();
    $("#spanNamedQuery1").hide();
    $("#spanNamedQuery3").hide();
}

function autoCompleteFormate(value, desc, imageUrl) {
    var result = "<table><tr><td rowspan='2'><img src='" + imageUrl + "' /></td><td>" + value + "</td></tr><tr><td>[" + desc + "]</td></tr></table>";
    // alert(result);
    return result;
}

function PrintOrderByQty(printUrl,qty) {
    if(printUrl == null || printUrl.length == 0 || qty == null||qty <= 0) 
    {
        return;
    }
    var xlApp = null;
    try {
        xlApp = new ActiveXObject("Excel.Application");
    } catch (e) {
        //alert("${Common.Warning.Please.Send.The.Site.To.Join.Trust.Site}");
        alert("Please add a site to trust the current site!");
        return;
    }
    var xlBook = xlApp.WorkBooks.open(printUrl);
    var xlsheet = xlBook.Worksheets(1);
    try {
        for(var i=0;i<qty;i++){
            xlsheet.PrintOut(); //��ӡ������
        }
    } catch (e) {
    }
    xlBook.Close(false); //�ر��ĵ�
    xlApp.Quit();   //����excel����
    xlApp = null;   //�ͷ�excel����
}

function PrintOrder(printUrl) {
    if(printUrl == null || printUrl.length == 0) 
    {
        return;
    }
    var xlApp = null;
    try {
        xlApp = new ActiveXObject("Excel.Application");
    } catch (e) {
        //alert("${Common.Warning.Please.Send.The.Site.To.Join.Trust.Site}");
        alert("Please add a site to trust the current site!");
        return;
    }
    var xlBook = xlApp.WorkBooks.open(printUrl);
    var xlsheet = xlBook.Worksheets(1);
    try {
        xlsheet.PrintOut(); //��ӡ������
    } catch (e) {
    }
    xlBook.Close(false); //�ر��ĵ�
    xlApp.Quit();   //����excel����
    xlApp = null;   //�ͷ�excel����
}