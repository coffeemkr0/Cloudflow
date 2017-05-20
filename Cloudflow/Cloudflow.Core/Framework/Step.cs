using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    #region Event Handlers
    public delegate void StepOutputEventHandler(Step step, OutputEventLevels level, string message);
    #endregion

    public abstract class Step
    {
        #region Events
        event StepOutputEventHandler StepOutput;
        #endregion

        #region Properties
        public StepConfiguration StepConfiguration { get; }

        public log4net.ILog StepLogger { get; }
        #endregion

        #region Constructors
        public Step(StepConfiguration stepConfiguration)
        {
            this.StepConfiguration = stepConfiguration;
            this.StepLogger = log4net.LogManager.GetLogger("Step." + this.StepConfiguration.Name);
        }
        #endregion

        #region Private Methods
        protected virtual void OnStepOutput(OutputEventLevels level, string message)
        {
            StepOutputEventHandler temp = StepOutput;
            if (temp != null)
            {
                temp(this, level, message);
            }
        }
        #endregion

        #region Public Methods
        public abstract void Execute();
        #endregion
    }
}
