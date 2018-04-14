using System.Collections.Generic;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Data.Shared.Models;
using log4net;

namespace Cloudflow.Core.Extensions.Controllers
{
    public interface IJobController
    {
        Job Job { get; }
        ExtensionConfiguration JobConfiguration { get; }
        ILog JobControllerLoger { get; }
        JobDefinition JobDefinition { get; }
        List<StepController> StepControllers { get; }
        List<TriggerController> TriggerControllers { get; }

        event JobController.RunStatusChangedEventHandler RunStatusChanged;
        event JobController.StepOutputEventHandler StepOutput;

        List<Run> GetQueuedRuns();
        void Start();
        void Stop();
        void Wait();
    }
}