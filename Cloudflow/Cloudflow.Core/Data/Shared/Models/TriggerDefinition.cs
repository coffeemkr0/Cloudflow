using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class TriggerDefinition
    {
        #region Properties
        public Guid TriggerDefinitionId { get; set; }

        public Guid TriggerConfigurationExtensionId { get; set; }

        public string TriggerConfigurationExtensionAssemblyPath { get; set; }

        public int Index { get; set; }

        public string Configuration { get; set; }

        public Guid JobDefinitionId { get; set; }

        [ScriptIgnore]
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
            var triggerConfigurationExtensionId = Guid.Parse("E325CD29-053E-4422-97CF-C1C187760E88");

            TriggerDefinition triggerDefinition = new TriggerDefinition()
            {
                TriggerConfigurationExtensionId = triggerConfigurationExtensionId,
                TriggerConfigurationExtensionAssemblyPath = extensionsAssemblyPath
            };

            var triggerConfigurationController = new ExtensionConfigurationController(triggerConfigurationExtensionId, extensionsAssemblyPath);
            var timerConfiguration = triggerConfigurationController.CreateNewConfiguration();
            timerConfiguration.ExtensionId = Guid.Parse("DABF8963-4B59-448E-BE5A-143EBDF123EF");
            timerConfiguration.Name = "Hard coded timer trigger";
            timerConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;
            timerConfiguration.GetType().GetProperty("Interval").SetValue(timerConfiguration, 5000);

            triggerDefinition.Configuration = timerConfiguration.ToJson();
            return triggerDefinition;
        }
        #endregion
    }
}
