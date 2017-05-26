using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Triggers
{
    [Export(typeof(TriggerConfiguration))]
    [ExportMetadata("TriggerExtensionId", "893809A2-C02D-488B-9808-27159BFBB580")]
    [ExportMetadata("Type", typeof(WatchFolderTriggerConfiguration))]
    public class WatchFolderTriggerConfiguration : TriggerConfiguration
    {
        #region Properties
        public string WatchFolderPath { get; set; }

        public List<string> FileNameMasks { get; set; }
        #endregion

        #region Constructors
        public WatchFolderTriggerConfiguration() : base(Guid.Parse("893809A2-C02D-488B-9808-27159BFBB580"))
        {
            this.FileNameMasks = new List<string>();
        }
        #endregion
    }
}
