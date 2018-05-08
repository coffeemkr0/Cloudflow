using System;
using System.Web.Script.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class StepConditionDefinition
    {
        public StepConditionDefinition()
        {
            StepConditionDefinitionId = Guid.NewGuid();
        }

        public string Name { get; set; }

        public string AssemblyPath { get; set; }

        public Guid ExtensionId { get; set; }

        public string Configuration { get; set; }

        public Guid StepConditionDefinitionId { get; set; }

        public int Index { get; set; }

        public Guid StepDefinitionId { get; set; }

        [ScriptIgnore] public virtual StepDefinition StepDefinition { get; set; }
    }
}