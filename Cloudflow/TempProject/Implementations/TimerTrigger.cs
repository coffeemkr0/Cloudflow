using System;
using System.Timers;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public interface ITimer : IDisposable
    {
        double Interval { get; set; }
        void Start();

        void Stop();

        event ElapsedEventHandler Elapsed;
    }

    public class TimerTriggerTimer : Timer, ITimer
    {
    }

    public class TimerTrigger : ITrigger
    {
        private readonly ITimer _timer;
        private readonly ITriggerMonitor _triggerMonitor;

        public TimerTrigger(ITimer timer, ITriggerMonitor triggerMonitor)
        {
            _timer = timer;
            timer.Elapsed += _timer_Elapsed;
            _triggerMonitor = triggerMonitor;
        }

        public void Dispose()
        {
            Stop();
            _timer.Dispose();

            _triggerMonitor.OnTriggerDisposed(this);
        }

        public void Start()
        {
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