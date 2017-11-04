$(function () {
    ExtensionBrowser.AddExtensionClicked = AddExtension;

    $(document).on("click", ".deleteTrigger", deleteTrigger_Clicked);
    $(document).on("click", ".deleteStep", deleteStep_Clicked);
    $(document).on("click", ".deleteCondition", deleteCondition_Clicked);

    $(document).on("click", ".addExtension", function () {
        OnAddExtensionClicked($(this));
    });

    $(".sortable").sortable({
        stop: function (event, ui) {
            SetSortablePositions(ui.item);
        }
    });
    $(".sortable").disableSelection();
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

function AddTrigger(extensionId) {
    var index = $(".triggerNavigationItem").length;

    $.ajax({
        type: 'POST',
        url: "/Jobs/AddTrigger",
        dataType: 'json',
        data: {
            triggerId: extensionId,
            index: index
        },
        success: function (result) {
            if (result !== null) {
                $("#triggerNavigationItems").append(result.triggerNavigationItemView);
                $("#triggers").append(result.triggerView);
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

function deleteTrigger_Clicked(e) {
    var id = $(e.target).attr("data-triggerid");
    var $navigationItem = $(e.target).parents("li").addClass("hidden");

    var $configurationItem = $("#tab" + id);
    $configurationItem.addClass("hidden");

    var $deletedInput = $configurationItem.find(".deletedInput");
    $deletedInput.val("True");
}

function deleteStep_Clicked(e) {
    var id = $(e.target).attr("data-stepid");
    var $navigationItem = $(e.target).parents("li").addClass("hidden");

    var $configurationItem = $("#tab" + id);
    $configurationItem.addClass("hidden");

    var $deletedInput = $configurationItem.find(".deletedInput");
    $deletedInput.val("True");
}

function deleteCondition_Clicked(e) {
    var id = $(e.target).attr("data-conditionid");
    var $navigationItem = $(e.target).parents("li").addClass("hidden");

    var $configurationItem = $("#tab" + id);
    $configurationItem.addClass("hidden");

    var $deletedInput = $configurationItem.find(".deletedInput");
    $deletedInput.val("True");
}

function SetSortablePositions(item) {
    var position = 0;
    item.parent().find(".sortableNavigationItem").each(function (index) {
        var $sortableItem = $("#" + $(this).attr("data-sortable-itemid"));
        var $positionInput = $sortableItem.find(".sortableItemPosition");
        $positionInput.val(position);
        position += 1;
    });
}