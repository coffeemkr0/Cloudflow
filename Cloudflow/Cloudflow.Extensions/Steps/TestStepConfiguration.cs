using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Steps
{
    [Export(typeof(StepConfiguration))]
    [ExportMetadata("StepName", "TestStep")]
    [ExportMetadata("Type", typeof(TestStepConfiguration))]
    public class TestStepConfiguration : StepConfiguration
    {
        #region Constructors
        public TestStepConfiguration() : base("TestStep")
        {

        }
        #endregion
    }
}
