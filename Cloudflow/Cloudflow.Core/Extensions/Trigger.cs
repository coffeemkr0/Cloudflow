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
            TriggerConfiguration = triggerConfiguration;
            TriggerLogger = log4net.LogManager.GetLogger($"Trigger.{triggerConfiguration.Name}");
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

        #region Public Methods
        public abstract void Start();

        public abstract void Stop();
        #endregion
    }
}
