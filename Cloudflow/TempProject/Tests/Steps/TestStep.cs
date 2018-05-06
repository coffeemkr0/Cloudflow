using System.ComponentModel.Composition;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    [Export(typeof(IStep))]
    [ExportMetadata("Type", typeof(TestStep))]
    public class TestStep : IStep
    {
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