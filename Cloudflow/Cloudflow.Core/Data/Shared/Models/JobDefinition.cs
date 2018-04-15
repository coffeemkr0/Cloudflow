using System;
using System.Collections.Generic;
using Cloudflow.Core.Extensions.Controllers;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class JobDefinition : ConfigurableExtensionDefinition
    {
        #region Constructors

        public JobDefinition()
        {
            JobDefinitionId = Guid.NewGuid();
            TriggerDefinitions = new List<TriggerDefinition>();
            StepDefinitions = new List<StepDefinition>();
            Version = 1;
        }

        #endregion

        public static JobDefinition CreateTestItem(string extensionsAssemblyPath)
        {
            var jobDefinition = new JobDefinition
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

            jobDefinition.TriggerDefinitions.Add(TriggerDefinition.CreateTestItem(extensionsAssemblyPath, "Trigger 1",
                0));
            jobDefinition.TriggerDefinitions.Add(TriggerDefinition.CreateTestItem(extensionsAssemblyPath, "Trigger 2",
                1));
            jobDefinition.StepDefinitions.Add(StepDefinition.CreateTestItem(extensionsAssemblyPath, "Step 1", 0));
            jobDefinition.StepDefinitions.Add(StepDefinition.CreateTestItem(extensionsAssemblyPath, "Step 2", 1));

            return jobDefinition;
        }

        #region Properties

        public Guid JobDefinitionId { get; set; }

        public int Version { get; set; }

        public virtual ICollection<TriggerDefinition> TriggerDefinitions { get; set; }

        public virtual ICollection<StepDefinition> StepDefinitions { get; set; }

        #endregion
    }
}