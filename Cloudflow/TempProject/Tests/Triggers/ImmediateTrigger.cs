using System.ComponentModel.Composition;
using TempProject.Interfaces;

namespace TempProject.Tests.Triggers
{
    [Export(typeof(IExtension))]
    [ExportMetadata("ExtensionId", ExtensionId)]
    public class ImmediateTrigger : ITrigger, IExtension
    {
        public const string ExtensionId = "{67B2EEA0-B255-4A62-9F45-3D440289ADC6}";

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