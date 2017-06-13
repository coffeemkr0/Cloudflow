$(function () {
    $("#btnAddTrigger").on("click", btnAddTrigger_Clicked);
    $("#btnAddStep").on("click", btnAddStep_Clicked);
    $(".deleteTrigger").on("click", deleteTrigger_Clicked);
    $(".deleteStep").on("click", deleteStep_Clicked);
});

function btnAddTrigger_Clicked() {
    alert("Add trigger clicked");
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