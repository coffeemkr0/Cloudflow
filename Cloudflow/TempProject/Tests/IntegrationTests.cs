using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Agents;
using TempProject.Jobs;
using TempProject.Steps;
using TempProject.Tests.Agents;
using TempProject.Tests.Steps;
using TempProject.Tests.Triggers;
using TempProject.Triggers;

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
            var jobDefinition = new JobDefinition
            {
                Name = "Integration Test Job"
            };

            jobDefinition.TriggerDefinitions.Add(new TriggerDefinition
            {
                AssemblyPath = GetType().Assembly.CodeBase,
                ExtensionId = Guid.Parse(ImmediateTriggerDescriptor.Id),
                Name = "Immediate Trigger"
            });

            jobDefinition.StepDefinitions.Add(new StepDefinition
            {
                AssemblyPath = GetType().Assembly.CodeBase,
                ExtensionId = Guid.Parse(TestStepDescriptor.Id),
                Name = "Test Step"
            });

            jobDefinition.StepDefinitions.Add(new StepDefinition
            {
                AssemblyPath = GetType().Assembly.CodeBase,
                ExtensionId = Guid.Parse(ConfigurableTestStepDescriptor.Id),
                Configuration = "{\"Message\":\"Integration Test\"}",
                Name = "Configurable Test Step"
            });

            var extensionService = new ExtensionService.ExtensionService();
            var jobConfigurationFactory = new JobConfigurationFactory(jobDefinition, extensionService);
            var jobConfiguration = jobConfigurationFactory.CreateJobConfiguration();

            var job = new Jobs.Job(jobConfiguration);
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
            Assert.IsFalse(_agentMonitor.ExceptionOccurred);
        }
    }
}