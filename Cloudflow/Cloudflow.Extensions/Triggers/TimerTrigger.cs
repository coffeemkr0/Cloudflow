using Cloudflow.Core.Extensions;
using System.ComponentModel.Composition;
using System.Timers;
using Cloudflow.Core.Extensions.ExtensionAttributes;

namespace Cloudflow.Extensions.Triggers
{
    [DisplayName("Timer")]
    [Description("A trigger that fires when a certain amount of time has elapsed.")]
    [ExportConfigurableExtension("DABF8963-4B59-448E-BE5A-143EBDF123EF", typeof(TimerTrigger), "E325CD29-053E-4422-97CF-C1C187760E88")]
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
