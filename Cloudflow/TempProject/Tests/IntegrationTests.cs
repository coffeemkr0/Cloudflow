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

        private List<ITrigger> GetTestTriggers()
        {
            var triggers = new List<ITrigger>();

            

            return triggers;
        }

        [TestInitialize]
        public void InitializeTest()
        {
            var assemblyCatalogProvider = new AssemblyCatalogProvider(this.GetType().Assembly.CodeBase);

            var stepConfigurationExtensionService = new StepConfigurationExtensionService(assemblyCatalogProvider);
            var stepConfiguration =
                (ConfigurableStepConfiguration)stepConfigurationExtensionService.GetConfiguration(
                    Guid.Parse(ConfigurableStepConfiguration.ExtensionId));
            stepConfiguration.Message = "Integration test";

            var stepExtensionService = new StepExtensionService(assemblyCatalogProvider, stepConfiguration);

            var steps = new List<IStep>
            {
                stepExtensionService.GetStep(Guid.Parse(ConfigurableTestStep.ExtensionId)),
                stepExtensionService.GetStep(Guid.Parse(TestStep.ExtensionId))
            };

            var triggers = new List<ITrigger>
            {
                new ImmediateTrigger()
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
