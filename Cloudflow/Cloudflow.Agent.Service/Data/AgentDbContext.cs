using Cloudflow.Agent.Service.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Agent.Service.Data
{
    public class AgentDbContext : DbContext
    {
        #region Properties
        public DbSet<Run> Runs { get; set; }
        #endregion
    }
}
