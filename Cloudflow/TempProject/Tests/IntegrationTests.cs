using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Implementations;
using TempProject.Interfaces;
using TempProject.Tests.Agents;
using TempProject.Tests.Steps;
using TempProject.Tests.Triggers;

namespace TempProject.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private Agent _agent;
        private AgentMonitor _agentMonitor;

        [TestInitialize]
        public void InitializeTest()
        {
            var assemblyCatalogProvider = new AssemblyCatalogProvider(this.GetType().Assembly.CodeBase);
            var extensionService = new ExtensionService(assemblyCatalogProvider);

            var triggers = new List<ITrigger>
            {
                (ITrigger)extensionService.GetExtension(ImmediateTrigger.ExtensionId)
            };

            var stepConfiguration =
                (ConfigurableStepConfiguration) extensionService.GetExtension(ConfigurableStepConfiguration
                    .ExtensionId);
            stepConfiguration.Message = "Integration test";

            var stepExtensionService = new ExtensionService(assemblyCatalogProvider, stepConfiguration);

            var steps = new List<IStep>
            {
                (IStep)stepExtensionService.GetExtension(ConfigurableTestStep.ExtensionId),
                (IStep)stepExtensionService.GetExtension(TestStep.ExtensionId)
            };

            var jobs = new List<IJob>
            {
                new DefaultJob(triggers, steps)
            };

            _agent = new Agent(jobs);

            _agentMonitor = new AgentMonitor();
        }

        [TestMethod]
        public void AgentStartsRunsAndStops()
        {
            _agent.Start(_agentMonitor);
            _agent.Stop();

            Assert.IsTrue(_agentMonitor.OnAgentStartedCalled);
            Assert.IsTrue(_agentMonitor.OnAgentActivityCalled);
            Assert.IsTrue(_agentMonitor.OnAgentStopCalled);
        }
    }
}
