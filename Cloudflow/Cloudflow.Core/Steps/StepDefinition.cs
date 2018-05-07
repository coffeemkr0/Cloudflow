using System;

namespace Cloudflow.Core.Steps
{
    public class StepDefinition
    {
        public string Name { get; set; }

        public string AssemblyPath { get; set; }

        public Guid ExtensionId { get; set; }

        public string Configuration { get; set; }
    }
}