using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Cloudflow.Core.Framework
{
    public class TestTrigger : Trigger
    {
        Timer _timer;
        static Random _rand = new Random();

        #region Properties

        #endregion

        #region Constructors
        public TestTrigger(TriggerConfiguration triggerConfiguration) : base(triggerConfiguration)
        {

        }
        #endregion

        #region Private Methods
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dictionary<string, object> triggerData = new Dictionary<string, object>();
            OnTriggerFired(triggerData);
        }
        #endregion

        #region Public Methods
        public override void Start()
        {
            this.TriggerLogger.Info("Starting the timer");

            if(_timer == null)
            {
                _timer = new Timer(3000);
                _timer.Elapsed += _timer_Elapsed;
            }
            
            _timer.Enabled = true;
        }

        public override void Stop()
        {
            this.TriggerLogger.Info("Stopping the timer");
            _timer.Enabled = false;
        }
        #endregion
    }
}
