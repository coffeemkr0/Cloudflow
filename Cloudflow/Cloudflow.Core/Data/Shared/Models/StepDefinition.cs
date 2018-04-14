using Cloudflow.Core.Extensions.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class StepDefinition : ConfigurableExtensionDefinition
    {
        #region Properties
        public Guid StepDefinitionId { get; set; }

        public int Index { get; set; }

        public virtual ICollection<StepConditionDefinition> StepConditionDefinitions { get; set; }

        public Guid JobDefinitionId { get; set; }

        [ScriptIgnore]
        public virtual JobDefinition JobDefinition { get; set; }
        #endregion

        #region Constructors
        public StepDefinition()
        {
            StepDefinitionId = Guid.NewGuid();
            StepConditionDefinitions = new List<StepConditionDefinition>();
        }
        #endregion

        #region Public Methods
        public static StepDefinition CreateTestItem(string extensionsAssemblyPath, string name, int index)
        {
            var stepDefinition = new StepDefinition()
            {
                Index = index,
                ExtensionId = Guid.Parse("43D6FD16-0344-4204-AEE9-A09B3998C017"),
                ExtensionAssemblyPath = extensionsAssemblyPath,
                ConfigurationExtensionId = Guid.Parse("191A3C1A-FD25-4790-8141-DFC132DA4970"),
                ConfigurationExtensionAssemblyPath = extensionsAssemblyPath
            };

            var stepConfigurationController = new ExtensionConfigurationController(
                stepDefinition.ConfigurationExtensionId, extensionsAssemblyPath);

            var logStepConfiguration = stepConfigurationController.CreateNewConfiguration();
            logStepConfiguration.Name = name;
            logStepConfiguration.GetType().GetProperty("LogMessage").SetValue(logStepConfiguration, "Hello World!");
            stepDefinition.Configuration = logStepConfiguration.ToJson();

            return stepDefinition;
        }
        #endregion
    }
}
