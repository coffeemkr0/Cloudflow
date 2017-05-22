using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    public abstract class Trigger
    {
        #region Events
        public delegate void TriggerFiredEventHandler(Trigger trigger, Dictionary<string, object> triggerData);
        public event TriggerFiredEventHandler Fired;
        #endregion

        #region Properties
        public TriggerConfiguration TriggerConfiguration { get; }

        public log4net.ILog TriggerLogger { get; }
        #endregion

        #region Constructors
        public Trigger(TriggerConfiguration triggerConfiguration)
        {
            this.TriggerConfiguration = triggerConfiguration;
            this.TriggerLogger = log4net.LogManager.GetLogger($"Trigger.{triggerConfiguration.Name}");
        }
        #endregion

        #region Private Methods
        protected virtual void OnTriggerFired(Dictionary<string, object> triggerData)
        {
            TriggerFiredEventHandler temp = Fired;
            if (temp != null)
            {
                temp(this, triggerData);
            }
        }
        #endregion

        #region Public Methods
        public abstract void Start();

        public abstract void Stop();
        #endregion
    }
}
