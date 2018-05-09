using System.Data.Entity;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Data.Shared;

namespace Cloudflow.Core.Data.Agent
{
    public class AgentDbContext : SharedDbContext
    {
        public DbSet<Run> Runs { get; set; }

        public AgentDbContext()
        {
            
        }

        public AgentDbContext(bool createTestData, string extensionsAssemblyPath) : this()
        {
            if (createTestData) CreateTestData(extensionsAssemblyPath);
        }
    }
}