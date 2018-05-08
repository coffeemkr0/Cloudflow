using System;
using System.Web.Script.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class TriggerConditionDefinition
    {
        public TriggerConditionDefinition()
        {
            TriggerConditionDefinitionId = Guid.NewGuid();
        }

        public string Name { get; set; }

        public string AssemblyPath { get; set; }

        public Guid ExtensionId { get; set; }

        public string Configuration { get; set; }

        public Guid TriggerConditionDefinitionId { get; set; }

        public int Index { get; set; }

        public Guid TriggerDefinitionId { get; set; }

        [ScriptIgnore] public virtual TriggerDefinition TriggerDefinition { get; set; }


        public static TriggerConditionDefinition CreateTestItem(string assemblyPath, string name, int index)
        {
            var conditionDefinition = new TriggerConditionDefinition
            {
                Index = index,
                ExtensionId = Guid.Parse("45C9872C-70DC-41E4-B769-3C27447F9E84"),
                AssemblyPath = assemblyPath
            };

            //var configurationController = new ExtensionConfigurationController(
            //    conditionDefinition.ConfigurationExtensionId, extensionsAssemblyPath);

            //var configuration = configurationController.CreateNewConfiguration();
            //configuration.Name = name;
            //conditionDefinition.Configuration = configuration.ToJson();

            //return conditionDefinition;

            return null;
        }
    }
}