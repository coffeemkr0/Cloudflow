
function HomeIndex() {
    this.MachineNames = null;
}

$(function () {
    AgentControllerClient.AgentConnected = AgentConnected;
    AgentControllerClient.AgentMessageReceived = AgentMessageReceived;

    AgentControllerClient.ConnectToAgents(HomeIndex.MachineNames);

    $(".startAgentLink").each(function () {
        $(this).on("click", function (e) {
            var $item = $(e.target);
            var machineName = $item.attr("data-machinename");
            AgentControllerClient.StartAgent(machineName, function () {
                SetStatusText(machineName);
            });
            return false;
        });
    });
});

function AgentConnected(machineName) {
    SetStatusText(machineName);
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
        $("#agentStatus-" + machineName).text(status);
    });
}