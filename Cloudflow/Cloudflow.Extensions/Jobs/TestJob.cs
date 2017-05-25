using Cloudflow.Core.Configuration;
using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Jobs
{
    [Export(typeof(Job))]
    [ExportMetadata("JobExtensionId", "F4A842C9-AB25-4B3F-90A9-DDC7A0C72430")]
    public class TestJob : Job
    {
        #region Properties

        #endregion

        #region Constructors
        [ImportingConstructor]
        public TestJob([Import("JobConfiguration")]JobConfiguration jobConfiguration) : base(jobConfiguration)
        {

        }
        #endregion

        #region Private Methods
        private void Trigger_Fired(Trigger trigger, Dictionary<string, object> triggerData)
        {
            OnTriggerFired(trigger, triggerData);
        }
        #endregion

        #region Public Methods

        #endregion
    }
}
