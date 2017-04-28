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
        #region Events
        public delegate void TriggerFiredEventHandler(object sender, Dictionary<string, object> triggerData);
        public event TriggerFiredEventHandler Fired;
        protected virtual void OnFired(Dictionary<string, object> triggerData)
        {
            TriggerFiredEventHandler temp = Fired;
            if (temp != null)
            {
                temp(this, triggerData);
            }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            Timer timer = new Timer(3000);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine(string.Format("{0} [Trigger] - trigger timer elapsed.",
                DateTime.Now.TimeOfDay.ToString()));
            Dictionary<string, object> triggerData = new Dictionary<string, object>();
            OnFired(triggerData);
        }
        #endregion
    }
}
