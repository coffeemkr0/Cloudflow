ExtensionBrowser = function () {
    this.AddExtensionClicked = null;
}

$(function () {
    $("#modalExtensionBrowser").on("show.bs.modal", function () {
        $(".extensionBrowser__extensionList").css('max-height', $(window).height() * 0.6);
    });

    $(".extensionBrowser__addButton").on("click", function (e) {
        var extensionId = $(e.target).attr("data-extensionid");
        ExtensionBrowser.AddExtensionClicked(extensionId);
    });
});