using System;
using TempProject.Jobs;

namespace TempProject.Tests.Job
{
    public class JobMonitor : IJobMonitor
    {
        public bool OnJobStartedCalled { get; set; }
        public bool OnJobActivityCalled { get; set; }
        public bool OnJobStoppedCalled { get; set; }
        public bool OnJobExceptionCalled { get; set; }

        public void OnJobStarted(IJob job)
        {
            OnJobStartedCalled = true;
            Console.WriteLine($"[{job.GetClassName()}] Job started");
        }

        public void OnJobActivity(IJob job, string activity)
        {
            OnJobActivityCalled = true;
            Console.WriteLine($"[{job.GetClassName()}] Job activity - {activity}");
        }

        public void OnJobStopped(IJob job)
        {
            OnJobStoppedCalled = true;
            Console.WriteLine($"[{job.GetClassName()}] Job stopped");
        }

        public void OnException(IJob job, Exception e)
        {
            OnJobExceptionCalled = true;
            Console.WriteLine($"[{job.GetClassName()}] Exception - {e}");
        }
    }
}