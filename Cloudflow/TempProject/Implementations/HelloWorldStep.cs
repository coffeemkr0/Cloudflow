using System;
using System.ComponentModel.Composition;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    [Export(typeof(IStep))]
    [ExportMetadata("ExtensionId", ExtensionId)]
    public class HelloWorldStep : IStep
    {
        public const string ExtensionId = "44415043-65F0-4A8D-B438-3EC5ADC2C770";
        private string _message;

        private IStepMonitor _stepMonitor;

        [ImportingConstructor]
        public HelloWorldStep([Import("ExtensionConfiguration")] string message)
        {
            _message = message;
        }

        public void Dispose()
        {
            _message = null;
        }

        public void Execute(IStepMonitor stepMonitor)
        {
            _stepMonitor = stepMonitor;

            _stepMonitor.OnStepStarted(this);

            _stepMonitor.OnStepActivity(this, _message);
            Console.WriteLine(_message);

            _stepMonitor.OnStepCompleted(this);
        }
    }
}