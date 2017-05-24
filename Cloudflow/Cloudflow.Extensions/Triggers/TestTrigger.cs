using Cloudflow.Core.Configuration;
using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Cloudflow.Extensions.Triggers
{
    [Export(typeof(Trigger))]
    [ExportMetadata("Name", "TestTrigger")]
    public class TestTrigger : Trigger
    {
        Timer _timer;
        static Random _rand = new Random();

        #region Properties

        #endregion

        #region Constructors
        [ImportingConstructor]
        public TestTrigger([Import("TriggerConfiguration")]TriggerConfiguration triggerConfiguration) : base(triggerConfiguration)
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
                _timer = new Timer(5000);
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
