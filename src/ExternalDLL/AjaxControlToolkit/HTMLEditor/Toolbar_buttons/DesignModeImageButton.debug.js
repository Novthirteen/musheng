Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton = function(element) {
    AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton.initializeBase(this, [element]);
    this._designPanel = null;
}

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton.prototype = {
    _onmousedown : function(e) {
        if(this._designPanel == null) return false;
        if(this._designPanel.isPopup()) return false;
        if(AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton.callBaseMethod(this, "_onmousedown",[e])===null) return false;
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

AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignModeImageButton", AjaxControlToolkit.HTMLEditor.ToolbarButton.ImageButton);

