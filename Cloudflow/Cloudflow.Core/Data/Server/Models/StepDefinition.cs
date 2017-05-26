using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Data.Server.Models
{
    public class StepDefinition
    {
        #region Properties
        public Guid Id { get; set; }

        public Guid StepConfigurationExtensionId { get; set; }

        public string StepConfigurationExtensionAssemblyPath { get; set; }

        public string Configuration { get; set; }


        public Guid JobDefinitionId { get; set; }

        public JobDefinition JobDefinition { get; set; }
        #endregion

        #region Constructors
        public StepDefinition()
        {
            this.Id = Guid.NewGuid();
        }
        #endregion

        #region Public Methods
        public static StepDefinition CreateTestItem(string extensionsAssemblyPath)
        {
            var stepExtensionId = Guid.Parse("191A3C1A-FD25-4790-8141-DFC132DA4970");

            StepDefinition stepDefinition = new StepDefinition()
            {
                StepConfigurationExtensionId = stepExtensionId,
                StepConfigurationExtensionAssemblyPath = extensionsAssemblyPath
            };

            var stepConfigurationController = new StepConfigurationController(stepExtensionId, extensionsAssemblyPath);
            var logStepConfiguration = stepConfigurationController.CreateNewConfiguration();
            logStepConfiguration.StepName = "Hard coded log step";
            logStepConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;
            logStepConfiguration.GetType().GetProperty("LogMessage").SetValue(logStepConfiguration, "Hello World!");

            stepDefinition.Configuration = logStepConfiguration.ToJson();
            return stepDefinition;
        }
        #endregion
    }
}
