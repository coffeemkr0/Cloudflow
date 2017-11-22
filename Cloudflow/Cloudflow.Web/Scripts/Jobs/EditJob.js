$(function () {
    $(document).on("click", ".objectCollectionNavigationItem__deleteButton", ObjectCollectionNavigationItem__deleteButton_Clicked);

    $(document).on("click", ".categorizedItemSelector__addButton", function () {
        var $categorizedItemSelectorElement = $(this).closest(".categorizedItemSelector");
        var collectionId = $categorizedItemSelectorElement.attr("data-collectionid");
        var metaData = $categorizedItemSelectorElement.find("[name='metaData']").val();
        var $objectCollectionEditElement = $("#" + collectionId);

        AddTrigger($objectCollectionEditElement, metaData);
    });

    $(".objectCollectionEdit__navigationItems").sortable({
        stop: function (event, ui) {
            UpdateObjectCollectionNames(ui.item.closest(".objectCollectionEdit__navigationItems"));
        }
    });
    $(".objectCollectionEdit__navigationItems").disableSelection();
});

function OnAddExtensionClicked(addButtonElement) {
    var viewModelPropertyName = addButtonElement.attr("data-viewmodelpropertyname");
    var itemId = addButtonElement.attr("data-itemid");
    var extensionBrowserId = addButtonElement.attr("data-target");

    $(extensionBrowserId).attr("data-itemid", itemId);
    $(extensionBrowserId).attr("data-viewmodelpropertyname", viewModelPropertyName);
}

function AddExtension(extensionId, extensionType, itemId, viewModelPropertyName) {
    switch (extensionType) {
        case "1":
            AddTrigger(extensionId);
            break;
        case "2":
            AddStep(extensionId);
            break;
        case "3":
            AddCondition(extensionId, itemId, viewModelPropertyName);
            break;
    }
}

function AddStep(extensionId) {
    var index = $(".stepNavigationItem").length;

    $.ajax({
        type: 'POST',
        url: "/Jobs/AddStep",
        dataType: 'json',
        data: {
            stepId: extensionId,
            index: index
        },
        success: function (result) {
            if (result !== null) {
                $("#stepNavigationItems").append(result.stepNavigationItemView);
                $("#stepConfigurations").append(result.stepConfigurationView);


            } else {
                alert('Error getting data.');
            }
        },
        error: function () {
            alert('Error getting data.');
        }
    });
}

function AddTrigger(objectCollectionEditElement, metaData) {
    $.ajax({
        type: 'POST',
        url: "/Jobs/AddTrigger",
        dataType: 'json',
        data: {
            propertyName: objectCollectionEditElement.attr("data-propertyname"),
            metaData: metaData
        },
        success: function (result) {
            if (result !== null) {
                objectCollectionEditElement.find(".objectCollectionEdit__navigationItems").first().append(result.navigationItemView);
                objectCollectionEditElement.find(".objectCollectionEdit__items").first().append(result.itemView);
                UpdateObjectCollectionNames(objectCollectionEditElement.find(".objectCollectionEdit__navigationItems").first());
            } else {
                alert('Error getting data.');
            }
        },
        error: function () {
            alert('Error getting data.');
        }
    });
}

function AddCondition(extensionId, itemId, viewModelPropertyName) {

    var $conditionNavigationItemsElement = $("#conditionNavigationItems" + itemId);
    var $conditionConfigurationsElement = $("#conditionConfigurations" + itemId);
    var index = $conditionNavigationItemsElement.find(".conditionNavigationItem").length;

    $.ajax({
        type: 'POST',
        url: "/Jobs/AddCondition",
        dataType: 'json',
        data: {
            conditionId: extensionId,
            index: index,
            viewModelPropertyName: viewModelPropertyName
        },
        success: function (result) {
            if (result !== null) {
                $conditionNavigationItemsElement.append(result.conditionNavigationItemView);
                $conditionConfigurationsElement.append(result.conditionConfigurationView);
            } else {
                alert('Error getting data.');
            }
        },
        error: function () {
            alert('Error getting data.');
        }
    });
}

function ObjectCollectionNavigationItem__deleteButton_Clicked(e) {
    var $navigationItemElement = $(e.target).closest(".objectCollectionNavigationItem");
    var $objectEditElement = $("#" + $navigationItemElement.attr("data-itemid"));

    $navigationItemElement.remove();
    $objectEditElement.remove();
}

function UpdateObjectCollectionNames(navigationItemsElement) {
    var index = 0;
    var propertyName = navigationItemsElement.closest(".objectCollectionEdit").attr("data-propertyname");

    //Iterate the navigation items in the object collection edit
    navigationItemsElement.children(".objectCollectionNavigationItem").each(function () {
        //Select the navigation item's object edit
        var $objectEditElement = $("#" + $(this).attr("data-itemid"));

        //Reindex name attributes
        var propertyNamePattern = propertyName.replace(".", "\\.").replace("[", "\\[").replace("]", "\\]");
        var nameRegEx = new RegExp("^" + propertyNamePattern + "\\[\\d+\\]");
        $objectEditElement.find("[name]").filter(function () {
            return nameRegEx.test($(this).attr("name"));
        }).each(function () {
            $(this).attr("name", $(this).attr("name").replace(nameRegEx, propertyName + "[" + index + "]"));
        });

        //Reindex label for attributes
        $objectEditElement.find("[for]").filter(function () {
            return nameRegEx.test($(this).attr("for"));
        }).each(function () {
            $(this).attr("for", $(this).attr("for").replace(nameRegEx, propertyName + "[" + index + "]"));
        });

        //Reindex any data-propertyname attributes
        $objectEditElement.find("[data-propertyname]").filter(function () {
            return nameRegEx.test($(this).attr("data-propertyname"));
        }).each(function () {
            $(this).attr("data-propertyname", $(this).attr("data-propertyname").replace(nameRegEx, propertyName + "[" + index + "]"));
        });

        //Reindex id attributes
        var idRegEx = new RegExp("^" + propertyName.replace(".", "_") + "\\[\\d+\\]");
        $objectEditElement.find("[id]").filter(function () {
            return idRegEx.test($(this).attr("id"));
        }).each(function () {
            $(this).attr("id", $(this).attr("id").replace(idRegEx, propertyName.replace(".", "_") + "[" + index + "]"));
        });

        index += 1;
    });
}