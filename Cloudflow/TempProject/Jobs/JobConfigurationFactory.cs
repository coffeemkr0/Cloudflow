using System.Collections.Generic;
using TempProject.ExtensionService;
using TempProject.Steps;
using TempProject.Triggers;

namespace TempProject.Jobs
{
    public class JobConfigurationFactory
    {
        private readonly IExtensionService _extensionService;
        private readonly JobDefinition _jobDefinition;

        public JobConfigurationFactory(JobDefinition jobDefinition, IExtensionService extensionService)
        {
            _jobDefinition = jobDefinition;
            _extensionService = extensionService;
        }

        public JobConfiguration CreateJobConfiguration()
        {
            var jobConfiguration = new JobConfiguration {Name = _jobDefinition.Name};

            var triggers = new List<ITrigger>();

            jobConfiguration.Triggers = triggers;

            foreach (var triggerDefinition in _jobDefinition.TriggerDefinitions)
            {
                var catalogProvider = new AssemblyCatalogProvider(triggerDefinition.AssemblyPath);
                var triggerConfiguration =
                    _extensionService.LoadTriggerConfiguration(catalogProvider,
                        triggerDefinition.ExtensionId, triggerDefinition.Configuration);

                triggers.Add(_extensionService.LoadTrigger(catalogProvider, triggerDefinition.ExtensionId,
                    triggerConfiguration));
            }

            var steps = new List<IStep>();

            foreach (var stepDefinition in _jobDefinition.StepDefinitions)
            {
                var catalogProvider = new AssemblyCatalogProvider(stepDefinition.AssemblyPath);
                var stepConfiguration =
                    _extensionService.LoadStepConfiguration(catalogProvider,
                        stepDefinition.ExtensionId, stepDefinition.Configuration);
                steps.Add(_extensionService.LoadStep(catalogProvider, stepDefinition.ExtensionId,
                    stepConfiguration));
            }

            jobConfiguration.Steps = steps;

            return jobConfiguration;
        }
    }
}