
function AgentControllerClient() {
    this.AgentControllerProxies = null;
    this.AgentConnected = null;
    this.AgentStatusUpdated = null;
}

AgentControllerClient.ConnectToAgents = function (agents) {

    AgentControllerClient.AgentControllerProxies = [];

    agents.forEach(function (agent) {
        //Create a connetion to the agent
        var connection = $.hubConnection("http://" + agent.machineName + ":" + agent.port + "/CloudflowAgent/signalr");

        //For each hub we care about, create a proxy and sign up for the methods that the server will call to the client
        var agentControllerProxy = connection.createHubProxy("agentController");
        agentControllerProxy.on('updateStatus', function (status) {
            AgentControllerClient.AgentStatusUpdated(agent.machineName, status);
        });

        //Open the connection
        connection.start().done(function () {
            //If the connection was successful, store the proxy so we can reuse it
            AgentControllerClient.AgentControllerProxies.push({ machineName: agent.machineName, proxy: agentControllerProxy });
            //Let subscribers know that the agent is now connected
            AgentControllerClient.AgentConnected(agent.machineName);
        }).fail(function () {
            AgentControllerClient.AgentStatusUpdated(agent.machineName, null);
        });
    });
};

AgentControllerClient.StartAgent = function (machineName, callback) {
    var agentControllerProxyEntry = AgentControllerClient.AgentControllerProxies.find(function (item) {
        return item.machineName === machineName;
    });

    if (typeof agentControllerProxyEntry !== "undefined") {
        agentControllerProxyEntry.proxy.invoke("startAgent").done(function () {
            callback();
        }).fail(function () {
            callback();
        });
    }
};

AgentControllerClient.StopAgent = function (machineName, callback) {
    var agentControllerProxyEntry = AgentControllerClient.AgentControllerProxies.find(function (item) {
        return item.machineName === machineName;
    });

    if (typeof agentControllerProxyEntry !== "undefined") {
        agentControllerProxyEntry.proxy.invoke("stopAgent").done(function () {
            callback();
        }).fail(function () {
            callback();
        });
    }
};

AgentControllerClient.GetAgentStatus = function (machineName, callback) {
    var agentControllerProxyEntry = AgentControllerClient.AgentControllerProxies.find(function (item) {
        return item.machineName === machineName;
    });

    if (typeof agentControllerProxyEntry !== "undefined") {
        agentControllerProxyEntry.proxy.invoke("getAgentStatus").done(function (status) {
            callback(status);
        }).fail(function (error) {
            callback(null);
        });
    }
};