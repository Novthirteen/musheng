
if(typeof(rdcjs)=="undefined")
_rdc=rdcjs={};_rdc.IE6SelectHelper=function(){this.selectList=new Array();}
_rdc.IE6SelectHelper.prototype={isInRange:function(elem,containerId){var containerDiv=document.getElementById(containerId);if(!containerDiv)
return false;var elemX1=this.getX(elem);var elemY1=this.getY(elem);var elemX2=elem.offsetWidth+elemX1;var elemY2=elem.offsetHeight+elemY1;var containerX1=this.getX(containerDiv);var containerY1=this.getY(containerDiv);var containerX2=containerDiv.offsetWidth+containerX1;var containerY2=containerDiv.offsetHeight+containerY1;if(elemX1<containerX1&&elemX2<containerX1)
return false;if(elemX1>containerX2&&elemX2>containerX2)
return false;if(elemY1<containerY1&&elemY2<containerY1)
return false;if(elemY1>containerY2&&elemY2>containerY2)
return false;return true;},hideSelect:function(containerId){if(this.msieversion()<=6&&document.all){var selects=document.getElementsByTagName("select");for(var i=0;i<selects.length;i++){var oneSelect=selects[i];if(!this.isInRange(oneSelect,containerId))
continue;if(oneSelect.style.visibility!="hidden"){oneSelect.style.visibility="hidden";this.selectList.push(oneSelect);}}}},showSelect:function(){for(var i=0;i<this.selectList.length;i++)
this.selectList[i].style.visibility="visible";while(this.selectList.length>0)
this.selectList.pop();},getX:function(oElement)
{var iReturnValue=0;while(oElement!=null){iReturnValue+=oElement.offsetLeft;oElement=oElement.offsetParent;}
return iReturnValue;},getY:function(oElement)
{var iReturnValue=0;while(oElement!=null){iReturnValue+=oElement.offsetTop;oElement=oElement.offsetParent;}
return iReturnValue;},msieversion:function()
{var ua=window.navigator.userAgent
var msie=ua.indexOf("MSIE ")
if(msie>0)
return parseInt(ua.substring(msie+5,ua.indexOf(".",msie)))
else
return 0}}
_rdc.ASContextMenuHelper=function(){}
_rdc.ASContextMenuHelper.Bind=function(object,fun){return function(){return fun.apply(object,arguments);}}
_rdc.ASContextMenuHelper.BindAsEventListener=function(object,fun){return function(event){return fun.call(object,(event||window.event));}}
_rdc.ASContextMenuHelper.addEvent=function(oTarget,sEventType,fnHandler){if(oTarget.addEventListener){oTarget.addEventListener(sEventType,fnHandler,false);}else if(oTarget.attachEvent){oTarget.attachEvent("on"+sEventType,fnHandler);}else{oTarget["on"+sEventType]=fnHandler;}};_rdc.ASContextMenuHelper.removeEvent=function(oTarget,sEventType,fnHandler){if(oTarget.removeEventListener){oTarget.removeEventListener(sEventType,fnHandler,false);}else if(oTarget.detachEvent){oTarget.detachEvent("on"+sEventType,fnHandler);}else{oTarget["on"+sEventType]=null;}};_rdc.ASContextMenuHelper.cancelEvent=function(e){e=e||window.event;if(e.preventDefault){e.preventDefault();e.stopPropagation();}else{e.returnValue=false;e.cancelBubble=true;}}
_rdc.ASContextMenuHelper.getEventTarget=function(e){e=e||window.event;var obj=e.srcElement?e.srcElement:e.target;return obj}
_rdc.ASContextMenuHelper.isIE=(document.all)?true:false;_rdc.ASContextMenuHelper.prev=function(elem){do{elem=elem.previousSibling;}while(elem&&elem.nodeType!=1);return elem;}
_rdc.ASContextMenuHelper.next=function(elem){do{elem=elem.nextSibling;}while(elem&&elem.nodeType!=1);return elem;}
_rdc.ASContextMenuHelper.first=function(elem){elem=elem.firstChild;return elem&&elem.nodeType!=1?_rdc.next(elem):elem;}
_rdc.ASContextMenuHelper.last=function(elem){elem=elem.lastChild;return elem&&elem.nodeType!=1?_rdc.prev(elem):elem;}
_rdc.ASContextMenuHelper.parent=function(elem,num){num=num||1;for(var i=0;i<num;i++)
if(elem!=null)
elem=elem.parentNode;return elem;}
_rdc.ASContextMenuHelper.mouseX=function(e){var posx=0;if(!e)var e=window.event;if(e.pageX){posx=e.pageX;}
else if(e.clientX){posx=e.clientX+document.body.scrollLeft
+document.documentElement.scrollLeft;}
return posx;}
_rdc.ASContextMenuHelper.mouseY=function(e){var posy=0;if(!e)var e=window.event;if(e.pageY){posy=e.pageY;}
else if(e.clientY){posy=e.clientY+document.body.scrollTop
+document.documentElement.scrollTop;}
return posy;}
_rdc.ASContextMenu=function(){this.a11=new Array();this.a10=null;this.a9=null;this.a12=true;this.a13=true;this.a15=null;this.a16=new _rdc.IE6SelectHelper();this.a3=_rdc.ASContextMenuHelper.BindAsEventListener(this,this.a4);this.a5=_rdc.ASContextMenuHelper.BindAsEventListener(this,this.a6);this._cNames=null;}
_rdc.ASContextMenu.IsContextMenuShowed={};_rdc.ASContextMenu.prototype={setup:function(conf){if(document.all&&document.getElementById&&!window.opera){this.IE=true;}
if(!document.all&&document.getElementById&&!window.opera){this.FF=true;}
if(navigator.appName.indexOf("Opera")!=-1){this.OP=true;}
if(this.IE||this.FF){_rdc.ASContextMenuHelper.addEvent(document,"contextmenu",this.a3);_rdc.ASContextMenuHelper.addEvent(document,"click",this.a5);if(conf&&typeof(conf.preventDefault)!="undefined"){this.a12=conf.preventDefault;}
if(conf&&typeof(conf.preventForms)!="undefined"){this.a13=conf.preventForms;}}
else if(this.OP){_rdc.ASContextMenuHelper.addEvent(document,"mousedown",this.a3);_rdc.ASContextMenuHelper.addEvent(document,"mousedown",this.a5);}},getSelectedItem:function(){return this.a15;},attachContextMenu:function(classNames,menuId){if(typeof(classNames)=="string"){this.a11[classNames]=menuId;}
if(typeof(classNames)=="object"){for(x=0;x<classNames.length;x++){this.a11[classNames[x]]=menuId;}}
this._cNames=classNames;},a1:function(e){if(this.IE){this.a10=event.srcElement;}else{this.a10=e.target;}
while(this.a10!=null){var className=this.a10.className;if(typeof(className)!="undefined"){className=className.replace(/^\s+/g,"").replace(/\s+$/g,"")
var classArray=className.split(/[ ]+/g);for(i=0;i<classArray.length;i++){if(this.a11[classArray[i]]){return this.a11[classArray[i]];}}}
if(this.IE){this.a10=this.a10.parentElement;}else{this.a10=this.a10.parentNode;}}
return null;},a2:function(e){var isContextMenuShowed=false;for(var k in _rdc.ASContextMenu.IsContextMenuShowed){if(k.indexOf("contextmenumark")!=-1)
if(_rdc.ASContextMenu.IsContextMenuShowed[k])
isContextMenuShowed=true;}
if(isContextMenuShowed)
return false;var returnValue=true;var evt=this.IE?window.event:e;if(evt.button!=1){if(evt.target){var el=evt.target;}else if(evt.srcElement){var el=evt.srcElement;}
var tname=el.tagName.toLowerCase();if((tname=="input"||tname=="textarea")){if(!this.a13){returnValue=true;}else{returnValue=false;}}else{if(!this.a12){returnValue=true;}else{returnValue=false;}}}
return returnValue;},getMenuA:function(li){var menuAs=li.getElementsByTagName("A");if(menuAs.length==0)
return null;return menuAs[0];},_findRealTarget:function(el){var p=el.parentNode;while(p){if(p&&p.className&&p.className.indexOf(this._cNames)>=0)
return p;p=p.parentNode;}
return null;},a4:function(e){e=e||window.event;if(this.OP){if(!e.ctrlKey)
return;}
var elem=e.srcElement||e.target;if(elem&&elem.className.indexOf(this._cNames)<0){elem=this._findRealTarget(elem);}
if(!elem)
return;var menuElementId=this.a1(e);if(menuElementId){this.a9=document.getElementById(menuElementId);this.a9.style.display='block';this._setElementByMousePosition(e,this.a9);this.a15=elem;this.a16.hideSelect(menuElementId);if(!_rdc.ASContextMenuHelper.isIE){e.preventDefault();}
for(var i=0;i<this.a9.childNodes.length;i++){var curMenuItem=this.a9.childNodes[i];if(curMenuItem.tagName=="LI"){curMenuItem.style.display="";var menuA=this.getMenuA(curMenuItem);if(menuA){var cmdName=menuA.getAttribute("cmdName");if(cmdName){if(elem.getAttribute("disable"+cmdName))
curMenuItem.style.display="none";}}}}
_rdc.ASContextMenu.IsContextMenuShowed["contextmenumark"+menuElementId]=true;return false;}
else
this.a6(e);return this.a2(e);},a6:function(e){if(this.OP){if(e.ctrlKey)
return;}
if(this.a9){this.a9.style.display='none';this.a16.showSelect();_rdc.ASContextMenu.IsContextMenuShowed["contextmenumark"+this.a9.id]=false;}},a7:function(e){var position={'x':_rdc.ASContextMenuHelper.mouseX(e),'y':_rdc.ASContextMenuHelper.mouseY(e)}
return position;},_setElementByMousePosition:function(e,elem){var m=this.a7(e);var offsetTotalX=0;var offsetTotalY=0;var pOffsetElem=elem.offsetParent;while(pOffsetElem){offsetTotalX+=pOffsetElem.offsetLeft;offsetTotalY+=pOffsetElem.offsetTop;pOffsetElem=pOffsetElem.offsetParent;}
elem.style.left=(m.x-offsetTotalX)+'px';elem.style.top=(m.y-offsetTotalY)+'px';},a8:function(){var x=0;var y=0;if(typeof(window.pageYOffset)=='number'){x=window.pageXOffset;y=window.pageYOffset;}else if(document.documentElement&&(document.documentElement.scrollLeft||document.documentElement.scrollTop)){x=document.documentElement.scrollLeft;y=document.documentElement.scrollTop;}else if(document.body&&(document.body.scrollLeft||document.body.scrollTop)){x=document.body.scrollLeft;y=document.body.scrollTop;}
var position={'x':x,'y':y}
return position;}}