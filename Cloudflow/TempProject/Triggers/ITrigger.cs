using System;

namespace TempProject.Triggers
{
    public interface ITrigger : IDisposable
    {
        void Start(ITriggerMonitor triggerMonitor);

        void Stop();
    }

    public static class TriggerExtensions
    {
        public static string GetClassName(this ITrigger step)
        {
            return step.GetType().Name;
        }
    }
}