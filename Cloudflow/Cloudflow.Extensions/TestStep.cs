using Cloudflow.Core.Configuration;
using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions
{
    [Export(typeof(Step))]
    [ExportMetadata("Name", "TestStep")]
    public class TestStep : Step
    {
        private static Random _rand = new Random();

        #region Constructors
        [ImportingConstructor]
        public TestStep([Import("StepConfiguration")]StepConfiguration stepConfiguration) : base(stepConfiguration)
        {

        }
        #endregion

        #region  Private Methods

        #endregion

        #region Public Methods
        public override void Execute()
        {
            try
            {
                OnStepOutput(OutputEventLevels.Info, "Executing step");

                this.StepLogger.Info("Hello World from inside a test step's code!");
                OnStepOutput(OutputEventLevels.Info, "Hello World from inside a test step's code!");

                System.Threading.Thread.Sleep(3000);

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
