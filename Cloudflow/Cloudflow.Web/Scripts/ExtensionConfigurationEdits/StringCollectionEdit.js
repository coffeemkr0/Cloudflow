
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
    //Remove each tr element that has a checked selector input
    stringCollectionEditElement.find(".stringCollectionEdit__itemSelectorInput:checked").closest("tr").remove();
    ReIndexItems(stringCollectionEditElement);
}

function OnUpButtonClicked(stringCollectionEditElement) {
    console.log("Up button clicked");
    console.log(stringCollectionEditElement);
}

function OnDownButtonClicked(stringCollectionEditElement) {
    console.log("Down button clicked");
    console.log(stringCollectionEditElement);
}

function ReIndexItems(stringCollectionEditElement) {
    var index = 0;
    var propertyName = stringCollectionEditElement.attr("data-propertyname");

    //Reset the name attributes on inputs so that they have indexes in their names that are in order
    stringCollectionEditElement.find(".stringCollectionEdit__itemsTable tr").each(function () {
        var $tableRow = $(this);

        $tableRow.find(".stringCollectionEdit__itemSelectorInput").attr("name", propertyName + "[" + index + "]");
        $tableRow.find(".stringCollectionEdit__itemInput").attr("name", propertyName + "[" + index + "]");
        index++;
    });
}