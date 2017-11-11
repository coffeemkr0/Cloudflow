using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class TriggerDefinition : ConfigurableExtensionDefinition
    {
        #region Properties
        [Index("IX_TriggerDefinitionId_Index", 1, IsUnique = true)]
        public Guid TriggerDefinitionId { get; set; }

        [Index("IX_TriggerDefinitionId_Index", 2, IsUnique = true)]
        public int Index { get; set; }

        public virtual ICollection<TriggerConditionDefinition> TriggerConditionDefinitions { get; set; }

        public Guid JobDefinitionId { get; set; }

        [ScriptIgnore]
        public virtual JobDefinition JobDefinition { get; set; }
        #endregion

        #region Constructors
        public TriggerDefinition()
        {
            this.TriggerDefinitionId = Guid.NewGuid();
            this.TriggerConditionDefinitions = new List<TriggerConditionDefinition>();
        }
        #endregion

        #region Public Methods
        public static TriggerDefinition CreateTestItem(string extensionsAssemblyPath, string name)
        {
            TriggerDefinition triggerDefinition = new TriggerDefinition()
            {
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

            triggerDefinition.TriggerConditionDefinitions.Add(TriggerConditionDefinition.CreateTestItem(extensionsAssemblyPath, "Condition 1"));
            triggerDefinition.TriggerConditionDefinitions.Add(TriggerConditionDefinition.CreateTestItem(extensionsAssemblyPath, "Condition 2"));

            return triggerDefinition;
        }
        #endregion
    }
}
