using System;

namespace TempProject.Interfaces
{
    public interface IStep : IDisposable
    {
        void Execute(IStepMonitor stepMonitor);
    }

    public static class StepExtensions
    {
        public static string GetClassName(this IStep step)
        {
            return step.GetType().Name;
        }
    }
}