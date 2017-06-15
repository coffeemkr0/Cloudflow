using Cloudflow.Core.Configuration;
using Cloudflow.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Triggers
{
    [ExportExtension("893809A2-C02D-488B-9808-27159BFBB580", typeof(WatchFolderTriggerConfiguration))]
    public class WatchFolderTriggerConfiguration : ExtensionConfiguration
    {
        #region Properties
        public string WatchFolderPath { get; set; }

        public List<string> FileNameMasks { get; set; }
        #endregion

        #region Constructors
        public WatchFolderTriggerConfiguration()
        {
            this.FileNameMasks = new List<string>();
        }
        #endregion
    }
}
