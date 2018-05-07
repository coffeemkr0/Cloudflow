using Cloudflow.Core.Extensions;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Core.Triggers;

namespace Cloudflow.Extensions.Triggers
{
    [Export(typeof(ITrigger))]
    [ExportMetadata("Type", typeof(WatchFolderTrigger))]
    public class WatchFolderTrigger : ITrigger
    {
        readonly FileSystemWatcher _fileSystemWatcher;
        ITriggerMonitor _triggerMonitor;

        [ImportingConstructor]
        public WatchFolderTrigger([Import("Configuration")] ITriggerConfiguration configuration)
        {
            _fileSystemWatcher = new FileSystemWatcher();

            var watchFolderTriggerConfiguration = (WatchFolderTriggerConfiguration)configuration;
            _fileSystemWatcher.Path = watchFolderTriggerConfiguration.WatchFolderPath;
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _fileSystemWatcher.Filter = string.Join(";", watchFolderTriggerConfiguration.FileNameMasks.ToArray());

            _fileSystemWatcher.Created += _fileSystemWatcher_Created;
        }

        private void _fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            _triggerMonitor.OnTriggerFired(this);
        }

        public void Start(ITriggerMonitor triggerMonitor)
        {
            _triggerMonitor = triggerMonitor;
            _fileSystemWatcher.EnableRaisingEvents = true;
            _triggerMonitor.OnTriggerStarted(this);
        }

        public void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
            _triggerMonitor.OnTriggerStopped(this);
        }

        public void Dispose()
        {
            _fileSystemWatcher.Dispose();
        }
    }
}
