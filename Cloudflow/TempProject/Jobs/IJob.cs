using System;

namespace TempProject.Jobs
{
    public interface IJob : IDisposable
    {
        void Start(IJobMonitor jobMonitor);

        void Stop();
    }

    public static class JobExtensions
    {
        public static string GetClassName(this IJob job)
        {
            return job.GetType().Name;
        }
    }
}