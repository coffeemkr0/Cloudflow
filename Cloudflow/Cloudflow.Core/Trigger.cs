using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Cloudflow.Core
{
    public class Trigger
    {
        private static readonly log4net.ILog _logger =
               log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Events
        public delegate void TriggerFiredEventHandler(object sender, Dictionary<string, object> triggerData);
        public event TriggerFiredEventHandler Fired;
        protected virtual void OnFired(Dictionary<string, object> triggerData)
        {
            TriggerFiredEventHandler temp = Fired;
            if (temp != null)
            {
                _logger.Debug("Firing the OnFired event for the trigger");
                temp(this, triggerData);
            }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            _logger.Debug("Initializing the trigger");
            Timer timer = new Timer(3000);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            _logger.Debug("Trigger timer initialized");
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _logger.Debug("Trigger time has elapsed");
            Dictionary<string, object> triggerData = new Dictionary<string, object>();
            OnFired(triggerData);
        }
        #endregion
    }
}
