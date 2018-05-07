using System;
using System.ComponentModel.Composition;
using System.Drawing;
using Cloudflow.Core.Steps;
using Cloudflow.Core.Triggers;
using Cloudflow.Extensions.Properties;

namespace Cloudflow.Extensions.Triggers
{
    [Export(typeof(ITriggerDescriptor))]
    [ExportMetadata("ExtensionId", Id)]
    public class TimerTriggerDescriptor : ITriggerDescriptor
    {
        public const string Id = "{DABF8963-4B59-448E-BE5A-143EBDF123EF}";

        public Guid ExtensionId => Guid.Parse(Id);

        public Type ExtensionType => typeof(TimerTrigger);

        public string Name => Resources.TimerTriggerName;

        public string Description => Resources.TimerTriggerDescription;

        public Type ConfigurationType => typeof(TimerTriggerConfiguration);

        public Image Icon => Resources.TimerTriggerIcon;
    }
}