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
        public Guid StepDefinitionId { get; set; }

        public Guid StepConfigurationExtensionId { get; set; }

        public string StepConfigurationExtensionAssemblyPath { get; set; }

        public string Configuration { get; set; }


        public Guid JobDefinitionId { get; set; }

        public virtual JobDefinition JobDefinition { get; set; }
        #endregion

        #region Constructors
        public StepDefinition()
        {
            this.StepDefinitionId = Guid.NewGuid();
        }
        #endregion

        #region Public Methods
        public static StepDefinition CreateTestItem(string extensionsAssemblyPath)
        {
            var stepConfigurationExtensionId = Guid.Parse("191A3C1A-FD25-4790-8141-DFC132DA4970");

            StepDefinition stepDefinition = new StepDefinition()
            {
                StepConfigurationExtensionId = stepConfigurationExtensionId,
                StepConfigurationExtensionAssemblyPath = extensionsAssemblyPath
            };

            var stepConfigurationController = new ExtensionConfigurationController(stepConfigurationExtensionId, extensionsAssemblyPath);
            var logStepConfiguration = stepConfigurationController.CreateNewConfiguration();

            logStepConfiguration.ExtensionId = Guid.Parse("43D6FD16-0344-4204-AEE9-A09B3998C017");
            logStepConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;
            logStepConfiguration.Name = "Hard coded log step";
            logStepConfiguration.GetType().GetProperty("LogMessage").SetValue(logStepConfiguration, "Hello World!");

            stepDefinition.Configuration = logStepConfiguration.ToJson();
            return stepDefinition;
        }
        #endregion
    }
}
