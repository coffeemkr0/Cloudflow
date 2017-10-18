
$(function () {
    InitializeStringCollectionEdits();
});

function InitializeStringCollectionEdits() {
    $(document).on("click", ".stringCollectionEdit__addButton", function () {
        OnAddButtonClicked($(this).closest(".stringCollectionEdit"));
    });

    $(document).on("click", ".stringCollectionEdit__removeButton", function () {
        OnRemoveButtonClicked($(this).closest(".stringCollectionEdit"));
    });

    $(document).on("click", ".stringCollectionEdit__upButton", function () {
        OnUpButtonClicked($(this).closest(".stringCollectionEdit"));
    });

    $(document).on("click", ".stringCollectionEdit__downButton", function () {
        OnDownButtonClicked($(this).closest(".stringCollectionEdit"));
    });
}

function OnAddButtonClicked(stringCollectionEditElement) {
    console.log("Add button clicked");
    console.log(stringCollectionEditElement);
}

function OnRemoveButtonClicked(stringCollectionEditElement) {
    console.log("Remove button clicked");
    console.log(stringCollectionEditElement);
}

function OnUpButtonClicked(stringCollectionEditElement) {
    console.log("Up button clicked");
    console.log(stringCollectionEditElement);
}

function OnDownButtonClicked(stringCollectionEditElement) {
    console.log("Down button clicked");
    console.log(stringCollectionEditElement);
}