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
    [ExportMetadata("Name","TestJob")]
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
