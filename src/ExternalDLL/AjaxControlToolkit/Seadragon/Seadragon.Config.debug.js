Type.registerNamespace('AjaxControlToolkit.Seadragon');
Type.registerNamespace('Seadragon');
AjaxControlToolkit.Seadragon.Config = function() {

    this.debugMode = true;

    this.animationTime = 1.5;

    this.blendTime = 0.5;

    this.alwaysBlend = false;

    this.autoHideControls = true;

    this.immediateRender = false;

    this.wrapHorizontal = false;

    this.wrapVertical = false;

    this.minZoomDimension = 0.8;

    this.maxZoomPixelRatio = 2;

    this.visibilityRatio = 0.5;

    this.springStiffness = 5.0;

    this.imageLoaderLimit = 2;

    this.clickTimeThreshold = 200;

    this.clickDistThreshold = 5;

    this.zoomPerClick = 2.0;

    this.zoomPerSecond = 2.0;

    this.showNavigationControl = true;

    this.maxImageCacheCount = 100;

    this.minPixelRatio = 0.5;

    this.mouseNavEnabled = true;

    this.navImages = {
        zoomIn: {
            REST: '<%= WebResource("AjaxControlToolkit.Seadragon.images.zoomin_rest.png") %>',
            GROUP: '<%= WebResource("AjaxControlToolkit.Seadragon.images.zoomin_grouphover.png") %>',
            HOVER: '<%= WebResource("AjaxControlToolkit.Seadragon.images.zoomin_hover.png") %>',
            DOWN: '<%= WebResource("AjaxControlToolkit.Seadragon.images.zoomin_pressed.png") %>'
        },
        zoomOut: {
            REST: '<%= WebResource("AjaxControlToolkit.Seadragon.images.zoomout_rest.png") %>',
            GROUP: '<%= WebResource("AjaxControlToolkit.Seadragon.images.zoomout_grouphover.png") %>',
            HOVER: '<%= WebResource("AjaxControlToolkit.Seadragon.images.zoomout_hover.png") %>',
            DOWN: '<%= WebResource("AjaxControlToolkit.Seadragon.images.zoomout_pressed.png") %>'
        },
        home: {
            REST: '<%= WebResource("AjaxControlToolkit.Seadragon.images.home_rest.png") %>',
            GROUP: '<%= WebResource("AjaxControlToolkit.Seadragon.images.home_grouphover.png") %>',
            HOVER: '<%= WebResource("AjaxControlToolkit.Seadragon.images.home_hover.png") %>',
            DOWN: '<%= WebResource("AjaxControlToolkit.Seadragon.images.home_pressed.png") %>'
        },
        fullpage: {
            REST: '<%= WebResource("AjaxControlToolkit.Seadragon.images.fullpage_rest.png") %>',
            GROUP: '<%= WebResource("AjaxControlToolkit.Seadragon.images.fullpage_grouphover.png") %>',
            HOVER: '<%= WebResource("AjaxControlToolkit.Seadragon.images.fullpage_hover.png") %>',
            DOWN: '<%= WebResource("AjaxControlToolkit.Seadragon.images.fullpage_pressed.png") %>'
        }
    }
}
AjaxControlToolkit.Seadragon.Config.registerClass('AjaxControlToolkit.Seadragon.Config', null, Sys.IDisposable);
