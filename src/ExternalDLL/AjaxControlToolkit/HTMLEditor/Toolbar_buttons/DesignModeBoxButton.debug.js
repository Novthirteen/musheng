Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton.initializeBase(this, [element]);

    this._designPanel = null;
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton.prototype = {
    _onmousedown : function(e) {
        if(this._designPanel == null) return false;
        if(this._designPanel.isPopup()) return false;
        if(AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton.callBaseMethod(this, "_onmousedown",[e])===null) return false;
        this.callMethod();
        return false;
    },
    
    onEditPanelActivity : function() {
        this._designPanel = this._editPanel.get_activePanel();
    },
    
    callMethod : function() {
        if(this._designPanel == null) return false;
        if(this._designPanel.isPopup()) return false;
        return true;
    }
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeBoxButton", AjaxControlToolkit.HTMLEditor.ToolbarButton.BoxButton);

