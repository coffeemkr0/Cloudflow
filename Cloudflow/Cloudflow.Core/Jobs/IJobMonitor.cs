using System;

namespace Cloudflow.Core.Jobs
{
    public interface IJobMonitor
    {
        void OnJobStarted(IJob job);

        void OnJobActivity(IJob job, string activity);

        void OnJobStopped(IJob job);

        void OnException(IJob job, Exception e);
    }
}