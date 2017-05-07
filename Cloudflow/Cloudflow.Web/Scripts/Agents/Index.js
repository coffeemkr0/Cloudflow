
$(function () {
    AgentControllerClient.AgentConnected = AgentConnected;
    AgentControllerClient.ConnectToAgents();
});

function AgentConnected(machineName) {
    AgentControllerClient.GetQueuedRuns(machineName, function (runs) {
        runs.forEach(function (run) {
            AddQueuedRun(run);
        });
    });

    AgentControllerClient.GetCompletedRuns(machineName, 0, 100, function (runs) {
        runs.forEach(function (run) {
            AddCompletedRun(run);
        });
    });
}

function GetStatusText(run) {
    switch (run.Status) {
        case 0:
            return "Queued";
            break;
        case 1:
            return "Running";
            break;
        case 2:
            return "Completed";
            break;
        case 3:
            return "CompletedWithWarnings";
            break;
        case 4:
            return "Canceled";
            break;
        case 4:
            return "Failed";
            break;
    }

    return "";
}

function AddQueuedRun(run) {
    $("#queuedGrid > tbody:last-child").append("<tr><td>" + GetStatusText(run) + "</td><td>" +
        run.Name + "</td><td>" + run.JobName + "</td><td>" +
        run.DateStarted + "</td></tr>");
}

function AddCompletedRun(run) {
    $("#completedGrid > tbody:last-child").append("<tr><td>" + GetStatusText(run) + "</td><td>" +
        run.Name + "</td><td>" + run.JobName + "</td><td>" +
        run.DateStarted + "</td><td>" + run.DateEnded + "</td></tr>");
}