using System.Timers;
using TempProject.Interfaces;
using TempProject.Tests.Triggers;

namespace TempProject.Implementations
{
    public class TimerTrigger : ITrigger
    {
        private readonly ITimer _timer;
        private ITriggerMonitor _triggerMonitor;

        public TimerTrigger(ITimer timer)
        {
            _timer = timer;
            timer.Elapsed += _timer_Elapsed;
        }

        public void Dispose()
        {
            Stop();
            _timer.Dispose();
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

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _triggerMonitor?.OnTriggerFired(this);
        }
    }
}