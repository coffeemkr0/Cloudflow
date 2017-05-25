using Cloudflow.Core.Configuration;
using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Jobs
{
    [Export(typeof(JobConfiguration))]
    [ExportMetadata("JobExtensionId", "F4A842C9-AB25-4B3F-90A9-DDC7A0C72430")]
    [ExportMetadata("Type", typeof(TestJobConfiguration))]
    public class TestJobConfiguration : JobConfiguration
    {
        #region Constructors
        public TestJobConfiguration() : base(Guid.Parse("F4A842C9-AB25-4B3F-90A9-DDC7A0C72430"))
        {
            var triggerConfigurationController = new TriggerConfigurationController("TestTrigger", @"..\..\..\Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll");
            this.TriggerConfigurations.Add(triggerConfigurationController.CreateNewConfiguration());

            var stepConfigurationController = new StepConfigurationController("TestStep", @"..\..\..\Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll");
            this.StepConfigurations.Add(stepConfigurationController.CreateNewConfiguration());
        }
        #endregion

        #region Public Methods

        #endregion
    }
}
