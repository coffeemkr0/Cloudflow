$(function () {
    $("#modalExtensionBrowser").on("show.bs.modal", function () {
        $(".extensionBrowser__extensionList").css('max-height', $(window).height() * 0.6);
    });
});