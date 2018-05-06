using System.ComponentModel.Composition;
using TempProject.Interfaces;

namespace TempProject.Tests.Triggers
{
    [Export(typeof(ITrigger))]
    [ExportMetadata("Type", typeof(ImmediateTrigger))]
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