using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TempProject.Agents;
using TempProject.ExtensionService;
using TempProject.Jobs;
using TempProject.Steps;
using TempProject.Tests.Agents;
using TempProject.Tests.Steps;
using TempProject.Tests.Triggers;
using TempProject.Triggers;
using System.Linq;
using Newtonsoft.Json;
using TempProject.Serialization;

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
            var testAssemblyCatalogProvider = new AssemblyCatalogProvider(this.GetType().Assembly.CodeBase);
            var extensionService = new ExtensionService.ExtensionService(new JsonConfigurationSerializer());

            var jobDefinition = new JobDefinition
            {
                Name = "Integration Test Job"
            };

            var immediateTriggerDescriptor = extensionService.GetTriggerDescriptors(testAssemblyCatalogProvider)
                .First(i => i.ExtensionId == Guid.Parse(ImmediateTriggerDescriptor.Id));

            jobDefinition.TriggerDefinitions.Add(new TriggerDefinition
            {
                AssemblyPath = GetType().Assembly.CodeBase,
                ExtensionId = immediateTriggerDescriptor.ExtensionId,
                Name = immediateTriggerDescriptor.Name
            });

            var testStepDescriptor = extensionService.GetStepDescriptors(testAssemblyCatalogProvider)
                .First(i => i.ExtensionId == Guid.Parse(TestStepDescriptor.Id));

            jobDefinition.StepDefinitions.Add(new StepDefinition
            {
                AssemblyPath = GetType().Assembly.CodeBase,
                ExtensionId = testStepDescriptor.ExtensionId,
                Name = testStepDescriptor.Name
            });

            var configurableTestStepDescriptor = extensionService.GetStepDescriptors(testAssemblyCatalogProvider)
                .First(i => i.ExtensionId == Guid.Parse(ConfigurableTestStepDescriptor.Id));

            var configuration = extensionService.CreateNewStepConfiguration(testAssemblyCatalogProvider,
                configurableTestStepDescriptor.ExtensionId);
            ((ConfigurableStepConfiguration) configuration).Message = "Integration Test";

            var configurationJson = JsonConvert.SerializeObject(configuration);

            jobDefinition.StepDefinitions.Add(new StepDefinition
            {
                AssemblyPath = GetType().Assembly.CodeBase,
                ExtensionId = configurableTestStepDescriptor.ExtensionId,
                Configuration = configurationJson,
                Name = configurableTestStepDescriptor.Name
            });

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