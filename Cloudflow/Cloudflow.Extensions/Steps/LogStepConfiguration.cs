using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;

namespace Cloudflow.Extensions.Steps
{
    
    [ExportExtension("191A3C1A-FD25-4790-8141-DFC132DA4970", typeof(LogStepConfiguration))]
    public class LogStepConfiguration : ExtensionConfiguration
    {
        [LabelTextResource("LogMessageLabel")]
        public string LogMessage { get; set; }
    }
}
