using System.Data.Entity;
using System.Linq;
using Cloudflow.Core.Data.Shared.Models;

namespace Cloudflow.Core.Data.Shared
{
    public class SharedDbContext : DbContext
    {
        public DbSet<JobDefinition> JobDefinitions { get; set; }

        public DbSet<TriggerDefinition> TriggerDefinitions { get; set; }

        public DbSet<StepDefinition> StepDefinitions { get; set; }

        public DbSet<TriggerConditionDefinition> TriggerConditionDefinitions { get; set; }

        public DbSet<StepConditionDefinition> StepConditionDefinitions { get; set; }

        protected virtual void CreateTestData(string extensionsAssemblyPath)
        {
            if (JobDefinitions.FirstOrDefault() == null)
            {
                JobDefinitions.Add(JobDefinition.CreateTestItem(extensionsAssemblyPath));
                SaveChanges();
            }
        }
    }
}