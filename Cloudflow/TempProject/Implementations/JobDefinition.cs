using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempProject.Implementations
{
    public class JobDefinition
    {
        public string Name { get; set; }

        public List<TriggerDefinition> TriggerDefinitions { get; set; }

        public List<StepDefinition> StepDefinitions { get; set; }

        public JobDefinition()
        {
            TriggerDefinitions = new List<TriggerDefinition>();
            StepDefinitions = new List<StepDefinition>();
        }
    }
}
