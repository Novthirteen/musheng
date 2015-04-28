Type.registerNamespace("AjaxControlToolkit.Seadragon");Type.registerNamespace("Seadragon");var QUOTA=100,MIN_PIXEL_RATIO=.5,browser=Seadragon.Utils.getBrowser(),browserVer=Seadragon.Utils.getBrowserVersion(),subpixelRenders=browser==Seadragon.Browser.FIREFOX||browser==Seadragon.Browser.OPERA||browser==Seadragon.Browser.SAFARI&&browserVer>=4||browser==Seadragon.Browser.CHROME&&browserVer>=2,useCanvas=typeof document.createElement("canvas").getContext=="function"&&subpixelRenders;AjaxControlToolkit.Seadragon.Tile=function(e,g,h,c,d,f){var b=null,a=this;a.level=e;a.x=g;a.y=h;a.bounds=c;a.exists=d;a.loaded=false;a.loading=false;a.elmt=b;a.image=b;a.url=f;a.style=b;a.position=b;a.size=b;a.blendStart=b;a.opacity=b;a.distance=b;a.visibility=b;a.beingDrawn=false;a.lastTouchTime=0};AjaxControlToolkit.Seadragon.Tile.prototype={dispose:function(){},toString:function(){return this.level+"/"+this.x+"_"+this.y},drawHTML:function(e){var b="px",a=this;if(!a.loaded){Seadragon.Debug.error("Attempting to draw tile "+a.toString()+" when it's not yet loaded.");return}if(!a.elmt){a.elmt=Seadragon.Utils.makeNeutralElement("img");a.elmt.src=a.url;a.style=a.elmt.style;a.style.position="absolute";a.style.msInterpolationMode="nearest-neighbor"}var d=a.elmt,c=a.style,f=a.position.apply(Math.floor),g=a.size.apply(Math.ceil);d.parentNode!=e&&e.appendChild(d);c.left=f.x+b;c.top=f.y+b;c.width=g.x+b;c.height=g.y+b;Seadragon.Utils.setElementOpacity(d,a.opacity)},drawCanvas:function(c){var a=this;if(!a.loaded){Seadragon.Debug.error("Attempting to draw tile "+a.toString()+" when it's not yet loaded.");return}var b=a.position,d=a.size;c.globalAlpha=a.opacity;c.drawImage(a.image,b.x,b.y,d.x,d.y)},unload:function(){var a=this;a.elmt&&a.elmt.parentNode&&a.elmt.parentNode.removeChild(a.elmt);a.elmt=null;a.image=null;a.loaded=false;a.loading=false}};AjaxControlToolkit.Seadragon.Tile.registerClass("AjaxControlToolkit.Seadragon.Tile",null,Sys.IDisposable);AjaxControlToolkit.Seadragon.Overlay=function(c,a,d){var b=this;b.elmt=c;b.scales=a instanceof AjaxControlToolkit.Seadragon.Rect;b.bounds=new AjaxControlToolkit.Seadragon.Rect(a.x,a.y,a.width,a.height);b.placement=a instanceof AjaxControlToolkit.Seadragon.Point?d:AjaxControlToolkit.Seadragon.OverlayPlacement.TOP_LEFT;b.position=new AjaxControlToolkit.Seadragon.Point(a.x,a.y);b.size=new AjaxControlToolkit.Seadragon.Point(a.width,a.height);b.style=c.style};AjaxControlToolkit.Seadragon.Overlay.prototype={adjust:function(a,b){switch(this.placement){case AjaxControlToolkit.Seadragon.OverlayPlacement.TOP_LEFT:break;case AjaxControlToolkit.Seadragon.OverlayPlacement.TOP:a.x-=b.x/2;break;case AjaxControlToolkit.Seadragon.OverlayPlacement.TOP_RIGHT:a.x-=b.x;break;case AjaxControlToolkit.Seadragon.OverlayPlacement.RIGHT:a.x-=b.x;a.y-=b.y/2;break;case AjaxControlToolkit.Seadragon.OverlayPlacement.BOTTOM_RIGHT:a.x-=b.x;a.y-=b.y;break;case AjaxControlToolkit.Seadragon.OverlayPlacement.BOTTOM:a.x-=b.x/2;a.y-=b.y;break;case AjaxControlToolkit.Seadragon.OverlayPlacement.BOTTOM_LEFT:a.y-=b.y;break;case AjaxControlToolkit.Seadragon.OverlayPlacement.LEFT:a.y-=b.y/2;break;case AjaxControlToolkit.Seadragon.OverlayPlacement.CENTER:default:a.x-=b.x/2;a.y-=b.y/2}},destroy:function(){var b=this.elmt,a=this.style;b.parentNode&&b.parentNode.removeChild(b);a.top="";a.left="";a.position="";if(this.scales){a.width="";a.height=""}},drawHTML:function(g){var e="px",a=this,f=a.elmt,c=a.style,h=a.scales;f.parentNode!=g&&g.appendChild(f);if(!h)a.size=Seadragon.Utils.getElementSize(f);var b=a.position,d=a.size;a.adjust(b,d);b=b.apply(Math.floor);d=d.apply(Math.ceil);c.left=b.x+e;c.top=b.y+e;c.position="absolute";if(h){c.width=d.x+e;c.height=d.y+e}},update:function(a,b){this.scales=a instanceof AjaxControlToolkit.Seadragon.Rect;this.bounds=new AjaxControlToolkit.Seadragon.Rect(a.x,a.y,a.width,a.height);this.placement=a instanceof AjaxControlToolkit.Seadragon.Point?b:AjaxControlToolkit.Seadragon.OverlayPlacement.TOP_LEFT}};AjaxControlToolkit.Seadragon.Overlay.registerClass("AjaxControlToolkit.Seadragon.Overlay",null,Sys.IDisposable);AjaxControlToolkit.Seadragon.Drawer=function(b,c,d){var a=this;a._container=Seadragon.Utils.getElement(d);a._canvas=Seadragon.Utils.makeNeutralElement(useCanvas?"canvas":"div");a._context=useCanvas?a._canvas.getContext("2d"):null;a._viewport=c;a._source=b;a.config=a._viewport.config;a._imageLoader=new AjaxControlToolkit.Seadragon.ImageLoader(a.config.imageLoaderLimit);a._profiler=new AjaxControlToolkit.Seadragon.Profiler;a._minLevel=b.minLevel;a._maxLevel=b.maxLevel;a._tileSize=b.tileSize;a._tileOverlap=b.tileOverlap;a._normHeight=b.dimensions.y/b.dimensions.x;a._cacheNumTiles={};a._cachePixelRatios={};a._tilesMatrix={};a._tilesLoaded=[];a._coverage={};a._overlays=[];a._lastDrawn=[];a._lastResetTime=0;a._midUpdate=false;a._updateAgain=true;a.elmt=a._container;a._init()};AjaxControlToolkit.Seadragon.Drawer.prototype={dispose:function(){},_init:function(){var a=this;a._canvas.style.width="100%";a._canvas.style.height="100%";a._canvas.style.position="absolute";a._container.style.textAlign="left";a._container.appendChild(a._canvas)},_compareTiles:function(b,a){if(!b)return a;if(a.visibility>b.visibility)return a;else if(a.visibility==b.visibility)if(a.distance<b.distance)return a;return b},_getNumTiles:function(b){var a=this;if(!a._cacheNumTiles[b])a._cacheNumTiles[b]=a._source.getNumTiles(b);return a._cacheNumTiles[b]},_getPixelRatio:function(b){var a=this;if(!a._cachePixelRatios[b])a._cachePixelRatios[b]=a._source.getPixelRatio(b);return a._cachePixelRatios[b]},_getTile:function(b,c,d,l,e,f){var a=this;if(!a._tilesMatrix[b])a._tilesMatrix[b]={};if(!a._tilesMatrix[b][c])a._tilesMatrix[b][c]={};if(!a._tilesMatrix[b][c][d]){var g=(e+c%e)%e,h=(f+d%f)%f,i=a._source.getTileBounds(b,g,h),k=a._source.tileExists(b,g,h),m=a._source.getTileUrl(b,g,h);i.x+=1*(c-g)/e;i.y+=a._normHeight*(d-h)/f;a._tilesMatrix[b][c][d]=new AjaxControlToolkit.Seadragon.Tile(b,c,d,i,k,m)}var j=a._tilesMatrix[b][c][d];j.lastTouchTime=l;return j},_loadTile:function(a,b){a.loading=this._imageLoader.loadImage(a.url,Seadragon.Utils.createCallback(null,Function.createDelegate(this,this._onTileLoad),a,b))},_onTileLoad:function(b,m,j){var a=this;b.loading=false;if(a._midUpdate){Seadragon.Debug.error("Tile load callback in middle of drawing routine.");return}else if(!j){Seadragon.Debug.log("Tile "+b+" failed to load: "+b.url);b.exists=false;return}else if(m<a._lastResetTime){Seadragon.Debug.log("Ignoring tile "+b+" loaded before reset: "+b.url);return}b.loaded=true;b.image=j;var g=a._tilesLoaded.length;if(a._tilesLoaded.length>=QUOTA){for(var n=Math.ceil(Math.log(a._tileSize)/Math.log(2)),c=null,f=-1,e=a._tilesLoaded.length-1;e>=0;e--){var d=a._tilesLoaded[e];if(d.level<=a._cutoff||d.beingDrawn)continue;else if(!c){c=d;f=e;continue}var i=d.lastTouchTime,h=c.lastTouchTime,l=d.level,k=c.level;if(i<h||i==h&&l>k){c=d;f=e}}if(c&&f>=0){c.unload();g=f}}a._tilesLoaded[g]=b;a._updateAgain=true},_clearTiles:function(){this._tilesMatrix={};this._tilesLoaded=[]},_providesCoverage:function(b,c,f){var a=this;if(!a._coverage[b])return false;if(c===undefined||f===undefined){var e=a._coverage[b];for(var g in e)if(e.hasOwnProperty(g)){var d=e[g];for(var h in d)if(d.hasOwnProperty(h)&&!d[h])return false}return true}return a._coverage[b][c]===undefined||a._coverage[b][c][f]===undefined||a._coverage[b][c][f]===true},_isCovered:function(b,c,d){var a=this;if(c===undefined||d===undefined)return a._providesCoverage(b+1);else return a._providesCoverage(b+1,2*c,2*d)&&a._providesCoverage(b+1,2*c,2*d+1)&&a._providesCoverage(b+1,2*c+1,2*d)&&a._providesCoverage(b+1,2*c+1,2*d+1)},_setCoverage:function(a,c,e,d){var b=this;if(!b._coverage[a]){Seadragon.Debug.error("Setting coverage for a tile before its level's coverage has been reset: "+a);return}if(!b._coverage[a][c])b._coverage[a][c]={};b._coverage[a][c][e]=d},_resetCoverage:function(a){this._coverage[a]={}},_compareTiles:function(b,a){if(!b)return a;if(a.visibility>b.visibility)return a;else if(a.visibility==b.visibility)if(a.distance<b.distance)return a;return b},_getOverlayIndex:function(b){for(var a=this._overlays.length-1;a>=0;a--)if(this._overlays[a].elmt==b)return a;return -1},_updateActual:function(){var c=true,e=false,a=this;a._updateAgain=e;var r=a._canvas,R=a._context,gb=a._container,K=useCanvas,m=a._lastDrawn;while(m.length>0){var b=m.pop();b.beingDrawn=e}var H=a._viewport.getContainerSize(),F=H.x,D=H.y;r.innerHTML="";if(K){r.width=F;r.height=D;R.clearRect(0,0,F,D)}var C=a._viewport.getBounds(c),h=C.getTopLeft(),g=C.getBottomRight();if(!a.config.wrapHorizontal&&(g.x<0||h.x>1))return;else if(!a.config.wrapVertical&&(g.y<0||h.y>a._normHeight))return;var kb=Math.abs,mb=Math.ceil,U=Math.floor,t=Math.log,y=Math.max,f=Math.min,bb=a.config.alwaysBlend,B=1e3*a.config.blendTime,W=a.config.immediateRender,u=a.config.minZoomDimension,lb=a.config.minImageRatio,E=a.config.wrapHorizontal,I=a.config.wrapVertical;if(!E){h.x=y(h.x,0);g.x=f(g.x,1)}if(!I){h.y=y(h.y,0);g.y=f(g.y,a._normHeight)}var s=null,p=e,n=(new Date).getTime(),Y=a._viewport.pixelFromPoint(a._viewport.getCenter()),fb=a._viewport.deltaPixelsFromPoints(a._source.getPixelRatio(0),e).x,z=W?1:fb;u=u||64;var o=y(a._minLevel,U(t(u)/t(2))),eb=a._viewport.deltaPixelsFromPoints(a._source.getPixelRatio(0),c).x,G=f(a._maxLevel,U(t(eb/MIN_PIXEL_RATIO)/t(2)));o=f(o,G);for(var d=G;d>=o;d--){var M=e,A=a._viewport.deltaPixelsFromPoints(a._source.getPixelRatio(d),c).x;if(!p&&A>=MIN_PIXEL_RATIO||d==o){M=c;p=c}else if(!p)continue;a._resetCoverage(d);var Z=f(1,(A-.5)/.5),V=a._viewport.deltaPixelsFromPoints(a._source.getPixelRatio(d),e).x,X=z/kb(z-V),T=a._source.getTileAtPoint(d,h),k=a._source.getTileAtPoint(d,g),Q=a._getNumTiles(d),N=Q.x,O=Q.y;if(!E)k.x=f(k.x,N-1);if(!I)k.y=f(k.y,O-1);for(var i=T.x;i<=k.x;i++)for(var j=T.y;j<=k.y;j++){var b=a._getTile(d,i,j,n,N,O),v=M;a._setCoverage(d,i,j,e);if(!b.exists)continue;if(p&&!v)if(a._isCovered(d,i,j))a._setCoverage(d,i,j,c);else v=c;if(!v)continue;var P=b.bounds.getTopLeft(),J=b.bounds.getSize(),hb=a._viewport.pixelFromPoint(P,c),x=a._viewport.deltaPixelsFromPoints(J,c);if(!a._tileOverlap)x=x.plus(new AjaxControlToolkit.Seadragon.Point(1,1));var ib=a._viewport.pixelFromPoint(P,e),jb=a._viewport.deltaPixelsFromPoints(J,e),db=ib.plus(jb.divide(2)),ab=Y.distanceTo(db);b.position=hb;b.size=x;b.distance=ab;b.visibility=X;if(b.loaded){if(!b.blendStart)b.blendStart=n;var L=n-b.blendStart,w=f(1,L/B);if(bb)w*=Z;b.opacity=w;m.push(b);if(w==1)a._setCoverage(d,i,j,c);else if(L<B)updateAgain=c}else if(!b.Loading)s=a._compareTiles(s,b)}if(a._providesCoverage(d))break}for(var l=m.length-1;l>=0;l--){var b=m[l];if(K)b.drawCanvas(R);else b.drawHTML(r);b.beingDrawn=c}for(var cb=a._overlays.length,l=0;l<cb;l++){var q=a._overlays[l],S=q.bounds;q.position=a._viewport.pixelFromPoint(S.getTopLeft(),c);q.size=a._viewport.deltaPixelsFromPoints(S.getSize(),c);q.drawHTML(gb)}if(s){a._loadTile(s,n);a._updateAgain=c}},addOverlay:function(a,c,b){var a=Seadragon.Utils.getElement(a);if(this._getOverlayIndex(a)>=0)return;this._overlays.push(new AjaxControlToolkit.Seadragon.Overlay(a,c,b));this._updateAgain=true},updateOverlay:function(a,d,c){var a=Seadragon.Utils.getElement(a),b=this._getOverlayIndex(a);if(b>=0){this._overlays[b].update(d,c);this._updateAgain=true}},removeOverlay:function(c){var a=this,c=Seadragon.Utils.getElement(c),b=a._getOverlayIndex(c);if(b>=0){a._overlays[b].destroy();a._overlays.splice(b,1);a._updateAgain=true}},clearOverlays:function(){while(this._overlays.length>0){this._overlays.pop().destroy();this._updateAgain=true}},needsUpdate:function(){return this._updateAgain},numTilesLoaded:function(){return this._tilesLoaded.length},reset:function(){this._clearTiles();this._lastResetTime=(new Date).getTime();this._updateAgain=true},update:function(){var a=this;a._profiler.beginUpdate();a._midUpdate=true;a._updateActual();a._midUpdate=false;a._profiler.endUpdate()},idle:function(){}};AjaxControlToolkit.Seadragon.Drawer.registerClass("AjaxControlToolkit.Seadragon.Drawer",null,Sys.IDisposable);