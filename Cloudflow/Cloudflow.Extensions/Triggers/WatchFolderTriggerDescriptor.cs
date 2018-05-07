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
    public class WatchFolderTriggerDescriptor : ITriggerDescriptor
    {
        public const string Id = "{DAD72E34-F229-43B6-8B4E-AEDA64BCCF4E}";

        public Guid ExtensionId => Guid.Parse(Id);

        public Type ExtensionType => typeof(WatchFolderTrigger);

        public string Name => Resources.WatchFolderTriggerName;

        public string Description => Resources.WatchFolderTriggerDescription;

        public Type ConfigurationType => typeof(WatchFolderTriggerConfiguration);

        public Image Icon => Resources.Folder;
    }
}