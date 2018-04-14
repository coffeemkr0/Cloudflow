using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System.Collections.Generic;

namespace Cloudflow.Extensions.Triggers
{
    [ExportExtension("893809A2-C02D-488B-9808-27159BFBB580", typeof(WatchFolderTriggerConfiguration))]
    public class WatchFolderTriggerConfiguration : ExtensionConfiguration
    {
        #region Properties
        [DisplayOrder(0)]
        [LabelTextResourceAttribute("WatchFolderPathLabel")]
        public string WatchFolderPath { get; set; }

        [DisplayOrder(1)]
        [LabelTextResourceAttribute("FileNameMasksLabel")]
        public List<string> FileNameMasks { get; set; }
        #endregion

        #region Constructors
        public WatchFolderTriggerConfiguration()
        {
            FileNameMasks = new List<string>();
        }
        #endregion
    }
}
