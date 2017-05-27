using Cloudflow.Core.Extensions;
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
    [ExportExtension("DABF8963-4B59-448E-BE5A-143EBDF123EF", typeof(TimerTrigger))]
    public class TimerTrigger : Trigger
    {
        private Timer _timer;

        [ImportingConstructor]
        public TimerTrigger([Import("ExtensionConfiguration")]ExtensionConfiguration triggerConfiguration) : base(triggerConfiguration)
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
