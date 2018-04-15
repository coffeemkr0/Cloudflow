using Cloudflow.Core.Agents;
using log4net;

namespace Cloudflow.Core.Extensions
{
    public abstract class Step : ConfigurableExtension
    {
        #region Constructors

        public Step(ExtensionConfiguration stepConfiguration)
        {
            StepConfiguration = stepConfiguration;
            StepLogger = LogManager.GetLogger($"Step.{StepConfiguration.Name}");
        }

        #endregion

        #region Public Methods

        public abstract void Execute();

        #endregion

        #region Events

        public delegate void StepOutputEventHandler(Step step, OutputEventLevels level, string message);

        public event StepOutputEventHandler StepOutput;

        protected virtual void OnStepOutput(OutputEventLevels level, string message)
        {
            var temp = StepOutput;
            if (temp != null) temp(this, level, message);
        }

        #endregion

        #region Properties

        public ExtensionConfiguration StepConfiguration { get; }

        public ILog StepLogger { get; }

        #endregion
    }
}