using System.Collections.Generic;
using TempProject.Steps;
using TempProject.Triggers;

namespace TempProject.Jobs
{
    public class JobDefinition
    {
        public JobDefinition()
        {
            TriggerDefinitions = new List<TriggerDefinition>();
            StepDefinitions = new List<StepDefinition>();
        }

        public string Name { get; set; }

        public List<TriggerDefinition> TriggerDefinitions { get; set; }

        public List<StepDefinition> StepDefinitions { get; set; }
    }
}