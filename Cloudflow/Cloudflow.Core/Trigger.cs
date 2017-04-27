using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core
{
    public class Trigger
    {
        #region Events
        public delegate void TriggerFiredEventHandler(object sender, Dictionary<string, object> triggerData);
        public event TriggerFiredEventHandler Fired;
        protected virtual void OnFired(Dictionary<string, object> triggerData)
        {
            TriggerFiredEventHandler temp = Fired;
            if (temp != null)
            {
                temp(this, triggerData);
            }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            //Setup a watch folder and fire the trigger whenever files are created in the watch folder
            if (!Directory.Exists("WatchFolder"))
            {
                Directory.CreateDirectory("WatchFolder");
            }
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher("WatchFolder");
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite |
                NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Dictionary<string, object> triggerData = new Dictionary<string, object>();
            triggerData.Add("FileName", e.FullPath);
            OnFired(triggerData);
        }
        #endregion
    }
}
