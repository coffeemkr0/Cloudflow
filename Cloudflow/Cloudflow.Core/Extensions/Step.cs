using Cloudflow.Core.Runtime;

namespace Cloudflow.Core.Extensions
{
    public abstract class Step : ConfigurableExtension
    {
        #region Events
        public delegate void StepOutputEventHandler(Step step, OutputEventLevels level, string message);
        public event StepOutputEventHandler StepOutput;
        protected virtual void OnStepOutput(OutputEventLevels level, string message)
        {
            var temp = StepOutput;
            if (temp != null)
            {
                temp(this, level, message);
            }
        }
        #endregion

        #region Properties
        public ExtensionConfiguration StepConfiguration { get; }

        public log4net.ILog StepLogger { get; }
        #endregion

        #region Constructors
        public Step(ExtensionConfiguration stepConfiguration) : base()
        {
            StepConfiguration = stepConfiguration;
            StepLogger = log4net.LogManager.GetLogger($"Step.{StepConfiguration.Name}");
        }
        #endregion

        #region Public Methods
        public abstract void Execute();
        #endregion
    }
}
