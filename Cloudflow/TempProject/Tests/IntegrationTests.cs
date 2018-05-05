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
                extensionService.GetTrigger(ImmediateTrigger.ExtensionId)
            };

            var stepConfiguration =
                (ConfigurableStepConfiguration) extensionService.GetExtension(ConfigurableStepConfiguration
                    .ExtensionId);
            stepConfiguration.Message = "Integration test";

            var stepExtensionService = new ExtensionService(assemblyCatalogProvider, stepConfiguration);

            var steps = new List<IStep>
            {
                stepExtensionService.GetStep(ConfigurableTestStep.ExtensionId),
                stepExtensionService.GetStep(TestStep.ExtensionId)
            };

            var jobConfiguration = new JobConfiguration
            {
                Triggers = triggers,
                Steps = steps
            };

            var job = new Implementations.Job(jobConfiguration);
            var jobs = new List<IJob>
            {
                job
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
