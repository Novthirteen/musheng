Type.registerNamespace('AjaxControlToolkit.Seadragon');

AjaxControlToolkit.Seadragon.DisplayRect = function(x, y, width, height, minLevel, maxLevel) {
AjaxControlToolkit.Seadragon.DisplayRect.initializeBase(this, [x, y, width, height]);

    this.minLevel = minLevel;
    this.maxLevel = maxLevel;
}
AjaxControlToolkit.Seadragon.DisplayRect.registerClass('AjaxControlToolkit.Seadragon.DisplayRect', AjaxControlToolkit.Seadragon.Rect);
