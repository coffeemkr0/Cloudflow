using System;

namespace TempProject.Steps
{
    public class StepDefinition
    {
        public string Name { get; set; }

        public string AssemblyPath { get; set; }

        public Guid ExtensionId { get; set; }

        public string Configuration { get; set; }
    }
}
