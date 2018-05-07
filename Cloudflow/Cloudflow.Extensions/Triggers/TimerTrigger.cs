using Cloudflow.Core.Extensions;
using System.ComponentModel.Composition;
using System.Timers;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Core.Triggers;

namespace Cloudflow.Extensions.Triggers
{
    [Export(typeof(ITrigger))]
    [ExportMetadata("Type", typeof(TimerTrigger))]
    public class TimerTrigger : ITrigger
    {
        private readonly Timer _timer;
        private ITriggerMonitor _triggerMonitor;

        [ImportingConstructor]
        public TimerTrigger([Import("Configuration")] ITriggerConfiguration configuration)
        {
            _timer = new Timer(((TimerTriggerConfiguration)configuration).Interval);
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _triggerMonitor.OnTriggerFired(this);
        }

        public void Start(ITriggerMonitor triggerMonitor)
        {
            _triggerMonitor = triggerMonitor;
            _timer.Start();
            _triggerMonitor.OnTriggerStarted(this);
        }

        public void Stop()
        {
            _timer.Stop();
            _triggerMonitor.OnTriggerStopped(this);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
