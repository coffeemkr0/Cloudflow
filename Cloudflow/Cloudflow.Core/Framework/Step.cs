using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    public abstract class Step
    {
        #region Events
        public delegate void StepOutputEventHandler(Step step, OutputEventLevels level, string message);
        public event StepOutputEventHandler StepOutput;
        protected virtual void OnStepOutput(OutputEventLevels level, string message)
        {
            StepOutputEventHandler temp = StepOutput;
            if (temp != null)
            {
                temp(this, level, message);
            }
        }
        #endregion

        #region Properties
        public StepConfiguration StepConfiguration { get; }

        public log4net.ILog StepLogger { get; }
        #endregion

        #region Constructors
        public Step(StepConfiguration stepConfiguration)
        {
            this.StepConfiguration = stepConfiguration;
            this.StepLogger = log4net.LogManager.GetLogger($"Step.{this.StepConfiguration.StepExtensionId}");
        }
        #endregion

        #region Public Methods
        public abstract void Execute();
        #endregion
    }
}
