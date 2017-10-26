using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions
{
    public abstract class Trigger : ConfigurableExtension
    {
        #region Events
        public delegate void TriggerFiredEventHandler(Trigger trigger);
        public event TriggerFiredEventHandler Fired;
        #endregion

        #region Properties
        public ExtensionConfiguration TriggerConfiguration { get; }

        public log4net.ILog TriggerLogger { get; }
        #endregion

        #region Constructors
        public Trigger(ExtensionConfiguration triggerConfiguration) : base()
        {
            this.TriggerConfiguration = triggerConfiguration;
            this.TriggerLogger = log4net.LogManager.GetLogger($"Trigger.{triggerConfiguration.Name}");
        }
        #endregion

        #region Private Methods
        protected virtual void OnTriggerFired()
        {
            TriggerFiredEventHandler temp = Fired;
            if (temp != null)
            {
                this.TriggerLogger.Info("Trigger fired");
                temp(this);
            }
        }
        #endregion

        #region Public Methods
        public abstract void Start();

        public abstract void Stop();
        #endregion
    }
}
