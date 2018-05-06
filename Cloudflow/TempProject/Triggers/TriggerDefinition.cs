using System;

namespace TempProject.Triggers
{
    public class TriggerDefinition
    {
        public string Name { get; set; }

        public string AssemblyPath { get; set; }

        public Guid ExtensionId { get; set; }

        public string Configuration { get; set; }
    }
}