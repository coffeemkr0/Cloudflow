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
    [ExportMetadata("TriggerName", "WatchFolderTrigger")]
    [ExportMetadata("Type", typeof(WatchFolderTriggerConfiguration))]
    public class WatchFolderTriggerConfiguration : TriggerConfiguration
    {
        #region Properties
        public string WatchFolderPath { get; set; }

        public List<string> FileNameMasks { get; set; }
        #endregion

        #region Constructors
        public WatchFolderTriggerConfiguration() : base("WatchFolderTrigger")
        {
            this.FileNameMasks = new List<string>();
        }
        #endregion
    }
}
