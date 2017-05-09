
$(function () {
    //Sign up for events from the agent controller client
    AgentControllerClient.AgentConnected = AgentConnected;
    AgentControllerClient.RunStatusChanged = RunStatusChanged;

    //Connect to the agents
    AgentControllerClient.ConnectToAgents();
});

function AgentConnected(machineName) {
    AgentControllerClient.GetQueuedRuns(machineName, function (runs) {
        runs.forEach(function (run) {
            AddOrUpdateRunInProgress(run);
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

function AddOrUpdateRunInProgress(run) {
    var $element = $('#queuedGrid > tr[data-id="' + run.Id + '"]');
    if ($element.length !== 0) {
        $element.find(".runGrid__statusColumn").text(GetStatusText(run));
    }
    else{
        $('#queuedGrid > tbody:last-child').append('<tr data-id="' + run.Id + '"><td class="runGrid__statusColumn">' + GetStatusText(run) +
            '</td><td>' + run.Name + '</td><td>' + run.JobName + '</td><td>' +
            run.DateStarted + '</td></tr>');
    }
}

function RemoveRunInProgress(run) {
    var $element = $('#queuedGrid').find('tr[data-id="' + run.Id + '"]');
    $element.remove();
}

function AddCompletedRun(run) {
    $('#completedGrid > tbody:last-child').append('<tr data-id="' + run.Id + '"><td class="runGrid__statusColumn">' + GetStatusText(run) +
        '</td><td>' + run.Name + '</td><td>' + run.JobName + '</td><td>' +
        run.DateStarted + '</td><td>' + run.DateEnded + '</td></tr>');
}

function RunStatusChanged(machineName, run){
    switch (run.Status) {
        case 0:
        case 1:
            AddOrUpdateRunInProgress(run);
            break;
        default:
            RemoveRunInProgress(run);
            AddCompletedRun(run);
            break;
    }
}