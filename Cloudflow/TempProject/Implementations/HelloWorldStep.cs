using System;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class HelloWorldStep : IStep
    {
        private readonly IStepMonitor _stepMonitor;
        private string _message;

        public HelloWorldStep(IStepMonitor stepMonitor, string message)
        {
            _stepMonitor = stepMonitor;
            _message = message;
        }

        public void Dispose()
        {
            _stepMonitor.OnStepDisposing(this);
            _message = null;
        }

        public void Execute()
        {
            _stepMonitor.OnStepStarted(this);

            _stepMonitor.OnStepActivity(this, _message);
            Console.WriteLine(_message);

            _stepMonitor.OnStepCompleted(this);
        }
    }
}