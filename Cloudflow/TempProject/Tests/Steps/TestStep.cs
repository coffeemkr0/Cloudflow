using System.ComponentModel.Composition;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IExtension))]
    [ExportMetadata("ExtensionId", ExtensionId)]
    public class TestStep : IStep, IExtension
    {
        public const string ExtensionId = "{5AA0FAE3-0703-438D-AE86-209ABA558C16}";

        private IStepMonitor _stepMonitor;

        public void Dispose()
        {
        }

        public void Execute(IStepMonitor stepMonitor)
        {
            _stepMonitor = stepMonitor;

            _stepMonitor.OnStepStarted(this);

            _stepMonitor.OnStepActivity(this, "Doing work");

            _stepMonitor.OnStepCompleted(this);
        }
    }
}