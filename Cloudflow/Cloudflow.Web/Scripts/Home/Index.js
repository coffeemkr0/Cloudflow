
function HomeIndex() {
    this.Agents = null;
}

$(function () {
    AgentControllerClient.AgentConnected = AgentConnected;
    AgentControllerClient.ConnectToAgents(HomeIndex.Agents);
    AgentControllerClient.AgentStatusUpdated = AgentStatusUpdated;

    $(".agentControlLink").each(function () {
        $(this).on("click", function (e) {
            AgentControlClicked(e);
        });
    });
});

function AgentConnected(machineName) {
    UpdateAgentStatus(machineName);
}

function AgentStatusUpdated(machineName, status) {
    SetAgentStatusText(machineName, status);
    SetAgentControlText(machineName, status);
}

function AgentControlClicked(e) {
    var $item = $(e.target);
    var machineName = $item.attr("data-machinename");
    if ($item.text() == "Start") {
        AgentControllerClient.StartAgent(machineName, function () {
            SetAgentStatusText(machineName, status);
            SetAgentControlText(machineName, status);
        });
    }
    else {
        AgentControllerClient.StopAgent(machineName, function () {
            SetAgentStatusText(machineName, status);
            SetAgentControlText(machineName, status);
        });
    }
    
    return false;
}

function StartAgent(machineName) {
    AgentControllerClient.StartAgent(machineName, function () {
        SetAgentStatusText(machineName, status);
        SetAgentControlText(machineName, status);
    });
}

function SetAgentStatusText(machineName, status) {
    var $element = $("#agentStatus-" + machineName);

    if (status !== null) {
        switch (status.Status) {
            case 0:
                $element.text("Not Running");
                break;
            case 1:
                $element.text("Starting");
                break;
            case 2:
                $element.text("Idle");
                break;
            case 3:
                $element.text("Processing " + status.Runs);
                break;
            case 4:
                $element.text("Stopping");
                break;
        }
    }
    else {
        $element.text("Unreachable");
    }
}

function SetAgentControlText(machineName, status) {
    var $element = $("#agentControl-" + machineName);
    var currentText = $element.text();

    if (status !== null) {
        switch (status.Status) {
            case 0:
                if (currentText !== "Start") {
                    $element.text("Start");
                }
                break;
            case 2:
            case 3:
                if (currentText !== "Stop") {
                    $element.text("Stop");
                }
                break;
            default:
                if (currentText !== "Stop") {
                    $element.text("");
                }
                break;
        }
    }
    else {
        if (currentText !== "Stop") {
            $element.text("");
        }
    }
}

function UpdateAgentStatus(machineName) {
    AgentControllerClient.GetAgentStatus(machineName, function (status) {
        SetAgentStatusText(machineName, status);
        SetAgentControlText(machineName, status);
    });
}