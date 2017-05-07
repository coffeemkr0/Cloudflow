
$(function () {
    AgentControllerClient.AgentConnected = AgentConnected;
    AgentControllerClient.ConnectToAgents();
});

function AgentConnected(machineName) {
    AgentControllerClient.GetCompletedRuns(machineName, function (runs) {
        runs.forEach(function (run) {
            AddCompletedRun(run);
        });
    });
}

function AddCompletedRun(run) {
    $("#completedGrid > tbody:last-child").append("<tr><td>" + run.Name + "</td><td>" +
        run.JobName + "</td><td>" + run.DateStarted + "</td><td>" + 
        run.DateCompleted + "</td></tr>");
}