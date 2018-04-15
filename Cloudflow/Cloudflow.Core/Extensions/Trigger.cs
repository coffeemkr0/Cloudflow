using log4net;

namespace Cloudflow.Core.Extensions
{
    public abstract class Trigger : ConfigurableExtension
    {
        #region Constructors

        public Trigger(ExtensionConfiguration triggerConfiguration)
        {
            TriggerConfiguration = triggerConfiguration;
            TriggerLogger = LogManager.GetLogger($"Trigger.{triggerConfiguration.Name}");
        }

        #endregion

        #region Private Methods

        protected virtual void OnTriggerFired()
        {
            var temp = Fired;
            if (temp != null)
            {
                TriggerLogger.Info("Trigger fired");
                temp(this);
            }
        }

        #endregion

        #region Events

        public delegate void TriggerFiredEventHandler(Trigger trigger);

        public event TriggerFiredEventHandler Fired;

        #endregion

        #region Properties

        public ExtensionConfiguration TriggerConfiguration { get; }

        public ILog TriggerLogger { get; }

        #endregion

        #region Public Methods

        public abstract void Start();

        public abstract void Stop();

        #endregion
    }
}