$(function () {
    //Assign event handlers that we care about
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
});

function OnAddButtonClicked(stringCollectionEditElement) {
    console.log("Add button clicked");
    console.log(stringCollectionEditElement);
}

function OnRemoveButtonClicked(stringCollectionEditElement) {
    //Remove each tr element that has a checked selector input
    GetSelectedItems(stringCollectionEditElement).remove();
    ReIndexItems(stringCollectionEditElement);
}

function OnUpButtonClicked(stringCollectionEditElement) {
    var $selectedItems = GetSelectedItems(stringCollectionEditElement);
    //Don't do anything if the first item is selected - it cannot be moved up any farther
    if ($selectedItems.length > 0) {
        $selectedItems.each(function () {
            var inputName = $(this).find(".stringCollectionEdit__itemSelectorInput").attr("name");
            console.log("Input Name = " + inputName + " Index = " + GetItemIndex(inputName));
        });
    }
}

function OnDownButtonClicked(stringCollectionEditElement) {
    console.log("Down button clicked");
    console.log(stringCollectionEditElement);
}

function GetSelectedItems(stringCollectionEditElement) {
    return stringCollectionEditElement.find("tr").has(".stringCollectionEdit__itemSelectorInput:checked");
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

function GetItemIndex(name){
    var firstBracketPosition = name.lastIndexOf("[");
    var lastBracketPosition = name.lastIndexOf("]");

    return name.substr(firstBracketPosition + 1, lastBracketPosition - firstBracketPosition - 1);
}