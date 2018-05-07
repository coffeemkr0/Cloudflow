using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Cloudflow.Core.ExtensionManagement;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class TriggerDefinition
    {
        public TriggerDefinition()
        {
            TriggerDefinitionId = Guid.NewGuid();
            TriggerConditionDefinitions = new List<TriggerConditionDefinition>();
        }

        public string Name { get; set; }

        public string AssemblyPath { get; set; }

        public Guid ExtensionId { get; set; }

        public string Configuration { get; set; }

        public Guid TriggerDefinitionId { get; set; }

        public int Index { get; set; }

        public virtual ICollection<TriggerConditionDefinition> TriggerConditionDefinitions { get; set; }

        public Guid JobDefinitionId { get; set; }

        [ScriptIgnore] public virtual JobDefinition JobDefinition { get; set; }


        public static TriggerDefinition CreateTestItem(string extensionsAssemblyPath, string name, int index,
            IConfigurationSerializer configurationSerializer)
        {
            var triggerDefinition = new TriggerDefinition
            {
                Name = name,
                Index = index,
                ExtensionId = Guid.Parse("DABF8963-4B59-448E-BE5A-143EBDF123EF"),
                AssemblyPath = extensionsAssemblyPath
            };

            var extensionService = new ExtensionService(configurationSerializer);
            var testCatalogProvider = new AssemblyCatalogProvider(extensionsAssemblyPath);

            var timerConfiguration =
                extensionService.CreateNewTriggerConfiguration(testCatalogProvider, triggerDefinition.ExtensionId);
            timerConfiguration.GetType().GetProperty("Interval").SetValue(timerConfiguration, 5000);
            triggerDefinition.Configuration = configurationSerializer.SerializeToString(timerConfiguration);

            //triggerDefinition.TriggerConditionDefinitions.Add(
            //    TriggerConditionDefinition.CreateTestItem(extensionsAssemblyPath, "Condition 1", 0));
            //triggerDefinition.TriggerConditionDefinitions.Add(
            //    TriggerConditionDefinition.CreateTestItem(extensionsAssemblyPath, "Condition 2", 1));

            return triggerDefinition;
        }
    }
}