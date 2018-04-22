using System;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class HelloWorldStep : IStep
    {
        private IStepMonitor _stepMonitor;
        private string _message;

        public HelloWorldStep(string message)
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