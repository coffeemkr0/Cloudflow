using Cloudflow.Core.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.Models
{
    public class CloudflowWebDb : DbContext
    {
        public DbSet<AgentConfiguration> AgentConfigurations { get; set; }
    }
}