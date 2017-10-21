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
    GetSelectedItems(stringCollectionEditElement).remove();
    ReIndexItems(stringCollectionEditElement);
}

function OnUpButtonClicked(stringCollectionEditElement) {
    GetSelectedItems(stringCollectionEditElement).each(function () {
        var $item = $(this);

        if ($item.is(':first-child')) {
            return false;
        }

        $item.prev().before($item);
    });

    ReIndexItems(stringCollectionEditElement);
}

function OnDownButtonClicked(stringCollectionEditElement) {
    $(GetSelectedItems(stringCollectionEditElement).get().reverse()).each(function () {
        var $item = $(this);
        console.log($item);
        if ($item.is(':last-child')) {
            console.log("Is last item");
            return false;
        }

        $item.next().after($item);
    });

    ReIndexItems(stringCollectionEditElement);
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

        $tableRow.find(".stringCollectionEdit__itemInput").attr("name", propertyName + "[" + index + "]");
        index++;
    });
}


////move element to top
//$el.parent().prepend($el);

////move element to end
//$el.parent().append($el);