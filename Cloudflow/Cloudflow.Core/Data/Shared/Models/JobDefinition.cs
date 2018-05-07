using System;
using System.Collections.Generic;
using Cloudflow.Core.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class JobDefinition
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
                Name = "Test job"
            };

            var jsonSerializer = new JsonConfigurationSerializer();

            jobDefinition.TriggerDefinitions.Add(TriggerDefinition.CreateTestItem(extensionsAssemblyPath, "Trigger 1",
                0, jsonSerializer));
            jobDefinition.TriggerDefinitions.Add(TriggerDefinition.CreateTestItem(extensionsAssemblyPath, "Trigger 2",
                1, jsonSerializer));
            jobDefinition.StepDefinitions.Add(StepDefinition.CreateTestItem(extensionsAssemblyPath, "Step 1", 0,
                jsonSerializer));
            jobDefinition.StepDefinitions.Add(StepDefinition.CreateTestItem(extensionsAssemblyPath, "Step 2", 1,
                jsonSerializer));

            return jobDefinition;
        }

        #region Properties

        public string Name { get; set; }

        public Guid JobDefinitionId { get; set; }

        public int Version { get; set; }

        public virtual ICollection<TriggerDefinition> TriggerDefinitions { get; set; }

        public virtual ICollection<StepDefinition> StepDefinitions { get; set; }

        #endregion
    }
}