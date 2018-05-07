using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Cloudflow.Core.ExtensionManagement;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class StepDefinition
    {
        public StepDefinition()
        {
            StepDefinitionId = Guid.NewGuid();
            StepConditionDefinitions = new List<StepConditionDefinition>();
        }

        public string Name { get; set; }

        public string AssemblyPath { get; set; }

        public Guid ExtensionId { get; set; }

        public string Configuration { get; set; }

        public Guid StepDefinitionId { get; set; }

        public int Index { get; set; }

        public virtual ICollection<StepConditionDefinition> StepConditionDefinitions { get; set; }

        public Guid JobDefinitionId { get; set; }

        [ScriptIgnore] public virtual JobDefinition JobDefinition { get; set; }

        public static StepDefinition CreateTestItem(string extensionsAssemblyPath, string name, int index,
            IConfigurationSerializer configurationSerializer)
        {
            var stepDefinition = new StepDefinition
            {
                Name = name,
                Index = index,
                ExtensionId = Guid.Parse("43D6FD16-0344-4204-AEE9-A09B3998C017"),
                AssemblyPath = extensionsAssemblyPath
            };

            var extensionService = new ExtensionService(configurationSerializer);
            var testCatalogProvider = new AssemblyCatalogProvider(extensionsAssemblyPath);

            var logStepConfiguration =
                extensionService.CreateNewStepConfiguration(testCatalogProvider, stepDefinition.ExtensionId);
            logStepConfiguration.GetType().GetProperty("LogMessage").SetValue(logStepConfiguration, "Hello World!");
            stepDefinition.Configuration = configurationSerializer.SerializeToString(logStepConfiguration);

            return stepDefinition;
        }
    }
}