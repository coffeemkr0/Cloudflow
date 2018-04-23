using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;
using TempProject.Interfaces;
using TempProject.Tests.Job;
using TempProject.Tests.Steps;
using TempProject.Tests.Triggers;

namespace TempProject.Tests.Agents
{
    [TestClass]
    public class AgentShould
    {
        private Agent _agent;
        private AgentMonitor _agentMonitor;

        private IJob GetTestJob()
        {
            var triggers = new List<ITrigger>
            {
                new ImmediateTrigger()
            };

            var steps = new List<IStep>
            {
                new TestStep()
            };

            return new DefaultJob(triggers, steps);
        }

        private IJob GetExceptionJob()
        {
            var triggers = new List<ITrigger>
            {
                new ImmediateTrigger()
            };

            var steps = new List<IStep>
            {
                new ExceptionTestStep()
            };

            return new DefaultJob(triggers, steps);
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _agentMonitor = new AgentMonitor();
        }

        [TestMethod]
        public void Start()
        {
            _agent = new Agent(new List<IJob> {GetTestJob()});
            _agent.Start(_agentMonitor);
            Assert.IsTrue(_agentMonitor.OnAgentStartedCalled);
        }

        [TestMethod]
        public void Stop()
        {
            _agent = new Agent(new List<IJob> {GetTestJob()});
            _agent.Start(_agentMonitor);
            _agent.Stop();
            Assert.IsTrue(_agentMonitor.OnAgentStopCalled);
        }

        [TestMethod]
        public void PostActivityWhenJobStarts()
        {
            _agent = new Agent(new List<IJob> {GetTestJob()});
            _agent.Start(_agentMonitor);
            Assert.IsTrue(_agentMonitor.OnAgentActivityCalled);
        }

        [TestMethod]
        public void PostActivityWhenJobHasAnException()
        {
            _agent = new Agent(new List<IJob> {GetExceptionJob()});
            _agent.Start(_agentMonitor);
            Assert.IsTrue(_agentMonitor.OnAgentActivityCalled);
        }
    }
}