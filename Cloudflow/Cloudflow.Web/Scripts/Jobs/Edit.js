$(function () {
    $("#btnAddTrigger").on("click", btnAddTrigger_Clicked);
    $("#btnAddStep").on("click", btnAddStep_Clicked);
});

function btnAddTrigger_Clicked() {
    alert("Add trigger clicked");
}

function btnAddStep_Clicked() {
    alert("Add step clicked");
}