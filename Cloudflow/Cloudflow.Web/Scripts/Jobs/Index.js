
$(function () {
    $(function () {
        AgentControllerClient.ConnectToAgents();

        $(".jobs__publishButton").each(function () {
            $(this).on("click", function (e) {
                PublishJobClicked(e);
            });
        });
    });
});

function PublishJobClicked(e) {
    var $item = $(e.target);
    var jobid = $item.attr("data-jobid");
    var getJobDefinitionUrl = $("#getJobDefinitionUrl").val();

    $.getJSON({
        url: getJobDefinitionUrl,
        data: { id: jobid },
        success: function (jobDefinition) {
            AgentControllerClient.PublishJob(jobDefinition, function () {
                console.log("Job published");
            });
        }
    });

    return false;
}