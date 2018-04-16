using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudflow.Core.Data.Shared.Models;
using log4net;

namespace Cloudflow.Core.Extensions.Controllers
{
    interface IJobControllerData
    {
        Job Job { get; }
        ExtensionConfiguration JobConfiguration { get; }
        ILog JobControllerLoger { get; }
        JobDefinition JobDefinition { get; }
        List<StepController> StepControllers { get; }
        List<TriggerController> TriggerControllers { get; }
    }
}
