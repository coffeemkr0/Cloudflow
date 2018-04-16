using System.Collections.Generic;
using Cloudflow.Core.Data.Shared.Models;

namespace Cloudflow.Core.Agents
{
    public interface IJobDefinitionService
    {
        IEnumerable<JobDefinition> GetJobDefinitions();
    }
}