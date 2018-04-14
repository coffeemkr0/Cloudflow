using Cloudflow.Core.Extensions.Controllers;
using System;
using System.Web.Script.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class TriggerConditionDefinition : ConfigurableExtensionDefinition
    {
        #region Properties
        public Guid TriggerConditionDefinitionId { get; set; }

        public int Index { get; set; }

        public Guid TriggerDefinitionId { get; set; }

        [ScriptIgnore]
        public virtual TriggerDefinition TriggerDefinition { get; set; }
        #endregion

        #region Constructors
        public TriggerConditionDefinition()
        {
            TriggerConditionDefinitionId = Guid.NewGuid();
        }
        #endregion

        #region Public Methods
        public static TriggerConditionDefinition CreateTestItem(string extensionsAssemblyPath, string name, int index)
        {
            var conditionDefinition = new TriggerConditionDefinition()
            {
                Index = index,
                ExtensionId = Guid.Parse("45C9872C-70DC-41E4-B769-3C27447F9E84"),
                ExtensionAssemblyPath = extensionsAssemblyPath,
                ConfigurationExtensionId = Guid.Parse("2822B8DB-56BF-42C2-869D-C4C658CF8A34"),
                ConfigurationExtensionAssemblyPath = extensionsAssemblyPath
            };

            var configurationController = new ExtensionConfigurationController(
                conditionDefinition.ConfigurationExtensionId, extensionsAssemblyPath);

            var configuration = configurationController.CreateNewConfiguration();
            configuration.Name = name;
            conditionDefinition.Configuration = configuration.ToJson();

            return conditionDefinition;
        }
        #endregion
    }
}
