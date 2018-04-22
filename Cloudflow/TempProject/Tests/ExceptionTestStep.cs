using System;
using TempProject.Interfaces;

namespace TempProject.Tests
{
    public class ExceptionTestStep : IStep
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Execute(IStepMonitor stepMonitor)
        {
            throw new Exception("Test exception from inside the Execute method of the step.");
        }
    }
}