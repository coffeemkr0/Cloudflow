using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Cloudflow.Core.Data.Models;

namespace Cloudflow.Core.Data
{
    public class CoreDbContext : DbContext
    {
        #region Models
        public DbSet<AgentConfiguration> AgentConfigurations { get; set; }
        #endregion

        #region Constructors
        public CoreDbContext()
        {

        }

        public CoreDbContext(bool createTestData) : this()
        {
            if (createTestData)
            {
                CreateTestData();
            }
        }
        #endregion

        #region Private Methods
        private void CreateTestData()
        {
            if (this.AgentConfigurations.ToList().FirstOrDefault() == null)
            {
                this.AgentConfigurations.Add(AgentConfiguration.CreateTestItem());
                this.SaveChanges();
            }
        }
        #endregion
    }
}
