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
    [ExportMetadata("StepExtensionId", "191A3C1A-FD25-4790-8141-DFC132DA4970")]
    [ExportMetadata("Type", typeof(LogStepConfiguration))]
    public class LogStepConfiguration : StepConfiguration
    {
        public string LogMessage { get; set; }

        public LogStepConfiguration() : base(Guid.Parse("191A3C1A-FD25-4790-8141-DFC132DA4970"))
        {

        }
    }
}
