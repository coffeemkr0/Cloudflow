using System.ComponentModel.Composition;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Core.Triggers;
using Cloudflow.Extensions.Steps;

namespace Cloudflow.Extensions.Triggers
{
    [Export(typeof(ITriggerConfiguration))]
    [ExportMetadata("Type", typeof(TimerTriggerConfiguration))]
    public class TimerTriggerConfiguration : ITriggerConfiguration
    {
        public double Interval { get; set; }
    }
}
