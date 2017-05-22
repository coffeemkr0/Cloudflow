using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public class TestJobConfiguration : JobConfiguration
    {
        #region Constructors
        public TestJobConfiguration()
        {
            this.Name = "TestJob";
            this.TriggerConfigurations.Add(new TestTriggerConfiguration());
            this.StepConfigurations.Add(new TestStepConfiguration());
            this.StepConfigurations.Add(new TestStepConfiguration());
        }
        #endregion

        #region Public Methods

        #endregion
    }
}
