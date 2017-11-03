using Cloudflow.Core.Data.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Data.Shared
{
    public class SharedDbContext : DbContext
    {
        public DbSet<JobDefinition> JobDefinitions { get; set; }

        public DbSet<TriggerDefinition> TriggerDefinitions { get; set; }

        public DbSet<StepDefinition> StepDefinitions { get; set; }

        public DbSet<TriggerConditionDefinition> TriggerConditionDefinitions { get; set; }

        protected virtual void CreateTestData(string extensionsAssemblyPath)
        {
            if (this.JobDefinitions.FirstOrDefault() == null)
            {
                this.JobDefinitions.Add(JobDefinition.CreateTestItem(extensionsAssemblyPath));
                this.SaveChanges();
            }
        }
    }
}
