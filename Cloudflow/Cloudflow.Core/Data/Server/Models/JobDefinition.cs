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
            Guid defaultJobConfigurationExtensionId = Guid.Parse("62A56D5B-07E5-41A3-A637-5E7C53FCF399");

            JobDefinition jobDefinition = new JobDefinition()
            {
                JobConfigurationExtensionId = defaultJobConfigurationExtensionId,
                JobConfigurationExtensionAssemblyPath = extensionsAssemblyPath
            };

            var jobConfigurationController = new ExtensionConfigurationController(defaultJobConfigurationExtensionId, extensionsAssemblyPath);
            var jobConfiguration = jobConfigurationController.CreateNewConfiguration();
            jobConfiguration.ExtensionId = Guid.Parse("3F6F5796-E313-4C53-8064-747C1989DA99");
            jobConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;
            jobConfiguration.Name = "Hard coded test job";
            
            jobDefinition.Configuration = jobConfiguration.ToJson();

            jobDefinition.TriggerDefinitions.Add(TriggerDefinition.CreateTestItem(extensionsAssemblyPath));
            jobDefinition.StepDefinitions.Add(StepDefinition.CreateTestItem(extensionsAssemblyPath));

            return jobDefinition;
        }
    }
}
