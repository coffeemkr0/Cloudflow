using System.Collections.Generic;
using Cloudflow.Core.Data.Agent.Models;

namespace Cloudflow.Core.Agents
{
    public interface IJobController
    {
        List<Run> GetQueuedRuns();
        void Start();
        void Stop();
        void Wait();
    }
}