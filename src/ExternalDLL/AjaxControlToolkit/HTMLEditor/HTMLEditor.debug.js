Type.registerNamespace("AjaxControlToolkit.HTMLEditor");

AjaxControlToolkit.HTMLEditor.Trim = function(value) {
    return value.replace(/[\x00-\x1F]+/g,"");
};

AjaxControlToolkit.HTMLEditor.TrimAll = function(value) {
    return value.replace(/[\x00-\x1F]/g,"").replace(/^[\x20]+/g,"").replace(/[\x20]+$/g,"");
};

AjaxControlToolkit.HTMLEditor.isIE = (Sys.Browser.agent == Sys.Browser.InternetExplorer);
AjaxControlToolkit.HTMLEditor.isSafari = (Sys.Browser.agent == Sys.Browser.Safari);
AjaxControlToolkit.HTMLEditor.isOpera = (Sys.Browser.agent == Sys.Browser.Opera);

AjaxControlToolkit.HTMLEditor.tryReplaceRgb = function(value) {
    var result = value;
    var re  = /(rgb\s*\(\s*([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)\s*\))/ig;

    function hex(d) {
        return (d < 16) ? ("0" + d.toString(16)) : d.toString(16);
    };
    function repl($0,$1,$2,$3,$4) {
        var r = parseInt($2);
        var g = parseInt($3);
        var b = parseInt($4);
        return "#" + hex(r) + hex(g) + hex(b);
    }
    try { // some versions of Safari don't support such replace
        result = result.replace(re, repl);
    } catch(e){}
    return result;
}

AjaxControlToolkit.HTMLEditor._getScrollTop = function(win) {
    var doc = win.document;
    var scrollTop = 0;
    if (typeof (win.pageYOffset) == 'number') {
        scrollTop = win.pageYOffset;
    }
    if (doc.body && doc.body.scrollTop) {
        scrollTop = doc.body.scrollTop;
    }
    else if (doc.documentElement && doc.documentElement.scrollTop) {
        scrollTop = doc.documentElement.scrollTop;
    }
    return scrollTop;
}

AjaxControlToolkit.HTMLEditor._getScrollLeft = function(win) {
    var doc = win.document;
    var scrollLeft = 0;
    if (typeof (win.pageXOffset) == 'number') {
        scrollLeft = win.pageXOffset;
    }
    else if (doc.body && doc.body.scrollLeft) {
        scrollLeft = doc.body.scrollLeft;
    }
    else if (doc.documentElement && doc.documentElement.scrollLeft) {
        scrollLeft = doc.documentElement.scrollLeft;
    }
    return scrollLeft;
}

AjaxControlToolkit.HTMLEditor.addFormOnSubmit = function(handler, editPanel) {
    var form = window.theForm;
    if (window.theForm != null && typeof window.theForm != "undefined") {
        if (form.AjaxControlToolkit_HTMLEditor_editPanels == null || typeof form.AjaxControlToolkit_HTMLEditor_editPanels == "undefined") {
            form.originalOnSubmit_AjaxControlToolkit_HTMLEditor = window.theForm.onsubmit;
            form.AjaxControlToolkit_HTMLEditor_editPanels = [];
            window.theForm.onsubmit = AjaxControlToolkit.HTMLEditor.EditPanelsOnSubmit;
            if (window.__doPostBack != null && typeof window.__doPostBack != "undefined") {
                if (window.__doPostBack_AjaxControlToolkit_HTMLEditor_original == null || typeof window.__doPostBack_AjaxControlToolkit_HTMLEditor_original == "undefined") {
                    window.__doPostBack_AjaxControlToolkit_HTMLEditor_original = window.__doPostBack;
                    window.__doPostBack = AjaxControlToolkit.HTMLEditor.EditPanelsOnPostBack;
                }
            }
            if (window.ValidatorGetValue != null && typeof window.ValidatorGetValue != "undefined") {
                if (window.ValidatorGetValue_AjaxControlToolkit_HTMLEditor_original == null || typeof window.ValidatorGetValue_AjaxControlToolkit_HTMLEditor_original == "undefined") {
                    window.ValidatorGetValue_AjaxControlToolkit_HTMLEditor_original = window.ValidatorGetValue;
                    window.ValidatorGetValue = AjaxControlToolkit.HTMLEditor.ValidatorGetValue;
                }
            }
        }
        form.AjaxControlToolkit_HTMLEditor_editPanels.push({ handler: handler, editPanel: editPanel });
    }
}

AjaxControlToolkit.HTMLEditor.removeFormOnSubmit = function(handler) {
    var form = window.theForm;
    if (window.theForm != null && typeof window.theForm != "undefined") {
        var original = form.originalOnSubmit_AjaxControlToolkit_HTMLEditor;
        if (form.AjaxControlToolkit_HTMLEditor_editPanels != null && typeof form.AjaxControlToolkit_HTMLEditor_editPanels != "undefined") {
            var newArr = [];
            for (var i = 0; i < form.AjaxControlToolkit_HTMLEditor_editPanels.length; i++) {
                var cur = form.AjaxControlToolkit_HTMLEditor_editPanels[i];
                if (cur.handler != handler) {
                    newArr.push(cur);
                }
            }
            form.AjaxControlToolkit_HTMLEditor_editPanels = newArr;
            if (form.AjaxControlToolkit_HTMLEditor_editPanels.length == 0) {
                window.theForm.onsubmit = original;
                form.originalOnSubmit_AjaxControlToolkit_HTMLEditor = null;
                form.AjaxControlToolkit_HTMLEditor_editPanels = null;
                if (window.__doPostBack_AjaxControlToolkit_HTMLEditor_original != null && typeof window.__doPostBack_AjaxControlToolkit_HTMLEditor_original != "undefined") {
                    window.__doPostBack = window.__doPostBack_AjaxControlToolkit_HTMLEditor_original;
                    window.__doPostBack_AjaxControlToolkit_HTMLEditor_original = null;
                }
                if (window.ValidatorGetValue_AjaxControlToolkit_HTMLEditor_original != null && typeof window.ValidatorGetValue_AjaxControlToolkit_HTMLEditor_original != "undefined") {
                    window.ValidatorGetValue = window.ValidatorGetValue_AjaxControlToolkit_HTMLEditor_original;
                    window.ValidatorGetValue_AjaxControlToolkit_HTMLEditor_original = null;
                }
            }
        }
    }
}

AjaxControlToolkit.HTMLEditor.EditPanelsOnSubmit = function(e) {
    var form = window.theForm;
    var ret = true;
    for (var i = 0; i < form.AjaxControlToolkit_HTMLEditor_editPanels.length; i++) {
        var ret = form.AjaxControlToolkit_HTMLEditor_editPanels[i].handler(e);
        if (!ret) break;
    }
    if (ret && form.originalOnSubmit_AjaxControlToolkit_HTMLEditor != null && typeof form.originalOnSubmit_AjaxControlToolkit_HTMLEditor != "undefined") {
        ret = form.originalOnSubmit_AjaxControlToolkit_HTMLEditor(e);
    }
    if (!ret || !window.Page_IsValid) {
        for (var i = 0; i < form.AjaxControlToolkit_HTMLEditor_editPanels.length; i++) {
            form.AjaxControlToolkit_HTMLEditor_editPanels[i].editPanel._contentPrepared = false;
        }
    }
    return ret;
}

AjaxControlToolkit.HTMLEditor.ValidatorGetValue = function(id) {
    var component = $find(id);
    if (component != null) {
        var editPanel = null;
        if (AjaxControlToolkit.HTMLEditor.Editor.isInstanceOfType(component)) {
            editPanel = component.get_editPanel();
        } else if (AjaxControlToolkit.HTMLEditor.EditPanel.isInstanceOfType(component)) {
            editPanel = component;
        }
        if (editPanel != null) {
            var content = editPanel._contentForValidation;
            if (content == null || typeof content == "undefined") {
                content = editPanel.get_content();
            }
            return content;
        }
    }
    return window.ValidatorGetValue_AjaxControlToolkit_HTMLEditor_original(id);
}

AjaxControlToolkit.HTMLEditor.EditPanelsOnPostBack = function(eventTarget, eventArgument) {
    var form = window.theForm;
    for(var i=0; i < form.AjaxControlToolkit_HTMLEditor_editPanels.length; i++) {
        var ret = form.AjaxControlToolkit_HTMLEditor_editPanels[i].handler(null);
        if(!ret) return false;
    }
    if (window.__doPostBack_AjaxControlToolkit_HTMLEditor_original != null && typeof window.__doPostBack_AjaxControlToolkit_HTMLEditor_original != "undefined") {
        return window.__doPostBack_AjaxControlToolkit_HTMLEditor_original(eventTarget, eventArgument);
    }
    return true;
}

AjaxControlToolkit.HTMLEditor.getRealAttributeIE = function(element, name, source) {
    var value = source;
    var n_value = "";
    function tempFunc(p0,p1) {
        n_value = p1;
    }

    element.outerHTML.replace(new RegExp("^(?:<[^>]*?"+name+"=\")([^\"]*?)\"","ig"),tempFunc);
    if (n_value == "") {
        element.outerHTML.replace(new RegExp("^(?:<[^>]*?"+name+"=')([^']*?)'","ig"),tempFunc);
    }
    if (n_value == "") {
        element.outerHTML.replace(new RegExp("^(?:<[^>]*?"+name+"=)([^\s>]*?)","ig"),tempFunc);
    }
    if (value != n_value && n_value != "") {
        value = n_value;
        value = value.replace(/&amp;/g,"&");
    }
    return value;
}

AjaxControlToolkit.HTMLEditor.getRealAttribute = function(element,name) {
    var searchName = name.toLowerCase();
    var attrs = element.attributes;
    var value = "";
    for (i = 0; i < attrs.length; ++i) {
        var a = attrs.item(i);
        if (!a.specified) continue;

        var name = a.name.toLowerCase();
        if (name == searchName) {
            value = a.value;
            if (AjaxControlToolkit.HTMLEditor.isIE) {
                value = AjaxControlToolkit.HTMLEditor.getRealAttributeIE(element, name, value);
            }
            if (name == "src" || name == "href")
                value = value.replace(/(\(S\([A-Za-z0-9_]+\)\)\/)/,"");
            break;
        }
    }
    return value;
}

AjaxControlToolkit.HTMLEditor.enabledWordTags = [
    "img",
    "strong",
    "p",
    "b",
    "i",
    "u",
    "a",
    "h1",
    "h2",
    "h3",
    "h4",
    "h5",
    "h6",
    "table",
    "tbody",
    "tr",
    "td",
    "ul",
    "ol",
    "li",
    "span",
    "div",
    "font",
    "xml",
    "del",
    "ins",
    "em",
    "sub",
    "sup",
    "hr",
    "br"
];

AjaxControlToolkit.HTMLEditor.cleanUp = function(html) {
    var old_ret;

    var ret = AjaxControlToolkit.HTMLEditor.Trim(html.replace(/[\x00-\x1F]+/g," "))
             .replace(/^[^\u0000]+?<html(?:[^>]*?)>/gi,"").replace(/<\/html(?:[^>]*?)>[^\u0000]*$/gi,"")
             .replace(/<head(?:[^>]*?)>[^\u0000]*?<\/head(?:[^>]*?)>/gi,"")
             .replace(/<body[^>]*?>([^\u0000]*?)<\/body(?:[^>]*?)>/gi,"$1")
             .replace(/<\/?html(?:[^>]*?)>/gi,"")
             .replace(/<\/?head(?:[^>]*?)>/gi,"")
             .replace(/<\/?body(?:[^>]*?)>/gi,"")
             .replace(/<!--(\w|\W)+?-->/ig,"") // remove comments
             .replace(/(<[\/]?)(?:o|v|x|p|w|\?xml):(\w+)([^>]*?>)/ig,"$1$2$3") // remove prefixes
             .replace(/<(IMAGEDATA)([^>]*?)>/ig,"<img$2>") // replace ImageData
             .replace(/<p[^>]*><p>&nbsp;<\/p><\/p>/ig,"<br>") // remove empty P
             .replace(/<p[^>]*?\/>/ig,"").replace(/<(p|div)[^>]*?>&nbsp;<\/(\1)[^>]*?>/ig,"").replace(/<(p|div)[^>]*?><\/(\1)[^>]*?>/ig,"");

    // remove some attributes
    do {
        old_ret = ret;
        ret = ret.replace(/<([^>]*)(?:class|size|lang|face|start|type|border|[ovwxp]:\w+)=(?:\'[^\']*\'|\"[^\"]*\"|[^> ]+)([^>]*)>/ig,  "<$1$2>");
    }
    while(ret != old_ret)

    // remain enabled tags only

    var div = document.createElement("div");

    div.innerHTML = ret;

    function diver(elem) {
        var n = elem.childNodes.length;

        for(var i=0; i<elem.childNodes.length; i++) {
            var child = elem.childNodes.item(i);
            if(child.nodeType==1){
                if(child.tagName.indexOf("/")>=0) {
                    i--;
                    child.parentNode.removeChild(child);
                } else {
                    var search = child.tagName.toLowerCase();
                    var found  = false;
                    var nn     = AjaxControlToolkit.HTMLEditor.enabledWordTags.length;

                    for(var j=0; j < nn; j++) {
                        if(AjaxControlToolkit.HTMLEditor.enabledWordTags[j] == search) {
                            found = true;
                            break;
                        }
                    }
                    diver(child);
                    if(!found) {
                        i += child.childNodes.length;
                        while(child.firstChild) child.parentNode.insertBefore(child.firstChild,child);
                        child.parentNode.removeChild(child);
                        i--;
                    } else {
                        var s_background_color = child.style.backgroundColor;
                        var s_color            = child.style.color;

                        child.style.cssText ="";
                        child.removeAttribute("style");

                        if(child.getAttribute("width") && child.getAttribute("width").length > 0) child.style.width=child.getAttribute("width");
                        if(child.width && child.width.length > 0) child.style.width=child.width;
                        child.width ="";
                        try{child.removeAttribute("width");}catch(e){}

                        if(child.getAttribute("height") && child.getAttribute("height").length > 0) child.style.height=child.getAttribute("height");
                        if(child.height && child.height.length > 0) child.style.height=child.height;
                        child.height ="";
                        try{child.removeAttribute("height");}catch(e){}

                        if(search=="table") {
                            child.style.borderLeftWidth = "1px";
                            child.style.borderLeftColor = "black";
                            child.style.borderLeftStyle = "solid";
                            child.style.borderTopWidth  = "1px";   
                            child.style.borderTopColor  = "black"; 
                            child.style.borderTopStyle  = "solid";

                            child.style.backgroundColor = s_background_color;
                            child.style.color           = s_color;
                        }

                        if(search=="td") {
                            child.style.borderRightWidth  = "1px";
                            child.style.borderRightColor  = "black";
                            child.style.borderRightStyle  = "solid";
                            child.style.borderBottomWidth = "1px";   
                            child.style.borderBottomColor = "black"; 
                            child.style.borderBottomStyle = "solid";

                            child.style.backgroundColor   = s_background_color;
                            child.style.color             = s_color;
                        }

                        if(search=="font" || search=="span") {
                            child.style.backgroundColor = s_background_color;
                            child.style.color           = s_color;

                            var attrs = child.attributes;
                            var n =0;
                            for (var m = 0; m < attrs.length; ++m) {
                                var a = attrs.item(m);
                                if (!a.specified) continue;
                                n++;
                            }

                            if(n== 0 && child.style.cssText=="") {
                                i += child.childNodes.length;
                                while(child.firstChild) child.parentNode.insertBefore(child.firstChild,child);
                                child.parentNode.removeChild(child);
                                i--;
                            }
                        }
                    }
                }
            }
        }
    }

    diver(div);
    ret = AjaxControlToolkit.HTMLEditor.Trim(div.innerHTML);
    delete div;

    
    ret = ret.replace(/<[\/]?(xml|del|ins)[^>]*?>/ig,"") // remove some tags (content should be remained)
         .replace(/<(p|div)[^>]*?>/ig,"") // remove P, DIV tags (content should be remained) <br> is added
         .replace(/<\/(p|div)[^>]*?>/ig,"<br>");

    // remove empty style tags
    do {
        old_ret = ret;
        ret = ret.replace(/<b><\/b>/ig,"").replace(/<i><\/i>/ig,"").replace(/<u><\/u>/ig,"").replace(/<strong><\/strong>/ig,"").replace(/<em><\/em>/ig,"").replace(/<sub><\/sub>/ig,"").replace(/<sup><\/sup>/ig,"");
        ret = ret.replace(/<span[^>]*?><\/span>/ig,"").replace(/<span>([^<]+?)<\/span>/ig,"$1");
        ret = ret.replace(/<font[^>]*?><\/font>/ig,"").replace(/<font>([^<]+?)<\/font>/ig,"$1");
    }
    while(ret != old_ret)

    // replace all Microsoft special characters
    ret = ret.replace(/&rsquo;/g,  "'")
         .replace(/&lsquo;/g,  "'")
         .replace(/&ndash;/g,  "-")
         .replace(/&mdash;/g,  "-")
         .replace(/&hellip;/g, "...")
         .replace(/&quot;/g,   "\"")
         .replace(/&ldquo;/g,  "\"")
         .replace(/&rdquo;/g,  "\"")
         .replace(//g,        "")
         .replace(/&bull;/g,   "")
         .replace(/[ \s]+/g," ").replace(/((&nbsp;)+)/g,"&nbsp;"); // remove extra spaces

    if(document.all) ret = ret.replace(/^[\x00-\x1F]*&nbsp;/,"");

    return ret;
};

AjaxControlToolkit.HTMLEditor.spanJoiner = function(element,doc, sFrom, sTo, nobr) {
    var sIndex  = 0;
    var sLength = element.childNodes.length;

    if(typeof sFrom != "undefined" && sFrom != null) sIndex  = sFrom;
    if(typeof sTo   != "undefined" && sTo   != null) sLength = sTo;

    for(var i=sIndex; i < sLength && i < element.childNodes.length; i++) {
        var child = element.childNodes.item(i)
        if (child.parentNode != element) {
            continue;
        }
        switch (child.nodeType) {
            case 1: // Node.ELEMENT_NODE
            if(child.childNodes.length==0 && AjaxControlToolkit.HTMLEditor.isStyleTag(child.tagName) && child.tagName.toUpperCase() != "A" && !(child.className.length > 0 || (child.getAttribute("class") && child.getAttribute("class").length > 0)) && !AjaxControlToolkit.HTMLEditor.isTempElement(child)) {
                element.removeChild(child);
                i--;
                sLength--;
                continue;
            }                             

            if( child.tagName.toUpperCase()=="SPAN") {
                while(child.childNodes.length==1 && child.firstChild.nodeType==1) {
                    if(child.firstChild.tagName.toUpperCase()=="SPAN" && !AjaxControlToolkit.HTMLEditor.isTempElement(child.firstChild)) {
                        var attrs  = AjaxControlToolkit.HTMLEditor.differAttr (child.firstChild,[]);
                        var styles = AjaxControlToolkit.HTMLEditor.differStyle(child.firstChild);
                        var oldSpan   = child.firstChild;
                        var chieldren = oldSpan.childNodes;

                        while(oldSpan.firstChild != null) {
                            child.insertBefore(oldSpan.firstChild,oldSpan);
                        }

                        for(var j=0; j < styles.length; j++) {
                            if(styles[j][1]) {
                                try {
                                    if(child.style[styles[j][0]]) {
                                        if(styles[j][0].toLowerCase().indexOf("color") >= 0) {
                                            child.style[styles[j][0]] = styles[j][1];
                                        } else {
                                            try {
                                                var sv = child.style[styles[j][0]];

                                                child.style[styles[j][0]] = child.style[styles[j][0]]+" "+styles[j][1];

                                                if(sv == child.style[styles[j][0]]) {
                                                    child.style[styles[j][0]] = styles[j][1];
                                                }
                                            }
                                            catch(e) {
                                                child.style[styles[j][0]] = styles[j][1];
                                            }
                                        }
                                    } else {
                                        child.style[styles[j][0]] = styles[j][1];
                                    }
                                } catch (ee) {}
                            }
                        }
                        for(var j=0; j < attrs.length; j++) {
                            if(attrs[j][1]) {
                                child.setAttribute(attrs[j][0],attrs[j][1]);
                            }
                        }
                        child.removeChild(oldSpan);
                        continue;
                    } else {
                        if(child.firstChild.tagName.toUpperCase()=="SPAN" && AjaxControlToolkit.HTMLEditor.isTempElement(child.firstChild)) {
                            var svv = child.firstChild;
                            child.parentNode.insertBefore(child.firstChild,child);
                            child.parentNode.removeChild(child);
                            child = svv;
                        }
                    }

                    break;
                }

                var tempArr = [];
                var nextChild = child.nextSibling;

                while(!AjaxControlToolkit.HTMLEditor.isTempElement(child) && nextChild && i+1 < sLength && (nextChild.nodeType == 3 || (nextChild.nodeType == 1 &&
                      (nextChild.tagName.toUpperCase()=="SPAN" || (nextChild.tagName.toUpperCase()=="BR") && typeof nobr == "undefined") &&
                      !AjaxControlToolkit.HTMLEditor.isTempElement(nextChild)))) {
                    if(nextChild.nodeType == 3) {
                        if((""+nextChild.data+"").length==0) {
                            nextChild.parentNode.removeChild(nextChild);
                            nextChild = child.nextSibling;
                            sLength--;
                        } else {
                            break;
                        }
                    }
                    else {
                        if(nextChild.tagName.toUpperCase()=="BR") {
                            tempArr.push(nextChild);
                            nextChild = nextChild.nextSibling;
                        } else {
                            var attrs  = AjaxControlToolkit.HTMLEditor.differAttr (child,[], nextChild);
                            var styles = AjaxControlToolkit.HTMLEditor.differStyle(child,    nextChild);

                            if(attrs.length==0 && styles.length==0 && child.className == nextChild.className) {
                                var n = tempArr.length;

                                for(var j=0; j < n; j++) {
                                    child.appendChild(tempArr[j]);
                                    sLength--;
                                }
                                tempArr = [];

                                while(nextChild.firstChild) child.appendChild(nextChild.firstChild);
                                nextChild.parentNode.removeChild(nextChild);
                                nextChild = child.nextSibling;
                                sLength--;
                            } else {
                                break;
                            }
                        }
                    }
                }

                if(!AjaxControlToolkit.HTMLEditor.isTempElement(child) && child.className.length == 0) {
                    var attrs  = AjaxControlToolkit.HTMLEditor.differAttr (child,[]);
                    var styles = AjaxControlToolkit.HTMLEditor.differStyle(child   );

                    if(attrs.length==0 && styles.length==0) {
                        i--;
                        sLength--;
                        while(child.firstChild) {
                            child.parentNode.insertBefore(child.firstChild,child);
                            sLength++;
                        }
                        child.parentNode.removeChild(child);
                        continue;
                    }
                }
            }

            if(child.parentNode != null) {
                if(child.childNodes.length==0 && AjaxControlToolkit.HTMLEditor.isStyleTag(child.tagName) && child.tagName.toUpperCase() != "A" && !(child.className.length > 0 || (child.getAttribute("class") && child.getAttribute("class").length > 0)) && !AjaxControlToolkit.HTMLEditor.isTempElement(child)) {
                    element.removeChild(child);
                    i--;
                    sLength--;
                    continue;
                } else {
                    AjaxControlToolkit.HTMLEditor.spanJoiner(child,doc);
                }
            }
            break;
        }
    }
};

AjaxControlToolkit.HTMLEditor._styleTags = [
    "strong",
    "em",
    "u",
    "strike",
    "s",
    "span",
    "font",
    "b",
    "sub",
    "sup",
    "a",
    "i"
];

AjaxControlToolkit.HTMLEditor.isStyleTag = function(tag) {
    if(!tag) return false;
    for(var i=0; i< AjaxControlToolkit.HTMLEditor._styleTags.length; i++) {
        if(AjaxControlToolkit.HTMLEditor._styleTags[i].toLowerCase()==tag.toLowerCase()) return true;
    }
    return false;
};

AjaxControlToolkit.HTMLEditor.smartClassName = "AjaxControlToolkitMSIEparagraph";
AjaxControlToolkit.HTMLEditor.noContextMenuAttribute= "obout-no-contextmenu";

AjaxControlToolkit.HTMLEditor.isTempElement = function(el) {
    if(el.id && el.id.length > 0 && el.id.indexOf(AjaxControlToolkit.HTMLEditor.smartClassName) >= 0) return true;
    return false;
};

AjaxControlToolkit.HTMLEditor.differAttr = function(element,pr, comp) {
    var result = [];
    var parent = element.parentNode;

    if(typeof comp != "undefined") parent = comp;

    if(!parent || !parent.tagName || !AjaxControlToolkit.HTMLEditor.isStyleTag(parent.tagName)) parent = null;

    if(element.attributes)
    for (var i=0; i < element.attributes.length; i++) {
        var attr = element.attributes[i];
        var brk=false;

        for(var j=0; j < pr.length; j++) {
            if(attr.name.toUpperCase() == pr[j].toUpperCase()) {
                brk=true;
                break;
            }
        }

        if(brk) continue;

        if(attr.name.toUpperCase() == "STYLE") continue;
        if(attr.name.toUpperCase().substr(0,4) == "_MOZ") continue;
        if(attr.specified)
        if(parent && parent.attributes && parent.attributes[attr.name]) {
            var pattr= parent.attributes[attr.name];

            if(pattr) {
                if(attr.name != pattr.name || attr.value != pattr.value) {
                    result.push([attr.name, attr.value]);
                }
            }
        } else {
            if(attr.name.toUpperCase() == "CLASS" && attr.value =="") continue;
            result.push([attr.name, attr.value]);
        }
    }
    return result;
};

AjaxControlToolkit.HTMLEditor.differStyle = function(element, comp) {
    var result = [];
    var parent = element.parentNode;

    if(typeof comp != "undefined") parent = comp;

    if(!parent || !parent.tagName || !AjaxControlToolkit.HTMLEditor.isStyleTag(parent.tagName)) parent = null;

    function _putStyle(i,_style) {
        _style=""+_style;

        if(i.toLowerCase()=="textdecoration") {
            var _arr = _style.split(" ");

            for(var j=0; j<_arr.length; j++) {
                result.push([i, AjaxControlToolkit.HTMLEditor.Trim(_arr[j])]);
            }
        } else {
            result.push([i, _style]);
        }
    }

    for (var i in element.style) {
        if(i && typeof i == "string" && i != "accelerator") {
            var ii = i;
            
            if(!isNaN(parseInt(i))) {
                if(!AjaxControlToolkit.HTMLEditor.isSafari) {
                    continue;
                }
                ii = element.style[i];
            }

            var style = element.style[ii];
            
            if(style && typeof style == "string" && style != "accelerator") {
                if(parent && parent.style) {
                    var pstyle= parent.style [ii];

                    if(ii.toLowerCase() != "csstext" && ii.toLowerCase() != "length")
                    if(style != pstyle) {
                        _putStyle(ii, style);
                    }
                } else {
                    if(ii.toLowerCase() != "csstext" && ii.toLowerCase() != "length") {
                        _putStyle(ii, style);
                    }
                }
            }
        }
    }

    if(typeof comp != "undefined")
    for (var i in parent.style) {
        if(i && typeof i == "string" && i != "accelerator") {
            var ii = i;
            
            if(!isNaN(parseInt(i))) {
                if(!AjaxControlToolkit.HTMLEditor.isSafari) {
                    continue;
                }
                ii = element.style[i];
            }

            var style = parent.style[ii];
            if(style && typeof style == "string" && style != "accelerator") {
                var pstyle= element.style [ii];

                if(i.toLowerCase() != "csstext" && ii.toLowerCase() != "length")
                if(style != pstyle) {
                    _putStyle(ii, style);
                }
            }
        }
    }

    return result;
};

AjaxControlToolkit.HTMLEditor.brXHTML = function(str) {
    return str.replace(/<br>/ig, "<br/>");
};


AjaxControlToolkit.HTMLEditor._needsClosingTag = function(el) {
    var closingTags = " script style div span a del strong em u strike font b sub sup p iframe li ul ol placeholder textarea td tr ";
    return (closingTags.indexOf(" " + el.tagName.toLowerCase() + " ") != -1);
};

AjaxControlToolkit.HTMLEditor._encodeText_ = function(str) {
    return str.replace(/&/ig, "&amp;").replace(/</ig, "&lt;").replace(/>/ig, "&gt;").replace(/\"/ig, "&quot;").replace(/\xA0/ig, "&nbsp;");
};

AjaxControlToolkit.HTMLEditor._noNeedsClosingTag = function(el) {
    var closingTags = " hr br ";
    return (closingTags.indexOf(" " + el.tagName.toLowerCase() + " ") != -1);
};

AjaxControlToolkit.HTMLEditor.canBeInsideP = function(el,prize)
{
    if(el && el.style && el.style.display && el.style.display.toLowerCase()=="inline") return true;

    var name = el.tagName.toUpperCase();

    if(name.length==2) {
        if(name.substr(0,1)=="H" && parseInt(name.substr(1,1)) > 0) {
            return false;
        }
    }
    switch(name) {
        case "TBODY"      :
        case "TR"         :
        case "TD"         :
            if(typeof prize != "undefined") {
                var par = el.parentNode;
                while(par && par.tagName && par.tagName.toUpperCase() != "TABLE") par = par.parentNode;

                if(par.tagName.toUpperCase() == "TABLE" && par.style && par.style.display && par.style.display.toLowerCase() == "inline") {
                    return true;
                }
            }
        case "P"          :
        case "PRE"        :
        case "TABLE"      :
        case "OL"         :
        case "UL"         :
        case "LI"         :
        case "HR"         :
        case "DIV"        :
        case "BLOCKQUOTE" :
        case "FORM"       :
        case "FIELDSET"   :
        case "LEGEND"     :
            return false;
        default:
            return true;   
    }
};

AjaxControlToolkit.HTMLEditor.convertAlign = function(aval) {
    var value;
    var n;

    try { n = parseInt(aval)-1;}
    catch(e){return aval;}

    switch(n) {
        case  1:
            value = "left";
            break;
        case  2:
            value = "right";
            break;
        case  3:
            value = "texttop";
            break;
        case  4:
            value = "absmiddle";
            break;
        case  5:
            value = "baseline";
            break;
        case  6:
            value = "absbottom";
            break;
        case  7:
            value = "bottom";
            break;
        case  8:
            value = "middle";
            break;
        case  9:
            value = "top";
            break;
        default:
            value = aval.replace(/\"/g,"&quot;");
    }

    return value;
};

AjaxControlToolkit.HTMLEditor.getHTML = function(root, outputRoot, must)
{
    try {
        if(typeof must == "undefined") {
            if(!outputRoot && root.nodeType==1) {
                return root.innerHTML;
            } else {
                if(outputRoot && root.nodeType==1 && AjaxControlToolkit.HTMLEditor.isIE) {
                    return root.outerHTML;
                }
            }
        }
    } catch(e){}

    var html = new AjaxControlToolkit.HTMLEditor.jsDocument(true);
    AjaxControlToolkit.HTMLEditor._getHTML_(html, root, outputRoot);

    return html.toString();
};

AjaxControlToolkit.HTMLEditor._getHTML_ = function(html, root, outputRoot, must)
{
    switch (root.nodeType) {
        case 1: // Node.ELEMENT_NODE
        case 11: // Node.DOCUMENT_FRAGMENT_NODE
            if(root.tagName && root.tagName.indexOf("/") >= 0) {
                if(AjaxControlToolkit.HTMLEditor.isIE) {
                    var tag  = root.tagName.toLowerCase().substr(root.tagName.indexOf("/")+1);
                    var prev = root.previousSibling;

                    if(tag == "embed") return;

                    while(prev != null) {
                        if(prev.nodeType == root.nodeType && prev.tagName && prev.tagName.toLowerCase() == tag) {
                            html.append("</teo"+AjaxControlToolkit.HTMLEditor.smartClassName+":"+root.tagName.toLowerCase().substr(root.tagName.indexOf("/")+1)+">");
                            return;
                        }

                        prev = prev.previousSibling;
                    }
                }

                return;
            }

            var closed;
            var noSlash;
            var i;
            if (outputRoot && root.tagName.length >0) {
                var tag = root.tagName.toLowerCase();
                closed = (!(root.hasChildNodes() || AjaxControlToolkit.HTMLEditor._needsClosingTag(root)));
                noSlash= true;

                var scope = "";
                if(AjaxControlToolkit.HTMLEditor.isIE && root.scopeName && typeof root.scopeName != "undefined") {
                    scope = (root.scopeName.toUpperCase()=="HTML")?"":(root.scopeName+":");
                }

                if(AjaxControlToolkit.HTMLEditor.isIE && (closed || tag == "placeholder") && !AjaxControlToolkit.HTMLEditor._noNeedsClosingTag(root) && tag !="embed") {
                    var next = root.nextSibling;

                    while(next != null) {
                        if(next.nodeType == root.nodeType && next.tagName) {
                            var nextTagName = next.tagName;
                            if(nextTagName.indexOf("/") >= 0)
                            if(nextTagName.toLowerCase().substr(nextTagName.indexOf("/")+1) == tag) {
                                closed = false;
                                noSlash= false;
                                break;
                            }
                        }

                        next = next.nextSibling;
                    }
                }

                if(!AjaxControlToolkit.HTMLEditor.canBeInsideP(root)) {
                    html.append("\n");
                }

                html.append("<"+((!closed && !noSlash)?"teo"+AjaxControlToolkit.HTMLEditor.smartClassName+":":scope)+ tag);

                if(AjaxControlToolkit.HTMLEditor.isIE && root.name && root.name.length > 0) {
                    html.append(" name=\"" + root.name.replace(/\"/g,"&quot;") + '"');
                }
                if(AjaxControlToolkit.HTMLEditor.isIE && root.value && root.value.length > 0 && tag != "textarea") {
                    html.append(" value=\"" + root.value.replace(/\"/g,"&quot;") + '"');
                }
                if(AjaxControlToolkit.HTMLEditor.isIE && root.className && root.className.length > 0) {
                    html.append(" class=\"" + root.className.replace(/\"/g,"&quot;") + '"');
                }
                if(AjaxControlToolkit.HTMLEditor.isIE && root.align && root.align.length > 0) {
                    html.append(" align=\"" + root.align.replace(/\"/g,"&quot;") + '"');
                }
                if(AjaxControlToolkit.HTMLEditor.isIE && root.color && root.color.length > 0) {
                    html.append(" color=\"" + root.color.replace(/\"/g,"&quot;") + '"');
                }
                if(AjaxControlToolkit.HTMLEditor.isIE && root.size && root.size.length > 0 && root.size != "+0") {
                    html.append(" size=\"" + root.size.replace(/\"/g,"&quot;") + '"');
                }
                if(AjaxControlToolkit.HTMLEditor.isIE && root.shape && root.shape.length > 0) {
                    html.write(" shape" + '="' + root.shape.replace(/\"/g,"&quot;") + '"');
                }
                if(AjaxControlToolkit.HTMLEditor.isIE && root.coords && root.coords.length > 0) {
                    html.write(" coords" + '="' + root.coords.replace(/\"/g,"&quot;") + '"');
                }

                var attrs = root.attributes;
                var cssForSafari = null;
                for (i = 0; i < attrs.length; ++i) {
                    var a = attrs.item(i);
                    if (!a.specified) continue;

                    var name = a.name.toLowerCase();
                    if (name.substr(0, 4) == "_moz") {
                        // Mozilla reports some special attributes
                        // here we don't need them.
                        continue;
                    }

                    if (name == "teoalign") {
                        continue;
                    }

                    var value;
                    if (name != 'style') {
                        if(name=='width') {
                            value= root.width;
                            if(AjaxControlToolkit.HTMLEditor.isIE && value == 0) {
                                var n_value = 0;

                                root.outerHTML.replace(new RegExp("^(?:<[^>]*?width=)([\\d]+)","ig"),function(p0,p1){n_value = p1;});

                                if(value != n_value) value = n_value;
                            }
                        }
                        else
                        if(name=='height') {
                            value= root.height;
                            if(AjaxControlToolkit.HTMLEditor.isIE && value == 0) {
                                var n_value = 0;

                                root.outerHTML.replace(new RegExp("^(?:<[^>]*?height=)([\\d]+)","ig"),function(p0,p1){n_value = p1;});

                                if(value != n_value) value = n_value;
                            }
                        }
                        else
                        if(AjaxControlToolkit.HTMLEditor.isIE && name=='name' && root.name && root.name.length > 0 )
                            continue;
                        else
                        if(AjaxControlToolkit.HTMLEditor.isIE && name=='value' && root.value && root.value.length > 0 )
                            continue;
                        else
                        if(AjaxControlToolkit.HTMLEditor.isIE && name=='align' && root.align && root.align.length > 0 )
                            continue;
                        else
                        if(AjaxControlToolkit.HTMLEditor.isIE && name=='class' && root.className && root.className.length > 0 )
                            continue;
                        else
                        if(AjaxControlToolkit.HTMLEditor.isIE && name=='color' && root.color && root.color.length > 0 )
                            continue;
                        else
                        if(AjaxControlToolkit.HTMLEditor.isIE && name=='size' && root.size && root.size.length > 0 )
                            continue;
                        else
                        if(AjaxControlToolkit.HTMLEditor.isIE && name=='shape' && root.shape && root.shape.length > 0 )
                            continue;
                        else
                        if(AjaxControlToolkit.HTMLEditor.isIE && name=='coords' && root.coords && root.coords.length > 0 )
                            continue;
                        else {
                            if(tag=="embed" && name=="align" && (AjaxControlToolkit.HTMLEditor.isIE)) {
                                value = AjaxControlToolkit.HTMLEditor.convertAlign(a.value);
                            }
                            else {
                                value = a.value;
                                if(AjaxControlToolkit.HTMLEditor.isSafari && name == "class") {
                                    if(/apple-style/ig.test(value)) {
                                        continue;
                                    }
                                }
                                if(name == "src" || name == "href") {
                                    if (AjaxControlToolkit.HTMLEditor.isIE) {
                                        value = AjaxControlToolkit.HTMLEditor.getRealAttributeIE(root, name, value);
                                    }
                                    value = value.replace(/(\(S\([A-Za-z0-9_]+\)\)\/)/,"");
                                }
                                value = value.replace(/\"/g,"&quot;");
                            }
                        }
                    } else {
                        if(AjaxControlToolkit.HTMLEditor.isSafari) {
                            cssForSafari = a.value;
                        }
                        continue;
                    }

                    var qchar = "\"";
                    if((""+value+"").indexOf("\"") >= 0) qchar="'";
                    if(name != null) html.append(" " + name + '=' + qchar + value + qchar);
                }

                if(root.style.cssText.length > 0 || cssForSafari != null) {
                    var name  = "style";
                    var re1 = /(url\((?:[^\)]*)\))/ig;
                    var urls = [];

                    function f2($0,$1) {
                        urls.push($1);
                    }

                    var value = ((cssForSafari !=null)?cssForSafari:root.style.cssText).toLowerCase();
                    value.replace(re1, f2);
                    var times = 0;

                    function f3() {
                        var temp = urls[times];
                        times++;
                        return temp;
                    }

                    value = AjaxControlToolkit.HTMLEditor.tryReplaceRgb(value.replace(re1, f3)).replace(/(font-weight\s*:\s*)(700)/ig, "$1bold")
                            .replace(/([\s]*-moz-[^;]*[;][\s]*)/ig, "").replace(/(-moz-.*)$/i, "")
                            .replace(/(background-position: 0% 0%[;]*[\s]*)/ig, "");
                    
                    if(AjaxControlToolkit.HTMLEditor.isSafari) {
                        function repSaf($0,$1,$2,$3) {
                            return $1+$2.replace(/(,)/g,"")+$3;
                        }
                        
                        value = value.replace(/(text-decoration:)([^;$]+)([;$])/ig, repSaf);
                    }
                    
                    if(AjaxControlToolkit.HTMLEditor.isSafari || AjaxControlToolkit.HTMLEditor.isOpera) {
                        function repSafOp($0,$1,$2,$3) {
                            return $1+$2.replace(/(['"])/g,"")+$3;
                        }
                        value = value.replace(/(font-family:)([^;]+)([;]*)/ig, repSafOp);
                    }

                    if(value.length > 0) {
                        var qchar = "\"";
                        if((""+value+"").indexOf("\"") >= 0) qchar="'";
                        html.append(" " + name + '=' + qchar + value + qchar);
                    }
                }

                html.append(closed ? " />" : ">");

                if(tag=="br") html.append("\n");
            }

            if(root.tagName && root.tagName.toUpperCase()=="SCRIPT") html.append(root.text);
            
            if(root.tagName && root.tagName.toUpperCase()=="STYLE") {
                html.append(root.innerHTML);
            } else {
                for (i = root.firstChild; i; i = i.nextSibling) {
                    AjaxControlToolkit.HTMLEditor._getHTML_(html,i, true)
                }
            }

            if (outputRoot && root.tagName.length >0 && !closed && noSlash) {
                html.append("</" + scope + root.tagName.toLowerCase() + ">");
            }
            break;
        case 3: // Node.TEXT_NODE
            html.append(AjaxControlToolkit.HTMLEditor._encodeText_(""+root.data+""));
            break;
        case 8: // Node.COMMENT_NODE
            if(root.length > 0) {
                html.append("<!--" + root.data + "-->");
                
            } else { // IE bug tricking (negative lengths happen there)
                html.append("<!---->");
            }
            break;
    }
};

AjaxControlToolkit.HTMLEditor.RemoveContextMenu = function() {
    var editor = this;
    var hhh=editor._contextElement.parentNode.removeChild(editor._contextElement);
    if (hhh) delete hhh;
    editor._contextElement=null;
    editor._contextTable=null;
    if (editor.__saved_range__){
        editor.__saved_range__.select();
        editor.__saved_range__=null;
    }
};

AjaxControlToolkit.HTMLEditor.contentEditable = function(el, prize) {
    while(el != null) {
        try {
            var mean  = null;
            if(el.contentEditable != null && typeof el.contentEditable != "undefined" && !(AjaxControlToolkit.HTMLEditor.isSafari || AjaxControlToolkit.HTMLEditor.isOpera)) {
                if(!el.contentEditable || el.contentEditable=="false") {
                    mean = false;
                } else {
                    mean = true;
                }
            }
            else {
                var value = el.getAttribute("contenteditable");

                if(typeof value == "boolean") {
                    mean = value;
                } else {
                    if(typeof value == "string" && value.toLowerCase()=="false") {
                        mean = false;
                    }
                }
            }

            if(mean != null && typeof mean == "boolean") {
                if(!mean) {
                    return el;
                }
            }
        } catch(ex) {}

        if(typeof prize != "undefined" && prize) {
            return null;
        }
        if(el.tagName != null && typeof el.tagName != "undefined" && (el.tagName.toUpperCase()=="BODY" || el.tagName.toUpperCase()=="HTML")) {
            break;
        }
        el = el.parentNode;
    }
    return null;
};

AjaxControlToolkit.HTMLEditor.getSelParent = function (editor) {
    var sel   = editor._getSelection();
    var range = editor._createRange(sel);
    var parent = null;

    if(AjaxControlToolkit.HTMLEditor.isIE) {
        if(sel.type.toLowerCase()=="control")
            parent =range.item(0);
        else
            parent= editor._getParent(range);
    } else {
        parent= editor._getParent(range);
        if(parent.nodeType != 3 && range.startContainer==range.endContainer) {
            var p=parent;
            parent = parent.childNodes.item(range.startOffset);
            if(parent==null) parent=p;
        }
    }
    return parent;
};

AjaxControlToolkit.HTMLEditor.__getIndex = function(el) {
    var ind =0;

    if(el.parentNode) {
        for(;ind<el.parentNode.childNodes.length;ind++) {
            if(el.parentNode.childNodes.item(ind)==el) {
                break;
            }
        }
    }
    return ind;
};

AjaxControlToolkit.HTMLEditor.isInlineElement = function(el) {
    if(el.nodeType == 3) return true;
    if(el.nodeType != 1) return false;
    if(!el.tagName || el.tagName.length == 0) return false;
    if(el && el.style && el.style.display && el.style.display.toLowerCase()=="inline") return true;

    var name = el.tagName.toUpperCase();

    if(name.length==2) {
        if(name.substr(0,1)=="H" && parseInt(name.substr(1,1)) > 0) {
            return false;
        }
    }
    switch(name) {
        case "BR"         :
        case "TBODY"      :
        case "TR"         :
        case "TD"         :
        case "P"          :
        case "PRE"        :
        case "TABLE"      :
        case "OL"         :
        case "UL"         :
        case "LI"         :
        case "HR"         :
        case "DIV"        :
        case "BLOCKQUOTE" :
        case "FORM"       :
        case "FIELDSET"   :
        case "LEGEND"     :
            return false;
        default:
            return true;   
    }
};

AjaxControlToolkit.HTMLEditor.capLock = function(e) { 
    var kc = e.charCode;
    var sk = e.shiftKey?e.shiftKey:((kc == 16)?true:false);
    if(((kc >= 65 && kc <= 90) && !sk)||((kc >= 97 && kc <= 122) && sk))
        return true;
    else
        return false;
};

AjaxControlToolkit.HTMLEditor.operateAnchors = function(editor, _doc, _prize) {
    var aList = _doc.getElementsByTagName("A");
    var ret = false;

    for(var i=0; i < aList.length; i++) {
        var a   = aList[i];

        if( a.name && a.name.length > 0) {
            var imgToDelete = [];
            
            for(var j=0; j < a.childNodes.length; j++) {
                var node = a.childNodes.item(j);
                if(node.nodeType==1 && node.tagName && node.tagName.toUpperCase()=="IMG" && node.src==editor._editPanel.get_imagePath_anchor()) {
                    imgToDelete.push(node);
                    ret = true;
                }
            }
            
            while (imgToDelete.length > 0) {
                a.removeChild(imgToDelete.pop());
            }
            
            if(!_prize) {
                var img   = _doc.createElement("IMG");
                img.title = a.name;
                img.src   = editor._editPanel.get_imagePath_anchor();
                img.setAttribute(editor.noContextMenuAttributeName(),"yes");

                a.appendChild(img);
            }
        }
    }
    return ret;
};

AjaxControlToolkit.HTMLEditor.operatePlaceHolders = function(editor, _doc, _prize) {
    var ret = false;
    if(_prize) {
        var tempCollection = _doc.getElementsByTagName("IMG");

        var aList =[];
        for(var i=0; i<tempCollection.length; i++) {
            aList.push(tempCollection[i]); 
        }

        for(var i=0; i < aList.length; i++)
        {
            var a   = aList[i];
            var dum = a.getAttribute("dummytag");

            if(dum && dum.length > 0 && dum.toLowerCase()=="placeholder") {
                var ph   = _doc.createElement("PLACEHOLDER");
                var title = a.title;

                if(title==null || typeof title=="undefined") {
                    title = a.getAttribute("title");
                }

                ph.name  = title;
                ph.setAttribute("name",title);

                a.parentNode.insertBefore(ph,a);
                a.parentNode.removeChild (a);
                ret = true;
            }
        }
    } else {
        var tempCollection = _doc.getElementsByTagName("PLACEHOLDER");

        var aList =[];
        for(var i=0; i<tempCollection.length; i++) {
            aList.push(tempCollection[i]);
        }
        for(var i=0; i < aList.length; i++) {
            var a   = aList[i];
            var nd  = true;

            try {
                if(a.childNodes.length > 0) {
                    nd = false;
                }
            } catch(ex) {}

            if(nd) {
                var name = a.name;

                if(name==null || typeof name=="undefined") {
                    name = a.getAttribute("name");
                }

                var img   = _doc.createElement("IMG");
                img.title = name;
                img.src   = editor._editPanel.get_imagePath_placeHolder();

                img.setAttribute("dummytag","placeholder");
                img.setAttribute("title",name);

                a.parentNode.insertBefore(img,a);
                a.parentNode.removeChild (a);
            }
        }
    }
    return ret;
};

AjaxControlToolkit.HTMLEditor.inspectForShadows = function(el) {
    var aList = el.getElementsByTagName("IMG");

    for(var i=0; i < aList.length; i++)
    {
        if(aList[i].getAttribute(AjaxControlToolkit.HTMLEditor.attachedIdAttribute) && aList[i].getAttribute(AjaxControlToolkit.HTMLEditor.attachedIdAttribute).length > 0) {
            try {
                if(AjaxControlToolkit.HTMLEditor.isIE) {
                    $removeHandler(aList[i],"dragstart", AjaxControlToolkit.HTMLEditor.stopDrag);
                } else {
                    $removeHandler(aList[i],"draggesture", AjaxControlToolkit.HTMLEditor.stopDrag);
                }
            } catch(e) {}

            if(AjaxControlToolkit.HTMLEditor.isIE) {
                $addHandler(aList[i],"dragstart", AjaxControlToolkit.HTMLEditor.stopDrag);
            } else {
                $addHandler(aList[i],"draggesture", AjaxControlToolkit.HTMLEditor.stopDrag);
            }
        }
    }
};

AjaxControlToolkit.HTMLEditor.attachedIdAttribute= "obout-attached-id";

AjaxControlToolkit.HTMLEditor.stopDrag = function(ev)
{
  if(ev) ev.stopPropagation();ev.preventDefault();
  return false;
};

AjaxControlToolkit.HTMLEditor.replacingRules =
[
[ "strong"  ,"font-weight"     , "bold"         ],
[ "b"       ,"font-weight"     , "bold"         ],
[ "strong"  ,"font-weight"     , "700"          ],
[ "em"      ,"font-style"      , "italic"       ],
[ "i"       ,"font-style"      , "italic"       ],
[ "u"       ,"text-decoration" , "underline"    ],
[ "strike"  ,"text-decoration" , "line-through" ]
];

AjaxControlToolkit.HTMLEditor.replaceOldTags = function(root,editor) {
    var innerHTML = root.innerHTML;
    var need = false;

    for(var j=0; j < AjaxControlToolkit.HTMLEditor.replacingRules.length; j++) {
        var reg = new RegExp("<"+AjaxControlToolkit.HTMLEditor.replacingRules[j][0]+"[\s>]", "ig");
        if(reg.test(innerHTML)) {
            need = true;
            break;
        }
    }
    
    if(!need) {
        if(!(/<font[\s>]/ig.test(innerHTML))) {
            return;
        }
    }

    for(var i=0; i<root.childNodes.length; i++) {
        var child = root.childNodes.item(i);
        if(child.nodeType == 1) {
            var found = null;
            var childTagName = child.tagName.toLowerCase();
            for(var j=0; j < AjaxControlToolkit.HTMLEditor.replacingRules.length; j++) {
                if(AjaxControlToolkit.HTMLEditor.replacingRules[j][0].toLowerCase() == childTagName) {
                    found = AjaxControlToolkit.HTMLEditor.replacingRules[j];
                    break;
                }
            }

            if(found) {
                var span = editor._doc.createElement("SPAN");

                span.style["cssText"] = child.style["cssText"];

                if(AjaxControlToolkit.HTMLEditor.isIE) {
                    span.style[found[1]] = found[2];
                } else {
                    span.style[found[1].replace(/\-(\w)/g, function (strMatch, p1){return p1.toUpperCase();})] = found[2];
                }
                while(child.firstChild) {
                    span.appendChild(child.firstChild);
                }

                root.insertBefore(span,child);
                root.removeChild(child);
                child = span;
            } else {
                if(childTagName == "font") {
                    var span = editor._doc.createElement("SPAN");
                    var save = child.size;

                    span.style["cssText"] = child.style["cssText"];

                    if(child.color) {
                        span.style.color = child.color;
                    }
                    if(child.face ) {
                        span.style.fontFamily = child.face;
                    }

                    while(child.firstChild) {
                        span.appendChild(child.firstChild);
                    }

                    root.insertBefore(span,child);
                    root.removeChild(child);

                    if(save) {
                        var font = editor._doc.createElement("FONT");
                        font.size = save;
                        root.insertBefore(font,span);

                        if(span.style["cssText"].length > 0) {
                            font.appendChild(span);
                            child = span;
                        } else {
                            while(span.firstChild) {
                                font.appendChild(span.firstChild);
                            }
                            root.removeChild(span);
                            child = font;
                        }
                    } else {
                        child = span;
                    }
                }
            }
            AjaxControlToolkit.HTMLEditor.replaceOldTags(child,editor);
        }
    }
};

AjaxControlToolkit.HTMLEditor.getStyle = function(oElm, strCssRule) {
    var strValue = "";
    if(oElm.nodeType==1) {
        if(oElm.ownerDocument && oElm.ownerDocument.defaultView && oElm.ownerDocument.defaultView.getComputedStyle) {
            strValue = oElm.ownerDocument.defaultView.getComputedStyle(oElm, "").getPropertyValue(strCssRule);
        }
        else if(oElm.currentStyle) {
            try {
                strCssRule = strCssRule.replace(/\-(\w)/g, function (strMatch, p1){return p1.toUpperCase();});
                strValue = oElm.currentStyle[strCssRule];
            } catch(ex) {
                strValue = oElm.style[strCssRule];
            }
        } else {
            strValue = oElm.style[strCssRule];
        }
    }
    return strValue;
};

AjaxControlToolkit.HTMLEditor._Marker = function(editor,rng,sel) {
    if(AjaxControlToolkit.HTMLEditor.isIE) {
        this._nArr  = AjaxControlToolkit.HTMLEditor.getNames(editor._doc.body);
        this._save = editor._doc.body.innerHTML;
        this._tree = null;

        if(sel.type.toLowerCase()=="control") {
            try {
                var el = rng.item(0);
                this._tree  =[];

                while(el && (el.nodeType==3 || !el.tagName || el.tagName.toUpperCase() != "BODY")) {
                    var n=0;
                    while(el.previousSibling) {
                        n++;
                        el = el.previousSibling;
                    }
                    this._tree.push(n);
                    el = el.parentNode;
                }
            } catch(e){}
        } else {
            this._offsetLeft=rng.offsetLeft;
            this._offsetTop =rng.offsetTop;
        }
    } else {
        if(AjaxControlToolkit.HTMLEditor.isOpera) {
            this._save = AjaxControlToolkit.HTMLEditor.Trim(editor._doc.body.innerHTML);
        } else {
            this._save = editor._doc.body.cloneNode(true);
        }

        this._tree  =[];
        this._offset= 0;

        try {
            var el =rng.startContainer;
            this._offset=rng.startOffset;
            if(el && el.nodeType==1 && el.tagName.toUpperCase()=="HTML") {
                el = editor._doc.body;

                setTimeout(function(){
                    try {
                        sel = editor._getSelection();
                        rng = editor._createRange();
                        editor._removeAllRanges(sel);
                        rng.setStart(el,0);
                        rng.setEnd  (el,0);
                        editor._selectRange(sel,rng);
                    } catch(e) {}
                },0);
            }

            while(el && el.nodeType && (el.nodeType==3 || !el.tagName || el.tagName.toUpperCase() != "BODY")) {
                var n=0;
                while(el.previousSibling) {
                    n++;
                    if(AjaxControlToolkit.HTMLEditor.isOpera) {
                        if(el.nodeType == 3 && el.previousSibling != null && el.previousSibling.nodeType == 3) {
                            n--;
                        }
                    }
                    el = el.previousSibling;
                }
                this._tree.push(n);
                el = el.parentNode;
            }
        } catch(e) {}
    }
};

AjaxControlToolkit.HTMLEditor.__stackMaxSize = 30;

AjaxControlToolkit.HTMLEditor.getNames = function(el) {
    var aList = el.all;
    var mArr  = [];
    var nArr =[]

    for(var i=0; i < aList.length; i++) {
        var a   = aList[i];

        if(a.name && a.name.length > 0) {
            var tag = a.tagName;
            var coll= el.getElementsByTagName(tag);
            var n = 0;

            for(var j=0; j < coll.length; j++) {
                if(coll[j] == a) {
                    n = j;
                    break;
                }
            }

            nArr[tag] = n;

            mArr.push([tag,nArr[tag],a.name]);
        }
    }
    return mArr;
};

AjaxControlToolkit.HTMLEditor.setNames = function(el,mArr) {
    for(var i=0; i < mArr.length; i++) {
        if(el.getElementsByTagName(mArr[i][0]).length > mArr[i][1]) {
            el.getElementsByTagName(mArr[i][0])[mArr[i][1]].name = mArr[i][2];
        }
    }
};

AjaxControlToolkit.HTMLEditor._lookChild = function(root,seek) {
    for(var i=0; i<root.childNodes.length; i++) {
        var child = root.childNodes.item(i);
        if(child==seek) {
            return i;
        }
        if(child.nodeType==1) {
            if(AjaxControlToolkit.HTMLEditor._lookChild(child,seek) >= 0 ) {
                return i;
            }
        }
    }
    return -1;
};

AjaxControlToolkit.HTMLEditor.getHrefsText = function(txt) {
    var result =[]
    function regRepl(p0,p1,p2,p3,p4,p5,p6,p7) {
        var tag = p1.replace(/^<([^\s>]+)/,"$1");
        var insert =true;
        var i =0;

        for(; i < result.length; i++) {
            if(result[i][0] == tag) {
                insert = false;
                break;
            }
        }

        if(insert) {
            result[i] =[tag];
        }

        result[i].push(p5);
    };

    var reg = new RegExp("(<[^\\s><]+)([^><]*?)(href=)(\"|')([^\\4]*?)(\\4)((?:[^><]*?)>)","ig");
    txt.replace(reg,regRepl);
    return result;
};

AjaxControlToolkit.HTMLEditor.setHrefsText = function(el, mArr) {
    for (var j = 0; j < mArr.length; j++) {
        var aList = el.getElementsByTagName(mArr[j][0]);
        var k = 1;

        for (var i = 0; i < aList.length; i++) {
            if (!aList[i].href) {
                continue;
            }
            if (mArr[j][k] && mArr[j][k].length > 0) {
                var trickIE;
                if (AjaxControlToolkit.HTMLEditor.isIE) {
                    trickIE = aList[i].innerHTML;
                }
                aList[i].href = mArr[j][k].replace(/&amp;/ig, "&");
                if (AjaxControlToolkit.HTMLEditor.isIE) {
                    aList[i].innerHTML = trickIE;
                }
            }
            k++;
        }
    }
};

AjaxControlToolkit.HTMLEditor.getImagesText = function(txt) {
    var mArr  = [];

    function regRepl(p0,p1,p2,p3,p4,p5) {
        mArr.push(p3);
        return p0;
    }

    txt.replace(/(<img(?:.*?))(src=")(.*?)(")((?:.*?)>)/ig,regRepl);
    return mArr;
};

AjaxControlToolkit.HTMLEditor.setImagesText = function(el,mArr) {
    var aList = el.getElementsByTagName("IMG");
    var k=0;

    for(var i=0; i < aList.length; i++) {
        if(!aList[i].src) {
            continue;
        }
        if(mArr[k] && mArr[k].length > 0) {
            aList[i].src = mArr[k].replace(/&amp;/ig,"&");
        }
        k++;
    }
};

AjaxControlToolkit.HTMLEditor.canHaveChildren = function(elem) {
    if(AjaxControlToolkit.HTMLEditor.isIE) {
        return elem.canHaveChildren;
    } else {
        return !/^(area|base|basefont|col|frame|hr|img|br|input|isindex|link|meta|param)$/.test(elem.tagName.toLowerCase());
    }
};

AjaxControlToolkit.HTMLEditor._setCursor = function(el1,editor) {
    var el = el1;
    if(AjaxControlToolkit.HTMLEditor.isIE) {
        var sel = editor._getSelection();
        var range=editor._createRange(sel);
        
        if(sel.type.toLowerCase() == "control") {
             range.remove(0);
             sel.empty();
             range = editor._createRange();
        }
        
        var isText = (el.nodeType==3);
        var span;

        if(isText)
        {
            span = editor._doc.createElement("SPAN");
            span.innerHTML = "&nbsp;";
            el.parentNode.insertBefore(span,el);
            el = span;
        }

        var location = $common.getLocation(el);
        var _left = location.x, _top = location.y;

        if(isText) {
            span.parentNode.removeChild(span);
        }

        try {
            range.moveToPoint(_left,_top);
        } catch(e) {}
        range.select();
    } else {
        var sel = editor._getSelection();
        var range=editor._createRange();

        range.setStart(el, 0);
        range.setEnd(el, 0);

        editor._removeAllRanges(sel);
        editor._selectRange(sel,range);
        editor.focusEditor();
    }
};

AjaxControlToolkit.HTMLEditor.myClone = function(el,doc,prize) {
    var ela;

    if(AjaxControlToolkit.HTMLEditor.isIE && el.tagName && (el.tagName.toUpperCase()=="EMBED" || el.tagName.toUpperCase()=="OBJECT")) {
        var div = doc.createElement("DIV");

        try {   
            div.innerHTML = el.outerHTML;
            ela = div.firstChild;
        } catch(e) {
            ela = el;
        }

        delete div;
    } else {
        ela = el.cloneNode(prize);
    }

    return ela;
};

AjaxControlToolkit.HTMLEditor.unStyle = function(el) {
    var _prn = (el.parentNode != null && typeof el.parentNode != "undefined")?el.parentNode:null;

    if(_prn) {
        var _fnd = null;

        while ( _prn && _prn.tagName && _prn.tagName.toUpperCase() != "BODY" && AjaxControlToolkit.HTMLEditor.isStyleTag(_prn.tagName) &&
               (_prn.tagName.toUpperCase() != "A")) {
            _fnd = _prn;
            _prn = _prn.parentNode;
        }

        if(_fnd) {
            function diver(add,el, rpr, before, prize) {
                var par=rpr.cloneNode(false);

                if(add) {
                    if(add.push && typeof add.push == "function") {
                        for(var iii=0; iii < add.length; iii++) {
                            par.appendChild(add[iii]);
                        }
                    } else {
                        par.appendChild(add);
                    }
                }

                if(prize) {
                    par.appendChild(el);
                } else {
                    while(el) {
                        var elSibling=before?el.previousSibling:el.nextSibling;
                        if(el.nodeType==1 || (el.nodeType==3 && AjaxControlToolkit.HTMLEditor.Trim(""+el.data+"").length>0)) {
                            if(el.nodeType==1) {
                                if(el.tagName && AjaxControlToolkit.HTMLEditor.isStyleTag(el.tagName) && el.childNodes.length==0 && !AjaxControlToolkit.HTMLEditor.isTempElement(el)) {
                                    el=null;
                                }
                            }
                            if(el) {
                                if(par.childNodes.length == 0 || !before) {
                                    par.appendChild(el);
                                } else {
                                    par.insertBefore(el,par.firstChild);
                                }
                            }
                        }
                        el=elSibling;
                    }
                }

                if(par.childNodes.length==0) {
                    delete par;
                    par=null;
                }
                else if(par.childNodes.length==1 && par.firstChild.nodeType==3 && (""+par.firstChild.data+"").length==0) {
                    delete par;
                    par=null;
                }  else  {
                    if(!prize && par.tagName && AjaxControlToolkit.HTMLEditor.isStyleTag(par.tagName) && (par.tagName.toUpperCase() != "A") && !AjaxControlToolkit.HTMLEditor.isTempElement(par)) {
                        var elNumber = par.childNodes.length;
                        for(var cnt=0; cnt< par.childNodes.length; cnt++) {
                            var inn = par.childNodes.item(cnt);
                            if(inn.nodeType==1 && inn.tagName && !AjaxControlToolkit.HTMLEditor.isStyleTag(inn.tagName) &&
                               (inn.tagName.toUpperCase()=="BR" || inn.tagName.toUpperCase()=="TABLE" ||
                               AjaxControlToolkit.HTMLEditor.isTempElement(inn))) {
                                elNumber--;
                            }
                        }

                        if(elNumber == 0) {
                            var parr = [];
                            while(par.firstChild) {
                                var inn = par.removeChild(par.firstChild);
                                parr.push(inn);
                            }
                            par = parr;
                        }
                    }
                }

                if(rpr==_fnd) {
                    return par;
                } else {
                    if(!prize) {
                        return diver(par,before?rpr.previousSibling:rpr.nextSibling,rpr.parentNode,before,prize);
                    } else {
                        return diver(null,par,rpr.parentNode,before,prize);
                    }
                }
            };

            _prn = el.parentNode;

            if( el.previousSibling == null && el.nextSibling == null &&
                _prn && _prn.tagName && _prn.tagName.toUpperCase() != "BODY" && AjaxControlToolkit.HTMLEditor.isStyleTag(_prn.tagName) &&
                AjaxControlToolkit.HTMLEditor.differAttr(_prn,["class","color","face","size"]).length > 0) {
                el = _prn;
            }

            var p1 = diver(null,el.previousSibling,el.parentNode,true , false);
            var p2 = diver(null,el.nextSibling    ,el.parentNode,false, false);

            var par    = _fnd.parentNode;
            if(p1) {
                if(p1.push && typeof p1.push == "function") {
                    for(var iii=0; iii < p1.length; iii++) {
                        par.insertBefore(p1[iii],_fnd);
                    }
                } else {
                    par.insertBefore(p1,_fnd);
                }
            }

            if(el.nodeType==1 && el.tagName &&
               (el.tagName.toUpperCase()=="BR" || el.tagName.toUpperCase()=="TABLE" ||
               AjaxControlToolkit.HTMLEditor.isTempElement(el))) {
                par.insertBefore(el,_fnd);
            } else {
                var p3 = diver(null,el,el.parentNode,false, true);
                par.insertBefore(p3,_fnd);
            }

            if(p2) {
                if(p2.push && typeof p2.push == "function") {
                    for(var iii=0; iii < p2.length; iii++) {
                        par.insertBefore(p2[iii],_fnd);
                    }
                } else {
                    par.insertBefore(p2,_fnd);
                }
            }
            par.removeChild (_fnd);
        }
    }
};

AjaxControlToolkit.HTMLEditor.isTempElement = function(el) {
    if(el.id && el.id.length > 0 && el.id.indexOf(AjaxControlToolkit.HTMLEditor.smartClassName) >= 0) {
        return true;
    }
    return false;
};

AjaxControlToolkit.HTMLEditor._moveTagsUp = function(lBound,rBound) {
    function _dive(next) {
        if(!AjaxControlToolkit.HTMLEditor.isInlineElement(next) || next.nodeType == 3) {
            AjaxControlToolkit.HTMLEditor.unStyle(next);
        } else if(next.tagName && AjaxControlToolkit.HTMLEditor.isStyleTag(next.tagName) && (next.tagName.toUpperCase() != "A") && !AjaxControlToolkit.HTMLEditor.isTempElement(next)) {
            var nnn = next.firstChild;
            while(nnn != null) {
                var nnnNext = nnn.nextSibling;
                _dive(nnn);
                nnn = nnnNext;
            }
        }
    }

    var next = lBound;
    while(next != null && next != rBound) {
        var nextSibling = next.nextSibling;
        _dive(next);
        next = nextSibling;
    }
};

AjaxControlToolkit.HTMLEditor._commonTotalParent = function(first,last) {
    var ret = null;
    var par = first.parentNode;
    var fst = first;

    while (par) {
        if(par.tagName && !AjaxControlToolkit.HTMLEditor.isStyleTag(par.tagName)) {
            var indexLast = AjaxControlToolkit.HTMLEditor._lookChild(par,last);

            if(indexLast >=0 ) {
                var indexFirst = 0;

                for(var i=0; i < par.childNodes.length; i++) {
                    if(par.childNodes.item(i) == fst) {
                        indexFirst = i;
                        break;
                    }
                }

                return {parent: par, indexFirst: indexFirst, indexLast: indexLast};
            }
        }

        fst = par;
        par = par.parentNode;
    }

    return ret;
};

AjaxControlToolkit.HTMLEditor._commonParent = function(first,last) {
    var ret = null;
    var par = first.parentNode;
    var fst = first;

    while (par && par.tagName.toUpperCase() != "BODY" && AjaxControlToolkit.HTMLEditor.isStyleTag(par.tagName)) {
        var indexLast = AjaxControlToolkit.HTMLEditor._lookChild(par,last);

        if(indexLast >=0 ) {
            var indexFirst = 0;

            for(var i=0; i < par.childNodes.length; i++) {
                if(par.childNodes.item(i) == fst) {
                    indexFirst = i;
                    break;
                }
            }

            return {parent: par, indexFirst: indexFirst, indexLast: indexLast};
        }

        fst = par;
        par = par.parentNode;
    }

    return ret;
};

AjaxControlToolkit.HTMLEditor.positionInParagraph = function(marker,el,left,par,wordBound) {
    while(true){
        var result = AjaxControlToolkit.HTMLEditor.positionInParagraphLevel(marker,el,left,wordBound);
        if(result != null) {
            return result;
        }
        if(par.tagName && AjaxControlToolkit.HTMLEditor.isStyleTag(par.tagName) && (par.tagName.toUpperCase() != "A") && !AjaxControlToolkit.HTMLEditor.isTempElement(par)) {
            el  = left?par.previousSibling:par.nextSibling;
            par = par.parentNode;
        } else {
            if(!left || par.firstChild == null) {
                par.appendChild(marker);
            } else {
                par.insertBefore(marker,par.firstChild);
            }
            return marker;
        }
    }
};

AjaxControlToolkit.HTMLEditor.positionInParagraphLevel = function(marker,el,left,wordBound) {
    while(el) {
        var elSibling = left?el.previousSibling:el.nextSibling;

        if(!AjaxControlToolkit.HTMLEditor.isInlineElement(el)) {
            var par = el.parentNode;

            if(!left) {
                par.insertBefore(marker,el);
            } else{
                if(el.nextSibling) {
                    par.insertBefore(marker,el.nextSibling);
                } else {
                    par.appendChild (marker);
                }
            }
            return marker;
        }
        else if(typeof wordBound == "function" && el.nodeType==3) {
            var j;
            var str = ""+el.data+"";
            if(left) {
                for(j=str.length-1; j >= 0; j--) {
                    if(wordBound(str.substr(j,1))) {
                        break;
                    }
                }
            } else {
                for(j=0; j < str.length; j++) {
                    if(wordBound(str.substr(j,1))) {
                        break;
                    }
                }
            }

            if(j >= 0 && j < str.length) {
                var par = el.parentNode;
                var newNode;

                if((j > 0 || (left && j==0)) && (j < str.length-1 || (!left && j==str.length-1))) {
                    if(left) {
                        newNode = el.splitText(j+1);
                    } else {
                        newNode = el.splitText(j);
                    }
                    par.insertBefore(marker,newNode);
                } else {
                    if(!left) {
                        par.insertBefore(marker,el);
                    } else {
                        if(el.nextSibling) {
                            par.insertBefore(marker,el.nextSibling);
                        } else {
                            par.appendChild (marker);
                        }
                    }
                }
                return marker;
            }
        }

        el = left?el.lastChild:el.firstChild;

        if(el) {
            var result = AjaxControlToolkit.HTMLEditor.positionInParagraphLevel(marker,el,left,wordBound);
            if(result != null) {
                return result;
            }
        }
        el=elSibling;
    }

    return null;
};

AjaxControlToolkit.HTMLEditor._addEvent = function(el, evname, func){
    if(el.attachEvent)
        el.attachEvent("on" + evname, func);
    else if(el.addEventListener)
        el.addEventListener(evname, func, true);
};

AjaxControlToolkit.HTMLEditor._addEvents = function(el, evs, func) {
    for(var i=0; i < evs.length; i++)
        AjaxControlToolkit.HTMLEditor._addEvent(el, evs[i], func);
};

AjaxControlToolkit.HTMLEditor._removeEvent = function(el, evname, func) {
    if(el.detachEvent)
        el.detachEvent("on" + evname, func);
    else if(el.removeEventListener)
        el.removeEventListener(evname, func, true);
};

AjaxControlToolkit.HTMLEditor._removeEvents = function(el, evs, func) {
    for(var i=0; i < evs.length; i++)
        AjaxControlToolkit.HTMLEditor._removeEvent(el, evs[i], func);
};

AjaxControlToolkit.HTMLEditor._stopEvent = function(ev) {
    if(ev) {
        if (AjaxControlToolkit.HTMLEditor.isIE) {
            ev.cancelBubble = true;
            ev.returnValue = false;
        } else {
            ev.preventDefault();
            ev.stopPropagation();
        }
    }
};

AjaxControlToolkit.HTMLEditor.restrictedTags = ["DIV","P","TD","TR","TABLE","TBODY","LI","OL","UL","FORM","INPUT"];  // this list can be increased
AjaxControlToolkit.HTMLEditor.isRestricted = function(element) {
    var elementTagName = element.tagName.toUpperCase();
    for(var i=0; i < AjaxControlToolkit.HTMLEditor.restrictedTags.length; i++) {
        if (AjaxControlToolkit.HTMLEditor.restrictedTags[i].toUpperCase() == elementTagName) {
            return true;
        }
    }

    if(AjaxControlToolkit.HTMLEditor.isIE && element.scopeName.toUpperCase()!="HTML") {
        return true;
    }

    return false;
};

AjaxControlToolkit.HTMLEditor.jsDocument = function(noExtraLf) {
    this.noExtraLf = (typeof noExtraLf != "undefined" && noExtraLf);
    this.text = [];                //array to store the string
    this.write = function (str) {
        if(!this.noExtraLf || (this.text.length == 0 && str != "\n") || (this.text.length > 0 && (this.text[this.text.length-1] != "\n" || str != "\n"))) {
            this.text[this.text.length] = str;
        }
    };
    this.append = this.write;
    this.writeln = function (str) { this.text[this.text.length] = str + "\n"; }
    this.toString = function () { return this.text.join(""); }
    this.clear = function () { delete this.text; this.text = null; this.text = new Array; }
};

AjaxControlToolkit.HTMLEditor.isHeader = function(el) {
    var name = el.tagName.toUpperCase();

    if(name.length==2) {
        if(name.substr(0,1)=="H" && parseInt(name.substr(1,1)) > 0) {
            return true;
        }
    }
    return false;
};

AjaxControlToolkit.HTMLEditor._getReallyFirst = function(root) {
    if(typeof root.firstChild != "undefined" && root.firstChild != null) {
        if(typeof root.firstChild.childNodes != "undefined" && root.firstChild.childNodes != null) {
            return AjaxControlToolkit.HTMLEditor._getReallyFirst(root.firstChild)
        }
    }
    return root;
};

AjaxControlToolkit.HTMLEditor._getReallyLast = function(root) {
    if(typeof root.lastChild != "undefined" && root.lastChild != null) {
        if(typeof root.lastChild.childNodes != "undefined" && root.lastChild.childNodes != null) {
            return AjaxControlToolkit.HTMLEditor._getReallyLast(root.lastChild)
        }
    }
    return root;
};

AjaxControlToolkit.HTMLEditor._reallyFirst = function(root,seek) {
    if(root.firstChild) {
        if(root.firstChild == seek) return true;
        if(root.firstChild.childNodes)
            if(AjaxControlToolkit.HTMLEditor._lookChild(root.firstChild,seek) == 0 ) {
                return AjaxControlToolkit.HTMLEditor._reallyFirst(root.firstChild,seek)
            }
    }
    return false;
};

AjaxControlToolkit.HTMLEditor._reallyLast = function(root,seek) {
    if(root.lastChild) {
        if(root.lastChild == seek) return true;
        if(root.lastChild.childNodes)
            if(AjaxControlToolkit.HTMLEditor._lookChild(root.lastChild,seek) == root.lastChild.childNodes.length-1 ) {
                return AjaxControlToolkit.HTMLEditor._reallyLast(root.lastChild,seek)
            }
    }
    return false;
};

AjaxControlToolkit.HTMLEditor.getContainer = function(container, el) {
    if(el==container) return container;

    if(container.nodeType == 1) {
        for(var i=0; i < container.childNodes.length; i++) {
            var child = container.childNodes.item(i);
            if(el==child) return child;

            if(child.nodeType == 1) {
                var ind = AjaxControlToolkit.HTMLEditor._lookChild(child,el);
                if(ind >= 0) {
                    if(child.tagName && AjaxControlToolkit.HTMLEditor.isStyleTag(child.tagName) && (child.tagName.toUpperCase() != "A") && !AjaxControlToolkit.HTMLEditor.isTempElement(child))
                        return AjaxControlToolkit.HTMLEditor.getContainer(child, el);
                    else
                        return child;
                }
            }
        }
    }
    return null;
};

AjaxControlToolkit.HTMLEditor._TryTransformFromPxToPt = function(fontSize,editor, _id) {
    var ret = fontSize.replace(/^(\d+)\.(\d+)px/i,"$1px");

    if(!AjaxControlToolkit.HTMLEditor.isIE) {
        if(ret && ret.length > 0) {
            var seek = ret.toLowerCase().split(",")[0];

            if (typeof _id != "undefined") {
                var el   = document.getElementById(_id);

                if(el != null) {
                    var i;
                    for(i=0; i< el.options.length; i++) {
                        var cur = AjaxControlToolkit.HTMLEditor.fontSizeSeek(el.options.item(i).value.toLowerCase().split(",")[0]);

                        if(cur==seek) break;
                    }
                    if(i==el.options.length) {
                        var span = editor._doc.createElement("SPAN");
                        editor._doc.body.appendChild(span);

                        for(i=1; i< 100; i++) {
                            span.style.fontSize = i+"pt";
                            if(AjaxControlToolkit.HTMLEditor.getStyle(span,"font-size").replace(/^(\d+)\.(\d+)px/i,"$1px") == seek) {
                                seek = i+"pt";
                                break;
                            }
                        }
                        span.parentNode.removeChild(span);
                    }
                }
            }
            ret = seek;
        }
    }
    return ret;
};

AjaxControlToolkit.HTMLEditor.fontSizeSeek = function(val) {
    var seek = val.toString();

    switch (seek) {
        case "1":
            seek="8pt";
            break;
        case "2":
            seek="10pt";
            break;
        case "3":
            seek="12pt";
            break;
        case "4":
            seek="14pt";
            break;
        case "5":
            seek="18pt";
            break;
        case "6":
            seek="24pt";
            break;
        case "7":
            seek="36pt";
            break;
    }

    return seek;
};

AjaxControlToolkit.HTMLEditor.getOwnerDocument = function(node) {
    return node.nodeType == 9 ? node : node.ownerDocument || node.document;
};

AjaxControlToolkit.HTMLEditor.getClientViewportElement = function(opt_node) {
    var doc;

    if (opt_node.nodeType == 9) {
        doc = opt_node;
    } else {
        doc = AjaxControlToolkit.HTMLEditor.getOwnerDocument(opt_node);
    }

    if (AjaxControlToolkit.HTMLEditor.isIE && doc.compatMode != 'CSS1Compat') {
        return doc.body;
    }
    return doc.documentElement;
};

AjaxControlToolkit.HTMLEditor.isReallyVisible = function(el) {
    var elem = el;
    var real_visible = true;

    while(elem) {
        if(elem.style && AjaxControlToolkit.HTMLEditor.getStyle(elem,"display") == "none") {
            real_visible = false;
            break;
        }
        elem = elem.parentNode;
    }
    return real_visible;
}


AjaxControlToolkit.HTMLEditor.setSelectionRange = function(input, selectionStart, selectionEnd) {
    input.focus();
    if (input.setSelectionRange) {
        input.setSelectionRange(selectionStart, selectionEnd);
    }
    else if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    }
};

AjaxControlToolkit.HTMLEditor.setElementVisibility = function(element) {
    var ret = new Array();
    var elem = element;

    while(elem && elem.nodeType==1 && elem.tagName.toUpperCase() != "BODY") {
        var display = elem.style.display;
        var visibility = elem.style.visibility;
        if(elem.style && (display == "none" || visibility == "hidden")) {
            ret.push({element: elem, display: display, visibility: visibility});
            elem.style.display = "";
            elem.style.visibility = "visible";
        }
        elem = elem.parentNode;
    }
    return ret;
};

AjaxControlToolkit.HTMLEditor.restoreElementVisibility = function(arr) {
    for(var i=0; i < arr.length; i++) {
        var item = arr[i];
        var style = item.element.style;
        style.display = item.display;
        style.visibility = item.visibility;
    }
};

if(!AjaxControlToolkit.HTMLEditor.isIE) {
    try { //not all such browsers support getter/setter
        AjaxControlToolkit.HTMLEditor.__MozillaGetInnerText = function(node, html) {
            var els=node.childNodes;
            for(var i=0;i<els.length;i++) {
                var elem = els[i];

                if(elem.nodeType == 3) {
                    html.write(elem.nodeValue.replace("\n",""));
                }
                if(elem.nodeType == 1) {
                    var display    =  AjaxControlToolkit.HTMLEditor.getStyle(elem,"display");
                    var visibility =  AjaxControlToolkit.HTMLEditor.getStyle(elem,"visibility");

                    if(AjaxControlToolkit.HTMLEditor.__needLineBreakBefore(elem)) {
                        html.write("\n");
                    }

                    if(AjaxControlToolkit.HTMLEditor.__needTabBefore(elem)) {
                        html.write("\t");
                    }

                    if(display != "none" && visibility != "hidden") {
                        AjaxControlToolkit.HTMLEditor.__MozillaGetInnerText(elem,html);
                    }

                    if(AjaxControlToolkit.HTMLEditor.__needLineBreakAfter(elem)) {
                        html.write("\n");
                    }
                }
            }
        };

        AjaxControlToolkit.HTMLEditor.__needLineBreakBefore = function(el)
        {
            var _Tags = " div table p pre ol ul blockquote form fieldset ";
            return (_Tags.indexOf(" " + el.tagName.toLowerCase() + " ") != -1);
        };

        AjaxControlToolkit.HTMLEditor.__needLineBreakAfter = function(el)
        {
            var _Tags = " br div table tr p pre ol ul li hr blockquote form fieldset legend ";
            return (_Tags.indexOf(" " + el.tagName.toLowerCase() + " ") != -1);
        };

        AjaxControlToolkit.HTMLEditor.__needTabBefore = function(el)
        {
            var _Tags = " td li ";
            return (_Tags.indexOf(" " + el.tagName.toLowerCase() + " ") != -1);
        };

//        HTMLElement.prototype.__defineGetter__("innerText",function() {
//                var html = new AjaxControlToolkit.HTMLEditor.jsDocument(true);
//                AjaxControlToolkit.HTMLEditor.__MozillaGetInnerText(this,html);
//                return html.toString();
//            }
//        );
    } catch(ex) {}
}

