using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Triggers
{
    [Export(typeof(TriggerConfiguration))]
    [ExportMetadata("TriggerName", "TestTrigger")]
    [ExportMetadata("Type", typeof(TestTriggerConfiguration))]
    public class TestTriggerConfiguration : TriggerConfiguration
    {
        #region Constructors
        public TestTriggerConfiguration() : base("TestTrigger")
        {

        }
        #endregion

        #region Public Methods

        #endregion
    }
}
