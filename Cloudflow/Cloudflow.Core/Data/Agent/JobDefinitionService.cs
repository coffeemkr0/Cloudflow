using System.Collections.Generic;
using Cloudflow.Core.Agents;
using Cloudflow.Core.Data.Shared.Models;

namespace Cloudflow.Core.Data.Agent
{
    public class JobDefinitionService : IJobDefinitionService
    {
        private readonly AgentDbContext _agentDbContext;

        public JobDefinitionService(AgentDbContext agentDbContext)
        {
            _agentDbContext = agentDbContext;
        }

        public IEnumerable<JobDefinition> GetJobDefinitions()
        {
            return _agentDbContext.JobDefinitions;
        }
    }
}