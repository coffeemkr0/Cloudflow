
function HomeIndex() {
    this.Agents = null;
}

$(function () {
    AgentControllerClient.AgentConnected = AgentConnected;
    AgentControllerClient.AgentMessageReceived = AgentMessageReceived;

    AgentControllerClient.ConnectToAgents(HomeIndex.Agents);

    $(".agentControlLink").each(function () {
        $(this).on("click", function (e) {
            AgentControlClicked(e);
        });
    });
});

function AgentConnected(machineName) {
    SetStatusText(machineName);
}

function AgentControlClicked(e) {
    var $item = $(e.target);
    var machineName = $item.attr("data-machinename");
    if ($item.text() == "Start") {
        AgentControllerClient.StartAgent(machineName, function () {
            SetStatusText(machineName);
            $item.text("Stop");
        });
    }
    else {
        AgentControllerClient.StopAgent(machineName, function () {
            SetStatusText(machineName);
            $item.text("Start");
        });
    }
    
    return false;
}

function AgentMessageReceived(machinName, message) {
    console.log("Message received from " + machineName + " - " + message);
}

function StartAgent(machineName) {
    AgentControllerClient.StartAgent(machineName, function () {
        SetStatusText(machineName);
    });
}

function SetStatusText(machineName) {
    AgentControllerClient.GetAgentStatus(machineName, function (status) {
        if (status !== null) {
            $("#agentStatus-" + machineName).text(status.StatusDisplayText);
            switch (status.Status) {
                case 0:
                    $("#agentControl-" + machineName).text("Start");
                    break;
                default:
                    $("#agentControl-" + machineName).text("Stop");
                    break;
            }
        }
        else {
            $("#agentStatus-" + machineName).text("Unreachable");
        }
    });
}