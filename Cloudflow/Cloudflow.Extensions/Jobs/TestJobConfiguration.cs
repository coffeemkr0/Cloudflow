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
    [ExportMetadata("JobName", "TestJob")]
    [ExportMetadata("Type", typeof(TestJobConfiguration))]
    public class TestJobConfiguration : JobConfiguration
    {
        #region Constructors
        public TestJobConfiguration() : base("TestJob")
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
