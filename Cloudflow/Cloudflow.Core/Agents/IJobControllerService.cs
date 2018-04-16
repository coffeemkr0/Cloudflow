using System.Collections.Generic;
using Cloudflow.Core.Extensions.Controllers;

namespace Cloudflow.Core.Agents
{
    public interface IJobControllerService
    {
        IList<IJobController> GetJobControllers(IJobMonitor jobMonitor);
    }
}