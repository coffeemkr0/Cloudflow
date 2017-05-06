using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Cloudflow.Core.Data.Server.Models;

namespace Cloudflow.Core.Data.Server
{
    public class ServerDbContext : DbContext
    {
        #region Models
        public DbSet<AgentConfiguration> AgentConfigurations { get; set; }
        #endregion

        #region Constructors
        public ServerDbContext()
        {

        }

        public ServerDbContext(bool createTestData) : this()
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
