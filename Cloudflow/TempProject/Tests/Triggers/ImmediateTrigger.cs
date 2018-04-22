using TempProject.Interfaces;

namespace TempProject.Tests.Triggers
{
    public class ImmediateTrigger : ITrigger
    {
        private ITriggerMonitor _triggerMonitor;

        public void Dispose()
        {
        }

        public void Start(ITriggerMonitor triggerMonitor)
        {
            _triggerMonitor = triggerMonitor;
            _triggerMonitor.OnTriggerStarted(this);
            _triggerMonitor.OnTriggerFired(this);
        }

        public void Stop()
        {
            _triggerMonitor.OnTriggerStopped(this);
        }
    }
}