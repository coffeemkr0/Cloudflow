using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Data.Shared;
using System.Data.Entity;

namespace Cloudflow.Core.Data.Agent
{
    public class AgentDbContext : SharedDbContext
    {
        #region Properties
        public DbSet<Run> Runs { get; set; }
        #endregion
    }
}
