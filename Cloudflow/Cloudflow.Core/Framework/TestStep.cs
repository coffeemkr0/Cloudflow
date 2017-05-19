using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    public class TestStep : IStep
    {
        private static Random _rand = new Random();

        #region Events
        public event StepOutputEventHandler StepOutput;
        #endregion

        #region Properties
        public Guid Id { get; }

        public string Name { get; }

        public log4net.ILog StepLogger { get; }
        #endregion

        #region Constructors
        public TestStep(string name)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.StepLogger = log4net.LogManager.GetLogger("StepController." + this.Name);
        }
        #endregion

        #region  Private Methods
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
        public void Execute()
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
