using Cloudflow.Core.Configuration;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Steps
{
    
    [ExportExtension("191A3C1A-FD25-4790-8141-DFC132DA4970", typeof(LogStepConfiguration))]
    public class LogStepConfiguration : ExtensionConfiguration
    {
        [LabelTextResource("LogMessageLabel")]
        public string LogMessage { get; set; }
    }
}
