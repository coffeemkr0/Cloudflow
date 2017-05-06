using Cloudflow.Core.Data.Agent.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Data.Agent
{
    public class AgentDbContext : DbContext
    {
        #region Properties
        public DbSet<Run> Runs { get; set; }
        #endregion
    }
}
