using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
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
