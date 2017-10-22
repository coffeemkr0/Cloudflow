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

    $(document).on("click", ".stringCollectionEdit__itemSelector", function (e) {
        OnItemSelectorClicked($(this), e);
    });
});

function OnAddButtonClicked(stringCollectionEditElement) {
    var index = stringCollectionEditElement.find(".stringCollectionEdit__itemInput").length;
    var propertyName = stringCollectionEditElement.attr("data-propertyname");

    $.ajax({
        url: "/ExtensionConfigurationEdits/StringCollectionEditItem",
        cache: false,
        data: {
            propertyName: propertyName,
            index: index
        },
        success: function (html) { stringCollectionEditElement.find(".stringCollectionEdit__itemsTable").find("tbody:last-child").append(html); },
        error: function () {
            alert('Error getting data.');
        }
    });
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

function OnItemSelectorClicked(itemSelectorElement, e) {
    var $itemSelectorInputElement = itemSelectorElement.find(".stringCollectionEdit__itemSelectorInput");

    $itemSelectorInputElement.prop("checked", true);

    OnItemSelectorInputClicked($itemSelectorInputElement, e);
}

function OnItemSelectorInputClicked(itemSelectorInputElement, e) {
    var $clickedItemRowElement = itemSelectorInputElement.closest("tr");

    if (!e.ctrlKey && !e.shiftKey) {
        //If not using the ctrl or shift key, deselect other selections
        itemSelectorInputElement.closest("tbody").find("tr").each(function () {
            var $item = $(this);

            if (!$clickedItemRowElement.is($item)) {
                $item.find(".stringCollectionEdit__itemSelectorInput").prop("checked", false);
            }
        });
    }
    else if (e.shiftKey) {
        //If using the shift key, select items in between the last selection and this one
        var $tableElement = $clickedItemRowElement.parent();
        var $lastSelectedRowElement = itemSelectorInputElement.closest(".stringCollectionEdit").data("lastSelectedRowElement");

        if (typeof ($lastSelectedRowElement) !== "undefined") {
            if ($clickedItemRowElement.index() < $lastSelectedRowElement.index()) {
                $clickedItemRowElement.nextUntil($lastSelectedRowElement).each(function () {
                    $(this).find(".stringCollectionEdit__itemSelectorInput").prop("checked", true);
                });
            }
            else {
                console.log("Selecting up");
                $clickedItemRowElement.prevUntil($lastSelectedRowElement).each(function () {
                    $(this).find(".stringCollectionEdit__itemSelectorInput").prop("checked", true);
                });
            }
        }
    }

    //If using the Ctrl key, the default behavior covers it - add to the existing selection

    itemSelectorInputElement.closest(".stringCollectionEdit").data("lastSelectedRowElement", $clickedItemRowElement);
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