$(function () {
    $(document).on("click", ".stringCollectionEdit__addButton", function () {
        OnAddButtonClicked($(this).closest(".stringCollectionEdit"));
    });
});