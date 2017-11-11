using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class JobDefinition : ConfigurableExtensionDefinition
    {
        #region Properties
        public Guid JobDefinitionId { get; set; }

        public int Version { get; set; }

        public virtual ICollection<TriggerDefinition> TriggerDefinitions { get; set; }

        public virtual ICollection<StepDefinition> StepDefinitions { get; set; }

        public virtual ICollection<JobConditionDefinition> JobConditionDefinitions { get; set; }
        #endregion

        #region Constructors
        public JobDefinition()
        {
            this.JobDefinitionId = Guid.NewGuid();
            this.TriggerDefinitions = new List<TriggerDefinition>();
            this.StepDefinitions = new List<StepDefinition>();
            this.JobConditionDefinitions = new List<JobConditionDefinition>();
            this.Version = 1;
        }
        #endregion

        public static JobDefinition CreateTestItem(string extensionsAssemblyPath)
        {
            JobDefinition jobDefinition = new JobDefinition()
            {
                ExtensionId = Guid.Parse("3F6F5796-E313-4C53-8064-747C1989DA99"),
                ExtensionAssemblyPath = extensionsAssemblyPath,
                ConfigurationExtensionId = Guid.Parse("62A56D5B-07E5-41A3-A637-5E7C53FCF399"),
                ConfigurationExtensionAssemblyPath = extensionsAssemblyPath
            };

            var jobConfigurationController = new ExtensionConfigurationController(
                jobDefinition.ConfigurationExtensionId, extensionsAssemblyPath);

            var jobConfiguration = jobConfigurationController.CreateNewConfiguration();
            jobConfiguration.Name = "Hard coded test job";
            jobDefinition.Configuration = jobConfiguration.ToJson();

            jobDefinition.TriggerDefinitions.Add(TriggerDefinition.CreateTestItem(extensionsAssemblyPath, "Trigger 1", 0));
            jobDefinition.TriggerDefinitions.Add(TriggerDefinition.CreateTestItem(extensionsAssemblyPath, "Trigger 2", 1));
            jobDefinition.StepDefinitions.Add(StepDefinition.CreateTestItem(extensionsAssemblyPath, "Step 1", 0));
            jobDefinition.StepDefinitions.Add(StepDefinition.CreateTestItem(extensionsAssemblyPath, "Step 2", 1));

            return jobDefinition;
        }
    }
}
