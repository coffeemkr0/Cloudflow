using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Cloudflow.Core.Triggers;

namespace Cloudflow.Extensions.Triggers
{
    [Export(typeof(ITriggerConfiguration))]
    [ExportMetadata("Type", typeof(WatchFolderTriggerConfiguration))]
    public class WatchFolderTriggerConfiguration : ITriggerConfiguration
    {
        [DisplayOrder(0)]
        [LabelTextResourceAttribute("WatchFolderPathLabel")]
        public string WatchFolderPath { get; set; }

        [DisplayOrder(1)]
        [LabelTextResourceAttribute("FileNameMasksLabel")]
        public List<string> FileNameMasks { get; set; }

        public WatchFolderTriggerConfiguration()
        {
            FileNameMasks = new List<string>();
        }
    }
}
