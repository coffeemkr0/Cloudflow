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
    [ExportMetadata("StepExtensionId", "43D6FD16-0344-4204-AEE9-A09B3998C017")]
    public class LogStep : Step
    {
        #region Constructors
        [ImportingConstructor]
        public LogStep([Import("ExtensionConfiguration")]ExtensionConfiguration stepConfiguration) : base(stepConfiguration)
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
                this.StepLogger.Info(((LogStepConfiguration)this.StepConfiguration).LogMessage);
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
