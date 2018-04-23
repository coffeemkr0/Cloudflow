using System;

namespace TempProject.Interfaces
{
    public interface ITrigger : IDisposable, IExtension
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