
function AgentHubClient() {
    //Properties
    this.Agents = null;
    this.AgentHubProxies = null;

    //Events
    this.AgentConnected = null;
    this.AgentStatusUpdated = null;
    this.RunStatusChanged = null;
}

AgentHubClient.ConnectToAgents = function () {

    AgentHubClient.AgentHubProxies = [];

    AgentHubClient.Agents.forEach(function (agent) {
        //Create a connetion to the agent
        var connection = $.hubConnection("http://" + agent.machineName + ":" + agent.port + "/CloudflowAgent/signalr");

        //Get the hub proxy object from the connection
        var agentHubProxy = connection.createHubProxy("agentHub");

        //Create methods that the server can call on the client
        agentHubProxy.on('updateStatus', function (status) {
            AgentHubClient.OnAgentStatusUpdated(agent.machineName, status);
        });
        agentHubProxy.on('runStatusChanged', function (run) {
            AgentHubClient.OnRunStatusChanged(agent.machineName, run);
        });

        //Open the connection
        connection.start().done(function () {
            //If the connection was successful, store the proxy so we can reuse it
            AgentHubClient.AgentHubProxies.push({ machineName: agent.machineName, proxy: agentHubProxy });

            //Let subscribers know that the agent is now connected
            AgentHubClient.OnAgentConnected(agent.machineName);
        }).fail(function () {
            AgentHubClient.OnAgentStatusUpdated(agent.machineName, null);
        });
    });
};

AgentHubClient.StartAgent = function (machineName, callback) {
    var agentHubProxyEntry = AgentHubClient.AgentHubProxies.find(function (item) {
        return item.machineName === machineName;
    });

    if (typeof agentHubProxyEntry !== "undefined") {
        agentHubProxyEntry.proxy.invoke("startAgent").done(function () {
            callback();
        }).fail(function () {
            callback();
        });
    }
};

AgentHubClient.StopAgent = function (machineName, callback) {
    var agentHubProxyEntry = AgentHubClient.AgentHubProxies.find(function (item) {
        return item.machineName === machineName;
    });

    if (typeof agentHubProxyEntry !== "undefined") {
        agentHubProxyEntry.proxy.invoke("stopAgent").done(function () {
            callback();
        }).fail(function () {
            callback();
        });
    }
};

AgentHubClient.GetAgentStatus = function (machineName, callback) {
    var agentHubProxyEntry = AgentHubClient.AgentHubProxies.find(function (item) {
        return item.machineName === machineName;
    });

    if (typeof agentHubProxyEntry !== "undefined") {
        agentHubProxyEntry.proxy.invoke("getAgentStatus").done(function (status) {
            callback(status);
        }).fail(function (error) {
            callback(null);
        });
    }
};

AgentHubClient.GetQueuedRuns = function (machineName, callback) {
    var agentHubProxyEntry = AgentHubClient.AgentHubProxies.find(function (item) {
        return item.machineName === machineName;
    });

    if (typeof agentHubProxyEntry !== "undefined") {
        agentHubProxyEntry.proxy.invoke("getQueuedRuns").done(function (runs) {
            callback(runs);
        }).fail(function (error) {
            callback(null);
        });
    }
};

AgentHubClient.GetCompletedRuns = function (machineName, startIndex, pageSize, callback) {
    var agentHubProxyEntry = AgentHubClient.AgentHubProxies.find(function (item) {
        return item.machineName === machineName;
    });

    if (agentHubProxyEntry !== undefined) {
        agentHubProxyEntry.proxy.invoke("getCompletedRuns", startIndex, pageSize).done(function (runs) {
            callback(runs);
        }).fail(function (error) {
            callback(null);
        });
    }
};

AgentHubClient.OnAgentConnected = function (machineName) {
    if (AgentHubClient.AgentConnected !== undefined) {
        AgentHubClient.AgentConnected(machineName);
    }
}

AgentHubClient.OnAgentStatusUpdated = function (machineName, status) {
    if (AgentHubClient.AgentStatusUpdated !== undefined) {
        AgentHubClient.AgentStatusUpdated(machineName, status);
    }
}

AgentHubClient.OnRunStatusChanged = function (machineName, run) {
    if (AgentHubClient.RunStatusChanged !== undefined) {
        AgentHubClient.RunStatusChanged(machineName, run);
    }
}

AgentHubClient.PublishJob = function (jobDefinition, callback) {
    AgentHubClient.AgentHubProxies.forEach(function (agentHubProxyEntry) {
        agentHubProxyEntry.proxy.invoke("publishJob", jobDefinition).done(function () {
            callback();
        }).fail(function (error) {
            console.log("Communication error " + error);
        });
    });
};