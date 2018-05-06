using System;
using TempProject.Triggers;

namespace TempProject.Tests.Triggers
{
    public class TriggerMonitor : ITriggerMonitor
    {
        public bool OnFiredCalled { get; set; }
        public bool OnStartCalled { get; set; }
        public bool OnStopCalled { get; set; }

        public void OnTriggerFired(ITrigger trigger)
        {
            OnFiredCalled = true;
            Console.WriteLine($"[{trigger.GetClassName()}] Trigger fired");
        }

        public void OnTriggerStarted(ITrigger trigger)
        {
            OnStartCalled = true;
            Console.WriteLine($"[{trigger.GetClassName()}] Trigger started");
        }

        public void OnTriggerStopped(ITrigger trigger)
        {
            OnStopCalled = true;
            Console.WriteLine($"[{trigger.GetClassName()}] Trigger stopped");
        }
    }
}