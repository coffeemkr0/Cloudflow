using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Data.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Data.Agent
{
    public class AgentDbContext : SharedDbContext
    {
        #region Properties
        public DbSet<Run> Runs { get; set; }
        #endregion
    }
}
