/**
* YoeJoy Object.js file
*/

/** ************************************************************************************************** */


// Object Definition
YoeJoy.Object = new function () {

    var _this = this;

    _this.FullScreenOverlay = function (overlayId, maskId, maskColor, overlayZIndex, maskZIndex) {
        var _overlay = this;
        _overlay.OverlayId = "#" + overlayId;
        _overlay.MaskId = "#" + maskId;
        _overlay.MaskColor = maskColor;
        _overlay.OverlayZIndex = overlayZIndex;
        _overlay.MaskZIndex = maskZIndex;
    };

};

//Object Feature Definition
YoeJoy.FullScreenOverlay = new function () {

    var _this = this;

    _this.initOverlayCss = function (overlay) {
        $(overlay.OverlayId).css({
            position: "absolute",
            "z-index": overlay.OverlayZIndex,
            "display": "none"
        });
        $(overlay.MaskId).css({
            position: "fixed",
            top: "0px",
            left: "0px"
        });
    };

    _this.hideOverlay = function (overlay) {
        $(overlay.MaskId).css({
            "width": "0px",
            "height": "0px",
            "z-index": 0
        });
        $(overlay.OverlayId).css("display", "none");
        $(window).unbind();
    };

    _this.showOverlay = function (overlay) {
        var overlayWidth = $(overlay.OverlayId).width();
        var overlayHeight = $(overlay.OverlayId).height();
        var windowWidth = $(window).width();
        var windowHeight = $(window).height();

        $(window).resize(function () {
            var windowWidth = $(window).width();
            var windowHeight = $(window).height();
            $(overlay.MaskId).css({
                "width": windowWidth,
                "height": windowHeight
            });
        });

        $(overlay.MaskId).css({
            "width": windowWidth,
            "height": windowHeight,
            "background-color": overlay.MaskColor,
            "z-index": overlay.MaskZIndex
        }).click(function () {
            YoeJoy.FullScreenOverlay.hideOverlay(overlay);
            return;
        });

        $(overlay.OverlayId).css({
            "display": "block"
        });
    };

    _this.initOverlay = function (overlay) {
        _this.initOverlayCss(overlay);
    };

};