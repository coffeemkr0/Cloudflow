$(function () {
    $(".categorizedItemSelector").on("show.bs.modal", function () {
        $(".categorizedItemSelector__extensionList").css('max-height', $(window).height() * 0.6);
    });
});