using Cloudflow.Core.Extensions.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class TriggerDefinition : ConfigurableExtensionDefinition
    {
        #region Properties
        public Guid TriggerDefinitionId { get; set; }

        public int Index { get; set; }

        public virtual ICollection<TriggerConditionDefinition> TriggerConditionDefinitions { get; set; }

        public Guid JobDefinitionId { get; set; }

        [ScriptIgnore]
        public virtual JobDefinition JobDefinition { get; set; }
        #endregion

        #region Constructors
        public TriggerDefinition()
        {
            TriggerDefinitionId = Guid.NewGuid();
            TriggerConditionDefinitions = new List<TriggerConditionDefinition>();
        }
        #endregion

        #region Public Methods
        public static TriggerDefinition CreateTestItem(string extensionsAssemblyPath, string name, int index)
        {
            var triggerDefinition = new TriggerDefinition()
            {
                Index = index,
                ExtensionId = Guid.Parse("DABF8963-4B59-448E-BE5A-143EBDF123EF"),
                ExtensionAssemblyPath = extensionsAssemblyPath,
                ConfigurationExtensionId = Guid.Parse("E325CD29-053E-4422-97CF-C1C187760E88"),
                ConfigurationExtensionAssemblyPath = extensionsAssemblyPath
            };

            var triggerConfigurationController = new ExtensionConfigurationController(
                triggerDefinition.ConfigurationExtensionId, extensionsAssemblyPath);

            var timerConfiguration = triggerConfigurationController.CreateNewConfiguration();
            timerConfiguration.Name = name;
            timerConfiguration.GetType().GetProperty("Interval").SetValue(timerConfiguration, 5000);
            triggerDefinition.Configuration = timerConfiguration.ToJson();

            triggerDefinition.TriggerConditionDefinitions.Add(TriggerConditionDefinition.CreateTestItem(extensionsAssemblyPath, "Condition 1", 0));
            triggerDefinition.TriggerConditionDefinitions.Add(TriggerConditionDefinition.CreateTestItem(extensionsAssemblyPath, "Condition 2", 1));

            return triggerDefinition;
        }
        #endregion
    }
}
