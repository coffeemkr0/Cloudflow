using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Runtime
{
    public class StepController
    {
        #region Events
        public event StepOutputEventHandler StepOutput;
        protected virtual void OnStepOutput(OutputEventLevels level, string message)
        {
            StepOutputEventHandler temp = StepOutput;
            if (temp != null)
            {
                temp(this.Step, level, message);
            }
        }
        #endregion

        #region Private Members
        private static Random _rand = new Random();
        #endregion

        #region Properties
        public Step Step { get; }

        public Dictionary<string, object> TriggerData { get; }

        public log4net.ILog StepLogger { get; }
        #endregion

        #region Constructors
        public StepController(Step step, Dictionary<string, object> triggerData)
        {
            this.Step = step;
            this.TriggerData = triggerData;
            this.StepLogger = log4net.LogManager.GetLogger("StepController." + this.Step.Name);
        }
        #endregion

        #region Public Methods
        public void ExecuteStep()
        {
            try
            {
                OnStepOutput(OutputEventLevels.Info, "Executing step");

                this.StepLogger.Info("Hello World from inside a test step's code!");
                OnStepOutput(OutputEventLevels.Info, "Hello World from inside a test step's code!");

                System.Threading.Thread.Sleep(_rand.Next(1, 5) * 1000);

                OnStepOutput(OutputEventLevels.Info, "Step execution complete");
            }
            catch (Exception ex)
            {
                this.StepLogger.Error(ex);
                OnStepOutput(OutputEventLevels.Error, ex.ToString());
            }
        }
        #endregion
    }
}
