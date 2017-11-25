$(function () {
    $(document).on("click", ".objectCollectionNavigationItem__deleteButton", ObjectCollectionNavigationItem__deleteButton_Clicked);

    $(document).on("click", ".objectCollectionEdit__addButton", function () {
        var addItemModalId = $(this).attr("data-target");
        console.log("Target=" + addItemModalId);
        $(addItemModalId).attr("data-collectionid", $(this).closest(".objectCollectionEdit").attr("id"));
    });

    $(document).on("click", ".categorizedItemSelector__addButton", function () {
        var $clickedItemElement = $(this).closest(".categorizedItemSelector__item");
        var $categorizedItemSelectorElement = $clickedItemElement.closest(".categorizedItemSelector");
        var collectionId = $categorizedItemSelectorElement.attr("data-collectionid");
        var $objectCollectionEditElement = $("#" + collectionId);

        var objectFactoryAssemblyPath = $clickedItemElement.find("input[name='objectFactoryAssemblyPath']").val();
        var objectFactoryExtensionId = $clickedItemElement.find("input[name='objectFactoryExtensionId']").val();
        var factoryData = $clickedItemElement.find("input[name='factoryData']").val();
        var instanceData = $clickedItemElement.find("input[name='instanceData']").val();

        AddObjectCollectionItem($objectCollectionEditElement, objectFactoryAssemblyPath,
            objectFactoryExtensionId, factoryData, instanceData);
    });

    $(".objectCollectionEdit__navigationItems").sortable({
        stop: function (event, ui) {
            UpdateObjectCollectionNames(ui.item.closest(".objectCollectionEdit__navigationItems"));
        }
    });
    $(".objectCollectionEdit__navigationItems").disableSelection();
});

function AddObjectCollectionItem(objectCollectionEditElement, objectFactoryAssemblyPath,
    objectFactoryExtensionId, factoryData, instanceData) {
    $.ajax({
        type: 'POST',
        url: "/Jobs/CreateObjectCollectionItem",
        dataType: 'json',
        data: {
            propertyName: objectCollectionEditElement.attr("data-propertyname"),
            objectFactoryAssemblyPath: objectFactoryAssemblyPath,
            objectFactoryExtensionId: objectFactoryExtensionId,
            factoryData: factoryData,
            instanceData: instanceData
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