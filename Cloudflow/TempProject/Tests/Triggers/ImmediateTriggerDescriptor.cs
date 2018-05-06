using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempProject.Properties;
using TempProject.Triggers;

namespace TempProject.Tests.Triggers
{
    [Export(typeof(ITriggerDescriptor))]
    [ExportMetadata("ExtensionId", Id)]
    [ExportMetadata("ExtensionType", typeof(ImmediateTriggerDescriptor))]
    public class ImmediateTriggerDescriptor : ITriggerDescriptor
    {
        public const string Id = "{67B2EEA0-B255-4A62-9F45-3D440289ADC6}";

        public Guid ExtensionId => Guid.Parse(Id);

        public Type ExtensionType => typeof(ImmediateTrigger);

        public string Name => Resources.ImmediateTriggerDescriptor_Name;

        public string Description => Resources.ImmediateTriggerDescriptor_Description;

        public Type ConfigurationType => null;

        public byte[] Icon => null;
    }
}
