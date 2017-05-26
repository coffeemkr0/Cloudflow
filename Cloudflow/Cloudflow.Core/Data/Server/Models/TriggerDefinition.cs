using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Data.Server.Models
{
    public class TriggerDefinition
    {
        #region Properties
        public Guid TriggerDefinitionId { get; set; }

        public Guid TriggerConfigurationExtensionId { get; set; }

        public string TriggerConfigurationExtensionAssemblyPath { get; set; }

        public string Configuration { get; set; }


        public Guid JobDefinitionId { get; set; }

        public virtual JobDefinition JobDefinition { get; set; }
        #endregion

        #region Constructors
        public TriggerDefinition()
        {
            this.TriggerDefinitionId = Guid.NewGuid();
        }
        #endregion

        #region Public Methods
        public static TriggerDefinition CreateTestItem(string extensionsAssemblyPath)
        {
            var triggerExtensionId = Guid.Parse("E325CD29-053E-4422-97CF-C1C187760E88");

            TriggerDefinition triggerDefinition = new TriggerDefinition()
            {
                TriggerConfigurationExtensionId = triggerExtensionId,
                TriggerConfigurationExtensionAssemblyPath = extensionsAssemblyPath
            };

            var triggerConfigurationController = new TriggerConfigurationController(triggerExtensionId, extensionsAssemblyPath);
            var timerConfiguration = triggerConfigurationController.CreateNewConfiguration();
            timerConfiguration.TriggerName = "Hard coded timer trigger";
            timerConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;
            timerConfiguration.GetType().GetProperty("Interval").SetValue(timerConfiguration, 5000);

            triggerDefinition.Configuration = timerConfiguration.ToJson();
            return triggerDefinition;
        }
        #endregion
    }
}
