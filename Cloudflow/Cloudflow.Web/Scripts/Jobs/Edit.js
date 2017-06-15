$(function () {
    $("#btnAddTrigger").on("click", btnAddTrigger_Clicked);
    $("#btnAddStep").on("click", btnAddStep_Clicked);

    $(document).on("click", ".deleteTrigger", deleteTrigger_Clicked);
    $(document).on("click", ".deleteStep", deleteStep_Clicked);
});

function btnAddTrigger_Clicked() {
    var index = parseInt($(".triggerNavigationItem").last().attr("data-index")) + 1;

    $.ajax({
        type: 'POST',
        url: "/Jobs/AddTrigger",
        dataType: 'json',
        data: {
            triggerId: "DABF8963-4B59-448E-BE5A-143EBDF123EF",
            index: index
        },
        success: function (result) {
            if (result != null) {
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

function btnAddStep_Clicked() {
    alert("Add step clicked");
}

function deleteTrigger_Clicked(e) {
    var triggerId = $(e.target).attr("data-triggerid");
    alert("Delete trigger clicked for trigger id " + triggerId);
}

function deleteStep_Clicked(e) {
    var stepId = $(e.target).attr("data-stepid");
    alert("Delete step clicked for step id " + stepId);
}