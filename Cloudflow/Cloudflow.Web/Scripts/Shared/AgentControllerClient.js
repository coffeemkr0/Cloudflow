
function AgentControllerClient() {
    //Properties
    this.Agents = null;
    this.AgentControllerProxies = null;

    //Events
    this.AgentConnected = null;
    this.AgentStatusUpdated = null;
    this.RunStatusChanged = null;
}

AgentControllerClient.ConnectToAgents = function () {

    AgentControllerClient.AgentControllerProxies = [];

    AgentControllerClient.Agents.forEach(function (agent) {
        //Create a connetion to the agent
        var connection = $.hubConnection("http://" + agent.machineName + ":" + agent.port + "/CloudflowAgent/signalr");

        //Get the hub proxy object from the connection
        var agentControllerProxy = connection.createHubProxy("agentController");

        //Create methods that the server can call on the client
        agentControllerProxy.on('updateStatus', function (status) {
            AgentControllerClient.OnAgentStatusUpdated(agent.machineName, status);
        });
        agentControllerProxy.on('runStatusChanged', function (run) {
            AgentControllerClient.OnRunStatusChanged(agent.machineName, run);
        });

        //Open the connection
        connection.start().done(function () {
            //If the connection was successful, store the proxy so we can reuse it
            AgentControllerClient.AgentControllerProxies.push({ machineName: agent.machineName, proxy: agentControllerProxy });

            //Let subscribers know that the agent is now connected
            AgentControllerClient.OnAgentConnected(agent.machineName);
        }).fail(function () {
            AgentControllerClient.OnAgentStatusUpdated(agent.machineName, null);
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

AgentControllerClient.GetQueuedRuns = function (machineName, callback) {
    var agentControllerProxyEntry = AgentControllerClient.AgentControllerProxies.find(function (item) {
        return item.machineName === machineName;
    });

    if (typeof agentControllerProxyEntry !== "undefined") {
        agentControllerProxyEntry.proxy.invoke("getQueuedRuns").done(function (runs) {
            callback(runs);
        }).fail(function (error) {
            callback(null);
        });
    }
};

AgentControllerClient.GetCompletedRuns = function (machineName, startIndex, pageSize, callback) {
    var agentControllerProxyEntry = AgentControllerClient.AgentControllerProxies.find(function (item) {
        return item.machineName === machineName;
    });

    if (typeof agentControllerProxyEntry !== "undefined") {
        agentControllerProxyEntry.proxy.invoke("getCompletedRuns", startIndex, pageSize).done(function (runs) {
            callback(runs);
        }).fail(function (error) {
            callback(null);
        });
    }
};

AgentControllerClient.OnAgentConnected = function (machineName) {
    if (AgentControllerClient.AgentConnected !== null) {
        AgentControllerClient.AgentConnected(machineName);
    }
}

AgentControllerClient.OnAgentStatusUpdated = function (machineName, status) {
    if (AgentControllerClient.AgentStatusUpdated !== null) {
        AgentControllerClient.AgentStatusUpdated(machineName, status);
    }
}

AgentControllerClient.OnRunStatusChanged = function (machineName, run) {
    if (AgentControllerClient.RunStatusChanged !== null) {
        AgentControllerClient.RunStatusChanged(machineName, run);
    }
}