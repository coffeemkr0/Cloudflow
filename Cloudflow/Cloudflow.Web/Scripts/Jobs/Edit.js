$(function () {
    ExtensionBrowser.AddExtensionClicked = AddExtension;

    $(document).on("click", ".deleteTrigger", deleteTrigger_Clicked);
    $(document).on("click", ".deleteStep", deleteStep_Clicked);

    $(".sortable").sortable({
        stop: function (event, ui) {
            SetSortablePositions(ui.item);
        }
    });
    $(".sortable").disableSelection();
});

function AddExtension(extensionId, extensionType) {
    switch (extensionType) {
        case "1":
            AddTrigger(extensionId);
            break;
        case "2":
            AddStep(extensionId);
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
                $("#triggerConfigurations").append(result.triggerConfigurationView);
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
    var triggerId = $(e.target).attr("data-triggerid");
    alert("Delete trigger clicked for trigger id " + triggerId);
}

function deleteStep_Clicked(e) {
    var stepId = $(e.target).attr("data-stepid");
    alert("Delete step clicked for step id " + stepId);
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