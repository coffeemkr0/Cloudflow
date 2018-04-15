
$(function () {
    //Sign up for events from the agent controller client
    AgentHubClient.AgentConnected = AgentConnected;
    AgentHubClient.RunStatusChanged = RunStatusChanged;

    //Connect to the agents
    AgentHubClient.ConnectToAgents();

    $("#runGrids").on("click", ".runGrid__runEntry", function () {
        RunGridRowClicked($(this));
    });
});

function RunGridRowClicked(e){
    e.addClass('active').siblings().removeClass('active');
}

function AgentConnected(machineName) {
    AgentHubClient.GetQueuedRuns(machineName, function (runs) {
        runs.forEach(function (run) {
            AddOrUpdateRunInProgress(run);
        });
    });

    AgentHubClient.GetCompletedRuns(machineName, 0, 100, function (runs) {
        runs.forEach(function (run) {
            AppendCompletedRun(run);
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
        $('#queuedGrid > tbody:last-child').append('<tr class="runGrid__runEntry" data-id="' + run.Id + '"><td class="runGrid__statusColumn">' + GetStatusText(run) +
            '</td><td>' + run.Name + '</td><td>' + run.JobName + '</td><td>' +
            run.DateStarted + '</td></tr>');
    }
}

function RemoveRunInProgress(run) {
    var $element = $('#queuedGrid').find('tr[data-id="' + run.Id + '"]');
    $element.remove();
}

function AppendCompletedRun(run) {
    $('#completedGrid > tbody:last-child').append('<tr class="runGrid__runEntry" data-id="' + run.Id + '"><td class="runGrid__statusColumn">' + GetStatusText(run) +
        '</td><td>' + run.Name + '</td><td>' + run.JobName + '</td><td>' +
        run.DateStarted + '</td><td>' + run.DateEnded + '</td></tr>');
}

function InsertCompletedRun(run, maxPageCount) {
    var $runEntries = $('#completedGrid').find(".runGrid__runEntry");

    if ($runEntries.length === 0) {
        AppendCompletedRun(run);
    }
    else {
        $('<tr class="runGrid__runEntry" data-id="' + run.Id + '"><td class="runGrid__statusColumn">' + GetStatusText(run) +
            '</td><td>' + run.Name + '</td><td>' + run.JobName + '</td><td>' +
            run.DateStarted + '</td><td>' + run.DateEnded + '</td></tr>').insertBefore($runEntries.first());

        if ($runEntries.length >= maxPageCount) {
            $runEntries.last().remove();
        }
    }
}

function RunStatusChanged(machineName, run){
    switch (run.Status) {
        case 0:
        case 1:
            AddOrUpdateRunInProgress(run);
            break;
        default:
            RemoveRunInProgress(run);
            InsertCompletedRun(run, 100);
            break;
    }
}