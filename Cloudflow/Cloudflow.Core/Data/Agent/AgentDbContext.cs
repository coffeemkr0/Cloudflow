using System.Data.Entity;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Data.Shared;

namespace Cloudflow.Core.Data.Agent
{
    public class AgentDbContext : SharedDbContext
    {
        #region Properties

        public DbSet<Run> Runs { get; set; }

        #endregion
    }
}