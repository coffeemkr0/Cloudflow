using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudflow.Core.Configuration;
using System.ComponentModel.Composition;
using System.IO;

namespace Cloudflow.Extensions.Triggers
{
    [Export(typeof(Trigger))]
    [ExportMetadata("TriggerExtensionId", "893809A2-C02D-488B-9808-27159BFBB580")]
    public class WatchFolderTrigger : Trigger
    {
        FileSystemWatcher _fileSystemWatcher;

        [ImportingConstructor]
        public WatchFolderTrigger([Import("TriggerConfiguration")]TriggerConfiguration triggerConfiguration) : base(triggerConfiguration)
        {
            _fileSystemWatcher = new FileSystemWatcher();

            var configuration = (WatchFolderTriggerConfiguration)triggerConfiguration;
            _fileSystemWatcher.Path = configuration.WatchFolderPath;
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _fileSystemWatcher.Filter = string.Join(";", configuration.FileNameMasks.ToArray());

            _fileSystemWatcher.Created += _fileSystemWatcher_Created;
        }

        private void _fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            var triggerData = new Dictionary<string, object>();
            triggerData.Add("FilePath", e.FullPath);
            OnTriggerFired(triggerData);
        }

        public override void Start()
        {
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public override void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
        }
    }
}
