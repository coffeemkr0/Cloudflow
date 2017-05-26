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
        public Guid Id { get; set; }

        public Guid JobConfigurationExtensionId { get; set; }

        public string JobConfigurationExtensionAssemblyPath { get; set; }

        public string Configuration { get; set; }
        #endregion

        public static JobDefinition CreateTestItem(string extensionsAssemblyPath)
        {
            JobDefinition jobDefinition = new JobDefinition()
            {
                Id = Guid.NewGuid(),
                JobConfigurationExtensionId = Guid.Parse("62A56D5B-07E5-41A3-A637-5E7C53FCF399"),
                JobConfigurationExtensionAssemblyPath = extensionsAssemblyPath
            };

            Guid defaultJobExtensionId = Guid.Parse("62A56D5B-07E5-41A3-A637-5E7C53FCF399");
            var jobConfigurationController = new JobConfigurationController(defaultJobExtensionId, extensionsAssemblyPath);
            var jobConfiguration = jobConfigurationController.CreateNewConfiguration();
            jobConfiguration.JobName = "Hard coded test job";
            jobConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;

            var triggerExtensionId = Guid.Parse("E325CD29-053E-4422-97CF-C1C187760E88");
            var triggerConfigurationController = new TriggerConfigurationController(triggerExtensionId, extensionsAssemblyPath);
            var timerConfiguration = triggerConfigurationController.CreateNewConfiguration();
            timerConfiguration.TriggerName = "Hard coded timer trigger";
            timerConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;
            timerConfiguration.GetType().GetProperty("Interval").SetValue(timerConfiguration, 5000);
            jobConfiguration.TriggerConfigurations.Add(timerConfiguration);

            var stepExtensionId = Guid.Parse("191A3C1A-FD25-4790-8141-DFC132DA4970");
            var stepConfigurationController = new StepConfigurationController(stepExtensionId, extensionsAssemblyPath);
            var logStepConfiguration = stepConfigurationController.CreateNewConfiguration();
            logStepConfiguration.StepName = "Hard coded log step";
            logStepConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;
            logStepConfiguration.GetType().GetProperty("LogMessage").SetValue(logStepConfiguration, "Hello World!");
            jobConfiguration.StepConfigurations.Add(logStepConfiguration);

            jobDefinition.Configuration = jobConfiguration.ToJson();

            return jobDefinition;
        }
    }
}
