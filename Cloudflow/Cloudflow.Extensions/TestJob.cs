using Cloudflow.Core.Configuration;
using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions
{
    [Export(typeof(Job))]
    public class TestJob : Job
    {
        #region Properties

        #endregion

        #region Constructors
        public TestJob(JobConfiguration jobConfiguration) : base(jobConfiguration)
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
