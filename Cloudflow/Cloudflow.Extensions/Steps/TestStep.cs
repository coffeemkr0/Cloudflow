using Cloudflow.Core.Configuration;
using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Steps
{
    [Export(typeof(Step))]
    [ExportMetadata("StepName", "TestStep")]
    public class TestStep : Step
    {
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
                OnStepOutput(OutputEventLevels.Info, "Executing the test step - waiting for 2 seconds.");

                System.Threading.Thread.Sleep(2000);

                OnStepOutput(OutputEventLevels.Info, "Test step execution complete.");
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
