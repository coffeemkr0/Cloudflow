using System.ComponentModel.Composition;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Core.Steps;

namespace Cloudflow.Extensions.Steps
{
    [Export(typeof(IStepConfiguration))]
    [ExportMetadata("Type", typeof(LogStepConfiguration))]
    public class LogStepConfiguration : IStepConfiguration
    {
        [LabelTextResource("LogMessageLabel")]
        public string LogMessage { get; set; }
    }
}
