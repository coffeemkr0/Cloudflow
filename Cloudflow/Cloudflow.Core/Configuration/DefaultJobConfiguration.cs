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
        public static DefaultJobConfiguration CreateTestJobConfiguration(string name)
        {
            DefaultJobConfiguration configuration = new DefaultJobConfiguration
            {
                Name = name
            };

            configuration.TriggerConfigurations.Add(DefaultTriggerConfiguration.CreateTestTriggerConfigurtion("Test Trigger"));
            configuration.StepConfigurations.Add(DefaultStepConfiguration.CreateTestStepConfiguration("Test Step"));

            return configuration;
        }
        #endregion
    }
}
