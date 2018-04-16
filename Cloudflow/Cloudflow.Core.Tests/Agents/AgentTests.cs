using System;
using System.Collections.Generic;
using Cloudflow.Core.Agents;
using Cloudflow.Core.Extensions.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cloudflow.Core.Tests.Agents
{
    [TestClass]
    public class AgentTests
    {
        private TestJobControllerService _jobControllerService;
        private TestAgentMonitor _agentMonitor;
        private Agent _agent;

        [TestInitialize]
        public void TestInitialize()
        {
            _jobControllerService = new TestJobControllerService();
            _agentMonitor = new TestAgentMonitor();
            _agent = new Agent(_jobControllerService, _agentMonitor);
        }

        [TestMethod]
        public void AgentStartsAndUpdatesAgentMonitorToStarted()
        {
            _agent.Start();

            Assert.AreEqual(_agentMonitor.LastAgentStatus.Status, AgentStatus.AgentStatuses.Running);
        }
    }
}
