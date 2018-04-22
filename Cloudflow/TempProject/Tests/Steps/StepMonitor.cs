using System;
using TempProject.Interfaces;

namespace TempProject.Tests.Steps
{
    public class StepMonitor : IStepMonitor
    {
        public bool OnCompletedCalled { get; set; }
        public bool OnStartedCalled { get; set; }
        public bool OnActivityCalled { get; set; }

        public void OnStepCompleted(IStep step)
        {
            OnCompletedCalled = true;
            Console.WriteLine($"[{step.GetClassName()}] Step completed");
        }

        public void OnStepStarted(IStep step)
        {
            OnStartedCalled = true;
            Console.WriteLine($"[{step.GetClassName()}] Step started");
        }

        public void OnStepActivity(IStep step, string activity)
        {
            OnActivityCalled = true;
            Console.WriteLine($"[{step.GetClassName()}] Step activity - {activity}");
        }
    }
}