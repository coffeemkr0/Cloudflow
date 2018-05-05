using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class JobConfigurationFactory
    {
        private readonly JobDefinition _jobDefinition;
        private readonly IExtensionService _extensionService;

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
                triggers.Add(_extensionService.GetTrigger(triggerDefinition.ExtensionId));
            }

            var steps = new List<IStep>();

            foreach (var stepDefinition in _jobDefinition.StepDefinitions)
            {
                steps.Add(_extensionService.GetStep(stepDefinition.ExtensionId));
            }

            jobConfiguration.Steps = steps;

            return jobConfiguration;
        }
    }
}
