using System;
using System.Collections.Generic;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cloudflow.Core.Tests.Agents
{
    [TestClass]
    public class AgentTests
    {
        private TestAgentMonitor _agentMonitor;
        private Agent _agent;

        [TestInitialize]
        public void TestInitialize()
        {
            _agentMonitor = new TestAgentMonitor();
            _agent = new Agent(new List<IJobController>(), _agentMonitor);
        }

        [TestMethod]
        public void AgentStartsAndUpdatesAgentMonitorToStarted()
        {
            _agent.Start();

            Assert.AreEqual(_agentMonitor.LastAgentStatus.Status, AgentStatus.AgentStatuses.Running);
        }
    }
}
