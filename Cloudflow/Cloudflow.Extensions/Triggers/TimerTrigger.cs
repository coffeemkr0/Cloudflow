using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudflow.Core.Configuration;
using System.ComponentModel.Composition;
using System.Timers;

namespace Cloudflow.Extensions.Triggers
{
    [Export(typeof(Trigger))]
    [ExportMetadata("TriggerExtensionId", "E325CD29-053E-4422-97CF-C1C187760E88")]
    public class TimerTrigger : Trigger
    {
        private Timer _timer;

        [ImportingConstructor]
        public TimerTrigger([Import("TriggerConfiguration")]TriggerConfiguration triggerConfiguration) : base(triggerConfiguration)
        {
            _timer = new Timer(((TimerTriggerConfiguration)triggerConfiguration).Interval);
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            OnTriggerFired(null);
        }

        public override void Start()
        {
            _timer.Enabled = true;
        }

        public override void Stop()
        {
            _timer.Enabled = false;
        }
    }
}
