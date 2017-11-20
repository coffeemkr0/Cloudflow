CategorizedItemSelector = function () {
    this.AddItemClicked = null;
};

$(function () {
    $(".categorizedItemSelector").on("show.bs.modal", function () {
        $(".categorizedItemSelector__extensionList").css('max-height', $(window).height() * 0.6);
    });

    $(".categorizedItemSelector__addButton").on("click", function (e) {
        console.log("Add button clicked");
        CategorizedItemSelector.AddItemClicked();
    });
});