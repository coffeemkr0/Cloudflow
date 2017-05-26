using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Data.Server.Models
{
    public class JobDefinition
    {
        #region Properties
        public Guid JobDefinitionId { get; set; }

        public Guid JobConfigurationExtensionId { get; set; }

        public string JobConfigurationExtensionAssemblyPath { get; set; }

        public string Configuration { get; set; }

        public virtual ICollection<TriggerDefinition> TriggerDefinitions { get; set; }

        public virtual ICollection<StepDefinition> StepDefinitions { get; set; }
        #endregion

        #region Constructors
        public JobDefinition()
        {
            this.JobDefinitionId = Guid.NewGuid();
            this.TriggerDefinitions = new List<TriggerDefinition>();
            this.StepDefinitions = new List<StepDefinition>();
        }
        #endregion

        public static JobDefinition CreateTestItem(string extensionsAssemblyPath)
        {
            Guid defaultJobExtensionId = Guid.Parse("62A56D5B-07E5-41A3-A637-5E7C53FCF399");

            JobDefinition jobDefinition = new JobDefinition()
            {
                JobConfigurationExtensionId = defaultJobExtensionId,
                JobConfigurationExtensionAssemblyPath = extensionsAssemblyPath
            };

            var jobConfigurationController = new JobConfigurationController(defaultJobExtensionId, extensionsAssemblyPath);
            var jobConfiguration = jobConfigurationController.CreateNewConfiguration();
            jobConfiguration.JobName = "Hard coded test job";
            jobConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;

            jobDefinition.Configuration = jobConfiguration.ToJson();

            jobDefinition.TriggerDefinitions.Add(TriggerDefinition.CreateTestItem(extensionsAssemblyPath));
            jobDefinition.StepDefinitions.Add(StepDefinition.CreateTestItem(extensionsAssemblyPath));

            return jobDefinition;
        }
    }
}
