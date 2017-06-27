ExtensionBrowser = function () {
    this.AddExtensionClicked = null;
};

$(function () {
    $(".extensionBrowser").on("show.bs.modal", function () {
        $(".extensionBrowser__extensionList").css('max-height', $(window).height() * 0.6);
    });

    $(".extensionBrowser__addButton").on("click", function (e) {
        var extensionId = $(e.target).attr("data-extensionid");
        var extensionType = $(e.target).attr("data-extensionType");
        ExtensionBrowser.AddExtensionClicked(extensionId, extensionType);
    });
});