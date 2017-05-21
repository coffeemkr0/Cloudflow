using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public class DefaultJobConfiguration : JobConfiguration
    {
        #region Public Methods
        public static DefaultJobConfiguration CreateTestJobConfiguration()
        {
            DefaultJobConfiguration configuration = new DefaultJobConfiguration
            {
                Name = "TestJob"
            };

            configuration.TriggerConfigurations.Add(DefaultTriggerConfiguration.CreateTestTriggerConfigurtion("Test Trigger"));
            configuration.StepConfigurations.Add(DefaultStepConfiguration.CreateTestStepConfiguration("Test Step 1"));
            configuration.StepConfigurations.Add(DefaultStepConfiguration.CreateTestStepConfiguration("Test Step 2"));

            return configuration;
        }
        #endregion
    }
}
